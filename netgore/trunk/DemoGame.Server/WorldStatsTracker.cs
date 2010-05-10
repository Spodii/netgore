using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.Shops;
using NetGore.Features.WorldStats;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides support for tracking the world statistics and events.
    /// </summary>
    public class WorldStatsTracker : WorldStatsTracker<User, NPC, ItemEntity>
    {
        static readonly IWorldStatsTracker<User, NPC, ItemEntity> _instance;

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
        WorldStatsTracker(IDbController dbController) : base(dbController)
        {
            // Locally cache an instance of all the queries we will be using
            _npcKillUserQuery = dbController.GetQuery<InsertWorldStatsNPCKillUserQuery>();
            _userConsumeItemQuery = dbController.GetQuery<InsertWorldStatsUserConsumeItemQuery>();
            _userKillNPCQuery = dbController.GetQuery<InsertWorldStatsUserKillNpcQuery>();
            _userLevelQuery = dbController.GetQuery<InsertWorldStatsUserLevelQuery>();
            _userShoppingQuery = dbController.GetQuery<InsertWorldStatsUserShoppingQuery>();
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

            var args = new WorldStatsNpcKillUserTable(npc.Map.ID, npc.CharacterTemplateID, (ushort)npc.Position.X,
                                                      (ushort)npc.Position.Y, Now(), user.ID, user.Level, (ushort)user.Position.X,
                                                      (ushort)user.Position.Y);

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

            var args = new WorldStatsUserConsumeItemTable(itemTemplate.Value, user.Map.ID, user.ID, Now(), (ushort)user.Position.X,
                                                          (ushort)user.Position.Y);

            _userConsumeItemQuery.Execute(args);
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

            var args = new WorldStatsUserKillNpcTable(user.Map.ID, npc.CharacterTemplateID, (ushort)npc.Position.X,
                                                      (ushort)npc.Position.Y, Now(), user.ID, user.Level, (ushort)user.Position.X,
                                                      (ushort)user.Position.Y);

            _userKillNPCQuery.Execute(args);
        }

        /// <summary>
        /// When overridden in the derived class, adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        protected override void InternalAddUserLevel(User user)
        {
            var map = user.Map != null ? user.Map.ID : (MapID?)null;

            var args = new WorldStatsUserLevelTable(user.ID, user.Level, map, Now(), (ushort)user.Position.X,
                                                    (ushort)user.Position.Y);

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

            var args = new WorldStatsUserShoppingTable(amount, user.ID, cost, (ItemTemplateID?)itemTemplateID, user.Map.ID, 0,
                                                       shopID, Now(), (ushort)user.Position.X, (ushort)user.Position.Y);

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

            var args = new WorldStatsUserShoppingTable(amount, user.ID, cost, (ItemTemplateID?)itemTemplateID, user.Map.ID, 1,
                                                       shopID, Now(), (ushort)user.Position.X, (ushort)user.Position.Y);

            _userShoppingQuery.Execute(args);
        }
    }
}