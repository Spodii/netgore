namespace NetGore.Features.Banning
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IBanningManager{T}"/>.
    /// </summary>
    /// <typeparam name="TAccountID">The type of account ID.</typeparam>
    /// <param name="sender">The <see cref="IBanningManager{T}"/> that this event came from.</param>
    /// <param name="accountID">The account that was banned.</param>
    public delegate void BanningManagerAccountBannedEventHandler<TAccountID>(IBanningManager<TAccountID> sender, TAccountID accountID);
}