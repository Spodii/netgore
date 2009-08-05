using System.Collections.Generic;
using System.Linq;

namespace DemoGame.Server.Queries
{
    public struct SelectAllianceHostileQueryValues
    {
        public readonly AllianceID AllianceID;
        public readonly IEnumerable<AllianceID> HostileIDs;

        public SelectAllianceHostileQueryValues(AllianceID allianceID, IEnumerable<AllianceID> hostileIDs)
        {
            AllianceID = allianceID;
            HostileIDs = hostileIDs;
        }
    }
}