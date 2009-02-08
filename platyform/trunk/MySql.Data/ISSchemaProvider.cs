using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    class ISSchemaProvider : SchemaProvider
    {
        public ISSchemaProvider(MySqlConnection connection) : base(connection)
        {
        }

        protected override DataTable GetCollections()
        {
            DataTable dt = base.GetCollections();

            var collections = new object[][]
                              {
                                  new object[] { "Views", 2, 3 }, new object[] { "ViewColumns", 3, 4 },
                                  new object[] { "Procedure Parameters", 5, 1 }, new object[] { "Procedures", 4, 3 },
                                  new object[] { "Triggers", 2, 4 }
                              };

            FillTable(dt, collections);
            return dt;
        }

        public override DataTable GetColumns(string[] restrictions)
        {
            var keys = new string[4];
            keys[0] = "TABLE_CATALOG";
            keys[1] = "TABLE_SCHEMA";
            keys[2] = "TABLE_NAME";
            keys[3] = "COLUMN_NAME";
            DataTable dt = Query("COLUMNS", null, keys, restrictions);
            dt.Columns.Remove("CHARACTER_OCTET_LENGTH");
            dt.TableName = "Columns";
            return dt;
        }

        public override DataTable GetDatabases(string[] restrictions)
        {
            var keys = new string[1];
            keys[0] = "SCHEMA_NAME";
            DataTable dt = Query("SCHEMATA", "", keys, restrictions);
            dt.Columns[1].ColumnName = "database_name";
            dt.TableName = "Databases";
            return dt;
        }

        static string GetDataTypeDefaults(string type, DataRow row)
        {
            string format = "({0},{1})";

            if (MetaData.IsNumericType(type) && row["NUMERIC_PRECISION"].ToString().Length == 0)
            {
                row["NUMERIC_PRECISION"] = 10;
                row["NUMERIC_SCALE"] = 0;
                if (!MetaData.SupportScale(type))
                    format = "({0})";
                return String.Format(format, row["NUMERIC_PRECISION"], row["NUMERIC_SCALE"]);
            }
            return String.Empty;
        }

        DataTable GetParametersFromIS(string[] restrictions)
        {
            StringBuilder sql = new StringBuilder(@"SELECT * FROM INFORMATION_SCHEMA.PARAMETERS");

            var keys = new string[5];
            keys[0] = "SPECIFIC_CATALOG";
            keys[1] = "SPECIFIC_SCHEMA";
            keys[2] = "SPECIFIC_NAME";
            keys[3] = "ROUTINE_TYPE";
            keys[4] = "PARAMETER_NAME";

            // now get our where clause and append it if there is one
            string where = GetWhereClause(null, keys, restrictions);
            if (!String.IsNullOrEmpty(where))
                sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}", where);

            DataTable dt = GetTable(sql.ToString());
            dt.TableName = "Procedure Parameters";

            return dt;
        }

        internal void GetParametersFromShowCreate(DataTable parametersTable, string[] restrictions, DataTable routines)
        {
            // this allows us to pass in a pre-populated routines table
            // and avoid the querying for them again.
            // we use this when calling a procedure or function
            if (routines == null)
                routines = GetSchema("procedures", restrictions);

            MySqlCommand cmd = connection.CreateCommand();

            foreach (DataRow routine in routines.Rows)
            {
                string showCreateSql = String.Format("SHOW CREATE {0} `{1}`.`{2}`", routine["ROUTINE_TYPE"],
                                                     routine["ROUTINE_SCHEMA"], routine["ROUTINE_NAME"]);
                cmd.CommandText = showCreateSql;
                try
                {
                    string nameToRestrict = null;
                    if (restrictions != null && restrictions.Length == 5 && restrictions[4] != null)
                        nameToRestrict = restrictions[4];
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        string body = reader.GetString(2);
                        reader.Close();
                        ParseProcedureBody(parametersTable, body, routine, nameToRestrict);
                    }
                }
                catch (SqlNullValueException snex)
                {
                    throw new InvalidOperationException(Resources.UnableToRetrieveSProcData, snex);
                }
            }
        }

        string GetProcedureParameterLine(DataRow isRow)
        {
            string sql = "SHOW CREATE {0} `{1}`.`{2}`";
            sql = String.Format(sql, isRow["ROUTINE_TYPE"], isRow["ROUTINE_SCHEMA"], isRow["ROUTINE_NAME"]);
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();

                // if we are not the owner of this proc or have permissions
                // then we will get null for the body
                if (reader.IsDBNull(2))
                    return null;

                string sql_mode = reader.GetString(1);

                string body = reader.GetString(2);
                SqlTokenizer tokenizer = new SqlTokenizer(body)
                                         {
                                             AnsiQuotes = (sql_mode.IndexOf("ANSI_QUOTES") != -1),
                                             BackslashEscapes = (sql_mode.IndexOf("NO_BACKSLASH_ESCAPES") == -1)
                                         };

                string token = tokenizer.NextToken();
                while (token != "(")
                {
                    token = tokenizer.NextToken();
                }
                int start = tokenizer.Index + 1;
                token = tokenizer.NextToken();
                while (token != ")" || tokenizer.Quoted)
                {
                    token = tokenizer.NextToken();
                    // if we see another ( and we are not quoted then we
                    // are in a size element and we need to look for the closing paren
                    if (token == "(" && !tokenizer.Quoted)
                    {
                        while (token != ")" || tokenizer.Quoted)
                        {
                            token = tokenizer.NextToken();
                        }
                        token = tokenizer.NextToken();
                    }
                }
                return body.Substring(start, tokenizer.Index - start);
            }
        }

        /// <summary>
        /// Return schema information about parameters for procedures and functions
        /// Restrictions supported are:
        /// schema, name, type, parameter name
        /// </summary>
        public virtual DataTable GetProcedureParameters(string[] restrictions, DataTable routines)
        {
            if (connection.driver.Version.isAtLeast(6, 0, 6))
                return GetParametersFromIS(restrictions);
            else
            {
                DataTable dt = new DataTable("Procedure Parameters");
                dt.Columns.Add("SPECIFIC_CATALOG", typeof(string));
                dt.Columns.Add("SPECIFIC_SCHEMA", typeof(string));
                dt.Columns.Add("SPECIFIC_NAME", typeof(string));
                dt.Columns.Add("ORDINAL_POSITION", typeof(Int32));
                dt.Columns.Add("PARAMETER_MODE", typeof(string));
                dt.Columns.Add("PARAMETER_NAME", typeof(string));
                dt.Columns.Add("DATA_TYPE", typeof(string));
                dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof(Int32));
                dt.Columns.Add("CHARACTER_OCTET_LENGTH", typeof(Int32));
                dt.Columns.Add("NUMERIC_PRECISION", typeof(byte));
                dt.Columns.Add("NUMERIC_SCALE", typeof(Int32));
                dt.Columns.Add("CHARACTER_SET_NAME", typeof(string));
                dt.Columns.Add("COLLATION_NAME", typeof(string));
                dt.Columns.Add("DTD_IDENTIFIER", typeof(string));
                dt.Columns.Add("ROUTINE_TYPE", typeof(string));
                GetParametersFromShowCreate(dt, restrictions, routines);

                return dt;
            }
        }

        /// <summary>
        /// Return schema information about procedures and functions
        /// Restrictions supported are:
        /// schema, name, type
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public override DataTable GetProcedures(string[] restrictions)
        {
            // if the user has said that we have access to mysql.proc then
            // we use that as it is a lot faster
            if (connection.Settings.UseProcedureBodies)
                return base.GetProcedures(restrictions);

            var keys = new string[4];
            keys[0] = "ROUTINE_CATALOG";
            keys[1] = "ROUTINE_SCHEMA";
            keys[2] = "ROUTINE_NAME";
            keys[3] = "ROUTINE_TYPE";

            DataTable dt = Query("ROUTINES", null, keys, restrictions);
            dt.TableName = "Procedures";
            return dt;
        }

        DataTable GetProceduresWithParameters(string[] restrictions)
        {
            DataTable dt = GetProcedures(restrictions);
            dt.Columns.Add("ParameterList", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["ParameterList"] = GetProcedureParameterLine(row);
            }
            return dt;
        }

        protected override DataTable GetRestrictions()
        {
            DataTable dt = base.GetRestrictions();

            var restrictions = new object[][]
                               {
                                   new object[] { "Procedure Parameters", "Database", "", 0 },
                                   new object[] { "Procedure Parameters", "Schema", "", 1 },
                                   new object[] { "Procedure Parameters", "Name", "", 2 },
                                   new object[] { "Procedure Parameters", "Type", "", 3 },
                                   new object[] { "Procedure Parameters", "Parameter", "", 4 },
                                   new object[] { "Procedures", "Database", "", 0 }, new object[] { "Procedures", "Schema", "", 1 },
                                   new object[] { "Procedures", "Name", "", 2 }, new object[] { "Procedures", "Type", "", 3 },
                                   new object[] { "Views", "Database", "", 0 }, new object[] { "Views", "Schema", "", 1 },
                                   new object[] { "Views", "Table", "", 2 }, new object[] { "ViewColumns", "Database", "", 0 },
                                   new object[] { "ViewColumns", "Schema", "", 1 }, new object[] { "ViewColumns", "Table", "", 2 },
                                   new object[] { "ViewColumns", "Column", "", 3 }, new object[] { "Triggers", "Database", "", 0 },
                                   new object[] { "Triggers", "Schema", "", 1 }, new object[] { "Triggers", "Name", "", 2 },
                                   new object[] { "Triggers", "EventObjectTable", "", 3 },
                               };
            FillTable(dt, restrictions);
            return dt;
        }

        protected override DataTable GetSchemaInternal(string collection, string[] restrictions)
        {
            DataTable dt = base.GetSchemaInternal(collection, restrictions);
            if (dt != null)
                return dt;

            switch (collection)
            {
                case "views":
                    return GetViews(restrictions);
                case "procedures":
                    return GetProcedures(restrictions);
                case "procedures with parameters":
                    return GetProceduresWithParameters(restrictions);
                case "procedure parameters":
                    return GetProcedureParameters(restrictions, null);
                case "triggers":
                    return GetTriggers(restrictions);
                case "viewcolumns":
                    return GetViewColumns(restrictions);
            }
            return null;
        }

        DataTable GetTable(string sql)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
            da.Fill(table);
            return table;
        }

        public override DataTable GetTables(string[] restrictions)
        {
            var keys = new string[4];
            keys[0] = "TABLE_CATALOG";
            keys[1] = "TABLE_SCHEMA";
            keys[2] = "TABLE_NAME";
            keys[3] = "TABLE_TYPE";
            DataTable dt = Query("TABLES", "TABLE_TYPE != 'VIEW'", keys, restrictions);
            dt.TableName = "Tables";
            return dt;
        }

        DataTable GetTriggers(string[] restrictions)
        {
            var keys = new string[4];
            keys[0] = "TRIGGER_CATALOG";
            keys[1] = "TRIGGER_SCHEMA";
            keys[2] = "EVENT_OBJECT_TABLE";
            keys[3] = "TRIGGER_NAME";
            DataTable dt = Query("TRIGGERS", null, keys, restrictions);
            dt.TableName = "Triggers";
            return dt;
        }

        DataTable GetViewColumns(string[] restrictions)
        {
            StringBuilder where = new StringBuilder();
            StringBuilder sql = new StringBuilder("SELECT C.* FROM information_schema.columns C");
            sql.Append(" JOIN information_schema.views V ");
            sql.Append("ON C.table_schema=V.table_schema AND C.table_name=V.table_name ");
            if (restrictions != null && restrictions.Length >= 2 && restrictions[1] != null)
                where.AppendFormat(CultureInfo.InvariantCulture, "C.table_schema='{0}' ", restrictions[1]);
            if (restrictions != null && restrictions.Length >= 3 && restrictions[2] != null)
            {
                if (where.Length > 0)
                    where.Append("AND ");
                where.AppendFormat(CultureInfo.InvariantCulture, "C.table_name='{0}' ", restrictions[2]);
            }
            if (restrictions != null && restrictions.Length == 4 && restrictions[3] != null)
            {
                if (where.Length > 0)
                    where.Append("AND ");
                where.AppendFormat(CultureInfo.InvariantCulture, "C.column_name='{0}' ", restrictions[3]);
            }
            if (where.Length > 0)
                sql.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}", where);
            DataTable dt = GetTable(sql.ToString());
            dt.TableName = "ViewColumns";
            dt.Columns[0].ColumnName = "VIEW_CATALOG";
            dt.Columns[1].ColumnName = "VIEW_SCHEMA";
            dt.Columns[2].ColumnName = "VIEW_NAME";
            return dt;
        }

        DataTable GetViews(string[] restrictions)
        {
            var keys = new string[3];
            keys[0] = "TABLE_CATALOG";
            keys[1] = "TABLE_SCHEMA";
            keys[2] = "TABLE_NAME";
            DataTable dt = Query("VIEWS", null, keys, restrictions);
            dt.TableName = "Views";
            return dt;
        }

        static string GetWhereClause(string initial_where, string[] keys, string[] values)
        {
            StringBuilder where = new StringBuilder(initial_where);
            if (values != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (i >= values.Length)
                        break;
                    if (string.IsNullOrEmpty(values[i]))
                        continue;
                    if (where.Length > 0)
                        where.Append(" AND ");
                    where.AppendFormat(CultureInfo.InvariantCulture, "{0} LIKE '{1}'", keys[i], values[i]);
                }
            }
            return where.ToString();
        }

        /// <summary>
        /// Initializes a new row for the procedure parameters table.
        /// </summary>
        static void InitParameterRow(DataRow procedure, DataRow parameter)
        {
            parameter["SPECIFIC_CATALOG"] = null;
            parameter["SPECIFIC_SCHEMA"] = procedure["ROUTINE_SCHEMA"];
            parameter["SPECIFIC_NAME"] = procedure["ROUTINE_NAME"];
            parameter["PARAMETER_MODE"] = "IN";
            parameter["ORDINAL_POSITION"] = 0;
            parameter["ROUTINE_TYPE"] = procedure["ROUTINE_TYPE"];
        }

        /// <summary>
        ///  Parses out the elements of a procedure parameter data type.
        /// </summary>
        string ParseDataType(DataRow row, SqlTokenizer tokenizer)
        {
            StringBuilder dtd = new StringBuilder(tokenizer.NextToken().ToUpper(CultureInfo.InvariantCulture));
            row["DATA_TYPE"] = dtd.ToString();
            string type = row["DATA_TYPE"].ToString();

            string token = tokenizer.NextToken();
            if (tokenizer.IsSize)
            {
                dtd.AppendFormat(CultureInfo.InvariantCulture, "({0})", token);
                if (type != "ENUM" && type != "SET")
                    ParseDataTypeSize(row, token);
                token = tokenizer.NextToken();
            }
            else
                dtd.Append(GetDataTypeDefaults(type, row));

            while (token != ")" && token != "," && String.Compare(token, "begin", true) != 0)
            {
                if (String.Compare(token, "CHARACTER", true) == 0 || String.Compare(token, "BINARY", true) == 0)
                {
                } // we don't need to do anything with this
                else if (String.Compare(token, "SET", true) == 0 || String.Compare(token, "CHARSET", true) == 0)
                    row["CHARACTER_SET_NAME"] = tokenizer.NextToken();
                else if (String.Compare(token, "ASCII", true) == 0)
                    row["CHARACTER_SET_NAME"] = "latin1";
                else if (String.Compare(token, "UNICODE", true) == 0)
                    row["CHARACTER_SET_NAME"] = "ucs2";
                else if (String.Compare(token, "COLLATE", true) == 0)
                    row["COLLATION_NAME"] = tokenizer.NextToken();
                else
                    dtd.AppendFormat(CultureInfo.InvariantCulture, " {0}", token);
                token = tokenizer.NextToken();
            }

            if (dtd.Length > 0)
                row["DTD_IDENTIFIER"] = dtd.ToString();

            // now default the collation if one wasn't given
            if (row["COLLATION_NAME"].ToString().Length == 0)
                row["COLLATION_NAME"] = CharSetMap.GetDefaultCollation(row["CHARACTER_SET_NAME"].ToString(), connection);

            // now set the octet length
            if (row["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            {
                row["CHARACTER_OCTET_LENGTH"] = CharSetMap.GetMaxLength(row["CHARACTER_SET_NAME"].ToString(), connection) *
                                                (int)row["CHARACTER_MAXIMUM_LENGTH"];
            }

            return token;
        }

        static void ParseDataTypeSize(DataRow row, string size)
        {
            size = size.Trim('(', ')');
            var parts = size.Split(',');

            if (!MetaData.IsNumericType(row["DATA_TYPE"].ToString()))
            {
                row["CHARACTER_MAXIMUM_LENGTH"] = Int32.Parse(parts[0]);
                // will set octet length in a minute
            }
            else
            {
                row["NUMERIC_PRECISION"] = Int32.Parse(parts[0]);
                if (parts.Length == 2)
                    row["NUMERIC_SCALE"] = Int32.Parse(parts[1]);
            }
        }

        void ParseProcedureBody(DataTable parametersTable, string body, DataRow row, string nameToRestrict)
        {
            var modes = new List<string>(new string[] { "IN", "OUT", "INOUT" });

            string sqlMode = row["SQL_MODE"].ToString();

            int pos = 1;
            SqlTokenizer tokenizer = new SqlTokenizer(body)
                                     {
                                         AnsiQuotes = (sqlMode.IndexOf("ANSI_QUOTES") != -1),
                                         BackslashEscapes = (sqlMode.IndexOf("NO_BACKSLASH_ESCAPES") == -1)
                                     };

            string token = tokenizer.NextToken();

            // this block will scan for the opening paren while also determining
            // if this routine is a function.  If so, then we need to add a
            // parameter row for the return parameter since it is ordinal position
            // 0 and should appear first.
            while (token != "(")
            {
                if (String.Compare(token, "FUNCTION", true) == 0 && nameToRestrict == null)
                {
                    parametersTable.Rows.Add(parametersTable.NewRow());
                    InitParameterRow(row, parametersTable.Rows[0]);
                }
                token = tokenizer.NextToken();
            }
            token = tokenizer.NextToken(); // now move to the next token past the (

            while (token != ")")
            {
                DataRow parmRow = parametersTable.NewRow();
                InitParameterRow(row, parmRow);
                parmRow["ORDINAL_POSITION"] = pos++;

                // handle mode and name for the parameter
                string mode = token.ToUpper(CultureInfo.InvariantCulture);
                if (!tokenizer.Quoted && modes.Contains(mode))
                {
                    parmRow["PARAMETER_MODE"] = mode;
                    token = tokenizer.NextToken();
                }
                parmRow["PARAMETER_NAME"] = token;

                // now parse data type
                token = ParseDataType(parmRow, tokenizer);
                if (token == ",")
                    token = tokenizer.NextToken();

                // now determine if we should include this row after all
                // we need to parse it before this check so we are correctly
                // positioned for the next parameter
                if (nameToRestrict == null || String.Compare(parmRow["PARAMETER_NAME"].ToString(), nameToRestrict, true) == 0)
                    parametersTable.Rows.Add(parmRow);
            }

            // now parse out the return parameter if there is one.
            token = tokenizer.NextToken().ToLower(CultureInfo.InvariantCulture);
            if (String.Compare(token, "returns", true) == 0)
            {
                DataRow parameterRow = parametersTable.Rows[0];
                parameterRow["PARAMETER_NAME"] = "RETURN_VALUE";
                ParseDataType(parameterRow, tokenizer);
            }
        }

        DataTable Query(string table_name, string initial_where, string[] keys, string[] values)
        {
            StringBuilder query = new StringBuilder("SELECT * FROM INFORMATION_SCHEMA.");
            query.Append(table_name);

            string where = GetWhereClause(initial_where, keys, values);

            if (where.Length > 0)
                query.AppendFormat(CultureInfo.InvariantCulture, " WHERE {0}", where);

            return GetTable(query.ToString());
        }
    }
}