using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// A string comparer that compares two strings using a natural string comaprison that will order
    /// numbers by their parsed numeric values. Will only work properly for strings containing numbers
    /// small enough to be parsed as a 64-bit integer.
    /// </summary>
    public class NaturalStringComparer : IComparer<string>
    {
        static readonly NaturalStringComparer _instance;

        /// <summary>
        /// The maximum length for comparing when using an int.
        /// If the numeric chunk of the string is greater than this length, it will parse to a long instead.
        /// </summary>
        static readonly int _maxIntLength = int.MaxValue.ToString().Length - 1;

        /// <summary>
        /// The maximum length for comparing when using a long.
        /// If the numeric chunk of the string is greater than this length, it cannot be parsed to a number,
        /// so a string comparison will be used instead.
        /// </summary>
        static readonly int _maxLongLength = long.MaxValue.ToString().Length - 1;

        /// <summary>
        /// The <see cref="IComparer{T}"/> for handling comparing the strings when both strings contain alpha
        /// characters.
        /// </summary>
        static readonly IComparer<string> _stringComparer = StringComparer.CurrentCulture;

        /// <summary>
        /// Initializes the <see cref="NaturalStringComparer"/> class.
        /// </summary>
        static NaturalStringComparer()
        {
            _instance = new NaturalStringComparer();
        }

        /// <summary>
        /// Gets the <see cref="NaturalStringComparer"/> instance.
        /// </summary>
        public static NaturalStringComparer Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the largest chunk of a string possible from the <paramref name="str"/>, starting with the
        /// <paramref name="marker"/>, that is either all numeric or all alpha.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to use to build up the string.</param>
        /// <param name="str">The original string.</param>
        /// <param name="marker">The position to start at.</param>
        /// <param name="isNumericString">When this method returns, contains if the string chunk grabbed
        /// is a numeric string.</param>
        /// <returns>
        /// The largest chunk of a string possible from the <paramref name="str"/>, starting with the
        /// <paramref name="marker"/>, that is either all numeric or all alpha.
        /// </returns>
        static string GrabStringChunk(StringBuilder sb, string str, ref int marker, out bool isNumericString)
        {
            // Clear the StringBuilder
            sb.Length = 0;

            // Grab the initial character and remember if it is a digit
            var ch = str[marker];
            isNumericString = char.IsDigit(ch);

            // Keep builing up until we run across a different character type, or the string runs out
            while ((marker < str.Length) && (sb.Length == 0 || (char.IsDigit(ch) == isNumericString)))
            {
                sb.Append(ch);
                marker++;

                if (marker < str.Length)
                    ch = str[marker];
            }

            // Return the resulting string
            return sb.ToString();
        }

        /// <summary>
        /// Compares two alphabet strings.
        /// </summary>
        /// <param name="l">The left string.</param>
        /// <param name="r">The right string.</param>
        /// <returns>Less than zero if <paramref name="l"/> is less than <paramref name="r"/>, greater than zero
        /// if <paramref name="l"/> is greater than <paramref name="r"/>, or zero if <paramref name="l"/>
        /// is equal to <paramref name="r"/>.</returns>
        static int SortAlphaString(string l, string r)
        {
            return _stringComparer.Compare(l, r);
        }

        /// <summary>
        /// Compares two numeric strings.
        /// </summary>
        /// <param name="l">The left string.</param>
        /// <param name="r">The right string.</param>
        /// <returns>Less than zero if <paramref name="l"/> is less than <paramref name="r"/>, greater than zero
        /// if <paramref name="l"/> is greater than <paramref name="r"/>, or zero if <paramref name="l"/>
        /// is equal to <paramref name="r"/>.</returns>
        static int SortNumericString(string l, string r)
        {
            if (l.Length > _maxLongLength || r.Length > _maxLongLength)
            {
                // Strings were too large to parse, so compare alphabetically
                return SortAlphaString(l, r);
            }

            if (l.Length > _maxIntLength || r.Length > _maxIntLength)
            {
                // Parse and compare as a 64-bit integer (slower, but allows for larger values)
                var lValue = Convert.ToInt64(l);
                var rValue = Convert.ToInt64(r);
                return (lValue < rValue ? -1 : 1);
            }
            else
            {
                // Parse and compare as a 32-bit integer (faster, but smaller value range)
                var lValue = Convert.ToInt32(l);
                var rValue = Convert.ToInt32(r);
                return (lValue < rValue ? -1 : 1);
            }
        }

        #region IComparer<string> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="l">The left string.</param>
        /// <param name="r">The right string.</param>
        /// <returns>Less than zero if <paramref name="l"/> is less than <paramref name="r"/>, greater than zero
        /// if <paramref name="l"/> is greater than <paramref name="r"/>, or zero if <paramref name="l"/>
        /// is equal to <paramref name="r"/>.</returns>
        public int Compare(string l, string r)
        {
            // If either strings are null, use the default string comparison
            if (l == null || r == null)
                return _stringComparer.Compare(l, r);

            // Set the initial markers both to zero
            var lMarker = 0;
            var rMarker = 0;

            // Create a generic string builder that will be used for building up the string chunks
            var sb = new StringBuilder();

            // Loop through the strings
            while ((lMarker < l.Length) || (rMarker < r.Length))
            {
                // Check if we have reached the end of either of the strings
                if (lMarker >= l.Length)
                    return -1;

                if (rMarker >= r.Length)
                    return 1;

                // Build up a collection of values until we either hit the end of the string, or we
                // reach the respective position in the strings where one char is numeric, and the other is alpha
                bool lChunkIsNumeric;
                bool rChunkIsNumeric;
                var lChunkStr = GrabStringChunk(sb, l, ref lMarker, out lChunkIsNumeric);
                var rChunkStr = GrabStringChunk(sb, r, ref rMarker, out rChunkIsNumeric);

                int result;

                // Check if both chunks contain a digit
                if (lChunkIsNumeric && rChunkIsNumeric)
                {
                    // Sort numerically
                    result = SortNumericString(lChunkStr, rChunkStr);
                }
                else
                {
                    // Sort alphabetically
                    result = SortAlphaString(lChunkStr, rChunkStr);
                }

                // If we found a difference (result != 0), we can return
                if (result != 0)
                    return result;
            }

            // No usable difference found in any part of the string
            return 0;
        }

        #endregion
    }
}