using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Lidgren.Network;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;

namespace NetGore.Features.WorldStats
{
    /// <summary>
    /// Interface for a class that is used to track world statistics.
    /// </summary>
    /// <typeparam name="TUser">The type of user character.</typeparam>
    /// <typeparam name="TNPC">The type of NPC character.</typeparam>
    /// <typeparam name="TItem">The type of item.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public interface IWorldStatsTracker<in TUser, in TNPC, in TItem> where TUser : class where TNPC : class where TItem : class
    {
        /// <summary>
        /// Gets or sets the <see cref="NetPeer"/> to log the statistics for.
        /// If null, the statistics will not be logged.
        /// </summary>
        NetPeer NetPeerToTrack { get; set; }

        /// <summary>
        /// Adds to the item purchase counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was purchased from a shop.</param>
        /// <param name="amount">The number of items purchased.</param>
        void AddCountBuyItem(int itemTID, int amount);

        /// <summary>
        /// Adds to the item consumption counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was consumed.</param>
        void AddCountConsumeItem(int itemTID);

        /// <summary>
        /// Adds to the item creation counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items created.</param>
        void AddCountCreateItem(int itemTID, int amount);

        /// <summary>
        /// Adds to the NPC kill user counter.
        /// </summary>
        /// <param name="npcTID">The template ID of the NPC that killed the user.</param>
        /// <param name="userID">The template ID of the user that was killed.</param>
        void AddCountNPCKillUser(int npcTID, int userID);

        /// <summary>
        /// Adds to the item sell counter.
        /// </summary>
        /// <param name="itemTID">The template ID of the item that was sold to a shop.</param>
        /// <param name="amount">The number of items sold.</param>
        void AddCountSellItem(int itemTID, int amount);

        /// <summary>
        /// Adds to the item being purchased from a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop that sold the item.</param>
        /// <param name="amount">The number of items the shop sold.</param>
        void AddCountShopBuy(int shopID, int amount);

        /// <summary>
        /// Adds to the item being sold to a shop counter.
        /// </summary>
        /// <param name="shopID">The ID of the shop the item was sold to.</param>
        /// <param name="amount">The number of items sold to the shop.</param>
        void AddCountShopSell(int shopID, int amount);

        /// <summary>
        /// Adds to the item consumption count.
        /// </summary>
        /// <param name="userID">The ID of the user who consumed the item.</param>
        /// <param name="itemTID">The item template ID of the item consumed.</param>
        void AddCountUserConsumeItem(int userID, int itemTID);

        /// <summary>
        /// Adds to the user kill a NPC counter.
        /// </summary>
        /// <param name="userID">The template ID of the user that killed the NPC.</param>
        /// <param name="npcTID">The template ID of the NPC that was killed.</param>
        void AddCountUserKillNPC(int userID, int npcTID);

        /// <summary>
        /// Adds when a NPC kills a user.
        /// </summary>
        /// <param name="npc">The NPC that killed the <paramref name="user"/>.</param>
        /// <param name="user">The User that was killed by the <paramref name="npc"/>.</param>
        void AddNPCKillUser(TNPC npc, TUser user);

        /// <summary>
        /// Adds when a user accepts a quest.
        /// </summary>
        /// <param name="user">The user that accepted a quest.</param>
        /// <param name="questID">The ID of the quest that the user accepted.</param>
        void AddQuestAccept(TUser user, QuestID questID);

        /// <summary>
        /// Adds when a user cancels a quest.
        /// </summary>
        /// <param name="user">The user that canceled a quest.</param>
        /// <param name="questID">The ID of the quest that the user canceled.</param>
        void AddQuestCancel(TUser user, QuestID questID);

        /// <summary>
        /// Adds when a user completes a quest.
        /// </summary>
        /// <param name="user">The user that completed a quest.</param>
        /// <param name="questID">The ID of the quest that the user completed.</param>
        void AddQuestComplete(TUser user, QuestID questID);

        /// <summary>
        /// Adds when a user consumes a consumable item.
        /// </summary>
        /// <param name="user">The user that consumed the item.</param>
        /// <param name="item">The item that was consumed.</param>
        void AddUserConsumeItem(TUser user, TItem item);

        /// <summary>
        /// Adds when a user changes their guild.
        /// </summary>
        /// <param name="user">The user that changed their guild.</param>
        /// <param name="guildID">The ID of the guild the user changed to. If this event is for when the user left a guild,
        /// this value will be null.</param>
        void AddUserGuildChange(TUser user, GuildID? guildID);

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