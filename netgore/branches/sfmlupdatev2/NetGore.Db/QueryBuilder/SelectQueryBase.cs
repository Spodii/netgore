using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="ISelectQuery"/>.
    /// </summary>
    public abstract class SelectQueryBase : ISelectQuery
    {
        readonly string _alias;
        readonly ColumnCollectionBuilder<ISelectQuery> _c;
        readonly List<string> _joins = new List<string>();
        readonly IQueryBuilderSettings _settings;
        readonly string _table;

        bool _allColumns = false;
        bool _distinct = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryBase"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="alias">The alias for the <paramref name="table"/>.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is not a valid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is not a valid table alias.</exception>
        protected SelectQueryBase(string table, string alias, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _table = table;
            _alias = alias;
            _settings = settings;

            Settings.IsValidTableName(table, true);
            Settings.IsValidTableAlias(alias, true);

            _c = new ColumnCollectionBuilder<ISelectQuery>(this, Settings);
        }

        /// <summary>
        /// Gets the alias for the table name. When null, no alias is to be used.
        /// </summary>
        public string Alias
        {
            get { return _alias; }
        }

        /// <summary>
        /// Gets if all the column values should be selected.
        /// </summary>
        public bool AllColumnsValue
        {
            get { return _allColumns; }
        }

        /// <summary>
        /// Gets the <see cref="ColumnCollectionBuilder{T}"/> used by this object.
        /// </summary>
        protected ColumnCollectionBuilder<ISelectQuery> ColumnCollection
        {
            get { return _c; }
        }

        /// <summary>
        /// Gets if only distinct rows should be selected.
        /// </summary>
        public bool DistinctValue
        {
            get { return _distinct; }
        }

        /// <summary>
        /// Gets the list of joins to make.
        /// </summary>
        protected IEnumerable<string> Joins
        {
            get { return _joins.ToArray(); }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Creates the INNER JOIN query string.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="joinCondition">The raw SQL join condition.</param>
        /// <returns>The query string.</returns>
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

        /// <summary>
        /// Creates the INNER JOIN query string.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="thisJoinColumn">The name of the column on the <paramref name="table"/> that is being joined on.</param>
        /// <param name="otherTable">The name or alias of the other table to join.</param>
        /// <param name="otherTableJoinColumn">The name of the column on the <paramref name="otherTable"/> that is to be
        /// joined.</param>
        /// <returns>The query string.</returns>
        protected virtual string CreateInnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable,
                                                         string otherTableJoinColumn)
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

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IQueryResultFilter"/> instance.
        /// </summary>
        /// <param name="parent">The parent for the <see cref="IQueryResultFilter"/>.</param>
        /// <returns>The <see cref="IQueryResultFilter"/> instance.</returns>
        protected abstract IQueryResultFilter CreateQueryResultFilter(object parent);

        #region ISelectQuery Members

        /// <summary>
        /// Adds a column to the collection if it does not already exist.
        /// </summary>
        /// <param name="column">The name of the column to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public ISelectQuery Add(string column)
        {
            return _c.Add(column);
        }

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public ISelectQuery Add(IEnumerable<string> columns)
        {
            return _c.Add(columns);
        }

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public ISelectQuery Add(params string[] columns)
        {
            return _c.Add(columns);
        }

        /// <summary>
        /// Adds a function call to the collection.
        /// </summary>
        /// <param name="sql">The function's SQL.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        public ISelectQuery AddFunc(string sql)
        {
            return _c.AddFunc(sql);
        }

        /// <summary>
        /// Adds a function call to the collection.
        /// </summary>
        /// <param name="sql">The function's SQL.</param>
        /// <param name="alias">The alias to give the function call.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="alias"/> is not a valid column alias.</exception>
        public ISelectQuery AddFunc(string sql, string alias)
        {
            return _c.AddFunc(sql, alias);
        }

        /// <summary>
        /// Forces all columns to be selected.
        /// </summary>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        public ISelectQuery AllColumns()
        {
            _allColumns = true;

            return this;
        }

        /// <summary>
        /// Forces all columns to be selected.
        /// </summary>
        /// <param name="table">The table or table alias from which all columns will be selected.</param>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name and table alias.</exception>
        public ISelectQuery AllColumns(string table)
        {
            if (!Settings.IsValidTableName(table, false))
                Settings.IsValidTableAlias(table, true);

            return _c.Add(table + ".*");
        }

        /// <summary>
        /// Makes it so that only rows with distinct values are selected.
        /// </summary>
        /// <returns></returns>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        public ISelectQuery Distinct()
        {
            _distinct = true;

            return this;
        }

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="joinCondition">The raw SQL join condition.</param>
        /// <returns>The <see cref="IJoinedSelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="joinCondition"/> is null or invaild.</exception>
        public IJoinedSelectQuery InnerJoin(string table, string alias, string joinCondition)
        {
            if (string.IsNullOrEmpty(joinCondition))
                throw new ArgumentNullException("joinCondition");

            Settings.IsValidTableName(table, true);
            Settings.IsValidTableAlias(alias, true);

            var s = CreateInnerJoin(table, alias, joinCondition);

            _joins.Add(s);

            return this;
        }

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="thisJoinColumn">The name of the column on the <paramref name="table"/> that is being joined on.</param>
        /// <param name="otherTable">The name or alias of the other table to join.</param>
        /// <param name="otherTableJoinColumn">The name of the column on the <paramref name="otherTable"/> that is to be
        /// joined.</param>
        /// <returns>The <see cref="IJoinedSelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="thisJoinColumn"/> is an invalid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="otherTable"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="otherTableJoinColumn"/> is an invalid column name.</exception>
        public IJoinedSelectQuery InnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable,
                                                    string otherTableJoinColumn)
        {
            Settings.IsValidTableName(table, true);
            Settings.IsValidTableAlias(alias, true);
            Settings.IsValidColumnName(thisJoinColumn, true);
            Settings.IsValidColumnName(otherTableJoinColumn, true);

            if (!Settings.IsValidTableName(otherTable, false))
                Settings.IsValidTableAlias(otherTable, true);

            var s = CreateInnerJoinOnColumn(table, alias, thisJoinColumn, otherTable, otherTableJoinColumn);

            _joins.Add(s);

            return this;
        }

        /// <summary>
        /// Limits the number of rows being returned or operated on.
        /// </summary>
        /// <param name="amount">The row limit. Must be greater than 0.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is not greater than 0.</exception>
        public IQueryResultFilter Limit(int amount)
        {
            return CreateQueryResultFilter(this).Limit(amount);
        }

        /// <summary>
        /// Orders the rows in the query.
        /// </summary>
        /// <param name="value">The condition to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public IQueryResultFilter OrderBy(string value, OrderByType order)
        {
            return CreateQueryResultFilter(this).OrderBy(value, order);
        }

        /// <summary>
        /// Orders the rows in the query based on a single column.
        /// </summary>
        /// <param name="columnName">The name of the column to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is not a valid column name.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order)
        {
            return CreateQueryResultFilter(this).OrderByColumn(columnName, order);
        }

        /// <summary>
        /// Removes a column from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public ISelectQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public ISelectQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public ISelectQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Filters the rows being returned or operated on.
        /// </summary>
        /// <param name="condition">The raw SQL condition to use to filter the rows.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null or empty.</exception>
        public IQueryResultFilter Where(string condition)
        {
            return CreateQueryResultFilter(this).Where(condition);
        }

        #endregion
    }
}