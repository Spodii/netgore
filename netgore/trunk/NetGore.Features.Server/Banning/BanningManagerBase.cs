using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Db;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// The default implementation of <see cref="IBanningManager{T}"/>.
    /// </summary>
    /// <typeparam name="TAccountID">The type of account ID.</typeparam>
    public abstract class BanningManagerBase<TAccountID> : IBanningManager<TAccountID>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // ReSharper disable ClassNeverInstantiated.Local

        readonly IDbController _dbController;
        readonly StoredProcGetReasons _procGetReasons;
        readonly StoredProcIsBanned _procIsBanned;

        /// <summary>
        /// Initializes a new instance of the <see cref="BanningManagerBase{TAccountID}"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use.</param>
        protected BanningManagerBase(IDbController dbController)
        {
            _dbController = dbController;

            _procIsBanned = DbController.GetQuery<StoredProcIsBanned>();
            _procGetReasons = DbController.GetQuery<StoredProcGetReasons>();
        }

        /// <summary>
        /// Notifies listeners when an account has been banned.
        /// </summary>
        public event TypedEventHandler<IBanningManager<TAccountID>, BanningManagerAccountBannedEventArgs<TAccountID>>
            AccountBanned;

        /// <summary>
        /// Notifies listeners when an account has been unbanned.
        /// </summary>
        public event TypedEventHandler<IBanningManager<TAccountID>, BanningManagerAccountUnBannedEventArgs<TAccountID>>
            AccountUnBanned;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a ban is successfully added to an account.
        /// </summary>
        /// <param name="accountID">The account that was banned.</param>
        protected virtual void OnAccountBanned(TAccountID accountID)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a ban is successfully removed from an account.
        /// </summary>
        /// <param name="accountID">The account that was unbanned.</param>
        protected virtual void OnAccountUnBanned(TAccountID accountID)
        {
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="accountID"/> to an int.
        /// </summary>
        /// <param name="accountID">The value to convert to an int.</param>
        /// <returns>The <paramref name="accountID"/> converted to an int.</returns>
        protected abstract int ToInt(TAccountID accountID);

        /// <summary>
        /// When overridden in the derived class, adds a ban to an account.
        /// </summary>
        /// <param name="accountID">The account to add the ban to.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        protected abstract bool TryAddBanInternal(TAccountID accountID, TimeSpan length, string reason, string issuedBy,
                                                  out BanManagerFailReason failReason);

        /// <summary>
        /// When overridden in the derived class, removes a ban from an account.
        /// </summary>
        /// <param name="accountID">The account to remove the ban from.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the unban failed to be removed.</param>
        /// <returns>
        /// True if the unban was successfully added; otherwise false.
        /// </returns>
        protected abstract bool TryRemoveBanInternal(TAccountID accountID, string issuedBy, out BanManagerFailReason failReason);

        /// <summary>
        /// When overridden in the derived class, gets the ID of an account from the account's name.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <param name="accountID">When this method returns true, contains the ID of the account for the given
        /// <paramref name="accountName"/>.</param>
        /// <returns>True if the ID of the account for the given <paramref name="accountName"/> was found; otherwise false.</returns>
        protected abstract bool TryGetAccountIDFromAccountName(string accountName, out TAccountID accountID);

        /// <summary>
        /// When overridden in the derived class, gets the ID of an account from the user's name.
        /// </summary>
        /// <param name="userName">The name of the user to get the account of.</param>
        /// <param name="accountID">When this method returns true, contains the ID of the account for the given
        /// <paramref name="userName"/>.</param>
        /// <returns>True if the ID of the account for the given <paramref name="userName"/> was found; otherwise false.</returns>
        protected abstract bool TryGetAccountIDFromUserName(string userName, out TAccountID accountID);

        #region IBanningManager<TAccountID> Members

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this object to execute queries to the database.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Checks if an account is currently banned.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        public virtual bool IsBanned(TAccountID accountID)
        {
            var id = ToInt(accountID);
            return _procIsBanned.Execute(id);
        }

        /// <summary>
        /// Checks if an account is currently banned, and if so, gets the reason(s) why and how long the ban will last.
        /// </summary>
        /// <param name="accountID">The ID of the account to check if banned.</param>
        /// <param name="reasons">When this method returns true, contains the reasons as to why the account is
        /// banned, where each reason is delimited by a new line.</param>
        /// <param name="minsLeft">When this method returns true, contains the amount of time remaining in minutes for
        /// all bans to expire.</param>
        /// <returns>True if the <paramref name="accountID"/> is currently banned; otherwise false.</returns>
        public virtual bool IsBanned(TAccountID accountID, out string reasons, out int minsLeft)
        {
            var id = ToInt(accountID);
            return _procGetReasons.Execute(id, out reasons, out minsLeft);
        }

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountID">The account to add the ban to.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>True if the ban was successfully added; otherwise false.</returns>
        public bool TryAddAccountBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy)
        {
            BanManagerFailReason failReason;
            return TryAddAccountBan(accountID, length, reason, issuedBy, out failReason);
        }

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
        public bool TryAddAccountBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy,
                                     out BanManagerFailReason failReason)
        {
            // Check the parameters
            if (length.TotalMilliseconds < 0)
            {
                failReason = BanManagerFailReason.NegativeBanDuration;
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason.GetDetailedString());
                return false;
            }

            if (string.IsNullOrEmpty(reason))
            {
                failReason = BanManagerFailReason.NoReasonProvided;
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason.GetDetailedString());
                return false;
            }

            if (string.IsNullOrEmpty(issuedBy))
            {
                failReason = BanManagerFailReason.NoIssuerProvided;
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason.GetDetailedString());
                return false;
            }

            // Try to add the ban
            try
            {
                if (!TryAddBanInternal(accountID, length, reason, issuedBy, out failReason))
                {
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason.GetDetailedString());

                    return false;
                }
            }
            catch (Exception ex)
            {
                failReason = BanManagerFailReason.ExceptionOccured;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}. Exception: {2}", accountID, failReason.GetDetailedString(),
                        ex);

                return false;
            }

            // Raise the event
            OnAccountBanned(accountID);

            if (AccountBanned != null)
                AccountBanned.Raise(this, BanningManagerAccountBannedEventArgs.Create(accountID, length, reason, issuedBy));

            if (log.IsInfoEnabled)
                log.InfoFormat("Successfully banned account `{0}` (length: {1}; reason: {2}; issuedBy: {3}).", accountID, length,
                    reason, issuedBy);

            failReason = BanManagerFailReason.Unknown;
            return true;
        }

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountName">The name of the account to ban.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>True if the ban was successfully added; otherwise false.</returns>
        public bool TryAddAccountBan(string accountName, TimeSpan length, string reason, string issuedBy)
        {
            BanManagerFailReason failReason;
            return TryAddAccountBan(accountName, length, reason, issuedBy, out failReason);
        }

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
        public bool TryAddAccountBan(string accountName, TimeSpan length, string reason, string issuedBy,
                                     out BanManagerFailReason failReason)
        {
            // Get the account ID
            TAccountID accountID;
            if (!TryGetAccountIDFromAccountName(accountName, out accountID))
            {
                const string errmsg = "Failed to ban account `{0}`: {1}";
                failReason = BanManagerFailReason.InvalidAccount;
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, accountName, failReason.GetDetailedString());
                return false;
            }

            // Ban the account by ID
            return TryAddAccountBan(accountID, length, reason, issuedBy, out failReason);
        }

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
        public bool TryAddUserBan(string userName, TimeSpan length, string reason, string issuedBy,
                                  out BanManagerFailReason failReason)
        {
            // Get the account ID
            TAccountID accountID;
            if (!TryGetAccountIDFromUserName(userName, out accountID))
            {
                const string errmsg = "Failed to ban user `{0}`: {1}";
                failReason = BanManagerFailReason.InvalidUser;
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, userName, failReason.GetDetailedString());
                return false;
            }

            // Ban the account by ID
            return TryAddAccountBan(accountID, length, reason, issuedBy, out failReason);
        }

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
        public bool TryAddUserBan(string userName, TimeSpan length, string reason, string issuedBy)
        {
            BanManagerFailReason failReason;
            return TryAddUserBan(userName, length, reason, issuedBy, out failReason);
        }

        /// <summary>
        /// Tries to remove a ban from an account.
        /// </summary>
        /// <param name="accountID">The account to remove the ban from.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the unban failed to be added.</param>
        /// <returns>
        /// True if the unban was successfully added; otherwise false.
        /// </returns>
        public bool TryRemoveAccountBan(TAccountID accountID, string issuedBy, out BanManagerFailReason failReason)
        {
            // Check the parameters
            if (string.IsNullOrEmpty(issuedBy))
            {
                failReason = BanManagerFailReason.NoIssuerProvided;
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to unban account `{0}`: {1}", accountID, failReason.GetDetailedString());
                return false;
            }

            // Try to remove the ban
            try
            {
                if (!TryRemoveBanInternal(accountID, issuedBy, out failReason))
                {
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Failed to unban account `{0}`: {1}", accountID, failReason.GetDetailedString());

                    return false;
                }
            }
            catch (Exception ex)
            {
                failReason = BanManagerFailReason.ExceptionOccured;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to unban account `{0}`: {1}. Exception: {2}", accountID, failReason.GetDetailedString(),
                        ex);

                return false;
            }

            // Raise the event
            OnAccountUnBanned(accountID);

            if (AccountUnBanned != null)
                AccountUnBanned.Raise(this, BanningManagerAccountUnBannedEventArgs.Create(accountID, issuedBy));

            if (log.IsInfoEnabled)
                log.InfoFormat("Successfully unbanned account `{0}` (issuedBy: {1}).", accountID, issuedBy);

            failReason = BanManagerFailReason.Unknown;
            return true;
        }

        /// <summary>
        /// Tries to remove a ban from an account by specifying the user's name.
        /// </summary>
        /// <param name="userName">The name of the user to unban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be removed.</param>
        /// <returns>
        /// True if the ban was successfully removed; otherwise false.
        /// </returns>
        public bool TryRemoveUserBan(string userName, string issuedBy, out BanManagerFailReason failReason)
        {
            // Get the account ID
            TAccountID accountID;
            if (!TryGetAccountIDFromUserName(userName, out accountID))
            {
                const string errmsg = "Failed to unban user `{0}`: {1}";
                failReason = BanManagerFailReason.InvalidUser;
                if (log.IsInfoEnabled)
                    log.InfoFormat(errmsg, userName, failReason.GetDetailedString());
                return false;
            }

            // UnBan the account by ID
            return TryRemoveAccountBan(accountID, issuedBy, out failReason);
        }

        #endregion
    }
}