using System;
using System.Linq;
using NetGore;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for a class that can read enum values from an <see cref="IValueReader"/>.
    /// </summary>
    /// <typeparam name="T">The Type of Enum.</typeparam>
    public interface IEnumValueReader<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/> with the given <paramref name="name"/>.</returns>
        T ReadEnum(IValueReader reader, string name);
    }
}