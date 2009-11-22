using System;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for acquiring random values.
    /// </summary>
    public static class RandomHelper
    {
        static readonly Random Random = new Random();

        /// <summary>
        /// Chooses a random element in the given collection <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="items">The collection of items to choose a random element from.</param>
        /// <returns>A randomly chosen element in the given collection <paramref name="items"/>.</returns>
        public static T Choose<T>(T[] items)
        {
            int index = NextInt(items.Length);
            return items[index];
        }

        /// <summary>
        /// Gets a random boolean value.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        public static bool NextBool()
        {
            return NextInt(2) == 1;
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random float between 0.0 and 1.0.</returns>
        public static float NextFloat()
        {
            return (float)Random.NextDouble();
        }

        /// <summary>
        /// Returns a random float betwen 0.0 and the specified <paramref name="maximum"/>.
        /// </summary>
        /// <param name="maximum">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random float betwen 0.0 and the specified <paramref name="maximum"/>.</returns>
        public static float NextFloat(float maximum)
        {
            return maximum * NextFloat();
        }

        /// <summary>
        /// Returns a random float within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random float within the specified range.</returns>
        public static float NextFloat(float min, float max)
        {
            return ((max - min) * NextFloat()) + min;
        }

        /// <summary>
        /// Gets a non-negative whole number.
        /// </summary>
        /// <returns>A non-negative whole number.</returns>
        public static int NextInt()
        {
            return Random.Next();
        }

        /// <summary>
        /// Gets a non-negetive random whole number less than the specified <paramref cref="maximum"/>.
        /// </summary>
        /// <param name="maximum">The exclusive upper bound the random number to be generated.</param>
        /// <returns>A non-negetive random whole number less than the specified <paramref cref="maximum"/>.</returns>
        public static int NextInt(int maximum)
        {
            return Random.Next(maximum);
        }

        /// <summary>
        /// Gets a random number within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.</param>
        /// <returns>A random number within a specified range.</returns>
        public static int NextInt(int min, int max)
        {
            return Random.Next(min, max);
        }
    }
}