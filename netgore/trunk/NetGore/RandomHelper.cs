using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for working with randomization and accessing random values without creating
    /// a random number generator instance. It is only recommended you access random numbers through this
    /// when you only want a few numbers infrequently. If you are generating random numbers more frequently,
    /// you should create a local <see cref="Random"/> or <see cref="SafeRandom"/> instance instead.
    /// </summary>
    public static class RandomHelper
    {
        static readonly SafeRandom _rand = new SafeRandom();

        /// <summary>
        /// Chooses a random element in the given collection <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="items">The collection of items to choose a random element from.</param>
        /// <returns>A randomly chosen element in the given collection <paramref name="items"/>.</returns>
        public static T Choose<T>(params T[] items)
        {
            int index = NextInt(items.Length);
            return items[index];
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a)
        {
            return a;
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b)
        {
            int index = NextInt(2);
            switch (index)
            {
                case 0:
                    return a;
                default:
                    return b;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c)
        {
            int index = NextInt(3);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                default:
                    return c;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <param name="d">The fourth item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c, T d)
        {
            int index = NextInt(4);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                default:
                    return d;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <param name="d">The fourth item.</param>
        /// <param name="e">The fifth item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c, T d, T e)
        {
            int index = NextInt(5);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
                default:
                    return e;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <param name="d">The fourth item.</param>
        /// <param name="e">The fifth item.</param>
        /// <param name="f">The sixth item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c, T d, T e, T f)
        {
            int index = NextInt(6);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
                case 4:
                    return e;
                default:
                    return f;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <param name="d">The fourth item.</param>
        /// <param name="e">The fifth item.</param>
        /// <param name="f">The sixth item.</param>
        /// <param name="g">The seventh item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c, T d, T e, T f, T g)
        {
            int index = NextInt(7);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
                case 4:
                    return e;
                case 5:
                    return f;
                default:
                    return g;
            }
        }

        /// <summary>
        /// Chooses a random element in the given set of items.
        /// </summary>
        /// <typeparam name="T">The Type of element.</typeparam>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <param name="c">The third item.</param>
        /// <param name="d">The fourth item.</param>
        /// <param name="e">The fifth item.</param>
        /// <param name="f">The sixth item.</param>
        /// <param name="g">The seventh item.</param>
        /// <param name="h">The eigth item.</param>
        /// <returns>A randomly chosen element in the given set of items.</returns>
        public static T Choose<T>(T a, T b, T c, T d, T e, T f, T g, T h)
        {
            int index = NextInt(8);
            switch (index)
            {
                case 0:
                    return a;
                case 1:
                    return b;
                case 2:
                    return c;
                case 3:
                    return d;
                case 4:
                    return e;
                case 5:
                    return f;
                case 6:
                    return g;
                default:
                    return h;
            }
        }

        /// <summary>
        /// Gets a random boolean value.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        public static bool NextBool()
        {
            return _rand.NextBool();
        }

        /// <summary>
        /// Returns a random float between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random float between 0.0 and 1.0.</returns>
        public static float NextFloat()
        {
            return (float)_rand.NextDouble();
        }

        /// <summary>
        /// Returns a random float betwen 0.0 and the specified <paramref name="maximum"/>.
        /// </summary>
        /// <param name="maximum">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random float betwen 0.0 and the specified <paramref name="maximum"/>.</returns>
        public static float NextFloat(float maximum)
        {
            return (float)(_rand.NextDouble() * maximum);
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
            return _rand.Next();
        }

        /// <summary>
        /// Gets a non-negative whole number.
        /// </summary>
        /// <returns>A non-negative whole number.</returns>
        public static uint NextUInt()
        {
            return _rand.NextUInt();
        }

        /// <summary>
        /// Gets a non-negetive random whole number less than the specified <paramref cref="maximum"/>.
        /// </summary>
        /// <param name="maximum">The exclusive upper bound the random number to be generated.</param>
        /// <returns>A non-negetive random whole number less than the specified <paramref cref="maximum"/>.</returns>
        public static int NextInt(int maximum)
        {
            return _rand.Next(maximum);
        }

        /// <summary>
        /// Gets a random number within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned.</param>
        /// <returns>A random number within a specified range.</returns>
        public static int NextInt(int min, int max)
        {
            return _rand.Next(min, max);
        }
    }
}