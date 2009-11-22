using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Holds information allowing the ability to call a message processor class
    /// for a message of a specified ID just from using attribute tags
    /// </summary>
    public class MessageProcessor
    {
        /// <summary>
        /// Delegate to the processing method
        /// </summary>
        readonly MessageProcessorHandler _call;

        /// <summary>
        /// ID of the message the delegate processes.
        /// </summary>
        readonly byte _msgID;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        /// <param name="msgID">ID of the message to process. Must be non-zero.</param>
        /// <param name="methodDelegate">Delegate to the processing method.</param>
        public MessageProcessor(byte msgID, MessageProcessorHandler methodDelegate)
        {
            if (methodDelegate == null)
                throw new ArgumentNullException("methodDelegate");

            if (msgID == 0)
                throw new ArgumentOutOfRangeException("msgID", "The message ID may not be zero.");

            _msgID = msgID;
            _call = methodDelegate;
        }

        /// <summary>
        /// Gets the delegate to the processing method.
        /// </summary>
        public MessageProcessorHandler Call
        {
            get { return _call; }
        }

        /// <summary>
        /// Gets the ID of the message the delegate processes.
        /// </summary>
        public byte MsgID
        {
            get { return _msgID; }
        }
    }
}