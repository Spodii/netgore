using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MySql.Data.MySqlClient;
using NetGore;

namespace SchemaCheckBuilder
{
    [Serializable]
    public class SchemaReader
    {
        const string _columnsAlias = "c";

        const string _queryStr =
            "SELECT {0}" + " FROM COLUMNS AS {2}" + " LEFT JOIN TABLES AS t" +
            " ON {2}.TABLE_NAME = t.TABLE_NAME AND c.TABLE_SCHEMA = t.TABLE_SCHEMA" + " WHERE t.TABLE_SCHEMA = '{1}'";

        readonly IEnumerable<TableSchema> _tableSchemas;

        public IEnumerable<TableSchema> TableSchemas
        {
            get { return _tableSchemas; }
        }

        public SchemaReader(DBConnectionSettings dbSettings)
        {
            var conn = OpenConnection(dbSettings);
            _tableSchemas = ExecuteQuery(conn, dbSettings);
            CloseConnection(conn);
        }

        static void CloseConnection(IDbConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }

        static IEnumerable<TableSchema> ExecuteQuery(MySqlConnection conn, DBConnectionSettings dbSettings)
        {
            Dictionary<string, List<ColumnSchema>> tableColumns = new Dictionary<string, List<ColumnSchema>>();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = GetQueryString(dbSettings);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var column = new ColumnSchema(r);

                        List<ColumnSchema> columnList;
                        if (!tableColumns.TryGetValue(column.TableName, out columnList))
                        {
                            columnList = new List<ColumnSchema>();
                            tableColumns.Add(column.TableName, columnList);
                        }

                        columnList.Add(column);
                    }
                }
            }

            var ret = tableColumns.Select(x => new TableSchema(x.Key, x.Value));

            // ToArray() required to serialize, otherwise it is a LINQ statement
            return ret.ToArray(); 
        }

        static string GetColumnsString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in ColumnSchema.ValueNames)
            {
                sb.Append(_columnsAlias);
                sb.Append(".");
                sb.Append(c);
                sb.Append(", ");
            }
            sb.Length -= 2;

            return sb.ToString();
        }

        static string GetQueryString(DBConnectionSettings dbSettings)
        {
            return string.Format(_queryStr, GetColumnsString(), dbSettings.Database, _columnsAlias);
        }

        static MySqlConnection OpenConnection(DBConnectionSettings dbSettings)
        {
            MySqlConnectionStringBuilder s = new MySqlConnectionStringBuilder { UserID = dbSettings.User, Password = dbSettings.Pass, Server = dbSettings.Host, Database = "information_schema" };

            var conn = new MySqlConnection(s.ToString());
            conn.Open();

            return conn;
        }

        public void Serialize(string filePath)
        {
            using (var s = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter f = GetBinaryFormatter();
                f.Serialize(s, this);
            }
        }

        static BinaryFormatter GetBinaryFormatter()
        {
            return new BinaryFormatter();
        }

        public static SchemaReader Deserialize(string filePath)
        {
            SchemaReader ret;

            using (var s = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter f = GetBinaryFormatter();
                ret = (SchemaReader)f.Deserialize(s);
            }

            return ret;
        }
    }
}