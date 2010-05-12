using System.Linq;
using NetGore.Features.Shops;

namespace NetGore.Features.WorldStats
{
    /// <summary>
    /// Interface for a class that is used to track world statistics.
    /// </summary>
    /// <typeparam name="TUser">The type of user character.</typeparam>
    /// <typeparam name="TNPC">The type of NPC character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    public interface IWorldStatsTracker<in TUser, in TNPC, in TItem> where TUser : class where TNPC : class where TItem : class
    {
        /// <summary>
        /// Adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        void AddNPCKillUser(TNPC npc, TUser user);

        /// <summary>
        /// Adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        void AddUserConsumeItem(TUser user, TItem item);

        /// <summary>
        /// Adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        void AddUserKillNPC(TUser user, TNPC npc);

        /// <summary>
        /// Adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        void AddUserLevel(TUser user);

        /// <summary>
        /// Adds when a user buys an item from a shop.
        /// </summary>
        /// <param name="user">The user that bought from a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was purchased.</param>
        /// <param name="amount">How many units of the item was purchased.</param>
        /// <param name="cost">How much the user bought the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        void AddUserShopBuyItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID);

        /// <summary>
        /// Adds when a user sells an item to a shop.
        /// </summary>
        /// <param name="user">The user that sold to a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was sold.</param>
        /// <param name="amount">How many units of the item was sold.</param>
        /// <param name="cost">How much the user sold the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        void AddUserShopSellItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID);

        /// <summary>
        /// Updates the statistics that are time-based.
        /// </summary>
        void Update();
    }
}