using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Provides a class capable of executing a SQL script containing
    /// multiple SQL statements including CREATE PROCEDURE statements
    /// that require changing the delimiter
    /// </summary>
    public class MySqlScript
    {
        MySqlConnection connection;
        string delimiter;
        string query;

        /// <summary>
        /// Notifies the listener when there was an error.
        /// </summary>
        public event MySqlScriptErrorEventHandler Error;

        /// <summary>
        /// NOtifies the listener when the script was completed.
        /// </summary>
        public event EventHandler ScriptCompleted;

        /// <summary>
        /// Notifies the listener when a statement was executed.
        /// </summary>
        public event MySqlStatementExecutedEventHandler StatementExecuted;

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public MySqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        /// <value>The delimiter.</value>
        public string Delimiter
        {
            get { return delimiter; }
            set { delimiter = value; }
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MySqlScript"/> class.
        /// </summary>
        public MySqlScript()
        {
            delimiter = ";";
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MySqlScript"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public MySqlScript(MySqlConnection connection) : this()
        {
            this.connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MySqlScript"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public MySqlScript(string query) : this()
        {
            this.query = query;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="MySqlScript"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="query">The query.</param>
        public MySqlScript(MySqlConnection connection, string query) : this()
        {
            this.connection = connection;
            this.query = query;
        }

        List<ScriptStatement> BreakIntoStatements(bool ansiQuotes, bool noBackslashEscapes)
        {
            int startPos = 0;
            var statements = new List<ScriptStatement>();
            var lineNumbers = BreakScriptIntoLines();
            SqlTokenizer tokenizer = new SqlTokenizer(query) { AnsiQuotes = ansiQuotes, BackslashEscapes = (!noBackslashEscapes) };

            string token = tokenizer.NextToken();
            while (token != null)
            {
                if (!tokenizer.Quoted && !tokenizer.IsSize)
                {
                    int delimiterPos = token.IndexOf(Delimiter);
                    if (delimiterPos != -1)
                    {
                        int endPos = tokenizer.Index - token.Length + delimiterPos;
                        if (tokenizer.Index == query.Length - 1)
                            endPos++;
                        string currentQuery = query.Substring(startPos, endPos - startPos);
                        ScriptStatement statement = new ScriptStatement
                                                    { text = currentQuery.Trim(), line = FindLineNumber(startPos, lineNumbers) };
                        statement.position = startPos - lineNumbers[statement.line];
                        statements.Add(statement);
                        startPos = endPos + delimiter.Length;
                    }
                }
                token = tokenizer.NextToken();
            }

            // now clean up the last statement
            if (tokenizer.Index > startPos)
            {
                string sqlLeftOver = query.Substring(startPos).Trim();
                if (!String.IsNullOrEmpty(sqlLeftOver))
                {
                    ScriptStatement statement = new ScriptStatement
                                                { text = sqlLeftOver, line = FindLineNumber(startPos, lineNumbers) };
                    statement.position = startPos - lineNumbers[statement.line];
                    statements.Add(statement);
                }
            }
            return statements;
        }

        List<int> BreakScriptIntoLines()
        {
            var lineNumbers = new List<int>();

            StringReader sr = new StringReader(query);
            string line = sr.ReadLine();
            int pos = 0;
            while (line != null)
            {
                lineNumbers.Add(pos);
                pos += line.Length;
                line = sr.ReadLine();
            }
            return lineNumbers;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>The number of statements executed as part of the script.</returns>
        public int Execute()
        {
            bool openedConnection = false;

            if (connection == null)
                throw new InvalidOperationException(Resources.ConnectionNotSet);
            if (string.IsNullOrEmpty(query))
                return 0;

            // next we open up the connetion if it is not already open
            if (connection.State != ConnectionState.Open)
            {
                openedConnection = true;
                connection.Open();
            }

            // since we don't allow setting of parameters on a script we can 
            // therefore safely allow the use of user variables.  no one should be using
            // this connection while we are using it so we can temporarily tell it
            // to allow the use of user variables
            bool allowUserVars = connection.Settings.AllowUserVariables;
            connection.Settings.AllowUserVariables = true;

            try
            {
                string mode = connection.driver.Property("sql_mode");
                mode = mode.ToLower(CultureInfo.InvariantCulture);
                bool ansiQuotes = mode.IndexOf("ansi_quotes") != -1;
                bool noBackslashEscapes = mode.IndexOf("no_backslash_escpaes") != -1;

                // first we break the query up into smaller queries
                var statements = BreakIntoStatements(ansiQuotes, noBackslashEscapes);

                int count = 0;
                MySqlCommand cmd = new MySqlCommand(null, connection);
                foreach (ScriptStatement statement in statements)
                {
                    cmd.CommandText = statement.text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        count++;
                        OnQueryExecuted(statement);
                    }
                    catch (Exception ex)
                    {
                        if (Error == null)
                            throw;
                        if (!OnScriptError(ex))
                            break;
                    }
                }
                OnScriptCompleted();
                return count;
            }
            finally
            {
                connection.Settings.AllowUserVariables = allowUserVars;
                if (openedConnection)
                    connection.Close();
            }
        }

        static int FindLineNumber(int position, IList<int> lineNumbers)
        {
            int i = 0;
            while (i < lineNumbers.Count && position < lineNumbers[i])
            {
                i++;
            }
            return i;
        }

        void OnQueryExecuted(ScriptStatement statement)
        {
            if (StatementExecuted != null)
            {
                MySqlScriptEventArgs args = new MySqlScriptEventArgs { Statement = statement };
                StatementExecuted(this, args);
            }
        }

        void OnScriptCompleted()
        {
            if (ScriptCompleted != null)
                ScriptCompleted(this, null);
        }

        bool OnScriptError(Exception ex)
        {
            if (Error != null)
            {
                MySqlScriptErrorEventArgs args = new MySqlScriptErrorEventArgs(ex);
                Error(this, args);
                return args.Ignore;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void MySqlStatementExecutedEventHandler(object sender, MySqlScriptEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public delegate void MySqlScriptErrorEventHandler(object sender, MySqlScriptErrorEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class MySqlScriptEventArgs : EventArgs
    {
        ScriptStatement statement;

        /// <summary>
        /// Gets the line.
        /// </summary>
        /// <value>The line.</value>
        public int Line
        {
            get { return statement.line; }
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position
        {
            get { return statement.position; }
        }

        internal ScriptStatement Statement
        {
            set { statement = value; }
        }

        /// <summary>
        /// Gets the statement text.
        /// </summary>
        /// <value>The statement text.</value>
        public string StatementText
        {
            get { return statement.text; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MySqlScriptErrorEventArgs : MySqlScriptEventArgs
    {
        readonly Exception exception;
        bool ignore;

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception
        {
            get { return exception; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MySqlScriptErrorEventArgs"/> is ignore.
        /// </summary>
        /// <value><c>true</c> if ignore; otherwise, <c>false</c>.</value>
        public bool Ignore
        {
            get { return ignore; }
            set { ignore = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlScriptErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public MySqlScriptErrorEventArgs(Exception exception)
        {
            this.exception = exception;
        }
    }

    struct ScriptStatement
    {
        public int line;
        public int position;
        public string text;
    }
}