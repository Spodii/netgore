using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for Enums.
    /// </summary>
    public static class EnumHelper
    {
        static MethodAccessException CreateGenericTypeIsNotEnumException<T>()
        {
            const string errmsg = "Type parameter T ({0}) must be an Enum.";
            return new MethodAccessException(string.Format(errmsg, typeof(T)));
        }

        /// <summary>
        /// Gets all of the values in an Enum of the given Type.
        /// </summary>
        /// <typeparam name="T">The Type of the Enum to get the values for.</typeparam>
        /// <returns>An array containing all of the values in an Enum of Type <typeparamref name="T"/>.</returns>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/> is not an Enum.</exception>
        public static T[] GetValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException<T>();

            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        public static bool IsDefined<T>(T value) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException<T>();

            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static T Parse<T>(string value) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException<T>();

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
        public static T Parse<T>(string value, bool ignoreCase) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException<T>();

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}