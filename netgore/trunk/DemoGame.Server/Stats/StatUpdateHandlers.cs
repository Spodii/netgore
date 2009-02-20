using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Network;

namespace DemoGame.Server
{
    public delegate void StatUpdateHandler(IUserStat userStat);

    static class StatUpdateHandlers
    {
        /// <summary>
        /// Updates only the owner of the UserStat, using ServerPacket.UpdateStat 
        /// </summary>
        /// <param name="userStat">UserStat to use for the update</param>
        public static void UpdateOwner(IUserStat userStat)
        {
            using (PacketWriter statPacket = ServerPacket.UpdateStat(userStat))
            {
                userStat.User.Send(statPacket);
            }
        }
    }
}