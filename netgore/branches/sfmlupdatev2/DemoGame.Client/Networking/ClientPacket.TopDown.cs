using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System;
using System.Collections.Generic;
using System.Text;
using NetGore.Network;

namespace DemoGame.Client
{
    public static partial class ClientPacket
    {
        public static PacketWriter MoveDown()
        {
            return GetWriter(ClientPacketID.MoveDown);
        }

        public static PacketWriter MoveStopHorizontal()
        {
            return GetWriter(ClientPacketID.MoveStopHorizontal);
        }

        public static PacketWriter MoveStopVertical()
        {
            return GetWriter(ClientPacketID.MoveStopVertical);
        }

        public static PacketWriter MoveUp()
        {
            return GetWriter(ClientPacketID.MoveUp);
        }
    }
}

#endif