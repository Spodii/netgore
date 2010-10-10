using System.Collections.Generic;

namespace NetGore.Db.QueryBuilder
{
    public interface IColumnCollectionBuilder<out T>
    {
        T Add(string column);
        T Add(IEnumerable<string> columns);
        T Add(params string[] columns);

        T Remove(string column);
        T Remove(IEnumerable<string> columns);
        T Remove(params string[] columns);
    }
}