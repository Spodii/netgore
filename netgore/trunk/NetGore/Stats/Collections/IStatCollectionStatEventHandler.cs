using System;
using System.Linq;

namespace NetGore.Stats
{

    /// <summary>
    /// Delegate for handling when a stat in a <see cref="IStatCollection{TStatType}"/> changes.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    /// <param name="sender">The <see cref="IStatCollection{TStatType}"/> the event came from.</param>
    /// <param name="statType">The type of the stat that changed.</param>
    /// <param name="oldValue">The old value of the stat.</param>
    /// <param name="newValue">The new value of the stat.</param>
    public delegate void StatCollectionStatChangedEventHandler<TStatType>(
        IStatCollection<TStatType> sender, TStatType statType, StatValueType oldValue, StatValueType newValue)
        where TStatType : struct, IComparable, IConvertible, IFormattable;
}