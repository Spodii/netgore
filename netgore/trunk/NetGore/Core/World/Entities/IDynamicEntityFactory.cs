using System;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Interface for a class that can be used to serialize a <see cref="DynamicEntity"/> to and from
    /// an <see cref="IValueReader"/> and <see cref="IValueWriter"/>.
    /// </summary>
    public interface IDynamicEntityFactory
    {
        /// <summary>
        /// Reads and constructs a <see cref="DynamicEntity"/> from a stream.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the <see cref="DynamicEntity"/> from.</param>
        /// <returns>The <see cref="DynamicEntity"/> created from the <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        DynamicEntity Read(IValueReader reader);

        /// <summary>
        /// Writes a <see cref="DynamicEntity"/> to a stream.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write the <see cref="DynamicEntity"/> to.</param>
        /// <param name="dEntity"><see cref="DynamicEntity"/> to write to the stream.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dEntity"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="dEntity"/> is not of a valid that <see cref="Type"/>
        /// that is supported by this factory.</exception>
        void Write(IValueWriter writer, DynamicEntity dEntity);
    }
}