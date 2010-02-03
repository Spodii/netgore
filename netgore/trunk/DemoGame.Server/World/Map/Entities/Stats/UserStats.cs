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
            // Check if any stat values have changed
            var statsToUpdate = _changedStats.GetChangedStats();
            if (statsToUpdate.IsEmpty())
                return;

            // Build a packet containing all the new stat values and send it to the user
            using (PacketWriter pw = ServerPacket.GetWriter())
            {
                foreach (var stat in statsToUpdate)
                {
                    ServerPacket.UpdateStat(pw, stat, StatCollectionType);
                }

                User.Send(pw);
            }
        }
    }
}