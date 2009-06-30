using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    public class ItemValues
    {
        readonly byte _amount;
        readonly string _description;
        readonly ushort _graphicIndex;
        readonly int _guid;
        readonly byte _height;
        readonly string _name;
        readonly ItemStats _stats;
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

        public ushort GraphicIndex
        {
            get { return _graphicIndex; }
        }

        public int Guid
        {
            get { return _guid; }
        }

        public byte Height
        {
            get { return _height; }
        }

        public string Name
        {
            get { return _name; }
        }

        public ItemStats Stats
        {
            get { return _stats; }
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

        public ItemValues(int guid, byte width, byte height, string name, string description, ItemType type, ushort graphicIndex,
                          byte amount, int value, ItemStats stats)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");
            if (name == null)
                throw new ArgumentNullException("name");
            if (description == null)
                throw new ArgumentNullException("description");

            _guid = guid;
            _width = width;
            _height = height;
            _name = name;
            _description = description;
            _type = type;
            _graphicIndex = graphicIndex;
            _amount = amount;
            _value = value;
            _stats = stats;
        }

        public ItemValues(ItemEntity ie, int guid)
            : this(
                guid, (byte)ie.CB.Width, (byte)ie.CB.Height, ie.Name, ie.Description, ie.Type, ie.GraphicIndex, ie.Amount,
                ie.Value, new ItemStats(ie.Stats))
        {
            if (ie == null)
                throw new ArgumentNullException("ie");
        }
    }
}