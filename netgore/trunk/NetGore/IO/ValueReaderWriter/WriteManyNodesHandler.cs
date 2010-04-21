using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Delegate for writing many nodes with the <see cref="IValueWriter"/>.
    /// </summary>
    /// <typeparam name="T">The Type of node.</typeparam>
    /// <param name="w">IValueWriter to write to.</param>
    /// <param name="item">The item to write.</param>
    public delegate void WriteManyNodesHandler<in T>(IValueWriter w, T item);
}