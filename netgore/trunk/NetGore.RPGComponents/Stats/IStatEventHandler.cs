using System;
using System.Linq;
using NetGore;
using NetGore.RPGComponents;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Handles basic IStat events.
    /// </summary>
    /// <param name="stat">The IStat that the event took place on.</param>
    public delegate void IStatEventHandler<T>(IStat<T> stat) where T : struct, IComparable, IConvertible, IFormattable;
}