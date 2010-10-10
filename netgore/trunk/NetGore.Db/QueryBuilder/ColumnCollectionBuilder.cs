using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public class ColumnCollectionBuilder<T> : IColumnCollectionBuilder<T>
    {
        readonly IList<string> _columns;
        readonly T _owner;

        public ColumnCollectionBuilder(T owner)
        {
            // TODO: Owner not null

            _owner = owner;
            _columns = new List<string>();
        }

        public string[] GetValues()
        {
            return _columns.ToArray();
        }

        static IQueryBuilderSettings Settings
        {
            get { return MySqlQueryBuilderSettings.Instance; }
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