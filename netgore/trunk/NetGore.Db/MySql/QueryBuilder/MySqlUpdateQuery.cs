using System;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    public class MySqlUpdateQuery : UpdateQueryBase
    {
        public MySqlUpdateQuery(string tableName) : base(tableName, MySqlQueryBuilderSettings.Instance)
        {
        }

        public override string ToString()
        {
            var values = ColumnValueCollection.GetValues();

            if (values == null || values.Length == 0)
                throw InvalidQueryException.CreateEmptyColumnList();

            var sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(Settings.EscapeTable(Table));
            sb.Append(" SET ");

            foreach (var kvp in values)
            {
                sb.Append(Settings.EscapeColumn(kvp.Key));
                sb.Append("=");
                sb.Append(kvp.Value);
                sb.Append(",");
            }

            sb.Length--;

            return sb.ToString();
        }

        protected override IQueryResultFilter CreateQueryResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }
    }
}