using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    public static class EnumHelper<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static MethodAccessException CreateGenericTypeIsNotEnumException()
        {
            const string errmsg = "Type parameter T ({0}) must be an Enum.";
            return new MethodAccessException(string.Format(errmsg, typeof(T)));
        }

        /// <summary>
        /// Returns an indication whether a constant with a specified value exists in the Enum of type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if the <paramref name="value"/> exists in the Enum of type
        /// <typeparamref name="T"/>; otherwise false.</returns>
        public static bool IsDefined(T value)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Gets the defined values in the Enum.
        /// </summary>
        public static IEnumerable<T> Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static T Parse(string value)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static T Parse(string value, bool ignoreCase)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static bool TryParse(string value, out T outValue)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            try
            {
                outValue = (T)Enum.Parse(typeof(T), value);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the <see cref="EnumHelper&lt;T&gt;"/> class.
        /// </summary>
        static EnumHelper()
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            _values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static bool TryParse(string value, bool ignoreCase, out T outValue)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            try
            {
                outValue = (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        static readonly T[] _values;
    }
}
