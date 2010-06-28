using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Quests;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Features.Emoticons;
using NetGore.Features.Quests;
using NetGore.IO;
using NetGore.Network;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    class ServerPacketHandler : ISocketReceiveDataProcessor, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly QuestManager _questManager = QuestManager.Instance;

        readonly Queue<IIPSocket> _disconnectedSockets = new Queue<IIPSocket>();
        readonly IMessageProcessorManager _ppManager;
        readonly SayHandler _sayHandler;
        readonly Server _server;
        readonly ServerSockets _serverSockets;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPacketHandler"/> class.
        /// </summary>
        /// <param name="serverSockets">The server sockets.</param>
        /// <param name="server">The server.</param>
        public ServerPacketHandler(ServerSockets serverSockets, Server server)
        {
            if (serverSockets == null)
                throw new ArgumentNullException("serverSockets");
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;
            _serverSockets = serverSockets;
            _serverSockets.Disconnected += ServerSockets_Disconnected;

            _sayHandler = new SayHandler(server);

            // When debugging, use the StatMessageProcessorManager instead (same thing as the other, but provides network statistics)
#if DEBUG
            var m = new StatMessageProcessorManager(this, EnumHelper<ClientPacketID>.BitsRequired);
            m.Stats.EnableFileOutput(ContentPaths.Build.Root.Join("netstats_in" + EngineSettings.DataFileSuffix));
            _ppManager = m;
#else
            _ppManager = new MessageProcessorManager(this, EnumHelper<ClientPacketID>.BitsRequired);
#endif
        }

        public IDbController DbController
        {
            get { return Server.DbController; }
        }

        /// <summary>
        /// Gets the server that the data is coming from.
        /// </summary>
        public Server Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Gets the World to use.
        /// </summary>
        public World World
        {
            get { return Server.World; }
        }

        /// <summary>
        /// Gets the <see cref="IIPSocket"/>s that have been disconnected since the last call to this method.
        /// </summary>
        /// <returns>The <see cref="IIPSocket"/>s that have been disconnected since the last call to this method.</returns>
        public IEnumerable<IIPSocket> GetDisconnectedSockets()
        {
            if (_disconnectedSockets.Count == 0)
                return Enumerable.Empty<IIPSocket>();

            lock (_disconnectedSockets)
            {
                if (_disconnectedSockets.Count == 0)
                    return Enumerable.Empty<IIPSocket>();

                var ret = _disconnectedSockets.ToArray();
                _disconnectedSockets.Clear();
                return ret;
            }
        }

        static Character GetTargetCharacter(Character user, MapEntityIndex? index)
        {
            if (!index.HasValue)
                return null;

            // Check for a valid user
            if (user == null || user.Map == null)
                return null;

            // Check for a valid target index
            var target = user.Map.GetDynamicEntity<Character>(index.Value);
            if (target == null || target.Map != user.Map)
                return null;

            // Check for a valid distance
            if (user.GetDistance(target) > GameData.MaxTargetDistance)
                return null;

            return target;
        }

        [MessageHandler((byte)ClientPacketID.AcceptOrTurnInQuest)]
        void RecvAcceptOrTurnInQuest(IIPSocket conn, BitStream r)
        {
            var providerIndex = r.ReadMapEntityIndex();
            var questID = r.ReadQuestID();

            // Get the user
            User user;
            if ((user = TryGetUser(conn)) == null || user.Map == null)
                return;

            // Get the provider
            var npc = user.Map.GetDynamicEntity<Character>(providerIndex);
            var provider = npc as IQuestProvider<User>;
            if (provider == null)
                return;

            // Check the distance and state
            if (user.Map != npc.Map || user.Map == null || !npc.IsAlive || npc.IsDisposed ||
                user.GetDistance(npc) > GameData.MaxNPCChatDistance)
                return;

            // Get the quest
            var quest = _questManager.GetQuest(questID);
            if (quest == null)
                return;

            // Ensure this provider even provides this quest
            if (!provider.Quests.Contains(quest))
                return;

            // If the user already has the quest, try to turn it in
            if (user.ActiveQuests.Contains(quest))
            {
                // Quest already started, try to turn in
                var success = user.TryFinishQuest(quest);
                using (var pw = ServerPacket.AcceptOrTurnInQuestReply(questID, success, false))
                {
                    user.Send(pw);
                }
            }
            else
            {
                // Quest not started yet, try to add it
                var success = user.TryAddQuest(quest);
                using (var pw = ServerPacket.AcceptOrTurnInQuestReply(questID, success, true))
                {
                    user.Send(pw);
                }
            }
        }

        [MessageHandler((byte)ClientPacketID.Attack)]
        void RecvAttack(IIPSocket conn, BitStream r)
        {
            User user;
            MapEntityIndex? targetIndex = null;

            var hasTarget = r.ReadBool();
            if (hasTarget)
                targetIndex = r.ReadMapEntityIndex();

            if ((user = TryGetUser(conn)) != null)
                user.Attack(GetTargetCharacter(user, targetIndex));
        }

        [MessageHandler((byte)ClientPacketID.BuyFromShop)]
        void RecvBuyFromShop(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadShopItemIndex();
            var amount = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ShoppingState.TryPurchase(slot, amount);
        }

        [MessageHandler((byte)ClientPacketID.Emoticon)]
        void RecvEmoticon(IIPSocket conn, BitStream r)
        {
            var emoticon = r.ReadEnum<Emoticon>();

            if (!EnumHelper<Emoticon>.IsDefined(emoticon))
            {
                const string errmsg = "Attempted to use undefined emoticon `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, emoticon);
                return;
            }

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.Emote(emoticon);
        }

        [MessageHandler((byte)ClientPacketID.DropInventoryItem)]
        void RecvDropInventoryItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.Inventory.Drop(slot);
        }

        [MessageHandler((byte)ClientPacketID.EndNPCChatDialog)]
        void RecvEndNPCChatDialog(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ChatState.EndChat();
        }

        [MessageHandler((byte)ClientPacketID.GetEquipmentItemInfo)]
        void RecvGetEquipmentItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.SendEquipmentItemStats(slot);
        }

        [MessageHandler((byte)ClientPacketID.HasQuestStartRequirements)]
        void RecvHasQuestStartRequirements(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            var quest = _questManager.GetQuest(questID);
            var hasRequirements = false;

            if (quest == null)
            {
                const string errmsg = "User `{0}` sent request for invalid quest ID `{1}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, user, questID);
            }
            else
                hasRequirements = quest.StartRequirements.HasRequirements(user);

            using (var pw = ServerPacket.HasQuestStartRequirements(questID, hasRequirements))
            {
                user.Send(pw);
            }
        }

        [MessageHandler((byte)ClientPacketID.HasQuestFinishRequirements)]
        void RecvHasQuestFinishRequirements(IIPSocket conn, BitStream r)
        {
            var questID = r.ReadQuestID();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            var quest = _questManager.GetQuest(questID);
            var hasRequirements = false;

            if (quest == null)
            {
                const string errmsg = "User `{0}` sent request for invalid quest ID `{1}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, user, questID);
            }
            else
                hasRequirements = quest.FinishRequirements.HasRequirements(user);

            using (var pw = ServerPacket.HasQuestFinishRequirements(questID, hasRequirements))
            {
                user.Send(pw);
            }
        }

        [MessageHandler((byte)ClientPacketID.GetInventoryItemInfo)]
        void RecvGetInventoryItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.SendInventoryItemStats(slot);
        }

