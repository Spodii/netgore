using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Threading;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/mysqlcommand.xml' path='docs/ClassSummary/*'/>
    [ToolboxBitmap(typeof(MySqlCommand), "MySqlClient.resources.command.bmp")]
    [DesignerCategory("Code")]
    public sealed class MySqlCommand : DbCommand, ICloneable
    {
        internal Int64 lastInsertedId;
        internal string parameterHash;
        internal Exception thrownException;
        readonly MySqlParameterCollection parameters;
        IAsyncResult asyncResult;
        List<MySqlCommand> batch;
        string batchableCommandText;
        string cmdText;
        CommandType cmdType;
        int commandTimeout;
        MySqlConnection connection;
        MySqlTransaction curTransaction;
        bool designTimeVisible;
        bool resetSqlSelect;
        PreparableStatement statement;
        bool timedOut;
        long updatedRowCount;
        UpdateRowSource updatedRowSource;

        internal List<MySqlCommand> Batch
        {
            get { return batch; }
        }

        internal string BatchableCommandText
        {
            get { return batchableCommandText; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/CommandText/*'/>
        [Category("Data")]
        [Description("Command text to execute")]
        [Editor("MySql.Data.Common.Design.SqlCommandTextEditor,MySqlClient.Design", typeof(UITypeEditor))]
        public override string CommandText
        {
            get { return cmdText; }
            set
            {
                cmdText = value;
                statement = null;
                if (cmdText != null && cmdText.EndsWith("DEFAULT VALUES"))
                {
                    cmdText = cmdText.Substring(0, cmdText.Length - 14);
                    cmdText = cmdText + "() VALUES ()";
                }
            }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/CommandTimeout/*'/>
        [Category("Misc")]
        [Description("Time to wait for command to execute")]
        [DefaultValue(30)]
        public override int CommandTimeout
        {
            get { return commandTimeout == 0 ? 30 : commandTimeout; }
            set { commandTimeout = value; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/CommandType/*'/>
        [Category("Data")]
        public override CommandType CommandType
        {
            get { return cmdType; }
            set { cmdType = value; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/Connection/*'/>
        [Category("Behavior")]
        [Description("Connection used by the command")]
        public new MySqlConnection Connection
        {
            get { return connection; }
            set
            {
                /*
				* The connection is associated with the transaction
				* so set the transaction object to return a null reference if the connection 
				* is reset.
				*/
                if (connection != value)
                    Transaction = null;

                connection = value;

                // if the user has not already set the command timeout, then
                // take the default from the connection
                if (connection != null && commandTimeout == 0)
                    commandTimeout = (int)connection.Settings.DefaultCommandTimeout;
            }
        }

        ///<summary>
        ///
        ///                    Gets or sets the <see cref="T:System.Data.Common.DbConnection" /> used by this <see cref="T:System.Data.Common.DbCommand" />.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The connection to the data source.
        ///                
        ///</returns>
        ///
        protected override DbConnection DbConnection
        {
            get { return Connection; }
            set { Connection = (MySqlConnection)value; }
        }

        ///<summary>
        ///
        ///                    Gets the collection of <see cref="T:System.Data.Common.DbParameter" /> objects.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The parameters of the SQL statement or stored procedure.
        ///                
        ///</returns>
        ///
        protected override DbParameterCollection DbParameterCollection
        {
            get { return Parameters; }
        }

        ///<summary>
        ///
        ///                    Gets or sets the <see cref="P:System.Data.Common.DbCommand.DbTransaction" /> within which this <see cref="T:System.Data.Common.DbCommand" /> object executes.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The transaction within which a Command object of a .NET Framework data provider executes. The default value is a null reference (Nothing in Visual Basic).
        ///                
        ///</returns>
        ///
        protected override DbTransaction DbTransaction
        {
            get { return Transaction; }
            set { Transaction = (MySqlTransaction)value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a Windows Form Designer control. 
        /// </summary>
        [Browsable(false)]
        public override bool DesignTimeVisible
        {
            get { return designTimeVisible; }
            set { designTimeVisible = value; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/IsPrepared/*'/>
        [Browsable(false)]
        public bool IsPrepared
        {
            get { return statement != null && statement.IsPrepared; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/LastInseredId/*'/>
        [Browsable(false)]
        public Int64 LastInsertedId
        {
            get { return lastInsertedId; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/Parameters/*'/>
        [Category("Data")]
        [Description("The parameters collection")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new MySqlParameterCollection Parameters
        {
            get { return parameters; }
        }

        internal bool TimedOut
        {
            get { return timedOut; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/Transaction/*'/>
        [Browsable(false)]
        public new MySqlTransaction Transaction
        {
            get { return curTransaction; }
            set { curTransaction = value; }
        }

        internal int UpdateCount
        {
            get { return (int)updatedRowCount; }
        }

        /// <summary>
        /// Gets or sets how command results are applied to the DataRow when used by the 
        /// Update method of the DbDataAdapter. 
        /// </summary>
        public override UpdateRowSource UpdatedRowSource
        {
            get { return updatedRowSource; }
            set { updatedRowSource = value; }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ctor1/*'/>
        public MySqlCommand()
        {
            designTimeVisible = true;
            cmdType = CommandType.Text;
            parameters = new MySqlParameterCollection();
            updatedRowSource = UpdateRowSource.Both;
            cmdText = String.Empty;
            timedOut = false;
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ctor2/*'/>
        public MySqlCommand(string cmdText) : this()
        {
            CommandText = cmdText;
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ctor3/*'/>
        public MySqlCommand(string cmdText, MySqlConnection connection) : this(cmdText)
        {
            Connection = connection;
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ctor4/*'/>
        public MySqlCommand(string cmdText, MySqlConnection connection, MySqlTransaction transaction) : this(cmdText, connection)
        {
            curTransaction = transaction;
        }

        internal void AddToBatch(MySqlCommand command)
        {
            if (batch == null)
                batch = new List<MySqlCommand>();
            batch.Add(command);
        }

        internal void AsyncExecuteWrapper(int type, CommandBehavior behavior)
        {
            thrownException = null;
            try
            {
                if (type == 1)
                    ExecuteReader(behavior);
                else
                    ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the SQL statement or stored procedure 
        /// that is described by this <see cref="MySqlCommand"/>. 
        /// </summary>
        /// <param name="callback">
        /// An <see cref="AsyncCallback"/> delegate that is invoked when the command's 
        /// execution has completed. Pass a null reference (<b>Nothing</b> in Visual Basic) 
        /// to indicate that no callback is required.</param>
        /// <param name="stateObject">A user-defined state object that is passed to the 
        /// callback procedure. Retrieve this object from within the callback procedure 
        /// using the <see cref="IAsyncResult.AsyncState"/> property.</param>
        /// <returns>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, 
        /// or both; this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected rows. </returns>
        public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
        {
            AsyncDelegate del = AsyncExecuteWrapper;
            asyncResult = del.BeginInvoke(2, CommandBehavior.Default, callback, stateObject);
            return asyncResult;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the SQL statement or stored procedure 
        /// that is described by this <see cref="MySqlCommand"/>. 
        /// </summary>
        /// <returns>An <see cref="IAsyncResult"/> that can be used to poll or wait for results, 
        /// or both; this value is also needed when invoking <see cref="EndExecuteNonQuery"/>, 
        /// which returns the number of affected rows. </returns>
        public IAsyncResult BeginExecuteNonQuery()
        {
            AsyncDelegate del = AsyncExecuteWrapper;
            asyncResult = del.BeginInvoke(2, CommandBehavior.Default, null, null);
            return asyncResult;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the SQL statement or stored procedure 
        /// that is described by this <see cref="MySqlCommand"/>, and retrieves one or more 
        /// result sets from the server. 
        /// </summary>
        /// <returns>An <see cref="IAsyncResult"/> that can be used to poll, wait for results, 
        /// or both; this value is also needed when invoking EndExecuteReader, 
        /// which returns a <see cref="MySqlDataReader"/> instance that can be used to retrieve 
        /// the returned rows. </returns>
        public IAsyncResult BeginExecuteReader()
        {
            return BeginExecuteReader(CommandBehavior.Default);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the SQL statement or stored procedure 
        /// that is described by this <see cref="MySqlCommand"/> using one of the 
        /// <b>CommandBehavior</b> values. 
        /// </summary>
        /// <param name="behavior">One of the <see cref="CommandBehavior"/> values, indicating 
        /// options for statement execution and data retrieval.</param>
        /// <returns>An <see cref="IAsyncResult"/> that can be used to poll, wait for results, 
        /// or both; this value is also needed when invoking EndExecuteReader, 
        /// which returns a <see cref="MySqlDataReader"/> instance that can be used to retrieve 
        /// the returned rows. </returns>
        public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
        {
            AsyncDelegate del = AsyncExecuteWrapper;
            asyncResult = del.BeginInvoke(1, behavior, null, null);
            return asyncResult;
        }

        /// <summary>
        /// Attempts to cancel the execution of a MySqlCommand.
        /// </summary>
        /// <remarks>
        /// Cancelling a currently active query only works with MySQL versions 5.0.0 and higher.
        /// </remarks>
        public override void Cancel()
        {
            if (!connection.driver.Version.isAtLeast(5, 0, 0))
                throw new NotSupportedException(Resources.CancelNotSupported);

            MySqlConnectionStringBuilder cb = new MySqlConnectionStringBuilder(connection.Settings.GetConnectionString(true))
                                              { Pooling = false };
            using (MySqlConnection c = new MySqlConnection(cb.ConnectionString))
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand(String.Format("KILL QUERY {0}", connection.ServerThread), c);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Check the connection to make sure
        ///		- it is open
        ///		- it is not currently being used by a reader
        ///		- and we have the right version of MySQL for the requested command type
        /// </summary>
        void CheckState()
        {
            // There must be a valid and open connection.
            if (connection == null)
                throw new InvalidOperationException("Connection must be valid and open.");

            if (connection.State != ConnectionState.Open && !connection.SoftClosed)
                throw new InvalidOperationException("Connection must be valid and open.");

            if (CommandType == CommandType.StoredProcedure && !connection.driver.Version.isAtLeast(5, 0, 0))
                throw new MySqlException("Stored procedures are not supported on this version of MySQL");
        }

        internal void Close()
        {
            if (statement != null)
                statement.Close();

            // if we are supposed to reset the sql select limit, do that here
            if (resetSqlSelect)
                new MySqlCommand("SET SQL_SELECT_LIMIT=-1", connection).ExecuteNonQuery();
            resetSqlSelect = false;
        }

        ///<summary>
        ///
        ///                    Creates a new instance of a <see cref="T:System.Data.Common.DbParameter" /> object.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.Data.Common.DbParameter" /> object.
        ///                
        ///</returns>
        ///
        protected override DbParameter CreateDbParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        /// Creates a new instance of a <see cref="MySqlParameter"/> object.
        /// </summary>
        /// <remarks>
        /// This method is a strongly-typed version of <see cref="IDbCommand.CreateParameter"/>.
        /// </remarks>
        /// <returns>A <see cref="MySqlParameter"/> object.</returns>
        /// 
        public new MySqlParameter CreateParameter()
        {
            return (MySqlParameter)CreateDbParameter();
        }

        ///<summary>
        ///
        ///                    Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally releases the managed resources.
        ///                
        ///</summary>
        ///
        ///<param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        ///                </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (statement != null && statement.IsPrepared)
                    statement.CloseStatement();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finishes asynchronous execution of a SQL statement. 
        /// </summary>
        /// <param name="pasyncResult">The <see cref="IAsyncResult"/> returned by the call 
        /// to <see cref="BeginExecuteNonQuery()"/>.</param>
        /// <returns></returns>
        public int EndExecuteNonQuery(IAsyncResult pasyncResult)
        {
            pasyncResult.AsyncWaitHandle.WaitOne();
            if (thrownException != null)
                throw thrownException;
            return (int)updatedRowCount;
        }

        /// <summary>
        /// Finishes asynchronous execution of a SQL statement, returning the requested 
        /// <see cref="MySqlDataReader"/>.
        /// </summary>
        /// <param name="result">The <see cref="IAsyncResult"/> returned by the call to 
        /// <see cref="BeginExecuteReader()"/>.</param>
        /// <returns>A <b>MySqlDataReader</b> object that can be used to retrieve the requested rows. </returns>
        public MySqlDataReader EndExecuteReader(IAsyncResult result)
        {
            result.AsyncWaitHandle.WaitOne();
            if (thrownException != null)
            {
                MySqlDataReader r = connection.Reader;
                if (r != null)
                    r.Close();
                throw thrownException;
            }
            return connection.Reader;
        }

        internal long EstimatedSize()
        {
            long size = CommandText.Length;
            foreach (MySqlParameter parameter in Parameters)
            {
                size += parameter.EstimatedSize();
            }
            return size;
        }

        ///<summary>
        ///
        ///                    Executes the command text against the connection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.Data.Common.DbDataReader" />.
        ///                
        ///</returns>
        ///
        ///<param name="behavior">
        ///                    An instance of <see cref="T:System.Data.CommandBehavior" />.
        ///                </param>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return ExecuteReader(behavior);
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ExecuteNonQuery/*'/>
        public override int ExecuteNonQuery()
        {
            lastInsertedId = -1;
            updatedRowCount = -1;

            MySqlDataReader reader = null;
            try
            {
                reader = ExecuteReader();
                if (reader != null)
                {
                    reader.Close();
                    lastInsertedId = reader.InsertedId;
                    updatedRowCount = reader.RecordsAffected;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return (int)updatedRowCount;
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ExecuteReader/*'/>
        public new MySqlDataReader ExecuteReader()
        {
            return ExecuteReader(CommandBehavior.Default);
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ExecuteReader1/*'/>
        public new MySqlDataReader ExecuteReader(CommandBehavior behavior)
        {
            lastInsertedId = -1;
            CheckState();

            if (cmdText == null || cmdText.Trim().Length == 0)
                throw new InvalidOperationException(Resources.CommandTextNotInitialized);

            string sql = TrimSemicolons(cmdText);

            // now we check to see if we are executing a query that is buggy
            // in 4.1
            connection.IsExecutingBuggyQuery = false;
            if (!connection.driver.Version.isAtLeast(5, 0, 0) && connection.driver.Version.isAtLeast(4, 1, 0))
            {
                string snippet = sql;
                if (snippet.Length > 17)
                    snippet = sql.Substring(0, 17);
                snippet = snippet.ToLower(CultureInfo.InvariantCulture);
                connection.IsExecutingBuggyQuery = snippet.StartsWith("describe") || snippet.StartsWith("show table status");
            }

            if (statement == null || !statement.IsPrepared)
            {
                if (CommandType == CommandType.StoredProcedure)
                    statement = new StoredProcedure(this, sql);
                else
                    statement = new PreparableStatement(this, sql);
            }

            // stored procs are the only statement type that need do anything during resolve
            statement.Resolve();

            // Now that we have completed our resolve step, we can handle our
            // command behaviors
            HandleCommandBehaviors(behavior);

            updatedRowCount = -1;

            Timer timer = null;
            MySqlDataReader reader = null;
            bool successful = false;
            try
            {
                Connection.ReserveReader();
                reader = new MySqlDataReader(this, statement, behavior);

                // start a threading timer on our command timeout 
                timedOut = false;

                // execute the statement
                statement.Execute();

                // start a timeout timer
                if (connection.driver.Version.isAtLeast(5, 0, 0) && commandTimeout > 0)
                {
                    TimerCallback timerDelegate = TimeoutExpired;
                    timer = new Timer(timerDelegate, this, CommandTimeout * 1000, Timeout.Infinite);
                }

                connection.Reader = reader;

                // wait for data to return
                reader.NextResult();

                successful = true;
                return reader;
            }
            catch (MySqlException ex)
            {
                // if we caught an exception because of a cancel, then just return null
                if (ex.Number == 1317)
                {
                    if (TimedOut)
                        throw new MySqlException(Resources.Timeout);
                    return null;
                }
                if (ex.IsFatal)
                    Connection.Close();
                if (ex.Number == 0)
                    throw new MySqlException(Resources.FatalErrorDuringExecute, ex);

                throw;
            }
            finally
            {
                if (timer != null)
                    timer.Dispose();

                if (reader == null)
                    reader = connection.Reader;
                if (!successful && reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/ExecuteScalar/*'/>
        public override object ExecuteScalar()
        {
            lastInsertedId = -1;
            object val = null;

            MySqlDataReader reader = ExecuteReader();
            if (reader == null)
                return null;

            try
            {
                if (reader.Read())
                    val = reader.GetValue(0);
            }
            finally
            {
                reader.Close();
                lastInsertedId = reader.InsertedId;
            }

            return val;
        }

        internal string GetCommandTextForBatching()
        {
            if (batchableCommandText == null)
            {
                // if the command starts with insert and is "simple" enough, then
                // we can use the multi-value form of insert
                if (String.Compare(CommandText.Substring(0, 6), "INSERT", true) == 0)
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT @@sql_mode", Connection);
                    string sql_mode = cmd.ExecuteScalar().ToString().ToLower(CultureInfo.InvariantCulture);
                    SqlTokenizer tokenizer = new SqlTokenizer(CommandText)
                                             {
                                                 AnsiQuotes = (sql_mode.IndexOf("ansi_quotes") != -1),
                                                 BackslashEscapes = (sql_mode.IndexOf("no_backslash_escapes") == -1)
                                             };
                    string token = tokenizer.NextToken().ToLower(CultureInfo.InvariantCulture);
                    while (token != null)
                    {
                        if (token.ToLower(CultureInfo.InvariantCulture) == "values" && !tokenizer.Quoted)
                        {
                            token = tokenizer.NextToken();
                            Debug.Assert(token == "(");
                            while (token != null && token != ")")
                            {
                                batchableCommandText += token;
                                token = tokenizer.NextToken();
                            }
                            if (token != null)
                                batchableCommandText += token;
                            token = tokenizer.NextToken();
                            if (token != null && (token == "," || token.ToLower(CultureInfo.InvariantCulture) == "on"))
                            {
                                batchableCommandText = null;
                                break;
                            }
                        }
                        token = tokenizer.NextToken();
                    }
                }
                if (batchableCommandText == null)
                    batchableCommandText = CommandText;
            }

            return batchableCommandText;
        }

        void HandleCommandBehaviors(CommandBehavior behavior)
        {
            if ((behavior & CommandBehavior.SchemaOnly) != 0)
            {
                new MySqlCommand("SET SQL_SELECT_LIMIT=0", connection).ExecuteNonQuery();
                resetSqlSelect = true;
            }
            else if ((behavior & CommandBehavior.SingleRow) != 0)
            {
                new MySqlCommand("SET SQL_SELECT_LIMIT=1", connection).ExecuteNonQuery();
                resetSqlSelect = true;
            }
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/Prepare/*'/>
        public override void Prepare()
        {
            if (connection == null)
                throw new InvalidOperationException("The connection property has not been set.");
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("The connection is not open.");
            if (!connection.driver.Version.isAtLeast(4, 1, 0) || connection.Settings.IgnorePrepare)
                return;

            Prepare(0);
        }

        /// <include file='docs/mysqlcommand.xml' path='docs/Prepare2/*'/>
        void Prepare(int cursorPageSize)
        {
            if (!connection.driver.Version.isAtLeast(5, 0, 0) && cursorPageSize > 0)
                throw new InvalidOperationException("Nested commands are only supported on MySQL 5.0 and later");

            // if the length of the command text is zero, then just return
            string psSQL = CommandText;
            if (psSQL == null || psSQL.Trim().Length == 0)
                return;

            if (CommandType == CommandType.StoredProcedure)
                statement = new StoredProcedure(this, CommandText);
            else
                statement = new PreparableStatement(this, CommandText);

            statement.Prepare();
        }

        void TimeoutExpired(object commandObject)
        {
            MySqlCommand cmd = (MySqlCommand)commandObject;

            cmd.timedOut = true;
            try
            {
                cmd.Cancel();
            }
            catch (Exception ex)
            {
                // if something goes wrong, we log it and eat it.  There's really nothing
                // else we can do.
                if (connection.Settings.Logging)
                    Logger.LogException(ex);
            }
        }

        static string TrimSemicolons(string sql)
        {
            StringBuilder sb = new StringBuilder(sql);
            int start = 0;
            while (sb[start] == ';')
            {
                start++;
            }

            int end = sb.Length - 1;
            while (sb[end] == ';')
            {
                end--;
            }
            return sb.ToString(start, end - start + 1);
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a clone of this MySqlCommand object.  CommandText, Connection, and Transaction properties
        /// are included as well as the entire parameter list.
        /// </summary>
        /// <returns>The cloned MySqlCommand object</returns>
        object ICloneable.Clone()
        {
            MySqlCommand clone = new MySqlCommand(cmdText, connection, curTransaction)
                                 {
                                     CommandType = CommandType, CommandTimeout = CommandTimeout,
                                     batchableCommandText = batchableCommandText
                                 };

            foreach (MySqlParameter p in parameters)
            {
                clone.Parameters.Add((p as ICloneable).Clone());
            }
            return clone;
        }

        #endregion

        internal delegate void AsyncDelegate(int type, CommandBehavior behavior);
    }
}