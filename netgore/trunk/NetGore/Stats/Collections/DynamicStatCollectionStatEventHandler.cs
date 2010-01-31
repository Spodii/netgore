using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Handles events from the <see cref="DynamicStatCollection{T}"/> related to an <see cref="IStat{T}"/>.
    /// </summary>
    /// <typeparam name="T">The stat type.</typeparam>
    /// <param name="sender">The <see cref="DynamicStatCollection{T}"/> the event came from.</param>
    /// <param name="stat">The stat related to the event.</param>
    public delegate void DynamicStatCollectionStatEventHandler<T>(DynamicStatCollection<T> sender, IStat<T> stat)
        where T : struct, IComparable, IConvertible, IFormattable;
}