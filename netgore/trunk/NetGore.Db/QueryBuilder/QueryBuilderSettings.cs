using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public class QueryBuilderSettings : IQueryBuilderSettings
    {
        static readonly QueryBuilderSettings _instance;

        static QueryBuilderSettings()
        {
            _instance = new QueryBuilderSettings();
        }

        QueryBuilderSettings()
        {
        }

        public static IQueryBuilderSettings Instance
        {
            get { return _instance; }
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