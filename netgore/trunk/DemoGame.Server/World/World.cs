using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DemoGame.Server.Guilds;
using DemoGame.Server.Properties;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Collections;
using NetGore.Db;
using NetGore.Features.Banning;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles the game's world, keeping track of all maps and characters
    /// </summary>
    public class World : WorldBase, IDisposable, IServerSaveable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly ItemTemplateManager _itemTemplateManager = ItemTemplateManager.Instance;

        readonly Queue<IDisposable> _delayedDisposeQueue = new Queue<IDisposable>(4);
        readonly object _delayedDisposeQueueSync = new object();
        readonly GuildMemberPerformer _guildMemberPerformer;
        readonly List<MapInstance> _instancedMaps = new List<MapInstance>();
        readonly object _instancedMapsSync = new object();
        readonly Map[] _maps;
        readonly RespawnTaskList _respawnTaskList;
        readonly Server _server;
        readonly ItemEntity _unarmedWeapon;
        readonly IDictionary<string, User> _users = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        readonly object _usersSync = new object();

        bool _disposed;

        /// <summary>
        /// The time that <see cref="User.SynchronizeExtraUserInformation"/> will be called next.
        /// </summary>
        TickCount _syncExtraUserInfoTime = TickCount.MinValue;

        /// <summary>
        /// The time that that the <see cref="_respawnTaskList"/> will be processed next.
        /// </summary>
        TickCount _updateRespawnablesTime = TickCount.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        /// <param name="parent">Server this world is part of.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The unarmed weapon ID is not a valid item template ID.</exception>
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
            var unarmedWeaponID = ServerSettings.Default.UnarmedItemTemplateID;
            var unarmedWeaponTemplate = _itemTemplateManager[unarmedWeaponID];
            if (unarmedWeaponTemplate == null)
            {
                const string errmsg = "Unable to create unarmed weapon - couldn't find item template with ID `{0}`.";
                if (log.IsFatalEnabled)
                    log.FatalFormat(errmsg, unarmedWeaponID);
                throw new ArgumentException(string.Format(errmsg, unarmedWeaponID));
            }

            _unarmedWeapon = new ItemEntity(unarmedWeaponTemplate, 1);

            // Create the maps
            var mapFiles = MapBase.GetMapFiles(ContentPaths.Build);
            _maps = mapFiles.LoadIntoIndexedArray(x => (int)x.ID, x => 
            {
                MapID mapID;
                if (!MapBase.TryGetIndexFromPath(x, out mapID))
                {
                    const string errmsg = "Failed to get the ID of map file `{0}`.";
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, x);
                    throw new ArgumentException(string.Format(errmsg, x));
                }

                return new Map(mapID, this);
            });
            
            // Load maps in parallel
            Parallel.ForEach(_maps, map =>
            {
                if (map != null)
                    map.Load();
            });

            // Add some event hooks
            BanningManager.Instance.AccountBanned -= BanningManager_AccountBanned;
            BanningManager.Instance.AccountBanned += BanningManager_AccountBanned;
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this World.
        /// </summary>
        public IDbController DbController
        {
            get { return Server.DbController; }
        }

        public GuildMemberPerformer GuildMemberPerformer
        {
            get { return _guildMemberPerformer; }
        }

        /// <summary>
        /// Gets the instanced maps.
        /// </summary>
        public IEnumerable<MapInstance> InstancedMaps
        {
            get
            {
                lock (_instancedMapsSync)
                {
                    return _instancedMaps;
                }
            }
        }

        /// <summary>
        /// Gets all the maps in this <see cref="World"/>.
        /// </summary>
        public IEnumerable<Map> Maps
        {
            get { return _maps.Where(x => x != null); }
        }

        /// <summary>
        /// Gets the <see cref="Server"/> this <see cref="World"/> belongs to.
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
        /// Adds a <see cref="MapInstance"/> to this <see cref="World"/>. Should probably only ever be called from the <see cref="MapInstance"/>
        /// constructor.
        /// </summary>
        /// <param name="instance">The <see cref="MapInstance"/> to add to this <see cref="World"/>.</param>
        public void AddMapInstance(MapInstance instance)
        {
            lock (_instancedMapsSync)
            {
                Debug.Assert(!_instancedMaps.Contains(instance),
                    string.Format("MapInstance `{0}` has already been added to the world `{1}`!", instance, this));

                _instancedMaps.Add(instance);
            }
        }

        /// <summary>
        /// Adds an <see cref="IRespawnable"/> to the list of objects that need to respawn.
        /// </summary>
        /// <param name="respawnable">The object to respawn.</param>
        /// <exception cref="ArgumentNullException"><paramref name="respawnable"/> is null.</exception>
        public void AddToRespawn(IRespawnable respawnable)
        {
            if (respawnable == null)
                throw new ArgumentNullException("respawnable");

            Debug.Assert(respawnable.DynamicEntity != null);

            _respawnTaskList.Add(respawnable);
        }

        /// <summary>
        /// Handles the <see cref="BanningManager.AccountBanned"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="BanningManagerAccountBannedEventArgs"/> instance containing the event data.</param>
        void BanningManager_AccountBanned(IBanningManager<AccountID> sender, BanningManagerAccountBannedEventArgs<AccountID> e)
        {
            // If the user is online, disconnect them

            // Get the name of the users in the account since we have no decent way to look it up a character by AccountID in memory
            var q = DbController.GetQuery<SelectAccountCharacterNamesQuery>();
            var accountChars = q.Execute(e.AccountID);

            // Check for all of the users by their name and, if they are online, disconnect them after we confirm that their
            // account matches the accountID
            foreach (var name in accountChars)
            {
                var c = FindUser(name);
                if (c == null)
                    continue;

                var acc = c.GetAccount();
                if (acc == null)
                    continue;

                if (acc.ID != e.AccountID)
                    continue;

                acc.Dispose(GameMessage.DisconnectedBanned, e.Length.TotalMinutes, e.Reason);
            }
        }

        /// <summary>
        /// Pushes an object into a stack for delayed disposal. This is thread-safe and helps avoid issues with disposing
        /// while in certain places (such as enumerating over a collection). Note that objects pushed into this stack
        /// will be called for disposal once for each time they are added, so be sure to keep track of if an object
        /// is disposed so you can avoid disposing multiple times.
        /// </summary>
        /// <param name="obj">The object to dispose.</param>
        public void DelayedDispose(IDisposable obj)
        {
            lock (_delayedDisposeQueueSync)
            {
                _delayedDisposeQueue.Enqueue(obj);
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

            lock (_usersSync)
            {
                if (!_users.TryGetValue(name, out user))
                    return null;
            }

            return user;
        }

        /// <summary>
        /// Returns a map by its <see cref="MapID"/>.
        /// </summary>
        /// <param name="mapID">The ID of the map.</param>
        /// <returns>Map for the given index, or null if invalid.</returns>
        public Map GetMap(MapID mapID)
        {
            Map map = null;

            if (mapID < _maps.Length && mapID >= 0)
            {
                map = _maps[(int)mapID];
            }

            if (map == null)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("GetMap() on ID `{0}` returned null because map does not exist.", mapID);
            }

            return map;
        }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public override TickCount GetTime()
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
        public static User GetUser(IIPSocket conn, bool errorOnFailure)
        {
            var userAccount = GetUserAccount(conn);
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

            return null;
        }

        /// <summary>
        /// Gets the <see cref="IUserAccount"/> attached to the <paramref name="conn"/>.
        /// </summary>
        /// <param name="conn">The <see cref="IIPSocket"/> to get the <see cref="IUserAccount"/> for.</param>
        /// <param name="warnIfNotFound">When true, when this method returns null, a warning message will be logged.</param>
        /// <returns>
        /// The <see cref="IUserAccount"/> for the <paramref name="conn"/>, or null if there
        /// was a problem getting the <see cref="IUserAccount"/>.
        /// </returns>
        public static IUserAccount GetUserAccount(IIPSocket conn, bool warnIfNotFound = true)
        {
            if (conn.Tag == null)
            {
                if (warnIfNotFound)
                {
                    const string errmsg = "Tried to get the UserAccount from IIPSocket `{0}`, but it has no tag.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, conn);
                }
                return null;
            }

            var userAccount = conn.Tag as IUserAccount;
            if (userAccount == null)
            {
                if (warnIfNotFound)
                {
                    const string errmsg = "Tried to get the UserAccount from IIPSocket `{0}`, but the tag was invalid (`{1}`).";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, conn, conn.Tag);
                }
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
            lock (_usersSync)
            {
                return _users.Values.ToImmutable();
            }
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
            lock (_delayedDisposeQueueSync)
            {
                // Keep popping until the stack is empty
                while (_delayedDisposeQueue.Count > 0)
                {
                    // Pop out the object then call Dispose on it
                    var obj = _delayedDisposeQueue.Dequeue();
                    obj.Dispose();
                }
            }
        }

        /// <summary>
        /// Sends data to all users in the world. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the users.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public void Send(BitStream data, ServerMessageType messageType)
        {
            if (_users.Count <= 0)
                return;

            foreach (var map in Maps)
            {
                map.Send(data, messageType);
            }

            foreach (var map in InstancedMaps)
            {
                map.Send(data, messageType);
            }
        }

        /// <summary>
        /// Sends data to all users in the world. This method is thread-safe.
        /// </summary>
        /// <param name="message">GameMessage to send.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        /// <param name="parameters">Message parameters.</param>
        public void Send(GameMessage message, ServerMessageType messageType, params object[] parameters)
        {
            using (var pw = ServerPacket.SendMessage(message, parameters))
            {
                foreach (var map in Maps)
                {
                    map.Send(pw, messageType);
                }

                foreach (var map in InstancedMaps)
                {
                    map.Send(pw, messageType);
                }
            }
        }

        /// <summary>
        /// Synchronizes the extra user information for all users.
        /// </summary>
        void SyncExtraUserInformation()
        {
            lock (_usersSync)
            {
                foreach (var user in _users.Values)
                {
                    user.SynchronizeExtraUserInformation();
                }
            }
        }

        /// <summary>
        /// Tries to add a <see cref="User"/> to the <see cref="World"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to add.</param>
        /// <returns>True if the <paramref name="user"/> was successfully added to the <see cref="World"/>; otherwise false.</returns>
        public bool TryAddUser(User user)
        {
            if (user == null)
            {
                const string errmsg = "Parameter `user` is null, but logically shouldn't be.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                const string errmsg = "User `{0}` contains a null or invalid name (`{1}`).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, user, user.Name);
                Debug.Fail(string.Format(errmsg, user, user.Name));
                return false;
            }

            var name = user.Name;

            lock (_usersSync)
            {
                // Make sure the user isn't already in the world
                if (_users.ContainsKey(name))
                {
                    const string errmsg = "User with name `{0}` already in the _users collection! Cannot add user `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, name, user);
                    Debug.Fail(string.Format(errmsg, name, user));
                    return false;
                }

                // Add the user to the collection
                _users.Add(user.Name, user);
            }

            // Listen for when the user is disposed
            user.Disposed -= User_Disposed;
            user.Disposed += User_Disposed;

            return true;
        }

        /// <summary>
        /// Updates the World.
        /// </summary>
        public override void Update()
        {
            ThreadAsserts.IsMainThread();

            // Process all of the stuff queued to be disposed
            ProcessDisposeStack();

            var currentTime = GetTime();

            // If enough time has elapsed, update stuff to be respawned
            if (_updateRespawnablesTime < currentTime)
            {
                _updateRespawnablesTime = currentTime + ServerSettings.Default.RespawnablesUpdateRate;
                _respawnTaskList.Process();
            }

            // If enough time has elapsed, update the extra user information
            if (_syncExtraUserInfoTime < currentTime)
            {
                _syncExtraUserInfoTime = currentTime + ServerSettings.Default.SyncExtraUserInformationRate;
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
            // Update non-instanced maps
            foreach (var map in Maps)
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

            // Update instanced maps
            lock (_instancedMapsSync)
            {
                for (var i = 0; i < _instancedMaps.Count; i++)
                {
                    var map = _instancedMaps[i];

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

                    // If the map has been disposed, remove it
                    if (map.IsDisposed)
                    {
                        _instancedMaps.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        /// <summary>
        /// Handles when a User is Disposed.
        /// </summary>
        /// <param name="sender">User that was Disposed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void User_Disposed(Entity sender, EventArgs e)
        {
            // Remove the User from the list of Users
            var name = ((User)sender).Name;

            lock (_usersSync)
            {
                _users.Remove(name);
            }

            // Send a notification to the world that the user left
            var param = new object[] { name };
            Send(GameMessage.UserLeftWorld, ServerMessageType.GUIChat, param);

        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the World and everything in it.
        /// </summary>
        public void Dispose()
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Disposing of `{0}`.", this);

            if (_disposed)
            {
                Debug.Fail("World is already disposed.");
                return;
            }

            // Set the World as disposed
            _disposed = true;

            // Process the dispose stack
            ProcessDisposeStack();

            // Dispose the unarmed weapon item
            _unarmedWeapon.Destroy();

            // Dispose of the maps
            foreach (var map in Maps)
            {
                map.Dispose();
            }

            lock (_instancedMapsSync)
            {
                foreach (var map in InstancedMaps)
                {
                    map.Dispose();
                }
            }

            // Process the dispose stack again, just in case disposing other stuff added to it
            ProcessDisposeStack();
        }

        #endregion

        #region IServerSaveable Members

        /// <summary>
        /// Saves the state of this object and all <see cref="IServerSaveable"/> objects under it to the database.
        /// </summary>
        public void ServerSave()
        {
            foreach (var map in Maps)
            {
                map.ServerSave();
            }
        }

        #endregion

        class RespawnTaskList : TaskList<IRespawnable>
        {
            readonly IGetTime _getTime;

            TickCount _currentTime;

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