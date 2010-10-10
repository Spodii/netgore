using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public class ColumnCollectionBuilder<T> : IColumnCollectionBuilder<T>
    {
        readonly IList<string> _columns;
        readonly T _owner;
        readonly IQueryBuilderSettings _settings;

        public ColumnCollectionBuilder(T owner, IQueryBuilderSettings settings)
        {
            // TODO: Owner not null

            _settings = settings;
            _owner = owner;
            _columns = new List<string>();
        }

        protected IQueryBuilderSettings Settings { get { return _settings; } }

        public string[] GetValues()
        {
            return _columns.ToArray();
        }

        public T Add(string column)
        {
            Settings.IsValidColumnName(column, true);

            if (!_columns.Contains(column, Settings.ColumnNameComparer))
                _columns.Add(column);

            return _owner;
        }

        public T Add(IEnumerable<string> columns)
        {
            foreach (var c in columns)
                Add(c);

            return _owner;
        }

        public T Add(params string[] columns)
        {
            return Add((IEnumerable<string>)columns);
        }

        public T Remove(string column)
        {
            Settings.IsValidColumnName(column, true);

            for (int i = 0; i < _columns.Count; i++)
            {
                if (Settings.ColumnNameComparer.Equals(column, _columns[i]))
                {
                    _columns.RemoveAt(i);
                    break;
                }
            }

            return _owner;
        }

        public T Remove(IEnumerable<string> columns)
        {
            foreach (var c in columns)
                Remove(c);

            return _owner;
        }

        public T Remove(params string[] columns)
        {
            return Remove((IEnumerable<string>)columns);
        }
    }
}