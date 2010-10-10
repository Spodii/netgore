using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IQueryBuilderSettings
    {
        StringComparer ColumnNameComparer { get; }

        string EscapeColumn(string columnName);

        string EscapeTable(string tableName);

        string Parameterize(string s);

        bool IsValidColumnName(string columnName, bool throwOnInvalid = false);

        bool IsValidTableName(string tableName, bool throwOnInvalid = false);
    }
}