using System;
using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.Features.WorldStats;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides support for tracking the world statistics and events.
    /// </summary>
    public class WorldStatsTracker : WorldStatsTracker<User, NPC, ItemEntity>
    {
        /// <summary>
        /// How frequently to log the network stats.
        /// </summary>
        const int _logNetStatsRate = 1000 * 60 * 1; // 1 minute

        /// <summary>
        /// The <see cref="WorldStatsTracker"/> instance.
        /// </summary>
        static readonly IWorldStatsTracker<User, NPC, ItemEntity> _instance;

        readonly InsertWorldStatsCountConsumeItemQuery _countConsumeItemQuery;
        readonly InsertWorldStatsCountItemBuyQuery _countItemBuyQuery;
        readonly InsertWorldStatsCountItemCreateQuery _countItemCreateQuery;
        readonly InsertWorldStatsCountItemSellQuery _countItemSellQuery;
        readonly InsertWorldStatsCountNPCKillUserQuery _countNPCKillUserQuery;
        readonly InsertWorldStatsCountShopBuyQuery _countShopBuyQuery;
        readonly InsertWorldStatsCountShopSellQuery _countShopSellQuery;
        readonly InsertWorldStatsCountUserConsumeItemQuery _countUserConsumeItemQuery;
        readonly InsertWorldStatsCountUserKillNPCQuery _countUserKillNPCQuery;
        readonly InsertWorldStatsGuildUserChangeQuery _guildUserChangeQuery;
        readonly InsertWorldStatsNetworkQuery _networkQuery;
        readonly InsertWorldStatsNPCKillUserQuery _npcKillUserQuery;
        readonly InsertWorldStatsQuestAcceptQuery _questAcceptQuery;
        readonly InsertWorldStatsQuestCancelQuery _questCancelQuery;
        readonly InsertWorldStatsQuestCompleteQuery _questCompleteQuery;
        readonly InsertWorldStatsUserConsumeItemQuery _userConsumeItemQuery;
        readonly InsertWorldStatsUserKillNpcQuery _userKillNPCQuery;
        readonly InsertWorldStatsUserLevelQuery _userLevelQuery;
        readonly InsertWorldStatsUserShoppingQuery _userShoppingQuery;

        /// <summary>
        /// Initializes the <see cref="WorldStatsTracker"/> class.
        /// </summary>
        static WorldStatsTracker()
        {
            _instance = new WorldStatsTracker(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsTracker"/> class.
        /// </summary>
        /// <param name="dbController">The db controller.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbController"/> is null.</exception>
        WorldStatsTracker(IDbController dbController) : base(_logNetStatsRate)
        {
            // Locally cache an instance of all the queries we will be using
            _npcKillUserQuery = dbController.GetQuery<InsertWorldStatsNPCKillUserQuery>();
            _userConsumeItemQuery = dbController.GetQuery<InsertWorldStatsUserConsumeItemQuery>();
            _userKillNPCQuery = dbController.GetQuery<InsertWorldStatsUserKillNpcQuery>();
            _userLevelQuery = dbController.GetQuery<InsertWorldStatsUserLevelQuery>();
            _userShoppingQuery = dbController.GetQuery<InsertWorldStatsUserShoppingQuery>();
            _networkQuery = dbController.GetQuery<InsertWorldStatsNetworkQuery>();
            _guildUserChangeQuery = dbController.GetQuery<InsertWorldStatsGuildUserChangeQuery>();
            _questCancelQuery = dbController.GetQuery<InsertWorldStatsQuestCancelQuery>();
            _questCompleteQuery = dbController.GetQuery<InsertWorldStatsQuestCompleteQuery>();
            _questAcceptQuery = dbController.GetQuery<InsertWorldStatsQuestAcceptQuery>();

            _countConsumeItemQuery = dbController.GetQuery<InsertWorldStatsCountConsumeItemQuery>();
            _countItemBuyQuery = dbController.GetQuery<InsertWorldStatsCountItemBuyQuery>();
            _countItemCreateQuery = dbController.GetQuery<InsertWorldStatsCountItemCreateQuery>();
            _countItemSellQuery = dbController.GetQuery<InsertWorldStatsCountItemSellQuery>();
            _countNPCKillUserQuery = dbController.GetQuery<InsertWorldStatsCountNPCKillUserQuery>();
            _countShopBuyQuery = dbController.GetQuery<InsertWorldStatsCountShopBuyQuery>();
            _countShopSellQuery = dbController.GetQuery<InsertWorldStatsCountShopSellQuery>();
            _countUserConsumeItemQuery = dbController.GetQuery<InsertWorldStatsCountUserConsumeItemQuery>();
            _countUserKillNPCQuery = dbController.GetQuery<InsertWorldStatsCountUserKillNPCQuery>();
        }

        /// <summary>
        /// Gets the <see cref="IWorldStatsTracker{T,U,V}"/> instance.
        /// </summary>
        public static IWorldStatsTracker<User, NPC, ItemEntity> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item purchase counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was purchased from a shop.</param>
        /// <param name="amount">The number of items purchased.</param>
        protected override void InternalAddCountBuyItem(int itemTID, int amount)
        {
            _countItemBuyQuery.Execute(itemTID, amount);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item consumption counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was consumed.</param>
        protected override void InternalAddCountConsumeItem(int itemTID)
        {
            _countConsumeItemQuery.Execute(itemTID);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item creation counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items created.</param>
        protected override void InternalAddCountCreateItem(int itemTID, int amount)
        {
            _countItemCreateQuery.Execute(itemTID, amount);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the NPC kill user counter.
        /// </summary>
        /// <param name="npcTID">The template ID of the NPC that killed the user.</param>
        /// <param name="userID">The template ID of the user that was killed.</param>
        protected override void InternalAddCountNPCKillUser(int npcTID, int userID)
        {
            _countNPCKillUserQuery.Execute(userID, npcTID);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item sell counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items sold.</param>
        protected override void InternalAddCountSellItem(int itemTID, int amount)
        {
            _countItemSellQuery.Execute(itemTID, amount);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item being purchased from a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop that sold the item.</param>
        /// <param name="amount">The number of items the shop sold.</param>
        protected override void InternalAddCountShopBuy(int shopID, int amount)
        {
            _countShopBuyQuery.Execute(shopID, amount);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item being sold to a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop the item was sold to.</param>
        /// <param name="amount">The number of items sold to the shop.</param>
        protected override void InternalAddCountShopSell(int shopID, int amount)
        {
            _countShopSellQuery.Execute(shopID, amount);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the item consumption count.
        /// </summary>
        /// <param name="userID">The ID of the user who consumed the item.</param>
        /// <param name="itemTID">The item template ID of the item consumed.</param>
        protected override void InternalAddCountUserConsumeItem(int userID, int itemTID)
        {
            _countUserConsumeItemQuery.Execute(userID, itemTID);
        }

        /// <summary>
        /// When overridden in the derived class, adds to the user kill a NPC counter.
        /// </summary>
        /// <param name="userID">The template ID of the user that killed the NPC.</param>
        /// <param name="npcTID">The template ID of the NPC that was killed.</param>
        protected override void InternalAddCountUserKillNPC(int userID, int npcTID)
        {
            _countUserKillNPCQuery.Execute(userID, npcTID);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        protected override void InternalAddNPCKillUser(NPC npc, User user)
        {
            var mapID = (npc.Map == null ? (MapID?)null : npc.Map.ID);

            var args = new WorldStatsNpcKillUserTable(when: Now(), mapID: mapID, nPCTemplateID: npc.CharacterTemplateID,
                npcX: (ushort)npc.Position.X, npcY: (ushort)npc.Position.Y, userID: user.ID, userLevel: user.Level,
                userX: (ushort)user.Position.X, userY: (ushort)user.Position.Y, iD: 0);

            _npcKillUserQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user accepts a quest.
        /// </summary>
        /// <param name="user">The user that accepted a quest.</param>
        /// <param name="questID">The ID of the quest that the user accepted.</param>
        protected override void InternalAddQuestAccept(User user, QuestID questID)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsQuestAcceptTable(when: Now(), mapID: mapID, questID: questID, userID: user.ID,
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _questAcceptQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user cancels a quest.
        /// </summary>
        /// <param name="user">The user that canceled a quest.</param>
        /// <param name="questID">The ID of the quest that the user canceled.</param>
        protected override void InternalAddQuestCancel(User user, QuestID questID)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsQuestCancelTable(when: Now(), mapID: mapID, questID: questID, userID: user.ID,
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _questCancelQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user completes a quest.
        /// </summary>
        /// <param name="user">The user that completed a quest.</param>
        /// <param name="questID">The ID of the quest that the user completed.</param>
        protected override void InternalAddQuestComplete(User user, QuestID questID)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsQuestCompleteTable(when: Now(), mapID: mapID, questID: questID, userID: user.ID,
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _questCompleteQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        protected override void InternalAddUserConsumeItem(User user, ItemEntity item)
        {
            if (item.Type != ItemType.UseOnce)
                return;

            var itemTemplate = item.ItemTemplateID;
            if (!itemTemplate.HasValue)
                return;

            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsUserConsumeItemTable(when: Now(), itemTemplateID: itemTemplate.Value, mapID: mapID,
                userID: user.ID, x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _userConsumeItemQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        protected override void InternalAddUserGuildChange(User user, GuildID? guildID)
        {
            var args = new WorldStatsGuildUserChangeTable(when: Now(), guildID: guildID, userID: user.ID, iD: 0);

            _guildUserChangeQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        protected override void InternalAddUserKillNPC(User user, NPC npc)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsUserKillNpcTable(when: Now(), mapID: mapID, nPCTemplateID: npc.CharacterTemplateID,
                npcX: (ushort)npc.Position.X, npcY: (ushort)npc.Position.Y, userID: user.ID, userLevel: user.Level,
                userX: (ushort)user.Position.X, userY: (ushort)user.Position.Y, iD: 0);

            _userKillNPCQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        protected override void InternalAddUserLevel(User user)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsUserLevelTable(when: Now(), characterID: user.ID, level: user.Level, mapID: mapID,
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _userLevelQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user buys an item from a shop.
        /// </summary>
        /// <param name="user">The user that bought from a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was purchased.</param>
        /// <param name="amount">How many units of the item was purchased.</param>
        /// <param name="cost">How much the user bought the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        protected override void InternalAddUserShopBuyItem(User user, int? itemTemplateID, byte amount, int cost, ShopID shopID)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsUserShoppingTable(saleType: 0, amount: amount, characterID: user.ID, cost: cost,
                itemTemplateID: (ItemTemplateID?)itemTemplateID, mapID: mapID, shopID: shopID, when: Now(),
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _userShoppingQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user sells an item to a shop.
        /// </summary>
        /// <param name="user">The user that sold to a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was sold.</param>
        /// <param name="amount">How many units of the item was sold.</param>
        /// <param name="cost">How much the user sold the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        protected override void InternalAddUserShopSellItem(User user, int? itemTemplateID, byte amount, int cost, ShopID shopID)
        {
            var mapID = (user.Map == null ? (MapID?)null : user.Map.ID);

            var args = new WorldStatsUserShoppingTable(saleType: 1, amount: amount, characterID: user.ID, cost: cost,
                itemTemplateID: (ItemTemplateID?)itemTemplateID, mapID: mapID, shopID: shopID, when: Now(),
                x: (ushort)user.Position.X, y: (ushort)user.Position.Y, iD: 0);

            _userShoppingQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, logs the network statistics to the database.
        /// </summary>
        /// <param name="connections">The current number of connections.</param>
        /// <param name="recvBytes">The average bytes received per second.</param>
        /// <param name="recvPackets">The average packets received per second.</param>
        /// <param name="recvMsgs">The average messages received per second.</param>
        /// <param name="sentBytes">The average bytes sent per second.</param>
        /// <param name="sentPackets">The average packets sent per second.</param>
        /// <param name="sentMsgs">The average messages sent per second.</param>
        protected override void LogNetStats(ushort connections, uint recvBytes, uint recvPackets, uint recvMsgs, uint sentBytes,
                                            uint sentPackets, uint sentMsgs)
        {
            var args = new WorldStatsNetworkTable(iD: 0, when: Now(), connections: connections, recvBytes: recvBytes,
                recvPackets: recvPackets, recvMessages: recvMsgs, sentBytes: sentBytes, sentPackets: sentPackets,
                sentMessages: sentMsgs);
            _networkQuery.Execute(args);
        }
    }
}