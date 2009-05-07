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
                    // HACK: Remove r.GetOrdinal()s
                    ushort guid = (ushort)r.GetInt16(r.GetOrdinal("guid")); // HACK: Make r.GetUShort()
                    string name = r.GetString(r.GetOrdinal("name"));
                    string description = r.GetString(r.GetOrdinal("description"));
                    ushort graphic = (ushort)r.GetInt16(r.GetOrdinal("graphic"));
                    int value = r.GetInt32(r.GetOrdinal("value"));
                    byte width = r.GetByte(r.GetOrdinal("width"));
                    byte height = r.GetByte(r.GetOrdinal("height"));
                    ItemType type = (ItemType)r.GetByte(r.GetOrdinal("type")); // HACK: Make r.GetItemType()

                    // HACK: Make r.GetItemStats()
                    ItemStats stats = new ItemStats();
                    foreach (StatType statType in ItemStats.DatabaseStats)
                    {
                        string dbColumn = statType.GetDatabaseField();
                        int ordinal = r.GetOrdinal(dbColumn);
                        IStat stat = stats.GetStat(statType);
                        stat.Read(r, ordinal); // HACK: Allow specifying field name
                    }

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
