using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Collections;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information about a single map instance and all of the Entities it contains.
    /// </summary>
    public class Map : MapBase<Wall>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<NPC> _npcs = new List<NPC>();
        readonly TSList<User> _users = new TSList<User>();
        readonly World _world;
        bool _disposed;

        /// <summary>
        /// Gets an IEnumerable of NPCs on the Map.
        /// </summary>
        public IEnumerable<NPC> NPCs
        {
            get { return _npcs; }
        }

        /// <summary>
        /// Gets the list of users on the Map.
        /// </summary>
        public IEnumerable<User> Users
        {
            get { return _users; }
        }

        /// <summary>
        /// Gets the DBController used by this Map.
        /// </summary>
        public DBController DBController { get { return World.DBController; } }

        /// <summary>
        /// Gets the World the Map belongs to.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        public Map(ushort mapIndex, World world) : base(mapIndex, world)
        {
            _world = world;
        }

        bool CharAdded(Entity entity)
        {
            Character character = entity as Character;
            if (character == null)
                return false;

            // If the character was already on a map, so remove them from the old map
            if (character.Map != null)
            {
                const string errmsg = "Character `{0}` [{1}] added to new map, but is already on a map!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, character, character.MapCharIndex);
                Debug.Fail(string.Format(errmsg, character, character.MapCharIndex));
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
                return true;
            }

            // Added character is a NPC
            NPC npc = character as NPC;
            if (npc != null)
            {
                Debug.Assert(!NPCs.Contains(npc), string.Format("NPCs list already contains `{0}`!", npc));
                _npcs.Add(npc);
                return true;
            }

            // Unknown added character type - not actually an error, but it is likely an oversight
            throw new Exception("Unknown Character type - not a NPC or User...?");
        }

        bool CharRemoved(Entity entity)
        {
            Character character = entity as Character;
            if (character == null)
                return false;

            User user;
            NPC npc;

            if ((user = character as User) != null)
                _users.Remove(user);
            else if ((npc = character as NPC) != null)
                _npcs.Remove(npc);

            return true;
        }

        /// <summary>
        /// Creates an ItemEntity on the map.
        /// </summary>
        /// <param name="template">ItemTemplate to create the item from.</param>
        /// <param name="pos">Position to create the item at.</param>
        /// <param name="amount">Amount of the item to create. Must be greater than 0.</param>
        /// <returns>Reference to the new ItemEntity created.</returns>
        public ItemEntity CreateItem(ItemTemplate template, Vector2 pos, byte amount)
        {
            // Check for a valid amount
            if (amount < 1)
            {
                const string errmsg = "Invalid item amount `{0}`! Amount must be > 0.";
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

            // Create the item, add it to the map, and return the reference
            ItemEntity item = new ItemEntity(template, pos, amount);
            AddEntity(item);
            return item;
        }

        protected override TeleportEntityBase CreateTeleportEntity(XmlReader r)
        {
            return new TeleportEntity(r);
        }

        protected override void EntityAdded(Entity entity)
        {
            base.EntityAdded(entity);

            // Create the entity for everyone on the map
            if (_users.Count > 0)
            {
                IDynamicEntity dynamicEntity = entity as IDynamicEntity;
                if (dynamicEntity != null)
                {
                    using (PacketWriter creationData = dynamicEntity.GetCreationData())
                    {
                        if (creationData != null)
                            Send(creationData);
                    }
                }
            }

            // Handle the different types of entities
            if (ItemAdded(entity) || CharAdded(entity))
            {
            }
        }

        protected override void EntityRemoved(Entity entity)
        {
            base.EntityRemoved(entity);

            // Handle the different types of entities
            if (ItemRemoved(entity) || CharRemoved(entity))
            {
            }

            // Destroy the entity for everyone on the map
            if (_users.Count > 0)
            {
                IDynamicEntity dynamicEntity = entity as IDynamicEntity;
                if (dynamicEntity != null)
                {
                    using (PacketWriter removalData = dynamicEntity.GetRemovalData())
                    {
                        if (removalData != null)
                            Send(removalData);
                    }
                }
            }
        }

        bool ItemAdded(Entity entity)
        {
            ItemEntity item = entity as ItemEntity;
            if (item == null)
                return false;

            // If the item was already on a map, so remove them from the old map
            if (item.Map != null)
            {
                const string errmsg = "Item `{0}` [{1}] added to new map, but is already on a map!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, item, item.MapItemIndex);
                Debug.Fail(string.Format(errmsg, item, item.MapItemIndex));
                item.Map.RemoveEntity(item);
            }

            // Set the new map
            ((IMapControlledEntity)item).SetMap(this);

            return true;
        }

        static bool ItemRemoved(Entity entity)
        {
            ItemEntity item = entity as ItemEntity;
            if (item == null)
                return false;

            ((IMapControlledEntity)item).SetMap(null);

            return true;
        }

        /// <summary>
        /// Send a message to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        public void Send(BitStream data)
        {
            if (data == null || data.Length < 1)
            {
                const string errmsg = "Attempted to send null or invalid data to the map.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            foreach (User user in Users)
            {
                if (user != null)
                    user.Send(data);
                else
                    Debug.Fail("Null user found in Map's Users list.");
            }
        }

        /// <summary>
        /// Send a packet to every user in the map. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        /// <param name="skipUser">User to skip sending to.</param>
        public void Send(BitStream data, User skipUser)
        {
            if (data == null || data.Length < 1)
            {
                const string errmsg = "Attempted to send null or invalid data to the map.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            foreach (User user in Users)
            {
                if (user != null)
                {
                    if (user != skipUser)
                        user.Send(data);
                }
                else
                    Debug.Fail("Null user found in Map's Users list.");
            }
        }

        /// <summary>
        /// Sends the data to the specified user of all existing content on the map
        /// </summary>
        /// <param name="user">User to send the map data to</param>
        void SendMapData(User user)
        {
            // Tell the user to change the map
            using (PacketWriter pw = ServerPacket.SetMap(Index))
            {
                user.Send(pw);
            }

            // Send characters
            foreach (Character character in Characters)
            {
                using (PacketWriter creationData = character.GetCreationData())
                {
                    if (creationData != null)
                        user.Send(creationData);
                }
            }

            // Send items
            foreach (ItemEntity item in Items)
            {
                using (PacketWriter creationData = item.GetCreationData())
                {
                    if (creationData != null)
                        user.Send(creationData);
                }
            }

            // Now that the user know about the map and every character on it, tell them which one is theirs
            using (PacketWriter pw = ServerPacket.SetUserChar(user.MapCharIndex))
            {
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

        public override void Update()
        {
            // Skip updating maps with no users
            if (_users.Count == 0)
                return;

            base.Update();
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