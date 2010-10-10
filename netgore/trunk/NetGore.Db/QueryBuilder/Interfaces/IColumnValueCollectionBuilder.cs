using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    public interface IColumnValueCollectionBuilder<out T>
    {
        T Add(string column, string value);

        T Add(KeyValuePair<string, string> columnAndValue);

        T Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues);

        T Add(params KeyValuePair<string, string>[] columnsAndValues);

        T AddAutoParam(string column);

        T AddAutoParam(params string[] columns);

        T AddAutoParam(IEnumerable<string> columns);

        T AddParam(string column, string value);

        T AddParam(KeyValuePair<string, string> columnAndValue);

        T AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues);

        T AddParam(params KeyValuePair<string, string>[] columnsAndValues);

        T Remove(IEnumerable<string> columns);

        T Remove(params string[] columns);

        T Remove(string column);
    }
}