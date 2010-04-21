using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Delegate for writing many values with the <see cref="IValueWriter"/>.
    /// </summary>
    /// <typeparam name="T">The Type of value.</typeparam>
    /// <param name="name">The unique name of the value.</param>
    /// <param name="value">The value to write.</param>
    public delegate void WriteManyHandler<in T>(string name, T value);
}