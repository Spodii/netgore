using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    class MySqlInsertODKUQuery : InsertODKUQueryBase
    {
        public MySqlInsertODKUQuery(IInsertQuery parent) : base(parent, MySqlQueryBuilderSettings.Instance)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Parent
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

        protected override KeyValuePair<string, string>[] GetColumnCollectionValuesFromInsert()
        {
            return ((MySqlInsertQuery)Parent).GetColumnValueCollectionValues();
        }
    }
}