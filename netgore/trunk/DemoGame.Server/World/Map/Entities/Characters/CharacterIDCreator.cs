using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for Characters.
    /// </summary>
    [DbControllerQuery]
    public class CharacterIDCreator : IDCreatorBase<CharacterID>
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

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="CharacterID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="CharacterID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="CharacterID"/>.</returns>
        protected override CharacterID FromInt(int value)
        {
            return new CharacterID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="CharacterID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(CharacterID value)
        {
            return (int)value;
        }
    }
}