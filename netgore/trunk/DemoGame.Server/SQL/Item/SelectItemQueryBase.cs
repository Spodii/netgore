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

        protected static ItemValues GetItemValues(IDataRecord r)
        {
            // Stats
            ItemStats stats = new ItemStats();
            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                IStat stat = stats.GetStat(statType);
                stat.Read(r, r.GetOrdinal(statType.GetDatabaseField()));
            }

            // General
            int guid = r.GetInt32(r.GetOrdinal("guid"));
            byte width = r.GetByte(r.GetOrdinal("width"));
            byte height = r.GetByte(r.GetOrdinal("height"));
            string name = r.GetString(r.GetOrdinal("name"));
            string description = r.GetString(r.GetOrdinal("description"));
            ushort graphicIndex = (ushort)r.GetInt16(r.GetOrdinal("graphic")); // NOTE: Will this work for reading a ushort? I don't like the looks of it...
            byte amount = r.GetByte(r.GetOrdinal("amount"));
            int value = r.GetInt32(r.GetOrdinal("value"));
            ItemType type = (ItemType)r.GetByte(r.GetOrdinal("type")); // TODO: Add GetItemType() extension for IDataReader in DemoGameServer

            // FUTURE: Recover from this error by just not creating an item
            if (!type.IsDefined())
                throw new InvalidCastException(string.Format("Invalid ItemType `{0}` for ItemEntity guid `{1}`", type, guid));

            return new ItemValues(guid, width, height, name, description, type, graphicIndex, amount, value, stats);
        }
    }
}
