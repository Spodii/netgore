using System.Linq;

#if !TOPDOWN
#pragma warning disable 1587
#endif

#if TOPDOWN

using System;
using System.Collections.Generic;
using System.Text;
using NetGore.IO;
using NetGore.Network;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    partial class ServerPacketHandler
    {
        [MessageHandler((uint)ClientPacketID.MoveDown)]
        void RecvMoveDown(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && !user.IsMovingDown)
                user.MoveDown();
        }

        [MessageHandler((uint)ClientPacketID.MoveStopHorizontal)]
        void RecvMoveStopHorizontal(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && (user.IsMovingLeft || user.IsMovingRight))
            {
                if (user.IsPeerTrading)
                    return;
                user.StopMovingHorizontal();
            }
        }

        [MessageHandler((uint)ClientPacketID.MoveStopVertical)]
        void RecvMoveStopVertical(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && (user.IsMovingUp || user.IsMovingDown))
            {
                if (user.IsPeerTrading)
                    return;

                user.StopMovingVertical();
            }
        }

        [MessageHandler((uint)ClientPacketID.MoveUp)]
        void RecvMoveUp(IIPSocket conn, BitStream r)
        {
            User user;
            if ((user = TryGetUser(conn)) != null && !user.IsMovingUp)
            {
                if (user.IsPeerTrading)
                    return;

                user.MoveUp();
            }
        }
    }
}

#endif