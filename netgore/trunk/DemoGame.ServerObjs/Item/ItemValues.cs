using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;

namespace DemoGame.Server
{
    public class ItemValues
    {
        readonly byte _amount;
        readonly IEnumerable<StatTypeValue> _baseStats;
        readonly string _description;
        readonly GrhIndex _graphic;
        readonly byte _height;
        readonly SPValueType _hp;
        readonly ItemID _id;
        readonly SPValueType _mp;
        readonly string _name;
        readonly IEnumerable<StatTypeValue> _reqStats;
        readonly ItemType _type;
        readonly int _value;
        readonly byte _width;

        public byte Amount
        {
            get { return _amount; }
        }

        public IEnumerable<StatTypeValue> BaseStats
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

        public IEnumerable<StatTypeValue> ReqStats
        {
            get { return _reqStats; }
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
                          IEnumerable<StatTypeValue> baseStats, IEnumerable<StatTypeValue> reqStats)
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