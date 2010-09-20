using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// Extension methods for the <see cref="BanManagerFailReason"/>.
    /// </summary>
    public static class BanManagerFailReasonExtensions
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets a string containing the details for the <see cref="BanManagerFailReason"/>.
        /// </summary>
        /// <param name="reason">The <see cref="BanManagerFailReason"/>.</param>
        /// <returns>The detailed reason string for the <paramref name="reason"/>.</returns>
        public static string GetDetailedString(this BanManagerFailReason reason)
        {
            switch (reason)
            {
                case BanManagerFailReason.Unknown:
                    return "[Unspecified failure]";

                case BanManagerFailReason.NegativeBanDuration:
                    return "Invalid ban length - duration cannot be negative.";

                case BanManagerFailReason.NoIssuerProvided:
                    return "A ban issuer must be provided.";

                case BanManagerFailReason.NoReasonProvided:
                    return "A reason for the ban must be provided.";

                case BanManagerFailReason.ExceptionOccured:
                    return "An exception occured while trying to add the ban.";

                case BanManagerFailReason.FailedToAddToDatabase:
                    return "Failed to add the ban to the database.";

                case BanManagerFailReason.InvalidAccount:
                    return "The given account does not exist.";

                case BanManagerFailReason.InvalidUser:
                    return "The given user does not exist.";

                default:
                    const string errmsg = "No detailed string provided for BanManagerFailReason `{0}`.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, reason);
                    Debug.Fail(string.Format(errmsg, reason));
                    return reason + " [Details not available]";
            }
        }
    }
}