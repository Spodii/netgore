using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Collections;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles the game's world, keeping track of all maps and characters
    /// </summary>
    public class World : WorldBase, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Stack<IDisposable> _disposeStack = new Stack<IDisposable>(4);

        readonly DArray<Map> _maps;
        readonly List<IRespawnable> _respawnables = new List<IRespawnable>();
        readonly Server _server;
        readonly TSDictionary<string, User> _users = new TSDictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        bool _disposed;

        /// <summary>
        /// Gets the DBController used by this World.
        /// </summary>
        public DBController DBController
        {
            get { return Server.DBController; }
        }

        /// <summary>
        /// Gets a stack of objects that need to be disposed. The stack is processed once every frame.
        /// Use for any Dispose call that would otherwise cause a potential exception (such as
        /// trying to Dispose a character during their Update) or threading complications.
        /// </summary>
        public Stack<IDisposable> DisposeStack
        {
            get { return _disposeStack; }
        }

        /// <summary>
        /// Gets an IEnuermable of all the maps in the world.
        /// </summary>
        public IEnumerable<Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Gets the server's NPCTemplateManager
        /// </summary>
        public NPCTemplateManager NPCTemplates
        {
            get { return _server.NPCTemplateManager; }
        }

        /// <summary>
        /// Gets the server the world belongs to
        /// </summary>
        public Server Server
        {
            get { return _server; }
        }

        /// <summary>
        /// World constructor
        /// </summary>
        /// <param name="parent">Server this world is part of</param>
        public World(Server parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            // Store the parent
            _server = parent;

            // Load the maps
            var mapFiles = Map.GetMapFiles(ContentPaths.Build);
            _maps = new DArray<Map>(mapFiles.Count() + 10, false);
            foreach (string mapFile in mapFiles)
            {
                MapIndex mapIndex = Map.GetIndexFromPath(mapFile);
                Map m = new Map(mapIndex, this);
                m.Load(ContentPaths.Build, true);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded map index {0} from file {1}", mapIndex, mapFile);
                _maps[(int)mapIndex] = m;
            }

            // Trim down the maps array under the assumption we won't be adding more maps
            _maps.Trim();

            // NOTE: Just temporary until we get real NPC creation going on
            // Create some test NPCs and items
#if true
            Random rand = new Random();
            foreach (Map m in Maps)
            {
                for (int i = 0; i < 3; i++)
                {
                    NPC npc = new NPC(this, NPCTemplates.GetTemplate(1)) { RespawnMap = m };
                    npc.ChangeMap(m);
                }
                for (int i = 0; i < 5; i++)
                {
                    float x = rand.Next(128, (int)m.Width - 256);
                    float y = rand.Next(128, (int)m.Height - 256);
                    int id = rand.Next(1, Server.ItemTemplates.Count);
                    ItemEntity item = new ItemEntity(Server.ItemTemplates[id], new Vector2(x, y), 1);
                    m.AddEntity(item);
                }
            }
#endif

            // HACK: This is just for testing the persistent NPCs
            var a = new NPC(this, 2);
            var b = new NPC(this, 3);

            a.AI = AIFactory.Create("testAI", a);
            b.AI = AIFactory.Create("testAI", b);
        }

        /// <summary>
        /// Adds an IRespawnable to the list of objects that need to respawn.
        /// </summary>
        /// <param name="respawnable">The object to respawn.</param>
        public void AddToRespawn(IRespawnable respawnable)
        {
            _respawnables.Add(respawnable);
        }

        /// <summary>
        /// Adds a user to the world
        /// </summary>
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentException("User contains a null or invalid name.", "user");

            // NOTE: If the user is already logged in, this will throw an exception. Will have to determine how to handle this scenario.
            _users.Add(user.Name, user);
        }

        /// <summary>
        /// Disposes of the World and everything in it.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Debug.Fail("World is already disposed.");
                return;
            }

            // Set the World as disposed
            _disposed = true;

            // Dispose of the maps
            foreach (Map map in Maps)
                map.Dispose();

            // Process the dispose stack
            while (_disposeStack.Count > 0)
            {
                ProcessDisposeStack();
            }
        }

        /// <summary>
        /// Searches for a User in this World.
        /// </summary>
        /// <param name="name">Name of the User to find.</param>
        /// <returns>User with the given name, or null if the user is not online or invalid.</returns>
        public User FindUser(string name)
        {
            User user;
            if (!_users.TryGetValue(name, out user))
                return null;

            return user;
        }

        /// <summary>
        /// Returns a map by its index
        /// </summary>
        /// <param name="mapIndex">Index of the map</param>
        /// <returns>Map for the given index, or null if invalid</returns>
        public Map GetMap(MapIndex mapIndex)
        {
            // If the map can be grabbed from the index, grab it
            if (_maps.CanGet((int)mapIndex))
            {
                // Check that the map is valid
                Debug.Assert(_maps[(int)mapIndex] != null, "Tried to get a null map.");

                // Return the map at the index
                return _maps[(int)mapIndex];
            }

            // Could not grab by index
            if (log.IsWarnEnabled)
                log.WarnFormat("GetMap() on index {0} returned null because map does not exist.");

            return null;
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public override int GetTime()
        {
            return _server.GetTime();
        }

        /// <summary>
        /// Finds a user based on their connection information.
        /// </summary>
        /// <param name="conn">Client connection information.</param>
        /// <param name="failRecover">If true, will try to find the User even if the TCPSocket isn't properly
        /// bound to the User.</param>
        /// <returns>User bound to the connection if any, else null.</returns>
        public User GetUser(IIPSocket conn, bool failRecover)
        {
            // Check for a user bound to the connection
            if (conn.Tag != null)
                return (User)conn.Tag;

            if (!failRecover)
                return null;

            Debug.Fail("User not bound to connection tag.");
            if (log.IsErrorEnabled)
                log.Warn("User not bound to connection tag.");

            // No user bound to connection, perform manual search
            User ret = _users.Values.FirstOrDefault(x => x.Conn == conn);
            if (ret == null)
            {
                const string errmsg = "No user found on socket `{0}`.";
                Debug.Fail(string.Format(errmsg, conn));
                if (log.IsWarnEnabled)
                    log.ErrorFormat(errmsg, conn);
            }

            // Return value, which will be null if none found
            return ret;
        }

        /// <summary>
        /// Finds a user based on their connection information
        /// </summary>
        /// <param name="conn">Client connection information</param>
        /// <returns>User bound to the connection if any, else null</returns>
        public User GetUser(IIPSocket conn)
        {
            return GetUser(conn, true);
        }

        /// <summary>
        /// Processes the stack of objects that need to be disposed. Objects are pushed into
        /// this stack instead of being disposed on the spot due that disposing objects will
        /// often times result in them being removed from certain collections. But if they
        /// are removed from a collection while it is enumerating, it will cause an error.
        /// Example:
        /// foreach (User usr in Users)
        ///     usr.Dispose(); // assume User.Dispose() removes the User from Users
        /// </summary>
        void ProcessDisposeStack()
        {
            // Keep popping until the stack is empty
            while (_disposeStack.Count > 0)
            {
                // Pop out the object then call Dispose on it
                _disposeStack.Pop().Dispose();
            }
        }

        /// <summary>
        /// Send a message to every user in the world. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        public void Send(BitStream data)
        {
            Send(data, true);
        }

        /// <summary>
        /// Send a message to every user in the world. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="reliable">Whether or not the data should be sent over a reliable stream.</param>
        public void Send(BitStream data, bool reliable)
        {
            // Ensure the data is not null and of a valid length
            if (data == null || data.Length < 1)
            {
                const string errmsg = "Tried to send null or empty data to the World.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Send to all users in all maps
            foreach (Map map in Maps)
            {
                map.Send(data, reliable);
            }
        }

        /// <summary>
        /// Updates the World.
        /// </summary>
        public override void Update()
        {
            ProcessDisposeStack();
            UpdateRespawnables();

            base.Update();
        }

        /// <summary>
        /// When overridden in the derived class, handles updating all of the Maps in this World.
        /// </summary>
        /// <param name="deltaTime">Delta time to use for updating the maps.</param>
        protected override void UpdateMaps(int deltaTime)
        {
            // Update every map
            foreach (Map map in Maps)
            {
                // Make sure we do not have a null map somehow
                if (map == null)
                {
                    const string errmsg = "Tried to update a null map. This should never be null.";
                    if (log.IsWarnEnabled)
                        log.Warn(errmsg);
                    Debug.Fail(errmsg);
                    continue;
                }

                // Update the map 
                map.Update(deltaTime);
            }
        }

        void UpdateRespawnables()
        {
            if (_respawnables.Count == 0)
                return;

            var toRespawn = new Stack<IRespawnable>(_respawnables);
            var respawned = new Stack<IRespawnable>();

            int currentTime = GetTime();

            // Try to respawn each IRespawnable
            while (toRespawn.Count > 0)
            {
                IRespawnable respawnable = toRespawn.Pop();
                if (respawnable.ReadyToRespawn(currentTime))
                {
                    respawnable.Respawn();
                    respawned.Push(respawnable);
                }
            }

            // Remove the successful respawns from the master list
            if (respawned.Count > 0)
                _respawnables.RemoveAll(respawned.Contains);
        }
    }
}