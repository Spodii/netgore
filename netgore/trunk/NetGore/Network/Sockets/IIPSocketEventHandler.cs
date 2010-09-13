namespace NetGore.Network
{
    /// <summary>
    /// Handles events from the <see cref="IIPSocket"/>.
    /// </summary>
    /// <param name="socket">The <see cref="IIPSocket"/> the event came from.</param>
    public delegate void IIPSocketEventHandler(IIPSocket socket);
}