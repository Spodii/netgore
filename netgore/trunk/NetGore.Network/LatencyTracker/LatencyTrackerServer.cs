using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetGore.Network
{
    public class LatencyTrackerServer : LatencyTrackerBase
    {
        readonly UDPSocket _socket;
        readonly ushort _port;

        /// <summary>
        /// Gets the port that this LatencyTrackerServer is bound to.
        /// </summary>
        public int BindPort { get { return _port; } }

        /// <summary>
        /// LatencyTrackerServer constructor.
        /// </summary>
        /// <param name="port">Port to bind to.</param>
        public LatencyTrackerServer(int port)
        {
            _port = (ushort)port;

            _socket = new UDPSocket();
            _socket.Bind(port);
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
            foreach (var recvPacket in data)
            {
                Debug.Assert(recvPacket.Data != null);
                Debug.Assert(recvPacket.RemoteEndPoint != null);

                byte[] packet = recvPacket.Data;

                // Ensure the length of the packet is valid
                if (packet.Length != SignatureSize)
                {
                    Debug.Fail("Received invalid data. Possibly just garbage on the channel.");
                    continue;
                }

                // Repeat the packet back to the sender
                _socket.Send(recvPacket.Data, recvPacket.RemoteEndPoint);
            }
        }
    }
}
