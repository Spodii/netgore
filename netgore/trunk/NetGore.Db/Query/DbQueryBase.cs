using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database query classes.
    /// </summary>
    public abstract class DbQueryBase : IDbQueryHandler
    {
        /// <summary>
        /// The prefix character for database query parameters.
        /// </summary>
        public const string ParameterPrefix = "@";

        const string _disposedErrorMessage = "Can not access methods on a disposed object.";
        const int _initialStackSize = 2;

        readonly Stack<DbCommand> _commands = new Stack<DbCommand>(_initialStackSize);
        readonly object _commandsLock = new object();
        readonly string _commandText;
        readonly DbConnectionPool _connectionPool;
        readonly bool _hasParameters;
        readonly IEnumerable<DbParameter> _parameters;
        bool _disposed;

        /// <summary>
        /// Gets if this DbQueryBase contains a query that has any parameters.
        /// </summary>
        public bool HasParameters
        {
            get { return _hasParameters; }
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
            _parameters = InitializeParameters() ?? Enumerable.Empty<DbParameter>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Keep track if we have any parameters or not
            _hasParameters = (_parameters.Count() > 0);
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
            throw new NotImplementedException("Only DbParameters that implement ICloneable are currently supported.");
        }

        /// <summary>
        /// Creates a DbParameter that can be used with this DbQueryBase.
        /// </summary>
        /// <param name="parameterName">Name of the parameter to create.</param>
        /// <returns>DbParameter that can be used with this DbQueryBase.</returns>
        protected DbParameter CreateParameter(string parameterName)
        {
            if (!parameterName.StartsWith(ParameterPrefix))
            {
                const string errmsg = "Parameter named `{0}` had an invalid or no prefix specified.";
                Debug.Fail(string.Format(errmsg, parameterName));
                parameterName = ParameterPrefix + parameterName;
            }

            return ConnectionPool.CreateParameter(parameterName);
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1)
        {
            return new DbParameter[] 
            { 
                CreateParameter(param1)
            };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.</param>
        /// <param name="param2">Name of the second parameter.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2)
        {
            return new DbParameter[] 
            { 
                CreateParameter(param1),
                CreateParameter(param2)
            };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.</param>
        /// <param name="param2">Name of the second parameter.</param>
        /// <param name="param3">Name of the third parameter.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2, string param3)
        {
            return new DbParameter[] 
            { 
                CreateParameter(param1),
                CreateParameter(param2),
                CreateParameter(param3)
            };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.</param>
        /// <param name="param2">Name of the second parameter.</param>
        /// <param name="param3">Name of the third parameter.</param>
        /// <param name="param4">Name of the fourth parameter.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2, string param3, string param4)
        {
            return new DbParameter[] 
            { 
                CreateParameter(param1),
                CreateParameter(param2),
                CreateParameter(param3),
                CreateParameter(param4)
            };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="parameterNames">Array of ParameterNames to create.</param>
        /// <returns>IEnumerable of DbParameters, one for each element in the <paramref name="parameterNames"/> array.</returns>
        protected IEnumerable<DbParameter> CreateParameters(params string[] parameterNames)
        {
            return CreateParameters((IEnumerable<string>)parameterNames);
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="parameterNames">IEnumerable of ParameterNames to create.</param>
        /// <returns>IEnumerable of DbParameters, one for each element in the <paramref name="parameterNames"/> array.</returns>
        protected IEnumerable<DbParameter> CreateParameters(IEnumerable<string> parameterNames)
        {
            if (parameterNames == null)
                throw new ArgumentNullException("parameterNames");

            DbParameter[] ret = new DbParameter[parameterNames.Count()];
            int i = 0;

            foreach (string parameterName in parameterNames)
            {
                ret[i] = CreateParameter(parameterName);
                i++;
            }

            return ret;
        }

        /// <summary>
        /// Loads all the values from all the rows in an IDataReader and converts it into a list of Dictionaries.
        /// </summary>
        /// <param name="dataReader">IDataReader to load the field names and values from.</param>
        /// <returns>List of Dictionaries with a key of the field's name and value of the field's value.</returns>
        public static List<Dictionary<string, object>> DataToDictionary(IDataReader dataReader)
        {
            // Get the name for each ordinal
            var ordinalToName = new string[dataReader.FieldCount];
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                ordinalToName[i] = dataReader.GetName(i);
            }

            // Start reading the values
            var fields = new List<Dictionary<string, object>>();
            while (dataReader.Read())
            {
                // Add all the field values to the dictionary
                var field = new Dictionary<string, object>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    field.Add(ordinalToName[i], dataReader.GetValue(i));
                }

                fields.Add(field);
            }

            return fields;
        }

        /// <summary>
        /// Turns an IEnumerable of strings into the format of: `field`=@field. Each field is comma-delimited.
        /// </summary>
        /// <returns>A comma-delimited string of all fields in the format of: `field`=@field.</returns>
        public static string FormatParametersIntoString(IEnumerable<string> fields)
        {
            StringBuilder sb = new StringBuilder(512);

            foreach (string field in fields)
            {
                sb.Append("`");
                sb.Append(field);
                sb.Append("`=@");
                sb.Append(field);
                sb.Append(",");
            }

            // Remove the trailing comma (last character)
            sb.Length--;

            return sb.ToString();
        }

        /// <summary>
        /// Turns an IEnumerable of strings into the format of: (`a`,`b`,...) VALUES (@a,@b,...). Each field is comma-delimited.
        /// </summary>
        /// <returns>A comma-delimited string of all fields in the format of: (`a`,`b`,...) VALUES (@a,@b,...).</returns>
        public static string FormatParametersIntoValuesString(IEnumerable<string> fields)
        {
            StringBuilder sb = new StringBuilder(512);

            // Turn the IEnumerable into an array so we can ensure that both iterations are the EXACT same since
            // IEnumerable iterators do not guarentee two iterations to return the same set in the same order
            var farray = fields.ToArray();

            // Field names
            sb.Append("(");

            for (int i = 0; i < farray.Length; i++)
            {
                sb.Append("`");
                sb.Append(farray[i]);
                sb.Append("`");
                sb.Append(",");
            }
            sb.Length--; // Remove the last extra comma

            // Parameter names

            sb.Append(") VALUES (");

            for (int i = 0; i < farray.Length; i++)
            {
                sb.Append("@");
                sb.Append(farray[i]);
                sb.Append(",");
            }

            sb.Length--; // Remove the last extra comma

            sb.Append(")");

            return sb.ToString();
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

                if (HasParameters)
                {
                    foreach (DbParameter parameter in _parameters)
                    {
                        DbParameter clone = CloneDbParameter(parameter);
                        cmd.Parameters.Add(clone);
                    }
                }
            }

            // Set the connection
            cmd.Connection = conn;

            return cmd;
        }

        /// <summary>
        /// Gets the name of a DbParameter minus the single-character ampersand or question-mark prefix.
        /// </summary>
        /// <param name="parameter">DbParameter to get the name of.</param>
        /// <returns>The name of a DbParameter minus the single-character prefix.</returns>
        public static string GetParameterNameWithoutPrefix(DbParameter parameter)
        {
            return GetParameterNameWithoutPrefix(parameter.ParameterName);
        }

        /// <summary>
        /// Gets the name of a DbParameter minus the single-character ampersand or question-mark prefix.
        /// </summary>
        /// <param name="parameterName">ParameterName to get the trimmed name of.</param>
        /// <returns>The name of a DbParameter minus the single-character prefix.</returns>
        public static string GetParameterNameWithoutPrefix(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentNullException(parameterName);

            // Check what the first character is
            switch (parameterName[0])
            {
                case '?':
                case '@':
                    // Remove the prefix
                    if (parameterName.Length == 1)
                        throw new ArgumentException("Invalid parameter name.");
                    return parameterName.Substring(1);

                default:
                    // No prefix, return as normal
                    return parameterName;
            }
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
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected abstract IEnumerable<DbParameter> InitializeParameters();

        /// <summary>
        /// Releases a DbCommand after it has been used. This must be called on every DbCommand once it is
        /// done being used to ensure it can be reused! Use this in place of disposing the DbCommand.
        /// </summary>
        /// <param name="cmd">DbCommand to release.</param>
        protected internal void ReleaseCommand(DbCommand cmd)
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
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