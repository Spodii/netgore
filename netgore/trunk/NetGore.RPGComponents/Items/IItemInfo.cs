using System;
using System.Linq;
using NetGore;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Interface for a class that contains the information on an item instance.
    /// </summary>
    public interface IItemInfo<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Gets the <see cref="IStatCollection&lt;T&gt;"/> containing the base stats.
        /// </summary>
        IStatCollection<T> BaseStats { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the <see cref="GrhIndex"/>.
        /// </summary>
        GrhIndex GrhIndex { get; }

        /// <summary>
        /// Gets the amount of HP the item restores upon use.
        /// </summary>
        SPValueType HP { get; }

        /// <summary>
        /// Gets the amount of MP the item restores upon use.
        /// </summary>
        SPValueType MP { get; }

        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="IStatCollection&lt;T&gt;"/> containing the required stats.
        /// </summary>
        IStatCollection<T> ReqStats { get; }

        /// <summary>
        /// Gets the base value of the item.
        /// </summary>
        int Value { get; }
    }
}