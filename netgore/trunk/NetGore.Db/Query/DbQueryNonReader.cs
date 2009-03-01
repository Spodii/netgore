using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public abstract class DbQueryNonReader<T> : DbQueryBase, IDbQueryNonReader<T>
    {
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText, IEnumerable<DbParameter> parameters) : base(connectionPool, commandText, parameters)
        {
        }

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        public int Execute(T item)
        {
            int returnValue;

            // Get the connection to use
            using (var pooledConn = GetPoolableConnection())
            {
                var conn = pooledConn.Connection;

                // Get and set up the command
                var cmd = GetCommand(conn);
                SetParameters(cmd.Parameters, item);
                
                // Execute the command
                returnValue = cmd.ExecuteNonQuery();

                // Release the command so it can be used again later
                ReleaseCommand(cmd);
            }

            // Return the value from ExecuteNonQuery
            return returnValue;
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="parameters">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected abstract void SetParameters(DbParameterCollection parameters, T item);
    }
}
