using System;
using System.Linq;

namespace NetGore.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Random"/> class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> to get the random value from.</param>
        /// <returns>A single-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }
    }
}