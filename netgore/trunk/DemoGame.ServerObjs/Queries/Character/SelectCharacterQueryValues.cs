using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

// TODO: !! Make obsolete

namespace DemoGame.Server.Queries
{
    public class SelectCharacterQueryValues
    {
        public readonly BodyIndex BodyIndex;
        public readonly uint Cash;
        public readonly uint Exp;
        public readonly SPValueType HP;
        public readonly CharacterID ID;
        public readonly byte Level;
        public readonly MapIndex MapIndex;
        public readonly SPValueType MP;
        public readonly string Name;
        public readonly Vector2 Position;
        public readonly MapIndex? RespawnMapIndex;
        public readonly Vector2 RespawnPosition;
        public readonly uint StatPoints;
        public readonly IEnumerable<StatTypeValue> Stats;
        public readonly CharacterTemplateID? TemplateID;

        public SelectCharacterQueryValues(CharacterID id, CharacterTemplateID? templateID, string name, MapIndex mapIndex,
                                          Vector2 position, BodyIndex bodyIndex, byte level, uint exp, uint statpoints, uint cash,
                                          SPValueType hp, SPValueType mp, MapIndex? respawnMapIndex, Vector2 respawnPosition,
                                          IEnumerable<StatTypeValue> stats)
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
            StatPoints = statpoints;
            Cash = cash;
            HP = hp;
            MP = mp;
            RespawnMapIndex = respawnMapIndex;
            RespawnPosition = respawnPosition;
        }
    }
}