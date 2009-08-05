using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for items.
    /// </summary>
    [DBControllerQuery]
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