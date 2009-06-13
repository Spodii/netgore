using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace NetGore.Network
{
    public class LatencyTrackerClient : LatencyTrackerBase
    {
        /// <summary>
        /// EndPoint that we will be pinging.
        /// </summary>
        readonly EndPoint _endPoint;

        /// <summary>
        /// Keeps track of how long the response took for the ping.
        /// </summary>
        readonly Stopwatch _pingTimer = new Stopwatch();

        /// <summary>
        /// Buffer that will be used to send our pings. This will be recycled so we don't create garbage for every send.
        /// </summary>
        readonly byte[] _sendBuffer = new byte[SignatureSize];

        readonly UDPSocket _socket;

        /// <summary>
        /// The latency of this connection.
        /// </summary>
        ushort _latency;

        /// <summary>
        /// The signature of the last ping we sent. This ensures that we listen for the correct ping, not just any
        /// data received on the channel.
        /// </summary>
        int _pingSignature;

        /// <summary>
        /// If we are currently waiting for a response from our ping.
        /// </summary>
        bool _waitingForPong;

        /// <summary>
        /// Gets if we are already waiting for a ping response.
        /// </summary>
        public bool IsBusy
        {
            get { return _waitingForPong; }
        }

        /// <summary>
        /// Gets the latency of this connection in milliseconds.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
        }

        public LatencyTrackerClient(string hostAddress, int hostPort)
        {
            // Find the remote endpoint
            IPAddress ipAddress = IPAddress.Parse(hostAddress);
            if (ipAddress == null)
                throw new ArgumentException("hostAddress");

            IPEndPoint endPoint = new IPEndPoint(ipAddress, hostPort);
            _endPoint = endPoint;

            // Create the socket
            _socket = new UDPSocket();
            _socket.Bind();
        }

        /// <summary>
        /// Begins the latency check. If IsBusy is true, a ping is already in progress. In this case, calling Ping()
        /// again will drop that request. So calling Ping() before the pings can respond will result in the Latency
        /// never updating.
        /// </summary>
        public void Ping()
        {
            _waitingForPong = true;

            // Increment the signature. Doesn't really matter how we increment it, just that we do and that
            // the resulting value isn't the same as the last 100-or-so pings.
            _pingSignature += 11;

            // Build the data and send it
            WriteSignature(_sendBuffer, _pingSignature, 0);
            _socket.Send(_sendBuffer, _endPoint);

            // Start the timer
            _pingTimer.Reset();
            _pingTimer.Start();
        }

        /// <summary>
        /// Sets the latency.
        /// </summary>
        /// <param name="roundTripTime">The round-trip ping time in milliseconds.</param>
        void SetLatency(int roundTripTime)
        {
            Debug.Assert(roundTripTime >= 0, "How did we get a negative latency?");

            // If the latency is over ushort.MaxValue, just push it down since that is already insanely high
            if (roundTripTime > ushort.MaxValue)
                roundTripTime = ushort.MaxValue;

            // Divide the ping by two to get the approximate one-way time
            _latency = (ushort)(roundTripTime / 2);
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

                // Grab the signature, which should be the same as what we sent
                int recvSignature = ReadSignature(packet, 0);
                if (recvSignature != _pingSignature)
                    continue;

                // Signature matches! This is the packet we wanted, so stop the timer
                _pingTimer.Stop();
                SetLatency((int)_pingTimer.ElapsedMilliseconds);
            }
        }
    }
}