using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    public class ItemTemplate : IItemTemplateTable
    {
        readonly ICollection<KeyValuePair<StatType, int>> _baseStats;
        readonly string _desc;
        readonly GrhIndex _graphic;
        readonly byte _height;
        readonly SPValueType _hp;
        readonly ItemTemplateID _id;
        readonly SPValueType _mp;
        readonly string _name;
        readonly ICollection<KeyValuePair<StatType, int>> _reqStats;
        readonly ItemType _type;
        readonly int _value;
        readonly byte _width;

        public string Description
        {
            get { return _desc; }
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

        public ItemTemplateID ID
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

        public IEnumerable<KeyValuePair<StatType, int>> ReqStats
        {
            get { return _reqStats; }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        public IEnumerable<KeyValuePair<StatType, int>> Stats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets the value of the database column `type`.
        /// </summary>
        byte IItemTemplateTable.Type
        {
            get {
                // TODO: !! Have the generated class use ItemType, not byte
                return (byte)Type; }
        }

        public Vector2 Size
        {
            get { return new Vector2(_width, _height); }
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

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IItemTemplateTable DeepCopy()
        {
            return new ItemTemplateTable(this);
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `ReqStat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        int IItemTemplateTable.GetReqStat(StatType key)
        {
            return _reqStats.FirstOrDefault(x => x.Key == key).Value;
        }

        /// <summary>
        /// Gets the value of the database column in the column collection `Stat`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        int IItemTemplateTable.GetStat(StatType key)
        {
            return _baseStats.FirstOrDefault(x => x.Key == key).Value;
        }

        public ItemTemplate(ItemTemplateID id, string name, string desc, ItemType type, GrhIndex graphic, int value, byte width,
                            byte height, SPValueType hp, SPValueType mp, IEnumerable<KeyValuePair<StatType, int>> baseStats,
                            IEnumerable<KeyValuePair<StatType, int>> reqStats)
        {
            _id = id;
            _name = name;
            _desc = desc;
            _type = type;
            _graphic = graphic;
            _value = value;
            _width = width;
            _height = height;
            _hp = hp;
            _mp = mp;

            _baseStats = baseStats.Where(x => x.Value != 0).ToArray();
            _reqStats = reqStats.Where(x => x.Value != 0).ToArray();

            // Make sure the ItemType is defined
            if (!type.IsDefined())
            {
                const string errmsg = "Invalid ItemType `{0}` for ItemTemplate ID `{1}`.";
                throw new InvalidCastException(string.Format(errmsg, type, id));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }
    }
}