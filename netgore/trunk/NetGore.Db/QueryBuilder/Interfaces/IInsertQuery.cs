using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IInsertQuery : IColumnValueCollectionBuilder<IInsertQuery>
    {
        IInsertODKUQuery OnDuplicateKeyUpdate();
    }
}