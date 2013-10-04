using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Collections;
using NetGore.Db;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information about a single map instance and all of the Entities it contains.
    /// </summary>
    public class Map : MapBase, IDisposable, IServerSaveable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How long, in milliseconds, it takes from when the last User in a Map leaves it for the Map to stop
        /// updating all-together until a User enters the Map again.
        /// </summary>
        const int _emptyMapNoUpdateDelay = 60000;

        /// <summary>
        /// If true, only the <see cref="DynamicEntity"/> objects should be checked when looking for objects
        /// that implement <see cref="IServerSaveable"/>. If false, we will have to check every <see cref="Entity"/>
        /// on the map. This value is determined at runtime through reflection, and will be false if there
        /// is at least one <see cref="Entity"/> that is not a <see cref="DynamicEntity"/> but implements
        /// <see cref="IServerSaveable"/>. This is much faster when true since there will always be much less
        /// <see cref="DynamicEntity"/> types than all <see cref="Entity"/> types, namely due to all
        /// <see cref="WallEntityBase"/>s that usually reside on a map.
        /// </summary>
        static readonly bool _checkDynamicEntitiesForIServerSaveableOnly;

        readonly MapItemsCleaner _mapItemsCleaner = new MapItemsCleaner();

        readonly List<NPC> _npcs = new List<NPC>();
        readonly TSList<User> _users = new TSList<User>();
        readonly World _world;

        bool _disposed;

        /// <summary>
        /// How much time, in milliseconds, remaining until the Map goes inactive. When this value is less than
        /// or equal to 0, the Map should be considered inactive.
        /// </summary>
        int _inactiveCounter;

        bool _isLoaded = false;

        /// <summary>
        /// IEnumerable of the NPCSpawners on this Map.
        /// </summary>
        IEnumerable<NPCSpawner> _npcSpawners;

        /// <summary>
        /// Initializes the <see cref="Map"/> class.
        /// </summary>
        static Map()
        {
            // Cache the _checkDynamicEntitiesForIServerSaveableOnly value
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                Subclass = typeof(Entity),
                Interfaces = new Type[] { typeof(IServerSaveable) },
                CustomFilter = (x => !x.IsSubclassOf(typeof(DynamicEntity)))
            };

            var filter = filterCreator.GetFilter();

            _checkDynamicEntitiesForIServerSaveableOnly = TypeHelper.FindTypes(filter, null).IsEmpty();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="mapID">ID of the Map.</param>
        /// <param name="world">World that the Map will be inside of.</param>
        /// <exception cref="ArgumentException"><paramref name="mapID"/> returned false for <see cref="MapBase.MapIDExists"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="world"/> is null.</exception>
        public Map(MapID mapID, World world) : base(mapID, world)
        {
            _world = world;
            if (log.IsInfoEnabled)
                log.InfoFormat("Created Map `{0}`.", this);
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this Map.
        /// </summary>
        public IDbController DbController
        {
            get { return World.DbController; }
        }

        /// <summary>
        /// Gets if this <see cref="Map"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Gets if the Map is currently inactive.
        /// </summary>
        bool IsInactive
        {
            get { return _inactiveCounter <= 0; }
        }

        /// <summary>
        /// Gets if this is an instanced map.
        /// </summary>
        public virtual bool IsInstanced
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this <see cref="Map"/> has been loaded.
        /// </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// Gets all of the NPCSpawners on this Map.
        /// </summary>
        public IEnumerable<NPCSpawner> NPCSpawners
        {
            get { return _npcSpawners; }
        }

        /// <summary>
        /// Gets all of the NPCs on the Map.
        /// </summary>
        public IEnumerable<NPC> NPCs
        {
            get { return _npcs; }
        }

        /// <summary>
        /// Gets all of the users on the Map.
        /// </summary>
        public IEnumerable<User> Users
        {
            get { return _users; }
        }

        /// <summary>
        /// Gets the World the Map belongs to.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Adds an Entity to the map.
        /// </summary>
        /// <param name="entity">Entity to add to the map.</param>
        public override void AddEntity(Entity entity)
        {
            // Add IRespawnable entities that are not ready to spawn to the respawn queue.
            // Everything else goes right into the map like normal.
            var respawnable = entity as IRespawnable;
            if (respawnable != null && !respawnable.ReadyToRespawn(GetTime()))
            {
                AddToRespawn(respawnable);
                return;
            }

            base.AddEntity(entity);
        }

        /// <summary>
        /// Adds an IRespawnable to the list of objects that need to respawn.
        /// </summary>
        /// <param name="respawnable">The object to respawn.</param>
        public void AddToRespawn(IRespawnable respawnable)
        {
            World.AddToRespawn(respawnable);
        }

        /// <summary>
        /// Handles when a Character is added to the Map. This is an extension of EntityAdded that handles
        /// special stuff just for Characters.
        /// </summary>
        /// <param name="character">The Character that was added to the map.</param>
        /// <exception cref="TypeException">Unknown Character type - not a NPC or User...?</exception>
        void CharacterAdded(Character character)
        {
            // If the character was already on a map, so remove them from the old map
            if (character.Map != null && character.Map != this)
            {
                const string errmsg = "Character `{0}` [{1}] added to new map, but is already on a map!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, character, character.MapEntityIndex);
                Debug.Fail(string.Format(errmsg, character, character.MapEntityIndex));
                character.Map.RemoveEntity(character);
            }

            // Syncronise paperdoll layers to all users on the same map
            foreach (User userOnMap in character.Map.Users)
            {
                if (userOnMap != null)
                {
                    character.SynchronizePaperdollTo(userOnMap);
                }
            }

            // Added character is a User
            var user = character as User;
            if (user != null)
            {
                EventCounterManager.Map.Increment(ID, MapEventCounterType.UserAdded);

                Debug.Assert(!Users.Contains(user), string.Format("Users list already contains `{0}`!", user));
                _users.Add(user);
                SendMapData(user);
                return;
            }

            // Added character is a NPC
            var npc = character as NPC;
            if (npc != null)
            {
                EventCounterManager.Map.Increment(ID, MapEventCounterType.NPCAdded);

                Debug.Assert(!NPCs.Contains(npc), string.Format("NPCs list already contains `{0}`!", npc));
                _npcs.Add(npc);
                return;
            }

            // Unknown added character type - not actually an error, but it is likely an oversight
            throw new TypeException("Unknown Character type - not a NPC or User...?");
        }

        /// <summary>
        /// Handles when a Character is removed from the Map. This is an extension of EntityRemoved that handles
        /// special stuff just for Characters.
        /// </summary>
        /// <param name="character">The Character that was removed from the Map.</param>
        void CharacterRemoved(Character character)
        {
            User user;
            NPC npc;

            if ((user = character as User) != null)
                _users.Remove(user);
            else if ((npc = character as NPC) != null)
                _npcs.Remove(npc);
        }

        /// <summary>
        /// Creates an ItemEntity on the map.
        /// </summary>
        /// <param name="template">ItemTemplate to create the characterID from.</param>
        /// <param name="pos">Position to create the characterID at.</param>
        /// <param name="amount">Amount of the characterID to create. Must be greater than 0.</param>
        /// <returns>Reference to the new ItemEntity created.</returns>
        public ItemEntity CreateItem(IItemTemplateTable template, Vector2 pos, byte amount)
        {
            // Check for a valid amount
            if (amount < 1)
            {
                const string errmsg = "Invalid characterID amount `{0}`! Amount must be > 0.";
                Debug.Fail(string.Format(errmsg, amount));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, amount);
                return null;
            }

            // Check for a valid template
            if (template == null)
            {
                const string errmsg = "Parameter `template` may not be null!";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return null;
            }

            // Create the characterID, add it to the map, and return the reference
            var item = new ItemEntity(template, pos, amount);
            AddEntity(item);
            return item;
        }

        /// <summary>
        /// When overridden in the derived class, creates a new WallEntityBase instance.
        /// </summary>
        /// <returns>WallEntityBase that is to be used on the map.</returns>
        protected override WallEntityBase CreateWall(IValueReader r)
        {
            return new WallEntity(r);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities added to the map.
        /// This is called after the Entity has finished being added to the map.
        /// </summary>
        /// <param name="entity">Entity that was added to the map.</param>
        protected override void EntityAdded(Entity entity)
        {
            base.EntityAdded(entity);

            // When a User enters the Map, reset the inactivity counter
            if (entity is User)
                _inactiveCounter = _emptyMapNoUpdateDelay;

            // Create the DynamicEntity for everyone on the map
            if (_users.Count > 0)
            {
                var de = entity as DynamicEntity;
                if (de != null)
                {
                    using (var pw = ServerPacket.CreateDynamicEntity(de))
                    {
                        Send(pw, ServerMessageType.Map);
                    }
                }
            }

            // Add items to the MapItemsCleaner
            var item = entity as ItemEntityBase;
            if (item != null)
            {
                if (item is ItemEntity)
                    ((ItemEntity)item).IsPersistent = false;

                EventCounterManager.Map.Increment(ID, MapEventCounterType.ItemAdded, item.Amount);

                _mapItemsCleaner.Add(item, GetTime());
            }

            // Handle the different types of entities
            var character = entity as Character;
            if (character != null)
                CharacterAdded(character);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities removed from the map.
        /// This is called after the Entity has finished being removed from the map.
        /// </summary>
        /// <param name="entity">Entity that was removed from the map.</param>
        protected override void EntityRemoved(Entity entity)
        {
            base.EntityRemoved(entity);

            // Handle the different types of entities
            DynamicEntity dynamicEntity;
            if ((dynamicEntity = entity as DynamicEntity) != null)
            {
                var character = entity as Character;
                if (character != null)
                    CharacterRemoved(character);

                // Destroy the DynamicEntity for everyone on the map
                if (_users.Count > 0)
                {
                    using (var pw = ServerPacket.RemoveDynamicEntity(dynamicEntity.MapEntityIndex))
                    {
                        Send(pw, ServerMessageType.Map);
                    }
                }
            }

            // Remove items to the MapItemsCleaner
            var item = entity as ItemEntityBase;
            if (item != null)
                _mapItemsCleaner.Remove(item);
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for. This should include
        /// all the <see cref="Type"/>s that are used frequently when querying the map's spatial collection.
        /// </summary>
        /// <returns>
        /// An IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for.
        /// </returns>
        protected override IEnumerable<Type> GetSpatialTypes()
        {
            return base.GetSpatialTypes().Concat(new Type[] { typeof(NPC), typeof(User) });
        }

        /// <summary>
        /// Finds the Users close enough to the <paramref name="toSynchronize"/> to synchronize their
        /// Position and Velocity to.
        /// </summary>
        /// <param name="toSynchronize">The <see cref="ISpatial"/> to synchronize.</param>
        /// <returns>An IEnumerable of Users close enough to the <paramref name="toSynchronize"/> that they
        /// need to have the <paramref name="toSynchronize"/>'s Position and Velocity synchronized.</returns>
        IEnumerable<User> GetUsersToSyncPandVTo(ISpatial toSynchronize)
        {
            int xPad = (int)(GameData.ScreenSize.X * 1.5);
            int yPad = (int)(GameData.ScreenSize.Y * 1.5);

            var syncRegion = toSynchronize.ToRectangle().Inflate(xPad, yPad);

            foreach (var user in Users)
            {
                Rectangle userRegion = user.ToRectangle();
                if (syncRegion.Intersects(userRegion))
                    yield return user;
            }
        }

        /// <summary>
        /// Loads the map.
        /// </summary>
        public void Load()
        {
            if (_isLoaded)
                return;

            _isLoaded = true;

            // Load the map
            Load(ContentPaths.Build, true, DynamicEntityFactory.Instance);

            // NPCs
            _npcSpawners = LoadNPCSpawners();
            LoadPersistentNPCs();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Map `{0}`.", this);
        }

        /// <summary>
        /// Handles loading the <see cref="NPCSpawner"/>s on the map.
        /// </summary>
        /// <returns>The <see cref="NPCSpawner"/>s on the map.</returns>
        protected IEnumerable<NPCSpawner> LoadNPCSpawners()
        {
            return NPCSpawner.LoadSpawners(this).ToCompact();
        }

        /// <summary>
        /// Handles loading the persistent NPCs on the map.
        /// </summary>
        /// <returns>The persistent <see cref="NPC"/>s that were loaded.</returns>
        protected virtual IEnumerable<NPC> LoadPersistentNPCs()
        {
            List<NPC> ret = new List<NPC>();
            var persistentNPCIDs = DbController.GetQuery<SelectPersistentMapNPCsByRespawnMapQuery>().Execute(ID);
            foreach (var characterID in persistentNPCIDs)
            {
                ret.Add(new NPC(World, characterID));
            }
            return ret;
        }

        /// <summary>
        /// Sends data to all users in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the users.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public void Send(BitStream data, ServerMessageType messageType)
        {
            if (_users.Count == 0)
                return;

            foreach (var user in Users)
            {
                user.Send(data, messageType);
            }
        }

        /// <summary>
        /// Sends data to all users in the map. This method is thread-safe.
        /// </summary>
        /// <param name="message">GameMessage to send.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        public void Send(GameMessage message, ServerMessageType messageType)
        {
            Send(message, messageType, null);
        }

        /// <summary>
        /// Sends data to all users in the map. This method is thread-safe.
        /// </summary>
        /// <param name="message">GameMessage to send.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        /// <param name="parameters">Message parameters.</param>
        public void Send(GameMessage message, ServerMessageType messageType, params object[] parameters)
        {
            if (_users.Count == 0)
                return;

            using (var pw = ServerPacket.SendMessage(message, parameters))
            {
                foreach (var user in Users)
                {
                    user.Send(pw, messageType);
                }
            }
        }

        /// <summary>
        /// Sends the data to the specified user of all existing content on the map.
        /// </summary>
        /// <param name="user">User to send the map data to.</param>
        void SendMapData(User user)
        {
            using (var pw = ServerPacket.GetWriter())
            {
                // Tell the user to change the map
                ServerPacket.SetMap(pw, ID);
                user.Send(pw, ServerMessageType.Map);

                // Send dynamic entities
                foreach (var dynamicEntity in DynamicEntities)
                {
                    pw.Reset();
                    ServerPacket.CreateDynamicEntity(pw, dynamicEntity);
                    user.Send(pw, ServerMessageType.Map);

                    // Perform special synchronizations for Characters
                    var character = dynamicEntity as Character;
                    if (character != null)
                    {
                        character.SynchronizeSPTo(user);
                        character.SynchronizePaperdollTo(user);
                    }
                }

                // Now that the user know about the Map and every Entity on it, tell them which one is theirs
                pw.Reset();
                ServerPacket.SetUserChar(pw, user.MapEntityIndex);
                user.Send(pw, ServerMessageType.Map);
            }
        }

        /// <summary>
        /// Send a packet to every user in the map within a reasonable range from the origin. Use this for packets
        /// that only affect those who are already in view from the origin such as brief visual effects.
        /// </summary>
        /// <param name="origin">The <see cref="ISpatial"/> that the event comes from.</param>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public void SendToArea(ISpatial origin, BitStream data, ServerMessageType messageType)
        {
            SendToArea(origin.Position + (origin.Size / 2f), data, messageType);
        }

        /// <summary>
        /// Send a packet to every user in the map within a reasonable range from the origin. Use this for packets
        /// that only affect those who are already in view from the origin such as brief visual effects.
        /// </summary>
        /// <param name="origin">Position in which the event creating the packet triggered.</param>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public void SendToArea(Vector2 origin, BitStream data, ServerMessageType messageType)
        {
            if (data == null)
                return;

            var screenSize = GameData.ScreenSize * 1.25f;
            var min = origin - screenSize;
            var max = origin + screenSize;

            foreach (var user in Users)
            {
                Debug.Assert(user != null, "There shouldn't be null users in the Users list!");

                var p = user.Position;
                if (p.X > min.X && p.Y > min.Y && p.X < max.X && p.Y < max.Y)
                    user.Send(data, messageType);
            }
        }

        /// <summary>
        /// Synchronizes all of the DynamicEntities.
        /// </summary>
        void SynchronizeDynamicEntities()
        {
            // Don't need to synchronize a map that has no Users on it since there would be nobody to synchronize to!
            if (_users.Count == 0)
                return;

            var currentTime = GetTime();

            using (var pw = ServerPacket.GetWriter())
            {
                // Loop through each DynamicEntity
                foreach (var dynamicEntity in DynamicEntities)
                {
                    // Check to synchronize everything but the Position and Velocity
                    if (!dynamicEntity.IsSynchronized)
                    {
                        // Write the data into the PacketWriter, then send it to everyone on the map
                        pw.Reset();
                        ServerPacket.SynchronizeDynamicEntity(pw, dynamicEntity);
                        Send(pw, ServerMessageType.MapDynamicEntityProperty);
                    }

                    // Check to synchronize the Position and Velocity
                    if (dynamicEntity.NeedSyncPositionAndVelocity(currentTime))
                    {
                        // Make sure there are users in range since, if there isn't, we don't even need to synchronize
                        var usersToSyncTo = GetUsersToSyncPandVTo(dynamicEntity);
                        if (usersToSyncTo.IsEmpty())
                            dynamicEntity.BypassPositionAndVelocitySync(currentTime);
                        else
                        {
                            pw.Reset();
                            ServerPacket.UpdateVelocityAndPosition(pw, dynamicEntity, currentTime);
                            foreach (var user in usersToSyncTo)
                            {
                                user.Send(pw, ServerMessageType.MapDynamicEntitySpatialUpdate);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name + " [" + ID + "]";
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="deltaTime">The amount of time that elapsed since the last update.</param>
        public override void Update(int deltaTime)
        {
            if (IsDisposed)
                return;

            // This assert will have to be removed once/if we add support for multithreaded World updates
            ThreadAsserts.IsMainThread();

            // If there are no Users on the Map, update the inactive counter or skip updating if already inactive
            if (_users.Count == 0)
            {
                if (IsInactive)
                    return;

                _inactiveCounter -= deltaTime;
            }

            // Remove old map items
            _mapItemsCleaner.Update(GetTime());

            base.Update(deltaTime);

            SynchronizeDynamicEntities();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the map and all of the Entities on it.
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed)
            {
                const string errmsg = "Tried to dispose of map `{0}`, but it is already disposed!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("Disposing of `{0}`.", this);

            _disposed = true;

            // Dispose of all the disposable entities
            foreach (var entity in Entities.ToImmutable())
            {
                World.DelayedDispose(entity);
            }
        }

        #endregion

        #region IServerSaveable Members

        /// <summary>
        /// Saves the state of this object and all <see cref="IServerSaveable"/> objects under it to the database.
        /// </summary>
        public void ServerSave()
        {
            // If possible, check only DynamicEntities. See the notes on _checkDynamicEntitiesForIServerSaveableOnly
            // for more details.
            if (_checkDynamicEntitiesForIServerSaveableOnly)
            {
                // Check only the DynamicEntities (fast)
                foreach (var saveable in DynamicEntities.OfType<IServerSaveable>())
                {
                    saveable.ServerSave();
                }
            }
            else
            {
                // Check all Entities (slow)
                foreach (var saveable in Entities.OfType<IServerSaveable>())
                {
                    saveable.ServerSave();
                }
            }
        }

        #endregion
    }
}