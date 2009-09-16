using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for items.
    /// </summary>
    [DbControllerQuery]
    public class ItemIDCreator : IDCreatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ItemIDCreator(DbConnectionPool connectionPool) : base(connectionPool, ItemTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }
    }
}