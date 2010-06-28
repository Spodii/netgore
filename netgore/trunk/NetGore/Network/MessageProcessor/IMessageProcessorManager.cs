using System.Collections.Generic;
using System.Linq;

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
        /// <param name="rec"><see cref="SocketReceiveData"/> to process.</param>
        void Process(SocketReceiveData rec);

        /// <summary>
        /// Handles received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="socket"><see cref="IIPSocket"/> the data came from.</param>
        /// <param name="data">Data to process.</param>
        void Process(IIPSocket socket, byte[] data);

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="recvData">IEnumerable of <see cref="SocketReceiveData"/>s to process.</param>
        void Process(IEnumerable<SocketReceiveData> recvData);
    }
}