using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// The primary manager of database connections and commands.
    /// </summary>
    public class DbManager
    {
        readonly IList<KeyValuePair<IDbConnection, IPoolableDbConnection>> _connToPoolableConn =
            new List<KeyValuePair<IDbConnection, IPoolableDbConnection>>();

        readonly DbConnectionPool _connectionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbManager"/> class.
        /// </summary>
        /// <param name="connectionPool"><see cref="DbConnectionPool"/> to use to create the connections managed by
        /// this <see cref="DbManager"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool" /> is <c>null</c>.</exception>
        public DbManager(DbConnectionPool connectionPool)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");

            _connectionPool = connectionPool;
        }

        /// <summary>
        /// Gets the <see cref="DbConnectionPool"/> used by this <see cref="DbManager"/>.
        /// </summary>
        public DbConnectionPool ConnectionPool
        {
            get { return _connectionPool; }
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
        /// <returns><see cref="IDataReader"/> used to read the results of the query.</returns>
        public IDataReader ExecuteReader(string commandText, CommandBehavior behavior)
        {
            var cmd = GetCommand(commandText);
            var reader = cmd.ExecuteReader(behavior);
            return reader;
        }

        /// <summary>
        /// Executes a command that returns a reader.
        /// </summary>
        /// <param name="commandText">Command text to execute.</param>
        /// <returns><see cref="IDataReader"/> used to read the results of the query.</returns>
        public IDataReader ExecuteReader(string commandText)
        {
            var cmd = GetCommand(commandText);
            var reader = cmd.ExecuteReader();
            return reader;
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
            var poolConn = GetConnection();
            var conn = poolConn.Connection;

            // Create the command and add it to the internal list so we can find it later
            var cmd = conn.CreateCommand();
            PushPoolableConnection(poolConn);

            // Set up the command
            cmd.CommandText = commandText;
            cmd.Disposed -= HandleIDbCommandDisposed;
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
            var poolableConn = _connectionPool.Acquire();
            return poolableConn;
        }

        /// <summary>
        /// Handles when an <see cref="IDbCommand"/> is disposed, making sure the <see cref="IPoolableDbConnection"/> for it is sent
        /// back to the pool.
        /// </summary>
        /// <param name="sender"><see cref="IDbCommand"/> that was disposed.</param>
        /// <param name="e">Event args.</param>
        void HandleIDbCommandDisposed(object sender, EventArgs e)
        {
            // Get the connection for the IDbCommand
            var cmd = (IDbCommand)sender;
            var conn = cmd.Connection;

            // Get the IPoolableDbConnection for the connection
            var poolableConn = PopPoolableConnection(conn);

            // Dispose of the IPoolableDbConnection, sending it back to the pool
            poolableConn.Dispose();
        }

        /// <summary>
        /// Finds a <see cref="IPoolableDbConnection"/> from an <see cref="IDbConnection"/> that was added earlier with
        /// <see cref="PushPoolableConnection"/>.
        /// </summary>
        /// <param name="conn"><see cref="IDbConnection"/> to find the <see cref="IPoolableDbConnection"/> for.</param>
        /// <returns><see cref="IPoolableDbConnection"/> for the specified <see cref="IDbConnection"/>.</returns>
        /// <exception cref="ArgumentException"><see cref="IDbConnection"/> <paramref name="conn"/> does not exist in the internal
        /// collection.</exception>
        IPoolableDbConnection PopPoolableConnection(IDbConnection conn)
        {
            IPoolableDbConnection poolableConn = null;
            lock (_connToPoolableConn)
            {
                for (var i = _connToPoolableConn.Count - 1; i >= 0; i--)
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
        /// Allows an <see cref="IPoolableDbConnection"/> to be found from the <see cref="IDbConnection"/> associated with it later by using
        /// the method <see cref="PopPoolableConnection"/>.
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