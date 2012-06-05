using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Delegate for reading many items with the <see cref="IValueReader"/>.
    /// </summary>
    /// <typeparam name="T">The Type of value.</typeparam>
    /// <param name="r">IValueReader to read from.</param>
    /// <param name="name">The item to read.</param>
    /// <returns>The value read from the IValueReader <paramref name="r"/>.</returns>
    public delegate T ReadManyHandler<out T>(IValueReader r, string name);
}