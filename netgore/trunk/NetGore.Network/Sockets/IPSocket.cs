using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A socket that abstracts TCP and UDP.
    /// </summary>
    public class IPSocket : IIPSocket
    {
        readonly TCPSocket _tcpSocket;
        readonly UDPSocket _udpSocket;

        internal TCPSocket TCPSocket { get { return _tcpSocket; } }

        internal UDPSocket UDPSocket { get { return _udpSocket; } }

        public IPSocket(TCPSocket tcpSocket, UDPSocket udpSocket)
        {
            if (tcpSocket == null)
                throw new ArgumentNullException("tcpSocket");
            if (udpSocket == null)
                throw new ArgumentNullException("udpSocket");

            _tcpSocket = tcpSocket;
            _udpSocket = udpSocket;
        }

        /// <summary>
        /// Gets the IP address string that this IIPSocket is connected to.
        /// </summary>
        public string Address
        {
            get { return _tcpSocket.Address; }
        }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the IIPSocket itself.
        /// </summary>
        public object Tag
        {
            get { return _tcpSocket.Tag; }
            set { _tcpSocket.Tag = value; }
        }

        /// <summary>
        /// Gets the queue of received data.
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data.</returns>
        public byte[][] GetRecvData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends data over a reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(BitStream data)
        {
            Send(data, true);
        }

        EndPoint _udpEndPoint;

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        public void Send(BitStream data, bool reliable)
        {
            if (reliable)
            {
                _tcpSocket.Send(data);
            }
            else
            {
                if (_udpEndPoint == null)
                    _udpEndPoint = _udpSocket.Send(data.GetBuffer(), Address);
                else
                    _udpSocket.Send(data.GetBuffer(), _udpEndPoint);
            }
        }
    }
}
