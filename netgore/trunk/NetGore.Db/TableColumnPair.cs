using System.Linq;

namespace NetGore.Db
{
    public struct TableColumnPair
    {
        public readonly string Column;
        public readonly string Table;

        public TableColumnPair(string table, string column)
        {
            Table = table;
            Column = column;
        }
    }
}