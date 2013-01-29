using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Banning;

namespace DemoGame.Server
{
    public sealed class BanningManager : BanningManagerBase<AccountID>
    {
        static readonly BanningManager _instance;

        /// <summary>
        /// Initializes the <see cref="BanningManager"/> class.
        /// </summary>
        static BanningManager()
        {
            _instance = new BanningManager(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BanningManager"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use.</param>
        BanningManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// Gets the <see cref="BanningManager"/> instance.
        /// </summary>
        public static BanningManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the bans for an account.
        /// </summary>
        /// <param name="accountName">The name of the account to get the bans for.</param>
        /// <returns>The bans for the given <paramref name="accountName"/>.</returns>
        public IEnumerable<IAccountBanTable> GetAccountBanInfo(string accountName)
        {
            AccountID accID;
            if (!TryGetAccountIDFromUserName(accountName, out accID))
                return Enumerable.Empty<IAccountBanTable>();

            return GetAccountBanInfo(accID);
        }

        /// <summary>
        /// Gets the bans for an account.
        /// </summary>
        /// <param name="accountID">The ID of the account to get the bans for.</param>
        /// <returns>The bans for the given <paramref name="accountID"/>.</returns>
        public IEnumerable<IAccountBanTable> GetAccountBanInfo(AccountID accountID)
        {
            var q = DbController.GetQuery<SelectAccountBansQuery>();
            return q.Execute(accountID);
        }

        /// <summary>
        /// Gets the bans for an account.
        /// </summary>
        /// <param name="username">The name of the user to get the bans for.</param>
        /// <returns>The bans placed on the account.</returns>
        public IEnumerable<IAccountBanTable> GetUserBanInfo(string username)
        {
            AccountID accID;
            if (!TryGetAccountIDFromUserName(username, out accID))
                return Enumerable.Empty<IAccountBanTable>();

            return GetAccountBanInfo(accID);
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="accountID"/> to an int.
        /// </summary>
        /// <param name="accountID">The value to convert to an int.</param>
        /// <returns>The <paramref name="accountID"/> converted to an int.</returns>
        protected override int ToInt(AccountID accountID)
        {
            return (int)accountID;
        }

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
        protected override bool TryAddBanInternal(AccountID accountID, TimeSpan length, string reason, string issuedBy,
                                                  out BanManagerFailReason failReason)
        {
            var q = DbController.GetQuery<InsertAccountBanQuery>();
            var rowsAffected = q.ExecuteWithResult(accountID, length, reason, issuedBy);

            if (rowsAffected == 0)
            {
                failReason = BanManagerFailReason.FailedToAddToDatabase;
                return false;
            }
            else
            {
                failReason = BanManagerFailReason.Unknown;
                return true;
            }
        }

        /// <summary>
        /// When overridden in the derived class, removes a ban from an account.
        /// </summary>
        /// <param name="accountID">The account to remove the ban from.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be removed.</param>
        /// <returns>
        /// True if the unban was successfully added; otherwise false.
        /// </returns>
        protected override bool TryRemoveBanInternal(AccountID accountID, string issuedBy, out BanManagerFailReason failReason)
        {
            var q = DbController.GetQuery<UpdateAccountUnBanQuery>();
            var rowsAffected = q.ExecuteWithResult(accountID, issuedBy);

            if (rowsAffected == 0)
            {
                failReason = BanManagerFailReason.FailedToRemoveFromDatabase;
                return false;
            }
            else
            {
                failReason = BanManagerFailReason.Unknown;
                return true;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the ID of an account from the account's name.
        /// </summary>
        /// <param name="accountName">The name of the account.</param>
        /// <param name="accountID">When this method returns true, contains the ID of the account for the given
        /// <paramref name="accountName"/>.</param>
        /// <returns>True if the ID of the account for the given <paramref name="accountName"/> was found; otherwise false.</returns>
        protected override bool TryGetAccountIDFromAccountName(string accountName, out AccountID accountID)
        {
            var query = DbController.GetQuery<SelectAccountIDFromNameQuery>();
            var acc = query.Execute(accountName);

            if (!acc.HasValue)
            {
                accountID = new AccountID(0);
                return false;
            }
            else
            {
                accountID = acc.Value;
                return true;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the ID of an account from the user's name.
        /// </summary>
        /// <param name="userName">The name of the user to get the account of.</param>
        /// <param name="accountID">When this method returns true, contains the ID of the account for the given
        /// <paramref name="userName"/>.</param>
        /// <returns>True if the ID of the account for the given <paramref name="userName"/> was found; otherwise false.</returns>
        protected override bool TryGetAccountIDFromUserName(string userName, out AccountID accountID)
        {
            var query = DbController.GetQuery<SelectAccountIDFromUserNameQuery>();
            var id = query.Execute(userName);

            if (!id.HasValue)
            {
                accountID = new AccountID(0);
                return false;
            }
            else
            {
                accountID = id.Value;
                return true;
            }
        }
    }
}