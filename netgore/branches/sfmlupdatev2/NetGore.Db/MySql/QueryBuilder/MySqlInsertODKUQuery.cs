using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IInsertODKUQuery"/> for MySql.
    /// </summary>
    class MySqlInsertODKUQuery : InsertODKUQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlInsertODKUQuery"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public MySqlInsertODKUQuery(IInsertQuery parent) : base(parent, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the column name and value pairs from the
        /// <see cref="InsertODKUQueryBase.Parent"/>.
        /// </summary>
        /// <returns>The column name and value pairs from the <see cref="InsertODKUQueryBase.Parent"/>.</returns>
        protected override KeyValuePair<string, string>[] GetColumnCollectionValuesFromInsert()
        {
            return ((InsertQueryBase)Parent).GetColumnValueCollectionValues();
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
            var sb = new StringBuilder();

            // Parent
            if (Parent != null)
                sb.Append(Parent);

            // Base operator
            sb.Append(" ON DUPLICATE KEY UPDATE ");

            // Sets
            var values = ColumnValueCollectionBuilder.GetValues();

            if (values == null || values.Length == 0)
                throw InvalidQueryException.CreateEmptyColumnList();

            foreach (var kvp in values)
            {
                sb.Append("`");
                sb.Append(kvp.Key);
                sb.Append("`=");
                sb.Append(kvp.Value);
                sb.Append(",");
            }

            sb.Length--;

            return sb.ToString().Trim();
        }
    }
}