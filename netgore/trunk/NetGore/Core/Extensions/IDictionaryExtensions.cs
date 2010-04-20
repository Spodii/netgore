using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return Parser.Invariant.ParseBool(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseByte(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseChar(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseDateTime(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseDateTimeOffset(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseDecimal(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseDouble(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as enum type
        /// <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <typeparam name="TEnum">The enum Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a GrhIndex.</returns>
        public static TEnum AsEnum<T, TEnum>(this IDictionary<T, string> dict, T key)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            return EnumHelper<TEnum>.Parse(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as enum type
        /// <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <typeparam name="TEnum">The enum Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static TEnum AsEnum<T, TEnum>(this IDictionary<T, string> dict, T key, TEnum defaultValue)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            var enumValue = EnumHelper<TEnum>.Parse(value);
            if (!EnumHelper<TEnum>.IsDefined(enumValue))
                return defaultValue;

            return enumValue;
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
            return Parser.Invariant.ParseFloat(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseInt(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseLong(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseSByte(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type short.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a short.</returns>
        public static short AsShort<T>(this IDictionary<T, string> dict, T key)
        {
            return Parser.Invariant.ParseShort(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type string.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a string.</returns>
        public static string AsString<T>(this IDictionary<T, string> dict, T key)
        {
            return dict[key];
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type string.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a string, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static string AsString<T>(this IDictionary<T, string> dict, T key, string defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type
        /// <see cref="TimeSpan"/>.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int.</returns>
        public static TimeSpan AsTimeSpan<T>(this IDictionary<T, string> dict, T key)
        {
            return Parser.Invariant.ParseTimeSpan(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type
        /// <see cref="TimeSpan"/>.
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseUInt(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseULong(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
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
            return Parser.Invariant.ParseUShort(dict[key]);
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
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Removes multiple items from a <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="T">The type of key.</typeparam>
        /// <typeparam name="U">The type of value.</typeparam>
        /// <param name="dict">The <see cref="IDictionary{T,U}"/> to remove the items from.</param>
        /// <param name="removePred">The <see cref="Predicate{T}"/> used to determine if an item should be
        /// removed from the <paramref name="dict"/>.</param>
        /// <returns>The items that were removed.</returns>
        public static IEnumerable<T> RemoveMany<T, U>(this IDictionary<T, U> dict, Predicate<KeyValuePair<T, U>> removePred)
        {
            // Skip an empty dictionary
            if (dict.Count == 0)
                return Enumerable.Empty<T>();

            // Gather the items to remove, creating the list only if something is found
            List<T> toRemove = null;
            foreach (var kvp in dict)
            {
                if (!removePred(kvp))
                    continue;

                if (toRemove == null)
                    toRemove = new List<T>();

                toRemove.Add(kvp.Key);
            }

            if (toRemove != null)
            {
                // Remove the items
                foreach (var key in toRemove)
                {
                    var b = dict.Remove(key);
                    Debug.Assert(b, "How did we fail to remove an item? Hmm... not good.");
                }

                return toRemove;
            }
            else
                return Enumerable.Empty<T>();
        }
    }
}