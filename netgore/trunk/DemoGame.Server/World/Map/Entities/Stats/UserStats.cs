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
        /// Is set to true whenever a stat changes. If false, we don't even need to try looking for changed stats
        /// in <see cref="UserStats.UpdateClient"/>.
        /// </summary>
        bool _anyStatsChanged = false;

        /// <summary>
        /// When overridden in the derived class, handles when an <see cref="IStat{StatType}"/> in this
        /// <see cref="DynamicStatCollection{StatType}"/> has changed their value.
        /// </summary>
        /// <param name="stat">The <see cref="IStat{StatType}"/> whos value has changed.</param>
        protected override void OnStatChanged(IStat<StatType> stat)
        {
            base.OnStatChanged(stat);

            _anyStatsChanged = true;
        }

        /// <summary>
        /// Synchronizes the User's stat values to the client.
        /// </summary>
        public void UpdateClient()
        {
            if (!_anyStatsChanged)
                return;

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

            _anyStatsChanged = false;
        }
    }
}