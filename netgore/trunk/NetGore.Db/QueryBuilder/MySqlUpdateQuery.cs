using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class MySqlUpdateQuery : IUpdateQuery
    {
        readonly ColumnValueCollectionBuilder<IUpdateQuery> _c;
        readonly string _table;

        public MySqlUpdateQuery(string table)
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
            get { return MySqlQueryBuilderSettings.Instance; }
        }

        public override string ToString()
        {
            var values = _c.GetValues();

            if (values == null || values.Length == 0)
                throw InvalidQueryException.CreateEmptyColumnList();

            var sb = new StringBuilder();

            sb.Append("UPDATE ");
            sb.Append(Settings.EscapeTable(_table));
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
            return new MySqlQueryResultFilter(this).Limit(amount);
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return new MySqlQueryResultFilter(this).OrderBy(value, order);
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return new MySqlQueryResultFilter(this).OrderByColumn(columnName, order);
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
            return new MySqlQueryResultFilter(this).Where(condition);
        }

        #endregion
    }
}