using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public class ItemGuidCreator : GuidCreatorBase
    {
        public ItemGuidCreator(DbConnection conn) : base(conn, "items", "guid", 2048, 128)
        {
        }
    }
}