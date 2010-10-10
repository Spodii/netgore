using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class MySqlSelectQuery : ISelectQuery
    {
        readonly string _alias;
        readonly ColumnCollectionBuilder<ISelectQuery> _c;
        readonly string _table;
        readonly List<string> _joins = new List<string>();

        bool _allColumns = false;
        bool _distinct = false;

        public MySqlSelectQuery(string table, string alias = null)
        {
            _table = table;
            _alias = alias;

            _c = new ColumnCollectionBuilder<ISelectQuery>(this);
        }

        static IQueryBuilderSettings Settings
        {
            get { return MySqlQueryBuilderSettings.Instance; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            // Base operator
            sb.Append("SELECT ");

            if (_distinct)
                sb.Append("DISTINCT ");

            // Columns
            if (_allColumns)
            {
                // All columns
                sb.Append("*");
            }
            else
            {
                // Specified columns only
                var values = _c.GetValues();
                if (values == null || values.Length == 0)
                    throw InvalidQueryException.CreateEmptyColumnList();

                foreach (var v in values)
                {
                    sb.Append(Settings.EscapeColumn(v));
                    sb.Append(",");
                }

                sb.Length--;
            }

            // From table
            sb.Append(" FROM ");
            sb.Append(Settings.EscapeTable(_table));

            if (_alias != null)
            {
                sb.Append(" ");
                sb.Append(_alias);
            }

            // Joins
            foreach (var j in _joins)
            {
                sb.Append(" ");
                sb.Append(j);
            }

            return sb.ToString();
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

        public IQueryResultFilter Limit(int amount)
        {
            return new MySqlQueryResultFilter(this).Limit(amount);
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order)
        {
            return new MySqlQueryResultFilter(this).OrderBy(value, order);
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order)
        {
            return new MySqlQueryResultFilter(this).OrderByColumn(columnName, order);
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
            return new MySqlQueryResultFilter(this).Where(condition);
        }

        #endregion

        public IJoinedSelectQuery InnerJoin(string table, string alias, string joinCondition)
        {
            var sb = new StringBuilder();

            sb.Append("INNER JOIN ");
            sb.Append(Settings.EscapeTable(table));
            sb.Append(" ");
            sb.Append(alias);
            sb.Append(" ON ");
            sb.Append(joinCondition);

            _joins.Add(sb.ToString());

            return this;
        }

        public IJoinedSelectQuery InnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable, string otherTableJoinColumn)
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

            _joins.Add(sb.ToString());

            return this;
        }
    }
}