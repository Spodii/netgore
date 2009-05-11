using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public class SelectNPCDropValues
    {
        public readonly ushort Chance;
        public readonly ushort Guid;
        public readonly int ItemGuid;
        public readonly byte Max;
        public readonly byte Min;

        public SelectNPCDropValues(ushort guid, int itemGuid, byte min, byte max, ushort chance)
        {
            Guid = guid;
            ItemGuid = itemGuid;
            Min = min;
            Max = max;
            Chance = chance;
        }
    }
}