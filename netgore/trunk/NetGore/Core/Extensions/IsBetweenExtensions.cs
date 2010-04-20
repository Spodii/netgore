using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extensions for multiple numeric types to check if the given number is between two other numbers.
    /// </summary>
    public static class IsBetweenExtensions
    {
        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this int num, int min, int max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this uint num, uint min, uint max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this short num, short min, short max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this ushort num, ushort min, ushort max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this byte num, byte min, byte max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this sbyte num, sbyte min, sbyte max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this float num, float min, float max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this double num, double min, double max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this long num, long min, long max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this ulong num, ulong min, ulong max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this decimal num, decimal min, decimal max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Checks if a number is between two other numbers. This check is inclusive to the <paramref name="min"/>
        /// and <paramref name="max"/> numbers, so will be considered valid if <paramref name="num"/> is
        /// equal to either the <paramref name="min"/> or <paramref name="max"/> numbers.
        /// </summary>
        /// <param name="num">Number to check.</param>
        /// <param name="min">Minimum </param>
        /// <param name="max"></param>
        /// <returns>True if <paramref name="num"/> is between or equal to <paramref name="min"/> and
        /// <paramref name="max"/>, else false.</returns>
        public static bool IsBetween(this DateTime num, DateTime min, DateTime max)
        {
            if (min > max)
                Swap(ref min, ref max);
            return num >= min && num <= max;
        }

        /// <summary>
        /// Swaps two objects, so that <paramref name="a"/> will equal <paramref name="b"/> and
        /// <paramref name="b"/> will equal <paramref name="a"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to swap.</typeparam>
        /// <param name="a">First object to swap.</param>
        /// <param name="b">Second object to swap.</param>
        static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}