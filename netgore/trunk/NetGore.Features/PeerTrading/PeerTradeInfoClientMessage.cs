namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Contains the types of messages that the client sends to the server for peer trading.
    /// Only needed for the internals of PeerTrading.
    /// </summary>
    public enum PeerTradeInfoClientMessage
    {
        Cancel,
        Accept,
        AddInventoryItem,
        RemoveItem
    }
}