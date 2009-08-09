using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

// TODO: !! Try to make obsolete

namespace DemoGame.Server.Queries
{
    public static class ItemQueryHelper
    {
        public static IEnumerable<string> AllDBFields { get; private set; }
        public static IEnumerable<StatTypeField> BaseDBStatFields { get; private set; }
        public static IEnumerable<string> NonStatFields { get; private set; }
        public static IEnumerable<StatTypeField> ReqDBStatFields { get; private set; }

        static ItemQueryHelper()
        {
            BaseDBStatFields = StatsQueryHelper.GetStatTypeFields(DBTables.Item, StatCollectionType.Base);
            ReqDBStatFields = StatsQueryHelper.GetStatTypeFields(DBTables.Item, StatCollectionType.Requirement);

            NonStatFields = new string[]
                            { "amount", "description", "graphic", "id", "height", "name", "type", "value", "width", "hp", "mp" };

            AllDBFields = (BaseDBStatFields.Concat(ReqDBStatFields)).Select(x => x.Field).Concat(NonStatFields).ToArray();
        }

        public static ItemValues ReadItemValues(IDataReader r)
        {
            // Stats
            var baseStats = StatsQueryHelper.ReadStatValues(r, BaseDBStatFields);
            var reqStats = StatsQueryHelper.ReadStatValues(r, ReqDBStatFields);

            // General
            ItemID id = r.GetItemID("id");
            byte width = r.GetByte("width");
            byte height = r.GetByte("height");
            string name = r.GetString("name");
            string description = r.GetString("description");
            GrhIndex graphicIndex = r.GetGrhIndex("graphic");
            byte amount = r.GetByte("amount");
            int value = r.GetInt32("value");
            ItemType type = r.GetItemType("type");
            SPValueType hp = r.GetSPValueType("hp");
            SPValueType mp = r.GetSPValueType("mp");

            return new ItemValues(id, width, height, name, description, type, graphicIndex, amount, value, hp, mp, baseStats,
                                  reqStats);
        }
    }
}