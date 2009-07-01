using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.IO;
using NetGore.Network;

// TODO: Once the NPCs have Inventory support, move all the inventory-related shit into Character

namespace DemoGame.Server
{
    /// <summary>
    /// A user-controlled character
    /// </summary>
    public class User : Character
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Connection information for the user
        /// </summary>
        readonly IIPSocket _conn;

        readonly UserEquipped _equipped;

        readonly UserInventory _inventory;

        readonly UserStats _stats;

        ushort _id;
        int _nextLevelExp = int.MaxValue;

        /// <summary>
        /// Lets us know if we have saved the user since they have been
        /// updated. Used to ensure saves aren't called back-to-back.
        /// </summary>
        bool _saved = false;

        /// <summary>
        /// Gets the socket connection info for the user
        /// </summary>
        public IIPSocket Conn
        {
            get { return _conn; }
        }

        public DBController DBController
        {
            get { return World.Server.DBController; }
        }

        /// <summary>
        /// Gets the equipped item handler for the User.
        /// </summary>
        public UserEquipped Equipped
        {
            get { return _equipped; }
        }

        /// <summary>
        /// Gets the unique User ID for this User
        /// </summary>
        public ushort Guid
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the Character's Inventory.
        /// </summary>
        public override Inventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets the CharacterStatsBase used for this Character's stats.
        /// </summary>
        public override CharacterStatsBase Stats
        {
            get { return _stats; }
        }

        [Obsolete("Do not use this empty constructor on the Server!")]
        public User()
        {
        }

        /// <summary>
        /// User constructor.
        /// </summary>
        /// <param name="conn">Connection to the user's client.</param>
        /// <param name="world">World the user belongs to.</param>
        /// <param name="name">User's name.</param>
        public User(IIPSocket conn, World world, string name) : base(world)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("User {0} logged in", name);

            // Set the connection information
            _conn = conn;
            _conn.Tag = this;

            // Set up the inventory and equipped items
            _inventory = new UserInventory(this);
            _equipped = new UserEquipped(this);

            // Create the stats and attach the stat listeners
            _stats = new UserStats(this);
            _stats.GetStat(StatType.Exp).OnChange += Exp_OnChange;
            _stats.GetStat(StatType.Level).OnChange += Level_OnChange;

            // Activate the user
            IsAlive = true;

            // Load the user
            Alliance = World.Server.AllianceManager["user"];
            Load(name, world);

