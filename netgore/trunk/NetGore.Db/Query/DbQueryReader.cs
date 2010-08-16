using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all data queries that execute queries and require reading the results.
    /// </summary>
    public abstract class DbQueryReader : DbQueryBase, IDbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>IDataReader used to read the results of the query.</returns>
        protected IDataReader ExecuteReader()
        {
            // Little hack that allows us to have IDbQueryReader<T>.ExecuteReader exposed protected instead of public 
            return ((IDbQueryReader)this).ExecuteReader();
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

        #region IDbQueryReader Members

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>IDataReader used to read the results of the query.</returns>
        IDataReader IDbQueryReader.ExecuteReader()
        {
            // Get the connection to use
            var pooledConn = GetPoolableConnection();
            var conn = pooledConn.Connection;

            // Get and set up the command
            var cmd = GetCommand(conn);

            // Execute the query
            var retReader = cmd.ExecuteReader();

            // Return the DbDataReader wrapped in a custom container that will allow us to
            // properly free the command and close the connection when the DbDataReader is disposed
            return new DbQueryReaderDataReaderContainer(this, pooledConn, cmd, retReader);
        }

        #endregion
    }

    /// <summary>
    /// Base class for all data queries that execute queries and require reading the results.
    /// </summary>
    /// <typeparam name="T">Type of the object or struct used to hold the query's arguments.</typeparam>
    public abstract class DbQueryReader<T> : DbQueryBase, IDbQueryReader<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryReader{T}"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        protected IDataReader ExecuteReader(T item)
        {
            // Little hack that allows us to have IDbQueryReader<T>.ExecuteReader exposed protected instead of public 
            return ((IDbQueryReader<T>)this).ExecuteReader(item);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected abstract void SetParameters(DbParameterValues p, T item);

        #region IDbQueryReader<T> Members

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        IDataReader IDbQueryReader<T>.ExecuteReader(T item)
        {
            // Get the connection to use
            var pooledConn = GetPoolableConnection();
            var conn = pooledConn.Connection;

            // Get and set up the command
            var cmd = GetCommand(conn);
            if (HasParameters)
            {
                using (var p = DbParameterValues.Create(cmd.Parameters))
                {
                    SetParameters(p, item);
                }
            }

            // Execute the query
            var retReader = cmd.ExecuteReader();

            // Return the DbDataReader wrapped in a custom container that will allow us to
            // properly free the command and close the connection when the DbDataReader is disposed
            return new DbQueryReaderDataReaderContainer(this, pooledConn, cmd, retReader);
        }

        #endregion
    }
}