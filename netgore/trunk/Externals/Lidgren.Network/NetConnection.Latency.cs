using System.Linq;

namespace Lidgren.Network
{
    public partial class NetConnection
    {
        float m_averageRoundtripTime;
        int m_sentPingNumber;
        float m_sentPingTime;
        float m_timeoutDeadline = float.MaxValue;

        /// <summary>
        /// Gets the current average roundtrip time in seconds
        /// </summary>
        public float AverageRoundtripTime
        {
            get { return m_averageRoundtripTime; }
        }

        internal void ReceivedPong(float now, int pongNumber)
        {
            if (pongNumber != m_sentPingNumber)
            {
                m_peer.LogVerbose("Ping/Pong mismatch; dropped message?");
                return;
            }

            m_timeoutDeadline = now + m_peerConfiguration.m_connectionTimeout;

            var rtt = now - m_sentPingTime;
            NetException.Assert(rtt >= 0);

            if (m_averageRoundtripTime < 0)
            {
                m_averageRoundtripTime = rtt; // initial estimate
                m_peer.LogDebug("Initiated average roundtrip time to " + NetTime.ToReadable(m_averageRoundtripTime));
            }
            else
            {
                m_averageRoundtripTime = (m_averageRoundtripTime * 0.7f) + (rtt * 0.3f);
                m_peer.LogVerbose("Updated average roundtrip time to " + NetTime.ToReadable(m_averageRoundtripTime));
            }

            // update resend delay for all channels
            var resendDelay = GetResendDelay();
            foreach (var chan in m_sendChannels)
            {
                var rchan = chan as NetReliableSenderChannel;
                if (rchan != null)
                    rchan.m_resendDelay = resendDelay;
            }

            m_peer.LogVerbose("Timeout deadline pushed to  " + m_timeoutDeadline);
        }

        internal void SendPing()
        {
            m_peer.VerifyNetworkThread();

            m_sentPingNumber++;
            if (m_sentPingNumber >= 256)
                m_sentPingNumber = 0;
            m_sentPingTime = (float)NetTime.Now;
            var om = m_peer.CreateMessage(1);
            om.Write((byte)m_sentPingNumber);
            om.m_messageType = NetMessageType.Ping;

            var len = om.Encode(m_peer.m_sendBuffer, 0, 0);
            bool connectionReset;
            m_peer.SendPacket(len, m_remoteEndpoint, 1, out connectionReset);
        }

        internal void SendPong(int pingNumber)
        {
            m_peer.VerifyNetworkThread();

            var om = m_peer.CreateMessage(1);
            om.Write((byte)pingNumber);
            om.m_messageType = NetMessageType.Pong;

            var len = om.Encode(m_peer.m_sendBuffer, 0, 0);
            bool connectionReset;
            m_peer.SendPacket(len, m_remoteEndpoint, 1, out connectionReset);
        }
    }
}