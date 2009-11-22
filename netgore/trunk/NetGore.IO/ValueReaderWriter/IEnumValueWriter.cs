using System;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for a class that can write enum values to an <see cref="IValueWriter"/>.
    /// </summary>
    /// <typeparam name="T">The Type of Enum.</typeparam>
    public interface IEnumValueWriter<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        void WriteEnum(IValueWriter writer, string name, T value);
    }
}