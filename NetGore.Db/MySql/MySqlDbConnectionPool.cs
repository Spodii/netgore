using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;
using NetGore.Db.MySql.QueryBuilder;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// Pool of MySql database connections.
    /// </summary>
    public class MySqlDbConnectionPool : DbConnectionPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbConnectionPool"/> class.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the MySqlConnections with.</param>
        public MySqlDbConnectionPool(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        public override IQueryBuilder QueryBuilder
        {
            get { return MySqlQueryBuilder.Instance; }
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected override DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Gets the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> that was executed and the last inserted id needs to be acquired for.</param>
        /// <returns>
        /// The last inserted id for the executed <paramref name="command"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        /// <exception cref="TypeException"><paramref name="command"/> is not of the excepted type.</exception>
        /// <exception cref="ArgumentException"><paramref name="command"/> is invalid in some other way.</exception>
        public override long GetLastInsertedId(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            var c = command as MySqlCommand;
            if (c == null)
            {
                const string errmsg = "Expected DbCommand `{0}` to be of type `{1}`, but was type `{2}`.";
                throw new TypeException(string.Format(errmsg, command, typeof(MySqlCommand), command.GetType()));
            }

            Debug.Assert(c.LastInsertedId != 0,
                "The LastInsertedId shouldn't ever be 0 for MySql since AutoIncrement starts at 1...");

            return c.LastInsertedId;
        }

        /// <summary>
        /// When overridden in the derived class, creates and returns a <see cref="DbParameter"/>
        /// that is compatible with the type of database used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this <see cref="IDbConnectionPool"/>.</returns>
        protected override DbParameter HandleCreateParameter(string parameterName)
        {
            return new MySqlParameter(parameterName, null);
        }
    }
}