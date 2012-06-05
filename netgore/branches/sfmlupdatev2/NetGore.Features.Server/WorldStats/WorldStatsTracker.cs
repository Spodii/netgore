using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
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
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class WorldStatsTracker<TUser, TNPC, TItem> : IWorldStatsTracker<TUser, TNPC, TItem>
        where TUser : class where TNPC : class where TItem : class
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly TickCount _logNetStatsRate;

        NetPeerStatisticsSnapshot _lastNetStatsValues;
        NetPeer _netPeer;
        TickCount _nextLogNetStatsTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStatsTracker{TUser, TNPC, TItem}"/> class.
        /// </summary>
        /// <param name="logNetStatsRate">The rate in milliseconds that the <see cref="NetPeerStatisticsSnapshot"/> information is
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
        /// Gets the rate in milliseconds that the <see cref="NetPeerStatisticsSnapshot"/> information is logged to the database.
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
        /// When overridden in the derived class, adds to the item purchase counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was purchased from a shop.</param>
        /// <param name="amount">The number of items purchased.</param>
        protected abstract void InternalAddCountBuyItem(int itemTID, int amount);

        /// <summary>
        /// When overridden in the derived class, adds to the item consumption counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was consumed.</param>
        protected abstract void InternalAddCountConsumeItem(int itemTID);

        /// <summary>
        /// When overridden in the derived class, adds to the item creation counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items created.</param>
        protected abstract void InternalAddCountCreateItem(int itemTID, int amount);

        /// <summary>
        /// When overridden in the derived class, adds to the NPC kill user counter.
        /// </summary>
        /// <param name="npcTID">The template ID of the NPC that killed the user.</param>
        /// <param name="userID">The template ID of the user that was killed.</param>
        protected abstract void InternalAddCountNPCKillUser(int npcTID, int userID);

        /// <summary>
        /// When overridden in the derived class, adds to the item sell counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items sold.</param>
        protected abstract void InternalAddCountSellItem(int itemTID, int amount);

        /// <summary>
        /// When overridden in the derived class, adds to the item being purchased from a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop that sold the item.</param>
        /// <param name="amount">The number of items the shop sold.</param>
        protected abstract void InternalAddCountShopBuy(int shopID, int amount);

        /// <summary>
        /// When overridden in the derived class, adds to the item being sold to a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop the item was sold to.</param>
        /// <param name="amount">The number of items sold to the shop.</param>
        protected abstract void InternalAddCountShopSell(int shopID, int amount);

        /// <summary>
        /// When overridden in the derived class, adds to the item consumption count.
        /// </summary>
        /// <param name="userID">The ID of the user who consumed the item.</param>
        /// <param name="itemTID">The item template ID of the item consumed.</param>
        protected abstract void InternalAddCountUserConsumeItem(int userID, int itemTID);

        /// <summary>
        /// When overridden in the derived class, adds to the user kill a NPC counter.
        /// </summary>
        /// <param name="userID">The template ID of the user that killed the NPC.</param>
        /// <param name="npcTID">The template ID of the NPC that was killed.</param>
        protected abstract void InternalAddCountUserKillNPC(int userID, int npcTID);

        /// <summary>
        /// When overridden in the derived class, adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        protected abstract void InternalAddNPCKillUser(TNPC npc, TUser user);

        /// <summary>
        /// When overridden in the derived class, adds when a user accepts a quest.
        /// </summary>
        /// <param name="user">The user that accepted a quest.</param>
        /// <param name="questID">The ID of the quest that the user accepted.</param>
        protected abstract void InternalAddQuestAccept(TUser user, QuestID questID);

        /// <summary>
        /// When overridden in the derived class, adds when a user cancels a quest.
        /// </summary>
        /// <param name="user">The user that canceled a quest.</param>
        /// <param name="questID">The ID of the quest that the user canceled.</param>
        protected abstract void InternalAddQuestCancel(TUser user, QuestID questID);

        /// <summary>
        /// When overridden in the derived class, adds when a user completes a quest.
        /// </summary>
        /// <param name="user">The user that completed a quest.</param>
        /// <param name="questID">The ID of the quest that the user completed.</param>
        protected abstract void InternalAddQuestComplete(TUser user, QuestID questID);

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
                // Ensure we have the NetPeer set
                var netPeer = NetPeerToTrack;
                var ss = _lastNetStatsValues;
                if (netPeer != null && ss != null)
                {
                    // Grab the current snapshot
                    var newSS = new NetPeerStatisticsSnapshot(netPeer.Statistics);

                    // Find the time delta
                    var deltaMS = newSS.Time - ss.Time;
                    var deltaSecs = deltaMS / 1000f;

                    // Find the values (most of which are diffs from the last value, averaged over time)
                    var conns = (ushort)netPeer.ConnectionsCount;
                    var recvBytes = NetAvg(newSS.ReceivedBytes, ss.ReceivedBytes, deltaSecs);
                    var recvPackets = NetAvg(newSS.ReceivedPackets, ss.ReceivedPackets, deltaSecs);
                    var recvMsgs = NetAvg(newSS.ReceivedMessages, ss.ReceivedMessages, deltaSecs);
                    var sentBytes = NetAvg(newSS.SentBytes, ss.SentBytes, deltaSecs);
                    var sentPackets = NetAvg(newSS.SentPackets, ss.SentPackets, deltaSecs);
                    var sentMsgs = NetAvg(newSS.SentMessages, ss.SentMessages, deltaSecs);

                    // Update the last snapshot to now
                    _lastNetStatsValues = newSS;

                    // Log
                    LogNetStats(conns, recvBytes, recvPackets, recvMsgs, sentBytes, sentPackets, sentMsgs);
                }

                // Update the time to perform the next logging
                _nextLogNetStatsTime = GetNextUpdateTime(currentTime, _nextLogNetStatsTime, LogNetStatsRate);
            }
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
        protected abstract void LogNetStats(ushort connections, uint recvBytes, uint recvPackets, uint recvMsgs, uint sentBytes,
                                            uint sentPackets, uint sentMsgs);

        /// <summary>
        /// Gets the average rate per second for a <see cref="NetPeerStatisticsSnapshot"/>.
        /// </summary>
        /// <param name="curr">The current value.</param>
        /// <param name="last">The previous value.</param>
        /// <param name="deltaSecs">The time delta between the <paramref name="curr"/> and <paramref name="last"/> in seconds.</param>
        /// <returns>The average per second difference between the two values.</returns>
        static uint NetAvg(int curr, int last, float deltaSecs)
        {
            if (curr < last)
            {
                Debug.Fail("How did the value decrease over time?");
                return 0;
            }

            var diff = curr - last;

            // Get the rate per second
            var ret = Math.Round(diff / deltaSecs);

            // Make sure we don't underflow
            return (uint)Math.Max(0, ret);
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
        /// Handles when an <see cref="Exception"/> is thrown while executing a query in the <see cref="WorldStatsTracker{T,U,V}"/>.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/>.</param>
        protected virtual void OnQueryException(Exception ex)
        {
            const string errmsg = "Error executing WorldStatsTracker query on `{0}`. Exception: {1}";
            if (log.IsErrorEnabled)
                log.ErrorFormat(errmsg, this, ex);
            Debug.Fail(string.Format(errmsg, this, ex));
        }

        #region IWorldStatsTracker<TUser,TNPC,TItem> Members

        /// <summary>
        /// Gets or sets the <see cref="NetPeer"/> to log the statistics for.
        /// If null, the statistics will not be logged.
        /// </summary>
        public NetPeer NetPeerToTrack
        {
            get { return _netPeer; }
            set
            {
                if (_netPeer == value)
                    return;

                _netPeer = value;

                _nextLogNetStatsTime = TickCount.Now + _logNetStatsRate;

                if (NetPeerToTrack != null)
                    _lastNetStatsValues = new NetPeerStatisticsSnapshot(NetPeerToTrack.Statistics);
                else
                    _lastNetStatsValues = null;
            }
        }

        /// <summary>
        /// Adds to the item purchase counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was purchased from a shop.</param>
        /// <param name="amount">The number of items purchased.</param>
        public void AddCountBuyItem(int itemTID, int amount)
        {
            try
            {
                InternalAddCountBuyItem(itemTID, amount);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item consumption counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was consumed.</param>
        public void AddCountConsumeItem(int itemTID)
        {
            try
            {
                InternalAddCountConsumeItem(itemTID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item creation counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items created.</param>
        public void AddCountCreateItem(int itemTID, int amount)
        {
            try
            {
                InternalAddCountCreateItem(itemTID, amount);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the NPC kill user counter.
        /// </summary>
        /// <param name="npcTID">The template ID of the NPC that killed the user.</param>
        /// <param name="userID">The template ID of the user that was killed.</param>
        public void AddCountNPCKillUser(int npcTID, int userID)
        {
            try
            {
                InternalAddCountNPCKillUser(npcTID, userID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item sell counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items sold.</param>
        public void AddCountSellItem(int itemTID, int amount)
        {
            try
            {
                InternalAddCountSellItem(itemTID, amount);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item being purchased from a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop that sold the item.</param>
        /// <param name="amount">The number of items the shop sold.</param>
        public void AddCountShopBuy(int shopID, int amount)
        {
            try
            {
                InternalAddCountShopBuy(shopID, amount);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item being sold to a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop the item was sold to.</param>
        /// <param name="amount">The number of items sold to the shop.</param>
        public void AddCountShopSell(int shopID, int amount)
        {
            try
            {
                InternalAddCountShopSell(shopID, amount);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the item consumption count.
        /// </summary>
        /// <param name="userID">The ID of the user who consumed the item.</param>
        /// <param name="itemTID">The item template ID of the item consumed.</param>
        public void AddCountUserConsumeItem(int userID, int itemTID)
        {
            try
            {
                InternalAddCountUserConsumeItem(userID, itemTID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds to the user kill a NPC counter.
        /// </summary>
        /// <param name="userID">The template ID of the user that killed the NPC.</param>
        /// <param name="npcTID">The template ID of the NPC that was killed.</param>
        public void AddCountUserKillNPC(int userID, int npcTID)
        {
            try
            {
                InternalAddCountUserKillNPC(userID, npcTID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        public void AddNPCKillUser(TNPC npc, TUser user)
        {
            try
            {
                if (npc == null)
                    return;
                if (user == null)
                    return;

                InternalAddNPCKillUser(npc, user);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user accepts a quest.
        /// </summary>
        /// <param name="user">The user that accepted a quest.</param>
        /// <param name="questID">The ID of the quest that the user accepted.</param>
        public void AddQuestAccept(TUser user, QuestID questID)
        {
            try
            {
                if (user == null)
                    return;

                InternalAddQuestAccept(user, questID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user cancels a quest.
        /// </summary>
        /// <param name="user">The user that canceled a quest.</param>
        /// <param name="questID">The ID of the quest that the user canceled.</param>
        public void AddQuestCancel(TUser user, QuestID questID)
        {
            try
            {
                if (user == null)
                    return;

                InternalAddQuestCancel(user, questID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user completes a quest.
        /// </summary>
        /// <param name="user">The user that completed a quest.</param>
        /// <param name="questID">The ID of the quest that the user completed.</param>
        public void AddQuestComplete(TUser user, QuestID questID)
        {
            try
            {
                if (user == null)
                    return;

                InternalAddQuestComplete(user, questID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        public void AddUserConsumeItem(TUser user, TItem item)
        {
            try
            {
                if (user == null)
                    return;
                if (item == null)
                    return;

                InternalAddUserConsumeItem(user, item);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        public void AddUserGuildChange(TUser user, GuildID? guildID)
        {
            try
            {
                if (user == null)
                    return;

                InternalAddUserGuildChange(user, guildID);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user kills a NPC.
        /// </summary>
        /// <param name="user">The user that killed the <paramref name="npc"/>.</param>
        /// <param name="npc">The NPC that was killed by the <paramref name="user"/>.</param>
        public void AddUserKillNPC(TUser user, TNPC npc)
        {
            try
            {
                if (user == null)
                    return;
                if (npc == null)
                    return;

                InternalAddUserKillNPC(user, npc);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Adds when a user gains a level.
        /// </summary>
        /// <param name="user">The user that leveled up.</param>
        public void AddUserLevel(TUser user)
        {
            try
            {
                if (user == null)
                    return;

                InternalAddUserLevel(user);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
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
            try
            {
                if (user == null)
                    return;
                if (amount <= 0)
                    return;

                InternalAddUserLevel(user);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
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
            try
            {
                if (user == null)
                    return;
                if (amount <= 0)
                    return;

                InternalAddUserLevel(user);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        /// <summary>
        /// Updates the statistics that are time-based.
        /// </summary>
        public void Update()
        {
            try
            {
                InternalUpdate(TickCount.Now);
            }
            catch (Exception ex)
            {
                OnQueryException(ex);
            }
        }

        #endregion
    }
}