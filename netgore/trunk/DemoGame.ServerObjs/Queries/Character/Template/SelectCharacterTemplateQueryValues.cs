using System.Collections.Generic;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterTemplateQueryValues
    {
        public readonly string AIName;
        public readonly AllianceID AllianceID;
        public readonly BodyIndex BodyIndex;
        public readonly uint Exp;
        public readonly ushort GiveCash;
        public readonly ushort GiveExp;
        public readonly CharacterTemplateID ID;
        public readonly byte Level;
        public readonly string Name;
        public readonly ushort Respawn;
        public readonly uint StatPoints;
        public readonly IEnumerable<StatTypeValue> StatValues;

        public SelectCharacterTemplateQueryValues(CharacterTemplateID id, string name, BodyIndex bodyIndex, string aiName,
                                                  AllianceID allianceID, ushort respawn, ushort giveExp, ushort giveCash, uint exp,
                                                  uint statpoints, byte level, IEnumerable<StatTypeValue> statValues)
        {
            ID = id;
            Name = name;
            BodyIndex = bodyIndex;
            AIName = aiName;
            AllianceID = allianceID;
            Respawn = respawn;
            GiveExp = giveExp;
            GiveCash = giveCash;
            StatValues = statValues;
            Exp = exp;
            StatPoints = statpoints;
            Level = level;
        }
    }
}