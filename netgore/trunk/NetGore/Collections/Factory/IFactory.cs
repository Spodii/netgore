using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for a type factory.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IFactory<TKey, TValue>
    {
        /// <summary>
        /// Gets the value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the given <paramref name="key"/>.</returns>
        TValue Get(TKey key);
    }
}
