using System;
using NetGore;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Server
{
    public class UserStats : CharacterStats
    {
        readonly ChangedStatsTracker<StatType> _changedStats;
        readonly User _user;

        public UserStats(User user, StatCollectionType statCollectionType) : base(statCollectionType)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _user = user;
            _changedStats = new ChangedStatsTracker<StatType>(this);
        }

        public User User
        {
            get { return _user; }
        }

        /// <summary>
        /// Synchronizes the User's stat values to the client.
        /// </summary>
        public void UpdateClient()
        {
            var statsToUpdate = _changedStats.GetChangedStats();
            if (statsToUpdate.IsEmpty())
                return;

            using (PacketWriter pw = ServerPacket.GetWriter())
            {
                foreach (var stat in statsToUpdate)
                {
                    pw.Reset();
                    ServerPacket.UpdateStat(pw, stat, StatCollectionType);
                    User.Send(pw);
                }
            }
        }
    }
}