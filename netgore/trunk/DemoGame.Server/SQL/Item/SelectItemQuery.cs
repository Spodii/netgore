using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using NetGore.Extensions;

namespace DemoGame.Server
{
    public class SelectItemQuery : IDisposable
    {
        static readonly ItemOC _ordinalCache = new ItemOC();
        readonly MySqlParameter _pGuid = new MySqlParameter("@guid", null);
        readonly MySqlParameter _pHigh = new MySqlParameter("@high", null);
        readonly MySqlParameter _pLow = new MySqlParameter("@low", null);

        readonly MySqlCommand _rangeCommand;
        readonly object _rangeLock = new object();

        readonly MySqlCommand _singleCommand;
        readonly object _singleLock = new object();

        public MySqlConnection MySqlConnection
        {
            get { return _singleCommand.Connection; }
        }

        public SelectItemQuery(MySqlConnection conn)
        {
            _singleCommand = conn.CreateCommand();
            _singleCommand.CommandText = "SELECT * FROM `items` WHERE `guid`=@guid";
            _singleCommand.Parameters.Add(_pGuid);

            _rangeCommand = conn.CreateCommand();
            _rangeCommand.CommandText = "SELECT * FROM `items` WHERE `guid` BETWEEN @low AND @high";
            _rangeCommand.Parameters.Add(_pLow);
            _rangeCommand.Parameters.Add(_pHigh);

            // FUTURE: _singleCommand.Prepare()
            // FUTURE: _rangeCommand.Prepare()
        }

        protected void AddParameter(MySqlParameter parameter)
        {
            _singleCommand.Parameters.Add(parameter);
        }

        protected void AddParameters(IEnumerable<MySqlParameter> parameters)
        {
            foreach (MySqlParameter p in parameters)
            {
                _singleCommand.Parameters.Add(p);
            }
        }

        public ItemValues Execute(int guid)
        {
            if (guid > 0)
                throw new ArgumentOutOfRangeException("guid");

            ItemValues retValues;

            lock (_singleLock)
            {
                // Set the parameter
                _pGuid.Value = guid;

                // Get the reader
                using (MySqlDataReader r = _singleCommand.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!r.Read())
                        throw new Exception("Query contained no results.");

                    retValues = GetValuesFromReader(r);
                }
            }

            return retValues;
        }

        public IEnumerable<ItemValues> Execute(int lowID, int highID)
        {
            if (lowID < 0)
                throw new ArgumentOutOfRangeException("lowID");
            if (highID < 0)
                throw new ArgumentOutOfRangeException("highID");

            // Flips the values if needed
            if (lowID > highID)
            {
                int swap = lowID;
                lowID = highID;
                highID = swap;
            }

            var retValues = new List<ItemValues>(Math.Min((highID - lowID) / 2, 32));

            lock (_rangeLock)
            {
                // Set the parameters
                _pLow.Value = lowID;
                _pHigh.Value = highID;

                // Get the reader
                using (MySqlDataReader r = _rangeCommand.ExecuteReader())
                {
                    while (r.Read())
                    {
                        retValues.Add(GetValuesFromReader(r));
                    }
                }
            }

            return retValues;
        }

        public IDictionary<EquipmentSlot, ItemValues> ExecuteUserEquippedItems(int userGuid)
        {
            // FUTURE: Optimize for faster query execution
            var retValues = new Dictionary<EquipmentSlot, ItemValues>();

            using (MySqlCommand cmd = MySqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT items.*,user_equipped.slot FROM `items`,`user_equipped` " +
                                  "WHERE user_equipped.user_guid = @userGuid " + "AND items.guid = user_equipped.item_guid";

                cmd.Parameters.AddWithValue("@userGuid", userGuid);

                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        byte slot = (byte)r["slot"];
                        ItemValues values = GetValuesFromReader(r);
                        retValues.Add((EquipmentSlot)slot, values);
                    }
                }
            }

            return retValues;
        }

        public IEnumerable<ItemValues> ExecuteUserInventoryItems(int userGuid)
        {
            // FUTURE: Optimize for faster query execution
            var retValues = new List<ItemValues>(Inventory.MaxInventorySize);

            using (MySqlCommand cmd = MySqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT items.* FROM `items`,`user_inventory` " + "WHERE user_inventory.user_guid = @userGuid " +
                                  "AND items.guid = user_inventory.item_guid";

                cmd.Parameters.AddWithValue("@userGuid", userGuid);

                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        retValues.Add(GetValuesFromReader(r));
                    }
                }
            }

            return retValues;
        }

        static ItemValues GetValuesFromReader(MySqlDataReader r)
        {
            // Ensure the ordinal cache is ready
            _ordinalCache.Initialize(r);

            // Stats
            ItemStats stats = new ItemStats();
            foreach (StatType statType in ItemStats.DatabaseStats)
            {
                IStat stat = stats.GetStat(statType);
                stat.Read(r, _ordinalCache.GetStatOrdinal(statType));
            }

            // General
            int guid = r.GetInt32(_ordinalCache.Guid);
            byte width = r.GetByte(_ordinalCache.Width);
            byte height = r.GetByte(_ordinalCache.Height);
            string name = r.GetString(_ordinalCache.Name);
            string description = r.GetString(_ordinalCache.Description);
            ushort graphicIndex = r.GetUInt16(_ordinalCache.Graphic);
            byte amount = r.GetByte(_ordinalCache.Amount);
            int value = r.GetInt32(_ordinalCache.Value);
            ItemType type = (ItemType)r.GetByte(_ordinalCache.Type);

            // FUTURE: Recover from this error by just not creating an item
            if (!type.IsDefined())
                throw new InvalidCastException(string.Format("Invalid ItemType `{0}` for ItemEntity guid `{1}`", type, guid));

            return new ItemValues(guid, width, height, name, description, type, graphicIndex, amount, value, stats);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _rangeCommand.Dispose();
            _singleCommand.Dispose();
        }

        #endregion

        class ItemOC : OrdinalCacheBase
        {
            byte _amount;
            byte _description;
            byte _graphicIndex;
            byte _guid;
            byte _height;
            byte _name;
            byte?[] _statOrdinals;
            byte _type;
            byte _value;
            byte _width;

            public int Amount
            {
                get { return _amount; }
            }

            public int Description
            {
                get { return _description; }
            }

            public int Graphic
            {
                get { return _graphicIndex; }
            }

            public int Guid
            {
                get { return _guid; }
            }

            public int Height
            {
                get { return _height; }
            }

            public int Name
            {
                get { return _name; }
            }

            public int Type
            {
                get { return _type; }
            }

            public int Value
            {
                get { return _value; }
            }

            public int Width
            {
                get { return _width; }
            }

            public int GetStatOrdinal(StatType statType)
            {
                return GetStatOrdinalHelper(statType, _statOrdinals);
            }

            protected override void LoadCache(IDataRecord dataRecord)
            {
                _guid = dataRecord.GetOrdinalAsByte("guid");
                _width = dataRecord.GetOrdinalAsByte("width");
                _height = dataRecord.GetOrdinalAsByte("height");
                _name = dataRecord.GetOrdinalAsByte("name");
                _description = dataRecord.GetOrdinalAsByte("description");
                _type = dataRecord.GetOrdinalAsByte("type");
                _graphicIndex = dataRecord.GetOrdinalAsByte("graphic");
                _amount = dataRecord.GetOrdinalAsByte("amount");
                _value = dataRecord.GetOrdinalAsByte("value");

                _statOrdinals = CreateStatOrdinalCache(dataRecord, ItemStats.DatabaseStats);
            }
        }
    }
}