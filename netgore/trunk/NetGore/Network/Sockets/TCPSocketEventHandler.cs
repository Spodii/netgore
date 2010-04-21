using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for an event from a <see cref="TCPSocket"/>.
    /// </summary>
    /// <param name="sender">The <see cref="TCPSocket"/> the event came from.</param>
    public delegate void TCPSocketEventHandler(TCPSocket sender);

    /// <summary>
    /// Delegate for an event from a <see cref="TCPSocket"/>.
    /// </summary>
    /// <typeparam name="T">The Type of additional argument.</typeparam>
    /// <param name="sender">The <see cref="TCPSocket"/> the event came from.</param>
    /// <param name="e">The additional event arguments.</param>
    public delegate void TCPSocketEventHandler<in T>(TCPSocket sender, T e);
}