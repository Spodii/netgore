using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an SQL UPDATE query.
    /// </summary>
    public interface IUpdateQuery : IColumnValueCollectionBuilder<IUpdateQuery>, IQueryResultFilter
    {
    }
}