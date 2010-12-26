using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IInsertQuery"/> for MySql.
    /// </summary>
    class MySqlInsertQuery : InsertQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlInsertQuery"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        public MySqlInsertQuery(string table) : base(table, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IInsertODKUQuery"/> instance.
        /// </summary>
        /// <param name="parent">The <see cref="IInsertQuery"/> to use as the parent.</param>
        /// <returns>The <see cref="IInsertODKUQuery"/> instance.</returns>
        protected override IInsertODKUQuery CreateInsertODKUQuery(IInsertQuery parent)
        {
            return new MySqlInsertODKUQuery(parent);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <exception cref="InvalidQueryException">The query is invalid.</exception>
        public override string ToString()
        {
            var values = ColumnValueCollection.GetValues();

            if (values == null || values.Length == 0)
                throw InvalidQueryException.CreateEmptyColumnList();

            var sb = new StringBuilder();

            // Base function
            sb.Append("INSERT ");

            if (IgnoreExistsValue)
                sb.Append("IGNORE ");

            sb.Append("INTO ");
            sb.Append(Settings.EscapeTable(Table));

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