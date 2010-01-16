using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Handles basic <see cref="IStat{StatType}"/> events.
    /// </summary>
    /// <param name="stat">The <see cref="IStat{StatType}"/> that the event took place on.</param>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public delegate void IStatEventHandler<TStatType>(IStat<TStatType> stat)
        where TStatType : struct, IComparable, IConvertible, IFormattable;
}