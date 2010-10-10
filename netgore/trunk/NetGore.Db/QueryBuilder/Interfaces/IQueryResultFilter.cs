using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IQueryResultFilter
    {
        IQueryResultFilter Limit(int amount);

        IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending);

        IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending);

        IQueryResultFilter Where(string condition);
    }
}