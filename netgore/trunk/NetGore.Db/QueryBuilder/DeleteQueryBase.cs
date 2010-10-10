using NetGore.Db.QueryBuilder;

namespace NetGore.Db.QueryBuilder
{
    public abstract class DeleteQueryBase : IDeleteQuery
    {
        readonly string _table;
        readonly IQueryBuilderSettings _settings;

        public string Table { get { return _table; } }

        public IQueryBuilderSettings Settings { get { return _settings; } }

        protected DeleteQueryBase(string table, IQueryBuilderSettings settings)
        {
            _table = table;
            _settings = settings;
        }

        protected abstract IQueryResultFilter CreateResultFilter(object parent);

        #region IDeleteQuery Members

        public virtual IQueryResultFilter Limit(int amount)
        {
            return CreateResultFilter(this).Limit(amount);
        }

        public virtual IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return CreateResultFilter(this).OrderBy(value, order);
        }

        public virtual IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return CreateResultFilter(this).OrderByColumn(columnName, order);
        }

        public virtual IQueryResultFilter Where(string condition)
        {
            return CreateResultFilter(this).Where(condition);
        }

        #endregion
    }
}