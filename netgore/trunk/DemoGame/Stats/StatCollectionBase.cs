using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Platyform.Extensions;

namespace DemoGame
{
    /// <summary>
    /// Base class for a collection of stats.
    /// </summary>
    public abstract class StatCollectionBase : IStatCollection
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<StatType, IStat> _stats = new Dictionary<StatType, IStat>();

        protected void Add(IStat stat)
        {
            if (stat == null)
                return;

            _stats.Add(stat.StatType, stat);
        }

        protected void Add(IEnumerable<IStat> stats)
        {
            foreach (IStat stat in stats)
            {
                Add(stat);
            }
        }

        protected void Add(params IStat[] stats)
        {
            foreach (IStat stat in stats)
            {
                Add(stat);
            }
        }

        /// <summary>
        /// Copies all of the IStat.Values from each stat in the source enumerable where
        /// the IStat.CanWrite is true. Any stat that is in the source enumerable that is not
        /// in the destination, or any stat where CanWrite is false will not have their value copied over.
        /// </summary>
        /// <param name="sourceStats">Source collection of IStats</param>
        /// <param name="errorOnFailure">If true, an ArgumentException will be thrown
        /// if one or more of the StatTypes in the sourceStats do not exist in this StatCollection. If
        /// false, this will not be treated like an error and the value will just not be copied over.</param>
        public void CopyStatValuesFrom(IEnumerable<IStat> sourceStats, bool errorOnFailure)
        {
            // Iterate through each stat in the source
            foreach (IStat sourceStat in sourceStats)
            {
                // Check that this collection handles the given stat
                IStat stat;
                if (!TryGetStat(sourceStat.StatType, out stat))
                {
                    if (errorOnFailure)
                    {
                        const string errmsg =
                            "Tried to copy over the value of StatType `{0}`, but the destination " +
                            "StatCollection did not contain the stat.";
                        throw new ArgumentException(string.Format(errmsg, stat.StatType));
                    }
                    continue;
                }

                // This collection contains the stat, too, and it can be written to, so copy the value over
                if (stat.CanWrite)
                    stat.Value = sourceStat.Value;
            }
        }

        #region IStatCollection Members

        public IEnumerator<IStat> GetEnumerator()
        {
            foreach (IStat stat in _stats.Values)
            {
                yield return stat;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[StatType statType]
        {
            get
            {
                Debug.Assert(Contains(statType), string.Format("Collection does not contain StatType `{0}`.", statType));
                return _stats[statType].Value;
            }
            set
            {
                Debug.Assert(Contains(statType), string.Format("Collection does not contain StatType `{0}`.", statType));

                IStat stat = _stats[statType];
                if (!stat.CanWrite)
                {
                    const string errmsg = "StatType `{0}` is not an ISetableStat.";
                    Debug.Fail(string.Format(errmsg, statType));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, statType);
                    return;
                }

                stat.Value = value;
            }
        }

        public bool Contains(StatType statType)
        {
            return _stats.ContainsKey(statType);
        }

        public IStat GetStat(StatType statType)
        {
            Debug.Assert(Contains(statType), string.Format("Collection does not contain StatType `{0}`.", statType));
            return _stats[statType];
        }

        public bool TryGetStat(StatType statType, out IStat stat)
        {
            return _stats.TryGetValue(statType, out stat);
        }

        #endregion
    }
}