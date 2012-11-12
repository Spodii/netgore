using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryResultFilter"/> for MySql.
    /// </summary>
    class MySqlQueryResultFilter : QueryResultFilterBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        /// <exception cref="InvalidQueryException">The generated query is invalid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "OrderByType")]
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

            var orderBys = OrderByValues;
            if (orderBys != null && !orderBys.IsEmpty())
            {
                sb.Append("ORDER BY ");

                foreach (var ob in orderBys)
                {
                    sb.Append(ob.Key);

                    switch (ob.Value)
                    {
                        case OrderByType.Ascending:
                            break;

                        case OrderByType.Descending:
                            sb.Append(" DESC");
                            break;

                        default:
                            const string errmsg = "Encountered undefined OrderByType `{0}`.";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ob.Value);
                            throw new InvalidQueryException(string.Format(errmsg, ob.Value));
                    }

                    sb.Append(", ");
                }

                sb.Length -= 2;

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