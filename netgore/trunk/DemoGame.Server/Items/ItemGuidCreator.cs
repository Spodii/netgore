using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// A thread-safe collection of available guids for items.
    /// </summary>
    public class ItemGuidCreator : GuidCreatorBase
    {
        /// <summary>
        /// ItemGuidCreator constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use to communicate with the database.</param>
        public ItemGuidCreator(DbConnectionPool connectionPool) : base(connectionPool, "items", "guid", 2048, 128)
        {
        }
    }
}