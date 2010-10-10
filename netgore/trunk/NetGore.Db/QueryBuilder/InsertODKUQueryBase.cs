using System.Collections.Generic;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class InsertODKUQueryBase : IInsertODKUQuery
    {
        readonly IInsertQuery _parent;
        readonly IQueryBuilderSettings _settings;
        readonly ColumnValueCollectionBuilder<IInsertODKUQuery> _c;

        public IInsertQuery Parent { get { return _parent; } }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        protected ColumnValueCollectionBuilder<IInsertODKUQuery> ColumnValueCollectionBuilder { get { return _c; } }

        protected InsertODKUQueryBase(IInsertQuery parent, IQueryBuilderSettings settings)
        {
            _parent = parent;
            _settings = settings;

            _c = new ColumnValueCollectionBuilder<IInsertODKUQuery>(this, settings);
        }

        #region IInsertODKUQuery Members

        public IInsertODKUQuery Add(string column, string value)
        {
            return _c.Add(column, value);
        }

        public IInsertODKUQuery Add(KeyValuePair<string, string> columnAndValue)
        {
            return _c.Add(columnAndValue);
        }

        public IInsertODKUQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IInsertODKUQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        public IInsertODKUQuery AddAutoParam(string column)
        {
            return _c.AddAutoParam(column);
        }

        public IInsertODKUQuery AddAutoParam(params string[] columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IInsertODKUQuery AddAutoParam(IEnumerable<string> columns)
        {
            return _c.AddAutoParam(columns);
        }

        public IInsertODKUQuery AddFromInsert()
        {
            return AddFromInsert((string[])null);
        }

        protected abstract KeyValuePair<string, string>[] GetColumnCollectionValuesFromInsert();

        public IInsertODKUQuery AddFromInsert(string except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        public IInsertODKUQuery AddFromInsert(IEnumerable<string> except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        public IInsertODKUQuery AddFromInsert(params string[] except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        public IInsertODKUQuery AddParam(string column, string value)
        {
            return _c.AddParam(column, value);
        }

        public IInsertODKUQuery AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return _c.AddParam(columnAndValue);
        }

        public IInsertODKUQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        public IInsertODKUQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        public IInsertODKUQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        public IInsertODKUQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        public IInsertODKUQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        #endregion
    }
}