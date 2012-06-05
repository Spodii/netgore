using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Delegate for reading many nodes with the <see cref="IValueReader"/>.
    /// </summary>
    /// <typeparam name="T">The Type of node.</typeparam>
    /// <param name="r">IValueReader to read from.</param>
    /// <returns>The node read from the IValueReader <paramref name="r"/>.</returns>
    public delegate T ReadManyNodesHandler<out T>(IValueReader r);
}