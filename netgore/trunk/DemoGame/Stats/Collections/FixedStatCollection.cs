using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace DemoGame
{
    /// <summary>
    /// A specialized collection that contains only a portion of the StatTypes. All StatTypes used must be defined
    /// at construction, and no new StatTypes may ever be added. This is the ideal IStatCollection to use when you want
    /// the collection to not contain every StatType, and you don't want to add any new StatTypes.
    /// </summary>
    public class FixedStatCollection : IStatCollection
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly StatCollectionLookupTable _lookupTable;
        readonly StatCollectionType _statCollectionType;
        readonly IStat[] _stats;

        /// <summary>
        /// Gets the StatCollectionLookupTable used by this FixedStatCollection.
        /// </summary>
        public StatCollectionLookupTable LookupTable
        {
            get { return _lookupTable; }
        }

        /// <summary>
        /// FixedStatCollection constructor.
        /// </summary>
        /// <param name="statTypes">IEnumerable of StatTypes to handle.</param>
        /// <param name="statCollectionType">The StatCollectionType that this collection is for.</param>
        public FixedStatCollection(IEnumerable<StatType> statTypes, StatCollectionType statCollectionType)
            : this(new StatCollectionLookupTable(statTypes), statCollectionType)
        {
        }

        /// <summary>
        /// FixedStatCollection constructor.
        /// </summary>
        /// <param name="lookupTable">StatCollectionLookupTable to use for this collection.</param>
        /// <param name="statCollectionType">The StatCollectionType that this collection is for.</param>
        public FixedStatCollection(StatCollectionLookupTable lookupTable, StatCollectionType statCollectionType)
        {
            _lookupTable = lookupTable;
            _statCollectionType = statCollectionType;

            _stats = LookupTable.CreateBuffer(StatCollectionType);
        }

        #region IStatCollection Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IStat> GetEnumerator()
        {
            return ((IEnumerable<IStat>)_stats).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets or sets the value of the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get or set the value for.</param>
        /// <returns>The value of the stat with the given <paramref name="statType"/>.</returns>
        public int this[StatType statType]
        {
            get { return _stats[LookupTable[statType]].Value; }
            set { _stats[LookupTable[statType]].Value = value; }
        }

        /// <summary>
        /// Gets the StatCollectionType that this collection is for.
        /// </summary>
        public StatCollectionType StatCollectionType
        {
            get { return _statCollectionType; }
        }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to check if exists in the collection.</param>
        /// <returns>True if this collection contains the stat with the given <paramref name="statType"/>;
        /// otherwise false.</returns>
        public bool Contains(StatType statType)
        {
            return LookupTable.Contains(statType);
        }

        /// <summary>
        /// Gets the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <returns>The IStat for the stat of the given <paramref name="statType"/>.</returns>
        public IStat GetStat(StatType statType)
        {
            return _stats[LookupTable[statType]];
        }

        /// <summary>
        /// Tries to get the IStat for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="stat">The IStat for the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be null.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStat(StatType statType, out IStat stat)
        {
            if (!LookupTable.Contains(statType))
            {
                stat = null;
                return false;
            }

            stat = GetStat(statType);
            return true;
        }

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        public bool TryGetStatValue(StatType statType, out int value)
        {
            if (!LookupTable.Contains(statType))
            {
                value = 0;
                return false;
            }

            value = this[statType];
            return true;
        }

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given StatType
        /// into this IStatCollection.
        /// </summary>
        /// <param name="values">IEnumerable of StatTypes and stat values to copy into this IStatCollection.</param>
        /// <param name="checkContains">If true, each StatType in <paramref name="values"/> will first be checked
        /// if it is in this IStatCollection before trying to copy over the value. Any StatType in
        /// <paramref name="values"/> but not in this IStatCollection will be skipped. If false, no checking will
        /// be done. Any StatType in <paramref name="values"/> but not in this IStatCollection will behave
        /// the same as if the value of a StatType not in this IStatCollection was attempted to be assigned
        /// in any other way.</param>
        public void CopyValuesFrom(IEnumerable<KeyValuePair<StatType, int>> values, bool checkContains)
        {
            foreach (var value in values)
            {
                if (checkContains && !Contains(value.Key))
                    continue;

                this[value.Key] = value.Value;
            }
        }

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given StatType
        /// into this IStatCollection.
        /// </summary>
        /// <param name="values">IEnumerable of StatTypes and stat values to copy into this IStatCollection.</param>
        /// <param name="checkContains">If true, each StatType in <paramref name="values"/> will first be checked
        /// if it is in this IStatCollection before trying to copy over the value. Any StatType in
        /// <paramref name="values"/> but not in this IStatCollection will be skipped. If false, no checking will
        /// be done. Any StatType in <paramref name="values"/> but not in this IStatCollection will behave
        /// the same as if the value of a StatType not in this IStatCollection was attempted to be assigned
        /// in any other way.</param>
        public void CopyValuesFrom(IEnumerable<IStat> values, bool checkContains)
        {
            CopyValuesFrom(values.Select(x => new KeyValuePair<StatType, int>(x.StatType, x.Value)), checkContains);
        }

        #endregion
    }
}