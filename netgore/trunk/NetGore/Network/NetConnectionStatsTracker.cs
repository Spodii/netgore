using System.Linq;
using Lidgren.Network;

namespace NetGore.Network
{
    /// <summary>
    /// A snapshot of the <see cref="NetPeerStatistics"/> at a given time.
    /// </summary>
    public class NetPeerStatisticsSnapshot
    {
        readonly int _recvBytes;
        readonly int _recvMsg;
        readonly int _recvPackets;
        readonly int _sentBytes;
        readonly int _sentMsg;
        readonly int _sentPackets;
        readonly TickCount _time = TickCount.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPeerStatisticsSnapshot"/> class.
        /// </summary>
        public NetPeerStatisticsSnapshot(int recvBytes, int recvPackets, int recvMsg, int sentBytes, int sentPackets, int sentMsg)
        {
            _recvBytes = recvBytes;
            _recvPackets = recvPackets;
            _recvMsg = recvMsg;

            _sentBytes = sentBytes;
            _sentPackets = sentPackets;
            _sentMsg = sentMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPeerStatisticsSnapshot"/> class.
        /// </summary>
        /// <param name="stats">The <see cref="NetPeerStatistics"/> to copy the values from.</param>
        public NetPeerStatisticsSnapshot(NetPeerStatistics stats)
        {
            _recvBytes = stats.ReceivedBytes;
            _recvPackets = stats.ReceivedPackets;
            _recvMsg = stats.ReceivedMessages;

            _sentBytes = stats.SentBytes;
            _sentPackets = stats.SentPackets;
            _sentMsg = stats.SentMessages;
        }

        public int ReceivedBytes
        {
            get { return _recvBytes; }
        }

        public int ReceivedMessages
        {
            get { return _recvMsg; }
        }

        public int ReceivedPackets
        {
            get { return _recvPackets; }
        }

        public int SentBytes
        {
            get { return _sentBytes; }
        }

        public int SentMessages
        {
            get { return _sentMsg; }
        }

        public int SentPackets
        {
            get { return _sentPackets; }
        }

        /// <summary>
        /// Gets the <see cref="TickCount"/> at which the values in this <see cref="NetPeerStatisticsSnapshot"/> were captured.
        /// </summary>
        public TickCount Time
        {
            get { return _time; }
        }

        /// <summary>
        /// Gets a <see cref="NetPeerStatisticsSnapshot"/> containing the changes in values in the <see cref="NetPeerStatistics"/>
        /// since the given <see cref="NetPeerStatisticsSnapshot"/> was made.
        /// </summary>
        /// <param name="a">The current <see cref="NetConnectionStatistics"/> containing the up-to-date values.</param>
        /// <param name="b">The <see cref="NetPeerStatisticsSnapshot"/> containing the base values.</param>
        /// <returns>A <see cref="NetPeerStatisticsSnapshot"/> containing the changes in values in the <see cref="NetPeerStatistics"/>
        /// since the given <see cref="NetPeerStatisticsSnapshot"/> was made.</returns>
        public static NetPeerStatisticsSnapshot Diff(NetPeerStatistics a, NetPeerStatisticsSnapshot b)
        {
            var ret = new NetPeerStatisticsSnapshot(a.ReceivedBytes - b.ReceivedBytes, a.ReceivedPackets - b.ReceivedPackets,
                a.ReceivedMessages - b.ReceivedMessages, a.SentBytes - b.SentBytes, a.SentPackets - b.SentPackets,
                a.SentMessages - b.SentMessages);
            return ret;
        }
    }
}