using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Base class for a database ordinal cache.
    /// </summary>
    abstract class OrdinalCacheBase
    {
        bool _isLoaded;

        /// <summary>
        /// Gets if the OrdinalCacheBase has been loaded.
        /// </summary>
        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        /// <summary>
        /// Creates a nullable byte array containing where the array index is the numeric value of the StatType,
        /// and the value is the ordinal of for the StatType.
        /// </summary>
        /// <param name="dataRecord">IDataRecord used to load the StatType ordinals from.</param>
        /// <param name="statTypes">IEnumerable of all the StatType ordinals to load.</param>
        /// <returns>A nullable byte array for the ordinal of each StatType.</returns>
        protected static byte?[] CreateStatOrdinalCache(IDataRecord dataRecord, IEnumerable<StatType> statTypes)
        {
            return CreateStatOrdinalCache(dataRecord, statTypes, false);
        }

        /// <summary>
        /// Creates a nullable byte array containing where the array index is the numeric value of the StatType,
        /// and the value is the ordinal of for the StatType. Any StatType that did not contain a valid
        /// ordinal in the IDataRecord will instead just be omitted from the final byte array.
        /// </summary>
        /// <param name="dataRecord">IDataRecord used to load the StatType ordinals from.</param>
        /// <param name="statTypes">IEnumerable of all the StatType ordinals to load.</param>
        /// <param name="skipNoOrdinal">If true, a DataException will be thrown if one or more StatTypes in
        /// the <paramref name="statTypes"/> was not found. If false, any StatType not found will simply
        /// be skipped.</param>
        /// <returns>A nullable byte array for the ordinal of each StatType found.</returns>
        static byte?[] CreateStatOrdinalCache(IDataRecord dataRecord, IEnumerable<StatType> statTypes, bool skipNoOrdinal)
        {
            if (dataRecord == null)
                throw new ArgumentNullException("dataRecord");
            if (statTypes == null)
                throw new ArgumentNullException("statTypes");

            // Find the number of indicies we will need from the greatest StatType value
            int greatestValue = (int)statTypes.Max() + 1;

            // Create the array for the stat ordinals
            var statOrdinals = new byte?[greatestValue];

            // Iterate through all the StatTypes and get the ordinal
            foreach (StatType statType in statTypes)
            {
                // Get the ordinal
                byte ordinal;
                if (!TryGetStatOrdinal(dataRecord, statType, out ordinal))
                {
                    // If we are not skipping StatTypes with no ordinal, throw an exception
                    if (!skipNoOrdinal)
                    {
                        const string errmsg = "Could not find the ordinal for StatType `{0}`";
                        throw new DataException(string.Format(errmsg, statType));
                    }

                    // Invalid ordinal, but that is fine (see: skipNoOrdinal), so set ordinal as null
                    statOrdinals[(int)statType] = null;
                    continue;
                }

                // Ordinal loaded properly so just set it
                statOrdinals[(int)statType] = ordinal;
            }

            // Find the highest index actually set
            int highestUsedIndex = statOrdinals.Length - 1;
            while (!statOrdinals[highestUsedIndex].HasValue)
            {
                highestUsedIndex--;
            }

            // Shrink down the array to the smallest size needed
            Array.Resize(ref statOrdinals, highestUsedIndex + 1);

            return statOrdinals;
        }

        /// <summary>
        /// Forces the OrdinalCacheBase to reload the ordinal caches.
        /// </summary>
        protected void ForceReload()
        {
            _isLoaded = false;
        }

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes that have valid, loaded ordinals.
        /// </summary>
        /// <param name="ordinals">Nullable byte array of the loaded StatType ordinals.</param>
        /// <returns>An IEnumerable of all of the StatTypes that have valid, loaded ordinals.</returns>
        protected static IEnumerable<StatType> GetLoadedStatOrdinalEnumerator(byte?[] ordinals)
        {
            for (int i = 0; i < ordinals.Length; i++)
            {
                if (ordinals[i].HasValue)
                    yield return (StatType)i;
            }
        }

        /// <summary>
        /// Helper for getting the ordinal for a StatType from a nullable byte array, ensuring that the
        /// ordinal has been properly set.
        /// </summary>
        /// <param name="statType">StatType to get the ordinal for.</param>
        /// <param name="ordinals">Nullable byte array containing the StatType ordinals.</param>
        /// <returns>Ordinal for the <paramref name="statType"/>.</returns>
        protected static byte GetStatOrdinalHelper(StatType statType, byte?[] ordinals)
        {
            // Get the ordinal
            var ordinal = ordinals[(int)statType];

            // Check that the ordinal is valid
            if (!ordinal.HasValue)
            {
                const string errmsg = "StatType `{0}` does not contain a valid ordinal in the cache.";
                throw new ArgumentOutOfRangeException("statType", string.Format(errmsg, statType));
            }

            // Return the ordinal
            return ordinal.Value;
        }

        /// <summary>
        /// Initializes the ordinal cache using an IDataRecord containing all of the items
        /// that need to be cached.
        /// </summary>
        /// <param name="dataRecord">An IDataRecord containing all of the items
        /// that need to be cached.</param>
        public void Initialize(IDataRecord dataRecord)
        {
            if (_isLoaded)
                return;

            if (dataRecord == null)
                throw new ArgumentNullException("dataRecord");

            LoadCache(dataRecord);

            _isLoaded = true;
        }

        /// <summary>
        /// When overridden in the derived class, loads the ordinal caches from a given IDataRecord.
        /// </summary>
        /// <param name="dataRecord">An IDataRecord containing all of the items
        /// that need to be cached.</param>
        protected abstract void LoadCache(IDataRecord dataRecord);

        /// <summary>
        /// Creates a nullable byte array containing where the array index is the numeric value of the StatType,
        /// and the value is the ordinal of for the StatType. Any StatType that did not contain a valid
        /// ordinal in the IDataRecord will instead just be omitted from the final byte array.
        /// </summary>
        /// <param name="dataRecord">IDataRecord used to load the StatType ordinals from.</param>
        /// <param name="statTypes">IEnumerable of all the StatType ordinals to load.</param>
        /// <returns>A nullable byte array for the ordinal of each StatType found.</returns>
        protected static byte?[] TryCreateStatOrdinalCache(IDataRecord dataRecord, IEnumerable<StatType> statTypes)
        {
            return CreateStatOrdinalCache(dataRecord, statTypes, true);
        }

        /// <summary>
        /// Tries to get the ordinal for a StatType.
        /// </summary>
        /// <param name="dataRecord">IDataRecord used to load the ordinals from.</param>
        /// <param name="statType">StatType to try to load.</param>
        /// <param name="ordinal">Ordinal for the StatType, if one was found.</param>
        protected static bool TryGetStatOrdinal(IDataRecord dataRecord, StatType statType, out byte ordinal)
        {
            string field = statType.GetDatabaseField();

            try
            {
                // Get the ordinal of the field with the same name as the StatType
                ordinal = dataRecord.GetOrdinalAsByte(field);
            }
            catch (IndexOutOfRangeException)
            {
                // Ordinal must not have existed in the IDataRecord - failed to load
                ordinal = 0;
                return false;
            }

            return true;
        }
    }
}