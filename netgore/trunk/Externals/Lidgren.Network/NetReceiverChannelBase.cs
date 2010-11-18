using System.Linq;

namespace Lidgren.Network
{
    abstract class NetReceiverChannelBase
    {
        internal NetConnection m_connection;
        internal NetPeer m_peer;

        public NetReceiverChannelBase(NetConnection connection)
        {
            m_connection = connection;
            m_peer = connection.m_peer;
        }

        internal abstract void ReceiveMessage(NetIncomingMessage msg);
    }
}