#if !TOPDOWN
        [MessageHandler((byte)ClientPacketID.Jump)]
        void RecvJump(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && user.CanJump)
                user.Jump();
        }
#endif

        [MessageHandler((byte)ClientPacketID.Login)]
        void RecvLogin(IIPSocket conn, BitStream r)
        {
            ThreadAsserts.IsMainThread();

            var name = r.ReadString();
            var password = r.ReadString();

            Server.LoginAccount(conn, name, password);
        }

        [MessageHandler((byte)ClientPacketID.CreateNewAccountCharacter)]
        void RecvCreateNewAccountCharacter(IIPSocket conn, BitStream r)
        {
            ThreadAsserts.IsMainThread();

            var name = r.ReadString();

            // Check for a valid account
            var account = TryGetAccount(conn);
            if (account == null)
            {
                const string errmsg =
                    "Connection `{0}` tried to create a new account character but no account is associated with this connection.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, conn);
                return;
            }

            // Ensure the connection isn't logged in
            if (account.User != null)
            {
                const string errmsg = "User `{0}` tried to create a new account character while already logged in.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, account.User);
                return;
            }

            // Create the new account character
            Server.CreateAccountCharacter(conn, name);
        }

        [MessageHandler((byte)ClientPacketID.CreateNewAccount)]
        void RecvCreateNewAccount(IIPSocket conn, BitStream r)
        {
            ThreadAsserts.IsMainThread();

            var name = r.ReadString();
            var password = r.ReadString();
            var email = r.ReadString();

            // Ensure the connection isn't logged in
            var user = TryGetUser(conn, false);
            if (user != null)
            {
                const string errmsg = "User `{0}` tried to create a new account while already logged in.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, user);
                return;
            }

            // Create the new account
            Server.CreateAccount(conn, name, password, email);
        }

