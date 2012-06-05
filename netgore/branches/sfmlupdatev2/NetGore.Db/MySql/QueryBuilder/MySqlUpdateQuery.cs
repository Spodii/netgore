using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IUpdateQuery"/> for MySql.
    /// </summary>
    class MySqlUpdateQuery : UpdateQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlUpdateQuery"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is not a valid table name.</exception>
        public MySqlUpdateQuery(string table) : base(table, MySqlQueryBuilderSettings.Instance)
        {
        }

        protected override IQueryResultFilter CreateQueryResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <exception cref="InvalidQueryException">The generated query is invalid.</exception>
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
    }
}