using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different in-game messages, including messages sent from the Server
    /// to the Client.
    /// </summary>
    public enum GameMessage : byte
    {
        /// <summary>
        /// Invalid Say chat command.
        /// </summary>
        InvalidCommand,

        /// <summary>
        /// Message received when a User shouts.
        /// </summary>
        CommandShout,

        /// <summary>
        /// Tell command contains no name.
        /// </summary>
        CommandTellNoName,

        /// <summary>
        /// Tell command contaisn no message.
        /// </summary>
        CommandTellNoMessage,

        /// <summary>
        /// Message received by the sender of a Tell command.
        /// </summary>
        CommandTellSender,

        /// <summary>
        /// Message received by the receiver of a Tell command.
        /// </summary>
        CommandTellReceiver,

        /// <summary>
        /// Tell command contains the name of a User that does not exist.
        /// </summary>
        CommandTellInvalidUser,

        /// <summary>
        /// Tell command contains the name of a User that exists, but is offline.
        /// </summary>
        CommandTellOfflineUser,

        /// <summary>
        /// Tried to log in, but an invalid account name was given.
        LoginInvalidName,

        /// <summary>
        /// Tried to log in, but an invalid password was given.
        /// </summary>
        LoginInvalidPassword,

        /// <summary>
        /// Tried to log in, but the account is already in use.
        /// </summary>
        LoginAccountInUse,

        #region Shops

        /// <summary>
        /// Tried to purchase an item from a shop, but they did not have enough money. Singular (tried to
        /// purchase just one item).
        /// </summary>
        ShopInsufficientFundsToPurchaseSingular,

        /// <summary>
        /// Tried to purchase an item from a shop, but they did not have enough money. Plural (tried to
        /// purchase more than one item).
        /// </summary>
        ShopInsufficientFundsToPurchasePlural,

        /// <summary>
        /// Purchased a single item from a shop.
        /// </summary>
        ShopPurchaseSingular,

        /// <summary>
        /// Purchased multiple items from a shop.
        /// </summary>
        ShopPurchasePlural,

        /// <summary>
        /// Sell a single item to a shop.
        /// </summary>
        ShopSellItemSingular,

        /// <summary>
        /// Sell more than one item to a shop.
        /// </summary>
        ShopSellItemPlural,

        #endregion

        #region Guilds

        InvalidCommandMustBeInGroup,
        InvalidCommandMustNotBeInGroup,

        GroupCreated,
        GroupCreateFailedUnknownReason,
        GroupInvited,
        GroupInvite,
        GroupInviteFailedCannotInviteSelf,
        GroupInviteFailedInvalidUser,
        GroupInviteFailedAlreadyInGroup,
        GroupInviteFailedUnknownReason,
        GroupJoinFailedGroupIsFull,
        GroupJoinFailedUnknownReason,
        GroupJoined,
        GroupLeave,
        GroupMemberLeft,
        GroupMemberJoined,

        #endregion

        #region Groups

        /// <summary>
        /// Cannot execute command because the user is in a guild.
        /// </summary>
        InvalidCommandMustBeInGuild,

        /// <summary>
        /// Cannot execute command because the user is not in a guild.
        /// </summary>
        InvalidCommandMustNotBeInGuild,

        GuildCreationSuccessful,
        GuildCreationFailedUnknownReason,
        GuildCreationFailedNameInvalid,
        GuildCreationFailedNameNotAvailable,
        GuildCreationFailedTagInvalid,
        GuildCreationFailedTagNotAvailable,

        GuildInvited,
        GuildInviteSuccess,
        GuildInviteFailedCannotInviteSelf,
        GuildInviteFailedInvalidUser,
        GuildInviteFailedAlreadyInGuild,
        GuildInviteFailedUnknownReason,

        GuildRenamed,
        GuildRenameFailedInvalidValue,
        GuildRenameFailedNameNotAvailable,
        GuildRenameFailedUnknownReason,

        GuildRetagged,
        GuildRetagFailedInvalidValue,
        GuildRetagFailedNameNotAvailable,
        GuildRetagFailedUnknownReason,

        GuildKick,
        GuildKickFailedInvalidUser,
        GuildKickFailedNotInGuild,
        GuildKickFailedTooHighRank,
        GuildKickFailedUnknownReason,

        GuildPromotion,
        GuildPromote,
        GuildPromoteFailed,

        GuildDemotion,
        GuildDemote,
        GuildDemoteFailed,

        GuildInsufficientPermissions,
        GuildJoin,
        GuildJoinFailedInvalidOrNoInvite,
        GuildLeave,
        #endregion
    }
}