using System;
using System.Bits;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate to a message processor method
    /// </summary>
    /// <param name="conn">Connection the message came from</param>
    /// <param name="reader">Binary reader containing the message to be processed</param>
    public delegate void MessageProcessorDelegate(TCPSocket conn, BitStream reader);

    /// <summary>
    /// Holds information allowing the ability to call a message processor class
    /// for a message of a specified ID just from using attribute tags
    /// </summary>
    public class MessageProcessor
    {
        /// <summary>
        /// Delegate to the processing method
        /// </summary>
        readonly MessageProcessorDelegate _call;

        /// <summary>
        /// ID of the message the delegate processes.
        /// </summary>
        readonly byte _msgID;

        /// <summary>
        /// Gets the delegate to the processing method.
        /// </summary>
        public MessageProcessorDelegate Call
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

        /// <summary>
        /// MessageProcessor constructor.
        /// </summary>
        /// <param name="msgID">ID of the message to process. Must be non-zero.</param>
        /// <param name="methodDelegate">Delegate to the processing method.</param>
        public MessageProcessor(byte msgID, MessageProcessorDelegate methodDelegate)
        {
            if (methodDelegate == null)
                throw new ArgumentNullException("methodDelegate");

            if (msgID == 0)
                throw new ArgumentOutOfRangeException("msgID", "The message ID may not be zero.");

            _msgID = msgID;
            _call = methodDelegate;
        }
    }
}