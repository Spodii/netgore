using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for PreparedStatement.
    /// </summary>
    class PreparableStatement : Statement
    {
        int executionCount;
        MySqlField[] paramList;
        int statementId;

        public int ExecutionCount
        {
            get { return executionCount; }
            set { executionCount = value; }
        }

        public bool IsPrepared
        {
            get { return statementId > 0; }
        }

        public int NumParameters
        {
            get { return paramList.Length; }
        }

        public int StatementId
        {
            get { return statementId; }
        }

        public PreparableStatement(MySqlCommand command, string text) : base(command, text)
        {
        }

        public virtual void CloseStatement()
        {
            if (!IsPrepared)
                return;

            Driver.CloseStatement(statementId);
            statementId = 0;
        }

        public override void Execute()
        {
            // if we are not prepared, then call down to our base
            if (!IsPrepared)
            {
                base.Execute();
                return;
            }

            MySqlStream stream = new MySqlStream(Driver.Encoding);

            // create our null bitmap
            BitArray nullMap = new BitArray(Parameters.Count);

            // now we run through the parameters that PREPARE sent back and use
            // those names to index into the parameters the user gave us.
            // if the user set that parameter to NULL, then we set the null map
            // accordingly
            if (paramList != null)
            {
                for (int x = 0; x < paramList.Length; x++)
                {
                    MySqlParameter p = Parameters[paramList[x].ColumnName];
                    if (p.Value == DBNull.Value || p.Value == null)
                        nullMap[x] = true;
                }
            }
            var nullMapBytes = new byte[(Parameters.Count + 7) / 8];

            // we check this because Mono doesn't ignore the case where nullMapBytes
            // is zero length.
            if (nullMapBytes.Length > 0)
                nullMap.CopyTo(nullMapBytes, 0);

            // start constructing our packet
            stream.WriteInteger(statementId, 4);
            stream.WriteByte(0); // flags; always 0 for 4.1
            stream.WriteInteger(1, 4); // interation count; 1 for 4.1
            stream.Write(nullMapBytes);
            //if (parameters != null && parameters.Count > 0)
            stream.WriteByte(1); // rebound flag
            //else
            //	packet.WriteByte( 0 );

            // write out the parameter types
            if (paramList != null)
            {
                foreach (MySqlField param in paramList)
                {
                    MySqlParameter parm = Parameters[param.ColumnName];
                    stream.WriteInteger(parm.GetPSType(), 2);
                }

                // now write out all non-null values
                foreach (MySqlField param in paramList)
                {
                    int index = Parameters.IndexOf(param.ColumnName);
                    if (index == -1)
                        throw new MySqlException("Parameter '" + param.ColumnName + "' is not defined.");
                    MySqlParameter parm = Parameters[index];
                    if (parm.Value == DBNull.Value || parm.Value == null)
                        continue;

                    stream.Encoding = param.Encoding;
                    parm.Serialize(stream, true);
                }
            }

            executionCount++;

            Driver.ExecuteStatement(stream.InternalBuffer.ToArray());
        }

        public override bool ExecuteNext()
        {
            if (!IsPrepared)
                return base.ExecuteNext();
            return false;
        }

        public virtual void Prepare()
        {
            // strip out names from parameter markers
            string text;
            ArrayList parameter_names = PrepareCommandText(out text);

            // ask our connection to send the prepare command
            statementId = Driver.PrepareStatement(text, ref paramList);

            // now we need to assign our field names since we stripped them out
            // for the prepare
            for (int i = 0; i < parameter_names.Count; i++)
            {
                paramList[i].ColumnName = (string)parameter_names[i];
            }
        }

        /// <summary>
        /// Prepares CommandText for use with the Prepare method
        /// </summary>
        /// <returns>Command text stripped of all paramter names</returns>
        /// <remarks>
        /// Takes the output of TokenizeSql and creates a single string of SQL
        /// that only contains '?' markers for each parameter.  It also creates
        /// the parameterMap array list that includes all the paramter names in the
        /// order they appeared in the SQL
        /// </remarks>
        ArrayList PrepareCommandText(out string stripped_sql)
        {
            StringBuilder newSQL = new StringBuilder();
            ArrayList parameterMap = new ArrayList();

            // tokenize the sql first
            ArrayList tokens = TokenizeSql(ResolvedCommandText);
            parameterMap.Clear();

            foreach (string token in tokens)
            {
                if (token[0] != '@' && token[0] != '?')
                    newSQL.Append(token);
                else
                {
                    parameterMap.Add(token);
                    newSQL.Append("?");
                }
            }

            stripped_sql = newSQL.ToString();
            return parameterMap;
        }
    }
}