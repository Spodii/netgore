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
        public readonly BodyIndex BodyIndex;
        public readonly CharacterID ID;
        public readonly MapIndex MapIndex;
        public readonly Vector2 Position;
        public readonly IEnumerable<StatTypeValue> Stats;
        public readonly CharacterTemplateID? TemplateID;
        public readonly byte Level;
        public readonly uint Exp;
        public readonly uint ExpSpent;
        public readonly uint Cash;

        public SelectCharacterQueryValues(CharacterID id, CharacterTemplateID? templateID, string name, MapIndex mapIndex, Vector2 position, BodyIndex bodyIndex, byte level, uint exp, uint expSpent, uint cash, IEnumerable<StatTypeValue> stats)
        {
            Name = name;
            ID = id;
            TemplateID = templateID;
            MapIndex = mapIndex;
            Position = position;
            BodyIndex = bodyIndex;
            Stats = stats;
            Level = level;
            Exp = exp;
            ExpSpent = expSpent;
            Cash = cash;
        }
    }
}