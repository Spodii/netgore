using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// A user-controlled character.
    /// </summary>
    public class User : Character
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            base.HandleAdditionalLoading(v);

            _password = v.Password;
            World.AddUser(this);
        }

        /// <summary>
        /// Not used by User.
        /// </summary>
        public override NetGore.NPCChat.NPCChatDialogBase ChatDialog
        {
            get { return null; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the Character's password.
        /// </summary>
        public override string Password
        {
            get { return _password; }
        }

        /// <summary>
        /// The socket used to communicate with the User.
        /// </summary>
        readonly IIPSocket _conn;

        readonly SocketSendQueue _unreliableBuffer;
        readonly UserInventory _userInventory;
        readonly UserStats _userStatsBase;
        readonly UserStats _userStatsMod;
        string _password;

        /// <summary>
        /// Gets the socket connection info for the user
        /// </summary>
        public IIPSocket Conn
        {
            get { return _conn; }
        }

        /// <summary>
        /// User constructor.
        /// </summary>
        /// <param name="conn">Connection to the user's client.</param>
        /// <param name="world">World the user belongs to.</param>
        /// <param name="name">User's name.</param>
        public User(IIPSocket conn, World world, string name) : base(world, true)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("User {0} logged in", name);

            // Set the connection information
            _conn = conn;
            _conn.Tag = this;

            _userStatsBase = (UserStats)BaseStats;
            _userStatsMod = (UserStats)ModStats;

            // Create some objects
            _unreliableBuffer = new SocketSendQueue(conn.MaxUnreliableMessageSize);

            // Load the character data
            Load(name);

            // Ensure the correct Alliance is being used
            Alliance = AllianceManager.GetAlliance("user");

            // Attach to some events
            OnKillCharacter += User_OnKillCharacter;
            OnChangeStatPoints += User_OnChangeStatPoints;
            OnChangeExp += User_OnChangeExp;
            OnChangeCash += User_OnChangeCash;
            OnChangeLevel += User_OnChangeLevel;

            _userInventory = (UserInventory)Inventory;

            // Activate the user
            IsAlive = true;

            // Send the initial information
            User_OnChangeLevel(this, Level, Level);
            User_OnChangeCash(this, Cash, Cash);
            User_OnChangeExp(this, Exp, Exp);
            User_OnChangeStatPoints(this, StatPoints, StatPoints);
        }

        protected override void AfterGiveItem(ItemEntity item, byte amount)
        {
            // If any was added, send the notification
            using (PacketWriter pw = ServerPacket.NotifyGetItem(item.Name, amount))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, checks if enough time has elapesd since the Character died
        /// for them to be able to respawn.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        /// <returns>True if enough time has elapsed; otherwise false.</returns>
        protected override bool CheckRespawnElapsedTime(int currentTime)
        {
            // Users don't need to wait for nuttin'!
            return true;
        }

        protected override CharacterEquipped CreateEquipped()
        {
            return new UserEquipped(this);
        }

        protected override CharacterInventory CreateInventory()
        {
            return new UserInventory(this);
        }

        protected override CharacterSPSynchronizer CreateSPSynchronizer()
        {
            return new UserSPSynchronizer(this);
        }

        protected override CharacterStatsBase CreateStats(StatCollectionType statCollectionType)
        {
            return new UserStats(this, statCollectionType);
        }

        /// <summary>
        /// Sends all the data buffered for the unreliable channel by SendUnreliableBuffered() to the User.
        /// </summary>
        public void FlushUnreliableBuffer()
        {
            if (IsDisposed)
                return;

            if (_conn == null || !_conn.IsConnected)
            {
                const string errmsg =
                    "Send to `{0}` failed - Conn is null or not connected." +
                    " Connection by client was probably not closed properly. Usually not a big deal. Disposing User...";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                DelayedDispose();

                return;
            }

            byte[] data;
            while ((data = _unreliableBuffer.Dequeue()) != null)
            {
                _conn.Send(data, false);
            }
        }

        /// <summary>
        /// Gets the next free unique User index
        /// </summary>
        /// <param name="conn">Database connection to use to find the unique index</param>
        /// <returns>Next free unique index</returns>
        static ushort GetNextUniqueIndex(MySqlConnection conn)
        {
            // TODO: This is worthless. Implement a proper ID stack using IDCreatorBase.

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT `unique_id` FROM `{0}` ORDER BY `unique_id` ASC", CharacterTable.TableName);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    // No Users, so return the first index
                    if (!r.HasRows)
                        return 1;

                    // Loop until we find a gap
                    ushort lastValue = 1;
                    while (r.Read())
                    {
                        ushort currValue = r.GetUInt16(0);
                        if (currValue - 1 > lastValue)
                            return (ushort)(lastValue + 1);
                        lastValue = currValue;
                    }

                    // No gap found, return one above the last highest index
                    return (ushort)(lastValue + 1);
                }
            }
        }

        protected override void GiveKillReward(uint exp, uint cash)
        {
            base.GiveKillReward(exp, cash);

            using (PacketWriter pw = ServerPacket.NotifyExpCash(exp, cash))
            {
                Send(pw);
            }
        }

        protected override void HandleDispose()
        {
            base.HandleDispose();

            // Close the User's connection
            if (_conn != null)
                _conn.Dispose();
        }

        /// <summary>
        /// Checks if a user account exists and the supplied password is valid.
        /// </summary>
        /// <param name="query">Query used to select the user's password.</param>
        /// <param name="name">Name of the user.</param>
        /// <param name="password">Password for the user's account.</param>
        /// <returns>True if the account exists.</returns>
        public static bool IsValidAccount(SelectUserPasswordQuery query, string name, string password)
        {
            // Check that the password matches the database password
            string dbPassword = query.Execute(name);
            return dbPassword == password;
        }

        /// <summary>
        /// Kills the user
        /// </summary>
        public override void Kill()
        {
            base.Kill();

            // TODO: Respawn the user to the correct respawn location
            Teleport(new Vector2(100, 100));

            UpdateModStats();

            HP = ModStats[StatType.MaxHP];
            MP = ModStats[StatType.MaxMP];
        }

        protected override void LevelUp()
        {
            base.LevelUp();

            // Notify users on the map of the level-up
            using (PacketWriter pw = ServerPacket.NotifyLevel(MapEntityIndex))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends data to the User. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        public void Send(BitStream data)
        {
            Send(data, true);
        }

        /// <summary>
        /// Sends data to the User. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        /// <param name="reliable">Whether or not the data should be sent over a reliable stream.</param>
        public void Send(BitStream data, bool reliable)
        {
            if (IsDisposed)
            {
                const string errmsg = "Tried to send data to disposed User `{0}` [reliable = `{1}`]";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this, reliable);
                return;
            }

            if (_conn == null || !_conn.IsConnected)
            {
                const string errmsg = "Send to `{0}` failed - Conn is null or not connected. Disposing User...";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                DelayedDispose();

                return;
            }

            _conn.Send(data, reliable);
        }

        /// <summary>
        /// Sends data to the User.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        public void Send(GameMessage message)
        {
            using (PacketWriter pw = ServerPacket.SendMessage(message))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends data to the User.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="parameters">Message parameters.</param>
        public void Send(GameMessage message, params object[] parameters)
        {
            using (PacketWriter pw = ServerPacket.SendMessage(message, parameters))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends the item information for an item in a given equipment slot to the client.
        /// </summary>
        /// <param name="slot">Equipment slot of the item to send the info for</param>
        public void SendEquipmentItemStats(EquipmentSlot slot)
        {
            // Check for a valid slot
            if (!slot.IsDefined())
            {
                const string errmsg = "User `{0}` attempted to access invalid equipment slot `{1}`.";
                Debug.Fail(string.Format(errmsg, this, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, slot);
                return;
            }

            // Get the item
            ItemEntity item = Equipped[slot];
            if (item == null)
            {
                const string errmsg = "User `{0}` requested info for equipment slot `{1}`, but the slot has no item.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, slot);
                return;
            }

            // Send the new item info
            using (PacketWriter pw = ServerPacket.SendItemInfo(item))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends the item information for an item in a given inventory slot to the client.
        /// </summary>
        /// <param name="slot">Inventory slot of the item to send the info for</param>
        public void SendInventoryItemStats(InventorySlot slot)
        {
            // Check for a valid slot
            if (slot >= InventoryBase.MaxInventorySize)
            {
                const string errmsg = "User `{0}` attempted to access invalid inventory slot `{1}`.";
                Debug.Fail(string.Format(errmsg, this, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, slot);
                return;
            }

            // Get the item
            ItemEntity item = Inventory[slot];
            if (item == null)
            {
                const string errmsg = "User `{0}` requested info for inventory slot `{1}`, but the slot has no item.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, slot);
                return;
            }

            // Send the new item info
            using (PacketWriter pw = ServerPacket.SendItemInfo(item))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends data to the User. The data is actually buffered indefinately until FlushUnreliableBuffer() is
        /// called. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        public void SendUnreliableBuffered(BitStream data)
        {
            _unreliableBuffer.Enqueue(data);
        }

        protected override void StatusEffects_HandleOnAdd(CharacterStatusEffects effects, ActiveStatusEffect ase)
        {
            base.StatusEffects_HandleOnAdd(effects, ase);

            int currentTime = GetTime();
            int timeLeft = ase.GetTimeRemaining(currentTime);

            using (PacketWriter pw = ServerPacket.AddStatusEffect(ase.StatusEffect.StatusEffectType, ase.Power, timeLeft))
            {
                Send(pw);
            }
        }

        protected override void StatusEffects_HandleOnRemove(CharacterStatusEffects effects, ActiveStatusEffect ase)
        {
            base.StatusEffects_HandleOnRemove(effects, ase);

            using (PacketWriter pw = ServerPacket.RemoveStatusEffect(ase.StatusEffect.StatusEffectType))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Perform the general character updating
            base.Update(imap, deltaTime);

            // Synchronize the User's stats
            _userStatsBase.UpdateClient();
            _userStatsMod.UpdateClient();

            // Synchronize the Inventory
            _userInventory.UpdateClient();
        }

        public void UseInventoryItem(InventorySlot slot)
        {
            // Get the item to use
            ItemEntity item = Inventory[slot];
            if (item == null)
            {
                const string errmsg = "Tried to use inventory slot `{0}`, but it contains no item.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return;
            }

            // Try to use the item
            if (!UseItem(item, slot))
            {
                // Check if the failure to use the item was due to an invalid amount of the item
                if (item.Amount <= 0)
                {
                    const string errmsg = "Tried to use inventory item `{0}` at slot `{1}`, but it had an invalid amount.";
                    Debug.Fail(string.Format(errmsg, item, slot));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, item, slot);

                    // Destroy the item
                    Inventory.RemoveAt(slot);
                    item.Dispose();
                }
            }

            // Lower the count of use-once items
            if (item.Type == ItemType.UseOnce)
                Inventory.DecreaseItemAmount(slot);
        }

        void User_OnChangeCash(Character character, uint oldCash, uint cash)
        {
            using (PacketWriter pw = ServerPacket.SetCash(cash))
            {
                Send(pw);
            }
        }

        void User_OnChangeExp(Character character, uint oldExp, uint exp)
        {
            using (PacketWriter pw = ServerPacket.SetExp(exp))
            {
                Send(pw);
            }
        }

        void User_OnChangeLevel(Character character, byte oldLevel, byte level)
        {
            using (PacketWriter pw = ServerPacket.SetLevel(level))
            {
                Send(pw);
            }
        }

        void User_OnChangeStatPoints(Character character, uint oldValue, uint newValue)
        {
            using (PacketWriter pw = ServerPacket.SetStatPoints(newValue))
            {
                Send(pw);
            }
        }

        void User_OnKillCharacter(Character killed, Character killer)
        {
            Debug.Assert(killer == this);
            Debug.Assert(killed != null);

            NPC killedNPC = killed as NPC;

            // Handle killing a NPC
            if (killedNPC != null)
                GiveKillReward(killedNPC.GiveExp, killedNPC.GiveCash);
        }
    }
}