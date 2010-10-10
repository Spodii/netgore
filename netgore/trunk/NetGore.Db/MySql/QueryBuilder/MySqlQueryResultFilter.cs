using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryResultFilter"/> for MySql.
    /// </summary>
    class MySqlQueryResultFilter : QueryResultFilterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlQueryResultFilter"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public MySqlQueryResultFilter(object parent) : base(parent, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Parent);

            sb.Append(" ");

            if (!string.IsNullOrEmpty(WhereValue))
            {
                sb.Append("WHERE ");
                sb.Append(WhereValue);
                sb.Append(" ");
            }

            if (!string.IsNullOrEmpty(OrderByValue))
            {
                sb.Append("ORDER BY ");
                sb.Append(OrderByValue);

                switch (OrderByTypeValue)
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

            if (!string.IsNullOrEmpty(LimitValue))
            {
                sb.Append("LIMIT ");
                sb.Append(LimitValue);
            }

            return sb.ToString().Trim();
        }
    }
}