using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Network
{
    /// <summary>
    /// Server that responds to pings received from a LatencyTrackerClient. This does not actually care about who
    /// is pinging it or the latency.
    /// </summary>
    public class LatencyTrackerServer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ushort _port;
        readonly UDPSocket _socket;

        /// <summary>
        /// Gets the port that this LatencyTrackerServer is bound to.
        /// </summary>
        public int BindPort
        {
            get { return _port; }
        }

        /// <summary>
        /// LatencyTrackerServer constructor.
        /// </summary>
        /// <param name="port">Port to bind to.</param>
        public LatencyTrackerServer(int port)
        {
            _port = (ushort)port;

            _socket = new UDPSocket();
            _socket.Bind(port);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created LatencyTrackerServer bound to port `{0}`.", port);
        }

        /// <summary>
        /// Updates the buffer and parses any received data. Recommended this is called every frame.
        /// </summary>
        public void Update()
        {
            // Get the received data
            var data = _socket.GetRecvData();
            if (data == null)
                return;

            // Parse the available data
            foreach (AddressedPacket recvPacket in data)
            {
                Debug.Assert(recvPacket.Data != null);
                Debug.Assert(recvPacket.RemoteEndPoint != null);

                var packet = recvPacket.Data;

                // Ensure the length of the packet is valid
                if (packet.Length != LatencyTrackerHelper.SignatureSize)
                {
                    Debug.Fail("Received invalid data. Possibly just garbage on the channel.");
                    continue;
                }

                // Repeat the packet back to the sender
                _socket.Send(recvPacket.Data, recvPacket.RemoteEndPoint);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Ping received and resent to `{0}`.", recvPacket.RemoteEndPoint);
            }
        }
    }
}