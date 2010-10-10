using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    class MySqlInsertODKUQuery : IInsertODKUQuery
    {
        readonly ColumnValueCollectionBuilder<MySqlInsertODKUQuery> _c;
        readonly MySqlInsertQuery _owner;

        public MySqlInsertODKUQuery(MySqlInsertQuery owner)
        {
            _owner = owner;
            _c = new ColumnValueCollectionBuilder<MySqlInsertODKUQuery>(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Owner
            sb.Append(_owner);

            // Base operator
            sb.Append(" ON DUPLICATE KEY UPDATE ");

            // Sets
            var values = _c.GetValues();

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

        public IInsertODKUQuery AddFromInsert(string except)
        {
            Add(_owner.ColumnValueCollection.GetValues());

            if (except != null)
                Remove(except);

            return this;
        }

        public IInsertODKUQuery AddFromInsert(IEnumerable<string> except)
        {
            Add(_owner.ColumnValueCollection.GetValues());

            if (except != null)
                Remove(except);

            return this;
        }

        public IInsertODKUQuery AddFromInsert(params string[] except)
        {
            Add(_owner.ColumnValueCollection.GetValues());

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