using System;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    public class MySqlSelectQuery : SelectQueryBase
    {
        public MySqlSelectQuery(string table, string alias = null) : base(table, alias, MySqlQueryBuilderSettings.Instance)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Base operator
            sb.Append("SELECT ");

            if (DistinctValue)
                sb.Append("DISTINCT ");

            // Columns
            if (AllColumnsValue)
            {
                // All columns
                sb.Append("*");
            }
            else
            {
                // Specified columns only
                var values = ColumnCollection.GetValues();
                if (values == null || values.Length == 0)
                    throw InvalidQueryException.CreateEmptyColumnList();

                foreach (var v in values)
                {
                    sb.Append(Settings.EscapeColumn(v));
                    sb.Append(",");
                }

                sb.Length--;
            }

            // From table
            sb.Append(" FROM ");
            sb.Append(Settings.EscapeTable(Table));

            if (Alias != null)
            {
                sb.Append(" ");
                sb.Append(Alias);
            }

            // Joins
            foreach (var j in Joins)
            {
                sb.Append(" ");
                sb.Append(j);
            }

            return sb.ToString();
        }

        protected override IQueryResultFilter CreateQueryResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }
    }
}