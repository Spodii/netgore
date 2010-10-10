using System;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class MySqlQueryResultFilter : IQueryResultFilter
    {
        readonly object _parent;
        string _limit;

        string _orderBy;
        OrderByType _orderByType;
        string _where;

        public MySqlQueryResultFilter(object parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;
        }

        static IQueryBuilderSettings Settings
        {
            get { return MySqlQueryBuilderSettings.Instance; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(_parent);

            sb.Append(" ");

            if (!string.IsNullOrEmpty(_where))
            {
                sb.Append("WHERE ");
                sb.Append(_where);
                sb.Append(" ");
            }

            if (!string.IsNullOrEmpty(_orderBy))
            {
                sb.Append("ORDER BY ");
                sb.Append(_orderBy);

                switch (_orderByType)
                {
                    case OrderByType.Ascending:
                        break;

                    case OrderByType.Descending:
                        sb.Append( "DESC");
                        break;

                    default:
                        // TODO: errmsg
                        break;
                }

                sb.Append(" ");
            }

            if (!string.IsNullOrEmpty(_limit))
            {
                sb.Append("LIMIT ");
                sb.Append(_limit);
            }

            return sb.ToString().Trim();
        }

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