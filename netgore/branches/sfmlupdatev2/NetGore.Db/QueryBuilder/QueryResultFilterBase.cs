using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IQueryResultFilter"/>.
    /// </summary>
    public abstract class QueryResultFilterBase : IQueryResultFilter
    {
        readonly List<KeyValuePair<string, OrderByType>> _orderBys = new List<KeyValuePair<string, OrderByType>>();
        readonly object _parent;
        readonly IQueryBuilderSettings _settings;

        string _limit;
        string _where;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResultFilterBase"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected QueryResultFilterBase(object parent, IQueryBuilderSettings settings)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (settings == null)
                throw new ArgumentNullException("settings");

            _parent = parent;
            _settings = settings;
        }

        /// <summary>
        /// Gets the LIMIT value, or null if none was entered.
        /// </summary>
        protected string LimitValue
        {
            get { return _limit; }
        }

        /// <summary>
        /// Gets the ORDER BY value, or null if none was entered.
        /// </summary>
        protected IEnumerable<KeyValuePair<string, OrderByType>> OrderByValues
        {
            get { return _orderBys.ToImmutable(); }
        }

        /// <summary>
        /// Gets the parent object that this <see cref="QueryResultFilterBase"/> was created from.
        /// </summary>
        public object Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the WHERE value, or null if none was entered.
        /// </summary>
        protected string WhereValue
        {
            get { return _where; }
        }

        #region IQueryResultFilter Members

        /// <summary>
        /// Limits the number of rows being returned or operated on.
        /// </summary>
        /// <param name="amount">The row limit. Must be greater than 0.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is not greater than 0.</exception>
        public IQueryResultFilter Limit(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("amount", "amount must be greater than 0.");

            _limit = amount.ToString();

            return this;
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
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            if (!EnumHelper<OrderByType>.IsDefined(order))
                throw new ArgumentException(string.Format("{0} is not a valid OrderByType value.", order), "order");

            _orderBys.Add(new KeyValuePair<string, OrderByType>(value, order));

            return this;
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
            if (!EnumHelper<OrderByType>.IsDefined(order))
                throw new ArgumentException(string.Format("{0} is not a valid OrderByType value.", order), "order");

            Settings.IsValidColumnName(columnName, true);

            _orderBys.Add(new KeyValuePair<string, OrderByType>(Settings.EscapeColumn(columnName), order));

            return this;
        }

        /// <summary>
        /// Filters the rows being returned or operated on.
        /// </summary>
        /// <param name="condition">The raw SQL condition to use to filter the rows.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null or empty.</exception>
        public IQueryResultFilter Where(string condition)
        {
            if (string.IsNullOrEmpty(condition))
                throw new ArgumentNullException("condition");

            _where = condition;

            return this;
        }

        #endregion
    }
}