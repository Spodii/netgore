using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Handles the Accept event from the <see cref="ListenSocket"/>.
    /// </summary>
    /// <param name="conn">TCPSocket the connection was accepted from.</param>
    public delegate void ListenSocketAcceptHandler(TCPSocket conn);
}