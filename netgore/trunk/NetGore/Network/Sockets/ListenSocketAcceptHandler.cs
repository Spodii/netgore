using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Handles the Accept event from the <see cref="ListenSocket"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ListenSocket"/> that accepted the connection.</param>
    /// <param name="conn">The <see cref="TCPSocket"/> for the connection that was accepted..</param>
    public delegate void ListenSocketAcceptHandler(ListenSocket sender, TCPSocket conn);
}