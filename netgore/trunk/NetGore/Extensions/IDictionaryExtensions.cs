using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IDictionary.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type bool.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static bool AsBool<T>(this IDictionary<T, string> dict, T key)
        {
            return bool.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type bool.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static bool AsBool<T>(this IDictionary<T, string> dict, T key, bool defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            bool parsed;
            if (!bool.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type byte.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static byte AsByte<T>(this IDictionary<T, string> dict, T key)
        {
            return byte.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type byte.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static byte AsByte<T>(this IDictionary<T, string> dict, T key, byte defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            byte parsed;
            if (!byte.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type char.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static char AsChar<T>(this IDictionary<T, string> dict, T key)
        {
            return char.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type char.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static char AsChar<T>(this IDictionary<T, string> dict, T key, char defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            char parsed;
            if (!char.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type DateTime.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static DateTime AsDateTime<T>(this IDictionary<T, string> dict, T key)
        {
            return DateTime.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type DateTime.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static DateTime AsDateTime<T>(this IDictionary<T, string> dict, T key, DateTime defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            DateTime parsed;
            if (!DateTime.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type DateTimeOffset.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static DateTimeOffset AsDateTimeOffset<T>(this IDictionary<T, string> dict, T key)
        {
            return DateTimeOffset.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type DateTimeOffset.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static DateTimeOffset AsDateTimeOffset<T>(this IDictionary<T, string> dict, T key, DateTimeOffset defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            DateTimeOffset parsed;
            if (!DateTimeOffset.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type decimal.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static decimal AsDecimal<T>(this IDictionary<T, string> dict, T key)
        {
            return decimal.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type decimal.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static decimal AsDecimal<T>(this IDictionary<T, string> dict, T key, decimal defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            decimal parsed;
            if (!decimal.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type double.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static double AsDouble<T>(this IDictionary<T, string> dict, T key)
        {
            return double.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type double.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static double AsDouble<T>(this IDictionary<T, string> dict, T key, double defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            double parsed;
            if (!double.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type float.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static float AsFloat<T>(this IDictionary<T, string> dict, T key)
        {
            return float.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type float.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static float AsFloat<T>(this IDictionary<T, string> dict, T key, float defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            float parsed;
            if (!float.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type int.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static int AsInt<T>(this IDictionary<T, string> dict, T key)
        {
            return int.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type int.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static int AsInt<T>(this IDictionary<T, string> dict, T key, int defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            int parsed;
            if (!int.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type long.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static long AsLong<T>(this IDictionary<T, string> dict, T key)
        {
            return long.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type long.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static long AsLong<T>(this IDictionary<T, string> dict, T key, long defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            long parsed;
            if (!long.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type sbyte.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static sbyte AsSByte<T>(this IDictionary<T, string> dict, T key)
        {
            return sbyte.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type sbyte.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static sbyte AsSByte<T>(this IDictionary<T, string> dict, T key, sbyte defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            sbyte parsed;
            if (!sbyte.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type short.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static short AsShort<T>(this IDictionary<T, string> dict, T key)
        {
            return short.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type short.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static short AsShort<T>(this IDictionary<T, string> dict, T key, short defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            short parsed;
            if (!short.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type TimeSpan.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static TimeSpan AsTimeSpan<T>(this IDictionary<T, string> dict, T key)
        {
            return TimeSpan.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type TimeSpan.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static TimeSpan AsTimeSpan<T>(this IDictionary<T, string> dict, T key, TimeSpan defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            TimeSpan parsed;
            if (!TimeSpan.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type uint.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static uint AsUInt<T>(this IDictionary<T, string> dict, T key)
        {
            return uint.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type uint.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static uint AsUInt<T>(this IDictionary<T, string> dict, T key, uint defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            uint parsed;
            if (!uint.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type ulong.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static ulong AsULong<T>(this IDictionary<T, string> dict, T key)
        {
            return ulong.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type ulong.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static ulong AsULong<T>(this IDictionary<T, string> dict, T key, ulong defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            ulong parsed;
            if (!ulong.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type ushort.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static ushort AsUShort<T>(this IDictionary<T, string> dict, T key)
        {
            return ushort.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type ushort.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static ushort AsUShort<T>(this IDictionary<T, string> dict, T key, ushort defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            ushort parsed;
            if (!ushort.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }
    }
}