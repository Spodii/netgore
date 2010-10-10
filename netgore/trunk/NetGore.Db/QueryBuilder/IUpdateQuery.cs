using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IUpdateQuery : IColumnValueCollectionBuilder<IUpdateQuery>, IQueryResultFilter
    {
    }
}