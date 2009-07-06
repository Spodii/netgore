using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame.Server.Queries
{
    public class SelectNPCTemplateQueryValues
    {
        public readonly string AIName;
        public readonly AllianceID AllianceID;
        public readonly ushort BodyIndex;
        public readonly ushort GiveCash;
        public readonly ushort GiveExp;
        public readonly CharacterTemplateID ID;
        public readonly string Name;
        public readonly ushort Respawn;
        public readonly IEnumerable<IStat> Stats;

        public SelectNPCTemplateQueryValues(CharacterTemplateID id, string name, ushort bodyIndex, string aiName, AllianceID allianceID,
                                            ushort respawn, ushort giveExp, ushort giveCash,
                                            IEnumerable<IStat> stats)
        {
            ID = id;
            Name = name;
            BodyIndex = bodyIndex;
            AIName = aiName;
            AllianceID = allianceID;
            Respawn = respawn;
            GiveExp = giveExp;
            GiveCash = giveCash;
            Stats = stats;
        }
    }
}