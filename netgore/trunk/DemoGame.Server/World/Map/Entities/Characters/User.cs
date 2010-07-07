using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Groups;
using DemoGame.Server.Guilds;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.Db;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.IO;
using NetGore.Network;
using NetGore.NPCChat;
using NetGore.Stats;
using SFML.Graphics;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// A user-controlled character.
    /// </summary>
    public class User : Character, IGuildMember, IClientCommunicator, IGroupable, IQuestPerformer<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly DeleteCharacterQuestStatusKillsQuery _deleteCharacterQuestStatusKillsQuery;
        static readonly DeleteCharacterQuestStatusQuery _deleteCharacterQuestStatusQuery;
        static readonly DeleteGuildMemberQuery _deleteGuildMemberQuery;
        static readonly GuildManager _guildManager = GuildManager.Instance;
        static readonly InsertCharacterQuestStatusStartQuery _insertCharacterQuestStatusStartQuery;
        static readonly ReplaceGuildMemberQuery _replaceGuildMemberQuery;
        static readonly SelectGuildMemberQuery _selectGuildMemberQuery;
        static readonly UpdateCharacterQuestStatusFinishedQuery _updateCharacterQuestStatusFinishedQuery;

        readonly UserChatDialogState _chatState;
        readonly IIPSocket _conn;
        readonly GroupMemberInfo _groupMemberInfo;
        readonly GuildMemberInfo _guildMemberInfo;
        readonly QuestPerformerStatusHelper _questInfo;
        readonly UserShoppingState _shoppingState;
        readonly SocketSendQueue _unreliableBuffer;
        readonly UserInventory _userInventory;
        readonly UserStats _userStatsBase;
        readonly UserStats _userStatsMod;

        /// <summary>
        /// Initializes the <see cref="User"/> class.
        /// </summary>
        static User()
        {
            var dbController = DbControllerBase.GetInstance();
            _deleteGuildMemberQuery = dbController.GetQuery<DeleteGuildMemberQuery>();
            _replaceGuildMemberQuery = dbController.GetQuery<ReplaceGuildMemberQuery>();
            _selectGuildMemberQuery = dbController.GetQuery<SelectGuildMemberQuery>();

            _insertCharacterQuestStatusStartQuery = dbController.GetQuery<InsertCharacterQuestStatusStartQuery>();
            _deleteCharacterQuestStatusQuery = dbController.GetQuery<DeleteCharacterQuestStatusQuery>();
            _deleteCharacterQuestStatusKillsQuery = dbController.GetQuery<DeleteCharacterQuestStatusKillsQuery>();
            _updateCharacterQuestStatusFinishedQuery = dbController.GetQuery<UpdateCharacterQuestStatusFinishedQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="conn">Connection to the user's client.</param>
        /// <param name="world">World the user belongs to.</param>
        /// <param name="characterID">User's CharacterID.</param>
        public User(IIPSocket conn, World world, CharacterID characterID) : base(world, true)
        {
            // Set the connection information
            _conn = conn;

            // Create some objects
            _guildMemberInfo = new GuildMemberInfo(this);
            _groupMemberInfo = new GroupMemberInfo(this);
            _shoppingState = new UserShoppingState(this);
            _chatState = new UserChatDialogState(this);
            _questInfo = CreateQuestInfo();
            _userStatsBase = (UserStats)BaseStats;
            _userStatsMod = (UserStats)ModStats;
            _unreliableBuffer = new SocketSendQueue(conn.MaxUnreliableMessageSize);

            // Load the character data
            Load(characterID);

            // Ensure the correct Alliance is being used
            Alliance = AllianceManager[new AllianceID(0)];

            // Activate the user
            _userInventory = (UserInventory)Inventory;
            IsAlive = true;

            // Send the initial information
            using (var pw = ServerPacket.SetLevel(Level))
            {
                Send(pw);
            }
            using (var pw = ServerPacket.SetCash(Cash))
            {
                Send(pw);
            }
            using (var pw = ServerPacket.SetExp(Exp))
            {
                Send(pw);
            }
            using (var pw = ServerPacket.SetStatPoints(StatPoints))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the Character's AI. Can be null if they have no AI.
        /// </summary>
        public override IAI AI
        {
            get { return null; }
        }

        /// <summary>
        /// Not used by User.
        /// </summary>
        public override NPCChatDialogBase ChatDialog
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the UserChatDialogState for this User.
        /// </summary>
        public UserChatDialogState ChatState
        {
            get { return _chatState; }
        }

        /// <summary>
        /// Gets the socket connection info for the user
        /// </summary>
        public IIPSocket Conn
        {
            get { return _conn; }
        }

        /// <summary>
        /// Gets the <see cref="User"/>'s quest information.
        /// </summary>
        public QuestPerformerStatusHelper QuestInfo
        {
            get { return _questInfo; }
        }

        /// <summary>
        /// Always returns null.
        /// </summary>
        public override IShop<ShopItem> Shop
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the <see cref="User"/>'s shopping state.
        /// </summary>
        public UserShoppingState ShoppingState
        {
            get { return _shoppingState; }
        }

        /// <summary>
        /// When overridden in the derived class, lets the Character handle being given items through GiveItem().
        /// </summary>
        /// <param name="item">The <see cref="ItemEntity"/> the Character was given.</param>
        /// <param name="amount">The amount of the <paramref name="item"/> the Character was given. Will be greater
        /// than 0.</param>
        protected override void AfterGiveItem(ItemEntity item, byte amount)
        {
            // If any was added, send the notification
            using (var pw = ServerPacket.NotifyGetItem(item.Name, amount))
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
        protected override bool CheckRespawnElapsedTime(TickCount currentTime)
        {
            // Users don't need to wait for nuttin'!
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterEquipped for this Character.
        /// </summary>
        /// <returns>
        /// The CharacterEquipped for this Character.
        /// </returns>
        protected override CharacterEquipped CreateEquipped()
        {
            return new UserEquipped(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterInventory for this Character.
        /// </summary>
        /// <returns>
        /// The CharacterInventory for this Character.
        /// </returns>
        protected override CharacterInventory CreateInventory()
        {
            return new UserInventory(this);
        }

        /// <summary>
        /// Creates the <see cref="QuestPerformerStatusHelper"/> for this user.
        /// </summary>
        /// <returns>The <see cref="QuestPerformerStatusHelper"/> for this user.</returns>
        QuestPerformerStatusHelper CreateQuestInfo()
        {
            var ret = new QuestPerformerStatusHelper(this);

            ret.QuestAccepted += delegate(QuestPerformerStatusHelper<User> sender, IQuest<User> quest)
            {
                OnQuestAccepted(quest);
                if (QuestAccepted != null)
                    QuestAccepted(this, quest);
            };

            ret.QuestCanceled += delegate(QuestPerformerStatusHelper<User> sender, IQuest<User> quest)
            {
                OnQuestCanceled(quest);
                if (QuestCanceled != null)
                    QuestCanceled(this, quest);
            };

            ret.QuestFinished += delegate(QuestPerformerStatusHelper<User> sender, IQuest<User> quest)
            {
                OnQuestFinished(quest);
                if (QuestFinished != null)
                    QuestFinished(this, quest);
            };

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterSPSynchronizer for this Character.
        /// </summary>
        /// <returns>
        /// The CharacterSPSynchronizer for this Character.
        /// </returns>
        protected override CharacterSPSynchronizer CreateSPSynchronizer()
        {
            return new UserSPSynchronizer(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates the CharacterStatsBase for this Character.
        /// </summary>
        /// <param name="statCollectionType">The type of <see cref="StatCollectionType"/> to create.</param>
        /// <returns>
        /// The CharacterStatsBase for this Character.
        /// </returns>
        protected override StatCollection<StatType> CreateStats(StatCollectionType statCollectionType)
        {
            return new UserStats(statCollectionType);
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
        /// When overridden in the derived class, gets the <see cref="MapID"/> that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The ID of the map to load this <see cref="Character"/> on.</returns>
        protected override MapID GetLoadMap()
        {
            if (!RespawnMapID.HasValue)
            {
                const string errmsg =
                    "Users are expected to have a valid respawn position at all times, but user `{0}` does not. Using ServerSettings.InvalidUserLoad... instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerSettings.InvalidUserLoadMap;
            }

            return RespawnMapID.Value;
        }

        /// <summary>
        /// When overridden in the derived class, gets the position that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The position to load this <see cref="Character"/> at.</returns>
        protected override Vector2 GetLoadPosition()
        {
            if (!RespawnMapID.HasValue)
            {
                const string errmsg =
                    "Users are expected to have a valid respawn position at all times, but user `{0}` does not. Using ServerSettings.InvalidUserLoad... instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerSettings.InvalidUserLoadPosition;
            }

            return RespawnPosition;
        }

        /// <summary>
        /// Gives the kill reward.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="cash">The cash.</param>
        protected override void GiveKillReward(int exp, int cash)
        {
            base.GiveKillReward(exp, cash);

            using (var pw = ServerPacket.NotifyExpCash(exp, cash))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            base.HandleAdditionalLoading(v);

            // Permissions
            Permissions = v.Permissions;

            // Load the guild information
            var guildInfo = _selectGuildMemberQuery.Execute(ID);
            if (guildInfo != null)
            {
                Debug.Assert(guildInfo.CharacterID == ID);
                _guildMemberInfo.Guild = _guildManager.GetGuild(guildInfo.GuildID);
                _guildMemberInfo.GuildRank = guildInfo.Rank;
            }

            World.AddUser(this);
        }

        /// <summary>
        /// Performs the actual disposing of the Entity. This is called by the base Entity class when
        /// a request has been made to dispose of the Entity. This is guarenteed to only be called once.
        /// All classes that override this method should be sure to call base.DisposeHandler() after
        /// handling what it needs to dispose.
        /// </summary>
        protected override void HandleDispose()
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Disposing User `{0}`.", this);

            base.HandleDispose();

            // Remove the User from being the active User in the account
            var account = World.GetUserAccount(Conn);
            if (account != null)
                account.CloseUser();

            _groupMemberInfo.HandleDisposed();
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional steps to be taken when saving.
        /// </summary>
        protected override void HandleSave()
        {
            base.HandleSave();

            ((IGuildMember)this).SaveGuildInformation();
        }

        /// <summary>
        /// Handles updating this <see cref="Entity"/>.
        /// </summary>
        /// <param name="imap">The map the <see cref="Entity"/> is on.</param>
        /// <param name="deltaTime">The amount of time (in milliseconds) that has elapsed since the last update.</param>
        protected override void HandleUpdate(IMap imap, int deltaTime)
        {
            // Don't allow movement while chatting
            if (!GameData.AllowMovementWhileChattingToNPC)
            {
                if (ChatState.IsChatting && Velocity != Vector2.Zero)
                    SetVelocity(Vector2.Zero);
            }

            // Perform the general character updating
            base.HandleUpdate(imap, deltaTime);
        }

        /// <summary>
        /// Kills the user
        /// </summary>
        public override void Kill()
        {
            base.Kill();

            if (!RespawnMapID.HasValue)
            {
                // TODO: Have a global User respawn map/position that can be defined, then use that here
                return;
            }

            var spawnMap = World.GetMap(RespawnMapID.Value);
            Teleport(spawnMap, RespawnPosition);

            UpdateModStats();

            HP = (int)ModStats[StatType.MaxHP];
            MP = (int)ModStats[StatType.MaxMP];

            IsAlive = true;
        }

        /// <summary>
        /// Makes the Character's level increase. Does not alter the experience in any way since it is assume that,
        /// when this is called, the Character already has enough experience for the next level.
        /// </summary>
        protected override void LevelUp()
        {
            base.LevelUp();

            WorldStatsTracker.Instance.AddUserLevel(this);

            // Notify users on the map of the level-up
            using (var pw = ServerPacket.NotifyLevel(MapEntityIndex))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.CashChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldCash">The old cash.</param>
        /// <param name="cash">The cash.</param>
        protected override void OnCashChanged(int oldCash, int cash)
        {
            base.OnCashChanged(oldCash, cash);

            using (var pw = ServerPacket.SetCash(cash))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.ExpChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldExp">The old exp.</param>
        /// <param name="exp">The exp.</param>
        protected override void OnExpChanged(int oldExp, int exp)
        {
            base.OnExpChanged(oldExp, exp);

            using (var pw = ServerPacket.SetExp(exp))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="killed">The <see cref="Character"/> that this <see cref="Character"/> killed.</param>
        protected override void OnKilledCharacter(Character killed)
        {
            base.OnKilledCharacter(killed);

            Debug.Assert(killed != null);

            var killedNPC = killed as NPC;

            // Handle killing a NPC
            if (killedNPC != null)
            {
                int giveExp = killedNPC.GiveExp;
                int giveCash = killedNPC.GiveCash;

                WorldStatsTracker.Instance.AddUserKillNPC(this, killedNPC);

                // If in a group, split among the group members (as needed)
                var group = ((IGroupable)this).Group;
                if (group == null || group.ShareMode == GroupShareMode.NoSharing)
                {
                    // Not in a group or no sharing is being used in the group, give all to self
                    GiveKillReward(giveExp, giveCash);
                }
                else
                {
                    // Share equally among the group members
                    var members = group.GetGroupMembersInShareRange(this, true).OfType<User>().ToArray();
                    giveExp = (giveExp / members.Length) + 1;
                    giveCash = (giveCash / members.Length) + 1;
                    foreach (var member in members)
                    {
                        member.GiveKillReward(giveExp, giveCash);
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.LevelChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnLevelChanged(byte oldValue, byte newValue)
        {
            base.OnLevelChanged(oldValue, newValue);

            using (var pw = ServerPacket.SetLevel(newValue))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Allows for handling of the <see cref="User.QuestAccepted"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was accepted.</param>
        protected virtual void OnQuestAccepted(IQuest<User> quest)
        {
            _insertCharacterQuestStatusStartQuery.Execute(ID, quest.QuestID);
        }

        /// <summary>
        /// Allows for handling of the <see cref="User.QuestCanceled"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was accepted.</param>
        protected virtual void OnQuestCanceled(IQuest<User> quest)
        {
            _deleteCharacterQuestStatusQuery.Execute(ID, quest.QuestID);
            _deleteCharacterQuestStatusKillsQuery.Execute(ID, quest.QuestID);
        }

        /// <summary>
        /// Allows for handling of the <see cref="User.QuestFinished"/> event without creating an event hook.
        /// </summary>
        /// <param name="quest">The quest that was accepted.</param>
        protected virtual void OnQuestFinished(IQuest<User> quest)
        {
            _updateCharacterQuestStatusFinishedQuery.Execute(ID, quest.QuestID);
            _deleteCharacterQuestStatusKillsQuery.Execute(ID, quest.QuestID);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.StatPointsChanged"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnStatPointsChanged(int oldValue, int newValue)
        {
            base.OnStatPointsChanged(oldValue, newValue);

            using (var pw = ServerPacket.SetStatPoints(newValue))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.KilledByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="item">The item that was used.</param>
        protected override void OnUsedItem(ItemEntity item)
        {
            base.OnUsedItem(item);

            if (item.Type == ItemType.UseOnce)
                WorldStatsTracker.Instance.AddUserConsumeItem(this, item);
        }

        /// <summary>
        /// Not supported by the User.
        /// </summary>
        public override void RemoveAI()
        {
        }

        /// <summary>
        /// Sends the item information for an item in a given equipment slot to the client.
        /// </summary>
        /// <param name="slot">Equipment slot of the ItemEntity to send the info for.</param>
        public void SendEquipmentItemStats(EquipmentSlot slot)
        {
            // Check for a valid slot
            if (!EnumHelper<EquipmentSlot>.IsDefined(slot))
            {
                const string errmsg = "User `{0}` attempted to access invalid equipment slot `{1}`.";
                Debug.Fail(string.Format(errmsg, this, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, slot);
                return;
            }

            // Get the item
            var item = Equipped[slot];
            if (item == null)
            {
                const string errmsg = "User `{0}` requested info for equipment slot `{1}`, but the slot has no ItemEntity.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, slot);
                return;
            }

            // Send the item info
            using (var pw = ServerPacket.SendEquipmentItemInfo(slot, item))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Sends the item information for an item in a given inventory slot to the client.
        /// </summary>
        /// <param name="slot">Inventory slot of the ItemEntity to send the info for.</param>
        public void SendInventoryItemStats(InventorySlot slot)
        {
            // Check for a valid slot
            if (!slot.IsLegalValue())
            {
                const string errmsg = "User `{0}` attempted to access invalid inventory slot `{1}`.";
                Debug.Fail(string.Format(errmsg, this, slot));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, slot);
                return;
            }

            // Get the item
            var item = Inventory[slot];
            if (item == null)
            {
                const string errmsg = "User `{0}` requested info for inventory slot `{1}`, but the slot has no ItemEntity.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, this, slot);
                return;
            }

            // Send the item info
            using (var pw = ServerPacket.SendInventoryItemInfo(slot, item))
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

        /// <summary>
        /// Not supported by the User.
        /// </summary>
        /// <param name="aiID">Unused.</param>
        /// <returns>False.</returns>
        public override bool SetAI(AIID aiID)
        {
            return false;
        }

        /// <summary>
        /// Not supported by the User.
        /// </summary>
        /// <param name="aiName">Unused.</param>
        /// <returns>False.</returns>
        public override bool SetAI(string aiName)
        {
            return false;
        }

        /// <summary>
        /// Handles when an <see cref="ActiveStatusEffect"/> is added to this Character's StatusEffects.
        /// </summary>
        /// <param name="effects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="ase">The <see cref="ActiveStatusEffect"/> that was added.</param>
        protected override void StatusEffects_HandleOnAdd(CharacterStatusEffects effects, ActiveStatusEffect ase)
        {
            base.StatusEffects_HandleOnAdd(effects, ase);

            var currentTime = GetTime();
            var timeLeft = ase.GetTimeRemaining(currentTime);

            using (var pw = ServerPacket.AddStatusEffect(ase.StatusEffect.StatusEffectType, ase.Power, timeLeft))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Handles when an <see cref="ActiveStatusEffect"/> is removed from this Character's StatusEffects.
        /// </summary>
        /// <param name="effects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="ase">The <see cref="ActiveStatusEffect"/> that was removed.</param>
        protected override void StatusEffects_HandleOnRemove(CharacterStatusEffects effects, ActiveStatusEffect ase)
        {
            base.StatusEffects_HandleOnRemove(effects, ase);

            using (var pw = ServerPacket.RemoveStatusEffect(ase.StatusEffect.StatusEffectType))
            {
                Send(pw);
            }
        }

        /// <summary>
        /// Synchronizes the <see cref="User"/> information with the client that the <see cref="User"/> belongs to for
        /// information that is not handled by the <see cref="SyncValueAttribute"/>. This includes information such
        /// as stats and inventory.
        /// </summary>
        public void SynchronizeExtraUserInformation()
        {
            // Stats
            _userStatsBase.UpdateClient(this);
            _userStatsMod.UpdateClient(this);

            // Inventory
            _userInventory.UpdateClient();
        }

        public bool TryBuyItem(IItemTemplateTable itemTemplate, byte amount)
        {
            if (itemTemplate == null || amount <= 0)
                return false;

            var totalCost = itemTemplate.Value * amount;

            // Check for enough money to buy
            if (Cash < totalCost)
            {
                if (amount == 1)
                {
                    using (
                        var pw = ServerPacket.SendMessage(GameMessage.ShopInsufficientFundsToPurchaseSingular, itemTemplate.Name))
                    {
                        Send(pw);
                    }
                }
                else
                {
                    using (
                        var pw = ServerPacket.SendMessage(GameMessage.ShopInsufficientFundsToPurchasePlural, amount,
                                                          itemTemplate.Name))
                    {
                        Send(pw);
                    }
                }
                return false;
            }

            // Create the item
            var itemEntity = new ItemEntity(itemTemplate, amount);

            // Check for room in the inventory
            if (!Inventory.CanAdd(itemEntity))
            {
                itemEntity.Dispose();
                return false;
            }

            // Add to the inventory
            var remainderItem = Inventory.Add(itemEntity);

            // Find the number of remaining items (in case something went wrong and not all was added)
            var remainderAmount = 0;
            if (remainderItem != null)
            {
                remainderAmount = remainderItem.Amount;
                remainderItem.Dispose();
            }

            // Find the difference in the requested amount and remaining amount to get the amount added, and
            // only charge the character for that (so they pay for what they got)
            var amountPurchased = amount - remainderAmount;

            if (amountPurchased < 0)
            {
                const string errmsg =
                    "Somehow, amountPurchased was negative ({0})!" +
                    " User: `{1}`. Item template: `{2}`. Requested amount: `{3}`. ItemEntity: `{4}`";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, amountPurchased, this, itemTemplate, amountPurchased, itemEntity);
                Debug.Fail(string.Format(errmsg, amountPurchased, this, itemTemplate, amountPurchased, itemEntity));

                // Raise the amount purchased to 0. They will get it for free, but that is better than PAYING them.
                amountPurchased = 0;
            }

            // Charge them
            var chargeAmount = Math.Max(0, amountPurchased * itemEntity.Value);
            Cash -= chargeAmount;

            // Send purchase message
            if (amountPurchased <= 1)
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.ShopPurchaseSingular, itemTemplate.Name, chargeAmount))
                {
                    Send(pw);
                }
            }
            else
            {
                using (
                    var pw = ServerPacket.SendMessage(GameMessage.ShopPurchasePlural, amountPurchased, itemTemplate.Name,
                                                      chargeAmount))
                {
                    Send(pw);
                }

                if (ShoppingState != null && ShoppingState.ShoppingAt != null)
                {
                    WorldStatsTracker.Instance.AddUserShopBuyItem(this, (int?)itemEntity.ItemTemplateID, (byte)amountPurchased,
                                                                  chargeAmount, ShoppingState.ShoppingAt.ID);
                }
            }

            // Dispose of the item entity since when we gave it to the user, we gave them a deep copy
            itemEntity.Dispose();

            return true;
        }

        /// <summary>
        /// Makes the user try and join whatever group they have an outstanding invite to.
        /// </summary>
        public void TryJoinGroup()
        {
            _groupMemberInfo.JoinGroup(GetTime());
        }

        /// <summary>
        /// Tries to join a guild.
        /// </summary>
        /// <param name="guildName">The name of the guild.</param>
        /// <returns>True if successfully joined the guild; otherwise false.</returns>
        public bool TryJoinGuild(string guildName)
        {
            var guild = _guildManager.GetGuild(guildName);
            return _guildMemberInfo.AcceptInvite(guild, GetTime());
        }

        public bool TrySellInventoryItem(InventorySlot slot, byte amount, IShop<ShopItem> shop)
        {
            if (amount <= 0 || !slot.IsLegalValue() || shop == null || !shop.CanBuy)
                return false;

            // Get the user's item
            var invItem = Inventory[slot];
            if (invItem == null)
                return false;

            var amountToSell = Math.Min(amount, invItem.Amount);
            if (amountToSell <= 0)
                return false;

            // Get the new item amount
            var newItemAmount = invItem.Amount - amountToSell;

            if (newItemAmount > byte.MaxValue)
            {
                const string errmsg = "Somehow, selling `{0}` of item `{1}` resulted in a new item amount of `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, amountToSell, invItem, newItemAmount);
                newItemAmount = byte.MaxValue;
            }
            else if (newItemAmount < 0)
            {
                const string errmsg = "Somehow, selling `{0}` of item `{1}` resulted in a new item amount of `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, amountToSell, invItem, newItemAmount);
                newItemAmount = 0;
            }

            // Give the user the money for selling
            var sellValue = ServerGameData.GetItemSellValue(invItem);
            var totalCash = sellValue * amountToSell;
            Cash += totalCash;

            // Send message
            if (amountToSell <= 1)
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.ShopSellItemSingular, invItem.Name, totalCash))
                {
                    Send(pw);
                }
            }
            else
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.ShopSellItemPlural, amount, invItem.Name, totalCash))
                {
                    Send(pw);
                }

                if (ShoppingState != null && ShoppingState.ShoppingAt != null)
                {
                    WorldStatsTracker.Instance.AddUserShopBuyItem(this, (int?)invItem.ItemTemplateID, amountToSell, totalCash,
                                                                  ShoppingState.ShoppingAt.ID);
                }
            }

            // Set the new item amount (or remove the item if the amount is 0)
            if (newItemAmount == 0)
                Inventory.RemoveAt(slot, true);
            else
                invItem.Amount = (byte)newItemAmount;

            return true;
        }

        public void UseInventoryItem(InventorySlot slot)
        {
            // Get the ItemEntity to use
            var item = Inventory[slot];
            if (item == null)
            {
                const string errmsg = "Tried to use inventory slot `{0}`, but it contains no ItemEntity.";
                Debug.Fail(string.Format(errmsg, slot));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return;
            }

            // Try to use the ItemEntity
            if (!UseItem(item, slot))
            {
                // Check if the failure to use the ItemEntity was due to an invalid amount of the ItemEntity
                if (item.Amount <= 0)
                {
                    const string errmsg = "Tried to use inventory ItemEntity `{0}` at slot `{1}`, but it had an invalid amount.";
                    Debug.Fail(string.Format(errmsg, item, slot));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, item, slot);

                    // Destroy the ItemEntity
                    Inventory.RemoveAt(slot, true);
                }
            }

            // Lower the count of use-once items
            if (item.Type == ItemType.UseOnce)
                Inventory.DecreaseItemAmount(slot);
        }

        #region IClientCommunicator Members

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
            using (var pw = ServerPacket.SendMessage(message))
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
            using (var pw = ServerPacket.SendMessage(message, parameters))
            {
                Send(pw);
            }
        }

        #endregion

        #region IGroupable Members

        /// <summary>
        /// Gets or sets the group that this <see cref="IGroupable"/> is currently part of. This property should only be
        /// set by the <see cref="IGroup"/> and <see cref="IGroupable"/>, and should never be used to try and add or
        /// remove a <see cref="IGroupable"/> from a <see cref="IGroup"/>.
        /// </summary>
        IGroup IGroupable.Group
        {
            get { return _groupMemberInfo.Group; }
            set { _groupMemberInfo.Group = value; }
        }

        /// <summary>
        /// Gets if this <see cref="IGroupable"/> is close enough to the <paramref name="other"/> to
        /// share group-based rewards with them. This method should return the same value for two
        /// <see cref="IGroupable"/>s, despite which one is used as the caller and which is used as
        /// the parameter.
        /// </summary>
        /// <param name="other">The <see cref="IGroupable"/> to check if within distance of.</param>
        /// <returns>True if the <paramref name="other"/> is close enough to this <see cref="IGroupable"/>
        /// to share group-based rewards with them.</returns>
        bool IGroupable.IsInShareDistance(IGroupable other)
        {
            return GroupHelper.IsInShareDistance(this, other as Character);
        }

        /// <summary>
        /// Notifies this <see cref="IGroupable"/> that they have been invited to join another group. This should only
        /// be called by the <see cref="IGroupManager"/>.
        /// </summary>
        /// <param name="group">The <see cref="IGroup"/> that this <see cref="IGroupable"/> was invited to join.</param>
        void IGroupable.NotifyInvited(IGroup group)
        {
            _groupMemberInfo.ReceiveInvite(group, GetTime());
        }

        #endregion

        #region IGuildMember Members

        /// <summary>
        /// Gets or sets the guild member's current guild. Will be null if they are not part of any guilds.
        /// This value should only be set by the <see cref="IGuildManager"/>. When the value is changed,
        /// <see cref="IGuild.RemoveOnlineMember"/> should be called for the old value (if not null) and
        /// <see cref="IGuild.AddOnlineMember"/> should be called for the new value (if not null).
        /// </summary>
        public IGuild Guild
        {
            get { return _guildMemberInfo.Guild; }
            set { _guildMemberInfo.Guild = value; }
        }

        /// <summary>
        /// Gets or sets the guild member's ranking in the guild. Only valid if in a guild.
        /// This value should only be set by the <see cref="IGuildManager"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is greater than the maximum
        /// rank value.</exception>
        GuildRank IGuildMember.GuildRank
        {
            get { return _guildMemberInfo.GuildRank; }
            set { _guildMemberInfo.GuildRank = value; }
        }

        /// <summary>
        /// Gets an ID that can be used to distinguish this <see cref="IGuildMember"/> from any other
        /// <see cref="IGuildMember"/> instance.
        /// </summary>
        int IGuildMember.ID
        {
            get { return (int)ID; }
        }

        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                if (base.Name == value)
                    return;

                if (!GameData.UserName.IsValid(value))
                {
                    const string errmsg = "Attempted to give User `{0}` an invalid name `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, this, value);
                    Debug.Fail(string.Format(errmsg, this, value));
                    return;
                }

                base.Name = value;
            }
        }

        /// <summary>
        /// Saves the guild member's information.
        /// </summary>
        void IGuildMember.SaveGuildInformation()
        {
            if (Guild == null)
                _deleteGuildMemberQuery.Execute(ID);
            else
            {
                var values = new ReplaceGuildMemberQuery.QueryArgs(ID, Guild.ID, ((IGuildMember)this).GuildRank);
                _replaceGuildMemberQuery.Execute(values);
            }
        }

        /// <summary>
        /// Notifies the guild member that they have been invited to join a guild.
        /// </summary>
        /// <param name="inviter">The guild member that invited them into the guild.</param>
        /// <param name="guild">The guild they are being invited to join.</param>
        void IGuildMember.SendGuildInvite(IGuildMember inviter, IGuild guild)
        {
            _guildMemberInfo.ReceiveInvite(guild, GetTime());
            Send(GameMessage.GuildInvited, inviter.Name, guild.Name);
        }

        #endregion

        #region IQuestPerformer<User> Members

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has accepted a new quest.
        /// </summary>
        public event QuestPerformerQuestEventHandler<User> QuestAccepted;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has canceled an active quest.
        /// </summary>
        public event QuestPerformerQuestEventHandler<User> QuestCanceled;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has finished a quest.
        /// </summary>
        public event QuestPerformerQuestEventHandler<User> QuestFinished;

        /// <summary>
        /// Gets the incomplete quests that this <see cref="IQuestPerformer{TCharacter}"/> is currently working on.
        /// </summary>
        public IEnumerable<IQuest<User>> ActiveQuests
        {
            get { return _questInfo.ActiveQuests; }
        }

        /// <summary>
        /// Gets the quests that this <see cref="IQuestPerformer{TCharacter}"/> has completed.
        /// </summary>
        public IEnumerable<IQuest<User>> CompletedQuests
        {
            get { return _questInfo.CompletedQuests; }
        }

        /// <summary>
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> can accept the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> can accept.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> can accept the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        public bool CanAcceptQuest(IQuest<User> quest)
        {
            return _questInfo.CanAcceptQuest(quest);
        }

        /// <summary>
        /// Cancels an active quest.
        /// </summary>
        /// <param name="quest">The active quest to cancel.</param>
        /// <returns>True if the <paramref name="quest"/> was canceled; false if the <paramref name="quest"/> failed to
        /// be canceled, such as if the <paramref name="quest"/> was not in the list of active quests.</returns>
        public bool CancelQuest(IQuest<User> quest)
        {
            return _questInfo.CancelQuest(quest);
        }

        /// <summary>
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> has finished the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> has completed.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> has completed the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        public bool HasCompletedQuest(IQuest<User> quest)
        {
            return _questInfo.HasCompletedQuest(quest);
        }

        /// <summary>
        /// Tries to add the given <paramref name="quest"/> to this <see cref="IQuestPerformer{TCharacter}"/>'s list
        /// of active quests.
        /// </summary>
        /// <param name="quest">The quest to try to add to this <see cref="IQuestPerformer{TCharacter}"/>'s list
        /// of active quests.</param>
        /// <returns>True if the <paramref name="quest"/> was successfully added; otherwise false.</returns>
        public bool TryAddQuest(IQuest<User> quest)
        {
            return _questInfo.TryAddQuest(quest);
        }

        /// <summary>
        /// Tries to finish the given <paramref name="quest"/> that this <see cref="IQuestPerformer{TCharacter}"/>
        /// has started.
        /// </summary>
        /// <param name="quest">The quest to turn in.</param>
        /// <returns>
        /// True if the <paramref name="quest"/> was successfully finished; otherwise false.
        /// </returns>
        public bool TryFinishQuest(IQuest<User> quest)
        {
            return _questInfo.TryFinishQuest(quest);
        }

        #endregion
    }
}