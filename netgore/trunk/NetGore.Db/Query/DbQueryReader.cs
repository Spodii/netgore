using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all data queries that execute queries and require reading the results.
    /// </summary>
    public abstract class DbQueryReader : DbQueryBase, IDbQueryReader
    {
        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        protected DbQueryReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        #region IDbQueryReader Members

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>IDataReader used to read the results of the query.</returns>
        public IDataReader ExecuteReader()
        {
            // Get the connection to use
            IPoolableDbConnection pooledConn = GetPoolableConnection();
            DbConnection conn = pooledConn.Connection;

            // Get and set up the command
            DbCommand cmd = GetCommand(conn);

            // Execute the query
            DbDataReader retReader = cmd.ExecuteReader();

            // Return the DbDataReader wrapped in a custom container that will allow us to
            // properly free the command and close the connection when the DbDataReader is disposed
            return new DbQueryReaderDataReaderContainer(this, pooledConn, cmd, retReader);
        }

        #endregion
    }

    /// <summary>
    /// Base class for all data queries that execute queries and require reading the results.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public abstract class DbQueryReader<T> : DbQueryBase, IDbQueryReader<T>
    {
        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        protected DbQueryReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <typeparam name="T">Type of the object containing the values to set.</typeparam>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected abstract void SetParameters(DbParameterValues p, T item);

        #region IDbQueryReader<T> Members

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        public IDataReader ExecuteReader(T item)
        {
            // Get the connection to use
            IPoolableDbConnection pooledConn = GetPoolableConnection();
            DbConnection conn = pooledConn.Connection;

            // Get and set up the command
            DbCommand cmd = GetCommand(conn);
            if (HasParameters)
                SetParameters(new DbParameterValues(cmd.Parameters), item);

            // Execute the query
            DbDataReader retReader = cmd.ExecuteReader();

            // Return the DbDataReader wrapped in a custom container that will allow us to
            // properly free the command and close the connection when the DbDataReader is disposed
            return new DbQueryReaderDataReaderContainer(this, pooledConn, cmd, retReader);
        }

        #endregion
    }
}