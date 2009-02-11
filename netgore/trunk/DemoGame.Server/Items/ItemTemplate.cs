using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class ItemTemplate
    {
        readonly string _desc;
        readonly ushort _graphic;
        readonly ushort _guid;
        readonly byte _height;
        readonly string _name;
        readonly ItemStats _stats;
        readonly ItemType _type;
        readonly int _value;
        readonly byte _width;

        public string Description
        {
            get { return _desc; }
        }

        public ushort Graphic
        {
            get { return _graphic; }
        }

        public ushort Guid
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

        public Vector2 Size
        {
            get { return new Vector2(_width, _height); }
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

        public ItemTemplate(ushort guid, string name, string desc, ItemType type, ushort graphic, int value, byte width,
                            byte height, ItemStats stats)
        {
            _guid = guid;
            _name = name;
            _desc = desc;
            _type = type;
            _graphic = graphic;
            _value = value;
            _width = width;
            _height = height;
            _stats = stats;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, Guid);
        }
    }
}