using System.Linq;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// Describes the different reasons why an attempt to add a ban fails.
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
        /// An exception occured while trying to add the ban.
        /// </summary>
        ExceptionOccured,

        /// <summary>
        /// Tried to ban an account that does not exist.
        /// </summary>
        InvalidAccount,

        /// <summary>
        /// Tried to ban a user that does not exist.
        /// </summary>
        InvalidUser,

        /// <summary>
        /// Tried to ban yourself.
        /// </summary>
        SelfBan,

        
    }
}