using System.Linq;
using NetGore;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Server
{
    /// <summary>
    /// A collection of stats for a <see cref="User"/>.
    /// </summary>
    public class UserStats : StatCollection<StatType>
    {
        readonly ChangedStatsTracker<StatType> _changedStats;

        /// <summary>
        /// Is set to true whenever a stat changes. If false, we don't even need to try looking for changed stats
        /// in <see cref="UserStats.UpdateClient"/>.
        /// </summary>
        bool _anyStatsChanged = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStats"/> class.
        /// </summary>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        public UserStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
            _changedStats = new ChangedStatsTracker<StatType>(this);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when a stat's value has changed.
        /// </summary>
        /// <param name="statType">The type of the stat that changed.</param>
        /// <param name="oldValue">The old value of the stat.</param>
        /// <param name="newValue">The new value of the stat.</param>
        protected override void OnStatChanged(StatType statType, StatValueType oldValue, StatValueType newValue)
        {
            base.OnStatChanged(statType, oldValue, newValue);

            _anyStatsChanged = true;
        }

        /// <summary>
        /// Synchronizes the User's stat values to the client.
        /// </summary>
        public void UpdateClient(INetworkSender sendTo)
        {
            if (!_anyStatsChanged)
                return;

            // Check if any stat values have changed
            var statsToUpdate = _changedStats.GetChangedStats();
            if (statsToUpdate.IsEmpty())
                return;

            // Build a packet containing all the new stat values and send it to the user
            using (var pw = ServerPacket.GetWriter())
            {
                foreach (var stat in statsToUpdate)
                {
                    ServerPacket.UpdateStat(pw, stat, StatCollectionType);
                }

                sendTo.Send(pw, ServerMessageType.GUIUserStats);
            }

            _anyStatsChanged = false;
        }
    }
}