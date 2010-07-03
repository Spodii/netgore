using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database query classes. This class is thread-safe, and all derived classes should be thread-safe as well.
    /// </summary>
    public abstract class DbQueryBase : IDbQueryHandler
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The prefix character for database query parameters.
        /// </summary>
        public const string ParameterPrefix = "@";

        /// <summary>
        /// The prefix character for database query parameters.
        /// </summary>
        public const char ParameterPrefixChar = '@';

        const string _disposedErrorMessage = "Can not access methods on a disposed object.";
        const int _initialStackSize = 2;

        /// <summary>
        /// Empty collection of <see cref="DbParameter"/>s to use when no <see cref="DbParameter"/>s are specified for
        /// a query.
        /// </summary>
        static readonly IEnumerable<DbParameter> _emptyDbParameters = Enumerable.Empty<DbParameter>().ToCompact();

        readonly string _commandText;

        readonly Stack<DbCommand> _commands = new Stack<DbCommand>(_initialStackSize);
        readonly object _commandsLock = new object();
        readonly DbConnectionPool _connectionPool;
        readonly IEnumerable<DbParameter> _parameters;

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryBase"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        /// <param name="commandText">The command text.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryBase(DbConnectionPool connectionPool, string commandText)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException("commandText");

            _connectionPool = connectionPool;
            _commandText = commandText;

            // Grab the parameters
            var p = InitializeParameters();
            if (p == null)
            {
                // No parameters specified - use the empty list
                _parameters = _emptyDbParameters;
            }
            else
            {
                // Use the given parameters, compacted since it is readonly
                _parameters = p.ToCompact();

                // Validate the supplied parameters
                ValidateParameters(_parameters);
            }
        }

        /// <summary>
        /// Gets if this DbQueryBase contains a query that has any parameters.
        /// </summary>
        public bool HasParameters
        {
            get { return !_parameters.IsEmpty(); }
        }

        /// <summary>
        /// Gets if this DbQueryBase has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Creates a new DbParameter object from the given source DbParameter.
        /// </summary>
        /// <param name="source">DbParameter to clone.</param>
        /// <returns>Clone of the <paramref name="source"/> DbParameter.</returns>
        static DbParameter CloneDbParameter(DbParameter source)
        {
            var cloneable = source as ICloneable;
            if (cloneable != null)
            {
                var ret = (DbParameter)cloneable.Clone();
                Debug.Assert(ret != source,
                             "Object was cloned, but this implement of ICloneable returns the same object! That is not what we want!");
                return ret;
            }

            throw new NotImplementedException("Only DbParameters that implement ICloneable are currently supported.");
        }

        /// <summary>
        /// Creates a <see cref="DbParameter"/> that can be used with this <see cref="DbQueryBase"/>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter to create.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>DbParameter that can be used with this DbQueryBase.</returns>
        DbParameter CreateParameter(string parameterName)
        {
            if (!parameterName.StartsWith(ParameterPrefix))
            {
                // Append the parameter prefix
                parameterName = ParameterPrefix + parameterName;
            }
            else
            {
                // Prefix already existed
                const string errmsg =
                    "Parameter `{0}` already contained the ParameterPrefix ({1}). It is recommended to create parameters without specify the prefix explicitly.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, parameterName, ParameterPrefix);
                Debug.Fail(string.Format(errmsg, parameterName, ParameterPrefix));
            }

            return ConnectionPool.CreateParameter(parameterName);
        }

        /// <summary>
        /// Helps creates multiple <see cref="DbParameter"/>s easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1)
        {
            return new DbParameter[] { CreateParameter(param1) };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param2">Name of the second parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2)
        {
            return new DbParameter[] { CreateParameter(param1), CreateParameter(param2) };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param2">Name of the second parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param3">Name of the third parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2, string param3)
        {
            return new DbParameter[] { CreateParameter(param1), CreateParameter(param2), CreateParameter(param3) };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="param1">Name of the first parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param2">Name of the second parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param3">Name of the third parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <param name="param4">Name of the fourth parameter.
        /// Should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters for each of the specified parameter names.</returns>
        protected IEnumerable<DbParameter> CreateParameters(string param1, string param2, string param3, string param4)
        {
            return new DbParameter[]
            { CreateParameter(param1), CreateParameter(param2), CreateParameter(param3), CreateParameter(param4) };
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="parameterNames">Array of parameter names to create.
        /// The names should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters, one for each element in the <paramref name="parameterNames"/> array.</returns>
        protected IEnumerable<DbParameter> CreateParameters(params string[] parameterNames)
        {
            return CreateParameters((IEnumerable<string>)parameterNames);
        }

        /// <summary>
        /// Helps creates multiple DbParameters easily.
        /// </summary>
        /// <param name="parameterNames">IEnumerable of parameter names to create.
        /// The names should not be explicitly prefixed with the <see cref="ParameterPrefix"/>.</param>
        /// <returns>IEnumerable of DbParameters, one for each element in the <paramref name="parameterNames"/> array.</returns>
        protected IEnumerable<DbParameter> CreateParameters(IEnumerable<string> parameterNames)
        {
            if (parameterNames == null)
                throw new ArgumentNullException("parameterNames");

            var ret = new DbParameter[parameterNames.Count()];
            var i = 0;

            foreach (var parameterName in parameterNames)
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
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                ordinalToName[i] = dataReader.GetName(i);
            }

            // Start reading the values
            var fields = new List<Dictionary<string, object>>();
            while (dataReader.Read())
            {
                // Add all the field values to the dictionary
                var field = new Dictionary<string, object>();
                for (var i = 0; i < dataReader.FieldCount; i++)
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
            var sb = new StringBuilder(128);

            foreach (var field in fields)
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
            var sb = new StringBuilder(128);

            // Turn the IEnumerable into an array so we can ensure that both iterations are the EXACT same since
            // IEnumerable iterators do not guarentee two iterations to return the same set in the same order
            var farray = fields.ToArray();

            // Field names
            sb.Append("(");

            for (var i = 0; i < farray.Length; i++)
            {
                sb.Append("`");
                sb.Append(farray[i]);
                sb.Append("`");
                sb.Append(",");
            }
            sb.Length--; // Remove the last extra comma

            // Parameter names

            sb.Append(") VALUES (");

            for (var i = 0; i < farray.Length; i++)
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
        /// Formats a string into a query string.
        /// </summary>
        /// <param name="queryString">The query string. Follows the same rules as String.Format(), but each parameter in the
        /// query is identified by the character @. If you want to use the @ in the literal query, you can escape the character
        /// by prefixing it with another @. So two @'s (@@) will not be replaced by the parameter prefix.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The formatted query string.</returns>
        public static string FormatQueryString(string queryString, params object[] args)
        {
            if (args != null && args.Length > 0)
                queryString = string.Format(queryString, args);

            // If the ParameterPrefix is not @, then replace @ in the input string with the ParameterPrefix. This should never actually
            // happen since the ParameterPrefix has no reason to be anything other than @, but is provided for completeness.
#pragma warning disable 162
            if (ParameterPrefix != "@")
                throw new NotImplementedException(
                    "No support for ParameterPrefix not being '@' at this time since its not needed...");
#pragma warning restore 162

            return queryString;
        }

        /// <summary>
        /// Gets and sets up an available <see cref="DbCommand"/> that can be used to execute this query.
        /// </summary>
        /// <param name="conn">The <see cref="DbConnection"/> to assign the <see cref="DbCommand"/> to.</param>
        /// <returns>A <see cref="DbCommand"/> that that is set up for the <paramref name="conn"/>, but has not
        /// had it's parameter values set. The returned <see cref="DbCommand"/> should not be exposed. Instead, return
        /// it back to the pool by calling <see cref="DbQueryBase.ReleaseCommand"/>.</returns>
        /// <seealso cref="DbQueryBase.ReleaseCommand"/>
        protected DbCommand GetCommand(DbConnection conn)
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            DbCommand cmd = null;

            // Pop the next available command, if it exists
            lock (_commandsLock)
            {
                if (_commands.Count > 0)
                    cmd = _commands.Pop();
            }

            // Create a new command if we couldn't pop one from the stack
            if (cmd == null)
            {
                // Create the new command and copy over the command text
                cmd = conn.CreateCommand();
                cmd.CommandText = CommandText;

                Debug.Assert(cmd.Parameters.Count == 0);

                // Clone all of the parameters and add them to the new command
                var clonedParameters = _parameters.Select(CloneDbParameter).ToArray();
                cmd.Parameters.AddRange(clonedParameters);

                Debug.Assert(cmd.Parameters.Count == _parameters.Count());
            }

            // Set the connection
            cmd.Connection = conn;

            return cmd;
        }

        /// <summary>
        /// Gets the name of a <see cref="DbParameter"/> minus the single-character ampersand or question-mark prefix.
        /// </summary>
        /// <param name="parameter"><see cref="DbParameter"/> to get the name of.</param>
        /// <returns>The name of a <see cref="DbParameter"/> minus the single-character prefix.</returns>
        public static string GetParameterNameWithoutPrefix(DbParameter parameter)
        {
            return GetParameterNameWithoutPrefix(parameter.ParameterName);
        }

        /// <summary>
        /// Gets the name of a <see cref="DbParameter"/> minus the single-character ampersand or question-mark prefix.
        /// </summary>
        /// <param name="parameterName">ParameterName to get the trimmed name of.</param>
        /// <returns>The name of a <see cref="DbParameter"/> minus the single-character prefix.</returns>
        public static string GetParameterNameWithoutPrefix(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new ArgumentNullException(parameterName);

            // Check what the first character is
            switch (parameterName[0])
            {
                case '?':
                case ParameterPrefixChar:
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
        /// Gets an available <see cref="IPoolableDbConnection"/> from the <see cref="IDbQueryHandler.ConnectionPool"/>.
        /// </summary>
        /// <returns>A free <see cref="IPoolableDbConnection"/>.</returns>
        protected IPoolableDbConnection GetPoolableConnection()
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            return ConnectionPool.Acquire();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected abstract IEnumerable<DbParameter> InitializeParameters();

        /// <summary>
        /// Releases a <see cref="DbCommand"/> after it has been used. This must be called on every <see cref="DbCommand"/> once it is
        /// done being used to ensure it can be reused! Use this in place of disposing the <see cref="DbCommand"/>. Only
        /// <see cref="DbCommand"/>s from this <see cref="DbQueryBase"/>'s <see cref="GetCommand"/> method should be passed as the
        /// parameter.
        /// </summary>
        /// <param name="cmd"><see cref="DbCommand"/> to release.</param>
        /// <seealso cref="DbQueryBase.GetCommand"/>
        protected internal void ReleaseCommand(DbCommand cmd)
        {
            if (_disposed)
                throw new MethodAccessException(_disposedErrorMessage);

            AssertValidReleasedCommand(cmd);

            lock (_commandsLock)
                _commands.Push(cmd);
        }

        /// <summary>
        /// Makes sure that a <see cref="DbCommand"/> returned to this <see cref="DbQueryBase"/>'s pool from <see cref="ReleaseCommand"/>
        /// was a <see cref="DbCommand"/> that came from this instance.
        /// </summary>
        /// <param name="cmd">The <see cref="DbCommand"/> to check.</param>
        /// <remarks>The checking is not perfect since <see cref="DbCommand"/> does not store what <see cref="DbQueryBase"/> it originated
        /// from or any other definitive checks we can use. So instead, we have to just make some guesses based on the values available to
        /// us.</remarks>
        [Conditional("DEBUG")]
        private void AssertValidReleasedCommand(DbCommand cmd)
        {
            const string errmsg = "DbCommand `{0}` was returned to this DbQueryBase `{1}` through ReleaseCommand(), but it does not seem to be" + 
                " a DbCommand that belongs to this DbQueryBase! This will almost definitely result in database corruption and query execution" +
                " failure. If this is a false positive, ensure the reason for the false positive is resolved.";

            Debug.Assert(StringComparer.Ordinal.Equals(cmd.CommandText, CommandText), string.Format(errmsg, cmd, this));
            Debug.Assert(cmd.Parameters.Count == _parameters.Count(), string.Format(errmsg, cmd, this));
        }

        /// <summary>
        /// Performs pedantic validation of <see cref="DbParameter"/>s to make sure they are set up correctly.
        /// Used in debug builds only.
        /// </summary>
        /// <param name="parameters">The <see cref="DbParameter"/>s.</param>
        [Conditional("DEBUG")]
        static void ValidateParameters(IEnumerable<DbParameter> parameters)
        {
            foreach (var p in parameters)
            {
                var n = p.ParameterName;
                Debug.Assert(n.Length >= 2,
                             "Parameters should always be 2 or more characters (one for the prefix, one or more for the name).");
                Debug.Assert(n[0] == ParameterPrefixChar, "Parameters should begin with the ParameterPrefix.");
                Debug.Assert(n[1] != ParameterPrefixChar, "The second character should not be the ParameterPrefix.");
            }
        }

        #region IDbQueryHandler Members

        /// <summary>
        /// Gets the CommandText used by this IDbQueryHandler. All commands executed by this <see cref="IDbQueryHandler"/>
        /// will use this same CommandText.
        /// </summary>
        /// <value></value>
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
        /// Gets the <see cref="DbConnectionPool"/> used to manage the database connections.
        /// </summary>
        /// <value></value>
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
        /// Gets the parameters used in this <see cref="IDbQueryHandler"/>.
        /// </summary>
        /// <value></value>
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            lock (_commandsLock)
            {
                foreach (var cmd in _commands)
                {
                    cmd.Dispose();
                }
                _commands.Clear();
            }
        }

        #endregion
    }
}