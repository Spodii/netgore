using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.Groups;
using DemoGame.Server.Guilds;
using DemoGame.Server.PeerTrading;
using DemoGame.Server.Properties;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using Lidgren.Network;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.Db;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.NPCChat;
using NetGore.Features.PeerTrading;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.IO;
using NetGore.Network;
using NetGore.Stats;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// A user-controlled character.
    /// </summary>
    public class User : Character, IGuildMember, INetworkSender, IGroupable, IQuestPerformer<User>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly DeleteCharacterQuestStatusKillsQuery _deleteCharacterQuestStatusKillsQuery;
        static readonly DeleteCharacterQuestStatusQuery _deleteCharacterQuestStatusQuery;
        static readonly DeleteGuildMemberQuery _deleteGuildMemberQuery;
        static readonly GuildManager _guildManager = GuildManager.Instance;
        static readonly InsertCharacterQuestStatusStartQuery _insertCharacterQuestStatusStartQuery;
        static readonly InsertGuildMemberQuery _insertGuildMemberQuery;
        static readonly SelectGuildMemberQuery _selectGuildMemberQuery;
        static readonly UpdateCharacterQuestStatusFinishedQuery _updateCharacterQuestStatusFinishedQuery;

        readonly UserChatDialogState _chatState;
        readonly GroupMemberInfo _groupMemberInfo;
        readonly GuildMemberInfo _guildMemberInfo;
        readonly QuestPerformerStatusHelper _questInfo;
        readonly UserShoppingState _shoppingState;
        readonly IUserAccount _userAccount;
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
            _insertGuildMemberQuery = dbController.GetQuery<InsertGuildMemberQuery>();
            _selectGuildMemberQuery = dbController.GetQuery<SelectGuildMemberQuery>();

            _insertCharacterQuestStatusStartQuery = dbController.GetQuery<InsertCharacterQuestStatusStartQuery>();
            _deleteCharacterQuestStatusQuery = dbController.GetQuery<DeleteCharacterQuestStatusQuery>();
            _deleteCharacterQuestStatusKillsQuery = dbController.GetQuery<DeleteCharacterQuestStatusKillsQuery>();
            _updateCharacterQuestStatusFinishedQuery = dbController.GetQuery<UpdateCharacterQuestStatusFinishedQuery>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userAccount">The user's account.</param>
        /// <param name="world">World the user belongs to.</param>
        /// <param name="characterID">User's CharacterID.</param>
        /// <exception cref="ArgumentNullException"><paramref name="userAccount"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="world"/> is null.</exception>
        public User(IUserAccount userAccount, World world, CharacterID characterID) : base(world, true)
        {
            if (userAccount == null)
                throw new ArgumentNullException("userAccount");
            if (world == null)
                throw new ArgumentNullException("world");

            _userAccount = userAccount;

            // Create some objects
            _guildMemberInfo = new GuildMemberInfo(this);
            _groupMemberInfo = new GroupMemberInfo(this);
            _shoppingState = new UserShoppingState(this);
            _chatState = new UserChatDialogState(this);
            _userStatsBase = (UserStats)BaseStats;
            _userStatsMod = (UserStats)ModStats;

            // Load the character data
            Load(characterID);

            // Load more stuff
            _questInfo = CreateQuestInfo();

            // Ensure the correct Alliance is being used
            Alliance = AllianceManager[new AllianceID(0)];

            // Activate the user
            _userInventory = (UserInventory)Inventory;
            IsAlive = true;

            // Send the initial information
            using (var pw = ServerPacket.SetLevel(Level))
            {
                Send(pw, ServerMessageType.GUIUserStats);
            }
            using (var pw = ServerPacket.SetCash(Cash))
            {
                Send(pw, ServerMessageType.GUIUserStats);
            }
            using (var pw = ServerPacket.SetExp(Exp))
            {
                Send(pw, ServerMessageType.GUIUserStats);
            }
            using (var pw = ServerPacket.SetStatPoints(StatPoints))
            {
                Send(pw, ServerMessageType.GUIUserStats);
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
            get { return UserAccount.Socket; }
        }

        public IGroup Group
        {
            get { return _groupMemberInfo.Group; }
            set { _groupMemberInfo.Group = value; }
        }

        /// <summary>
        /// Gets if this character is currently involved in a peer trade session.
        /// </summary>
        public bool IsPeerTrading
        {
            get { return PeerTradeSession != null; }
        }

        /// <summary>
        /// Gets or sets the peer trade session that this <see cref="User"/> is currently participating in, or null if they
        /// are not currently trading. This should only be set by the <see cref="PeerTradeSession"/> class!
        /// </summary>
        public IPeerTradeSession<User, ItemEntity> PeerTradeSession { get; set; }

        /// <summary>
        /// Gets the <see cref="UserPermissions"/> level for this user.
        /// </summary>
        public UserPermissions Permissions
        {
            get
            {
                var acc = UserAccount;
                if (acc == null)
                    return UserPermissions.None;

                return acc.Permissions;
            }
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
        /// Gets or sets the <see cref="User"/>'s shopping state.
        /// </summary>
        public UserShoppingState ShoppingState
        {
            get { return _shoppingState; }
        }

        /// <summary>
        /// Gets the <see cref="IUserAccount"/> for the account that this <see cref="User"/> is on.
        /// Will not be null.
        /// </summary>
        public IUserAccount UserAccount
        {
            get { return _userAccount; }
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
                Send(pw, ServerMessageType.GUI);
            }
        }

        /// <summary>
        /// If this <see cref="User"/> is involved in a peer trade, forces them to cancel it. Does nothing if the <see cref="User"/>
        /// is not involved in any peer trade.
        /// </summary>
        public void CancelPeerTradeIfTrading()
        {
            var pts = PeerTradeSession;
            if (pts == null)
                return;

            pts.Cancel(this);
        }

        /// <summary>
        /// Gets if data can be sent to this <see cref="User"/>.
        /// </summary>
        /// <returns>True if the data sending can continue safely; otherwise false.</returns>
        bool CheckIfCanSendToUser()
        {
            if (IsDisposed)
            {
                const string errmsg = "Tried to send data to disposed User `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return false;
            }

            if (Conn == null || !Conn.IsConnected)
            {
                const string errmsg = "Send to `{0}` failed - Conn is null or not connected. Disposing User...";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                DelayedDispose();

                return false;
            }

            return true;
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

            ret.QuestAccepted += delegate(QuestPerformerStatusHelper<User> sender, EventArgs<IQuest<User>> e)
            {
                OnQuestAccepted(e.Item1);
                if (QuestAccepted != null)
                    QuestAccepted.Raise(this, e);
            };

            ret.QuestCanceled += delegate(QuestPerformerStatusHelper<User> sender, EventArgs<IQuest<User>> e)
            {
                OnQuestCanceled(e.Item1);
                if (QuestCanceled != null)
                    QuestCanceled.Raise(this, e);
            };

            ret.QuestFinished += delegate(QuestPerformerStatusHelper<User> sender, EventArgs<IQuest<User>> e)
            {
                OnQuestFinished(e.Item1);
                if (QuestFinished != null)
                    QuestFinished.Raise(this, e);
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
        /// Gets this <see cref="User"/>'s <see cref="IUserAccount"/>.
        /// </summary>
        /// <returns>This <see cref="User"/>'s <see cref="IUserAccount"/>. Shouldn't be null, but may potentially be and the caller
        /// should always be prepared for the value being null.</returns>
        public IUserAccount GetAccount()
        {
            return World.GetUserAccount(Conn);
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="MapID"/> that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The ID of the map to load this <see cref="Character"/> on.</returns>
        protected override MapID GetLoadMap()
        {
            if (Map == null)
            {
                const string errmsg =
                    "Users are expected to have a valid respawn position at all times, but user `{0}` does not. Using ServerSettings.InvalidUserLoadMap/Position instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerSettings.Default.InvalidUserLoadMap;
            }

            return Map.ID;
        }

        /// <summary>
        /// Handles when no legal position could be found for this <see cref="Character"/>.
        /// This will usually occur when performing a teleport into an area that is completely blocked off, and no near-by
        /// position can be found. Moving a <see cref="Character"/> too far from the original position can result in them
        /// going somewhere that they are not supposed to, so it is best to send them to a predefined location.
        /// </summary>
        /// <param name="position">The position that the <see cref="Character"/> tried to go to, but failed to.</param>
        /// <returns>The position to warp the <see cref="Character"/> to.</returns>
        protected override Vector2 HandleNoLegalPositionFound(Vector2 position)
        {
            // Users always get sent to the invalid pos
            const string errmsg = "User `{0}` getting set to invalid user position (defined in ServerSettings.settings)." + 
                " If this occurs non-stop, ensure the map/pos defined for InvalidUserLoadMap/Position are valid.";

            if (log.IsInfoEnabled)
                log.InfoFormat(errmsg, this);

            var mapID = ServerSettings.Default.InvalidUserLoadMap;
            var map = World.GetMap(mapID);
            var pos = ServerSettings.Default.InvalidUserLoadPosition;
            Teleport(map, pos);

            return pos;
        }

        /// <summary>
        /// When overridden in the derived class, gets the position that this <see cref="Character"/>
        /// will use for when loading.
        /// </summary>
        /// <returns>The position to load this <see cref="Character"/> at.</returns>
        protected override Vector2 GetLoadPosition()
        {
            Vector2 pos = Position;
            var map = Map;

            if (map == null)
            {
                const string errmsg =
                    "Users are expected to have a valid respawn position at all times, but user `{0}` does not." +
                    " Using ServerSettings.InvalidUserLoad... instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));

                return ServerSettings.Default.InvalidUserLoadPosition;
            }

            return pos;
        }

        /// <summary>
        /// Gives the kill reward.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="cash">The cash.</param>
        protected override void GiveKillReward(int exp, int cash)
        {
            base.GiveKillReward(exp, cash);

            var map = Map;
            if (map != null)
            {
                EventCounterManager.Map.Increment(map.ID, MapEventCounterType.UserGainedKillExp, exp);
                EventCounterManager.Map.Increment(map.ID, MapEventCounterType.UserGainedKillCash, cash);
            }

            using (var pw = ServerPacket.NotifyExpCash(exp, cash))
            {
                Send(pw, ServerMessageType.GUI);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles additional loading stuff.
        /// </summary>
        /// <param name="v">The ICharacterTable containing the database values for this Character.</param>
        protected override void HandleAdditionalLoading(ICharacterTable v)
        {
            base.HandleAdditionalLoading(v);

            if (!World.TryAddUser(this))
            {
                // Failed to add user to the world (probably because they are already online - dispose of them
                DelayedDispose();
                return;
            }

            // Load the guild information
            var guildInfo = _selectGuildMemberQuery.Execute(ID);
            if (guildInfo != null)
            {
                Debug.Assert(guildInfo.CharacterID == ID);
                _guildMemberInfo.Guild = _guildManager.GetGuild(guildInfo.GuildID);
                _guildMemberInfo.GuildRank = guildInfo.Rank;
            }

            // Restore any of the user's lost cash or items from an improperly closed trade (namely for if the server crashed)
            PeerTradingHelper.RecoverLostTradeItems(this);
            PeerTradingHelper.RecoverLostTradeCash(this);
        }

        /// <summary>
        /// Performs the actual disposing of the Entity. This is called by the base Entity class when
        /// a request has been made to dispose of the Entity. This is guarenteed to only be called once.
        /// All classes that override this method should be sure to call base.DisposeHandler() after
        /// handling what it needs to dispose.
        /// </summary>
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was explicitly called and managed resources need to be
        /// disposed. When false, managed resources do not need to be disposed since this object was garbage-collected.</param>
        protected override void HandleDispose(bool disposeManaged)
        {
            CancelPeerTradeIfTrading();

            base.HandleDispose(disposeManaged);

            // Remove the User from being the active User in the account
            var account = GetAccount();
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
            CancelPeerTradeIfTrading();

            var map = Map;
            if (map != null)
                EventCounterManager.Map.Increment(map.ID, MapEventCounterType.UserKilled);

            base.Kill();

            // Respawn the user
            MapID rMapID;
            Vector2 rPos;
            ServerGameData.GetUserRespawnPosition(this, out rMapID, out rPos);

            var rMap = World.GetMap(rMapID);
            Teleport(rMap, rPos);

            // Restore the stats
            UpdateModStats();

            HP = (int)ModStats[StatType.MaxHP];
            MP = (int)ModStats[StatType.MaxMP];

            // Bring them back to life instantly
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
                Send(pw, ServerMessageType.GUIUserStats);
            }
        }

        /// <summary>
        /// Translates the entity from its current position.
        /// </summary>
        /// <param name="adjustment">Amount to move.</param>
        public override void Move(Vector2 adjustment)
        {
            CancelPeerTradeIfTrading();

            base.Move(adjustment);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.AttackedByCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="attacker">The <see cref="Character"/> that attacked us.</param>
        /// <param name="damage">The amount of damage inflicted on this <see cref="Character"/>.</param>
        protected override void OnAttackedByCharacter(Character attacker, int damage)
        {
            if (attacker is User)
                EventCounterManager.User.Increment(ID, UserEventCounterType.DamageTakenFromUser, damage);
            else
                EventCounterManager.User.Increment(ID, UserEventCounterType.DamageTakenFromNonUser, damage);
            EventCounterManager.User.Increment(ID, UserEventCounterType.Attacked);

            base.OnAttackedByCharacter(attacker, damage);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of the
        /// <see cref="Character.AttackedCharacter"/> event. It is recommended you override this method instead of
        /// using the corresponding event when possible.
        /// </summary>
        /// <param name="attacked">The <see cref="Character"/> that was attacked.</param>
        /// <param name="damage">The amount of damage inflicted on the <paramref name="attacked"/> by
        /// the this <see cref="Character"/>.</param>
        protected override void OnAttackedCharacter(Character attacked, int damage)
        {
            if (attacked is User)
                EventCounterManager.User.Increment(ID, UserEventCounterType.DamageDealtToUser, damage);
            else
                EventCounterManager.User.Increment(ID, UserEventCounterType.DamageDealtToNonUser, damage);
            EventCounterManager.User.Increment(ID, UserEventCounterType.Attack);

            base.OnAttackedCharacter(attacked, damage);
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
                Send(pw, ServerMessageType.GUIUserStats);
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
                Send(pw, ServerMessageType.GUIUserStats);
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

            if (killed is User)
                EventCounterManager.User.Increment(ID, UserEventCounterType.KillUser);
            else
                EventCounterManager.User.Increment(ID, UserEventCounterType.KillNonUser);

            var killedNPC = killed as NPC;

            // Handle killing a NPC
            if (killedNPC != null)
            {
                var giveExp = killedNPC.GiveExp;
                var giveCash = killedNPC.GiveCash;

                WorldStatsTracker.Instance.AddUserKillNPC(this, killedNPC);

                bool hasTemplateID = killedNPC.CharacterTemplateID.HasValue;

                if (hasTemplateID)
                {
                    WorldStatsTracker.Instance.AddCountUserKillNPC((int)ID, (int)killedNPC.CharacterTemplateID.Value);
                }

                // If in a group, split among the group members (as needed)
                var group = ((IGroupable)this).Group;
                if (group == null || group.ShareMode == GroupShareMode.NoSharing)
                {
                    // Not in a group or no sharing is being used in the group, give all to self
                    GiveKillReward(giveExp, giveCash);
                    if (hasTemplateID)
                    {
                        _questInfo.QuestKillCounter.IncrementCounter((CharacterTemplateID)killedNPC.CharacterTemplateID);
                    }
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
                        if (hasTemplateID)
                        {
                            member._questInfo.QuestKillCounter.IncrementCounter((CharacterTemplateID)killedNPC.CharacterTemplateID);
                        }
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
        protected override void OnLevelChanged(short oldValue, short newValue)
        {
            base.OnLevelChanged(oldValue, newValue);

            using (var pw = ServerPacket.SetLevel(newValue))
            {
                Send(pw, ServerMessageType.GUIUserStats);
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
                Send(pw, ServerMessageType.GUIUserStats);
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
            {
                WorldStatsTracker.Instance.AddUserConsumeItem(this, item);
                EventCounterManager.User.Increment(ID, UserEventCounterType.ItemConsumed);

                if (item.ItemTemplateID.HasValue)
                    WorldStatsTracker.Instance.AddCountUserConsumeItem((int)ID, (int)item.ItemTemplateID.Value);
            }
        }

        /// <summary>
        /// Not supported by the User.
        /// </summary>
        public override void RemoveAI()
        {
        }

        /// <summary>
        /// Sends data to the client. This method is thread-safe.
        /// </summary>
        /// <param name="data">BitStream containing the data to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="data"/>.</param>
        public void Send(BitStream data, ServerMessageType messageType)
        {
            if (!CheckIfCanSendToUser())
                return;

            Conn.Send(data, messageType);
        }

        /// <summary>
        /// Sends data to the client. This method is thread-safe.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        public void Send(GameMessage message, ServerMessageType messageType)
        {
            Send(message, messageType, null);
        }

        /// <summary>
        /// Sends data to the client. This method is thread-safe.
        /// </summary>
        /// <param name="message">GameMessage to send to the User.</param>
        /// <param name="messageType">The <see cref="ServerMessageType"/> to use for sending the <paramref name="message"/>.</param>
        /// <param name="parameters">Message parameters.</param>
        public void Send(GameMessage message, ServerMessageType messageType, params object[] parameters)
        {
            using (var pw = ServerPacket.SendMessage(message, parameters))
            {
                Send(pw, messageType);
            }
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
                Send(pw, ServerMessageType.GUIItemInfo);
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
                Send(pw, ServerMessageType.GUIItemInfo);
            }
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
        /// <param name="e">The <see cref="NetGore.EventArgs{ActiveStatusEffect}"/> instance containing the event data.</param>
        protected override void StatusEffects_HandleOnAdd(CharacterStatusEffects effects, EventArgs<ActiveStatusEffect> e)
        {
            base.StatusEffects_HandleOnAdd(effects, e);

            var currentTime = GetTime();
            var timeLeft = e.Item1.GetTimeRemaining(currentTime);

            using (var pw = ServerPacket.AddStatusEffect(e.Item1.StatusEffect.StatusEffectType, e.Item1.Power, timeLeft))
            {
                Send(pw, ServerMessageType.GUIUserStatus);
            }
        }

        /// <summary>
        /// Handles when an <see cref="ActiveStatusEffect"/> is removed from this Character's StatusEffects.
        /// </summary>
        /// <param name="effects">The CharacterStatusEffects the event took place on.</param>
        /// <param name="e">The <see cref="NetGore.EventArgs{ActiveStatusEffect}"/> instance containing the event data.</param>
        protected override void StatusEffects_HandleOnRemove(CharacterStatusEffects effects, EventArgs<ActiveStatusEffect> e)
        {
            base.StatusEffects_HandleOnRemove(effects, e);

            using (var pw = ServerPacket.RemoveStatusEffect(e.Item1.StatusEffect.StatusEffectType))
            {
                Send(pw, ServerMessageType.GUIUserStatus);
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

        /// <summary>
        /// Teleports the character to a new position and informs clients in the area of
        /// interest that the character has teleported.
        /// </summary>
        /// <param name="position">Position to teleport to.</param>
        protected override void Teleport(Vector2 position)
        {
            CancelPeerTradeIfTrading();

            base.Teleport(position);
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
                        Send(pw, ServerMessageType.GUI);
                    }
                }
                else
                {
                    using (
                        var pw = ServerPacket.SendMessage(GameMessage.ShopInsufficientFundsToPurchasePlural, amount,
                            itemTemplate.Name))
                    {
                        Send(pw, ServerMessageType.GUI);
                    }
                }
                return false;
            }

            // Create the item
            var itemEntity = new ItemEntity(itemTemplate, amount);

            // Check for room in the inventory
            if (!Inventory.CanAdd(itemEntity))
            {
                itemEntity.Destroy();
                return false;
            }

            // Store some info about the item
            var itemValue = itemEntity.Value;

            // Add to the inventory keeping track of the changed slots.
            var remainderItem = Inventory.TryAdd(itemEntity);

            // Find the number of remaining items (in case something went wrong and not all was added)
            var remainderAmount = 0;
            if (remainderItem != null)
            {
                remainderAmount = remainderItem.Amount;
                remainderItem.Destroy();
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
            var chargeAmount = Math.Max(0, amountPurchased * itemValue);
            Cash -= chargeAmount;

            // Send purchase message
            if (amountPurchased <= 1)
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.ShopPurchaseSingular, itemTemplate.Name, chargeAmount))
                {
                    Send(pw, ServerMessageType.GUI);
                }
            }
            else
            {
                using (
                    var pw = ServerPacket.SendMessage(GameMessage.ShopPurchasePlural, amountPurchased, itemTemplate.Name,
                        chargeAmount))
                {
                    Send(pw, ServerMessageType.GUI);
                }
            }

            // Track event
            if (ShoppingState != null && ShoppingState.ShoppingAt != null)
            {
                var shoppingAt = ShoppingState.ShoppingAt;

                WorldStatsTracker.Instance.AddUserShopBuyItem(this, (int?)itemEntity.ItemTemplateID, (byte)amountPurchased,
                    chargeAmount, shoppingAt.ID);

                if (itemEntity.ItemTemplateID.HasValue)
                {
                    WorldStatsTracker.Instance.AddCountBuyItem((int)itemEntity.ItemTemplateID.Value, amountPurchased);
                    EventCounterManager.ItemTemplate.Increment(itemEntity.ItemTemplateID.Value,
                        ItemTemplateEventCounterType.BuyFromShop, amountPurchased);
                }

                WorldStatsTracker.Instance.AddCountShopBuy((int)ShoppingState.ShoppingAt.ID, amountPurchased);
                EventCounterManager.Shop.Increment(shoppingAt.ID, ShopEventCounterType.Buy);
                EventCounterManager.Shop.Increment(shoppingAt.ID, ShopEventCounterType.BuyAmount, amountPurchased);
                EventCounterManager.Shop.Increment(shoppingAt.ID, ShopEventCounterType.BuyValue, chargeAmount);
            }

            // Destroy the item entity since when we gave it to the user, we gave them a deep copy.
            itemEntity.Destroy();

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

        /// <summary>
        /// Tries to sell an item from the <see cref="User"/>'s inventory to a shop.
        /// </summary>
        /// <param name="slot">The slot of the <see cref="User"/>'s inventory containing the item to sell.</param>
        /// <param name="amount">The number of items in the <paramref name="slot"/> to sell. If this value is greater than the
        /// number of items the <see cref="User"/> has in the given <paramref name="slot"/>, then all of the items in the
        /// <paramref name="slot"/> will be sold.</param>
        /// <param name="shop">The shop to sell the inventory item to.</param>
        /// <returns>True if the item in the <see cref="User"/>'s invetory at the given <paramref name="slot"/> was sold
        /// to the <paramref name="shop"/>; otherwise false.</returns>
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
                    Send(pw, ServerMessageType.GUI);
                }
            }
            else
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.ShopSellItemPlural, amount, invItem.Name, totalCash))
                {
                    Send(pw, ServerMessageType.GUI);
                }
            }

            // Track event
            WorldStatsTracker.Instance.AddUserShopSellItem(this, (int?)invItem.ItemTemplateID, amountToSell, totalCash, shop.ID);

            if (invItem.ItemTemplateID.HasValue)
            {
                WorldStatsTracker.Instance.AddCountSellItem((int)invItem.ItemTemplateID.Value, amountToSell);
                EventCounterManager.ItemTemplate.Increment(invItem.ItemTemplateID.Value, ItemTemplateEventCounterType.SellToShop,
                    amountToSell);
            }

            WorldStatsTracker.Instance.AddCountShopSell((int)shop.ID, amountToSell);
            EventCounterManager.Shop.Increment(shop.ID, ShopEventCounterType.Sell);
            EventCounterManager.Shop.Increment(shop.ID, ShopEventCounterType.SellAmount, amountToSell);
            EventCounterManager.Shop.Increment(shop.ID, ShopEventCounterType.SellValue, totalCash);

            // Set the new item amount (or remove the item if the amount is 0)
            if (newItemAmount == 0)
                Inventory.RemoveAt(slot, true);
            else
                invItem.Amount = (byte)newItemAmount;

            return true;
        }

        /// <summary>
        /// Tries to start a peer trade with another <see cref="Character"/>.
        /// </summary>
        /// <param name="target">The <see cref="Character"/> to trade with.</param>
        /// <returns>True if the peer was successfully started; otherwise false.</returns>
        public bool TryStartPeerTrade(User target)
        {
            // Make sure we can start the trade
            if (target == null)
                return false;

            // Perform some initial checks so we can give more detailed error messages as to why the peer trade failed
            // All of these checks should already be performed by PeerTradingSession.Create()
            if (!IsAlive || PeerTradeSession != null)
            {
                Send(GameMessage.PeerTradingCannotStartTrade, ServerMessageType.GUI);
                return false;
            }

            if (!target.IsAlive || target.PeerTradeSession != null)
            {
                Send(GameMessage.PeerTradingTargetCannotStartTrade, ServerMessageType.GUI);
                return false;
            }

            if (target.Map != Map || this.GetDistance(target) > PeerTradingSettings.Instance.MaxDistance)
            {
                Send(GameMessage.PeerTradingTooFarAway, ServerMessageType.GUI);
                return false;
            }

            // Make sure they are not moving
            StopMoving();
            target.StopMoving();

            // Try to start the trade
            var ts = PeerTrading.PeerTradeSession.Create(this, target);

            // Check if the trade could not be started
            if (ts == null)
            {
                Send(GameMessage.PeerTradingCannotStartTrade, ServerMessageType.GUI);
                return false;
            }
            else
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

            // Check if the item enables the user to learn a skill
            if (item.SkillID.HasValue)
                if (!this.KnownSkills.Knows(item.SkillID.Value))
                    KnownSkills.SetSkill(item.SkillID.Value, true);
        }

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
                var values = new InsertGuildMemberQuery.QueryArgs(ID, Guild.ID, ((IGuildMember)this).GuildRank);
                _insertGuildMemberQuery.Execute(values);
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
            Send(GameMessage.GuildInvited, ServerMessageType.GUI, inviter.Name, guild.Name);
        }

        #endregion

        #region INetworkSender Members

        /// <summary>
        /// Gets if the connection is alive and functional.
        /// </summary>
        bool INetworkSender.IsConnected
        {
            get { return Conn != null && Conn.IsConnected; }
        }

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value greater than or equal to 0 and
        /// less than <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than or equal to
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        bool INetworkSender.Send(BitStream data, NetDeliveryMethod deliveryMethod, int sequenceChannel)
        {
            if (!CheckIfCanSendToUser())
                return false;

            return Conn.Send(data, deliveryMethod, sequenceChannel);
        }

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value between 0 and
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        bool INetworkSender.Send(byte[] data, NetDeliveryMethod deliveryMethod, int sequenceChannel)
        {
            if (!CheckIfCanSendToUser())
                return false;

            return Conn.Send(data, deliveryMethod, sequenceChannel);
        }

        #endregion

        #region IQuestPerformer<User> Members

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has accepted a new quest.
        /// </summary>
        public event TypedEventHandler<User, EventArgs<IQuest<User>>> QuestAccepted;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has canceled an active quest.
        /// </summary>
        public event TypedEventHandler<User, EventArgs<IQuest<User>>> QuestCanceled;

        /// <summary>
        /// Notifies listeners when this <see cref="IQuestPerformer{TCharacter}"/> has finished a quest.
        /// </summary>
        public event TypedEventHandler<User, EventArgs<IQuest<User>>> QuestFinished;

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
        /// Gets the quests that this <see cref="IQuestPerformer{TCharacter}"/> has repeatable.
        /// </summary>
        public IEnumerable<IQuest<User>> RepeatableQuests
        {
            get { return _questInfo.RepeatableQuests; }
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
        /// Gets if this <see cref="IQuestPerformer{TCharacter}"/> can repeat the given <paramref name="quest"/>.
        /// </summary>
        /// <param name="quest">The quest to check if this <see cref="IQuestPerformer{TCharacter}"/> can repeat.</param>
        /// <returns>True if this <see cref="IQuestPerformer{TCharacter}"/> can repeat the given <paramref name="quest"/>;
        /// otherwise false.</returns>
        public bool IsRepeatableQuest(IQuest<User> quest)
        {
            return _questInfo.IsRepeatableQuest(quest);
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