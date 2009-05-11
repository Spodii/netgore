using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetGore.Db;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    public abstract class SelectItemQueryBase<T> : DbQueryReader<T>
    {
        protected SelectItemQueryBase(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        protected static ItemValues GetItemValues(IDataReader r)
        {
            // Stats
            ItemStats stats = new ItemStats();
            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                IStat stat = stats.GetStat(statType);
                stat.Read(r, r.GetOrdinal(statType.GetDatabaseField()));
            }

            // General
            int guid = r.GetInt32("guid");
            byte width = r.GetByte("width");
            byte height = r.GetByte("height");
            string name = r.GetString("name");
            string description = r.GetString("description");
            ushort graphicIndex = r.GetUInt16("graphic");
            byte amount = r.GetByte("amount");
            int value = r.GetInt32("value");
            ItemType type = r.GetItemType("type");

            // FUTURE: Recover from this error by just not creating an item
            if (!type.IsDefined())
                throw new InvalidCastException(string.Format("Invalid ItemType `{0}` for ItemEntity guid `{1}`", type, guid));

            return new ItemValues(guid, width, height, name, description, type, graphicIndex, amount, value, stats);
        }
    }
}
