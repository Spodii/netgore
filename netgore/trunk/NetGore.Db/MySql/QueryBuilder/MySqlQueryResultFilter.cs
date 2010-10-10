using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    public class MySqlQueryResultFilter : QueryResultFilterBase
    {

        public MySqlQueryResultFilter(object parent) : base(parent, MySqlQueryBuilderSettings.Instance)
        {
        }

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