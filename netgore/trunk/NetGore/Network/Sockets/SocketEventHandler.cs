using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for a method that handles a generic socket event.
    /// </summary>
    /// <param name="socketManager">The <see cref="SocketManager"/> the event came from.</param>
    /// <param name="conn">Connection on which the event occured.</param>
    public delegate void SocketManagerSocketEventHandler(SocketManager socketManager, IIPSocket conn);


    /// <summary>
    /// Delegate for a method that handles a generic socket event.
    /// </summary>
    /// <param name="socketManager">The <see cref="SocketManager"/> the event came from.</param>
    public delegate void SocketManagerEventHandler(SocketManager socketManager);

    /// <summary>
    /// Delegate for a method that handles a generic socket event.
    /// </summary>
    /// <param name="socketManager">The <see cref="SocketManager"/> the event came from.</param>
    /// <param name="conn">Connection on which the event occured.</param>
    /// <param name="args">Additional arguments for the event.</param>
    public delegate void SocketManagerSocketEventHandler<T>(SocketManager socketManager, IIPSocket conn, T args);
}