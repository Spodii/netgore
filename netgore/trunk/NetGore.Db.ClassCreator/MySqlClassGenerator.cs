using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MySql.Data.MySqlClient;

namespace NetGore.Db.ClassCreator
{
    public class MySqlClassGenerator : DbClassGenerator
    {
        readonly MySqlConnection _conn;
        bool _disposed = false;

        public MySqlClassGenerator(string server, string userID, string password, string database)
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder
            { Server = server, UserID = userID, Password = password, Database = database };

            _conn = new MySqlConnection(sb.ToString());

            SetDbConnection(_conn);
        }

        public override void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_conn != null)
                _conn.Dispose();
        }

        static bool GetColumnInfoNull(string value)
        {
            value = value.ToUpper();
            if (value == "YES")
                return true;
            if (value == "NO")
                return false;
            throw new ArgumentException("Unexpected argument value.");
        }

        static DbColumnKeyType GetColumnKeyType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DbColumnKeyType.None;
            if (value.ToUpper() == "PRI")
                return DbColumnKeyType.Primary;
            return DbColumnKeyType.Foreign;
        }

        protected override IEnumerable<DbColumnInfo> GetColumns(string table)
        {
            var ret = new List<DbColumnInfo>();

            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SHOW FULL COLUMNS IN `" + table + "`";
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string name = r.GetString("Field");
                        string dbType = r.GetString("Type");
                        bool nullable = GetColumnInfoNull(r.GetString("Null"));
                        object defaultValue = r.GetValue(r.GetOrdinal("Default"));
                        DbColumnKeyType keyType = GetColumnKeyType(r.GetString("Key"));
                        string comment = r.GetString("Comment");

                        DbColumnInfo column = new DbColumnInfo(name, dbType, null, nullable, defaultValue, comment, keyType);
                        ret.Add(column);
                    }
                }
            }

            foreach (DbColumnInfo column in ret)
            {
                Type type = GetColumnType(table, column.Name);
                if (column.Nullable)
                    type = Type.GetType("System.Nullable`1[" + type.FullName + "]", true);

                if (type == null)
                    throw new Exception(string.Format("Failed to get the type for column `{0}` on table `{1}`.", column.Name,
                                                      table));

                column.Type = type;
            }

            return ret;
        }

        Type GetColumnType(string table, string column)
        {
            Type ret;

            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SELECT `" + column + "` FROM `" + table + "` WHERE 0=1";
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    Debug.Assert(r.FieldCount == 1);
                    ret = r.GetFieldType(0);
                }
            }

            return ret;
        }

        protected override IEnumerable<string> GetTables()
        {
            var ret = new List<string>();

            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SHOW TABLES";
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        ret.Add(r.GetString(0));
                    }
                }
            }

            return ret;
        }
    }
}