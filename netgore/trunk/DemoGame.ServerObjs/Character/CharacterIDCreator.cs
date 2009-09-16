using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for Characters.
    /// </summary>
    [DbControllerQuery]
    public class CharacterIDCreator : IDCreatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public CharacterIDCreator(DbConnectionPool connectionPool)
            : base(connectionPool, CharacterTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }
    }
}
