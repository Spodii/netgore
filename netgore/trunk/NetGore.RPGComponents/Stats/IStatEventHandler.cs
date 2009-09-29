using System;
using System.Linq;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Handles basic IStat events.
    /// </summary>
    /// <param name="stat">The IStat that the event took place on.</param>
    public delegate void IStatEventHandler<T>(IStat<T> stat) where T : struct, IComparable, IConvertible, IFormattable;
}