using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    public struct SelectAllianceAttackableQueryValues
    {
        public readonly byte AllianceID;
        public readonly IEnumerable<byte> AttackableIDs;

        public SelectAllianceAttackableQueryValues(byte allianceID, IEnumerable<byte> attackableIDs)
        {
            AllianceID = allianceID;
            AttackableIDs = attackableIDs;
        }
    }
}
