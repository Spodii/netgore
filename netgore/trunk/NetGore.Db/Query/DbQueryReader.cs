using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db.Query
{
    /// <summary>
    /// Base class for all database queries that execute queries that require reading the results of.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public abstract class DbQueryReader<T> : DbQueryBase, IDbQueryReader<T>
    {
        protected DbQueryReader(DbConnectionPool connectionPool, string commandText, IEnumerable<DbParameter> parameters) : base(connectionPool, commandText, parameters)
        {
        }

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        public IDataReader Execute(T item)
        {
            // Get the connection to use
            var pooledConn = GetPoolableConnection();
            var conn = pooledConn.Connection;

            // Get and set up the command
            var cmd = GetCommand(conn);
            SetParameters(cmd.Parameters, item);

            // Execute the query
            var retReader = cmd.ExecuteReader();

            // Return the DbDataReader wrapped in a custom container that will allow us to
            // properly free the command and close the connection when the DbDataReader is disposed
            return new SpecialDataReaderContainer(this, pooledConn, cmd, retReader);
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="parameters">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected abstract void SetParameters(DbParameterCollection parameters, T item);

        class SpecialDataReaderContainer : DataReaderContainer
        {
            readonly DbQueryReader<T> _dbQueryReader;
            readonly IPoolableDbConnection _poolableConn;

// ReSharper disable SuggestBaseTypeForParameter
            internal SpecialDataReaderContainer(DbQueryReader<T> dbQueryReader, IPoolableDbConnection poolableConn, DbCommand command, IDataReader dataReader)
// ReSharper restore SuggestBaseTypeForParameter
                : base(command, dataReader)
            {
                if (dbQueryReader == null)
                    throw new ArgumentNullException("dbQueryReader");
                if (poolableConn == null)
                    throw new ArgumentNullException("poolableConn");

                _dbQueryReader = dbQueryReader;
                _poolableConn = poolableConn;
            }

            public override void Dispose()
            {
                DataReader.Dispose();
                _dbQueryReader.ReleaseCommand((DbCommand)Command);
                _poolableConn.Dispose();
            }
        }
    }
}
