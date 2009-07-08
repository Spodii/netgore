using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Queues data to be sent on a socket, concatenating as much as possible while preserving the data
    /// and ordering, then returning the data to the socket when dequeued. This class is thread-safe.
    /// </summary>
    public class SocketSendQueue
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ushort _maxMessageSize;
        readonly object _queueLock = new object();

        /// <summary>
        /// Queue of messages that have surpassed the maximum size of the _sendStream. Anything
        /// in this queue needs to be sent before the _sendStream is sent.
        /// </summary>
        readonly Queue<byte[]> _sendQueue = new Queue<byte[]>(1);

        /// <summary>
        /// Stream used to hold the messages currently ready to be sent. New messages will be written to this
        /// BitStream until it exceeds the MaxMessageSize. If that happens, the stream will be flushed and
        /// copied to the _sendQueue.
        /// </summary>
        readonly BitStream _sendStream;

        /// <summary>
        /// Gets the maximum size of each message in bytes. When concatenating sends, the messages will never
        /// exceed this size.
        /// </summary>
        public int MaxMessageSize
        {
            get { return _maxMessageSize; }
        }

        /// <summary>
        /// SocketSendQueue constructor.
        /// </summary>
        /// <param name="maxMessageSize">The maximum size of each message in bytes. When concatenating sends, the
        /// messages will never exceed this size.</param>
        public SocketSendQueue(int maxMessageSize)
        {
            if (maxMessageSize > ushort.MaxValue || maxMessageSize < 1)
                throw new ArgumentOutOfRangeException("maxMessageSize");

            _maxMessageSize = (ushort)maxMessageSize;

            _sendStream = new BitStream(new byte[MaxMessageSize])
                          { WriteMode = BitStreamBufferMode.Static, Mode = BitStreamMode.Write };
        }

        /// <summary>
        /// Adds data to the _sendStream. If the _sendStream's size will exceed MaxMessageSize when adding the
        /// data from the <paramref name="sourceStream"/>, the _sendStream will first be flushed, the data will
        /// be added to the _sendQueue, then the content from the <paramref name="sourceStream"/> will be
        /// copied to the _sendStream. You MUST acquire the _queueLock when calling this!
        /// </summary>
        /// <param name="sourceStream">BitStream containing the data to add to the _sendStream.</param>
        void AddToSendStream(BitStream sourceStream)
        {
            int buildStreamLenBits = sourceStream.LengthBits;

            // Check for data in the stream
            if (buildStreamLenBits == 0)
            {
                const string errmsg = "Send() call contained no data to be sent.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // Check for too much data in the stream
            if (buildStreamLenBits > MaxMessageSize * 8)
            {
                const string errmsg = "Send() call contained too much data [`{0}` bytes] to be sent! Unable to send.";
                if (log.IsFatalEnabled)
                    log.FatalFormat(errmsg, buildStreamLenBits);
                Debug.Fail(string.Format(errmsg, buildStreamLenBits));
                return;
            }

            // Check if the temporary stream can fit into the primary stream
            if (_sendStream.LengthBits + buildStreamLenBits > MaxMessageSize * 8)
            {
                // Did not fit, so we have to dump the primary stream's contents first
                var bufferCopy = _sendStream.GetBufferCopy();
                _sendQueue.Enqueue(bufferCopy);
                _sendStream.Reset();
            }

            // Add the build stream's contents to the primary stream
            _sendStream.Write(sourceStream);
        }

        /// <summary>
        /// Dequeues a byte array containing the next message to be sent, or null if the queue is empty.
        /// </summary>
        /// <returns>A byte array for message to be sent. The length of this message is guarenteed to be
        /// less than or equal to MaxMessageSize.</returns>
        public byte[] Dequeue()
        {
            lock (_queueLock)
            {
                // Check the queue
                if (_sendQueue.Count > 0)
                {
                    var ret = _sendQueue.Dequeue();
                    return ret;
                }

                // Check the stream
                if (_sendStream.LengthBits > 0)
                {
                    var ret = _sendStream.GetBufferCopy();
                    _sendStream.Reset();
                    return ret;
                }

                // Nothing available
                return null;
            }
        }

        /// <summary>
        /// Enqueues data into the SocketSendQueue.
        /// </summary>
        /// <param name="bitStream">A BitStream containing the data to be sent.</param>
        public void Enqueue(BitStream bitStream)
        {
            if (bitStream == null)
                throw new ArgumentNullException("bitStream");

            if (bitStream.Mode == BitStreamMode.Read)
            {
                const string errmsg = "The sourceStream should already be in Write mode.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);

                bitStream.Mode = BitStreamMode.Write;
            }

            lock (_queueLock)
            {
                AddToSendStream(bitStream);
            }
        }
    }
}