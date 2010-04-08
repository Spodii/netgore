using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Extensions for multiple numeric types to clamp the value between a range.
    /// </summary>
    public static class ClampExtensions
    {
        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static int Clamp(this int num, int min, int max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static uint Clamp(this uint num, uint min, uint max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static short Clamp(this short num, short min, short max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static ushort Clamp(this ushort num, ushort min, ushort max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static byte Clamp(this byte num, byte min, byte max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static sbyte Clamp(this sbyte num, sbyte min, sbyte max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static float Clamp(this float num, float min, float max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static double Clamp(this double num, double min, double max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static decimal Clamp(this decimal num, decimal min, decimal max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static long Clamp(this long num, long min, long max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }

        /// <summary>
        /// Clamps a number into a specified range of values.
        /// </summary>
        /// <param name="num">Number to clamp.</param>
        /// <param name="min">Minimum allowed value.</param>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>If <paramref name="num"/> is less than <paramref name="min"/>, <paramref name="min"/>
        /// is returned. If <paramref name="num"/> is greater than <paramref name="max"/>, <paramref name="max"/>
        /// is returned. Else, the original value of <paramref name="num"/> is returned.</returns>
        public static ulong Clamp(this ulong num, ulong min, ulong max)
        {
            if (num < min)
                return min;
            if (num > max)
                return max;
            return num;
        }
    }
}