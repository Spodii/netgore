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
        public event BanningManagerAccountBannedEventHandler<TAccountID> AccountBanned;

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a ban is successfully added to an account.
        /// </summary>
        /// <param name="accountID">The account that was banned.</param>
        protected virtual void OnAccountBanned(TAccountID accountID)
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
        /// <param name="reason">The reason for the ban. Strings longer than 255 characters will be truncated.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        protected abstract bool TryAddBanInternal(TAccountID accountID, TimeSpan length, string reason, string issuedBy,
                                                  out string failReason);

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
        /// <param name="reason">The reason for the ban. Strings longer than 255 characters will be truncated.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>True if the ban was successfully added; otherwise false.</returns>
        public bool TryAddBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy)
        {
            string failReason;
            return TryAddBan(accountID, length, reason, issuedBy, out failReason);
        }

        /// <summary>
        /// Tries to add a ban to an account.
        /// </summary>
        /// <param name="accountID">The account to add the ban to.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban. Strings longer than 255 characters will be truncated.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        public bool TryAddBan(TAccountID accountID, TimeSpan length, string reason, string issuedBy, out string failReason)
        {
            // Check the parameters
            if (length.TotalMilliseconds < 0)
            {
                failReason = "Invalid ban length - cannot use a negative time span.";
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason);
                return false;
            }

            if (string.IsNullOrEmpty(reason))
            {
                failReason = "A valid reason must be given.";
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason);
                return false;
            }

            if (string.IsNullOrEmpty(issuedBy))
            {
                failReason = "A valid ban issuer must be given.";
                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason);
                return false;
            }

            // Try to add the ban
            try
            {
                if (!TryAddBanInternal(accountID, length, reason, issuedBy, out failReason))
                {
                    if (string.IsNullOrEmpty(failReason))
                        failReason = "[Unspecified failure]";

                    if (log.IsInfoEnabled)
                        log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason);

                    return false;
                }
            }
            catch (Exception ex)
            {
                failReason = ex.Message;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Failed to ban account `{0}`: {1}", accountID, failReason);

                return false;
            }

            // Raise the event
            OnAccountBanned(accountID);

            if (AccountBanned != null)
                AccountBanned(this, accountID);

            if (log.IsInfoEnabled)
                log.InfoFormat("Successfully banned account `{0}` (length: {1}; reason: {2}; issuedBy: {3}).", accountID, length,
                               reason, issuedBy);

            failReason = null;
            return true;
        }

        #endregion
    }
}