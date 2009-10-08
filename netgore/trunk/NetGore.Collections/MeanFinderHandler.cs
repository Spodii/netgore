using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Delegate for finding the mean of an IEnumerable of values.
    /// </summary>
    /// <typeparam name="T">Type of values to work with.</typeparam>
    /// <param name="values">Array of values to find the mean of.</param>
    /// <param name="offset">Array offset to start at.</param>
    /// <param name="count">Number of elements in the array to use.</param>
    /// <returns>Mean of all the values in the <paramref name="values"/> array.</returns>
    public delegate T MeanFinderHandler<T>(T[] values, int offset, int count);
}