using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="IList{T}"/> interface.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Removes all duplicate items from the <see cref="IList{T}"/>. The item with the lowest
        /// index is the one that is preserved.
        /// </summary>
        /// <typeparam name="T">The type of list items.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="equalityComparer">The equality comparer to use to check for duplicates. The two
        /// arguments will be the items to compare. Returns true if the items are the same, and false if they
        /// are different.</param>
        public static void RemoveDuplicates<T>(this IList<T> list, Func<T, T, bool> equalityComparer)
        {
            for (var i = 0; i < list.Count - 1; i++)
            {
                for (var j = i + 1; j < list.Count; j++)
                {
                    if (equalityComparer(list[i], list[j]))
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}