using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="ISelectQuery"/> for MySql.
    /// </summary>
    class MySqlSelectQuery : SelectQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryBase"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="alias">The alias for the <paramref name="table"/>.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is not a valid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is not a valid table alias.</exception>
        public MySqlSelectQuery(string table, string alias = null) : base(table, alias, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IQueryResultFilter"/> instance.
        /// </summary>
        /// <param name="parent">The parent for the <see cref="IQueryResultFilter"/>.</param>
        /// <returns>The <see cref="IQueryResultFilter"/> instance.</returns>
        protected override IQueryResultFilter CreateQueryResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        /// <exception cref="InvalidQueryException">The generated query is invalid.</exception>
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
                    sb.Append(v);
                    sb.Append(",");
                }

                sb.Length--;
            }

            // From table
            sb.Append(" FROM ");

            if (Alias != null)
                sb.Append(Settings.ApplyTableAlias(Settings.EscapeTable(Table), Alias));
            else
                sb.Append(Settings.EscapeTable(Table));

            // Joins
            foreach (var j in Joins)
            {
                sb.Append(" ");
                sb.Append(j);
            }

            return sb.ToString();
        }
    }
}