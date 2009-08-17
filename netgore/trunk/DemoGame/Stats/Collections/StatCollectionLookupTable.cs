using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace DemoGame
{
    /// <summary>
    /// A lookup table used for a collection of stats.
    /// </summary>
    public class StatCollectionLookupTable
    {
        /// <summary>
        /// The value to give unused indices in the lookup table. 
        /// </summary>
        const byte _unusedIndexValue = byte.MaxValue;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The lookup table that accepts the value from StatType.GetValue() and returns the index to use for the
        /// StatType's value array.
        /// </summary>
        readonly byte[] _lookupTable;

        /// <summary>
        /// The StatTypes handled by this StatCollectionLookupTable.
        /// </summary>
        readonly StatType[] _statTypes;

        /// <summary>
        /// Gets the number of StatTypes in this StatCollectionLookupTable.
        /// </summary>
        public int Count
        {
            get { return _statTypes.Length; }
        }

        /// <summary>
        /// Gets the collection index to use for the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to get the array index for.</param>
        /// <returns>The collection index to use for the given <paramref name="statType"/>.</returns>
        /// <exception cref="ArgumentException">The given <paramref name="statType"/> is not handled by
        /// this StatCollectionLookupTable.</exception>
        public int this[StatType statType]
        {
            get { return GetIndex(statType); }
        }

        /// <summary>
        /// Gets the StatTypes handled by this StatCollectionLookupTable.
        /// </summary>
        public IEnumerable<StatType> StatTypes
        {
            get { return _statTypes; }
        }

        /// <summary>
        /// StatCollectionLookupTable constructor.
        /// </summary>
        /// <param name="statTypes">The StatTypes that this StatCollectionLookupTable will contain.</param>
        public StatCollectionLookupTable(IEnumerable<StatType> statTypes)
        {
            _statTypes = statTypes.Distinct().ToArray();
            _lookupTable = CreateLookupTable(_statTypes);
        }

        /// <summary>
        /// Gets if this StatCollectionLookupTable handles the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to check if this StatCollectionLookupTable handles.</param>
        /// <returns>True if this StatCollectionLookupTable handles the given <paramref name="statType"/>;
        /// otherwise false.</returns>
        public bool Contains(StatType statType)
        {
            byte statTypeValue = statType.GetValue();

            if (statTypeValue >= _lookupTable.Length)
                return false;

            byte index = _lookupTable[statTypeValue];

            if (index == _unusedIndexValue)
                return false;

            return true;
        }

        /// <summary>
        /// Creates an array of IStats to be used as the buffer for this StatCollectionLookupTable.
        /// </summary>
        /// <param name="statCollectionType">The StatCollectionType to use for the generated IStats.</param>
        /// <returns>An array of IStats to be used as the buffer for this StatCollectionLookupTable.</returns>
        public IStat[] CreateBuffer(StatCollectionType statCollectionType)
        {
            var ret = new IStat[Count];
            foreach (StatType statType in StatTypes)
            {
                int index = GetIndex(statType);
                IStat istat = StatFactory.CreateStat(statType, statCollectionType);
                ret[index] = istat;
            }

            Debug.Assert(!ret.Any(x => x == null), "Uhm, shouldn't the return array be completely populated?");

            return ret;
        }

        /// <summary>
        /// Creates the lookup table for the given <paramref name="statTypes"/>.
        /// </summary>
        /// <param name="statTypes">The array of StatTypes to create the lookup table for.</param>
        /// <returns>The lookup table for the given <paramref name="statTypes"/>.</returns>
        static byte[] CreateLookupTable(StatType[] statTypes)
        {
            // Get the greatest StatType index value
            int max = statTypes.Select(x => x.GetValue()).Max();

            Debug.Assert(max <= byte.MaxValue, "The lookup table can't handle this many StatTypes!");

            // Create the lookup table, and set the initial values to _unusedIndexValue, which we will use to denote
            // an index in the lookup table as unused
            var ret = new byte[max];
            for (int i = 0; i < max; i++)
            {
                ret[i] = _unusedIndexValue;
            }

            // Populate the lookup table
            for (int i = 0; i < max; i++)
            {
                byte statTypeIndex = statTypes[i].GetValue();
                ret[statTypeIndex] = (byte)i;
            }

            return ret;
        }

        /// <summary>
        /// Gets the collection index to use for the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The StatType to get the array index for.</param>
        /// <returns>The collection index to use for the given <paramref name="statType"/>.</returns>
        /// <exception cref="ArgumentException">The given <paramref name="statType"/> is not handled by
        /// this StatCollectionLookupTable.</exception>
        public int GetIndex(StatType statType)
        {
            byte statTypeValue = statType.GetValue();

            // Check that the value is in range of our lookup table
            if (statTypeValue >= _lookupTable.Length)
                throw GetStatTypeNotHandledArgumentException(statType, "statType");

            byte index = _lookupTable[statTypeValue];

            // Check that the index is a used one
            if (index == _unusedIndexValue)
                throw GetStatTypeNotHandledArgumentException(statType, "statType");

            return index;
        }

        /// <summary>
        /// Gets an ArgumentException to use when the StatType argument is not handled by this StatCollectionLookupTable.
        /// </summary>
        /// <param name="statType">StatType that caused the error.</param>
        /// <param name="argumentName">Name of the argument containing the <paramref name="statType"/>.</param>
        /// <returns>An ArgumentException to use when the StatType argument is not handled by
        /// this StatCollectionLookupTable.</returns>
        static ArgumentException GetStatTypeNotHandledArgumentException(StatType statType, string argumentName)
        {
            const string errmsg = "StatType `{0}` is not handled by this lookup table.";
            string err = string.Format(errmsg, statType);

            if (log.IsErrorEnabled)
                log.Error(err);

            return new ArgumentException(err, argumentName);
        }
    }
}