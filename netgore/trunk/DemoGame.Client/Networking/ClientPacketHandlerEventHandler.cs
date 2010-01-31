using System.Linq;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Delegate for handling events from the <see cref="ClientPacketHandler"/>.
    /// </summary>
    /// <param name="sender">The <see cref="ClientPacketHandler"/> the event came from.</param>
    /// <param name="conn">The <see cref="IIPSocket"/> the event is related to.</param>
    delegate void ClientPacketHandlerEventHandler(ClientPacketHandler sender, IIPSocket conn);

    /// <summary>
    /// Delegate for handling events from the <see cref="ClientPacketHandler"/>.
    /// </summary>
    /// <typeparam name="T">The type of event args.</typeparam>
    /// <param name="sender">The <see cref="ClientPacketHandler"/> the event came from.</param>
    /// <param name="conn">The <see cref="IIPSocket"/> the event is related to.</param>
    /// <param name="e">The event args.</param>
    delegate void ClientPacketHandlerEventHandler<T>(ClientPacketHandler sender, IIPSocket conn, T e);
}