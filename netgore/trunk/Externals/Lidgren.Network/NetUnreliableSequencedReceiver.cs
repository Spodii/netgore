using System.Linq;

namespace Lidgren.Network
{
    sealed class NetUnreliableSequencedReceiver : NetReceiverChannelBase
    {
        int m_lastReceivedSequenceNumber;

        public NetUnreliableSequencedReceiver(NetConnection connection) : base(connection)
        {
        }

        internal override void ReceiveMessage(NetIncomingMessage msg)
        {
            var nr = msg.m_sequenceNumber;

            // ack no matter what
            m_connection.QueueAck(msg.m_receivedMessageType, nr);

            var relate = NetUtility.RelativeSequenceNumber(nr, m_lastReceivedSequenceNumber);
            if (relate < 0)
                return; // drop if late

            m_lastReceivedSequenceNumber = nr;
            m_peer.ReleaseMessage(msg);
        }
    }
}