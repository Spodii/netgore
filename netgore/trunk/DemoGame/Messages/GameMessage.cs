using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different in-game messages, including messages sent from the Server
    /// to the Client.
    /// </summary>
    public enum GameMessage : ushort
    {
        GameTitle,

        #region Core: Combat messages

        /// <summary>
        /// The attacker tried to attack using a weapon that doesn't allow such an attack. Usually, this is for if they try
        /// to attack with something that isn't a weapon.
        /// </summary>
        CannotAttackWithWeapon,

        /// <summary>
        /// The target that the attacker is trying to attack is too far away.
        /// </summary>
        CannotAttackTooFarAway,

        /// <summary>
        /// A target is required to perform the requested attack, but no target was given.
        /// </summary>
        CannotAttackNeedTarget,

        /// <summary>
        /// Cannot attack a character since the attacker's alliance does not let them.
        /// </summary>
        CannotAttackAllianceConflict,

        /// <summary>
        /// Cannot attack a character since the target is not in the attacker's line of sight.
        /// </summary>
        CannotAttackNotInSight,

        /// <summary>
        /// Displayed when a status effect is worn off during combat
        /// </summary>
        CombatStatusEffectWoreOff,

        /// <summary>
        /// A combat status effect has worn off on a user
        /// </summary>
        CombatStatusEffectGained,

        /// <summary>
        /// A message recieved after defeating a monster
        /// </summary>
        CombatRecieveReward,


        /// <summary>
        /// A message recieved for when you level up
        /// </summary>
        CombatSelfLevelUp, 

        /// <summary>
        /// Displayed when a spell has begun to be casted
        /// </summary>
        CombatCastingBegin,

        #endregion

        #region Core: General chat command responses

        /// <summary>
        /// Do not have the <see cref="UserPermissions"/> level needed to perform an action (can be any kind of action).
        /// </summary>
        CommandGeneralInsufficientPermissions,

        /// <summary>
        /// The user the command was targeted at contain an invalid/illegal name.
        /// </summary>
        CommandGeneralInvalidUser,

        /// <summary>
        /// The user the command was targeted at does not exist (but the name is valid).
        /// </summary>
        CommandGeneralUnknownUser,

        /// <summary>
        /// An invalid parameter was supplied for the command.
        /// </summary>
        CommandGeneralInvalidParameter,

        /// <summary>
        /// Same as <see cref="GameMessage.CommandGeneralInvalidParameter"/>, but includes a details string for why it failed.
        /// </summary>
        CommandGeneralInvalidParameterEx,

        /// <summary>
        /// The user the command was targeted at exists, but they are offline and this command requires them to be online.
        /// </summary>
        CommandGeneralUserOffline,

        /// <summary>
        /// The user the command was targeted cannot be the target for this command, or the one issuing the command is not
        /// allowed to do it to the given user.
        /// </summary>
        CommandGeneralUserNotAllowed,

        /// <summary>
        /// Invalid/non-existent Say chat command.
        /// </summary>
        CommandGeneralInvalidCommand,

        #endregion

        #region Core: Specialized chat command responses

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
        /// Message received when a User shouts.
        /// </summary>
        CommandShout,

        /// <summary>
        /// Message recived from Administrator sent globally.
        /// </summary>
        CommandAnnounce,


        #endregion

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

        #region Core: World Notifications
        
        /// <summary>
        /// Message sent when a user has successfully joined the world
        /// </summary>
        UserJoinedWorld,

        /// <summary>
        /// Message sent when a user has successfully left the world
        /// </summary>
        UserLeftWorld,

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

        /// <summary>
        /// The connection was dropped since it was idle for too long.
        /// </summary>
        DisconnectTimedOut,

        /// <summary>
        /// The user was forcibly disconnected from the server by an administrator.
        /// </summary>
        DisconnectUserKicked,

        #endregion

        #region Feature: Skills

        /// <summary>
        /// Tried to use a skill that they do not know.
        /// </summary>
        SkillNotKnown,

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

        #region Feature: Groups

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
        GroupSay,

        #endregion

        #region Feature: Guilds

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

        /// <summary>
        /// Message shown to a user when they try to log in while banned.
        /// </summary>
        AccountBanned,

        /// <summary>
        /// User was disconnected because they were just banned.
        /// </summary>
        DisconnectedBanned,

        /// <summary>
        /// Ban was successfully added for the given user.
        /// </summary>
        BanUserSuccessful,

        /// <summary>
        /// Ban failed to be added for the given user.
        /// </summary>
        BanUserFailed,

        /// <summary>
        /// Ban was successfully removed for the given user.
        /// </summary>
        UnBanUserSuccessful,

        /// <summary>
        /// Ban failed to be removed for the given user.
        /// </summary>
        UnBanUserFailed,

        /// <summary>
        /// Ban failed to be added for yourself.
        /// </summary>
        SelfBanFailed,

        /// <summary>
        /// UnBan failed to be removed for yourself.
        /// </summary>
        SelfUnBanFailed,

        /// <summary>
        /// User has insufficient permissions to ban the required user.
        /// </summary>
        BanInsufficientPermissions,

        #endregion

        #region Command Responses
        
        /// <summary>
        /// Triggered when the "GiveCash" command is fired
        /// </summary>
        AdminGaveCash,

        /// <summary>
        /// Triggered when the "TakeCash" command is fired
        /// </summary>
        AdminTookCash,
        #endregion
    }
}