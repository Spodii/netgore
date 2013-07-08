using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.PeerTrading;
using DemoGame.Server.Properties;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using SFML.Graphics;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Features.Quests;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    partial class ServerPacketHandler : IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly QuestManager _questManager = QuestManager.Instance;

        readonly SayHandler _sayHandler;
        readonly Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPacketHandler"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is <c>null</c>.</exception>
        public ServerPacketHandler(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;

            _sayHandler = new SayHandler(server);
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

        [MessageHandler((uint)ClientPacketID.AcceptOrTurnInQuest)]
        void RecvAcceptOrTurnInQuest(IIPSocket conn, BitStream r)
        {
            var providerIndex = r.ReadMapEntityIndex();
            var questID = r.ReadQuestID();

            // Get the user
            User user;
            if ((user = TryGetUser(conn)) == null || user.Map == null)
                return;

            if (user.IsPeerTrading)
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
                    user.Send(pw, ServerMessageType.GUI);
                }
            }
            else
            {
                // Quest not started yet, try to add it
                var success = user.TryAddQuest(quest);
                using (var pw = ServerPacket.AcceptOrTurnInQuestReply(questID, success, true))
                {
                    user.Send(pw, ServerMessageType.GUI);
                }
            }
        }

        [MessageHandler((uint)ClientPacketID.Attack)]
        void RecvAttack(IIPSocket conn, BitStream r)
        {
            User user;
            MapEntityIndex? targetIndex = null;

            var hasTarget = r.ReadBool();
            if (hasTarget)
                targetIndex = r.ReadMapEntityIndex();

            if ((user = TryGetUser(conn)) != null)
            {
                if (user.IsPeerTrading)
                    return;

                user.Attack(GetTargetCharacter(user, targetIndex));
            }
        }

        [MessageHandler((uint)ClientPacketID.BuyFromShop)]
        void RecvBuyFromShop(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadShopItemIndex();
            var amount = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            if (user.IsPeerTrading)
                return;

            user.ShoppingState.TryPurchase(slot, amount);
        }

        [MessageHandler((uint)ClientPacketID.ClickWarp)]
        void RecvClickWarp(IIPSocket conn, BitStream r)
        {
            var worldPos = r.ReadVector2();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            var map = user.Map;
            if (map == null)
                return;

            worldPos = worldPos.Max(Vector2.Zero).Min(map.Size);

            user.Teleport(user.Map, worldPos);
        }

        [MessageHandler((uint)ClientPacketID.CreateNewAccount)]
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

        [MessageHandler((uint)ClientPacketID.CreateNewAccountCharacter)]
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

        [MessageHandler((uint)ClientPacketID.DeleteAccountCharacter)]
        void RecvDeleteAccountCharacter(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadByte();

            // Check for a valid account
            var account = TryGetAccount(conn);
            if (account == null)
            {
                const string errmsg =
                    "Connection `{0}` tried to delete account character but no account is associated with this connection.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, conn);
                return;
            }

            // Ensure the connection isn't logged in
            if (account.User != null)
            {
                const string errmsg = "User `{0}` tried to delete account character while already logged in.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, account.User);
                return;
            }

            // Get the character ID
            CharacterID charID;
            if (!account.TryGetCharacterID(slot, out charID))
            {
                const string errmsg = "Could not delete character in slot `{0}` - no character exists.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return;
            }

            // Delete
            var q = DbController.GetQuery<DeleteUserCharacterQuery>();
            q.Execute(charID);

            // Update
            account.LoadCharacterIDs();
            account.SendAccountCharacterInfos();
        }

        [MessageHandler((uint)ClientPacketID.DropInventoryItem)]
        void RecvDropInventoryItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            var amount = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            if (user.IsPeerTrading)
                return;

            if (amount < 1)
                return;

            user.Inventory.Drop(slot, amount);
        }

        [MessageHandler((uint)ClientPacketID.Emoticon)]
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

        [MessageHandler((uint)ClientPacketID.EndNPCChatDialog)]
        void RecvEndNPCChatDialog(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ChatState.EndChat();
        }

        [MessageHandler((uint)ClientPacketID.GetEquipmentItemInfo)]
        void RecvGetEquipmentItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.SendEquipmentItemStats(slot);
        }

        [MessageHandler((uint)ClientPacketID.GetInventoryItemInfo)]
        void RecvGetInventoryItemInfo(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.SendInventoryItemStats(slot);
        }

        [MessageHandler((uint)ClientPacketID.HasQuestFinishRequirements)]
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
            {
                if (quest.FinishRequirements != null)
                    hasRequirements = quest.FinishRequirements.HasRequirements(user);
            }

            using (var pw = ServerPacket.HasQuestFinishRequirements(questID, hasRequirements))
            {
                user.Send(pw, ServerMessageType.GUI);
            }
        }

        [MessageHandler((uint)ClientPacketID.HasQuestStartRequirements)]
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
            {
                if (quest.StartRequirements != null)
                    hasRequirements = quest.StartRequirements.HasRequirements(user);
            }

            using (var pw = ServerPacket.HasQuestStartRequirements(questID, hasRequirements))
            {
                user.Send(pw, ServerMessageType.GUI);
            }
        }

        [MessageHandler((uint)ClientPacketID.Login)]
        void RecvLogin(IIPSocket conn, BitStream r)
        {
            ThreadAsserts.IsMainThread();

            var name = r.ReadString();
            var password = r.ReadString();

            Server.LoginAccount(conn, name, password);
        }

        [MessageHandler((uint)ClientPacketID.MoveLeft)]
        void RecvMoveLeft(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && !user.IsMovingLeft)
            {
                if (user.IsPeerTrading)
                    return;

                user.MoveLeft();
            }
        }

        [MessageHandler((uint)ClientPacketID.MoveRight)]
        void RecvMoveRight(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && !user.IsMovingRight)
            {
                if (user.IsPeerTrading)
                    return;

                user.MoveRight();
            }
        }

        [MessageHandler((uint)ClientPacketID.MoveStop)]
        void RecvMoveStop(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && user.IsMoving)
                user.StopMoving();
        }

        [MessageHandler((uint)ClientPacketID.PeerTradeEvent)]
        void RecvPeerTradeEvent(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            ServerPeerTradeInfoHandler.Instance.Read(user, r);
        }

        [MessageHandler((uint)ClientPacketID.PickupItem)]
        void RecvPickupItem(IIPSocket conn, BitStream r)
        {
            var mapEntityIndex = r.ReadMapEntityIndex();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            // Get the item
            var item = map.GetDynamicEntity<ItemEntityBase>(mapEntityIndex);
            if (item == null)
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

        [MessageHandler((uint)ClientPacketID.RaiseStat)]
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

        [MessageHandler((uint)ClientPacketID.RequestDynamicEntity)]
        void RecvRequestMapEntityIndex(IIPSocket conn, BitStream r)
        {
            var index = r.ReadMapEntityIndex();

            // Get the user and their map
            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            Map map;
            if (!TryGetMap(user, out map))
                return;

            // Get the DynamicEntity
            var de = map.GetDynamicEntity(index);

            if (de == null)
            {
                // The DynamicEntity for the index was null, so tell the client to delete whatever is at that index
                using (var pw = ServerPacket.RemoveDynamicEntity(index))
                {
                    conn.Send(pw, ServerMessageType.Map);
                }
            }
            else
            {
                // A DynamicEntity does exist at that index, so tell the client to create it
                using (var pw = ServerPacket.CreateDynamicEntity(de))
                {
                    conn.Send(pw, ServerMessageType.Map);
                }
            }
        }

        [MessageHandler((uint)ClientPacketID.Say)]
        void RecvSay(IIPSocket conn, BitStream r)
        {
            var text = r.ReadString(GameData.MaxClientSayLength);

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            _sayHandler.Process(user, text);
        }

        [MessageHandler((uint)ClientPacketID.SelectAccountCharacter)]
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


            var user = userAccount.User;
            if (user != null)
            {
                // Send the MOTD
                if (!string.IsNullOrEmpty(ServerSettings.Default.MOTD))
                {
                    using (var pw = ServerPacket.Chat(ServerSettings.Default.MOTD))
                    {
                        user.Send(pw, ServerMessageType.GUIChat);
                    }
                }

                // Send a notification to the world that the user joined
                var param = new object[] {user.Name};
                World.Send(GameMessage.UserJoinedWorld, ServerMessageType.GUIChat, param);

            }
        }

        [MessageHandler((uint)ClientPacketID.SelectNPCChatDialogResponse)]
        void RecvSelectNPCChatDialogResponse(IIPSocket conn, BitStream r)
        {
            var responseIndex = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ChatState.EnterResponse(responseIndex);
        }

        [MessageHandler((uint)ClientPacketID.SellInventoryToShop)]
        void RecvSellInventoryToShop(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();
            var amount = r.ReadByte();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.ShoppingState.TrySellInventory(slot, amount);
        }

        [MessageHandler((uint)ClientPacketID.StartNPCChatDialog)]
        void RecvStartNPCChatDialog(IIPSocket conn, BitStream r)
        {
            var npcIndex = r.ReadMapEntityIndex();
            var forceSkipQuestDialog = r.ReadBool();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            if (user.IsPeerTrading)
                return;

            var npc = map.GetDynamicEntity<NPC>(npcIndex);
            if (npc == null)
                return;

            // Check the distance and state
            if (user.Map != npc.Map || user.Map == null || !npc.IsAlive || npc.IsDisposed || user.GetDistance(npc) > GameData.MaxNPCChatDistance)
                return;

            // If the NPC provides any quests that this user can do or turn in, show that instead
            if (!forceSkipQuestDialog && !npc.Quests.IsEmpty())
            {
                IQuest<User>[] availableQuests;
                IQuest<User>[] turnInQuests;
                QuestHelper.GetAvailableQuests(user, npc, out availableQuests, out turnInQuests);

                if (availableQuests.Length > 0 || turnInQuests.Length > 0)
                {
                    using (var pw = ServerPacket.StartQuestChatDialog(npcIndex, availableQuests.Select(x => x.QuestID), turnInQuests.Select(x => x.QuestID)))
                    {
                        user.Send(pw, ServerMessageType.GUI);
                    }
                    return;
                }
            }

            // Force-skipped the quest dialog, or there was no available quests, so start the chat dialog
            user.ChatState.StartChat(npc);
        }

        [MessageHandler((uint)ClientPacketID.StartShopping)]
        void RecvStartShopping(IIPSocket conn, BitStream r)
        {
            var entityIndex = r.ReadMapEntityIndex();

            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            if (user.IsPeerTrading)
                return;

            var shopkeeper = map.GetDynamicEntity<Character>(entityIndex);
            if (shopkeeper == null)
                return;

            user.ShoppingState.TryStartShopping(shopkeeper);
        }

        [MessageHandler((uint)ClientPacketID.SwapInventorySlots)]
        void RecvSwapInventorySlots(IIPSocket conn, BitStream r)
        {
            var a = r.ReadInventorySlot();
            var b = r.ReadInventorySlot();

            var user = TryGetUser(conn);
            if (user == null)
                return;

            user.Inventory.SwapSlots(a, b);
        }

        [MessageHandler((uint)ClientPacketID.SynchronizeGameTime)]
        void RecvSynchronizeGameTime(IIPSocket conn, BitStream r)
        {
            // Just reply immediately with the current game time
            using (var pw = ServerPacket.SetGameTime(DateTime.Now))
            {
                conn.Send(pw, ServerMessageType.GUI);
            }
        }

        [MessageHandler((uint)ClientPacketID.UnequipItem)]
        void RecvUnequipItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadEnum<EquipmentSlot>();

            User user;
            if ((user = TryGetUser(conn)) != null)
                user.Equipped.RemoveAt(slot);
        }

        [MessageHandler((uint)ClientPacketID.UseInventoryItem)]
        void RecvUseInventoryItem(IIPSocket conn, BitStream r)
        {
            var slot = r.ReadInventorySlot();

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            user.UseInventoryItem(slot);
        }

        [MessageHandler((uint)ClientPacketID.UseSkill)]
        void RecvUseSkill(IIPSocket conn, BitStream r)
        {
            SkillType skillType;
            MapEntityIndex? targetIndex = null;

            // Get the SkillType to use
            try
            {
                skillType = r.ReadEnum<SkillType>();
            }
            catch (InvalidCastException)
            {
                const string errmsg = "Failed to read SkillType from stream.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                r.ReadBool();
                return;
            }

            // Check for a target
            var hasTarget = r.ReadBool();
            if (hasTarget)
                targetIndex = r.ReadMapEntityIndex();

            // Get the user
            User user;
            if ((user = TryGetUser(conn)) != null)
            {
                // Check that they know the skill
                if (!user.KnownSkills.Knows(skillType))
                    user.Send(GameMessage.SkillNotKnown, ServerMessageType.GUIChat);
                else
                {
                    // Use the skill
                    user.UseSkill(skillType, GetTargetCharacter(user, targetIndex));
                }
            }
        }

        [MessageHandler((uint)ClientPacketID.UseWorld)]
        void RecvUseWorld(IIPSocket conn, BitStream r)
        {
            var useEntityIndex = r.ReadMapEntityIndex();

            // Get the map and user
            User user;
            Map map;
            if (!TryGetMap(conn, out user, out map))
                return;

            if (user.IsPeerTrading)
                return;

            if (!user.IsAlive)
            {
                const string errmsg = "User `{0}` tried to use world entity while dead.";
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, user);
                return;
            }

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
                        map.Send(pw, ServerMessageType.Map);
                    }
                }
            }
        }

        [MessageHandler((uint)ClientPacketID.GetFriends)]
        void RecvGetFriends(IIPSocket conn, BitStream r)
        {
            var account = TryGetAccount(conn);
            if (account == null)
                return;

            User user;
            if ((user = TryGetUser(conn)) == null)
                return;

            string FriendsString = account.Friends;

            List<string> FriendsList = FriendsString.Split(',').ToList<string>();
            string OnlineFriendsString = "";
            string FriendsMap = "";

            var OnlineMembers = Server.World.GetUsers();

            foreach (var Member in OnlineMembers)
            {
                if (FriendsList.Contains(Member.Name))
                {
                    OnlineFriendsString += Member.Name + ",";
                    var parentMap = World.GetMap(Member.Map.ParentMapID);
                    FriendsMap += parentMap.Name + ",";
                }
            }

            using (var pw = ServerPacket.ReceiveFriends(OnlineFriendsString, FriendsMap, FriendsString))
            {
                user.Send(pw, ServerMessageType.GUI);
            }
        }

        [MessageHandler((uint)ClientPacketID.SaveFriends)]
        void RecvSaveFriends(IIPSocket conn, BitStream r)
        {
            var account = TryGetAccount(conn);
            if (account == null)
                return;

            account.SetFriends(r.ReadString());
        }


        [MessageHandler((uint)ClientPacketID.SendPrivateMessage)]
        void RecvSendPrivateMessage(IIPSocket conn, BitStream r)
        {

            string TargetName = r.ReadString();
            string Text = r.ReadString();

            // Get the user to send the message to
            User TargetChar = World.FindUser(TargetName);

            string PrivateMessage = TargetName + " Says: " + Text;

            using (var pw = ServerPacket.ReceivePrivateMessage(PrivateMessage))
            {
                TargetChar.Send(pw, ServerMessageType.GUIChat);
            }
        }

        [MessageHandler((uint)ClientPacketID.GetOnlineUsers)]
        void RecvGetOnlineUsers(IIPSocket conn, BitStream r)
        {
            User myUser;
            if ((myUser = TryGetUser(conn)) == null)
                return;

            var allUsers = Server.World.GetUsers();
            var orderedUsers = allUsers.OrderBy(x => !x.Permissions.IsSet(UserPermissions.None)).ThenBy(x => x.Name).ToList();
            string onlineUsers = string.Empty;

            foreach (var user in orderedUsers)
            {
                if (myUser == user)
                    continue;

                onlineUsers += user.Name + ";";
            }

            using (var pw = ServerPacket.ReceiveAllUsers(onlineUsers))
            {
                myUser.Send(pw, ServerMessageType.GUI);
            }
        }

        static IUserAccount TryGetAccount(IIPSocket conn)
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

            return conn.Tag as IUserAccount;
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
                if (log.IsWarnEnabled)
                    log.WarnFormat(errorMsg, user);
                Debug.Fail(string.Format(errorMsg, user));
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
        static bool TryGetMap(IIPSocket conn, out User user, out Map map)
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
        static User TryGetUser(IIPSocket conn, bool errorOnFailure = true)
        {
            // Check for a valid connection
            if (conn == null)
            {
                const string errmsg = "conn is null.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
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
                    if (log.IsErrorEnabled)
                        log.Error(errmsg);
                    Debug.Fail(errmsg);
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
    }
}