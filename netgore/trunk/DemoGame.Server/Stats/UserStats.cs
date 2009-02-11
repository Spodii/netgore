using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class UserStats : CharacterStatsBase
    {
        static IEnumerable<StatType> _dbStats;
        readonly List<IUpdateableStat> _updateableStats = new List<IUpdateableStat>();
        readonly User _user;

        public static IEnumerable<StatType> DatabaseStats
        {
            get
            {
                if (_dbStats == null)
                    _dbStats = from stat in new UserStats() where stat.CanWrite select stat.StatType;

                return _dbStats;
            }
        }

        public IEnumerable<IUpdateableStat> UpdateableStats
        {
            get { return _updateableStats; }
        }

        public User User
        {
            get { return _user; }
        }

        public UserStats(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _user = user;
            CreateStats();
        }

        internal UserStats()
        {
            CreateStats();
        }

        protected override IStat CreateBaseStat<T>(StatType statType)
        {
            StatUpdateHandler updateHandler = StatUpdateHandlers.UpdateOwner;
            if (User == null)
                updateHandler = null;

            var stat = new UserStat<T>(User, updateHandler, statType);
            _updateableStats.Add(stat);
            return stat;
        }

        protected override IStat CreateModStat<T>(StatType statType)
        {
            if (User == null)
                return null;

            StatUpdateHandler updateHandler = StatUpdateHandlers.UpdateOwner;
            ModStatHandler modHandler = GetModStatHandler(statType);

            var stat = new ModUserStat<T>(User, updateHandler, modHandler, statType);
            _updateableStats.Add(stat);
            return stat;
        }

        protected override int GetEquipmentBonus(StatType statType)
        {
            return User.Equipped.GetStatBonus(statType);
        }
    }
}