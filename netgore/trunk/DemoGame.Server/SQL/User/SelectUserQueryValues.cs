using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    public class SelectUserQueryValues
    {
        public readonly int BodyIndex;
        public readonly ushort Guid;
        public readonly MapIndex MapIndex;
        public readonly Vector2 Position;
        public readonly CharacterStatsBase Stats;

        public SelectUserQueryValues(ushort guid, MapIndex mapIndex, Vector2 position, int bodyIndex, CharacterStatsBase stats)
        {
            Guid = guid;
            MapIndex = mapIndex;
            Position = position;
            BodyIndex = bodyIndex;
            Stats = stats;
        }
    }
}