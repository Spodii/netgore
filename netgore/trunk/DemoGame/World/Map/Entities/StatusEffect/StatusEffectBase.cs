using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// The base class for describing a single status effect that can be placed on a Character. This provides a
    /// description of the StatusEffect as a whole, and not each instance of this StatusEffect being used. Therefore,
    /// only one instance needs to be made for each derived type.
    /// </summary>
    public abstract class StatusEffectBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly StatusEffectMergeType _mergeType;
        readonly StatType[] _modifiedStats;
        readonly StatusEffectType _statusEffectType;

        /// <summary>
        /// StatusEffectBase constructor.
        /// </summary>
        /// <param name="statusEffectType">The StatusEffectType that this StatusEffectBase handles.</param>
        /// <param name="mergeType">The StatusEffectMergeType that describes how to handle merging multiple
        /// applications of this StatusEffect onto the same object.</param>
        protected StatusEffectBase(StatusEffectType statusEffectType, StatusEffectMergeType mergeType)
        {
            _statusEffectType = statusEffectType;
            _mergeType = mergeType;

            _modifiedStats = GetUsedStatTypes();

            AssertReturnValuesAreConsistent();
        }

        /// <summary>
        /// Gets the StatusEffectMergeType that describes how to handle merging multiple applications
        /// of this StatusEffect onto the same object.
        /// </summary>
        public StatusEffectMergeType MergeType
        {
            get { return _mergeType; }
        }

        /// <summary>
        /// Gets the StatTypes that this StatusEffectBase modifies. Any StatType that is not in this IEnumerable is
        /// never affected by this StatusEffect.
        /// </summary>
        public IEnumerable<StatType> ModifiedStats
        {
            get { return _modifiedStats; }
        }

        /// <summary>
        /// Gets the type of StatusEffect that this StatusEffectBase handles.
        /// </summary>
        public StatusEffectType StatusEffectType
        {
            get { return _statusEffectType; }
        }

        /// <summary>
        /// Performs a quick check that this StatusEffect is returning the same value for each StatType and power pair.
        /// </summary>
        [Conditional("DEBUG")]
        void AssertReturnValuesAreConsistent()
        {
            Random r = new Random();

            // Perform 10 test iterations
            for (int i = 0; i < 10; i++)
            {
                // Grab a different, random power for each iteration
                int power = r.Next(0, 100);

                // Make the first iteration the lowest power, and the second iteration the highest power
                if (i == 0)
                    power = 0;
                else if (i == 1)
                    power = GameData.MaxStatusEffectPower;

                // Test each StatType that this StatusEffect actually modifies (in opposed to testing every single one)
                foreach (StatType statType in _modifiedStats)
                {
                    int a = GetStatModifier(statType, (ushort)power);
                    int b = GetStatModifier(statType, (ushort)power);

                    if (a != b)
                    {
                        const string errmsg =
                            "StatusEffect `{0}`, handling StatusEffectType `{1}`, failed to return" +
                            " the same value for StatType `{2}` when using power `{3}`. The values were `{4}` and `{5}`." +
                            " It is vital that a StatusEffect always returns the same value for a given StatType and Power.";

                        string err = string.Format(errmsg, GetType(), StatusEffectType, statType, power, a, b);
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        Debug.Fail(err);
                        throw new Exception(err);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the value for the effect time using the given input values.
        /// </summary>
        /// <param name="minutes">The number of minutes.</param>
        /// <returns>The value for the effect time using the given input values.</returns>
        protected static int CalculateEffectTime(int minutes)
        {
            return 1000 * 60 * minutes;
        }

        /// <summary>
        /// Calculates the value for the effect time using the given input values.
        /// </summary>
        /// <param name="minutes">The number of minutes.</param>
        /// <param name="seconds">The number of seconds.</param>
        /// <returns>The value for the effect time using the given input values.</returns>
        protected static int CalculateEffectTime(int minutes, int seconds)
        {
            return (1000 * 60 * minutes) + (1000 * seconds);
        }

        /// <summary>
        /// Calculates the value for the effect time using the given input values.
        /// </summary>
        /// <param name="minutes">The number of minutes.</param>
        /// <param name="seconds">The number of seconds.</param>
        /// <param name="milliseconds">The number of milliseconds.</param>
        /// <returns>The value for the effect time using the given input values.</returns>
        protected static int CalculateEffectTime(int minutes, int seconds, int milliseconds)
        {
            return (1000 * 60 * minutes) + (1000 * seconds) + milliseconds;
        }

        /// <summary>
        /// When overridden in the derived class, gets the time, in milliseconds, that the StatusEffect will last.
        /// </summary>
        /// <param name="power">The power of the StatusEffect to get the time of.</param>
        /// <returns>The time a StatusEffect with the given <paramref name="power"/> will last.</returns>
        public abstract int GetEffectTime(ushort power);

        /// <summary>
        /// Gets the stat modifier bonus from this StatusEffect on the given <paramref name="statType"/> with
        /// the given <paramref name="power"/>.
        /// </summary>
        /// <param name="statType">The StatType to get the modifier bonus of.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <returns>The modifier bonus from this StatusEffect on the given <paramref name="statType"/> with
        /// the given <paramref name="power"/>.</returns>
        public int GetStatModifier(StatType statType, ushort power)
        {
            int value;
            if (!TryGetStatModifier(statType, power, out value))
                return 0;

            return value;
        }

        /// <summary>
        /// Gets the StatTypes that this StatusEffect modifies.
        /// </summary>
        /// <returns>The StatTypes that this StatusEffect modifies.</returns>
        StatType[] GetUsedStatTypes()
        {
            var usedStatTypes = new List<StatType>();

            foreach (StatType statType in StatTypeHelper.Values)
            {
                int value;
                if (TryGetStatModifier(statType, 1, out value))
                    usedStatTypes.Add(statType);
            }

            return usedStatTypes.ToArray();
        }

        /// <summary>
        /// When overridden in the derived class, gets the stat bonus for the given <paramref name="statType"/> for
        /// a StatusEffect with the given <paramref name="power"/>. All return values must be the same for each
        /// <paramref name="statType"/> and <paramref name="power"/> pair. That is, the same two input values must
        /// always result in the exact same output value.
        /// </summary>
        /// <param name="statType">The StatType to get the modifier bonus of.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <returns>The modifier bonus from this StatusEffect on the given <paramref name="statType"/> with
        /// the given <paramref name="power"/>, or null if the <paramref name="statType"/> is not altered
        /// by this StatusEffect.</returns>
        protected abstract int? InternalTryGetStatModifier(StatType statType, ushort power);

        /// <summary>
        /// Tries to get the stat bonus for the given <paramref name="statType"/> for a StatusEffect with
        /// the given <paramref name="power"/>.
        /// </summary>
        /// <param name="statType">The StatType to get the modifier bonus of.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <param name="value">The modifier bonus from this StatusEffect on the given <paramref name="statType"/>
        /// with the given <paramref name="power"/>.</param>
        /// <returns>True if this StatModifier modifies the given <paramref name="statType"/>. False if the given
        /// <paramref name="statType"/> is not modified by this StatusEffect.</returns>
        public bool TryGetStatModifier(StatType statType, ushort power, out int value)
        {
            var v = InternalTryGetStatModifier(statType, power);

            if (!v.HasValue)
            {
                value = 0;
                return false;
            }
            else
            {
                value = v.Value;
                return true;
            }
        }
    }
}