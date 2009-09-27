using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the information for an instance of an item.
    /// </summary>
    public class ItemInfo : IItemInfo
    {
        IStatCollection _baseStats;
        string _description;
        GrhIndex _grhIndex;
        SPValueType _hp;
        SPValueType _mp;
        string _name;
        IStatCollection _reqStats;
        int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfo"/> class.
        /// </summary>
        public ItemInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfo"/> class.
        /// </summary>
        /// <param name="r">The BitStream to read the information from.</param>
        public ItemInfo(BitStream r)
        {
            Read(r);
        }

        /// <summary>
        /// Reads the values for this <see cref="ItemInfo"/> from the given <see cref="BitStream"/>.
        /// </summary>
        /// <param name="r">The BitStream to read the information from.</param>
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


        /// <summary>
        /// Writes the ItemInfo to the specified BitStream.
        /// </summary>
        /// <param name="w">The <see cref="BitStream"/> to write to.</param>
        /// <param name="name">The name.</param>
        /// <param name="desc">The desc.</param>
        /// <param name="value">The value.</param>
        /// <param name="grhIndex">Index of the GRH.</param>
        /// <param name="hp">The hp.</param>
        /// <param name="mp">The mp.</param>
        /// <param name="baseStats">The base stats.</param>
        /// <param name="reqStats">The req stats.</param>
        public static void Write(BitStream w, string name, string desc, int value, GrhIndex grhIndex, SPValueType hp,
                                 SPValueType mp, IStatCollection baseStats, IStatCollection reqStats)
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

        /// <summary>
        /// Writes the ItemInfo to the specified BitStream.
        /// </summary>
        /// <param name="w">The <see cref="BitStream"/> to write to.</param>
        public void Write(BitStream w)
        {
            Write(w, Name, Description, Value, GrhIndex, HP, MP, BaseStats, ReqStats);
        }

        #region IItemInfo Members

        /// <summary>
        /// Gets the <see cref="IItemInfo.GrhIndex"/>.
        /// </summary>
        public GrhIndex GrhIndex
        {
            get { return _grhIndex; }
        }

        /// <summary>
        /// Gets the <see cref="IStatCollection"/> containing the base stats.
        /// </summary>
        public IStatCollection BaseStats
        {
            get { return _baseStats; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the amount of HP the item restores upon use.
        /// </summary>
        public SPValueType HP
        {
            get { return _hp; }
        }

        /// <summary>
        /// Gets the amount of MP the item restores upon use.
        /// </summary>
        public SPValueType MP
        {
            get { return _mp; }
        }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the <see cref="IStatCollection"/> containing the required stats.
        /// </summary>
        public IStatCollection ReqStats
        {
            get { return _reqStats; }
        }

        /// <summary>
        /// Gets the base value of the item.
        /// </summary>
        public int Value
        {
            get { return _value; }
        }

        #endregion
    }
}