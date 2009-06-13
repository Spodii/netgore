using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using log4net;

namespace NetGore.Network
{
    /// <summary>
    /// Client that pings a LatencyTrackerServer to find the latency between the location of this LatencyTrackerClient
    /// and the target LatencyTrackerServer.
    /// </summary>
    public class LatencyTrackerClient
    {
        /// <summary>
        /// Default size of the latency buffer.
        /// </summary>
        const int _defaultBufferSize = 3;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        readonly byte[] _sendBuffer = new byte[LatencyTrackerHelper.SignatureSize];

        readonly UDPSocket _socket;

        /// <summary>
        /// The buffer of latencies.
        /// </summary>
        readonly ushort[] _latencies = new ushort[_defaultBufferSize];

        /// <summary>
        /// The current position in the latencies buffer.
        /// </summary>
        byte _latencyBufferPos;
    
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
        /// The calculated average latency.
        /// </summary>
        ushort _latency;

        /// <summary>
        /// Gets the average latency of this connection in milliseconds.
        /// </summary>
        public int Latency
        {
            get { return _latency; }
        }

        /// <summary>
        /// Gets the number of latencies are buffered. The greater this value, the greater time-span and range of lantencies
        /// are used when calculating the Latency property value.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return _latencies.Length;
            }
        }

        /// <summary>
        /// LatencyTrackerClient constructor.
        /// </summary>
        /// <param name="hostAddress">Remote address of the LatencyTrackerServer to ping.</param>
        /// <param name="hostPort">Remote port of the LatencyTrackerServer to ping.</param>
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

            if (log.IsInfoEnabled)
                log.InfoFormat("Created LatencyTrackerClient to ping remote address `{0}:{1}`.", hostAddress, hostPort);
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
            LatencyTrackerHelper.WriteSignature(_sendBuffer, _pingSignature, 0);
            _socket.Send(_sendBuffer, _endPoint);

            // Start the timer
            _pingTimer.Reset();
            _pingTimer.Start();

            if (log.IsInfoEnabled)
                log.InfoFormat("Ping sent to remote address `{0}`.", _endPoint);
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
            ushort latency = (ushort)(roundTripTime / 2);

            // Add the latency to the buffer
            // If this is the first time adding to the buffer, flood the whole buffer
            if (IsFirstLatencyUpdate)
            {
                for (int i = 0; i < _latencies.Length; i++)
                    _latencies[i] = latency;
            }
            else
            {
                _latencies[_latencyBufferPos] = latency;
            }

            // Increment the buffer index, or roll back to the first index if needed
            if (_latencyBufferPos + 1 >= _latencies.Length)
                _latencyBufferPos = 0;
            else
                _latencyBufferPos++;

            // Recalculate the average latency
            _latency = (ushort)_latencies.Average(x => x);

            if (log.IsInfoEnabled)
                log.InfoFormat("Ping had a one-way latency of `{0}`.", _latency);
        }

        /// <summary>
        /// Gets if this is the first time the latency buffer has been updated.
        /// </summary>
        bool IsFirstLatencyUpdate
        {
            get
            {
                // Average latency will be 0 if it has never been updated
                if (_latency != 0)
                    return false;

                // All latencies will be 0, too
                foreach (var latency in _latencies)
                {
                    if (latency != 0)
                        return false;
                }

                return true;
            }
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
                if (packet.Length != LatencyTrackerHelper.SignatureSize)
                {
                    Debug.Fail("Received invalid data. Possibly just garbage on the channel.");
                    continue;
                }

                // Grab the signature, which should be the same as what we sent
                int recvSignature = LatencyTrackerHelper.ReadSignature(packet, 0);
                if (recvSignature != _pingSignature)
                    continue;

                // Signature matches! This is the packet we wanted, so stop the timer
                _pingTimer.Stop();
                _waitingForPong = false;
                SetLatency((int)_pingTimer.ElapsedMilliseconds);

                // We can break since we found the only thing we were looking for
                break;
            }
        }
    }
}