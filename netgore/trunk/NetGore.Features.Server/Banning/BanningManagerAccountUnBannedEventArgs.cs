using System;
using System.Linq;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// <see cref="EventArgs"/> describing unbans from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    public class BanningManagerAccountUnBannedEventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="BanningManagerAccountUnBannedEventArgs{T}"/> class.
        /// </summary>
        /// <typeparam name="TAccountID">The type of the account ID.</typeparam>
        /// <param name="accountID">The account that was unbanned.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        /// <returns>An instance of the <see cref="BanningManagerAccountUnBannedEventArgs{T}"/> class.</returns>
        public static BanningManagerAccountUnBannedEventArgs<TAccountID> Create<TAccountID>(TAccountID accountID, string issuedBy)
        {
            return new BanningManagerAccountUnBannedEventArgs<TAccountID>(accountID, issuedBy);
        }
    }

    /// <summary>
    /// <see cref="EventArgs"/> describing unbans from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    /// <typeparam name="TAccountID">The type of account ID.</typeparam>
    public class BanningManagerAccountUnBannedEventArgs<TAccountID> : EventArgs
    {
        readonly TAccountID _accountID;
        readonly string _issuedBy;

        /// <summary>
        /// Bannings the manager account unbanned event handler.
        /// </summary>
        /// <param name="accountID">The account that was unbanned.</param>
        /// <param name="issuedBy">The name of the user or source that issued the unban.</param>
        public BanningManagerAccountUnBannedEventArgs(TAccountID accountID, string issuedBy)
        {
            _accountID = accountID;
            _issuedBy = issuedBy;
        }

        /// <summary>
        /// Gets the account that was unbanned.
        /// </summary>
        public TAccountID AccountID
        {
            get { return _accountID; }
        }

        /// <summary>
        /// Gets the name of the user or source that issued the unban.
        /// </summary>
        public string IssuedBy
        {
            get { return _issuedBy; }
        }
    }
}