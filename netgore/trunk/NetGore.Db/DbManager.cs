using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// The primary manager of database connections and commands.
    /// </summary>
    public class DbManager
    {
        readonly DbConnectionPool _connectionPool;

        readonly IList<KeyValuePair<IDbConnection, IPoolableDbConnection>> _connToPoolableConn =
            new List<KeyValuePair<IDbConnection, IPoolableDbConnection>>();

        /// <summary>
        /// Gets the DbConnectionPool used by this DbManager.
        /// </summary>
        public DbConnectionPool ConnectionPool
        {
            get { return _connectionPool; }
        }

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
        /// Executes a NonQuery command.
        /// </summary>
        /// <param name="commandText">CommandText to execute.</param>
        /// <returns>Number of rows affected.</returns>
        public int ExecuteNonQuery(string commandText)
        {
            int ret;
            using (IDbCommand cmd = GetCommand(commandText))
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
            IDbCommand cmd = GetCommand(commandText);
            IDataReader reader = cmd.ExecuteReader(behavior);
            return new DataReaderContainer(cmd, reader);
        }

        /// <summary>
        /// Executes a command that returns a reader.
        /// </summary>
        /// <param name="commandText">Command text to execute.</param>
        /// <returns>DataReader used to read the results of the query.</returns>
        public DataReaderContainer ExecuteReader(string commandText)
        {
            IDbCommand cmd = GetCommand(commandText);
            IDataReader reader = cmd.ExecuteReader();
            return new DataReaderContainer(cmd, reader);
        }

        /// <summary>
        /// Gets the next available command.
        /// </summary>
        /// <returns>Next available command.</returns>
        public IDbCommand GetCommand()
        {
            return GetCommand(string.Empty);
        }

        /// <summary>
        /// Gets the next available command.
        /// </summary>
        /// <param name="commandText">CommandText to set on the command.</param>
        /// <returns>Next available command.</returns>
        public IDbCommand GetCommand(string commandText)
        {
            // Get the connection to use
            IPoolableDbConnection poolConn = GetConnection();
            DbConnection conn = poolConn.Connection;

            // Create the command and add it to the internal list so we can find it later
            DbCommand cmd = conn.CreateCommand();
            PushPoolableConnection(poolConn);

            // Set up the command
            cmd.CommandText = commandText;
            cmd.Disposed += HandleIDbCommandDisposed;

            // FUTURE: GetCommand()'s performance can be improved by creating a custom class to wrap around DbCommand instead of using events and lookups

            return cmd;
        }

        /// <summary>
        /// Gets the next available connection.
        /// </summary>
        /// <returns>Next available connection.</returns>
        public IPoolableDbConnection GetConnection()
        {
            PooledDbConnection poolableConn = _connectionPool.Create();
            return poolableConn;
        }

        /// <summary>
        /// Handles when an IDbCommand is disposed, making sure the IPoolableDbConnection for it is sent
        /// back to the pool.
        /// </summary>
        /// <param name="sender">IDbCommand that was disposed.</param>
        /// <param name="e">Event args.</param>
        void HandleIDbCommandDisposed(object sender, EventArgs e)
        {
            // Get the connection for the IDbCommand
            IDbCommand cmd = (IDbCommand)sender;
            IDbConnection conn = cmd.Connection;

            // Get the IPoolableDbConnection for the connection
            IPoolableDbConnection poolableConn = PopPoolableConnection(conn);

            // Dispose of the IPoolableDbConnection, sending it back to the pool
            poolableConn.Dispose();
        }

        /// <summary>
        /// Finds a IPoolableDbConnection from an IDbConnection that was added earlier with PushPoolableConnection().
        /// </summary>
        /// <param name="conn">IDbConnection to find the IPoolableDbConnection for.</param>
        /// <returns>IPoolableDbConnection for the specified IDbConnection.</returns>
        /// <exception cref="ArgumentException">IDbConnection <paramref name="conn"/> does not exist in the internal
        /// collection.</exception>
        IPoolableDbConnection PopPoolableConnection(IDbConnection conn)
        {
            IPoolableDbConnection poolableConn = null;
            lock (_connToPoolableConn)
            {
                for (int i = _connToPoolableConn.Count - 1; i >= 0; i--)
                {
                    var item = _connToPoolableConn[i];
                    if (item.Key == conn)
                    {
                        poolableConn = item.Value;
                        _connToPoolableConn.RemoveAt(i);
                        break;
                    }
                }
            }

            if (poolableConn == null)
            {
                throw new ArgumentException(
                    "Failed to find the IPoolableDbConnection for the specified IDbConnection. " +
                    "Item was either already popped or never pushed in the first place.", "conn");
            }

            return poolableConn;
        }

        /// <summary>
        /// Allows an IPoolableDbConnection to be found from the IDbConnection associated with it later by using
        /// the method PopPoolableConnection().
        /// </summary>
        /// <param name="poolableConn"></param>
        void PushPoolableConnection(IPoolableDbConnection poolableConn)
        {
            lock (_connToPoolableConn)
            {
                var item = new KeyValuePair<IDbConnection, IPoolableDbConnection>(poolableConn.Connection, poolableConn);
                _connToPoolableConn.Add(item);
            }
        }
    }
}