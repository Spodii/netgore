using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Db;

// TODO: Make this a query to just select ONE ItemTemplate, then add a new query to get all ItemTemplateIDs

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectItemTemplatesQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}`", DBTables.ItemTemplate);

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
                    ItemTemplateID id = r.GetItemTemplateID("id");
                    string name = r.GetString("name");
                    string description = r.GetString("description");
                    GrhIndex graphic = r.GetGrhIndex("graphic");
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
                    ItemTemplate values = new ItemTemplate(id, name, description, type, graphic, value, width, height, stats);
                    ret.Add(values);
                }
            }

            return ret;
        }
    }
}