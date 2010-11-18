using System.Linq;
using System.Threading;

namespace Lidgren.Network
{
    /// <summary>
    /// Sender part of Selective repeat ARQ for a particular NetChannel
    /// </summary>
    sealed class NetUnreliableSenderChannel : NetSenderChannelBase
    {
        readonly NetConnection m_connection;
        readonly NetBitVector m_receivedAcks;
        readonly int m_windowSize;
        int m_sendStart;
        int m_windowStart;

        internal NetUnreliableSenderChannel(NetConnection connection, int windowSize)
        {
            m_connection = connection;
            m_windowSize = windowSize;
            m_windowStart = 0;
            m_sendStart = 0;
            m_receivedAcks = new NetBitVector(NetConstants.NumSequenceNumbers);
            m_queuedSends = new NetQueue<NetOutgoingMessage>(8);
        }

        internal override int WindowSize
        {
            get { return m_windowSize; }
        }

        internal override NetSendResult Enqueue(NetOutgoingMessage message)
        {
            var queueLen = m_queuedSends.Count + 1;
            var left = m_windowSize -
                       ((m_sendStart + NetConstants.NumSequenceNumbers) - m_windowStart) % NetConstants.NumSequenceNumbers;
            if (queueLen > left)
                return NetSendResult.Dropped;

            m_queuedSends.Enqueue(message);
            return NetSendResult.Sent;
        }

        // call this regularely

        void ExecuteSend(float now, NetOutgoingMessage message)
        {
            m_connection.m_peer.VerifyNetworkThread();

            var seqNr = m_sendStart;
            m_sendStart = (m_sendStart + 1) % NetConstants.NumSequenceNumbers;

            m_connection.QueueSendMessage(message, seqNr);

            Interlocked.Decrement(ref message.m_recyclingCount);
            if (message.m_recyclingCount <= 0)
                m_connection.m_peer.Recycle(message);

            return;
        }

        internal override int GetAllowedSends()
        {
            var retval = m_windowSize - ((m_sendStart + NetConstants.NumSequenceNumbers) - m_windowStart) % m_windowSize;
            NetException.Assert(retval >= 0 && retval <= m_windowSize);
            return retval;
        }

        // remoteWindowStart is remote expected sequence number; everything below this has arrived properly
        // seqNr is the actual nr received
        internal override void ReceiveAcknowledge(float now, int seqNr)
        {
            // late (dupe), on time or early ack?
            var relate = NetUtility.RelativeSequenceNumber(seqNr, m_windowStart);

            if (relate < 0)
            {
                //m_connection.m_peer.LogDebug("Received late/dupe ack for #" + seqNr);
                return; // late/duplicate ack
            }

            if (relate == 0)
            {
                //m_connection.m_peer.LogDebug("Received right-on-time ack for #" + seqNr);

                // ack arrived right on time
                NetException.Assert(seqNr == m_windowStart);

                m_receivedAcks[m_windowStart] = false;
                m_windowStart = (m_windowStart + 1) % NetConstants.NumSequenceNumbers;

                return;
            }

            // Advance window to this position
            m_receivedAcks[seqNr] = true;

            while (m_windowStart != seqNr)
            {
                m_receivedAcks[m_windowStart] = false;
                m_windowStart = (m_windowStart + 1) % NetConstants.NumSequenceNumbers;
            }
        }

        internal override void Reset()
        {
            m_receivedAcks.Clear();
            m_queuedSends.Clear();
            m_windowStart = 0;
            m_sendStart = 0;
        }

        internal override void SendQueuedMessages(float now)
        {
            var num = GetAllowedSends();
            if (num < 1)
                return;

            // queued sends
            while (m_queuedSends.Count > 0 && num > 0)
            {
                NetOutgoingMessage om;
                if (m_queuedSends.TryDequeue(out om))
                    ExecuteSend(now, om);
                num--;
            }
        }
    }
}