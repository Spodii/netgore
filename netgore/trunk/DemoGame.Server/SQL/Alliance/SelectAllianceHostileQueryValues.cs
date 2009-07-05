using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public struct SelectAllianceHostileQueryValues
    {
        public readonly byte AllianceID;
        public readonly IEnumerable<byte> HostileIDs;

        public SelectAllianceHostileQueryValues(byte allianceID, IEnumerable<byte> hostileIDs)
        {
            AllianceID = allianceID;
            HostileIDs = hostileIDs;
        }
    }
}
