using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Provides support for extra types for the MeanStack.
    /// </summary>
    public static class MeanStackExtras
    {
        /// <summary>
        /// Finds the mean for a collection of values of the specified type.
        /// </summary>
        /// <param name="values">Values to find the mean of.</param>
        /// <param name="offset">Array offset to start at.</param>
        /// <param name="count">Number of elements in the array to use.</param>
        /// <returns>Mean of the values in the <paramref name="values"/> array.</returns>
        public static Vector2 Mean(Vector2[] values, int offset, int count)
        {
            if (count == 0)
                return Vector2.Zero;

            Vector2 sum = Vector2.Zero;

            for (int i = offset; i < offset + count; i++)
            {
                sum += values[i];
            }

            return sum / count;
        }
    }
}