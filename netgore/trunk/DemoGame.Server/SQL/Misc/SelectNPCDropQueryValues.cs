using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Values returned by the SelectNPCDropQuery.
    /// </summary>
    public class SelectNPCDropQueryValues
    {
        public readonly ushort Chance;
        public readonly ushort Guid;
        public readonly int ItemGuid;
        public readonly byte Max;
        public readonly byte Min;

        public SelectNPCDropQueryValues(ushort guid, int itemGuid, byte min, byte max, ushort chance)
        {
            Guid = guid;
            ItemGuid = itemGuid;
            Min = min;
            Max = max;
            Chance = chance;
        }
    }
}