using System.Collections.Generic;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class SelectQueryBase : ISelectQuery
    {
        readonly string _alias;
        readonly ColumnCollectionBuilder<ISelectQuery> _c;
        readonly string _table;
        readonly List<string> _joins = new List<string>();
        readonly IQueryBuilderSettings _settings;

        bool _allColumns = false;
        bool _distinct = false;

        protected ColumnCollectionBuilder<ISelectQuery> ColumnCollection { get { return _c; } }

        public string Table { get { return _table; } }

        public string Alias { get { return _alias; } }

        public bool AllColumnsValue { get { return _allColumns; } }

        public bool DistinctValue { get { return _distinct; } }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        protected IEnumerable<string> Joins { get { return _joins.ToArray(); } }

        protected SelectQueryBase(string table, string alias, IQueryBuilderSettings settings)
        {
            _table = table;
            _alias = alias;
            _settings = settings;

            _c = new ColumnCollectionBuilder<ISelectQuery>(this, Settings);
        }

        #region ISelectQuery Members

        public ISelectQuery Add(string column)
        {
            return _c.Add(column);
        }

        public ISelectQuery Add(IEnumerable<string> columns)
        {
            return _c.Add(columns);
        }

        public ISelectQuery Add(params string[] columns)
        {
            return _c.Add(columns);
        }

        public ISelectQuery AllColumns()
        {
            _allColumns = true;

            return this;
        }

        public ISelectQuery Distinct()
        {
            _distinct = true;

            return this;
        }

        protected abstract IQueryResultFilter CreateQueryResultFilter(object parent);

        public IQueryResultFilter Limit(int amount)
        {
            return CreateQueryResultFilter(this).Limit(amount);
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order)
        {
            return CreateQueryResultFilter(this).OrderBy(value, order);
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order)
        {
            return CreateQueryResultFilter(this).OrderByColumn(columnName, order);
        }

        public ISelectQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        public ISelectQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        public ISelectQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        public IQueryResultFilter Where(string condition)
        {
            return CreateQueryResultFilter(this).Where(condition);
        }

        #endregion

        protected virtual string CreateInnerJoin(string table, string alias, string joinCondition)
        {
            var sb = new StringBuilder();

            sb.Append("INNER JOIN ");
            sb.Append(Settings.EscapeTable(table));
            sb.Append(" ");
            sb.Append(alias);
            sb.Append(" ON ");
            sb.Append(joinCondition);

            return sb.ToString();
        }

        public IJoinedSelectQuery InnerJoin(string table, string alias, string joinCondition)
        {
            var s = CreateInnerJoin(table, alias, joinCondition);

            _joins.Add(s);

            return this;
        }

        protected virtual string CreateInnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable, string otherTableJoinColumn)
        {
            var sb = new StringBuilder();

            sb.Append("INNER JOIN ");
            sb.Append(Settings.EscapeTable(table));
            sb.Append(" ");
            sb.Append(alias);
            sb.Append(" ON ");
            sb.Append(alias);
            sb.Append(".");
            sb.Append(thisJoinColumn);
            sb.Append("=");
            sb.Append(otherTable);
            sb.Append(".");
            sb.Append(otherTableJoinColumn);

            return sb.ToString();
        }

        public IJoinedSelectQuery InnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable, string otherTableJoinColumn)
        {
            var s = CreateInnerJoinOnColumn(table, alias, thisJoinColumn, otherTable, otherTableJoinColumn);

            _joins.Add(s);

            return this;
        }
    }
}