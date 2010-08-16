using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    public abstract class DbQueryNonReader : DbQueryBase, IDbQueryNonReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryNonReader"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        #region IDbQueryNonReader Members

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>Number of rows affected by the query.</returns>
        public virtual int Execute()
        {
            int returnValue;

            // Get the connection to use
            using (var pooledConn = GetPoolableConnection())
            {
                var conn = pooledConn.Connection;

                // Get and set up the command
                var cmd = GetCommand(conn);

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
        /// Initializes a new instance of the <see cref="DbQueryNonReader{T}"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected abstract void SetParameters(DbParameterValues p, T item);

        #region IDbQueryNonReader<T> Members

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">Trying to insert a value who's primary key already exists.</exception>
        public virtual int Execute(T item)
        {
            int returnValue;

            // Get the connection to use
            using (var pooledConn = GetPoolableConnection())
            {
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

                // Execute the command
                try
                {
                    returnValue = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    // Throw a custom exception for common errors
                    if (ex.Number == 1062)
                        throw new DuplicateKeyException(ex);

                    // Everything else, just throw the default exception
                    throw;
                }
                finally
                {
                    // Release the command so it can be used again later
                    ReleaseCommand(cmd);
                }
            }

            // Return the value from ExecuteNonQuery
            return returnValue;
        }

        #endregion
    }
}