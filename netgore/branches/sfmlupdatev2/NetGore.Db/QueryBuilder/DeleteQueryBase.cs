using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IDeleteQuery"/>.
    /// </summary>
    public abstract class DeleteQueryBase : IDeleteQuery
    {
        readonly IQueryBuilderSettings _settings;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteQueryBase"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected DeleteQueryBase(string table, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _table = table;
            _settings = settings;

            Settings.IsValidTableName(table, true);
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the name of the table being operated on.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IQueryResultFilter"/> instance.
        /// </summary>
        /// <param name="parent">The <see cref="IQueryResultFilter"/>'s parent.</param>
        /// <returns>The <see cref="IQueryResultFilter"/> instance.</returns>
        protected abstract IQueryResultFilter CreateResultFilter(object parent);

        #region IDeleteQuery Members

        /// <summary>
        /// Limits the number of rows being returned or operated on.
        /// </summary>
        /// <param name="amount">The row limit. Must be greater than 0.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is not greater than 0.</exception>
        public virtual IQueryResultFilter Limit(int amount)
        {
            return CreateResultFilter(this).Limit(amount);
        }

        /// <summary>
        /// Orders the rows in the query.
        /// </summary>
        /// <param name="value">The condition to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public virtual IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return CreateResultFilter(this).OrderBy(value, order);
        }

        /// <summary>
        /// Orders the rows in the query based on a single column.
        /// </summary>
        /// <param name="columnName">The name of the column to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is not a valid column name.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public virtual IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return CreateResultFilter(this).OrderByColumn(columnName, order);
        }

        /// <summary>
        /// Filters the rows being returned or operated on.
        /// </summary>
        /// <param name="condition">The raw SQL condition to use to filter the rows.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null or empty.</exception>
        public virtual IQueryResultFilter Where(string condition)
        {
            return CreateResultFilter(this).Where(condition);
        }

        #endregion
    }
}