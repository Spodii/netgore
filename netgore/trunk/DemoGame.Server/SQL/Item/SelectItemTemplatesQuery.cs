using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectItemTemplatesQuery : DbQueryReader
    {
        const string _queryString = "SELECT * FROM `item_templates`";

        public SelectItemTemplatesQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemTemplate> Execute()
        {
            var ret = new List<ItemTemplate>();

            using (IDataReader r = ExecuteReader())
            {
                while (r.Read())
                {
                    // Read the general stat values
                    ushort guid = r.GetUInt16("guid");
                    string name = r.GetString("name");
                    string description = r.GetString("description");
                    ushort graphic = r.GetUInt16("graphic");
                    int value = r.GetInt32("value");
                    byte width = r.GetByte("width");
                    byte height = r.GetByte("height");
                    ItemType type = r.GetItemType("type");

                    // Load the item's stats
                    ItemStats stats = new ItemStats();
                    foreach (StatType statType in ItemStats.DatabaseStats)
                    {
                        IStat stat = stats.GetStat(statType);
                        string dbColumn = statType.GetDatabaseField();
                        stat.Read(r, dbColumn);
                    }

                    // Create the template and enqueue it for returning
                    ItemTemplate values = new ItemTemplate(guid, name, description, type, graphic, value, width, height, stats);
                    ret.Add(values);
                }
            }

            return ret;
        }
    }
}