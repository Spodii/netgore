using System;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class QueryResultFilterBase : IQueryResultFilter
    {
        readonly object _parent;
        readonly IQueryBuilderSettings _settings;

        string _limit;
        string _orderBy;
        OrderByType _orderByType;
        string _where;

        public object Parent { get { return _parent; } }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        protected QueryResultFilterBase(object parent, IQueryBuilderSettings settings)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (settings == null)
                throw new ArgumentNullException("settings");

            _parent = parent;
            _settings = settings;
        }

        protected string LimitValue { get { return _limit; } }

        protected string WhereValue { get { return _where; } }

        protected string OrderByValue { get { return _orderBy; } }

        protected OrderByType OrderByTypeValue { get { return _orderByType; } }

        #region IQueryResultFilter Members

        public IQueryResultFilter Limit(int amount)
        {
            _limit = amount.ToString();

            return this;
        }

        public IQueryResultFilter OrderBy(string value, OrderByType order)
        {
            _orderBy = value;
            _orderByType = order;

            return this;
        }

        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order)
        {
            _orderBy = Settings.EscapeColumn(columnName);
            _orderByType = order;

            return this;
        }

        public IQueryResultFilter Where(string condition)
        {
            _where = condition;

            return this;
        }

        #endregion
    }
}