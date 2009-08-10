using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;

namespace DemoGame.Server
{
    public static class DBTableStatTypes
    {
        static readonly object _initLock = new object();

        static Dictionary<TableAndType, IEnumerable<StatType>> _dict = null;

        static void AddTables(DBController controller, params string[] tables)
        {
            var scts = new StatCollectionType[] { StatCollectionType.Base, StatCollectionType.Requirement };

            foreach (string table in tables)
            {
                var columns = controller.GetTableColumns(table);
                foreach (StatCollectionType statCollectionType in scts)
                {
                    StatCollectionType type = statCollectionType;
                    var statTypes =
                        StatFactory.AllStats.Where(
                            x => columns.Contains(x.GetDatabaseField(type), StringComparer.OrdinalIgnoreCase));
                    TableAndType v = new TableAndType(table, statCollectionType);
                    _dict.Add(v, statTypes.ToArray());
                }
            }
        }

        public static IEnumerable<StatType> GetTableStatTypes(string table, StatCollectionType statCollectionType)
        {
            TableAndType v = new TableAndType(table, statCollectionType);

            IEnumerable<StatType> ret;
            if (!_dict.TryGetValue(v, out ret))
                return Enumerable.Empty<StatType>();

            return ret;
        }

        public static void Initialize(DBController controller)
        {
            lock (_initLock)
            {
                if (_dict != null)
                    return;

                _dict = new Dictionary<TableAndType, IEnumerable<StatType>>();
                AddTables(controller, CharacterTable.TableName, CharacterTemplateTable.TableName, ItemTable.TableName,
                          ItemTemplateTable.TableName);
            }
        }

        struct TableAndType : IComparer<TableAndType>, IComparable<TableAndType>, IEquatable<TableAndType>
        {
            readonly StatCollectionType StatCollectionType;
            readonly string Table;

            public TableAndType(string table, StatCollectionType statCollectionType)
            {
                Table = table;
                StatCollectionType = statCollectionType;
            }

            #region IComparable<TableAndType> Members

            public int CompareTo(TableAndType other)
            {
                return Compare(this, other);
            }

            #endregion

            #region IComparer<TableAndType> Members

            public int Compare(TableAndType x, TableAndType y)
            {
                int i = StringComparer.OrdinalIgnoreCase.Compare(x.Table, y.Table);
                if (i != 0)
                    return i;

                return x.StatCollectionType.CompareTo(y.StatCollectionType);
            }

            #endregion

            #region IEquatable<TableAndType> Members

            public bool Equals(TableAndType other)
            {
                return string.Equals(Table, other.Table, StringComparison.OrdinalIgnoreCase) &&
                       StatCollectionType == other.StatCollectionType;
            }

            #endregion
        }
    }
}