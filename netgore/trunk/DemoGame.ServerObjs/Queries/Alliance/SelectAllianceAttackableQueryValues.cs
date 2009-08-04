using System.Collections.Generic;

namespace DemoGame.Server.Queries
{
    public struct SelectAllianceAttackableQueryValues
    {
        public readonly AllianceID AllianceID;
        public readonly IEnumerable<AllianceID> AttackableIDs;

        public SelectAllianceAttackableQueryValues(AllianceID allianceID, IEnumerable<AllianceID> attackableIDs)
        {
            AllianceID = allianceID;
            AttackableIDs = attackableIDs;
        }
    }
}