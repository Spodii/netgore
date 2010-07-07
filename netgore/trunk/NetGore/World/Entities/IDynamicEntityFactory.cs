using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.World
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
        /// <param name="compact">Whether or not the <see cref="DynamicEntity"/> is to be stored in a way that is optimized
        /// for size. The compact format is not guaranteed to remain stable. Because of this, the compact format should
        /// never be used for persistent storage. It is recommended to only use the compact format in network IO.
        /// The <paramref name="compact"/> value must be the same when reading and writing. That is, you cannot write
        /// with <paramref name="compact"/> set to true, then read back with it set to false, or vise versa.</param>
        /// <returns>The <see cref="DynamicEntity"/> created from the <paramref name="reader"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        DynamicEntity Read(IValueReader reader, bool compact = false);

        /// <summary>
        /// Writes a <see cref="DynamicEntity"/> to a stream.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write the <see cref="DynamicEntity"/> to.</param>
        /// <param name="dEntity"><see cref="DynamicEntity"/> to write to the stream.</param>
        /// <param name="compact">Whether or not the <see cref="DynamicEntity"/> is to be stored in a way that is optimized
        /// for size. The compact format is not guaranteed to remain stable. Because of this, the compact format should
        /// never be used for persistent storage. It is recommended to only use the compact format in network IO.
        /// The <paramref name="compact"/> value must be the same when reading and writing. That is, you cannot write
        /// with <paramref name="compact"/> set to true, then read back with it set to false, or vise versa.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dEntity"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="dEntity"/> is not of a valid that <see cref="Type"/>
        /// that is supported by this factory.</exception>
        void Write(IValueWriter writer, DynamicEntity dEntity, bool compact = false);
    }
}