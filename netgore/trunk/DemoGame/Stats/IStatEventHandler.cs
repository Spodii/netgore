using System;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Handles basic IStat events.
    /// </summary>
    /// <param name="stat">The IStat that the event took place on.</param>
    public delegate void IStatEventHandler<TStatType>(IStat<TStatType> stat) where TStatType : struct, IComparable, IConvertible, IFormattable;
}