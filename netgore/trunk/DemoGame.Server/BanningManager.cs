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
        /// <param name="accountID">The ID of the account to get the bans for.</param>
        /// <returns>The bans for the given <paramref name="accountID"/>.</returns>
        public IEnumerable<IAccountBanTable> GetBanInfo(AccountID accountID)
        {
            var q = DbController.GetQuery<SelectAccountBansQuery>();
            return q.Execute(accountID);
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
        /// <param name="reason">The reason for the ban. Strings longer than 255 characters will be truncated.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <param name="failReason">When this method returns false, contains the reason why the ban failed to be added.</param>
        /// <returns>
        /// True if the ban was successfully added; otherwise false.
        /// </returns>
        protected override bool TryAddBanInternal(AccountID accountID, TimeSpan length, string reason, string issuedBy,
                                                  out string failReason)
        {
            var q = DbController.GetQuery<InsertAccountBanQuery>();
            var rowsAffected = q.Execute(accountID, length, reason, issuedBy);

            if (rowsAffected == 0)
            {
                failReason = "Failed to insert the row into the database.";
                return false;
            }
            else
            {
                failReason = null;
                return true;
            }
        }
    }
}