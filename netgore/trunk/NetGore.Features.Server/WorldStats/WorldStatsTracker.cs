using System;
using System.Linq;
using NetGore.Features.Guilds;
using NetGore.Features.Shops;
using NetGore.Network;

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
        readonly TickCount _logNetStatsRate;

        NetStats _lastNetStatsValues = new NetStats();
        TickCount _nextLogNetStatsTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsTracker{TUser, TNPC, TItem}"/> class.
        /// </summary>
        /// <param name="logNetStatsRate">The rate in milliseconds that the <see cref="NetStats"/> information is
        /// logged to the database.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="logNetStatsRate"/> is less than
        /// or equal to zero.</exception>
        protected WorldStatsTracker(TickCount logNetStatsRate)
        {
            if (logNetStatsRate <= 0)
                throw new ArgumentOutOfRangeException("logNetStatsRate", "logNetStatsRate must be greater than or equal to zero.");

            _logNetStatsRate = logNetStatsRate;

            _nextLogNetStatsTime = TickCount.Now;
        }

        /// <summary>
        /// Gets the rate in milliseconds that the <see cref="NetStats"/> information is logged to the database.
        /// </summary>
        public TickCount LogNetStatsRate
        {
            get { return _logNetStatsRate; }
        }

        /// <summary>
        /// Gets the time to use for the next update.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="lastUpdateTime">The time the last update happened.</param>
        /// <param name="updateRate">The update rate.</param>
        /// <returns>The time to use for the next update.</returns>
        protected virtual TickCount GetNextUpdateTime(TickCount currentTime, TickCount lastUpdateTime, TickCount updateRate)
        {
            // Increment by the update rate
            var nextTime = lastUpdateTime + updateRate;

            // If there is a large gap between updates, do not let it result in a bunch of consecutive updates happening
            // as we catch up
            if (nextTime < currentTime)
                nextTime = currentTime;

            return nextTime;
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
        /// When overridden in the derived class, adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        protected abstract void InternalAddUserGuildChange(TUser user, GuildID? guildID);

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

        /// <summary>
        /// Updates the statistics.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void InternalUpdate(TickCount currentTime)
        {
            // NetStats
            if (_nextLogNetStatsTime < currentTime)
            {
                var statDiffs = NetStats.Global - _lastNetStatsValues;
                _lastNetStatsValues = NetStats.Global;
                LogNetStats(statDiffs);

                _nextLogNetStatsTime = GetNextUpdateTime(currentTime, _nextLogNetStatsTime, LogNetStatsRate);
            }
        }

        /// <summary>
        /// When overridden in the derived class, logs the <see cref="NetStats"/> to the database.
        /// </summary>
        /// <param name="netStats">The <see cref="NetStats"/> containing the statistics to log. This contains the difference
        /// in the <see cref="NetStats"/> from the last call.</param>
        protected abstract void LogNetStats(NetStats netStats);

        /// <summary>
        /// Gets the <see cref="DateTime"/> for the current time.
        /// </summary>
        /// <returns>The <see cref="DateTime"/> for the current time.</returns>
        protected virtual DateTime Now()
        {
            return DateTime.Now;
        }

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
        /// Adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        public void AddUserGuildChange(TUser user, GuildID? guildID)
        {
            if (user == null)
                return;

            InternalAddUserGuildChange(user, guildID);
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

        /// <summary>
        /// Updates the statistics that are time-based.
        /// </summary>
        public void Update()
        {
            InternalUpdate(TickCount.Now);
        }

        #endregion
    }
}