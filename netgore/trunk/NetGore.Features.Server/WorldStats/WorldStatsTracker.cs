using System;
using System.Linq;
using NetGore.Db;
using NetGore.Db.ClassCreator;
using NetGore.Features.Shops;

namespace NetGore.Features.WorldStats
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IWorldStatsTracker{T,U,V}"/> that can handle the default world
    /// statistics tracking.
    /// </summary>
    /// <typeparam name="TUser">The type of user character.</typeparam>
    /// <typeparam name="TNPC">The type of NPC character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    public abstract class WorldStatsTracker<TUser, TNPC, TItem> : IWorldStatsTracker<TUser, TNPC, TItem> where TUser : class
                                                                                                             where TNPC : class
                                                                                                             where TItem : class
    {
        readonly IDbController _dbController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsTracker{TUser, TNPC, TItem}"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> used to grab the database query objects.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbController"/> is null.</exception>
        protected WorldStatsTracker(IDbController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this object instance to perform queries to the database.
        /// </summary>
        public virtual IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// When overridden in the derived class, adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        protected abstract void InternalAddNPCKillUser(TNPC npc, TUser user);

        /// <summary>
        /// When overridden in the derived class, adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        protected abstract void InternalAddUserConsumeItem(TUser user, TItem item);

        /// <summary>
        /// When overridden in the derived class, adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        protected abstract void InternalAddUserKillNPC(TUser user, TNPC npc);

        /// <summary>
        /// When overridden in the derived class, adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        protected abstract void InternalAddUserLevel(TUser user);

        /// <summary>
        /// When overridden in the derived class, adds when a user buys an item from a shop.
        /// </summary>
        /// <param name="user">The user that bought from a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was purchased.</param>
        /// <param name="amount">How many units of the item was purchased.</param>
        /// <param name="cost">How much the user bought the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        protected abstract void InternalAddUserShopBuyItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID);

        /// <summary>
        /// When overridden in the derived class, adds when a user sells an item to a shop.
        /// </summary>
        /// <param name="user">The user that sold to a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was sold.</param>
        /// <param name="amount">How many units of the item was sold.</param>
        /// <param name="cost">How much the user sold the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        protected abstract void InternalAddUserShopSellItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID);

        #region IWorldStatsTracker<TUser,TNPC,TItem> Members

        /// <summary>
        /// Adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        public void AddNPCKillUser(TNPC npc, TUser user)
        {
            if (npc == null)
                return;
            if (user == null)
                return;

            InternalAddNPCKillUser(npc, user);
        }

        /// <summary>
        /// Adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        public void AddUserConsumeItem(TUser user, TItem item)
        {
            if (user == null)
                return;
            if (item == null)
                return;

            InternalAddUserConsumeItem(user, item);
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> for the current time.
        /// </summary>
        /// <returns>The <see cref="DateTime"/> for the current time.</returns>
        protected virtual DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        public void AddUserKillNPC(TUser user, TNPC npc)
        {
            if (user == null)
                return;
            if (npc == null)
                return;

            InternalAddUserKillNPC(user, npc);
        }

        /// <summary>
        /// Adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        public void AddUserLevel(TUser user)
        {
            if (user == null)
                return;

            InternalAddUserLevel(user);
        }

        /// <summary>
        /// Adds when a user buys an item from a shop.
        /// </summary>
        /// <param name="user">The user that bought from a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was purchased.</param>
        /// <param name="amount">How many units of the item was purchased.</param>
        /// <param name="cost">How much the user bought the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        public void AddUserShopBuyItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID)
        {
            if (user == null)
                return;
            if (amount <= 0)
                return;

            InternalAddUserLevel(user);
        }

        /// <summary>
        /// Adds when a user sells an item to a shop.
        /// </summary>
        /// <param name="user">The user that sold to a shop.</param>
        /// <param name="itemTemplateID">The template ID of the item that was sold.</param>
        /// <param name="amount">How many units of the item was sold.</param>
        /// <param name="cost">How much the user sold the items for. When the amount is greater than one, this includes
        /// the cost of all the items together, not a single item. That is, the cost of the transaction as a whole.</param>
        /// <param name="shopID">The ID of the shop the transaction took place at.</param>
        public void AddUserShopSellItem(TUser user, int? itemTemplateID, byte amount, int cost, ShopID shopID)
        {
            if (user == null)
                return;
            if (amount <= 0)
                return;

            InternalAddUserLevel(user);
        }

        #endregion
    }
}