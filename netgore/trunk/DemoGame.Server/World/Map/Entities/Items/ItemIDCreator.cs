using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for items.
    /// </summary>
    [DbControllerQuery]
    public class ItemIDCreator : IDCreatorBase<ItemID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public ItemIDCreator(DbConnectionPool connectionPool) : base(connectionPool, ItemTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="ItemID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="ItemID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="ItemID"/>.</returns>
        protected override ItemID FromInt(int value)
        {
            return new ItemID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="ItemID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(ItemID value)
        {
            return (int)value;
        }
    }
}