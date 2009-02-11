using System;
using System.Bits;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles the game's world, keeping track of all maps and characters
    /// </summary>
    public class World : IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Stack<IDisposable> _disposeStack = new Stack<IDisposable>(4);

        readonly DArray<Map> _maps;
        readonly Server _parent;
        readonly List<User> _users = new List<User>();

        bool _disposed;

        /// <summary>
        /// Time that the World was last updated
        /// </summary>
        int _lastUpdateTime;

        /// <summary>
        /// Gets the MySqlConnection used by the world
        /// </summary>
        public MySqlConnection Conn
        {
            get { return _parent.DBController.Connection; }
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
        /// A list of all the maps in the world
        /// </summary>
        public DArray<Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Gets the server's NPCTemplateManager
        /// </summary>
        public NPCTemplateManager NPCTemplates
        {
            get { return _parent.NPCTemplateManager; }
        }

        /// <summary>
        /// Gets the server the world belongs to
        /// </summary>
        public Server Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// An IEnumerable of all the Users in the world
        /// </summary>
        public IEnumerable<User> Users
        {
            get { return _users; }
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
            _parent = parent;

            // Load the maps
            var mapFiles = Map.GetMapFiles(ContentPaths.Build);
            _maps = new DArray<Map>(mapFiles.Count() + 10, false);
            foreach (string mapFile in mapFiles)
            {
                ushort mapIndex = Map.GetIndexFromPath(mapFile);
                Map m = new Map(mapIndex, this);
                m.Load(ContentPaths.Build);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded map index {0} from file {1}", mapIndex, mapFile);
                _maps[mapIndex] = m;
            }

            // Trim down the maps array under the assumption we won't be adding more maps
            _maps.Trim();

            // Create some test NPCs
            foreach (Map m in _maps)
            {
                for (int i = 0; i < 5; i++)
                {
                    NPC npc = new NPC(this, NPCTemplates.GetTemplate(1));
                    npc.SetMap(m);
                }
            }
        }

        /// <summary>
        /// Adds a user to the world
        /// </summary>
        public void AddUser(User user)
        {
            if (user != null)
                _users.Add(user);
            else
            {
                Debug.Fail("user is null.");
                if (log.IsErrorEnabled)
                    log.Error("user is null.");
            }
        }

        /// <summary>
        /// Disposes of the world and disposes all maps and users on it
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the world and disposes all maps and users on it
        /// </summary>
        /// <param name="disposeManaged">If true, dispose of managed resources</param>
        void Dispose(bool disposeManaged)
        {
            if (_disposed)
            {
                Debug.Fail("World is already disposed.");
                return;
            }

            // Set the World as disposed
            _disposed = true;

            // Dispose of managed objects
            if (disposeManaged)
            {
                foreach (Map map in _maps)
                {
                    _disposeStack.Push(map);
                }

                ProcessDisposeStack();
            }
        }

        /// <summary>
        /// Returns a map by its index
        /// </summary>
        /// <param name="mapIndex">Index of the map</param>
        /// <returns>Map for the given index, or null if invalid</returns>
        public Map GetMap(ushort mapIndex)
        {
            // If the map can be grabbed from the index, grab it
            if (_maps.CanGet(mapIndex))
            {
                // Check that the map is valid
                Debug.Assert(_maps[mapIndex] != null, "Tried to get a null map.");

                // Return the map at the index
                return _maps[mapIndex];
            }

            // Could not grab by index
            if (log.IsWarnEnabled)
                log.WarnFormat("GetMap() on index {0} returned null because map does not exist.");

            return null;
        }

        /// <summary>
        /// Finds a user based on their connection information
        /// </summary>
        /// <param name="conn">Client connection information</param>
        /// <returns>User bound to the connection if any, else null</returns>
        public User GetUser(TCPSocket conn)
        {
            // Check for a user bound to the connection
            if (conn.Tag != null)
                return (User)conn.Tag;

            Debug.Fail("User not bound to connection tag.");
            if (log.IsErrorEnabled)
                log.Error("User not bound to connection tag.");

            User ret = null;

            // No user bound to connection, perform manual search
            foreach (User user in _users)
            {
                if (user.Conn == conn)
                {
                    ret = user;
                    break;
                }
            }

            Debug.Assert(ret != null, "No valid user found.");
            if (log.IsWarnEnabled)
                log.Warn("No valid user found.");

            // Return value, which will be null if none found
            return ret;
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
        /// Send a message to every user in the world
        /// </summary>
        /// <param name="data">BitStream containing the data to send</param>
        public void Send(BitStream data)
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

            // Send to all users in the world
            foreach (User user in _users)
            {
                if (user != null)
                    user.Send(data);
            }
        }

        /// <summary>
        /// Update the world and all of its contents
        /// </summary>
        public void Update(int currentTime)
        {
            // If the last update time is greater than current, we have a problem
            // Too much time has elapsed that if we update, we may cause problems
            if (_lastUpdateTime > currentTime)
            {
                _lastUpdateTime = currentTime;
                return;
            }

            // Set the new update time
            _lastUpdateTime = currentTime;

            // Process the dispose stack
            ProcessDisposeStack();

            // Update every map
            foreach (Map map in _maps)
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

                // Update the map if there are any users on it
                if (map.Users.Count > 0)
                    map.Update();
            }
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time
        /// </summary>
        /// <returns>Current time</returns>
        public int GetTime()
        {
            return _parent.GetTime();
        }

        #endregion
    }
}