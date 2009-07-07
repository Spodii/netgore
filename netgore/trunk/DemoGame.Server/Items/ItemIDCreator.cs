using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    /// <summary>
    /// A thread-safe collection of available IDs for items.
    /// </summary>
    public class ItemIDCreator : IDCreatorBase
    {
        /// <summary>
        /// ItemIDCreator constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use to communicate with the database.</param>
        public ItemIDCreator(DbConnectionPool connectionPool) : base(connectionPool, "item", "id", 2048, 128)
        {
        }
    }
}