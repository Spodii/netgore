using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Base class for a collection of stats for Characters.
    /// </summary>
    public abstract class CharacterStatsBase : StatCollectionBase
    {
        protected CharacterStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
            foreach (var statType in StatFactory.AllStats)
            {
                Add(StatFactory.CreateStat(statType, statCollectionType));
            }
        }
    }
}