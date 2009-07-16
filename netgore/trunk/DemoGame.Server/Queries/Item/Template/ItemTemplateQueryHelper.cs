using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public static class ItemTemplateQueryHelper
    {
        public static IEnumerable<string> AllDBFields { get; private set; }
        public static IEnumerable<StatTypeField> BaseStatDBFields { get; private set; }
        public static IEnumerable<string> NonStatDBFields { get; private set; }
        public static IEnumerable<StatTypeField> ReqStatDBFields { get; private set; }

        static ItemTemplateQueryHelper()
        {
            BaseStatDBFields = StatsQueryHelper.GetStatTypeFields(DBTables.ItemTemplate, StatCollectionType.Base);
            ReqStatDBFields = StatsQueryHelper.GetStatTypeFields(DBTables.ItemTemplate, StatCollectionType.Requirement);

            NonStatDBFields = new string[] { "id", "name", "description", "graphic", "value", "width", "height", "type" };

            AllDBFields = (BaseStatDBFields.Concat(ReqStatDBFields)).Select(x => x.Field).Concat(NonStatDBFields).ToArray();
        }

        public static ItemTemplate ReadItemTemplate(IDataReader r)
        {
            // Read the general stat values
            ItemTemplateID id = r.GetItemTemplateID("id");
            string name = r.GetString("name");
            string description = r.GetString("description");
            GrhIndex graphic = r.GetGrhIndex("graphic");
            int value = r.GetInt32("value");
            byte width = r.GetByte("width");
            byte height = r.GetByte("height");
            ItemType type = r.GetItemType("type");

            // Stats
            var baseStats = StatsQueryHelper.ReadStatValues(r, BaseStatDBFields);
            var reqStats = StatsQueryHelper.ReadStatValues(r, ReqStatDBFields);

            // Create the template and enqueue it for returning
            ItemTemplate template = new ItemTemplate(id, name, description, type, graphic, value, width, height, baseStats,
                                                     reqStats);

            return template;
        }
    }
}