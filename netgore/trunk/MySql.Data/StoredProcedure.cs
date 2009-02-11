using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for StoredProcedure.
    /// </summary>
    class StoredProcedure : PreparableStatement
    {
        string outSelect;
        DataTable parametersTable;
        string resolvedCommandText;

        public override string ResolvedCommandText
        {
            get { return resolvedCommandText; }
        }

        public StoredProcedure(MySqlCommand cmd, string text) : base(cmd, text)
        {
            // set our parameter hash to something very unique
            uint code = (uint)DateTime.Now.GetHashCode();
            cmd.parameterHash = code.ToString();
        }

        public override void Close()
        {
            base.Close();

            if (outSelect.Length == 0)
                return;

            MySqlCommand cmd = new MySqlCommand("SELECT " + outSelect, Connection);

            // set the parameter hash for this new command to our current parameter hash
            // so the inout and out parameters won't cause a problem
            string parameterHash = command.parameterHash;
            cmd.parameterHash = parameterHash;

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                // since MySQL likes to return user variables as strings
                // we reset the types of the readers internal value objects
                // this will allow those value objects to parse the string based
                // return values
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    fieldName = fieldName.Remove(0, parameterHash.Length + 1);
                    MySqlParameter parameter = Parameters.GetParameterFlexible(fieldName, true);
                    reader.values[i] = MySqlField.GetIMySqlValue(parameter.MySqlDbType);
                }

                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string fieldName = reader.GetName(i);
                        fieldName = fieldName.Remove(0, parameterHash.Length + 1);
                        MySqlParameter parameter = Parameters.GetParameterFlexible(fieldName, true);
                        parameter.Value = reader.GetValue(i);
                    }
                }
            }
        }

        public static string GetFlags(string dtd)
        {
            int x = dtd.Length - 1;
            while (x > 0 && (Char.IsLetterOrDigit(dtd[x]) || dtd[x] == ' '))
            {
                x--;
            }
            return dtd.Substring(x).ToUpper(CultureInfo.InvariantCulture);
        }

        DataSet GetParameters(string procName)
        {
            // if we can use mysql.proc, then do so
            if (Connection.Settings.UseProcedureBodies)
                return Connection.ProcedureCache.GetProcedure(Connection, procName);

            // we can't use mysql.proc so we attempt to "make do"
            DataSet ds = new DataSet();
            var restrictions = new string[4];
            int dotIndex = procName.IndexOf('.');
            restrictions[1] = procName.Substring(0, dotIndex++);
            restrictions[2] = procName.Substring(dotIndex, procName.Length - dotIndex);
            ds.Tables.Add(Connection.GetSchema("procedures", restrictions));

            // we use an internal method to create our procedure parameters table.  We pass
            // in a non-null routines table and this will prevent the code from attempting
            // a show create. It will process zero routine records but will return an empty
            // parameters table we can then fill.
            DataTable zeroRoutines = new DataTable();
            ISSchemaProvider sp = new ISSchemaProvider(Connection);
            DataTable pTable = sp.GetProcedureParameters(null, zeroRoutines);
            pTable.TableName = "procedure parameters";
            ds.Tables.Add(pTable);

            // now we run through the parameters that were set and fill in the parameters table
            // the best we can
            int pos = 1;
            foreach (MySqlParameter p in command.Parameters)
            {
                // in this mode, all parameters must have their type set
                if (!p.TypeHasBeenSet)
                    throw new InvalidOperationException(Resources.NoBodiesAndTypeNotSet);

                DataRow row = pTable.NewRow();
                row["PARAMETER_NAME"] = p.ParameterName;
                row["PARAMETER_MODE"] = "IN";
                if (p.Direction == ParameterDirection.InputOutput)
                    row["PARAMETER_MODE"] = "INOUT";
                else if (p.Direction == ParameterDirection.Output)
                    row["PARAMETER_MODE"] = "OUT";
                else if (p.Direction == ParameterDirection.ReturnValue)
                {
                    row["PARAMETER_MODE"] = "OUT";
                    row["ORDINAL_POSITION"] = 0;
                }
                else
                    row["ORDINAL_POSITION"] = pos++;
                pTable.Rows.Add(row);
            }
            return ds;
        }

        string GetReturnParameter()
        {
            if (Parameters != null)
            {
                foreach (MySqlParameter p in Parameters)
                {
                    if (p.Direction == ParameterDirection.ReturnValue)
                    {
                        string pName = p.ParameterName.Substring(1);
                        return command.parameterHash + pName;
                    }
                }
            }
            return null;
        }

        public override void Resolve()
        {
            // first retrieve the procedure definition from our
            // procedure cache
            string spName = commandText;
            string parameterHash = command.parameterHash;
            if (spName.IndexOf(".") == -1)
                spName = Connection.Database + "." + spName;

            DataSet ds = GetParameters(spName);

            DataTable procTable = ds.Tables["procedures"];
            parametersTable = ds.Tables["procedure parameters"];

            StringBuilder sqlStr = new StringBuilder();
            StringBuilder setStr = new StringBuilder();
            outSelect = String.Empty;

            string retParm = GetReturnParameter();
            foreach (DataRow param in parametersTable.Rows)
            {
                if (param["ORDINAL_POSITION"].Equals(0))
                    continue;
                string mode = (string)param["PARAMETER_MODE"];
                string pName = (string)param["PARAMETER_NAME"];

                // make sure the parameters given to us have an appropriate
                // type set if it's not already
                MySqlParameter p = command.Parameters.GetParameterFlexible(pName, true);
                if (!p.TypeHasBeenSet)
                {
                    string datatype = (string)param["DATA_TYPE"];
                    bool unsigned = GetFlags(param["DTD_IDENTIFIER"].ToString()).IndexOf("UNSIGNED") != -1;
                    bool real_as_float = procTable.Rows[0]["SQL_MODE"].ToString().IndexOf("REAL_AS_FLOAT") != -1;
                    p.MySqlDbType = MetaData.NameToType(datatype, unsigned, real_as_float, Connection);
                }

                string basePName = pName;
                if (pName.StartsWith("@") || pName.StartsWith("?"))
                    basePName = pName.Substring(1);
                string vName = string.Format("@{0}{1}", parameterHash, basePName);

                // if our parameter doesn't have a leading marker then we need to give it one
                pName = p.ParameterName;
                if (!pName.StartsWith("@") && !pName.StartsWith("?"))
                    pName = "@" + pName;

                if (mode == "OUT" || mode == "INOUT")
                {
                    outSelect += vName + ", ";
                    sqlStr.Append(vName);
                    sqlStr.Append(", ");
                }
                else
                {
                    sqlStr.Append(pName);
                    sqlStr.Append(", ");
                }

                if (mode == "INOUT")
                {
                    setStr.AppendFormat(CultureInfo.InvariantCulture, "SET {0}={1};", vName, pName);
                    outSelect += vName + ", ";
                }
            }

            string sqlCmd = sqlStr.ToString().TrimEnd(' ', ',');
            outSelect = outSelect.TrimEnd(' ', ',');
            if (procTable.Rows[0]["ROUTINE_TYPE"].Equals("PROCEDURE"))
                sqlCmd = String.Format("call {0} ({1})", commandText, sqlCmd);
            else
            {
                if (retParm == null)
                    retParm = parameterHash + "dummy";
                else
                    outSelect = String.Format("@{0}", retParm);
                sqlCmd = String.Format("SET @{0}={1}({2})", retParm, commandText, sqlCmd);
            }

            if (setStr.Length > 0)
                sqlCmd = setStr + sqlCmd;

            resolvedCommandText = sqlCmd;
        }
    }
}