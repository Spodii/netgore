using System.Collections.Generic;

namespace DemoGame
{
    public interface IStatCollection : IEnumerable<IStat>
    {
        int this[StatType statType] { get; set; }

        bool Contains(StatType statType);

        IStat GetStat(StatType statType);

        bool TryGetStat(StatType statType, out IStat stat);

        bool TryGetStatValue(StatType statType, out int value);
    }
}