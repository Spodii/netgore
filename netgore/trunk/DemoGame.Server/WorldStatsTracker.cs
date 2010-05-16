﻿using System;
using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.Guilds;
using NetGore.Features.Shops;
using NetGore.Features.WorldStats;
using NetGore.Network;

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

        readonly InsertWorldStatsGuildUserChangeQuery _guildUserChangeQuery;
        readonly InsertWorldStatsNetworkQuery _networkQuery;
        readonly InsertWorldStatsNPCKillUserQuery _npcKillUserQuery;
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
        }

        /// <summary>
        /// Gets the <see cref="IWorldStatsTracker{T,U,V}"/> instance.
        /// </summary>
        public static IWorldStatsTracker<User, NPC, ItemEntity> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        protected override void InternalAddNPCKillUser(NPC npc, User user)
        {
            if (npc.Map == null)
                return;

            var args = new WorldStatsNpcKillUserTable(when: Now(), mapID: npc.Map.ID, npcTemplateId: npc.CharacterTemplateID,
                                                      npcX: (ushort)npc.Position.X, npcY: (ushort)npc.Position.Y, userId: user.ID,
                                                      userLevel: user.Level, userX: (ushort)user.Position.X,
                                                      userY: (ushort)user.Position.Y);

            _npcKillUserQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        protected override void InternalAddUserConsumeItem(User user, ItemEntity item)
        {
            if (user.Map == null)
                return;

            if (item.Type != ItemType.UseOnce)
                return;

            var itemTemplate = item.ItemTemplateID;
            if (!itemTemplate.HasValue)
                return;

            var args = new WorldStatsUserConsumeItemTable(when: Now(), itemTemplateID: itemTemplate.Value, mapID: user.Map.ID,
                                                          userId: user.ID, x: (ushort)user.Position.X, y: (ushort)user.Position.Y);

            _userConsumeItemQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        protected override void InternalAddUserGuildChange(User user, int? guildID)
        {
            if (user.Map == null)
                return;

            var args = new WorldStatsGuildUserChangeTable(when: Now(), guildID: (GuildID?)guildID, userId: user.ID);

            _guildUserChangeQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        protected override void InternalAddUserKillNPC(User user, NPC npc)
        {
            if (user.Map == null)
                return;

            var args = new WorldStatsUserKillNpcTable(when: Now(), mapID: user.Map.ID, npcTemplateId: npc.CharacterTemplateID,
                                                      npcX: (ushort)npc.Position.X, npcY: (ushort)npc.Position.Y, userId: user.ID,
                                                      userLevel: user.Level, userX: (ushort)user.Position.X,
                                                      userY: (ushort)user.Position.Y);

            _userKillNPCQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        protected override void InternalAddUserLevel(User user)
        {
            var map = user.Map != null ? user.Map.ID : (MapID?)null;

            var args = new WorldStatsUserLevelTable(when: Now(), characterID: user.ID, level: user.Level, mapID: map,
                                                    x: (ushort)user.Position.X, y: (ushort)user.Position.Y);

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
            if (user.Map == null)
                return;

            var args = new WorldStatsUserShoppingTable(saleType: 0, amount: amount, characterID: user.ID, cost: cost,
                                                       itemTemplateID: (ItemTemplateID?)itemTemplateID, mapID: user.Map.ID,
                                                       shopID: shopID, when: Now(), x: (ushort)user.Position.X,
                                                       y: (ushort)user.Position.Y);

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
            if (user.Map == null)
                return;

            var args = new WorldStatsUserShoppingTable(saleType: 1, amount: amount, characterID: user.ID, cost: cost,
                                                       itemTemplateID: (ItemTemplateID?)itemTemplateID, mapID: user.Map.ID,
                                                       shopID: shopID, when: Now(), x: (ushort)user.Position.X,
                                                       y: (ushort)user.Position.Y);

            _userShoppingQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, logs the <see cref="NetStats"/> to the database.
        /// </summary>
        /// <param name="netStats">The <see cref="NetStats"/> containing the statistics to log. This contains the difference
        /// in the <see cref="NetStats"/> from the last call.</param>
        protected override void LogNetStats(NetStats netStats)
        {
            var args = new WorldStatsNetworkTable(when: Now(), connections: (uint)netStats.Connections,
                                                  tcpRecv: (uint)netStats.TCPRecv, tcpRecvs: (uint)netStats.TCPReceives,
                                                  tcpSends: (uint)netStats.TCPSends, tcpSent: (uint)netStats.TCPSent,
                                                  udpRecv: (uint)netStats.UDPRecv, udpRecvs: (uint)netStats.UDPReceives,
                                                  udpSends: (uint)netStats.UDPSends, udpSent: (uint)netStats.UDPSent);

            _networkQuery.Execute(args);
        }
    }
}