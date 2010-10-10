using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public class MySqlQueryBuilderSettings : IQueryBuilderSettings
    {
        static readonly MySqlQueryBuilderSettings _instance;

        static MySqlQueryBuilderSettings()
        {
            _instance = new MySqlQueryBuilderSettings();
        }

        MySqlQueryBuilderSettings()
        {
        }

        public static IQueryBuilderSettings Instance
        {
            get { return _instance; }
        }

        public bool IsValidColumnName(string columnName, bool throwOnInvalid = false)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new InvalidQueryException("The column name cannot be empty.");

            if (columnName.Contains(' '))
                throw new InvalidQueryException("The column name may not contain a space.");

            return true;
        }

        public bool IsValidTableName(string tableName, bool throwOnInvalid = false)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table name cannot be empty.");
                else
                    return false;
            }

            if (tableName.Contains(' '))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table name may not contain a space.");
                else
                    return false;
            }

            return true;
        }

        #region IQueryBuilderSettings Members

        public StringComparer ColumnNameComparer
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }

        public string EscapeColumn(string columnName)
        {
            if (!columnName.Contains("."))
                return "`" + columnName + "`";
            else
                return columnName;
        }

        public string EscapeTable(string tableName)
        {
            if (!tableName.Contains("."))
                return "`" + tableName + "`";
            else
                return tableName;
        }

        public string Parameterize(string s)
        {
            if (!s.StartsWith("@"))
                return "@" + s;
            else
            {
                // TODO: errmsg - already escaped
                return s;
            }
        }

        #endregion
    }
}