using System.Linq;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// Describes the different reasons why an attempt to add or remove a ban fails.
    /// </summary>
    public enum BanManagerFailReason : byte
    {
        /// <summary>
        /// No reason given for the failure.
        /// </summary>
        Unknown,

        /// <summary>
        /// A negative ban length was supplied.
        /// </summary>
        NegativeBanDuration,

        /// <summary>
        /// No reason was provided for the ban.
        /// </summary>
        NoReasonProvided,

        /// <summary>
        /// No issuer was provided for the ban.
        /// </summary>
        NoIssuerProvided,

        /// <summary>
        /// The ban failed to be added to the database.
        /// </summary>
        FailedToAddToDatabase,

        /// <summary>
        /// The ban failed to be removed from the database.
        /// </summary>
        FailedToRemoveFromDatabase,

        /// <summary>
        /// An exception occurred while trying to add or remove the ban.
        /// </summary>
        ExceptionOccured,

        /// <summary>
        /// Tried to ban or unban an account that does not exist.
        /// </summary>
        InvalidAccount,

        /// <summary>
        /// Tried to ban or unban a user that does not exist.
        /// </summary>
        InvalidUser,

        /// <summary>
        /// Tried to ban or unban yourself.
        /// </summary>
        SelfBan,
    }
}