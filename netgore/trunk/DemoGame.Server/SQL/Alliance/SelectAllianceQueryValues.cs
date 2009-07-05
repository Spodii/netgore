using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public struct SelectAllianceQueryValues
    {
        public readonly byte ID;
        public readonly string Name;

        public SelectAllianceQueryValues(byte id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
