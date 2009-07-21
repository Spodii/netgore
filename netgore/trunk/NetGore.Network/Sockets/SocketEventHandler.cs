namespace NetGore.Network
{
    /// <summary>
    /// Delegate for a generic socket event.
    /// </summary>
    /// <param name="conn">Connection on which the event occured.</param>
    public delegate void SocketEventHandler(IIPSocket conn);

    /// <summary>
    /// Delegate for a generic socket event.
    /// </summary>
    /// <param name="conn">Connection on which the event occured.</param>
    /// <param name="args">Additional arguments for the event.</param>
    public delegate void SocketEventHandler<T>(IIPSocket conn, T args);
}