#if TOPDOWN
        [MessageHandler((byte)ClientPacketID.MoveDown)]
        void RecvMoveDown(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && !user.IsMovingDown)
                user.MoveDown();
        }
#endif

        [MessageHandler((byte)ClientPacketID.MoveLeft)]
        void RecvMoveLeft(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && !user.IsMovingLeft)
                user.MoveLeft();
        }

        [MessageHandler((byte)ClientPacketID.MoveRight)]
        void RecvMoveRight(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && !user.IsMovingRight)
                user.MoveRight();
        }

        [MessageHandler((byte)ClientPacketID.MoveStop)]
        void RecvMoveStop(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && user.IsMoving)
                user.StopMoving();
        }

#if TOPDOWN
        [MessageHandler((byte)ClientPacketID.MoveStopHorizontal)]
        void RecvMoveStopHorizontal(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && (user.IsMovingLeft || user.IsMovingRight))
                user.StopMovingHorizontal();
        }
#endif

#if TOPDOWN
        [MessageHandler((byte)ClientPacketID.MoveStopVertical)]
        void RecvMoveStopVertical(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && (user.IsMovingUp || user.IsMovingDown))
                user.StopMovingVertical();
        }
#endif

#if TOPDOWN
        [MessageHandler((byte)ClientPacketID.MoveUp)]
        void RecvMoveUp(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && !user.IsMovingUp)
                user.MoveUp();
        }
