using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for a class that handles processing messages with methods that are identified by a <see cref="MessageHandlerAttribute"/>.
    /// </summary>
    public interface IMessageProcessorManager
    {
        /// <summary>
        /// Gets the <see cref="IMessageProcessor"/>s handled by this <see cref="IMessageProcessorManager"/>.
        /// </summary>
        IEnumerable<IMessageProcessor> Processors { get; }

        /// <summary>
        /// Handles received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="socket"><see cref="IIPSocket"/> the data came from.</param>
        /// <param name="data">The <see cref="BitStream"/> containing the data to process.</param>
        void Process(IIPSocket socket, BitStream data);

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="recvData">IEnumerable of <see cref="BitStream"/> to process and the corresponding
        /// <see cref="IIPSocket"/> that the data came from.</param>
        void Process(IEnumerable<KeyValuePair<IIPSocket, BitStream>> recvData);
    }
}