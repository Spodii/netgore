using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.Db;
using NetGore.IO;

namespace NetGore.Db.Schema
{
    /// <summary>
    /// Reads the schema for a database.
    /// </summary>
    [Serializable]
    public class SchemaReader
    {
        const string _columnsAlias = "c";

        const string _queryStr =
            "SELECT {0}" + " FROM COLUMNS AS {2}" + " LEFT JOIN TABLES AS t" +
            " ON {2}.TABLE_NAME = t.TABLE_NAME AND c.TABLE_SCHEMA = t.TABLE_SCHEMA" + " WHERE t.TABLE_SCHEMA = '{1}'";

        const string _rootNodeName = "DbSchema";
        const string _tablesNodeName = "Tables";

        readonly IEnumerable<TableSchema> _tableSchemas;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaReader"/> class.
        /// </summary>
        /// <param name="dbSettings">The <see cref="DbConnectionSettings"/> for connecting to the database to read
        /// the schema of.</param>
        public SchemaReader(DbConnectionSettings dbSettings)
        {
            var conn = OpenConnection(dbSettings);
            _tableSchemas = ExecuteQuery(conn, dbSettings);
            CloseConnection(conn);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public SchemaReader(IValueReader reader)
        {
            _tableSchemas = reader.ReadManyNodes(_tablesNodeName, r => new TableSchema(r)).ToCompact();
        }

        /// <summary>
        /// Gets the schema for the database tables.
        /// </summary>
        public IEnumerable<TableSchema> TableSchemas
        {
            get { return _tableSchemas; }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="conn">The conn.</param>
        static void CloseConnection(IDbConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }

        static IEnumerable<TableSchema> ExecuteQuery(MySqlConnection conn, DbConnectionSettings dbSettings)
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

        static string GetQueryString(DbConnectionSettings dbSettings)
        {
            return string.Format(_queryStr, GetColumnsString(), dbSettings.Database, _columnsAlias);
        }

        /// <summary>
        /// Loads a <see cref="SchemaReader"/> from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded <see cref="SchemaReader"/></returns>
        public static SchemaReader Load(string filePath)
        {
            var reader = new XmlValueReader(filePath, _rootNodeName);
            return new SchemaReader(reader);
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="dbSettings">The db settings.</param>
        /// <returns>The <see cref="MySqlConnection"/>.</returns>
        static MySqlConnection OpenConnection(DbConnectionSettings dbSettings)
        {
            MySqlConnectionStringBuilder s = new MySqlConnectionStringBuilder
            { UserID = dbSettings.User, Password = dbSettings.Pass, Server = dbSettings.Host, Database = "information_schema" };

            var conn = new MySqlConnection(s.ToString());
            conn.Open();

            return conn;
        }

        /// <summary>
        /// Saves the values to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            var tables = _tableSchemas.ToArray();

            using (var writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                writer.WriteManyNodes(_tablesNodeName, tables, (w, table) => table.Write(w));
            }
        }
    }
}