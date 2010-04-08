using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets if a <see cref="string"/> contains another <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to search on.</param>
        /// <param name="toCheck">The <see cref="string"/> to check if is in the <paramref name="str"/>.</param>
        /// <param name="strComp">The <see cref="StringComparison"/>.</param>
        /// <returns>True if the <paramref name="str"/> contains <paramref name="toCheck"/>; otherwise false.</returns>
        public static bool Contains(this string str, string toCheck, StringComparison strComp)
        {
            return str.IndexOf(toCheck, strComp) >= 0;
        }
    }
}