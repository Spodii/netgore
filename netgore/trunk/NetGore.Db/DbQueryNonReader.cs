using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    public abstract class DbQueryNonReader<T> : IDbQueryNonReader<T>
    {
        const int _initialStackSize = 2;

        readonly DbConnectionPool _connectionPool;
        readonly IEnumerable<DbParameter> _parameters;
        readonly Stack<IDbCommand> _commands = new Stack<IDbCommand>(_initialStackSize);
        readonly object _commandsLock = new object();
        readonly string _commandText;

        public DbConnectionPool ConnectionPool { get { return _connectionPool; } }

        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText, IEnumerable<DbParameter> parameters)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException("commandText");

            _connectionPool = connectionPool;
            _parameters = parameters;
            _commandText = commandText;
        }

        void ReleaseCommand(IDbCommand cmd)
        {
            lock (_commandsLock)
                _commands.Push(cmd);
        }

        IDbCommand GetCommand(IDbConnection conn)
        {
            IDbCommand cmd = null;

            lock (_commandsLock)
            {
                // Pop the next available command, if exists
                if (_commands.Count > 0)
                    cmd = _commands.Pop();
            }

            // Create a new command if we couldn't pop one from the stack
            if (cmd == null)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = CommandText;

                foreach (var parameter in _parameters)
                    cmd.Parameters.Add(parameter);
            }

            // Set up the command
            cmd.Connection = conn;

            return cmd;
        }

        public int Execute(T item)
        {
            int returnValue;

            // Get the connection to use
            using (var pooledConn = _connectionPool.Create())
            {
                var conn = pooledConn.Connection;

                // Get and set up the command
                var cmd = GetCommand(conn);
                SetParameters((DbParameterCollection)cmd.Parameters, item);
                
                // Execute the command
                returnValue = cmd.ExecuteNonQuery();

                // Release the command so it can be used again later
                ReleaseCommand(cmd);
            }

            // Return the value from ExecuteNonQuery
            return returnValue;
        }

        protected abstract void SetParameters(DbParameterCollection parameters, T item);

        public void Dispose()
        {
            // TODO: Dispose
        }

        public string CommandText
        {
            get { return _commandText; }
        }
    }
}
