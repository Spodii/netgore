using System;
using System.Linq;
using NetGore.Db;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// Interface for a class that manages bans for the banning feature.
    /// </summary>
    /// <typeparam name="TAccountID">The type of the account ID.</typeparam>
    public interface IBanningManager<in TAccountID>
    {
        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this object to execute queries to the database.
        /// </summary>
        IDbController DbController { get; }

        /// <summary>
        /// Checks if an account is currently banned.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        bool IsBanned(TAccountID accountID);

        /// <summary>
        /// Checks if an account is currently banned, and if so, gets the reason(s) why and how long the ban will last.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <param name="reasons">When this method returns true, contains the reasons as to why the account is
        /// banned, where each reason is delimited by a new line.</param>
        /// <param name="minsLeft">When this method returns true, contains the amount of time remaining in minutes for
        /// all bans to expire.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        bool IsBanned(TAccountID accountID, out string reasons, out int minsLeft);

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountID">The account to add the ban to.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>True if the ban was successfully added; otherwise false.</returns>
        bool TryAddAccountBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy);

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountID">The account to add the ban to.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        bool TryAddAccountBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy,
                              out BanManagerFailReason failReason);

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountName">The name of the account to ban.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>True if the ban was successfully added; otherwise false.</returns>
        bool TryAddAccountBan(string accountName, TimeSpan length, string reason, string issuedBy);

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountName">The name of the account to ban.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        bool TryAddAccountBan(string accountName, TimeSpan length, string reason, string issuedBy,
                              out BanManagerFailReason failReason);

        /// <summary>
        /// Tries to add a ban to an account by specifying the user's name.
        /// </summary>
        /// <param name="userName">The name of the user to ban.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        bool TryAddUserBan(string userName, TimeSpan length, string reason, string issuedBy, out BanManagerFailReason failReason);

        /// <summary>
        /// Tries to add a ban to an account by specifying the user's name.
        /// </summary>
        /// <param name="userName">The name of the user to ban.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        bool TryAddUserBan(string userName, TimeSpan length, string reason, string issuedBy);
    }
}