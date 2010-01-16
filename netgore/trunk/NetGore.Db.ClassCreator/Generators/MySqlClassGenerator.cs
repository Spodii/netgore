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

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlClassGenerator"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="password">The password.</param>
        /// <param name="database">The database.</param>
        public MySqlClassGenerator(string server, string userID, string password, string database)
        {
            var sb = new MySqlConnectionStringBuilder
            { Server = server, UserID = userID, Password = password, Database = database };

            _conn = new MySqlConnection(sb.ToString());

            SetDbConnection(_conn);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="DbColumnInfo"/>s for the given
        /// <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The name of the table to get the <see cref="DbColumnInfo"/>s for.</param>
        /// <returns>
        /// The <see cref="DbColumnInfo"/>s for the given <paramref name="table"/>.
        /// </returns>
        protected override IEnumerable<DbColumnInfo> GetColumns(string table)
        {
            var ret = new List<DbColumnInfo>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SHOW FULL COLUMNS IN `" + table + "`";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var name = r.GetString("Field");
                        var dbType = r.GetString("Type");
                        var nullable = GetColumnInfoNull(r.GetString("Null"));
                        var defaultValue = r.GetValue(r.GetOrdinal("Default"));
                        var keyType = GetColumnKeyType(r.GetString("Key"));
                        var comment = r.GetString("Comment");

                        var column = new DbColumnInfo(name, dbType, null, nullable, defaultValue, comment, keyType);
                        ret.Add(column);
                    }
                }
            }

            foreach (var column in ret)
            {
                var type = GetColumnType(table, column.Name);
                if (column.IsNullable)
                {
                    if (type != typeof(string))
                        type = Type.GetType("System.Nullable`1[" + type.FullName + "]", true);
                }

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

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SELECT `" + column + "` FROM `" + table + "` WHERE 0=1";
                using (var r = cmd.ExecuteReader())
                {
                    Debug.Assert(r.FieldCount == 1);
                    ret = r.GetFieldType(0);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the tables in the database.
        /// </summary>
        /// <returns>The name of the tables in the database.</returns>
        protected override IEnumerable<string> GetTables()
        {
            var ret = new List<string>();

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SHOW TABLES";
                using (var r = cmd.ExecuteReader())
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