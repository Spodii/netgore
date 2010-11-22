#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System.Linq;
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