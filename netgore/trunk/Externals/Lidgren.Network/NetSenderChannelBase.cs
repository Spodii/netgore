using System.Linq;

namespace Lidgren.Network
{
    abstract class NetSenderChannelBase
    {
        // access this directly to queue things in this channel
        internal NetQueue<NetOutgoingMessage> m_queuedSends;

        internal abstract int WindowSize { get; }

        internal abstract NetSendResult Enqueue(NetOutgoingMessage message);

        internal abstract int GetAllowedSends();

        internal abstract void ReceiveAcknowledge(float now, int sequenceNumber);

        internal abstract void Reset();

        internal abstract void SendQueuedMessages(float now);
    }
}