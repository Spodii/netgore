using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IInsertODKUQuery : IColumnValueCollectionBuilder<IInsertODKUQuery>
    {
        IInsertODKUQuery AddFromInsert();

        IInsertODKUQuery AddFromInsert(string except);

        IInsertODKUQuery AddFromInsert(IEnumerable<string> except);

        IInsertODKUQuery AddFromInsert(params string[] except);
    }
}