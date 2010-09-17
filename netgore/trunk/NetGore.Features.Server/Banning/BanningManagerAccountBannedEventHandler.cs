using System;
using System.Linq;

namespace NetGore.Features.Banning
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    /// <typeparam name="TAccountID">The type of account ID.</typeparam>
    /// <param name="sender">The <see cref="IBanningManager{T}"/> that this event came from.</param>
    /// <param name="accountID">The account that was banned.</param>
    /// <param name="length">How long the ban will last.</param>
    /// <param name="reason">The reason for the ban.</param>
    /// <param name="issuedBy">The name of the user or source that issued the ban.</param>
    public delegate void BanningManagerAccountBannedEventHandler<TAccountID>(
        IBanningManager<TAccountID> sender, TAccountID accountID, TimeSpan length, string reason, string issuedBy);
}