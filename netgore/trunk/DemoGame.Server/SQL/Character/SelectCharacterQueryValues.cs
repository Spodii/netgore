using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterQueryValues
    {
        public readonly string Name;
        public readonly int BodyIndex;
        public readonly CharacterID ID;
        public readonly MapIndex MapIndex;
        public readonly Vector2 Position;
        public readonly CharacterStatsBase Stats;

        public SelectCharacterQueryValues(CharacterID id, string name, MapIndex mapIndex, Vector2 position, int bodyIndex, CharacterStatsBase stats)
        {
            Name = name;
            ID = id;
            MapIndex = mapIndex;
            Position = position;
            BodyIndex = bodyIndex;
            Stats = stats;
        }
    }
}