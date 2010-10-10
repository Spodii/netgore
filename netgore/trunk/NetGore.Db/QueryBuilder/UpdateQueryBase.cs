using System.Collections.Generic;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class UpdateQueryBase : IUpdateQuery
    {
        readonly string _table;
        readonly IQueryBuilderSettings _settings;
        readonly ColumnValueCollectionBuilder<IUpdateQuery> _c;

        protected ColumnValueCollectionBuilder<IUpdateQuery> ColumnValueCollection
        {
            get { return _c; }
        }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        public string Table { get { return _table; } }

        public KeyValuePair<string, string>[] GetColumnValueCollectionValues()
        {
            return ColumnValueCollection.GetValues();
        }

        protected UpdateQueryBase(string table, IQueryBuilderSettings settings)
        {
            _table = table;
            _settings = settings;

            _c = new ColumnValueCollectionBuilder<IUpdateQuery>(this, settings);
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
            return CreateQueryResultFilter(this).Limit(amount);
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return CreateQueryResultFilter(this).OrderBy(value, order);
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return CreateQueryResultFilter(this).OrderByColumn(columnName, order);
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
            return CreateQueryResultFilter(this).Where(condition);
        }

        #endregion

        protected abstract IQueryResultFilter CreateQueryResultFilter(object parent);
    }
}