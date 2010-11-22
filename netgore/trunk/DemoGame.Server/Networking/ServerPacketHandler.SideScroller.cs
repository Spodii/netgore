#if TOPDOWN
#pragma warning disable 1587
#endif

#if !TOPDOWN

using System.Linq;
using NetGore.IO;
using NetGore.Network;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    partial class ServerPacketHandler
    {
        [MessageHandler((uint)ClientPacketID.Jump)]
        void RecvJump(IIPSocket conn, BitStream r)
        {
            User user;
            if (((user = TryGetUser(conn)) != null) && user.CanJump)
            {
                if (user.IsPeerTrading)
                    return;

                user.Jump();
            }
        }
    }
}

#endif