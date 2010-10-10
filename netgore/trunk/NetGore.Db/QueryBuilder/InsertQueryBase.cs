using System.Collections.Generic;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class InsertQueryBase : IInsertQuery
    {
        readonly string _tableName;
        readonly IQueryBuilderSettings _settings;
        readonly ColumnValueCollectionBuilder<IInsertQuery> _c;

        protected ColumnValueCollectionBuilder<IInsertQuery> ColumnValueCollection
        {
            get { return _c; }
        }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        public string TableName { get { return _tableName; } }

        public KeyValuePair<string, string>[] GetColumnValueCollectionValues()
        {
            return ColumnValueCollection.GetValues();
        }

        protected InsertQueryBase(string tableName, IQueryBuilderSettings settings)
        {
            _tableName = tableName;
            _settings = settings;

            _c = new ColumnValueCollectionBuilder<IInsertQuery>(this, settings);
        }

        #region IInsertQuery Members

        public IInsertQuery Add(string column, string value)
        {
            return _c.Add(column, value);
        }

        public IInsertQuery Add(KeyValuePair<string, string> columnAndValue)
        {
            return _c.Add(columnAndValue);
        }

        public IInsertQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IInsertQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IInsertQuery AddAutoParam(string column)
        {
            return _c.AddAutoParam(column);
        }

        public IInsertQuery AddAutoParam(params string[] columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IInsertQuery AddAutoParam(IEnumerable<string> columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IInsertQuery AddParam(string column, string value)
        {
            return _c.AddParam(column, value);
        }

        public IInsertQuery AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return _c.AddParam(columnAndValue);
        }

        public IInsertQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        public IInsertQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        public IInsertODKUQuery OnDuplicateKeyUpdate()
        {
            return CreateInsertODKUQuery(this);
        }

        public IInsertQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        public IInsertQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        public IInsertQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        #endregion

        protected abstract IInsertODKUQuery CreateInsertODKUQuery(IInsertQuery parent);
    }
}