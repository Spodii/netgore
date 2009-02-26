using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// The primary manager of database connections and commands.
    /// </summary>
    public class DbManager
    {
        readonly DbConnectionPool _connectionPool;

        /// <summary>
        /// DbManager constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use to create the connections managed by this DbManager.</param>
        public DbManager(DbConnectionPool connectionPool)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");

            _connectionPool = connectionPool;
        }

        /// <summary>
        /// Gets the next available connection.
        /// </summary>
        /// <returns>Next available connection.</returns>
        public IPoolableDbConnection GetConnection()
        {
            return _connectionPool.Create();
        }

        /// <summary>
        /// Gets the next available command.
        /// </summary>
        /// <returns>Next available command.</returns>
        public IDbCommand GetCommand()
        {
            IPoolableDbConnection poolConn = _connectionPool.Create();
            return poolConn.Command;
        }

        /// <summary>
        /// Gets the next available command.
        /// </summary>
        /// <param name="commandText">CommandText to set on the command.</param>
        /// <returns>Next available command.</returns>
        public IDbCommand GetCommand(string commandText)
        {
            IPoolableDbConnection poolConn = _connectionPool.Create();
            var cmd = poolConn.Command;
            cmd.CommandText = commandText;
            
            return cmd;
        }

        /// <summary>
        /// Executes a NonQuery command.
        /// </summary>
        /// <param name="commandText">CommandText to execute.</param>
        /// <returns>Number of rows affected.</returns>
        public int ExecuteNonQuery(string commandText)
        {
            int ret;
            using (var cmd = GetCommand(commandText))
            {
                ret = cmd.ExecuteNonQuery();
            }
            return ret;
        }

        /// <summary>
        /// Executes a command that returns a reader.
        /// </summary>
        /// <param name="commandText">Command text to execute.</param>
        /// <param name="behavior">Command behavior to use when executing the command.</param>
        /// <returns>DataReader used to read the results of the query.</returns>
        public DataReaderContainer ExecuteReader(string commandText, CommandBehavior behavior)
        {
            var cmd = GetCommand(commandText);
            var reader = cmd.ExecuteReader(behavior);
            return new DataReaderContainer(cmd, reader);
        }

        /// <summary>
        /// Executes a command that returns a reader.
        /// </summary>
        /// <param name="commandText">Command text to execute.</param>
        /// <returns>DataReader used to read the results of the query.</returns>
        public DataReaderContainer ExecuteReader(string commandText)
        {
            var cmd = GetCommand(commandText);
            var reader = cmd.ExecuteReader();
            return new DataReaderContainer(cmd, reader);
        }
    }
}
