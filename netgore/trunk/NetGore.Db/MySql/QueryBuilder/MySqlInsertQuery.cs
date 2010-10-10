using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    public class MySqlInsertQuery : InsertQueryBase
    {
        public MySqlInsertQuery(string tableName) : base(tableName, MySqlQueryBuilderSettings.Instance)
        {
        }

        protected override IInsertODKUQuery CreateInsertODKUQuery(IInsertQuery parent)
        {
            return new MySqlInsertODKUQuery(parent);
        }

        public override string ToString()
        {
            var values = ColumnValueCollection.GetValues();

            if (values == null || values.Length == 0)
                throw InvalidQueryException.CreateEmptyColumnList();

            var sb = new StringBuilder();

            // Base function
            sb.Append("INSERT INTO ");
            sb.Append(Settings.EscapeTable(TableName));

            // Columns
            sb.Append(" (");

            foreach (var kvp in values)
            {
                sb.Append("`");
                sb.Append(kvp.Key);
                sb.Append("`,");
            }

            sb.Length--;

            // Values
            sb.Append(") VALUES (");

            foreach (var kvp in values)
            {
                sb.Append(kvp.Value);
                sb.Append(",");
            }

            sb.Length--;
            sb.Append(") ");

            return sb.ToString().Trim();
        }
    }
}