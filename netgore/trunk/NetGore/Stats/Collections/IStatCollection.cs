using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Interface for a collection of <see cref="IStat{TStatType}"/>s that provides access to the
    /// <see cref="IStat{TStatType}"/>'s value through the <typeparamref name="TStatType"/>.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public interface IStatCollection<TStatType> : IEnumerable<IStat<TStatType>>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        // TODO: IStatCollection tests...

        /// <summary>
        /// Gets or sets the value of the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to get or set the value for.</param>
        /// <returns>The value of the <see cref="IStat{TStatType}"/> with the given <paramref name="statType"/>.</returns>
        int this[TStatType statType] { get; set; }

        /// <summary>
        /// Gets the <see cref="StatCollectionType"/> that this collection is for.
        /// </summary>
        StatCollectionType StatCollectionType { get; }

        /// <summary>
        /// Checks if this collection contains the stat with the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to check if exists in the collection.</param>
        /// <returns>True if this collection contains the <see cref="IStat{TStatType}"/> with the given
        /// <paramref name="statType"/>; otherwise false.</returns>
        bool Contains(TStatType statType);

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given
        /// <typeparamref name="TStatType"/> into this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <param name="values">IEnumerable of stat types and stat values to copy into this
        /// <see cref="IStatCollection{TStatType}"/>.</param>
        /// <param name="checkContains">If true, each stat type in <paramref name="values"/> will first be checked
        /// if it is in this <see cref="IStatCollection{TStatType}"/> before trying to copy over the value.
        /// Any stat type in <paramref name="values"/> but not in this <see cref="IStatCollection{TStatType}"/> will be
        /// skipped. If false, no checking will be done. Any stat type in <paramref name="values"/> but not in this
        /// <see cref="IStatCollection{TStatType}"/> will behave the same as if the value of a stat type not in this
        /// <see cref="IStatCollection{TStatType}"/> was attempted to be assigned in any other way.</param>
        void CopyValuesFrom(IEnumerable<KeyValuePair<TStatType, int>> values, bool checkContains);

        /// <summary>
        /// Copies the values from the given IEnumerable of <paramref name="values"/> using the given stat type
        /// into this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <param name="values">IEnumerable of <typeparamref name="TStatType"/>s and stat values to copy into this
        /// <see cref="IStatCollection{TStatType}"/>.</param>
        /// <param name="checkContains">If true, each stat type in <paramref name="values"/> will first be checked
        /// if it is in this <see cref="IStatCollection{TStatType}"/> before trying to copy over the value. Any stat
        /// type in <paramref name="values"/> but not in this <see cref="IStatCollection{TStatType}"/> will be skipped.
        /// If false, no checking will be done. Any stat type in <paramref name="values"/> but not in this
        /// <see cref="IStatCollection{TStatType}"/> will behave the same as if the value of a stat type not in this
        /// <see cref="IStatCollection{TStatType}"/> was attempted to be assigned in any other way.</param>
        void CopyValuesFrom(IEnumerable<IStat<TStatType>> values, bool checkContains);

        /// <summary>
        /// Gets the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <returns>The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.</returns>
        IStat<TStatType> GetStat(TStatType statType);

        /// <summary>
        /// Tries to get the <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <param name="stat">The <see cref="IStat{StatType}"/> for the stat of the given <paramref name="statType"/>.
        /// If this method returns false, this value will be null.</param>
        /// <returns>True if the <see cref="IStat{StatType}"/> with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        bool TryGetStat(TStatType statType, out IStat<TStatType> stat);

        /// <summary>
        /// Tries to get the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The stat type of the stat to get.</param>
        /// <param name="value">The value of the stat of the given <paramref name="statType"/>. If this method
        /// returns false, this value will be 0.</param>
        /// <returns>True if the stat with the given <paramref name="statType"/> was found and
        /// successfully returned; otherwise false.</returns>
        bool TryGetStatValue(TStatType statType, out int value);
    }
}