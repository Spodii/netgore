using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public interface IItemInfo
    {
        IStatCollection BaseStats { get; }
        string Description { get; }
        SPValueType HP { get; }
        SPValueType MP { get; }
        string Name { get; }
        IStatCollection ReqStats { get; }
        int Value { get; }
        GrhIndex GrhIndex { get; }
    }

    public class ItemInfo : IItemInfo
    {
        string _name;
        string _description;
        int _value;
        GrhIndex _grhIndex;
        SPValueType _hp;
        SPValueType _mp;
        IStatCollection _baseStats;
        IStatCollection _reqStats;

        public ItemInfo()
        {
        }

        public ItemInfo(BitStream r)
        {
            Read(r);
        }

        public static void Write(BitStream w, string name, string desc, int value, GrhIndex grhIndex, SPValueType hp, SPValueType mp, IStatCollection baseStats, IStatCollection reqStats)
        {
            w.Write(name);
            w.Write(desc);
            w.Write(value);
            w.Write(grhIndex);
            w.Write(hp);
            w.Write(mp);
            w.Write(baseStats);
            w.Write(reqStats);
        }

        public void Write(BitStream w)
        {
            Write(w, Name, Description, Value, GrhIndex, HP, MP, BaseStats, ReqStats);
        }

        public void Read(BitStream r)
        {
            _name = r.ReadString();
            _description = r.ReadString();
            _value = r.ReadInt();
            _grhIndex = r.ReadGrhIndex();
            _hp = r.ReadSPValueType();
            _mp = r.ReadSPValueType();

            _baseStats = new ItemStatsBase(StatCollectionType.Base);
            _reqStats = new ItemStatsBase(StatCollectionType.Requirement);

            r.ReadStatCollection(_baseStats);
            r.ReadStatCollection(_reqStats);
        }

        public GrhIndex GrhIndex { get { return _grhIndex; } }

        public IStatCollection BaseStats
        {
            get { return _baseStats; }
        }

        public string Description
        {
            get { return _description; }
        }

        public SPValueType HP
        {
            get { return _hp; }
        }

        public SPValueType MP
        {
            get { return _mp; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IStatCollection ReqStats
        {
            get { return _reqStats; }
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
