using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IEnumerable class.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// The default delimiter to use for Implode.
        /// </summary>
        const string _defaultImplodeDelimiter = ", ";

        /// <summary>
        /// Checks if two IEnumerables contain the exact same elements. Order does not matter.
        /// </summary>
        /// <typeparam name="T">The Type of object.</typeparam>
        /// <param name="a">The first collection.</param>
        /// <param name="b">The second collection.</param>
        /// <returns>True if both IEnumerables contain the same items, and same number of items; otherwise, false.</returns>
        public static bool ContainSameElements<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a.Count() != b.Count())
                return false;

            var listB = b.ToList();

            foreach (var item in a)
            {
                if (!listB.Remove(item))
                    return false;
            }

            Debug.Assert(listB.Count == 0);

            return true;
        }

        public static bool HasDuplicates<T>(this IEnumerable<T> source)
        {
            if (source.Count() <= 1)
                return false;

            var a = source.ToArray();

            for (var outer = 0; outer < a.Length - 1; outer++)
            {
                for (var inner = outer + 1; inner < a.Length; inner++)
                {
                    if (a[inner].Equals(a[outer]))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Combines all items in an IEnumerable together into a delimited string.
        /// </summary>
        /// <param name="source">A sequence that contains elements to be imploded.</param>
        /// <param name="delimiter">Character to use when combining the characters.</param>
        /// <returns>All items in an IEnumerable together into a delimited string.</returns>
        public static string Implode(this IEnumerable<string> source, char delimiter)
        {
            var sb = new StringBuilder(128);

            // Add all to the StringBuilder
            foreach (var item in source)
            {
                sb.Append(item);
                sb.Append(delimiter);
            }

            // Remove the last delimiter, or else our list will look like: a,b,c,d,f,
            if (sb.Length >= 1)
                sb.Remove(sb.Length - 1, 1);

            // Return the built string
            return sb.ToString();
        }

        /// <summary>
        /// Combines all items in an IEnumerable together into a delimited string.
        /// </summary>
        /// <param name="source">A sequence that contains elements to be imploded.</param>
        /// <param name="delimiter">Character to use when combining the characters.</param>
        /// <returns>All items in an IEnumerable together into a delimited string.</returns>
        public static string Implode(this IEnumerable<string> source, string delimiter = _defaultImplodeDelimiter)
        {
            var sb = new StringBuilder(128);

            // Add all to the StringBuilder
            foreach (var item in source)
            {
                sb.Append(item);
                sb.Append(delimiter);
            }

            // Remove the last delimiter, or else our list will look like: a,b,c,d,f,
            if (sb.Length >= delimiter.Length)
                sb.Remove(sb.Length - delimiter.Length, delimiter.Length);

            // Return the built string
            return sb.ToString();
        }

        /// <summary>
        /// Combines all items in an IEnumerable together into a delimited string.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="source">A sequence that contains elements to be imploded.</param>
        /// <param name="delimiter">Character to use when combining the characters.</param>
        /// <returns>All items in an IEnumerable together into a delimited string.</returns>
        public static string Implode<T>(this IEnumerable<T> source, char delimiter)
        {
            // Allocate 16 characters for each value, plus room for the delimiter
            var sb = new StringBuilder(source.Count() * (8 + 1));

            // Add all to the StringBuilder
            foreach (var item in source)
            {
                sb.Append(item);
                sb.Append(delimiter);
            }

            // Remove the last delimiter, or else our list will look like: a,b,c,d,f,
            if (sb.Length >= 1)
                sb.Remove(sb.Length - 1, 1);

            // Return the built string
            return sb.ToString();
        }

        /// <summary>
        /// Combines all items in an IEnumerable together into a delimited string.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="source">A sequence that contains elements to be imploded.</param>
        /// <param name="delimiter">Character to use when combining the characters.</param>
        /// <returns>All items in an IEnumerable together into a delimited string.</returns>
        public static string Implode<T>(this IEnumerable<T> source, string delimiter = _defaultImplodeDelimiter)
        {
            // Allocate 8 characters for each value, plus room for the delimiter
            var sb = new StringBuilder(source.Count() * (8 + delimiter.Length));

            // Add all to the StringBuilder
            foreach (var item in source)
            {
                sb.Append(item);
                sb.Append(delimiter);
            }

            // Remove the last delimiter, or else our list will look like: a,b,c,d,f,
            if (sb.Length >= delimiter.Length)
                sb.Remove(sb.Length - delimiter.Length, delimiter.Length);

            // Return the built string
            return sb.ToString();
        }

        /// <summary>
        /// Checks if an IEnumerable is empty.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        /// <param name="source">The IEnumerable to check if empty.</param>
        /// <returns>True if the <paramref name="source"/> is null or empty; otherwise false.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;

            return !source.Any();
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the greatest value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <returns>The element in the <paramref name="src"/> with the greatest value as defined
        /// by <paramref name="func"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="src"/> is null or empty.</exception>
        public static T MaxElement<T, TCompare>(this IEnumerable<T> src, Func<T, TCompare> func)
            where TCompare : IComparable<TCompare>
        {
            return MaxElement(src, func, true);
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the greatest value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <param name="throwOnEmpty">If true, an <see cref="ArgumentException"/> will be thrown if the
        /// <paramref name="src"/> is empty.</param>
        /// <returns>
        /// The element in the <paramref name="src"/> with the greatest value as defined
        /// by <paramref name="func"/>.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="src"/> is null or empty and <paramref name="throwOnEmpty"/>
        /// is true..</exception>
        static T MaxElement<T, TCompare>(IEnumerable<T> src, Func<T, TCompare> func, bool throwOnEmpty)
            where TCompare : IComparable<TCompare>
        {
            if (src == null)
                throw new ArgumentException("The source IEnumerable cannot be null.", "src");

            // Turn the elements into an array to reduce some overhead (since we have to digest anyways)
            var elements = src.ToArray();

            // Check if the elements array is empty
            if (elements.Length == 0)
            {
                if (throwOnEmpty)
                    throw new ArgumentException("The source IEnumerable cannot be empty.", "src");
                else
                    return default(T);
            }

            // Set the initial max element to the first element
            var maxItem = elements[0];
            var maxValue = func(maxItem);

            // Compare against all the remaining values
            for (var i = 1; i < elements.Length; i++)
            {
                // Get the "value" of the element
                var temp = func(elements[i]);

                // Set the new min element if the value is greater
                if (temp.CompareTo(maxValue) > 0)
                {
                    maxValue = temp;
                    maxItem = elements[i];
                }
            }

            return maxItem;
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the greatest value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <returns>The element in the <paramref name="src"/> with the greatest value as defined
        /// by <paramref name="func"/>.</returns>
        public static T MaxElementOrDefault<T, TCompare>(this IEnumerable<T> src, Func<T, TCompare> func)
            where TCompare : IComparable<TCompare>
        {
            return MaxElement(src, func, false);
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the least value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <returns>The element in the <paramref name="src"/> with the least value as defined
        /// by <paramref name="func"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="src"/> is null or empty.</exception>
        public static T MinElement<T, TCompare>(this IEnumerable<T> src, Func<T, TCompare> func)
            where TCompare : IComparable<TCompare>
        {
            return MinElement(src, func, true);
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the least value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <param name="throwOnEmpty">If true, an <see cref="ArgumentException"/> will be thrown if the
        /// <paramref name="src"/> is empty.</param>
        /// <returns>
        /// The element in the <paramref name="src"/> with the least value as defined
        /// by <paramref name="func"/>.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="src"/> is null or empty and <paramref name="throwOnEmpty"/>
        /// is true..</exception>
        static T MinElement<T, TCompare>(IEnumerable<T> src, Func<T, TCompare> func, bool throwOnEmpty)
            where TCompare : IComparable<TCompare>
        {
            if (src == null)
                throw new ArgumentException("The source IEnumerable cannot be null.", "src");

            // Turn the elements into an array to reduce some overhead (since we have to digest anyways)
            var elements = src.ToArray();

            // Check if the elements array is empty
            if (elements.Length == 0)
            {
                if (throwOnEmpty)
                    throw new ArgumentException("The source IEnumerable cannot be empty.", "src");
                else
                    return default(T);
            }

            // Set the initial min element to the first element
            var minItem = elements[0];
            var minValue = func(minItem);

            // Compare against all the remaining values
            for (var i = 1; i < elements.Length; i++)
            {
                // Get the "value" of the element
                var temp = func(elements[i]);

                // Set the new min element if the value is less
                if (temp.CompareTo(minValue) < 0)
                {
                    minValue = temp;
                    minItem = elements[i];
                }
            }

            return minItem;
        }

        /// <summary>
        /// Returns the element in the given IEnumerable with the least value.
        /// </summary>
        /// <typeparam name="T">The Type of collection element.</typeparam>
        /// <typeparam name="TCompare">The Type of comparison value.</typeparam>
        /// <param name="src">The IEnumerable containing the elements to compare.</param>
        /// <param name="func">The Func used to find the value to compare on.</param>
        /// <returns>The element in the <paramref name="src"/> with the least value as defined
        /// by <paramref name="func"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="src"/> is null or empty.</exception>
        public static T MinElementOrDefault<T, TCompare>(this IEnumerable<T> src, Func<T, TCompare> func)
            where TCompare : IComparable<TCompare>
        {
            return MinElement(src, func, false);
        }

        /// <summary>
        /// Gets the smallest free value available.
        /// </summary>
        /// <param name="usedValues">The used values.</param>
        /// <param name="minValue">The lowest acceptable value.</param>
        /// <returns>The next free value available.</returns>
        public static int NextFreeValue(this IEnumerable<int> usedValues, int minValue = 0)
        {
            var expected = minValue;

            // Loop through the sorted used values until we find a gap
            foreach (var value in usedValues.OrderBy(x => x))
            {
                if (value <= expected)
                    expected = value + 1;
                else
                    break;
            }

            return Math.Max(expected, minValue);
        }

        /// <summary>
        /// Creates a compact IEnumerable from the given IEnumerable. The created IEnumerable is intended to have as
        /// small of a memory footprint as possible while retaining the ability to quickly iterate over. This method is
        /// intended for being used on an unchanging IEnumerable that will remain in memory for a while.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="e">The IEnumerable to make compact.</param>
        /// <returns>The given IEnumerable as a compact IEnumerable.</returns>
        public static IEnumerable<T> ToCompact<T>(this IEnumerable<T> e)
        {
            return e.ToArray();
        }

        /// <summary>
        /// Creates an immutable IEnumerable from the given IEnumerable. At the point this method is called, the
        /// given IEnumerable will be digested and the returned IEnumerable will be safe from the underlying
        /// collection changing.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="e">The IEnumerable to make immutable.</param>
        /// <returns>The given IEnumerable as an immutable IEnumerable.</returns>
        public static IEnumerable<T> ToImmutable<T>(this IEnumerable<T> e)
        {
            // An array is already considered "safe" since, while the values may change, it will not break the enumeration
            if (e is T[])
                return e;

            return e.ToArray();
        }
    }
}