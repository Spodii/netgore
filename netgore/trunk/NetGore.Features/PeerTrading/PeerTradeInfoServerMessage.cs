using System.Linq;

namespace NetGore.Features.PeerTrading
{
    /// <summary>
    /// Contains the types of messages that the server sends to the client for peer trading.
    /// Only needed for the internals of PeerTrading.
    /// </summary>
    public enum PeerTradeInfoServerMessage : byte
    {
        Open,
        UpdateSlot,
        UpdateAccepted,
        UpdateCash,
        Completed,
        Canceled,
        Closed
    }
}