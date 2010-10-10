using System.Collections.Generic;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class DeleteQuery : IDeleteQuery
    {
        readonly string _table;

        public DeleteQuery(string table)
        {
            _table = table;
        }

        static IQueryBuilderSettings Settings
        {
            get { return QueryBuilderSettings.Instance; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(Settings.EscapeTable(_table));

            return sb.ToString();
        }

        #region IDeleteQuery Members

        public IQueryResultFilter Limit(int amount)
        {
            return new QueryResultFilter(this).Limit(amount);
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return new QueryResultFilter(this).OrderBy(value, order);
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return new QueryResultFilter(this).OrderByColumn(columnName, order);
        }

        public IQueryResultFilter Where(string condition)
        {
            return new QueryResultFilter(this).Where(condition);
        }

        #endregion
    }
}