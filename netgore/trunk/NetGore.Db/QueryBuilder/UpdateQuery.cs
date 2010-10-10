using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class UpdateQuery : IUpdateQuery
    {
        readonly ColumnValueCollectionBuilder<IUpdateQuery> _c;
        readonly string _table;

        public UpdateQuery(string table)
        {
            _table = table;

            _c = new ColumnValueCollectionBuilder<IUpdateQuery>(this);
        }

        public ColumnValueCollectionBuilder<IUpdateQuery> ColumnValueCollection
        {
            get { return _c; }
        }

        static IQueryBuilderSettings Settings
        {
            get { return QueryBuilderSettings.Instance; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(Settings.EscapeTable(_table));
            sb.Append(" SET");

            var values = _c.GetValues();

            // TODO: Require values not empty

            foreach (var kvp in values)
            {
                sb.Append(" ");
                sb.Append(Settings.EscapeColumn(kvp.Key));
                sb.Append("=");
                sb.Append(kvp.Value);
                sb.Append(",");
            }

            sb.Length--;

            return sb.ToString();
        }

        #region IUpdateQuery Members

        public IUpdateQuery Add(string column, string value)
        {
            return _c.Add(column, value);
        }

        public IUpdateQuery Add(KeyValuePair<string, string> columnAndValue)
        {
            return _c.Add(columnAndValue);
        }

        public IUpdateQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IUpdateQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IUpdateQuery AddAutoParam(string column)
        {
            return _c.AddAutoParam(column);
        }

        public IUpdateQuery AddAutoParam(params string[] columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IUpdateQuery AddAutoParam(IEnumerable<string> columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IUpdateQuery AddParam(string column, string value)
        {
            return _c.AddParam(column, value);
        }

        public IUpdateQuery AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return _c.AddParam(columnAndValue);
        }

        public IUpdateQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        public IUpdateQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

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

        public IUpdateQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        public IUpdateQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        public IUpdateQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        public IQueryResultFilter Where(string condition)
        {
            return new QueryResultFilter(this).Where(condition);
        }

        #endregion
    }
}