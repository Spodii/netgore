#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Network;

namespace DemoGame.Client
{
    public static partial class ClientPacket
    {
        public static PacketWriter Jump()
        {
            return GetWriter(ClientPacketID.Jump);
        }
    }
}

#endif