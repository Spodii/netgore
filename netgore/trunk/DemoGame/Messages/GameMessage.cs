using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different in-game messages, including messages sent from the Server
    /// to the Client.
    /// </summary>
    public enum GameMessage : ushort
    {
        CannotAttackWithWeapon,
        CannotAttackTooFarAway,
        CannotAttackNeedTarget,
        CannotAttackAllianceConflict,

        /// <summary>
        /// Do not have the <see cref="UserPermissions"/> level needed to perform an action.
        /// </summary>
        InsufficientPermissions,

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

        #region Core: Login failure reasons

        /// <summary>
        /// Tried to log in, but an invalid account name was given.
        /// </summary>
        LoginInvalidName,

        /// <summary>
        /// Tried to log in, but an invalid password was given.
        /// </summary>
        LoginInvalidPassword,

        /// <summary>
        /// Tried to log in, but the account is already in use.
        /// </summary>
        LoginAccountInUse,

        #endregion

        #region Core: Account creation

        /// <summary>
        /// The user successfully created a new account.
        /// </summary>
        CreateAccountSuccessful,

        /// <summary>
        /// Tried to create a new account, but an invalid name was specified.
        /// </summary>
        CreateAccountInvalidName,

        /// <summary>
        /// Tried to create an account, but an invalid password was specified.
        /// </summary>
        CreateAccountInvalidPassword,

        /// <summary>
        /// Tried to create an account, but an invalid email address was specified.
        /// </summary>
        CreateAccountInvalidEmail,

        /// <summary>
        /// Tried to create an account, but too many accounts have been created from the client's IP address recently.
        /// </summary>
        CreateAccountTooManyCreated,

        /// <summary>
        /// Tried to create an account, but an account with that name already exists.
        /// </summary>
        CreateAccountAlreadyExists,

        /// <summary>
        /// Tried to create an account, but there was an unknown error.
        /// </summary>
        CreateAccountUnknownError,

        #endregion

        #region Core: Disconnect reasons

        /// <summary>
        /// Tried to connect to the server, but there are too many connections from this IP.
        /// </summary>
        DisconnectTooManyConnectionsFromIP,

        /// <summary>
        /// The server has disposed the user's character.
        /// </summary>
        DisconnectUserDisposed,

        /// <summary>
        /// No reason was given for why the client was disconnected.
        /// </summary>
        DisconnectNoReasonSpecified,

        #endregion

        #region Feature: Shops

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

        #region Feature: Guilds

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

        #region Feature: Groups

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
        GuildSay,

        #endregion

        #region Feature: PeerTrading

        PeerTradingNotEnoughSpaceInInventory,
        PeerTradingInvalidTarget,
        PeerTradingTooFarAway,
        PeerTradingCannotStartTrade,
        PeerTradingTargetCannotStartTrade,
        PeerTradingTradeCanceledByYou,
        PeerTradingTradeCanceledByOther,
        PeerTradingTradeComplete,
        PeerTradingTradeOpened,
        PeerTradingItemsRecovered,
        PeerTradingItemsRecoveredNoRemaining,

        #endregion

        #region Feature: Quests

        QuestAccepted,
        QuestAcceptFailedAlreadyCompleted,
        QuestAcceptFailedAlreadyStarted,
        QuestAcceptFailedDoNotHaveStartRequirements,
        QuestAcceptFailedTooManyActive,
        QuestCanceled,
        QuestFinished,
        QuestFinishFailedCannotGiveRewards,

        #endregion

        #region Feature: Banning

        AccountBanned,

        #endregion
    }
}