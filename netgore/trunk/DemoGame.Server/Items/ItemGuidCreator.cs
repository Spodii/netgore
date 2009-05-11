using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class ItemGuidCreator : GuidCreatorBase
    {
        public ItemGuidCreator(DbConnectionPool connectionPool) : base(connectionPool, "items", "guid", 2048, 128)
        {
        }
    }
}