#endif

        [MessageHandler((byte)ClientPacketID.PickupItem)]
        void RecvPickupItem(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            // Get the item
            ItemEntityBase item;
            if (!map.TryGetDynamicEntity(mapEntityIndex, out item))
                return;

            // Ensure the distance is valid
            if (!GameData.IsValidPickupDistance(user, item))
            {
                const string errmsg = "User `{0}` failed to pick up item `{1}` - distance was too great.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, user, item);
                return;
            }

            // Pick it up
            item.Pickup(user);
        }

        [MessageHandler((byte)ClientPacketID.Ping)]
        void RecvPing(IIPSocket conn, BitStream r)
        {
            // Get the User
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            using (var pw = ServerPacket.Ping())
            {
                user.Send(pw);
            }
        }

        [MessageHandler((byte)ClientPacketID.RaiseStat)]
        void RecvRaiseStat(IIPSocket conn, BitStream r)
        {
            StatType statType;

            // Get the StatType
            try
            {
                statType = r.ReadEnum<StatType>();
            }
            catch (InvalidCastException)
            {
                const string errorMsg = "Received invaild StatType on connection `{0}`.";
                Debug.Fail(string.Format(errorMsg, conn));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errorMsg, conn);
                return;
            }

            // Get the User
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            // Raise the user's stat
            user.RaiseStat(statType);
        }

        [MessageHandler((byte)ClientPacketID.Say)]
        void RecvSay(IIPSocket conn, BitStream r)
        {
            var text = r.ReadString(GameData.MaxClientSayLength);

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            _sayHandler.Process(user, text);
        }

        [MessageHandler((byte)ClientPacketID.SelectAccountCharacter)]
        void RecvSelectAccountCharacter(IIPSocket conn, BitStream r)
        {
            ThreadAsserts.IsMainThread();

            var index = r.ReadByte();

            // Ensure the client is in a valid state to select an account character
            var userAccount = World.GetUserAccount(conn);
            if (userAccount == null)
                return;

            if (userAccount.User != null)
            {
                const string errmsg = "Account `{0}` tried to change characters while a character was already selected.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, userAccount);
                return;
            }

            // Get the CharacterID
            CharacterID characterID;
            if (!userAccount.TryGetCharacterID(index, out characterID))
            {
                const string errmsg = "Invalid account character index `{0}` given.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, characterID);
                return;
            }

            // Load the user
            userAccount.SetUser(World, characterID);

            // Send the MOTD
            var user = userAccount.User;
            if (user != null && !string.IsNullOrEmpty(Server.MOTD))
            {
                using (var pw = ServerPacket.Chat(Server.MOTD))
                {
                    user.Send(pw);
                }
            }
        }

        [MessageHandler((byte)ClientPacketID.SelectNPCChatDialogResponse)]
        void RecvSelectNPCChatDialogResponse(IIPSocket conn, BitStream r)
        {
            var responseIndex = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ChatState.EnterResponse(responseIndex);
        }

        [MessageHandler((byte)ClientPacketID.SellInventoryToShop)]
        void RecvSellInventoryToShop(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            var amount = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ShoppingState.TrySellInventory(slot, amount);
        }

        [MessageHandler((byte)ClientPacketID.SetUDPPort)]
        void RecvSetUDPPort(IIPSocket conn, BitStream r)
        {
            var remotePort = r.ReadUShort();
            conn.SetRemoteUnreliablePort(remotePort);
        }

        [MessageHandler((byte)ClientPacketID.StartNPCChatDialog)]
        void RecvStartNPCChatDialog(IIPSocket conn, BitStream r)
        {
            var npcIndex = r.ReadMapEntityIndex();
            var forceSkipQuestDialog = r.ReadBool();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            var npc = map.GetDynamicEntity<NPC>(npcIndex);
            if (npc == null)
                return;

            // Check the distance and state
            if (user.Map != npc.Map || user.Map == null || !npc.IsAlive || npc.IsDisposed ||
                user.GetDistance(npc) > GameData.MaxNPCChatDistance)
                return;

            // If the NPC provides any quests that this user can do or turn in, show that instead
            if (!forceSkipQuestDialog && !npc.Quests.IsEmpty())
            {
                IQuest<User>[] availableQuests;
                IQuest<User>[] turnInQuests;
                QuestHelper.GetAvailableQuests(user, npc, out availableQuests, out turnInQuests);

                if (availableQuests.Length > 0 || turnInQuests.Length > 0)
                {
                    using (
                        var pw = ServerPacket.StartQuestChatDialog(npcIndex, availableQuests.Select(x => x.QuestID),
                                                                   turnInQuests.Select(x => x.QuestID)))
                    {
                        user.Send(pw);
                    }
                    return;
                }
            }

            // Force-skipped the quest dialog, or there was no available quests, so start the chat dialog
            user.ChatState.StartChat(npc);
        }

        [MessageHandler((byte)ClientPacketID.SwapInventorySlots)]
        void RecvSwapInventorySlots(IIPSocket conn, BitStream r)
        {
            var a = r.ReadInventorySlot();
            var b = r.ReadInventorySlot();

            var user = TryGetUser(conn);
            if (user == null)
                return;

            user.Inventory.SwapSlots(a, b);
        }

        [MessageHandler((byte)ClientPacketID.StartShopping)]
        void RecvStartShopping(IIPSocket conn, BitStream r)
        {
            var entityIndex = r.ReadMapEntityIndex();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            var shopkeeper = map.GetDynamicEntity<Character>(entityIndex);
            if (shopkeeper == null)
                return;

            user.ShoppingState.TryStartShopping(shopkeeper);
        }

        [MessageHandler((byte)ClientPacketID.UnequipItem)]
        void RecvUnequipItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.Equipped.RemoveAt(slot);
        }

        [MessageHandler((byte)ClientPacketID.UseInventoryItem)]
        void RecvUseInventoryItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.UseInventoryItem(slot);
        }

        [MessageHandler((byte)ClientPacketID.UseSkill)]
        void RecvUseSkill(IIPSocket conn, BitStream r)
        {
            SkillType skillType;
            MapEntityIndex? targetIndex = null;

            try
            {
                skillType = r.ReadEnum<SkillType>();
            }
            catch (InvalidCastException)
            {
                const string errmsg = "Failed to read SkillType from stream.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                return;
            }

            var hasTarget = r.ReadBool();
            if (hasTarget)
                targetIndex = r.ReadMapEntityIndex();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.UseSkill(skillType, GetTargetCharacter(user, targetIndex));
        }

        [MessageHandler((byte)ClientPacketID.UseWorld)]
        void RecvUseWorld(IIPSocket conn, BitStream r)
        {
            var useEntityIndex = r.ReadMapEntityIndex();

            // Get the map and user
            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            // Grab the DynamicEntity to use
            var useEntity = map.GetDynamicEntity(useEntityIndex);
            if (useEntity == null)
            {
                const string errmsg = "UseEntity received but usedEntityIndex `{0}` is not a valid DynamicEntity.";
                Debug.Fail(string.Format(errmsg, useEntityIndex));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, useEntityIndex);
                return;
            }

            // Ensure the used DynamicEntity is even usable
            var asUsable = useEntity as IUsableEntity;
            if (asUsable == null)
            {
                const string errmsg =
                    "UseEntity received but useByIndex `{0}` refers to DynamicEntity `{1}` which does " +
                    "not implement IUsableEntity.";
                Debug.Fail(string.Format(errmsg, useEntityIndex, useEntity));
                if (log.IsErrorEnabled)
                    log.WarnFormat(errmsg, useEntityIndex, useEntity);
                return;
            }

            // Use it
            if (asUsable.Use(user))
            {
                // Notify everyone in the map it was used
                if (asUsable.NotifyClientsOfUsage)
                {
                    using (var pw = ServerPacket.UseEntity(useEntity.MapEntityIndex, user.MapEntityIndex))
                    {
                        map.Send(pw);
                    }
                }
            }
        }

        /// <summary>
        /// A connection has been lost with a client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="conn">Connection the user was using.</param>
        void ServerSockets_Disconnected(SocketManager sender, IIPSocket conn)
        {
            lock (_disconnectedSockets)
            {
                _disconnectedSockets.Enqueue(conn);
            }
        }

        static UserAccount TryGetAccount(IIPSocket conn)
        {
            // Check for a valid conn
            if (conn == null)
            {
                const string errmsg = "conn is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return null;
            }

            return conn.Tag as UserAccount;
        }

        static bool TryGetMap(Character user, out Map map)
        {
            // Check for a valid user
            if (user == null)
            {
                const string errmsg = "user is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                map = null;
                return false;
            }

            // Get the map
            map = user.Map;
            if (map == null)
            {
                // Invalid map
                const string errorMsg = "Received UseWorld from user `{0}`, but their map is null.";
                Debug.Fail(string.Format(errorMsg, user));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errorMsg, user);
                return false;
            }

            // Valid map
            return true;
        }

        /// <summary>
        /// Tries to get the <see cref="Map"/> and <see cref="User"/> from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="conn">The <see cref="IIPSocket"/> to get the <see cref="Map"/> and <see cref="User"/> from.</param>
        /// <param name="user">When this method returns true, contains the <see cref="User"/>.</param>
        /// <param name="map">When this method returns true, contains the <see cref="Map"/>.</param>
        /// <returns>True if the <paramref name="user"/> and <paramref name="map"/> were successfully found; otherwise
        /// false.</returns>
        bool TryGetMap(IIPSocket conn, out User user, out Map map)
        {
            if ((user = TryGetUser(conn)) == null)
            {
                map = null;
                return false;
            }

            return TryGetMap(user, out map);
        }

        /// <summary>
        /// Tries to get the <see cref="User"/> from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="conn">The <see cref="IIPSocket"/> to get the <see cref="User"/> from.</param>
        /// <param name="errorOnFailure">If true, an error will be printed if the <see cref="User"/> for the
        /// <paramref name="conn"/> could not be found. This should only be false when it is expected that
        /// there will be no <see cref="User"/>.</param>
        /// <returns>The <see cref="User"/> from the <paramref name="conn"/>, or null if no <see cref="User"/>
        /// could be found.</returns>
        User TryGetUser(IIPSocket conn, bool errorOnFailure = true)
        {
            // Check for a valid connection
            if (conn == null)
            {
                const string errmsg = "conn is null.";
                Debug.Fail(errmsg);
                log.Warn(errmsg);
                return null;
            }

            // Get the user
            var user = World.GetUser(conn, errorOnFailure);

            // Check for a valid user
            if (user == null)
            {
                if (errorOnFailure)
                {
                    const string errmsg = "user is null.";
                    Debug.Fail(errmsg);
                    log.Error(errmsg);
                }
            }

            return user;
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public TickCount GetTime()
        {
            return Server.GetTime();
        }

        #endregion

        #region IMessageProcessor Members

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding MessageProcessors.
        /// </summary>
        /// <param name="recvData">List of SocketReceiveData to process.</param>
        public void Process(IEnumerable<SocketReceiveData> recvData)
        {
            ThreadAsserts.IsMainThread();
            _ppManager.Process(recvData);
        }

        #endregion
    }
}