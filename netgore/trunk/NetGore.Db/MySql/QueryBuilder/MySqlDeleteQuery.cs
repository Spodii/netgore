using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IDeleteQuery"/> for MySql.
    /// </summary>
    class MySqlDeleteQuery : DeleteQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDeleteQuery"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        public MySqlDeleteQuery(string table) : base(table, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IQueryResultFilter"/> instance.
        /// </summary>
        /// <param name="parent">The <see cref="IQueryResultFilter"/>'s parent.</param>
        /// <returns>The <see cref="IQueryResultFilter"/> instance.</returns>
        protected override IQueryResultFilter CreateResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(Settings.EscapeTable(Table));

            return sb.ToString();
        }
    }
}