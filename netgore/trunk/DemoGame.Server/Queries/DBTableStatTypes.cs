using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public static class DBTableStatTypes
    {
        static readonly object _initLock = new object();

        static Dictionary<TableAndType, IEnumerable<StatType>> _dict = null;

        public static void Initialize(DBController controller)
        {
            lock (_initLock)
            {
                if (_dict != null)
                    return;

                _dict = new Dictionary<TableAndType, IEnumerable<StatType>>();
                AddTables(controller, DBTables.Character, DBTables.CharacterTemplate, DBTables.Item, DBTables.ItemTemplate);
            }
        }

        static void AddTables(DBController controller, params string[] tables)
        {
            StatCollectionType[] scts = new StatCollectionType[] { StatCollectionType.Base, StatCollectionType.Requirement };

            foreach (var table in tables)
            {
                var columns = controller.GetTableColumns(table);
                foreach (var statCollectionType in scts)
                {
                    var type = statCollectionType;
                    var statTypes = StatFactory.AllStats.Where(x => columns.Contains(x.GetDatabaseField(type), StringComparer.OrdinalIgnoreCase));
                    var v = new TableAndType(table, statCollectionType);
                    _dict.Add(v, statTypes.ToArray());
                }
            }
        }

        public static IEnumerable<StatType> GetTableStatTypes(string table, StatCollectionType statCollectionType)
        {
            var v = new TableAndType(table, statCollectionType);

            IEnumerable<StatType> ret;
            if (!_dict.TryGetValue(v, out ret))
                return Enumerable.Empty<StatType>();

            return ret;
        }

        struct TableAndType : IComparer<TableAndType>, IComparable<TableAndType>, IEquatable<TableAndType>
        {
            private readonly string Table;
            private readonly StatCollectionType StatCollectionType;

            public TableAndType(string table, StatCollectionType statCollectionType)
            {
                Table = table;
                StatCollectionType = statCollectionType;
            }

            public int Compare(TableAndType x, TableAndType y)
            {
                int i = StringComparer.OrdinalIgnoreCase.Compare(x.Table, y.Table);
                if (i != 0)
                    return i;

                return x.StatCollectionType.CompareTo(y.StatCollectionType);
            }

            public int CompareTo(TableAndType other)
            {
                return Compare(this, other);
            }

            public bool Equals(TableAndType other)
            {
                return string.Equals(Table, other.Table, StringComparison.OrdinalIgnoreCase) && StatCollectionType == other.StatCollectionType;
            }
        }
    }
}
