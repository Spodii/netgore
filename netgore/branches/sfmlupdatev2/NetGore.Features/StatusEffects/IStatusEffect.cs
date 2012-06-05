using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.StatusEffects
{
    /// <summary>
    /// Interface for an object that describes an effect that can be placed on an entity.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    /// <typeparam name="TStatusEffectType">The type of status effect.</typeparam>
    public interface IStatusEffect<TStatType, out TStatusEffectType>
        where TStatType : struct, IComparable, IConvertible, IFormattable
        where TStatusEffectType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Gets the <see cref="StatusEffectMergeType"/> that describes how to handle merging multiple applications
        /// of this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> onto the same object.
        /// </summary>
        StatusEffectMergeType MergeType { get; }

        /// <summary>
        /// Gets the <typeparamref name="TStatType"/>s that this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> modifies. Any <typeparamref name="TStatType"/>
        /// that is not in this IEnumerable is never affected by this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>.
        /// </summary>
        IEnumerable<TStatType> ModifiedStats { get; }

        /// <summary>
        /// Gets the <see cref="StatusEffectType"/> that this
        /// <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> handles.
        /// </summary>
        TStatusEffectType StatusEffectType { get; }

        /// <summary>
        /// Gets the time, in milliseconds, that the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> will last.
        /// </summary>
        /// <param name="power">The power of the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// to get the time of.</param>
        /// <returns>The time a <see cref="IStatusEffect{TStatType, TStatusEffectType}"/> with the given
        /// <paramref name="power"/> will last.</returns>
        int GetEffectTime(ushort power);

        /// <summary>
        /// Gets the stat modifier bonus from this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// on the given <paramref name="statType"/> with the given <paramref name="power"/>.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> to get the modifier bonus of.</param>
        /// <param name="power">The power of the <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>.</param>
        /// <returns>The modifier bonus from this <see cref="IStatusEffect{TStatType, TStatusEffectType}"/>
        /// on the given <paramref name="statType"/> with the given <paramref name="power"/>.</returns>
        int GetStatModifier(TStatType statType, ushort power);

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
        bool TryGetStatModifier(TStatType statType, ushort power, out int value);
    }
}