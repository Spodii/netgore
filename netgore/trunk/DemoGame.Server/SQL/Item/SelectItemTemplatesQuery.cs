using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectItemTemplatesQuery : DbQueryReader<object>
    {
        const string _queryString = "SELECT * FROM `item_templates`";

        public SelectItemTemplatesQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<ItemTemplate> Execute()
        {
            List<ItemTemplate> ret = new List<ItemTemplate>();

            using (var r = ExecuteReader(null))
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
                    var values = new ItemTemplate(guid, name, description, type, graphic, value, width, height, stats);
                    ret.Add(values);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        protected override void SetParameters(DbParameterValues p, object item)
        {
            throw new MethodAccessException("Should not be accessed - no parameters");
        }
    }
}
