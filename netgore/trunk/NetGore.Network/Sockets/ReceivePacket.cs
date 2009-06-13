using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NetGore.Network
{
    public struct ReceivePacket
    {
        readonly public byte[] Data;
        readonly public EndPoint RemoteEndPoint;

        public ReceivePacket(byte[] data, EndPoint remoteEndPoint)
        {
            Data = data;
            RemoteEndPoint = remoteEndPoint;
        }
    }
}
