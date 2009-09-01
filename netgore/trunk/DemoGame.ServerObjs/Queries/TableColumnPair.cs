using System.Linq;

namespace DemoGame.Server.Queries
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