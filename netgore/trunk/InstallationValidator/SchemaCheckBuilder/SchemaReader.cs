using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NetGore;

namespace SchemaCheckBuilder
{
    public class SchemaReader
    {
        const string _columnsAlias = "c";

        const string _queryStr =
            "SELECT {0}" + " FROM COLUMNS AS {2}" + " LEFT JOIN TABLES AS t" +
            " ON {2}.TABLE_NAME = t.TABLE_NAME AND c.TABLE_SCHEMA = t.TABLE_SCHEMA" + " WHERE t.TABLE_SCHEMA = '{1}'";

        readonly DBConnectionSettings _dbSettings;
        readonly IEnumerable<TableSchema> _tableSchemas;
        MySqlConnection _conn;

        public IEnumerable<TableSchema> TableSchemas
        {
            get { return _tableSchemas; }
        }

        public SchemaReader(DBConnectionSettings dbSettings)
        {
            _dbSettings = dbSettings;

            OpenConnection();
            _tableSchemas = ExecuteQuery();
            CloseConnection();
        }

        void CloseConnection()
        {
            _conn.Close();
            _conn.Dispose();
        }

        IEnumerable<TableSchema> ExecuteQuery()
        {
            Dictionary<string, List<ColumnSchema>> tableColumns = new Dictionary<string, List<ColumnSchema>>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = GetQueryString();
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
            return ret;
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

        string GetQueryString()
        {
            return string.Format(_queryStr, GetColumnsString(), _dbSettings.Database, _columnsAlias);
        }

        void OpenConnection()
        {
            MySqlConnectionStringBuilder s = new MySqlConnectionStringBuilder
            { UserID = _dbSettings.User, Password = _dbSettings.Pass, Server = _dbSettings.Host, Database = "information_schema" };

            _conn = new MySqlConnection(s.ToString());
            _conn.Open();
        }
    }
}