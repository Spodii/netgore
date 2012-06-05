using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Features.StatusEffects
{
    /// <summary>
    /// The base class for describing a single status effect that can be placed on a Character. This provides a
    /// description of the status effect as a whole, and not each instance of this
    /// <see cref="StatusEffect{TStatType, TStatusEffectType}"/> being used. Therefore, only one instance needs
    /// to be made for each derived type.
    /// </summary>
    public abstract class StatusEffect<TStatType, TStatusEffectType> : IStatusEffect<TStatType, TStatusEffectType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
        where TStatusEffectType : struct, IComparable, IConvertible, IFormattable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly ushort _maxStatusEffectPower = StatusEffectsSettings.Instance.MaxStatusEffectPower;

        readonly StatusEffectMergeType _mergeType;
        readonly TStatType[] _modifiedStats;
        readonly TStatusEffectType _statusEffectType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffect{TStatType, TStatusEffectType}"/> class.
        /// </summary>
        /// <param name="statusEffectType">The <see cref="StatusEffectType"/> that this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> handles.</param>
        /// <param name="mergeType">The <see cref="StatusEffectMergeType"/> that describes how to handle merging multiple
        /// applications of this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> onto the same object.</param>
        protected StatusEffect(TStatusEffectType statusEffectType, StatusEffectMergeType mergeType)
        {
            _statusEffectType = statusEffectType;
            _mergeType = mergeType;

            _modifiedStats = GetUsedStatTypes();

            AssertReturnValuesAreConsistent();
        }

        /// <summary>
        /// Performs a quick check that this <see cref="StatusEffect{TStatType, TStatusEffectType}"/>
        /// is returning the same value for each <typeparamref name="TStatType"/> and power pair.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="StatusEffect{T,U}"/> doesn't always return the same
        /// value for a given power as expected.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StatusEffectType")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StatusEffect")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StatType")]
        [Conditional("DEBUG")]
        void AssertReturnValuesAreConsistent()
        {
            var r = new Random();

            // Perform 10 test iterations
            for (var i = 0; i < 10; i++)
            {
                // Grab a different, random power for each iteration
                var power = r.Next(0, 100);

                // Make the first iteration the lowest power, and the second iteration the highest power
                if (i == 0)
                    power = 0;
                else if (i == 1)
                    power = _maxStatusEffectPower;

                // Test each StatType that this instance actually modifies (in opposed to testing every single one)
                foreach (var statType in _modifiedStats)
                {
                    var a = GetStatModifier(statType, (ushort)power);
                    var b = GetStatModifier(statType, (ushort)power);

                    if (a != b)
                    {
                        const string errmsg =
                            "StatusEffect `{0}`, handling StatusEffectType `{1}`, failed to return" +
                            " the same value for StatType `{2}` when using power `{3}`. The values were `{4}` and `{5}`." +
                            " It is vital that a StatusEffect always returns the same value for a given StatType and Power.";

                        var err = string.Format(errmsg, GetType(), StatusEffectType, statType, power, a, b);
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        Debug.Fail(err);
                        throw new InvalidOperationException(err);
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
        /// Gets the <typeparamref name="TStatType"/>s that this StatusEffect modifies.
        /// </summary>
        /// <returns>The <typeparamref name="TStatType"/>s that this StatusEffect modifies.</returns>
        TStatType[] GetUsedStatTypes()
        {
            int dummy;
            return EnumHelper<TStatType>.Values.Where(x => TryGetStatModifier(x, 1, out dummy)).OrderBy(x => x).ToArray();
        }

        /// <summary>
        /// When overridden in the derived class, gets the stat bonus for the given <paramref name="statType"/> for
        /// a StatusEffect with the given <paramref name="power"/>. All return values must be the same for each
        /// <paramref name="statType"/> and <paramref name="power"/> pair. That is, the same two input values must
        /// always result in the exact same output value.
        /// </summary>
        /// <param name="statType">The stat type to get the modifier bonus of.</param>
        /// <param name="power">The power of the StatusEffect.</param>
        /// <returns>The modifier bonus from this StatusEffect on the given <paramref name="statType"/> with
        /// the given <paramref name="power"/>, or null if the <paramref name="statType"/> is not altered
        /// by this StatusEffect.</returns>
        protected abstract int? InternalTryGetStatModifier(TStatType statType, ushort power);

        #region IStatusEffect<TStatType,TStatusEffectType> Members

        /// <summary>
        /// Gets the <see cref="StatusEffectMergeType"/> that describes how to handle merging multiple applications
        /// of this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> onto the same object.
        /// </summary>
        public StatusEffectMergeType MergeType
        {
            get { return _mergeType; }
        }

        /// <summary>
        /// Gets the <typeparamref name="TStatType"/>s that this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> modifies. Any <typeparamref name="TStatType"/>
        /// that is not in this IEnumerable is never affected by this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>.
        /// </summary>
        public IEnumerable<TStatType> ModifiedStats
        {
            get { return _modifiedStats; }
        }

        /// <summary>
        /// Gets the <see cref="StatusEffectType"/> that this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> handles.
        /// </summary>
        public TStatusEffectType StatusEffectType
        {
            get { return _statusEffectType; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the time, in milliseconds, that the
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> will last.
        /// </summary>
        /// <param name="power">The power of the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// to get the time of.</param>
        /// <returns>The time a <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> with the given
        /// <paramref name="power"/> will last.</returns>
        public abstract int GetEffectTime(ushort power);

        /// <summary>
        /// Gets the stat modifier bonus from this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// on the given <paramref name="statType"/> with the given <paramref name="power"/>.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> to get the modifier bonus of.</param>
        /// <param name="power">The power of the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>.</param>
        /// <returns>The modifier bonus from this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// on the given <paramref name="statType"/> with the given <paramref name="power"/>.</returns>
        public int GetStatModifier(TStatType statType, ushort power)
        {
            int value;
            if (!TryGetStatModifier(statType, power, out value))
                return 0;

            return value;
        }

        /// <summary>
        /// Tries to get the stat bonus for the given <paramref name="statType"/> for a
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> with the given <paramref name="power"/>.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> to get the modifier bonus of.</param>
        /// <param name="power">The power of the status effect.</param>
        /// <param name="value">The modifier bonus from this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// on the given <paramref name="statType"/> with the given <paramref name="power"/>.</param>
        /// <returns>True if this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> modifies the given
        /// <paramref name="statType"/>. False if the given <paramref name="statType"/> is not modified by this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>.</returns>
        public bool TryGetStatModifier(TStatType statType, ushort power, out int value)
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

        #endregion
    }
}