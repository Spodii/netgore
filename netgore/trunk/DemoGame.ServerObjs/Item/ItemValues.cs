using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;

namespace DemoGame.Server
{
    public class ItemValues : IItemTable
    {
        readonly byte _amount;
        readonly KeyValuePair<StatType, int>[] _baseStats;
        readonly string _description;
        readonly GrhIndex _graphic;
        readonly byte _height;
        readonly SPValueType _hp;
        readonly ItemID _id;
        readonly SPValueType _mp;
        readonly string _name;
        readonly KeyValuePair<StatType, int>[] _reqStats;
        readonly ItemType _type;
        readonly int _value;
        readonly byte _width;

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IItemTable DeepCopy()
        {
            return new ItemTable(this);
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        public int GetStat(StatType key)
        {
            return _baseStats.FirstOrDefault(x => x.Key == key).Value;
        }

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void IItemTable.SetStat(StatType key, int value)
        {
            for (int i = 0; i < _baseStats.Length; i++)
            {
                if (_baseStats[i].Key == key)
                {
                    _baseStats[i] = new KeyValuePair<StatType, int>(key, value);
                    break;
                }
            }
        }

        public byte Amount
        {
            get { return _amount; }
        }

        public IEnumerable<KeyValuePair<StatType, int>> Stats
        {
            get { return _baseStats; }
        }

        public string Description
        {
            get { return _description; }
        }

        public GrhIndex Graphic
        {
            get { return _graphic; }
        }

        public byte Height
        {
            get { return _height; }
        }

        public SPValueType HP
        {
            get { return _hp; }
        }

        public ItemID ID
        {
            get { return _id; }
        }

        public SPValueType MP
        {
            get { return _mp; }
        }

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        public int GetReqStat(StatType key)
        {
            return _reqStats.FirstOrDefault(x => x.Key == key).Value;
        }

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void IItemTable.SetReqStat(StatType key, int value)
        {
            for (int i = 0; i < _reqStats.Length; i++)
            {
                if (_reqStats[i].Key == key)
                {
                    _reqStats[i] = new KeyValuePair<StatType, int>(key, value);
                    break;
                }
            }
        }

        public IEnumerable<KeyValuePair<StatType, int>> ReqStats
        {
            get
            {
                return _reqStats;
            }
        }

        public ItemType Type
        {
            get { return _type; }
        }

        public int Value
        {
            get { return _value; }
        }

        public byte Width
        {
            get { return _width; }
        }

        public ItemValues(ItemID id, byte width, byte height, string name, string description, ItemType type,
                          GrhIndex graphicIndex, byte amount, int value, SPValueType hp, SPValueType mp,
                          IEnumerable<KeyValuePair<StatType, int>> baseStats, IEnumerable<KeyValuePair<StatType, int>> reqStats)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (description == null)
                throw new ArgumentNullException("description");
            if (!type.IsDefined())
                throw new InvalidCastException(string.Format("Invalid ItemType `{0}` for ItemEntity ID `{1}`", type, id));

            _id = id;
            _width = width;
            _height = height;
            _name = name;
            _description = description;
            _type = type;
            _graphic = graphicIndex;
            _amount = amount;
            _value = value;
            _hp = hp;
            _mp = mp;

            _baseStats = baseStats.Where(x => x.Value != 0).ToArray();
            _reqStats = reqStats.Where(x => x.Value != 0).ToArray();
        }
    }
}