using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class SelectNPCDropValues
    {
        public readonly ushort Guid;
        public readonly int ItemGuid;
        public readonly byte Min;
        public readonly byte Max;
        public readonly ushort Chance;

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
