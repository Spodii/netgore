using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Guilds;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Collections;
using NetGore.Db;
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
        static readonly ItemTemplateManager _itemTemplateManager = ItemTemplateManager.Instance;

        readonly Stack<IDisposable> _disposeStack = new Stack<IDisposable>(4);
        readonly GuildMemberPerformer _guildMemberPerformer;
        readonly DArray<Map> _maps;
        readonly RespawnTaskList _respawnTaskList;
        readonly Server _server;
        readonly ItemEntity _unarmedWeapon;
        readonly IDictionary<string, User> _users = new TSDictionary<string, User>(StringComparer.OrdinalIgnoreCase);

        bool _disposed;

        /// <summary>
        /// The time that <see cref="User.SynchronizeExtraUserInformation"/> will be called next.
        /// </summary>
        int _syncExtraUserInfoTime = int.MinValue;

        /// <summary>
        /// The time that that the <see cref="_respawnTaskList"/> will be processed next.
        /// </summary>
        int _updateRespawnablesTime = int.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        /// <param name="parent">Server this world is part of.</param>
        public World(Server parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _respawnTaskList = new RespawnTaskList(this);

            // Store the parent
            _server = parent;

            // Create some objects
            _guildMemberPerformer = new GuildMemberPerformer(DbController, FindUser);

            // Create the unarmed weapon
            _unarmedWeapon = new ItemEntity(_itemTemplateManager[ServerSettings.UnarmedItemTemplateID], 1);

            // Load the maps
            var mapFiles = MapBase.GetMapFiles(ContentPaths.Build);
            _maps = new DArray<Map>(mapFiles.Count() + 10, false);
            foreach (string mapFile in mapFiles)
            {
                MapIndex mapIndex;
                if (!MapBase.TryGetIndexFromPath(mapFile, out mapIndex))
                    throw new Exception(string.Format("Failed to get the index of map file `{0}`.", mapFile));

                Map m = new Map(mapIndex, this);
                _maps[(int)mapIndex] = m;
                m.Load();
            }

            // Trim down the maps array under the assumption we won't be adding more maps
            _maps.Trim();

#if true
            // NOTE: Create some test items
            Random rand = new Random();
            foreach (Map m in Maps)
            {
                for (int i = 0; i < 10; i++)
                {
                    float x = rand.Next(128, (int)m.Width - 256);
                    float y = rand.Next(128, (int)m.Height - 256);

                    IItemTemplateTable template = null;
                    while (template == null || template.ID == ServerSettings.UnarmedItemTemplateID)
                    {
                        template = _itemTemplateManager.GetRandomTemplate();
                    }

                    new ItemEntity(template, new Vector2(x, y), 1, m);
                }
            }
#endif
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this World.
        /// </summary>
        public IDbController DbController
        {
            get { return Server.DbController; }
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

        public GuildMemberPerformer GuildMemberPerformer
        {
            get { return _guildMemberPerformer; }
        }

        /// <summary>
        /// Gets an IEnuermable of all the maps in the world.
        /// </summary>
        public IEnumerable<Map> Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Gets the server the world belongs to
        /// </summary>
        public Server Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Gets the <see cref="ItemEntity"/> that is used to represent attacking unarmed (i.e. fists). This is
        /// the item used for attacking when no weapon is specified.
        /// </summary>
        public ItemEntity UnarmedWeapon
        {
            get { return _unarmedWeapon; }
        }

        /// <summary>
        /// Adds an IRespawnable to the list of objects that need to respawn.
        /// </summary>
        /// <param name="respawnable">The object to respawn.</param>
        public void AddToRespawn(IRespawnable respawnable)
        {
            if (respawnable == null)
                throw new ArgumentNullException("respawnable");

            Debug.Assert(respawnable.DynamicEntity != null);

            _respawnTaskList.Add(respawnable);
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

            // TODO: If the user is already logged in, this will throw an exception. Will have to determine how to handle this scenario.
            user.Disposed += User_Disposed;
            _users.Add(user.Name, user);
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
        /// <param name="errorOnFailure">If true, an error message will be generated when the <see cref="User"/>
        /// for the <see cref="IIPSocket"/> cannot be found. If false, it will fail silently.</param>
        /// <returns>
        /// User bound to the connection if any, else null.
        /// </returns>
        public User GetUser(IIPSocket conn, bool errorOnFailure)
        {
            UserAccount userAccount = GetUserAccount(conn);
            if (userAccount == null)
                return null;

            if (userAccount.User != null)
                return userAccount.User;

            if (errorOnFailure)
            {
                const string errmsg = "User not bound to UserAccount.User property.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
            }

            // No user bound to connection, perform manual search
            User ret = _users.Values.FirstOrDefault(x => x.Conn == conn);
            if (ret == null && errorOnFailure)
            {
                const string errmsg2 = "No user found on socket `{0}`.";
                Debug.Fail(string.Format(errmsg2, conn));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg2, conn);
            }

            // Return value, which will be null if none found
            return ret;
        }

        /// <summary>
        /// Gets the <see cref="UserAccount"/> attached to the <paramref name="conn"/>.
        /// </summary>
        /// <param name="conn">The <see cref="IIPSocket"/> to get the <see cref="UserAccount"/> for.</param>
        /// <returns>The <see cref="UserAccount"/> for the <paramref name="conn"/>, or null if there
        /// was a problem getting the <see cref="UserAccount"/>.</returns>
        public static UserAccount GetUserAccount(IIPSocket conn)
        {
            if (conn.Tag == null)
            {
                const string errmsg = "Tried to get the UserAccount from IIPSocket `{0}`, but it has no tag.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, conn);
                return null;
            }

            UserAccount userAccount = conn.Tag as UserAccount;
            if (userAccount == null)
            {
                const string errmsg = "Tried to get the UserAccount from IIPSocket `{0}`, but the tag was invalid (`{1}`).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, conn, conn.Tag);
                return null;
            }

            return userAccount;
        }

        /// <summary>
        /// Gets all the Users in the world.
        /// </summary>
        /// <returns>All the Users in the world.</returns>
        public IEnumerable<User> GetUsers()
        {
            return _users.Values.ToImmutable();
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
        /// Synchronizes the extra user information for all users.
        /// </summary>
        void SyncExtraUserInformation()
        {
            foreach (var user in _users.Values)
            {
                user.SynchronizeExtraUserInformation();
            }
        }

        /// <summary>
        /// Updates the World.
        /// </summary>
        public override void Update()
        {
            ThreadAsserts.IsMainThread();

            ProcessDisposeStack();

            int currentTime = GetTime();

            if (_updateRespawnablesTime < currentTime)
            {
                _updateRespawnablesTime = currentTime + ServerSettings.RespawnablesUpdateRate;
                _respawnTaskList.Process();
            }

            if (_syncExtraUserInfoTime < currentTime)
            {
                _syncExtraUserInfoTime = currentTime + ServerSettings.SyncExtraUserInformationRate;
                SyncExtraUserInformation();
            }

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

        /// <summary>
        /// Handles when a User is Disposed.
        /// </summary>
        /// <param name="entity">User that was Disposed.</param>
        void User_Disposed(Entity entity)
        {
            // Remove the User from the list of Users
            _users.Remove(((User)entity).Name);
        }

        #region IDisposable Members

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
            {
                map.Dispose();
            }

            // Process the dispose stack
            while (_disposeStack.Count > 0)
            {
                ProcessDisposeStack();
            }
        }

        #endregion

        class RespawnTaskList : TaskList<IRespawnable>
        {
            readonly IGetTime _getTime;

            int _currentTime;

            /// <summary>
            /// Initializes a new instance of the <see cref="RespawnTaskList"/> class.
            /// </summary>
            /// <param name="getTime">The <see cref="IGetTime"/>.</param>
            public RespawnTaskList(IGetTime getTime)
            {
                _getTime = getTime;
            }

            /// <summary>
            /// When overridden in the derived class, allows for additional processing before tasks are processed.
            /// </summary>
            protected override void PreProcess()
            {
                _currentTime = _getTime.GetTime();
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the given task.
            /// </summary>
            /// <param name="item">The value of the task to process.</param>
            /// <returns>True if the <paramref name="item"/> is to be removed from the collection; otherwise false.</returns>
            protected override bool ProcessItem(IRespawnable item)
            {
                if (!item.ReadyToRespawn(_currentTime))
                    return false;

                item.Respawn();
                return true;
            }
        }
    }
}