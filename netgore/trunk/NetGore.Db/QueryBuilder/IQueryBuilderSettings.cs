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
    }
}