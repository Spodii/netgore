using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Network;

// TODO: Rename to StatSynchronizeHandler

namespace DemoGame.Server
{
    public delegate void StatUpdateHandler(UserStats userStats, IStat stat);

    static class StatUpdateHandlers
    {
        /// <summary>
        /// Updates only the owner of the IStat, using ServerPacket.UpdateStat().
        /// </summary>
        /// <param name="userStats">Stat collection that the <paramref name="stat"/> came from.</param>
        /// <param name="stat">IStat to use for the update.</param>
        public static void UpdateOwner(UserStats userStats, IStat stat)
        {
            using (PacketWriter statPacket = ServerPacket.UpdateStat(stat, userStats.StatCollectionType))
            {
                userStats.User.Send(statPacket);
            }
        }
    }
}