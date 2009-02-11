using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using DemoGame.Extensions;
using log4net;
using MySql.Data.MySqlClient;
using NetGore.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Base handler for a thread-safe, asynchronous SQL query that does not result in a return. All
    /// queries handled by this NonQueryBase will not guarentee to be executed immediately -
    /// queries can end up being pooled.
    /// </summary>
    /// <typeparam name="T">Type of item containing the parameters used for preparing the statement
    /// in the overridden SetParameters() method. Because these are handled asynchronously, it is highly
    /// recommended that they are immutable types.</typeparam>
    public abstract class NonReaderQueryBase<T> : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly MySqlCommand _command;

        readonly object _queryLock = new object();
        readonly Queue<T> _workQueue = new Queue<T>();
        IAsyncResult _asyncResult;
        bool _disposed = false;
        bool _isBusy = false;
        bool _useAsync = false; // FUTURE: Can enable this when I get the MySQL shit worked out

        /// <summary>
        /// Gets the MySqlConnection used by this NonReaderQueryBase.
        /// </summary>
        public MySqlConnection MySqlConnection
        {
            get { return _command.Connection; }
        }

        /// <summary>
        /// Gets or sets if the NonReadyQueryBase is used asynchronously. Recommended to set as false
        /// when trying to debug database queries.
        /// </summary>
        public bool UseAsync
        {
            get
            {
                lock (_queryLock)
                {
                    return _useAsync;
                }
            }
            set
            {
                lock (_queryLock)
                {
                    _useAsync = value;
                }
            }
        }

        protected NonReaderQueryBase(MySqlConnection conn)
        {
            _command = conn.CreateCommand();
        }

        /// <summary>
        /// Adds a parameter to the underlying MySqlCommand. These parameters are later used
        /// in SetParameters(). All parameters must be set before the first call to Execute().
        /// </summary>
        /// <param name="parameter">Parameter to add</param>
        protected void AddParameter(MySqlParameter parameter)
        {
            _command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Adds parameters to the underlying MySqlCommand. These parameters are later used
        /// in SetParameters(). All parameters must be set before the first call to Execute().
        /// </summary>
        /// <param name="parameters">Parameters to add</param>
        protected void AddParameters(IEnumerable<MySqlParameter> parameters)
        {
            foreach (MySqlParameter p in parameters)
            {
                _command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// Adds parameters to the underlying MySqlCommand. These parameters are later used
        /// in SetParameters(). All parameters must be set before the first call to Execute().
        /// </summary>
        /// <param name="parameters">Parameters to add</param>
        protected void AddParameters(params MySqlParameter[] parameters)
        {
            _command.Parameters.AddRange(parameters);
        }

        void BeginExecute(T item)
        {
            // We are now busy
            _isBusy = true;

            // Set up the parameters
            SetParameters(item);

            // Begin executing the query
            if (UseAsync)
                _asyncResult = _command.BeginExecuteNonQuery(QueryCallback, _command);
            else
            {
                CheckRowsAffected(_command.ExecuteNonQuery());
                _isBusy = false;
            }
        }

        void CheckRowsAffected(int rowsAffected)
        {
            if (rowsAffected != 0)
                return;

            const string errmsg = "Prepared sql query resulted in no rows affected: {0} {1}Parameters: {2}";
            Debug.Fail(string.Format(errmsg, _command.CommandText, Environment.NewLine, _command.Parameters.Implode(',')));
            if (log.IsErrorEnabled)
                log.ErrorFormat(errmsg, _command.CommandText, Environment.NewLine, _command.Parameters.Implode(','));
        }

        /// <summary>
        /// Adds an item to the NonReaderQueryBase's queue so it can be executed. The item may be processed
        /// immediately or after quite some time, so it is best to only use an immutable object.
        /// </summary>
        /// <param name="item">Item to process.</param>
        public void Execute(T item)
        {
            Debug.Assert(_command.CommandText.Length != 0,
                         "The CommandText was never set. Initialize() may not have been called yet.");

            lock (_queryLock)
            {
                if (!_isBusy)
                {
                    // We are not busy, so execute the query right away
                    BeginExecute(item);
                }
                else
                {
                    // We are busy so we have to push the item into the queue
                    _workQueue.Enqueue(item);
                }
            }
        }

        /// <summary>
        /// Flushes out all of the pooled queries, blocking until each one has been executed.
        /// </summary>
        public void Flush()
        {
            // FUTURE: This is a very poor implementation, but whatever - it is easy. Flush shouldn't really be called except for when disposing.
            while (_workQueue.Count > 0)
            {
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Gets the name of a field from a IDataParameter by cropping off the @ prefix.
        /// </summary>
        /// <param name="param">IDataParameter to get the field name for.</param>
        /// <returns>Field name for the IDataParameter.</returns>
        protected static string GetFieldName(IDataParameter param)
        {
            return param.ParameterName.Substring(1);
        }

        /// <summary>
        /// Turns an IEnumerable of a string into the format of: `field`=@field. Each entry is comma-delimited.
        /// </summary>
        /// <returns>A comma-delimited string of all fields in the format of: `field`=@field.</returns>
        protected static string GetValuesQuery(IEnumerable<string> fields)
        {
            StringBuilder sb = new StringBuilder(4096);

            foreach (string s in fields)
            {
                sb.Append("`" + s + "`=@" + s + ",");
            }

            // Remove the trailing comma
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// Sets and initializes the query.
        /// </summary>
        /// <param name="commandText">Query that this NonReaderQueryBase will perform.</param>
        protected void Initialize(string commandText)
        {
            // Set the command text
            _command.CommandText = commandText;

            // FUTURE: Why the fuck are my fucking prepared statments not fucking working!?
            // This can be turned into prepared statements by just uncommenting the block below
            // but the problem is that, for some reason, the values aren't handled properly during DB I/O

            /*
            // Prepare the command
            _command.Prepare();

            // Ensure the command was prepared
            if (!_command.IsPrepared)
            {
                const string errmsg = "Failed to Prepare() the MySqlCommand.";
                Debug.Fail(errmsg);
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);
            }
            */
        }

        void QueryCallback(IAsyncResult result)
        {
            Debug.Assert(_asyncResult == result, "Stored IAsyncResult does not match the received result.");
            Debug.Assert(result.IsCompleted, "Uhm... uh... what!? Uh-oh!");
            Debug.Assert(result.AsyncState == _command, "How the hell did we get a message from somewhere else!?");

            CheckRowsAffected(_command.EndExecuteNonQuery(result));

            lock (_queryLock)
            {
                // If we have a queue, process the next item in the queue right away
                if (_workQueue.Count > 0)
                    BeginExecute(_workQueue.Dequeue());
                else
                    _isBusy = false;
            }
        }

        /// <summary>
        /// Sets the database parameters to the appropriate parameters for the <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item to set the parameters for.</param>
        protected abstract void SetParameters(T item);

        #region IDisposable Members

        /// <summary>
        /// Flushes the unfinished queries and disposes of the MySqlCommand used to perform the queries.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            Flush();

            _disposed = true;
            _command.Dispose();
        }

        #endregion
    }
}