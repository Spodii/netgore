using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    public struct TableColumnPair
    {
        public readonly string Table;
        public readonly string Column;

        public TableColumnPair(string table, string column)
        {
            Table = table;
            Column = column;
        }
    }
}
