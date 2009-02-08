using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace DemoGame
{
    public interface IStatCollection : IEnumerable<IStat>
    {
        int this[StatType statType] { get; set; }

        bool Contains(StatType statType);

        IStat GetStat(StatType statType);

        bool TryGetStat(StatType statType, out IStat stat);
    }
}