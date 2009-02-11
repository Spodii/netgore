using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    abstract class Statement
    {
        protected MySqlCommand command;
        protected string commandText;
        readonly ArrayList buffers;

        protected MySqlConnection Connection
        {
            get { return command.Connection; }
        }

        protected Driver Driver
        {
            get { return command.Connection.driver; }
        }

        protected MySqlParameterCollection Parameters
        {
            get { return command.Parameters; }
        }

        public virtual string ResolvedCommandText
        {
            get { return commandText; }
        }

        protected Statement(MySqlCommand cmd, string text) : this(cmd)
        {
            commandText = text;
        }

        Statement(MySqlCommand cmd)
        {
            command = cmd;
            buffers = new ArrayList();
        }

        protected virtual void BindParameters()
        {
            MySqlParameterCollection parameters = command.Parameters;
            int index = 0;

            while (true)
            {
                InternalBindParameters(ResolvedCommandText, parameters, null);

                // if we are not batching, then we are done.  This is only really relevant the
                // first time through
                if (command.Batch == null)
                    return;
                while (index < command.Batch.Count)
                {
                    MySqlCommand batchedCmd = command.Batch[index++];
                    MySqlStream stream = (MySqlStream)buffers[buffers.Count - 1];

                    // now we make a guess if this statement will fit in our current stream
                    long estimatedCmdSize = batchedCmd.EstimatedSize();
                    if ((stream.InternalBuffer.Length + estimatedCmdSize) > Connection.driver.MaxPacketSize)
                    {
                        // it won't, so we setup to start a new run from here
                        parameters = batchedCmd.Parameters;
                        break;
                    }

                    // looks like we might have room for it so we remember the current end of the stream
                    buffers.RemoveAt(buffers.Count - 1);
                    long originalLength = stream.InternalBuffer.Length;

                    // and attempt to stream the next command
                    string text = batchedCmd.BatchableCommandText;
                    if (text.StartsWith("("))
                        stream.WriteStringNoNull(", ");
                    else
                        stream.WriteStringNoNull("; ");
                    InternalBindParameters(text, batchedCmd.Parameters, stream);
                    if (stream.InternalBuffer.Length > Connection.driver.MaxPacketSize)
                    {
                        stream.InternalBuffer.SetLength(originalLength);
                        parameters = batchedCmd.Parameters;
                        break;
                    }
                }
                if (index == command.Batch.Count)
                    return;
            }
        }

        public virtual void Close()
        {
        }

        public virtual void Execute()
        {
            // we keep a reference to this until we are done
            BindParameters();
            ExecuteNext();
        }

        public virtual bool ExecuteNext()
        {
            if (buffers.Count == 0)
                return false;

            MySqlStream stream = (MySqlStream)buffers[0];
            MemoryStream ms = stream.InternalBuffer;
            Driver.Query(ms.GetBuffer(), (int)ms.Length);
            buffers.RemoveAt(0);
            return true;
        }

        void InternalBindParameters(string sql, MySqlParameterCollection parameters, MySqlStream stream)
        {
            // tokenize the sql
            ArrayList tokenArray = TokenizeSql(sql);

            if (stream == null)
                stream = new MySqlStream(Driver.Encoding) { Version = Driver.Version };

            // make sure our token array ends with a ;
            string lastToken = (string)tokenArray[tokenArray.Count - 1];
            if (lastToken != ";")
                tokenArray.Add(";");

            foreach (String token in tokenArray)
            {
                if (token.Trim().Length == 0)
                    continue;
                if (token == ";")
                {
                    buffers.Add(stream);
                    stream = new MySqlStream(Driver.Encoding);
                    continue;
                }
                if (token.Length >= 2 && ((token[0] == '@' && token[1] != '@') || token[0] == '?'))
                {
                    if (SerializeParameter(parameters, stream, token))
                        continue;
                }

                // our fall through case is to write the token to the byte stream
                stream.WriteStringNoNull(token);
            }
        }

        public virtual void Resolve()
        {
        }

        /// <summary>
        /// Serializes the given parameter to the given memory stream
        /// </summary>
        /// <remarks>
        /// <para>This method is called by PrepareSqlBuffers to convert the given
        /// parameter to bytes and write those bytes to the given memory stream.
        /// </para>
        /// </remarks>
        /// <returns>True if the parameter was successfully serialized, false otherwise.</returns>
        bool SerializeParameter(MySqlParameterCollection parameters, MySqlStream stream, string parmName)
        {
            MySqlParameter parameter = parameters.GetParameterFlexible(parmName, false);
            if (parameter == null)
            {
                // if we are allowing user variables and the parameter name starts with @
                // then we can't throw an exception
                if (parmName.StartsWith("@") && ShouldIgnoreMissingParameter(parmName))
                    return false;
                throw new MySqlException(String.Format(Resources.ParameterMustBeDefined, parmName));
            }
            parameter.Serialize(stream, false);
            return true;
        }

        protected virtual bool ShouldIgnoreMissingParameter(string parameterName)
        {
            if (Connection.Settings.AllowUserVariables)
                return true;
            if (command.parameterHash != null && parameterName.StartsWith("@" + command.parameterHash))
                return true;
            if (parameterName.Length > 1 && (parameterName[1] == '`' || parameterName[1] == '\''))
                return true;
            return false;
        }

        /// <summary>
        /// Breaks the given SQL up into 'tokens' that are easier to output
        /// into another form (bytes, preparedText, etc).
        /// </summary>
        /// <param name="sql">SQL to be tokenized</param>
        /// <returns>Array of tokens</returns>
        /// <remarks>The SQL is tokenized at parameter markers ('?') and at 
        /// (';') sql end markers if the server doesn't support batching.
        /// </remarks>
        public ArrayList TokenizeSql(string sql)
        {
            bool batch = Connection.Settings.AllowBatch & Driver.SupportsBatch;
            char delim = Char.MinValue;
            StringBuilder sqlPart = new StringBuilder();
            bool escaped = false;
            ArrayList tokens = new ArrayList();

            sql = sql.TrimStart(';').TrimEnd(';');

            for (int i = 0; i < sql.Length; i++)
            {
                char c = sql[i];
                if (escaped)
                    escaped = !true;
                else if (c == delim)
                    delim = Char.MinValue;
                else if (c == ';' && delim == Char.MinValue && !batch)
                {
                    tokens.Add(sqlPart.ToString());
                    tokens.Add(";");
                    sqlPart.Remove(0, sqlPart.Length);
                    continue;
                }
                else if ((c == '\'' || c == '\"' || c == '`') && delim == Char.MinValue)
                    delim = c;
                else if (c == '\\')
                    escaped = !false;
                else if (sqlPart.Length == 1 && sqlPart[0] == '@' && c == '@')
                {
                }
                else if (sqlPart.Length > 0 && sqlPart[0] == '?' && c == '@')
                {
                }
                else if ((c == '@' || c == '?') && delim == Char.MinValue)
                {
                    if (sqlPart[0] != c)
                    {
                        tokens.Add(sqlPart.ToString());
                        sqlPart.Remove(0, sqlPart.Length);
                    }
                }
                else if (sqlPart.Length > 0 && (sqlPart[0] == '@' || sqlPart[0] == '?') && !Char.IsLetterOrDigit(c) && c != '_' &&
                         c != '.' && c != '$')
                {
                    tokens.Add(sqlPart.ToString());
                    sqlPart.Remove(0, sqlPart.Length);
                }

                sqlPart.Append(c);
            }
            tokens.Add(sqlPart.ToString());
            return tokens;
        }
    }
}