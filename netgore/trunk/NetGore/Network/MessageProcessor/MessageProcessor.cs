using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Holds information allowing the ability to call a message processor class
    /// for a message of a specified ID just from using attribute tags
    /// </summary>
    public class MessageProcessor : IMessageProcessor
    {
        /// <summary>
        /// Delegate to the processing method
        /// </summary>
        readonly MessageProcessorHandler _call;

        /// <summary>
        /// ID of the message the delegate processes.
        /// </summary>
        readonly MessageProcessorID _msgID;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        /// <param name="msgID">ID of the message to process. Must be non-zero.</param>
        /// <param name="methodDelegate">Delegate to the processing method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodDelegate" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="msgID"/> is zero.</exception>
        public MessageProcessor(MessageProcessorID msgID, MessageProcessorHandler methodDelegate)
        {
            if (methodDelegate == null)
                throw new ArgumentNullException("methodDelegate");

            if (msgID.GetRawValue() == 0)
                throw new ArgumentOutOfRangeException("msgID", "The message ID may not be zero.");

            _msgID = msgID;
            _call = methodDelegate;
        }

        #region IMessageProcessor Members

        /// <summary>
        /// Gets the <see cref="MessageProcessorHandler"/> used to process the message.
        /// </summary>
        public MessageProcessorHandler Call
        {
            get { return _call; }
        }

        /// <summary>
        /// Gets the message ID that <see cref="IMessageProcessor"/> processes.
        /// </summary>
        public MessageProcessorID MsgID
        {
            get { return _msgID; }
        }

        #endregion
    }
}