            // Attach to some events
            OnKillCharacter += User_OnKillCharacter;
        }

        /// <summary>
        /// Adds a new User to the database. Assumes all parameters are valid.
        /// </summary>
        /// <param name="insertUserQuery">InsertUserQuery used to create the new User</param>
        /// <param name="name">Name of the new User</param>
        /// <param name="password">Password for the new User</param>
        /// <returns>True if the User was successfully created, else false</returns>
        public static bool AddNewUser(InsertUserQuery insertUserQuery, string name, string password)
        {
            // TODO: Have to add back in support for creating a new user
            throw new ArgumentException("Have to add back in support for new users");

            //ushort uniqueID = GetNextUniqueIndex(conn);

            insertUserQuery.Execute(null);

            // TODO: When a user is created, create an entry for them in the user_equipped table

            return true;
        }

        public bool DropInventoryItem(byte slot)
        {
            return Inventory.Drop(slot);
        }

        /// <summary>
        /// Attempts to equip an item from the User's inventory.
        /// </summary>
        /// <param name="inventorySlot">Index of the slot containing the item to equip.</param>
        /// <returns>True if the item was successfully equipped, else false.</returns>
        public bool Equip(byte inventorySlot)
        {
            // Get the item from the inventory
            ItemEntity item = _inventory[inventorySlot];

            // Do not try to equip null items
            if (item == null)
                return false;

            // If there is more than one of the item, split it first
            if (item.Amount > 1)
            {
                // Split the existing item into two parts
                item = item.Split(1);
                if (item == null)
                {
                    Debug.Fail("Failed to split item for some reason.");
                    return false;
                }
            }
            else
            {
                // We are using all (1) of the item, so remove it from the inventory
                Inventory.RemoveAt(inventorySlot);
            }

            // Try to equip the item
            bool successful = Equipped.Equip(item);

            // If failed to equip, give the item back to the User
            if (!successful)
            {
                ItemEntity remainder = GiveItem(item);
                if (remainder != null)
                {
                    Debug.Fail("What the hell just happened?");
                    DropItem(item);
                }
            }

            return successful;
        }

        void Exp_OnChange(IStat stat)
        {
            // If the current level is 0, we probably haven't even set the level yet
            if (Stats[StatType.Level] == 0)
                return;

            // Check if we have enough experience to move to the next level
            if (Stats[StatType.Exp] < _nextLevelExp)
                return;

            // Increase the level
            Stats[StatType.Level]++;

            // Notify users on the map of the level-up
            using (PacketWriter pw = ServerPacket.NotifyLevel(MapEntityIndex))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Gets the next free unique User index
        /// </summary>
        /// <param name="conn">Database connection to use to find the unique index</param>
        /// <returns>Next free unique index</returns>
        static ushort GetNextUniqueIndex(MySqlConnection conn)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT `unique_id` FROM `users` ORDER BY `unique_id` ASC";
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

        public override ItemEntity GiveItem(ItemEntity item)
        {
            if (item == null)
            {
                Debug.Fail("Item is null.");
                return null;
            }

            Debug.Assert(item.Amount != 0, "Invalid item amount.");

            // Add as much of the item to the inventory as we can
            byte startAmount = item.Amount;
            ItemEntity remainder = _inventory.Add(item);

            // Check how much was added
            byte amountAdded;
            if (remainder == null)
                amountAdded = startAmount;
            else
            {
                Debug.Assert(startAmount >= item.Amount, "Somehow the startAmount is less than the current amount of items.");
                Debug.Assert(startAmount - item.Amount >= 0, "Negative item amount given...?");
                amountAdded = (byte)(startAmount - item.Amount);
            }

            // If any was added, send the notification
            if (amountAdded > 0)
            {
                using (PacketWriter pw = ServerPacket.NotifyGetItem(item.Name, amountAdded))
                {
                    Send(pw);
                }
            }

            // Return the remainder
            return remainder;
        }

        /// <summary>
        /// Raises the user's exp and cash along with notifies them of the amounts.
        /// </summary>
        /// <param name="exp">Amount of exp to give.</param>
        /// <param name="cash">Amount of cash to give.</param>
        void GiveKillReward(int exp, int cash)
        {
            using (PacketWriter pw = ServerPacket.NotifyExpCash(exp, cash))
            {
                Send(pw);
            }

            Stats[StatType.Exp] += exp;
            Stats[StatType.Cash] += cash;
        }

        protected override void HandleDispose()
        {
            // Make sure the user was saved
            Save();

            // Close the user's connection
            if (_conn != null)
                _conn.Dispose();

            base.HandleDispose();
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

            Teleport(new Vector2(100, 100));
            Stats[StatType.HP] = Stats[StatType.MaxHP];
            Stats[StatType.MP] = Stats[StatType.MaxMP];
        }

        void Level_OnChange(IStat stat)
        {
            _nextLevelExp = GameData.LevelCost(Stats[StatType.Level]);
        }

        /// <summary>
        /// Loads the user information and returns the index of the map they are on
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="world">World that the user is a part of</param>
        /// <returns>Index of the map the user is on</returns>
        void Load(string userName, World world)
        {
            Name = userName;

            // Get the user information from the database
            SelectUserQueryValues userValues = DBController.SelectUser.Execute(userName, Stats);
            _id = userValues.Guid;

            // Use the retreived values
            BodyInfo = GameData.Body(userValues.BodyIndex);
            CB = new CollisionBox(userValues.Position, BodyInfo.Width, BodyInfo.Height);

            // Create the user in the world
            Map m = world.GetMap(userValues.MapIndex);
            if (m == null)
                throw new Exception("Unable to get Map with index " + userValues.MapIndex);
            world.AddUser(this);
            ChangeMap(m);

            // Load the User's items
            _inventory.Load();
            _equipped.Load();

            // Mark the User as loaded
            SetAsLoaded();
        }

        /// <summary>
        /// Makes the User raise their Stat of the corresponding type by one point, assuming they have enough
        /// points available to raise the Stat, and lowers the amount of spendable points accordingly.
        /// </summary>
        /// <param name="st">StatType of the stat to raise.</param>
        public void RaiseStat(StatType st)
        {
            int cost = GameData.StatCost(Stats[st]);

            if (Stats.Points <= cost)
            {
                const string errmsg = "User `{0}` tried to raise stat `{1}`, but only has {2} of {3} points needed.";
                Debug.Fail(string.Format(errmsg, this, st, Stats.Points, cost));
                return;
            }

            Stats[st]++;
            Stats[StatType.ExpSpent] += cost;
        }

        /// <summary>
        /// Saves the user information
        /// </summary>
        public void Save()
        {
            // Do not save if the user is already saved
            if (_saved)
                return;

            // Set the user as saved
            _saved = true;

            // Execute the user save query
            DBController.UpdateUser.Execute(this);

            if (log.IsInfoEnabled)
                log.InfoFormat("Saved user `{0}`", this);
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
            if (data == null)
            {
                Debug.Fail("data is null.");
                return;
            }

            if (_conn != null && _conn.IsConnected)
                _conn.Send(data, reliable);
            else
                DelayedDispose();
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
            ItemEntity item = _equipped[slot];
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
        public void SendInventoryItemStats(byte slot)
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
            ItemEntity item = _inventory[slot];
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

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        public override void Update(IMap imap, float deltaTime)
        {
            // Perform the general character updating
            base.Update(imap, deltaTime);

            // Update the user's stats
            UpdateStats();

            // Synchronize the inventory
            _inventory.UpdateClient();

            // Set the user as not saved
            _saved = false;
        }

        /// <summary>
        /// Updates the user's stats
        /// </summary>
        void UpdateStats()
        {
            foreach (IUpdateableStat stat in _stats.UpdateableStats)
            {
                stat.Update();
            }
        }

        public void UseInventoryItem(byte slot)
        {
            // Get the item to use
            ItemEntity item = _inventory[slot];
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
                    _inventory.RemoveAt(slot);
                    item.Dispose();
                }
            }

            // Lower the count of use-once items
            // NOTE: Once I hook to the item's amount in the inventory to listen for changes, I can put this in UseItem()
            if (item.Type == ItemType.UseOnce)
                _inventory.DecreaseItemAmount(slot);
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