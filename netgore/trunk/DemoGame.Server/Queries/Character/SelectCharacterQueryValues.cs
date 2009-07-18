using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterQueryValues
    {
        public readonly BodyIndex BodyIndex;
        public readonly uint Cash;
        public readonly uint Exp;
        public readonly uint ExpSpent;
        public readonly CharacterID ID;
        public readonly byte Level;
        public readonly MapIndex MapIndex;
        public readonly string Name;
        public readonly Vector2 Position;
        public readonly IEnumerable<StatTypeValue> Stats;
        public readonly CharacterTemplateID? TemplateID;
        public readonly SPValueType HP;
        public readonly SPValueType MP;

        public SelectCharacterQueryValues(CharacterID id, CharacterTemplateID? templateID, string name, MapIndex mapIndex,
                                          Vector2 position, BodyIndex bodyIndex, byte level, uint exp, uint expSpent, uint cash,
                                          SPValueType hp, SPValueType mp, IEnumerable<StatTypeValue> stats)
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
            HP = hp;
            MP = mp;
        }
    }
}