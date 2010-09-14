using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Manages a group of message processing methods, each identified by the attribute
    /// <see cref="MessageHandlerAttribute"/>.
    /// </summary>
    public class MessageProcessorManager : IMessageProcessorManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly byte _messageIDBitLength;
        readonly IMessageProcessor[] _processors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorManager"/> class.
        /// </summary>
        /// <param name="source">Root object instance containing all the classes (null if static).</param>
        /// <param name="messageIDBitLength">The length of the message ID in bits. Must be between a value
        /// greater than or equal to 1, and less than or equal to 32.</param>
        /// <returns>Returns a list of all the found message processors for a given class.</returns>
        public MessageProcessorManager(object source, int messageIDBitLength)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (messageIDBitLength > 32 || messageIDBitLength < 1)
                throw new ArgumentOutOfRangeException("messageIDBitLength");

            _messageIDBitLength = (byte)messageIDBitLength;

            _processors = BuildMessageProcessors(source);
        }

        /// <summary>
        /// Builds the array of <see cref="IMessageProcessor"/>s.
        /// </summary>
        /// <param name="source">Root object instance containing all the classes (null if static).</param>
        /// <returns>The array of <see cref="IMessageProcessor"/>s.</returns>
        IMessageProcessor[] BuildMessageProcessors(object source)
        {
            const BindingFlags bindFlags =
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.Static;

            var mpdType = typeof(MessageProcessorHandler);
            var atbType = typeof(MessageHandlerAttribute);
            var voidType = typeof(void);

            var tmpProcessors = new DArray<IMessageProcessor>();

            // Search through all types in the Assembly
            var assemb = Assembly.GetAssembly(source.GetType());
            foreach (var type in assemb.GetTypes())
            {
                // Search through every method in the class
                foreach (var method in type.GetMethods(bindFlags))
                {
                    // Only accept a method if it returns a void
                    if (method.ReturnType != voidType)
                        continue;

                    // Get all of the MessageAttributes for the method (should only be one)
                    var atbs = (MessageHandlerAttribute[])method.GetCustomAttributes(atbType, true);
                    if (atbs.Length > 1)
                    {
                        const string errmsg = "Multiple MessageHandlerAttributes found for method `{0}`.";
                        Debug.Fail(string.Format(errmsg, method.Name));
                        throw new Exception(string.Format(errmsg, method.Name));
                    }

                    // Create the message processor for the method
                    foreach (var atb in atbs)
                    {
                        if (tmpProcessors.CanGet(atb.MsgID) && tmpProcessors[atb.MsgID] != null)
                        {
                            const string errmsg =
                                "A MessageHandlerAttribute with ID `{0}` already exists. Methods in question: {1} and {2}";
                            Debug.Fail(string.Format(errmsg, atb.MsgID, tmpProcessors[atb.MsgID].Call.Method, method));
                            throw new Exception(string.Format(errmsg, atb.MsgID, tmpProcessors[atb.MsgID].Call.Method, method));
                        }

                        var del = (MessageProcessorHandler)Delegate.CreateDelegate(mpdType, source, method);
                        Debug.Assert(del != null);
                        var msgProcessor = CreateMessageProcessor(atb, del);
                        tmpProcessors.Insert(atb.MsgID, msgProcessor);
                    }
                }
            }

            return tmpProcessors.ToArray();
        }

        /// <summary>
        /// Creates a <see cref="IMessageProcessor"/>. Can be overridden in the derived classes to provide a different
        /// <see cref="IMessageProcessor"/> implementation. 
        /// </summary>
        /// <param name="atb">The <see cref="MessageHandlerAttribute"/> that was attached to the method that the
        /// <see cref="IMessageProcessor"/> is being created for.</param>
        /// <param name="handler">The <see cref="MessageProcessorHandler"/> for invoking the method.</param>
        /// <returns>The <see cref="IMessageProcessor"/> instance.</returns>
        protected virtual IMessageProcessor CreateMessageProcessor(MessageHandlerAttribute atb, MessageProcessorHandler handler)
        {
            return new MessageProcessor(atb.MsgID, handler);
        }

        /// <summary>
        /// Creates the <see cref="BitStream"/> to read the data.
        /// </summary>
        /// <param name="data">The data to read.</param>
        /// <returns>The <see cref="BitStream"/> to read the <paramref name="data"/>.</returns> 
        protected virtual BitStream CreateReader(byte[] data)
        {
            return new BitStream(data);
        }

        /// <summary>
        /// Gets the <see cref="IMessageProcessor"/> with the given ID. This will only ever return the <see cref="IMessageProcessor"/>
        /// for the method that was specified using a <see cref="MessageHandlerAttribute"/>. Is intended to only be used by
        /// <see cref="MessageProcessorManager.GetMessageProcessor"/> to allow for access of the <see cref="IMessageProcessor"/>s loaded
        /// through the attribute.
        /// </summary>
        /// <param name="msgID">The ID of the message.</param>
        /// <returns>The <see cref="IMessageProcessor"/> for the <paramref name="msgID"/>, or null if an invalid ID.</returns>
        protected IMessageProcessor GetInternalMessageProcessor(MessageProcessorID msgID)
        {
            return _processors[msgID];
        }

        /// <summary>
        /// Gets the <see cref="IMessageProcessor"/> for the corresponding message ID. Can be overridden by the derived class to allow
        /// for using a <see cref="IMessageProcessor"/> other than what is acquired by
        /// <see cref="MessageProcessorManager.GetInternalMessageProcessor"/>.
        /// </summary>
        /// <param name="msgID">The ID of the message.</param>
        /// <returns>The <see cref="IMessageProcessor"/> for the <paramref name="msgID"/>, or null if an invalid ID.</returns>
        protected virtual IMessageProcessor GetMessageProcessor(MessageProcessorID msgID)
        {
            return GetInternalMessageProcessor(msgID);
        }

        /// <summary>
        /// Invokes the <see cref="IMessageProcessor"/> for handle processing a message block.
        /// </summary>
        /// <param name="socket">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="processor">The <see cref="IMessageProcessor"/> to invoke.</param>
        /// <param name="reader">The <see cref="BitStream"/> containing the data to process.</param>
        protected virtual void InvokeProcessor(IIPSocket socket, IMessageProcessor processor, BitStream reader)
        {
            processor.Call(socket, reader);
        }

        /// <summary>
        /// Reads the next message ID.
        /// </summary>
        /// <param name="reader">The <see cref="BitStream"/> to read the ID from.</param>
        /// <returns>The next message ID.</returns>
        protected virtual MessageProcessorID ReadMessageID(BitStream reader)
        {
            return (MessageProcessorID)reader.ReadUInt(_messageIDBitLength);
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
                // TODO: !! Optimize by using bitStream.ReadInt() to read up to 32 bits at once
                if (bitStream.ReadBool())
                    return false;
            }

            return true;
        }

        #region IMessageProcessorManager Members

        /// <summary>
        /// Gets the <see cref="IMessageProcessor"/>s handled by this <see cref="IMessageProcessorManager"/>.
        /// </summary>
        public IEnumerable<IMessageProcessor> Processors
        {
            get { return _processors.Where(x => x != null); }
        }

        /// <summary>
        /// Handles received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="socket"><see cref="IIPSocket"/> the data came from.</param>
        /// <param name="data">Data to process.</param>
        public void Process(IIPSocket socket, BitStream data)
        {
            ThreadAsserts.IsMainThread();

            // Validate the arguments
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

            Debug.Assert(data.Mode == BitStreamMode.Read);
            
            // Loop through the data until it is emptied
            while (data.PositionBits < data.LengthBits)
            {
                // Get the ID of the message
                var msgID = ReadMessageID(data);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Parsing message ID `{0}`. Stream now at bit position `{1}`.", msgID, data.PositionBits);

                // If we get a message ID of 0, we have likely hit the end
                if (msgID == 0)
                {
#if DEBUG
                    if (!RestOfStreamIsZero(data))
                    {
                        const string errmsg = "Encountered msgID = 0, but there was set bits remaining in the stream.";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg);
                        Debug.Fail(errmsg);
                    }
#endif
                    return;
                }

                // Find the corresponding processor and call it
                var processor = GetMessageProcessor(msgID);

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
                InvokeProcessor(socket, processor, data);
            }
        }

        /// <summary>
        /// Handles a list of received data and forwards it to the corresponding <see cref="IMessageProcessor"/>.
        /// </summary>
        /// <param name="recvData">IEnumerable of <see cref="BitStream"/> to process and the corresponding
        /// <see cref="IIPSocket"/> that the data came from.</param>
        public void Process(IEnumerable<KeyValuePair<IIPSocket, BitStream>> recvData)
        {
            if (recvData == null)
                return;

            // Loop through the received data collections from each socket
            foreach (var rec in recvData)
            {
                Process(rec.Key, rec.Value);
            }
        }

        #endregion
    }
}