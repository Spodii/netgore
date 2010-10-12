using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an SQL SELECT query that selects from a function.
    /// </summary>
    public interface ISelectFunctionQuery : IValueCollectionBuilder<ISelectFunctionQuery>
    {
    }
}