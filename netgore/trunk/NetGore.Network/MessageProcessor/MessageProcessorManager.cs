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
    /// Manages a group of message processors.
    /// </summary>
    public class MessageProcessorManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly MessageProcessor[] _processors;

        /// <summary>
        /// Gets an IEnumerable of all the MessageProcessors handled by this MessageProcessorManager.
        /// </summary>
        public IEnumerable<MessageProcessor> Processors
        {
            get { return _processors; }
        }

        /// <summary>
        /// Loads a list of all the found message processors. The array will always
        /// contain 256 indices even if theres isn't a message processor for all
        /// of them. Only messages marked with the MessageHandler attribute
        /// will be found and returned.
        /// </summary>
        /// <param name="source">Root object instance containing all the classes (null if static)</param>
        /// <returns>Returns a list of all the found message processors for a given class</returns>
        public MessageProcessorManager(object source)
        {
            // Store the types we will use
            Type mpdType = typeof(MessageProcessorDelegate);
            Type atbType = typeof(MessageHandlerAttribute);
            Type voidType = Type.GetType("System.Void");

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
                        MessageProcessorDelegate del = (MessageProcessorDelegate)Delegate.CreateDelegate(mpdType, source, method);
                        _processors[atb.MsgID] = new MessageProcessor(atb.MsgID, del);
                    }
                }
            }
        }

        /// <summary>
        /// Handles received data and forwards it to the corresponding MessageProcessors
        /// </summary>
        /// <param name="rec">SocketReceiveData to process</param>
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
        /// Handles received data and forwards it to the corresponding MessageProcessors
        /// </summary>
        /// <param name="socket">Socket the data came from</param>
        /// <param name="data">Data to process</param>
        public void Process(TCPSocket socket, byte[] data)
        {
            if (socket == null)
            {
                Debug.Fail("socket is null.");
                return;
            }
            if (data == null)
            {
                Debug.Fail("data is null.");
                return;
            }
            if (data.Length == 0)
            {
                Debug.Fail("data array is empty.");
                return;
            }

            // Create a BitStream to read the data
            BitStream recvReader = new BitStream(data);

            // Loop through the data until it is emptied
            while (recvReader.PositionBytes < data.Length)
            {
                // Get the ID of the message
                byte msgID = recvReader.ReadByte();

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
        /// Handles a list of received data and forwards it to the corresponding MessageProcessors
        /// </summary>
        /// <param name="recvData">List of SocketReceiveData to process</param>
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
        /// Checks that the rest of the bitStream contains only unset bits.
        /// </summary>
        /// <param name="bitStream">BitStream to check.</param>
        /// <returns>True if all of the bits in the <paramref name="bitStream"/> are unset, else false.</returns>
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