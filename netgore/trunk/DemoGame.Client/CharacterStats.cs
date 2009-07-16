using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame.Client
{
    public class CharacterStats : CharacterStatsBase
    {
        public CharacterStats(StatCollectionType statCollectionType)
            : base(statCollectionType)
        {
        }
    }
}