using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Interface for an object that contains a <see cref="MessageProcessorHandler"/> and the message ID used to recognize the
    /// corresponding <see cref="MessageProcessorHandler"/>.
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// Gets the <see cref="MessageProcessorHandler"/> used to process the message.
        /// </summary>
        MessageProcessorHandler Call { get; }

        /// <summary>
        /// Gets the message ID that <see cref="IMessageProcessor"/> processes.
        /// </summary>
        MessageProcessorID MsgID { get; }
    }
}