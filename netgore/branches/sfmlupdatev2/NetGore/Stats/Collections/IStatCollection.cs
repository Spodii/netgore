using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Interface for a collection of stats.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public interface IStatCollection<TStatType> : IEnumerable<Stat<TStatType>>
        where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Notifies listeners when the value of any of the stats in this collection have changed.
        /// </summary>
        event TypedEventHandler<IStatCollection<TStatType>, StatCollectionStatChangedEventArgs<TStatType>> StatChanged;

        /// <summary>
        /// Gets or sets the value of the stat of the given <paramref name="statType"/>.
        /// </summary>
        /// <param name="statType">The type of the stat to get or set the value of.</param>
        /// <returns>The value of the stat of the given <paramref name="statType"/>.</returns>
        StatValueType this[TStatType statType] { get; set; }

        /// <summary>
        /// Gets the <see cref="StatCollectionType"/> that this collection is for.
        /// </summary>
        StatCollectionType StatCollectionType { get; }

        /// <summary>
        /// Creates a deep copy of this <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="IStatCollection{TStatType}"/> with all of the
        /// same stat values.</returns>
        IStatCollection<TStatType> DeepCopy();

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another collection of stats for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.</returns>
        bool HasAllGreaterOrEqualValues(IEnumerable<Stat<TStatType>> other);

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another collection of stats for the respective stat type for all stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>True if every stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.</returns>
        bool HasAllGreaterValues(IEnumerable<Stat<TStatType>> other);

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than or equal to the values
        /// in another collection of stats for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// or equal to the stats in the <paramref name="other"/>; otherwise false.</returns>
        bool HasAnyGreaterOrEqualValues(IEnumerable<Stat<TStatType>> other);

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/>'s values are greater than the values
        /// in another collection of stats for the respective stat type for any stats.
        /// </summary>
        /// <param name="other">The stat collection to compare to.</param>
        /// <returns>True if any stat in this <see cref="IStatCollection{TStatType}"/> is greater than
        /// the stats in the <paramref name="other"/>; otherwise false.</returns>
        bool HasAnyGreaterValues(IEnumerable<Stat<TStatType>> other);

        /// <summary>
        /// Checks if this <see cref="IStatCollection{TStatType}"/> contains the same stat values as another
        /// <see cref="IStatCollection{TStatType}"/> for the respective stat type.
        /// </summary>
        /// <param name="other">The <see cref="IStatCollection{TSstatType}"/> to compare against.</param>
        /// <returns>True if this <see cref="IStatCollection{TStatType}"/> contains the same values as the
        /// <paramref name="other"/> for the respective stat type; otherwise false.</returns>
        bool HasSameValues(IStatCollection<TStatType> other);

        /// <summary>
        /// Sets the value of all stats in this collection to the specified value.
        /// </summary>
        /// <param name="value">The value to set all stats to.</param>
        void SetAll(StatValueType value);
    }
}