using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class InsertQuery : IInsertQuery
    {
        readonly ColumnValueCollectionBuilder<IInsertQuery> _c;
        readonly string _tableName;

        public InsertQuery(string tableName)
        {
            _tableName = tableName;
            _c = new ColumnValueCollectionBuilder<IInsertQuery>(this);
        }

        public ColumnValueCollectionBuilder<IInsertQuery> ColumnValueCollection
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

            // Base function
            sb.Append("INSERT INTO ");
            sb.Append(Settings.EscapeTable(_tableName));
            sb.Append(" ");

            var values = _c.GetValues();

            // TODO: Require values not empty

            // Columns
            sb.Append("(");

            foreach (var kvp in values)
            {
                sb.Append("`");
                sb.Append(kvp.Key);
                sb.Append("`,");
            }

            sb.Length--;
            sb.Append(") ");

            // Values
            sb.Append("VALUES (");

            foreach (var kvp in values)
            {
                sb.Append(kvp.Value);
                sb.Append(",");
            }

            sb.Length--;
            sb.Append(") ");

            return sb.ToString().Trim();
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
            return new InsertODKUQuery(this);
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
    }
}