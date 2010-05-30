using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for accounts.
    /// </summary>
    [DbControllerQuery]
    public class GuildIDCreator : IDCreatorBase<GuildID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuildIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public GuildIDCreator(DbConnectionPool connectionPool) : base(connectionPool, GuildTable.TableName, "id", 2048, 128)
        {
            QueryAsserts.ArePrimaryKeys(GuildTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="GuildID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="GuildID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="GuildID"/>.</returns>
        protected override GuildID FromInt(int value)
        {
            return new GuildID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="GuildID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(GuildID value)
        {
            return (int)value;
        }
    }
}