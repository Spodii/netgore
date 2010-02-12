using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Handles basic <see cref="IStat{StatType}"/> events.
    /// </summary>
    /// <param name="statCollection">The <see cref="IStatCollection{StatType}"/> the event took place on.</param>
    /// <param name="stat">The <see cref="IStat{StatType}"/> related to the event.</param>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public delegate void IStatCollectionStatEventHandler<TStatType>(
        IStatCollection<TStatType> statCollection, IStat<TStatType> stat)
        where TStatType : struct, IComparable, IConvertible, IFormattable;
}