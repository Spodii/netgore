using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// A thread-safe collection of available IDs for accounts.
    /// </summary>
    [DbControllerQuery]
    public class AccountIDCreator : IDCreatorBase<AccountID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountIDCreator"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public AccountIDCreator(DbConnectionPool connectionPool) : base(connectionPool, AccountTable.TableName, "id", 64)
        {
            QueryAsserts.ArePrimaryKeys(AccountTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// When overridden in the derived class, converts the given int to type <see cref="AccountID"/>.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="AccountID"/>.</param>
        /// <returns>The <paramref name="value"/> as type <see cref="AccountID"/>.</returns>
        protected override AccountID FromInt(int value)
        {
            return new AccountID(value);
        }

        /// <summary>
        /// When overridden in the derived class, converts the given value of type <see cref="AccountID"/> to
        /// an int.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The int value of the <paramref name="value"/>.</returns>
        protected override int ToInt(AccountID value)
        {
            return (int)value;
        }
    }
}