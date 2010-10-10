using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public class ColumnValueCollectionBuilder<T> : IColumnValueCollectionBuilder<T>
    {
        readonly IDictionary<string, string> _cvs;
        readonly T _owner;

        public ColumnValueCollectionBuilder(T owner)
        {
            // TODO: Owner not null

            _owner = owner;
            _cvs = new SortedDictionary<string, string>(QueryBuilderSettings.Instance.ColumnNameComparer);
        }

        static IQueryBuilderSettings Settings
        {
            get { return QueryBuilderSettings.Instance; }
        }

        public KeyValuePair<string, string>[] GetValues()
        {
            return _cvs.ToArray();
        }

        #region IColumnValueCollectionBuilder<T> Members

        public T Add(string column, string value)
        {
            _cvs.Remove(column);
            _cvs.Add(column, value);

            return _owner;
        }

        public T Add(KeyValuePair<string, string> columnAndValue)
        {
            return Add(columnAndValue.Key, columnAndValue.Value);
        }

        public T Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            if (columnsAndValues != null)
            {
                foreach (var c in columnsAndValues)
                {
                    Add(c);
                }
            }

            return _owner;
        }

        public T Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return Add((IEnumerable<KeyValuePair<string, string>>)columnsAndValues);
        }

        public T AddAutoParam(string column)
        {
            return AddParam(column, Settings.Parameterize(column));
        }

        public T AddAutoParam(params string[] columns)
        {
            return AddAutoParam((IEnumerable<string>)columns);
        }

        public T AddAutoParam(IEnumerable<string> columns)
        {
            if (columns != null)
            {
                foreach (var c in columns)
                {
                    AddAutoParam(c);
                }
            }

            return _owner;
        }

        public T AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return AddParam((IEnumerable<KeyValuePair<string, string>>)columnsAndValues);
        }

        public T AddParam(string column, string value)
        {
            _cvs.Remove(column);
            _cvs.Add(column, Settings.Parameterize(value));

            return _owner;
        }

        public T AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return AddParam(columnAndValue.Key, columnAndValue.Value);
        }

        public T AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            if (columnsAndValues != null)
            {
                foreach (var c in columnsAndValues)
                {
                    Add(c);
                }
            }

            return _owner;
        }

        public T Remove(IEnumerable<string> columns)
        {
            if (columns != null)
            {
                foreach (var c in columns)
                {
                    Remove(c);
                }
            }

            return _owner;
        }

        public T Remove(params string[] columns)
        {
            return Remove((IEnumerable<string>)columns);
        }

        public T Remove(string column)
        {
            _cvs.Remove(column);

            return _owner;
        }

        #endregion
    }
}