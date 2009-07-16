using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    public class ItemValues
    {
        readonly byte _amount;
        readonly string _description;
        readonly GrhIndex _graphicIndex;
        readonly ItemID _id;
        readonly byte _height;
        readonly string _name;
        readonly IEnumerable<StatTypeValue> _baseStats;
        readonly IEnumerable<StatTypeValue> _reqStats;
        readonly ItemType _type;
        readonly int _value;
        readonly byte _width;

        public byte Amount
        {
            get { return _amount; }
        }

        public string Description
        {
            get { return _description; }
        }

        public GrhIndex GraphicIndex
        {
            get { return _graphicIndex; }
        }

        public ItemID ID
        {
            get { return _id; }
        }

        public byte Height
        {
            get { return _height; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<StatTypeValue> BaseStats
        {
            get { return _baseStats; }
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

        public ItemValues(ItemID id, byte width, byte height, string name, string description, ItemType type, GrhIndex graphicIndex,
                          byte amount, int value, IEnumerable<StatTypeValue> baseStats
            , IEnumerable<StatTypeValue> reqStats)
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
            _graphicIndex = graphicIndex;
            _amount = amount;
            _value = value;

            _baseStats = baseStats.ToArray();
            _reqStats = reqStats.ToArray();
        }

        public ItemValues(ItemEntity ie, ItemID id)
            : this(
                id, (byte)ie.CB.Width, (byte)ie.CB.Height, ie.Name, ie.Description, ie.Type, ie.GraphicIndex, ie.Amount,
                ie.Value, ie.BaseStats.ToStatTypeValues(), ie.ReqStats.ToStatTypeValues())
        {
            if (ie == null)
                throw new ArgumentNullException("ie");
        }
    }
}