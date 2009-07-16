using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    class ChangedStatsTracker
    {
        // TODO: [STATS] This totally sucks, fix it up

        readonly int[] _lastValues;
        readonly IStatCollection _statCollection;

        public ChangedStatsTracker(IStatCollection statCollection)
        {
            _statCollection = statCollection;
            int size = _statCollection.Select(x => x.StatType.GetValue()).Max() + 1;
            _lastValues = new int[size];

            foreach (IStat istat in statCollection)
            {
                _lastValues[istat.StatType.GetValue()] = istat.Value;
            }
        }

        public IEnumerable<IStat> GetChangedStats()
        {
            var changed = new List<IStat>();

            foreach (IStat istat in _statCollection)
            {
                byte i = istat.StatType.GetValue();
                int v = istat.Value;
                if (_lastValues[i] != v)
                {
                    _lastValues[i] = v;
                    changed.Add(istat);
                }
            }

            return changed;
        }
    }

    public class UserStats : CharacterStats
    {
        readonly ChangedStatsTracker _changedStats;
        readonly User _user;

        public User User
        {
            get { return _user; }
        }

        public UserStats(User user, StatCollectionType statCollectionType) : base(user, statCollectionType)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _user = user;
            _changedStats = new ChangedStatsTracker(this);
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
                foreach (IStat stat in statsToUpdate)
                {
                    pw.Reset();
                    ServerPacket.UpdateStat(pw, stat, StatCollectionType);
                    User.Send(pw);
                }
            }
        }
    }
}