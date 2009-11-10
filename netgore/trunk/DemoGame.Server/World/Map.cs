using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
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
    /// Contains the information about a single map instance and all of the Entities it contains.
    /// </summary>
    public class Map : MapBase, IDisposable
    {
        /// <summary>
        /// How long, in milliseconds, it takes from when the last User in a Map leaves it for the Map to stop
        /// updating all-together until a User enters the Map again.
        /// </summary>
        const int _emptyMapNoUpdateDelay = 60000;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<NPC> _npcs;
        readonly TSList<User> _users;
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
        /// Gets the <see cref="IDbController"/> used by this Map.
        /// </summary>
        public IDbController DbController
        {
            get { return World.DbController; }
        }

        /// <summary>
        /// Gets if the Map is currently inactive.
        /// </summary>
        bool IsInactive
        {
            get { return _inactiveCounter <= 0; }
        }

        /// <summary>
        /// Gets if this <see cref="Map"/> has been loaded.
        /// </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// Gets an IEnumerable of NPCs on the Map.
        /// </summary>
        public IEnumerable<NPC> NPCs
        {
            get { return _npcs; }
        }

        /// <summary>
        /// Gets an IEnumerable of the NPCSpawners on this Map.
        /// </summary>
        public IEnumerable<NPCSpawner> NPCSpawners
        {
            get { return _npcSpawners; }
        }

        /// <summary>
        /// Gets the list of users on the Map.
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
        /// Map constructor.
        /// </summary>
        /// <param name="mapIndex">Index of the Map.</param>
        /// <param name="world">World that the Map will be inside of.</param>
        public Map(MapIndex mapIndex, World world) : base(mapIndex, world)
        {
            _world = world;

            _npcs = new List<NPC>();
            _users = new TSList<User>();

            if (log.IsInfoEnabled)
                log.InfoFormat("Created Map `{0}`.", this);
        }

        /// <summary>
        /// Adds an Entity to the map.
        /// </summary>
        /// <param name="entity">Entity to add to the map.</param>
        public override void AddEntity(Entity entity)
        {
            // Add IRespawnable entities that are not ready to spawn to the respawn queue
            // Everything else goes right into the map like normal
            IRespawnable respawnable = entity as IRespawnable;
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
        void CharacterAdded(Character character)
        {
            // If the character was already on a map, so remove them from the old map
            if (character.Map != null)
            {
                const string errmsg = "Character `{0}` [{1}] added to new map, but is already on a map!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, character, character.MapEntityIndex);
                Debug.Fail(string.Format(errmsg, character, character.MapEntityIndex));
                character.Map.RemoveEntity(character);
            }

            // Set the new map
            character.Map = this;

            // Added character is a User
            User user = character as User;
            if (user != null)
            {
                Debug.Assert(!Users.Contains(user), string.Format("Users list already contains `{0}`!", user));
                _users.Add(user);
                SendMapData(user);
                return;
            }

            // Added character is a NPC
            NPC npc = character as NPC;
            if (npc != null)
            {
                Debug.Assert(!NPCs.Contains(npc), string.Format("NPCs list already contains `{0}`!", npc));
                _npcs.Add(npc);
                return;
            }

            // Unknown added character type - not actually an error, but it is likely an oversight
            throw new Exception("Unknown Character type - not a NPC or User...?");
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

            return;
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
            ItemEntity item = new ItemEntity(template, pos, amount);
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
                DynamicEntity de = entity as DynamicEntity;
                if (de != null)
                {
                    using (PacketWriter pw = ServerPacket.CreateDynamicEntity(de))
                    {
                        Send(pw);
                    }
                }
            }

            // Handle the different types of entities
            Character character = entity as Character;
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
                Character character = entity as Character;
                if (character != null)
                    CharacterRemoved(character);

                // Destroy the DynamicEntity for everyone on the map
                if (_users.Count > 0)
                {
                    using (PacketWriter pw = ServerPacket.RemoveDynamicEntity(dynamicEntity))
                    {
                        Send(pw);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the Users close enough to the <paramref name="entityToSynchronize"/> to synchronize their
        /// Position and Velocity to.
        /// </summary>
        /// <param name="entityToSynchronize">The DyanmicEntity to synchronize.</param>
        /// <returns>An IEnumerable of Users close enough to the <paramref name="entityToSynchronize"/> that they
        /// need to have the <paramref name="entityToSynchronize"/>'s Position and Velocity synchronized.</returns>
        // ReSharper disable SuggestBaseTypeForParameter
        IEnumerable<User> GetUsersToSyncPandVTo(DynamicEntity entityToSynchronize)
            // ReSharper restore SuggestBaseTypeForParameter
        {
            int xPad = (int)GameData.ScreenSize.X;
            int yPad = (int)GameData.ScreenSize.Y;

            Rectangle r = entityToSynchronize.CB.ToRectangle();
            Rectangle syncRegion = new Rectangle(r.X - xPad, r.Y - yPad, r.Width + xPad * 2, r.Height + xPad * 2);

            foreach (User user in Users)
            {
                Rectangle userRegion = user.CB.ToRectangle();
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

            Load(ContentPaths.Build, true);

            _npcSpawners = NPCSpawner.LoadSpawners(this).ToArray();

            // Spawn persistent NPCs
            var persistentNPCIDs = DbController.GetQuery<SelectPersistentMapNPCsQuery>().Execute(Index);
            foreach (var characterID in persistentNPCIDs)
            {
                new NPC(World, characterID);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Map `{0}`.", this);
        }

        /// <summary>
        /// Send a message to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        public void Send(BitStream data)
        {
            Send(data, null, true);
        }

        /// <summary>
        /// Send a message to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="reliable">Whether or not the data should be sent over a reliable stream.</param>
        public void Send(BitStream data, bool reliable)
        {
            Send(data, null, reliable);
        }

        /// <summary>
        /// Send a packet to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="skipUser">User to skip sending to.</param>
        public void Send(BitStream data, User skipUser)
        {
            Send(data, skipUser, true);
        }

        /// <summary>
        /// Send a packet to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="skipUser">User to skip sending to.</param>
        /// <param name="reliable">Whether or not the data should be sent over a reliable stream.</param>
        public void Send(BitStream data, User skipUser, bool reliable)
        {
            // Check for valid data
            if (data == null || data.Length < 1)
            {
                const string errmsg = "Attempted to send null or invalid data to the map.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Send the data to all users in the map
            foreach (User user in Users)
            {
                if (user != null)
                {
                    if (user != skipUser)
                        user.Send(data, reliable);
                }
                else
                {
                    const string errmsg = "Null User found in the Map's User list. This should never happen.";
                    Debug.Fail(errmsg);
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                }
            }
        }

        /// <summary>
        /// Sends the data to the specified user of all existing content on the map
        /// </summary>
        /// <param name="user">User to send the map data to</param>
        void SendMapData(User user)
        {
            using (PacketWriter pw = ServerPacket.GetWriter())
            {
                // Tell the user to change the map
                ServerPacket.SetMap(pw, Index);
                user.Send(pw);

                // NOTE: !! Temp
                Debug.Print("Map: " + Index);
                foreach (var de in DynamicEntities)
                    Debug.Print(de.MapEntityIndex + " - " + de);

                if (DynamicEntities.HasDuplicates())
                {
                }

                // Send dynamic entities
                foreach (DynamicEntity dynamicEntity in DynamicEntities)
                {
                    pw.Reset();
                    ServerPacket.CreateDynamicEntity(pw, dynamicEntity);
                    user.Send(pw);

                    Character character = dynamicEntity as Character;
                    if (character != null)
                        character.SynchronizeSPTo(user);
                }

                // Now that the user know about the Map and every Entity on it, tell them which one is theirs
                pw.Reset();
                ServerPacket.SetUserChar(pw, user.MapEntityIndex);
                user.Send(pw);
            }
        }

        /// <summary>
        /// Send a packet to every user in the map within a reasonable
        /// range from the origin. Use this for packets that only affect
        /// those who are already in view from the origin such as brief
        /// visual effects.
        /// </summary>
        /// <param name="origin">Position in which the event creating the packet triggered</param>
        /// <param name="data">BitStream containing the data to send</param>
        public void SendToArea(Vector2 origin, BitStream data)
        {
            if (data == null)
                return;

            Vector2 screenSize = GameData.ScreenSize * 1.25f;
            Vector2 min = origin - screenSize;
            Vector2 max = origin + screenSize;

            foreach (User user in Users)
            {
                if (user != null)
                {
                    Vector2 p = user.Position;
                    if (p.X > min.X && p.Y > min.Y && p.X < max.X && p.Y < max.Y)
                        user.Send(data);
                }
                else
                    Debug.Fail("Null user found in Map's Users list.");
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

            int currentTime = GetTime();

            using (PacketWriter pw = ServerPacket.GetWriter())
            {
                // Loop through each DynamicEntity
                foreach (DynamicEntity dynamicEntity in DynamicEntities)
                {
                    // Check to synchronize everything but the Position and Velocity
                    if (!dynamicEntity.IsSynchronized)
                    {
                        // Write the data into the PacketWriter, then send it to everyone on the map
                        pw.Reset();
                        ServerPacket.SynchronizeDynamicEntity(pw, dynamicEntity);
                        Send(pw);
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
                            foreach (User user in usersToSyncTo)
                            {
                                user.SendUnreliableBuffered(pw);
                            }
                        }
                    }
                }
            }

            // Flush the unreliable buffers for all of the users
            foreach (User user in Users)
            {
                user.FlushUnreliableBuffer();
            }
        }

        public override void Update(int deltaTime)
        {
            // NOTE: This assert will have to be removed once we add support for multithreaded World updates
            ThreadAsserts.IsMainThread();

            // If there are no Users on the Map, update the inactive counter or skip updating if already inactive
            if (_users.Count == 0)
            {
                if (IsInactive)
                    return;

                _inactiveCounter -= deltaTime;
            }

            base.Update(deltaTime);

            SynchronizeDynamicEntities();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the map and all of the Entities on it.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                Debug.Fail("Map is already disposed.");
                return;
            }

            _disposed = true;

            // Dispose of all the disposable entities
            var disposableEntities = Entities.OfType<IDisposable>();
            foreach (IDisposable entity in disposableEntities)
            {
                _world.DisposeStack.Push(entity);
            }
        }

        #endregion
    }
}