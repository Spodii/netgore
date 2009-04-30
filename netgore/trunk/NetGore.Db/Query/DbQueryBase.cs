using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database query classes.
    /// </summary>
    public abstract class DbQueryBase : IDbQueryHandler
    {
        const string _disposedErrorMessage = "Can not access methods on a disposed object.";
        const int _initialStackSize = 2;

        readonly Stack<DbCommand> _commands = new Stack<DbCommand>(_initialStackSize);
        readonly object _commandsLock = new object();
        readonly string _commandText;
        readonly DbConnectionPool _connectionPool;
        readonly IEnumerable<DbParameter> _parameters;
        bool _disposed;

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries.</returns>
        protected abstract IEnumerable<DbParameter> InitializeParameters();

        /// <summary>
        /// Creates a DbParameter that can be used with this DbQueryBase.
        /// </summary>
        /// <param name="parameterName">Name of the parameter to create.</param>
        /// <returns>DbParameter that can be used with this DbQueryBase.</returns>
        protected DbParameter CreateParameter(string parameterName)
        {
            return ConnectionPool.CreateParameter(parameterName);
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="parameterNames">Array of ParameterNames to create.</param>
        /// <returns>IEnumerable of DbParameters, one for each element in the <paramref name="parameterNames"/> array.</returns>
        protected IEnumerable<DbParameter> CreateParameters(params string[] parameterNames)
        {
            if (parameterNames == null)
                throw new ArgumentNullException("parameterNames");

            foreach (string parameterName in parameterNames)
                yield return CreateParameter(parameterName);
        }

        /// <summary>
        /// Gets if this DbQueryBase has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        protected DbQueryBase(DbConnectionPool connectionPool, string commandText)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException("commandText");

            _connectionPool = connectionPool;
            _commandText = commandText;

// ReSharper disable DoNotCallOverridableMethodsInConstructor
            // NOTE: Is this going to be okay? Will the virtual member call in constructor cause problems with this design?
            _parameters = InitializeParameters();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Creates a new DbParameter object from the given source DbParameter.
        /// </summary>
        /// <param name="source">DbParameter to clone.</param>
        /// <returns>Clone of the <paramref name="source"/> DbParameter.</returns>
        static DbParameter CloneDbParameter(DbParameter source)
        {
            ICloneable cloneable = source as ICloneable;
            if (cloneable != null)
                return (DbParameter)cloneable.Clone();

            // FUTURE: Would be nice to have support of cloning ALL DbParameters...
            throw new NotImplementedException("Only DbParameters that implementICloneable are currently supported.");
        }

        /// <summary>
        /// Gets and sets up an available DbCommand.
        /// </summary>
        /// <param name="conn">DbConnection to assign the DbCommand to.</param>
        /// <returns>A DbCommand that that is set up for the DbConnection <paramref name="conn"/>, but has not
        /// had it's parameter values set.</returns>
        protected DbCommand GetCommand(DbConnection conn)
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            DbCommand cmd = null;

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

                foreach (DbParameter parameter in _parameters)
                {
                    DbParameter clone = CloneDbParameter(parameter);
                    cmd.Parameters.Add(clone);
                }
            }

            // Set up the command
            cmd.Connection = conn;

            return cmd;
        }

        /// <summary>
        /// Gets an available IPoolableDbConnection from the ConnectionPool.
        /// </summary>
        /// <returns>A free IPoolableDbConnection.</returns>
        protected IPoolableDbConnection GetPoolableConnection()
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            return ConnectionPool.Create();
        }

        /// <summary>
        /// Releases a DbCommand after it has been used. This must be called on every DbCommand once it is
        /// done being used to ensure it can be reused! Use this in place of disposing the DbCommand.
        /// </summary>
        /// <param name="cmd">DbCommand to release.</param>
        protected void ReleaseCommand(DbCommand cmd)
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            lock (_commandsLock)
                _commands.Push(cmd);
        }

        #region IDbQueryHandler Members

        /// <summary>
        /// Gets the DbConnectionPool used by this DbQueryBase.
        /// </summary>
        public DbConnectionPool ConnectionPool
        {
            get
            {
                if (_disposed)
                    throw new MethodAccessException(_disposedErrorMessage);

                return _connectionPool;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the DbParameters used in executing queries in this DbQueryBase.
        /// </summary>
        public IEnumerable<DbParameter> Parameters
        {
            get
            {
                if (_disposed)
                    throw new MethodAccessException(_disposedErrorMessage);

                return _parameters;
            }
        }

        /// <summary>
        /// Gets the CommandText used by this IDbQueryHandler. All commands executed by this IDbQueryHandler
        /// will use this CommandText.
        /// </summary>
        public string CommandText
        {
            get
            {
                if (_disposed)
                    throw new MethodAccessException(_disposedErrorMessage);

                return _commandText;
            }
        }

        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            lock (_commandsLock)
            {
                foreach (DbCommand cmd in _commands)
                {
                    cmd.Dispose();
                }
                _commands.Clear();
            }
        }

        #endregion
    }
}