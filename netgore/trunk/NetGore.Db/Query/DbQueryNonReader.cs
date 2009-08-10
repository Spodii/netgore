using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    public abstract class DbQueryNonReader : DbQueryBase, IDbQueryNonReader
    {
        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        #region IDbQueryNonReader Members

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>Number of rows affected by the query.</returns>
        public int Execute()
        {
            int returnValue;

            // Get the connection to use
            using (IPoolableDbConnection pooledConn = GetPoolableConnection())
            {
                DbConnection conn = pooledConn.Connection;

                // Get and set up the command
                DbCommand cmd = GetCommand(conn);

                // Execute the command
                returnValue = cmd.ExecuteNonQuery();

                // Release the command so it can be used again later
                ReleaseCommand(cmd);
            }

            // Return the value from ExecuteNonQuery
            return returnValue;
        }

        #endregion
    }

    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public abstract class DbQueryNonReader<T> : DbQueryBase, IDbQueryNonReader<T>
    {
        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <typeparam name="T">Type of the object containing the values to set.</typeparam>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected abstract void SetParameters(DbParameterValues p, T item);

        #region IDbQueryNonReader<T> Members

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        public int Execute(T item)
        {
            int returnValue;

            // Get the connection to use
            using (IPoolableDbConnection pooledConn = GetPoolableConnection())
            {
                DbConnection conn = pooledConn.Connection;

                // Get and set up the command
                DbCommand cmd = GetCommand(conn);
                if (HasParameters)
                    SetParameters(new DbParameterValues(cmd.Parameters), item);

                // Execute the command
                returnValue = cmd.ExecuteNonQuery();

                // Release the command so it can be used again later
                ReleaseCommand(cmd);
            }

            // Return the value from ExecuteNonQuery
            return returnValue;
        }

        #endregion
    }
}