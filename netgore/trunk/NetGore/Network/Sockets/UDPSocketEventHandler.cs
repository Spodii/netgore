using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for an event from a <see cref="UDPSocket"/>.
    /// </summary>
    /// <param name="sender">The <see cref="UDPSocket"/> the event came from.</param>
    public delegate void UDPSocketEventHandler(UDPSocket sender);

    /// <summary>
    /// Delegate for an event from a <see cref="UDPSocket"/>.
    /// </summary>
    /// <typeparam name="T">The Type of additional argument.</typeparam>
    /// <param name="sender">The <see cref="UDPSocket"/> the event came from.</param>
    /// <param name="e">The additional event arguments.</param>
    public delegate void UDPSocketEventHandler<in T>(UDPSocket sender, T e);
}