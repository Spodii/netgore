using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets if a <see cref="String"/> contains another <see cref="String"/>.
        /// </summary>
        /// <param name="str">The <see cref="String"/> to search on.</param>
        /// <param name="toCheck">The <see cref="String"/> to check if is in the <paramref name="str"/>.</param>
        /// <param name="strComp">The <see cref="StringComparison"/>.</param>
        /// <returns>True if the <paramref name="str"/> contains <paramref name="toCheck"/>; otherwise false.</returns>
        public static bool Contains(this string str, string toCheck, StringComparison strComp)
        {
            return str.IndexOf(toCheck, strComp) >= 0;
        }
    }
}
