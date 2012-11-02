using System.Collections.Generic;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Interface for a segment of the <see cref="GridSpatialCollectionBase"/>.
    /// </summary>
    public interface IGridSpatialCollectionSegment
    {
        /// <summary>
        /// Adds the <see cref="ISpatial"/> to the segment.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        void Add(ISpatial spatial);

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the segment.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        void AddRange(IEnumerable<ISpatial> spatials);

        /// <summary>
        /// Clears all <see cref="ISpatial"/>s from the segment.
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove the <see cref="ISpatial"/> from the segment.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        void Remove(ISpatial spatial);

        /// <summary>
        /// Gets if the given ISpatial is in this collection.
        /// </summary>
        bool Contains(ISpatial spatial);

        /// <summary>
        /// Gets the ISpatials in this collection.
        /// </summary>
        IEnumerable<ISpatial> Items { get; }
    }
}