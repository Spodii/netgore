using System;
using NetGore;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// A collection of stats for a <see cref="User"/>.
    /// </summary>
    public class UserStats : CharacterStats
    {
        readonly ChangedStatsTracker<StatType> _changedStats;
        readonly User _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStats"/> class.
        /// </summary>
        /// <param name="user">The <see cref="User"/> these stats belong to.</param>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is null.</exception>
        public UserStats(User user, StatCollectionType statCollectionType) : base(statCollectionType)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _user = user;
            _changedStats = new ChangedStatsTracker<StatType>(this);
        }

        /// <summary>
        /// Gets the <see cref="User"/> these stats belong to.
        /// </summary>
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