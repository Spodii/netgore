using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// An implementation of the <see cref="MessageProcessorManager"/> that provides the implementation of the
    /// <see cref="MessageProcessorStatistics"/> so you don't have to hook it up manually.
    /// </summary>
    public class StatMessageProcessorManager : MessageProcessorManager
    {
        readonly MessageProcessorStatistics _stats;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatMessageProcessorManager"/> class.
        /// </summary>
        /// <param name="source">Root object instance containing all the classes (null if static).</param>
        /// <param name="messageIDBitLength">The length of the message ID in bits. Must be between a value
        /// greater than or equal to 1, and less than or equal to 32.</param>
        /// <returns>Returns a list of all the found message processors for a given class.</returns>
        public StatMessageProcessorManager(object source, int messageIDBitLength) : base(source, messageIDBitLength)
        {
            _stats = new MessageProcessorStatistics(this);
        }

        /// <summary>
        /// Gets the <see cref="IMessageProcessorStatistics"/> instance.
        /// </summary>
        public IMessageProcessorStatistics Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// Invokes the <see cref="IMessageProcessor"/> for handle processing a message block.
        /// </summary>
        /// <param name="socket">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="processor">The <see cref="IMessageProcessor"/> to invoke.</param>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to process.</param>
        protected override void InvokeProcessor(IIPSocket socket, IMessageProcessor processor, BitStream reader)
        {
            // Invoke the processor as normal, but keeping track of the bit position before and after invoking
            var startBits = reader.PositionBits;

            base.InvokeProcessor(socket, processor, reader);

            var endBits = reader.PositionBits;

            // Update the stats
            _stats.HandleProcessorInvoked(processor.MsgID, endBits - startBits);
        }
    }
}