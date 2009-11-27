using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for a method that handles processing a message.
    /// </summary>
    /// <param name="conn">Connection the message came from.</param>
    /// <param name="reader"><see cref="BitStream"/> containing the message to be processed.</param>
    public delegate void MessageProcessorHandler(IIPSocket conn, BitStream reader);
}