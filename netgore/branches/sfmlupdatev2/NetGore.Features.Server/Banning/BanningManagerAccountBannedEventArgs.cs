using System;
using System.Linq;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// <see cref="EventArgs"/> describing bans from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    public class BanningManagerAccountBannedEventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="BanningManagerAccountBannedEventArgs{T}"/> class.
        /// </summary>
        /// <typeparam name="TAccountID">The type of the account ID.</typeparam>
        /// <param name="accountID">The account that was banned.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        /// <returns>An instance of the <see cref="BanningManagerAccountBannedEventArgs{T}"/> class.</returns>
        public static BanningManagerAccountBannedEventArgs<TAccountID> Create<TAccountID>(TAccountID accountID, TimeSpan length,
                                                                                          string reason, string issuedBy)
        {
            return new BanningManagerAccountBannedEventArgs<TAccountID>(accountID, length, reason, issuedBy);
        }
    }

    /// <summary>
    /// <see cref="EventArgs"/> describing bans from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    /// <typeparam name="TAccountID">The type of account ID.</typeparam>
    public class BanningManagerAccountBannedEventArgs<TAccountID> : EventArgs
    {
        readonly TAccountID _accountID;
        readonly string _issuedBy;
        readonly TimeSpan _length;
        readonly string _reason;

        /// <summary>
        /// Bannings the manager account banned event handler.
        /// </summary>
        /// <param name="accountID">The account that was banned.</param>
        /// <param name="length">How long the ban will last.</param>
        /// <param name="reason">The reason for the ban.</param>
        /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
        public BanningManagerAccountBannedEventArgs(TAccountID accountID, TimeSpan length, string reason, string issuedBy)
        {
            _accountID = accountID;
            _length = length;
            _reason = reason;
            _issuedBy = issuedBy;
        }

        /// <summary>
        /// Gets the account that was banned.
        /// </summary>
        public TAccountID AccountID
        {
            get { return _accountID; }
        }

        /// <summary>
        /// Gets the name of the user or source that issued the ban.
        /// </summary>
        public string IssuedBy
        {
            get { return _issuedBy; }
        }

        /// <summary>
        /// Gets how long the ban will last.
        /// </summary>
        public TimeSpan Length
        {
            get { return _length; }
        }

        /// <summary>
        /// Gets the reason for the ban.
        /// </summary>
        public string Reason
        {
            get { return _reason; }
        }
    }
}