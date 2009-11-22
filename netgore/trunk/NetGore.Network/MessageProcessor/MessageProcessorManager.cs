using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Manages a group of message processing methods, each identified by the attribute
    /// <see cref="MessageHandlerAttribute"/>.
    /// </summary>
    public class MessageProcessorManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly int _messageIDBitLength;
        readonly MessageProcessor[] _processors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorManager"/> class.
        /// </summary>
        /// <param name="source">Root object instance containing all the classes (null if static).</param>
        /// <param name="messageIDBitLength">The length of the message ID in bits. Must be between a value
        /// greater than or equal to 1, and less than or equal to 8.</param>
        /// <returns>Returns a list of all the found message processors for a given class.</returns>
        public MessageProcessorManager(object source, int messageIDBitLength)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (messageIDBitLength > 8 || messageIDBitLength < 1)
                throw new ArgumentOutOfRangeException("messageIDBitLength");

            _messageIDBitLength = messageIDBitLength;

            // Store the types we will use
            Type mpdType = typeof(MessageProcessorHandler);
            Type atbType = typeof(MessageHandlerAttribute);
            Type voidType = typeof(void);

            // Create the processors array
            _processors = new MessageProcessor[256];

            // Search through all types in the Assembly
            Assembly assemb = Assembly.GetAssembly(source.GetType());
            foreach (Type type in assemb.GetTypes())
            {
                const BindingFlags bindFlags =
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.Static;

                // Search through every method in the class
                foreach (MethodInfo method in type.GetMethods(bindFlags))
                {
                    // Only accept a method if it returns a void
                    if (method.ReturnType != voidType)
                        continue;

                    // Get all of the MessageAttributes for the method (should only be one)
                    var atbs = (MessageHandlerAttribute[])method.GetCustomAttributes(atbType, true);
                    if (atbs.Length > 1)
                        throw new Exception(string.Format("Multiple MessageAttributes found for method `{0}`.", method.Name));

                    // Create the message processor for the method
                    foreach (MessageHandlerAttribute atb in atbs)
                    {
                        MessageProcessorHandler del = (MessageProcessorHandler)Delegate.CreateDelegate(mpdType, source, method);
                        _processors[atb.MsgID] = new MessageProcessor(atb.MsgID, del);
                    }
                }
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the MessageProcessors handled by this MessageProcessorManager.
        /// </summary>
        public IEnumerable<MessageProcessor> Processors
        {
            get { return _processors; }
        }

        /// <summary>
        /// Handles received data and forwards it to the corresponding <see cref="MessageProcessor"/>.
        /// </summary>
        /// <param name="rec"><see cref="SocketReceiveData"/> to process.</param>
        public void Process(SocketReceiveData rec)
        {
            if (rec.Socket == null)
            {
                Debug.Fail("rec.Socket is null.");
                return;
            }

            if (rec.Data == null)
            {
                Debug.Fail("rec.Data is null.");
                return;
            }

            // Go through each piece of data and forward to the message processor
            foreach (var data in rec.Data)
            {
                Process(rec.Socket, data);
            }
        }

        /// <summary>
        /// Handles received data and forwards it to the corresponding <see cref="MessageProcessor"/>.
        /// </summary>
        /// <param name="socket"><see cref="IIPSocket"/> the data came from.</param>
        /// <param name="data">Data to process.</param>
        public void Process(IIPSocket socket, byte[] data)
        {
            if (socket == null)
            {
                const string errmsg = "socket is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }
            if (data == null)
            {
                const string errmsg = "data is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }
            if (data.Length == 0)
            {
                const string errmsg = "data array is empty.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            // Create a BitStream to read the data
            BitStream recvReader = new BitStream(data);

            // Loop through the data until it is emptied
            while (recvReader.PositionBytes < data.Length)
            {
                // Get the ID of the message
                byte msgID = recvReader.ReadByte(_messageIDBitLength);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Parsing message ID `{0}`. Stream now at bit position `{1}`.", msgID, recvReader.PositionBits);

                // If we get a message ID of 0, we have likely hit the end
                if (msgID == 0)
                {
                    const string errmsg = "Encountered msgID = 0, but there was set bits remaining in the stream.";
                    Debug.Assert(RestOfStreamIsZero(recvReader), errmsg);
                    return;
                }

                // Find the corresponding processor and call it
                MessageProcessor processor = _processors[msgID];

                // Ensure the processor exists
                if (processor == null)
                {
                    const string errmsg = "Processor for message ID {0} is null.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, msgID);
                    Debug.Fail(string.Format(errmsg, msgID));
                    return;
                }

                // Ensure the processor contains a valid handler
                if (processor.Call == null)
                {
                    const string errmsg = "Processor for message ID {0} contains null Call delegate.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, msgID);
                    Debug.Fail(string.Format(errmsg, msgID));
                    return;
                }

                // Call the processor
                processor.Call(socket, recvReader);
            }
        }

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding <see cref="MessageProcessor"/>.
        /// </summary>
        /// <param name="recvData">IEnumerable of <see cref="SocketReceiveData"/>s to process.</param>
        public void Process(IEnumerable<SocketReceiveData> recvData)
        {
            if (recvData == null)
                return;

            // Loop through the received data collections from each socket
            foreach (SocketReceiveData rec in recvData)
            {
                Process(rec);
            }
        }

        /// <summary>
        /// Checks that the rest of the <paramref name="bitStream"/> contains only unset bits.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to check.</param>
        /// <returns>True if all of the bits in the <paramref name="bitStream"/> are unset; otherwise false.</returns>
        static bool RestOfStreamIsZero(BitStream bitStream)
        {
            while (bitStream.PositionBits < bitStream.LengthBits)
            {
                if (bitStream.ReadBool())
                    return false;
            }

            return true;
        }
    }
}