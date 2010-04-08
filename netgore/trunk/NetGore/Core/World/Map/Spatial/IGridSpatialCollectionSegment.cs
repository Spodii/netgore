using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Interface for a segment of the <see cref="GridSpatialCollectionBase"/>.
    /// </summary>
    public interface IGridSpatialCollectionSegment : IEnumerable<ISpatial>
    {
        /// <summary>
        /// Adds the <see cref="ISpatial"/> to the segment.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        void Add(ISpatial spatial);

        /// <summary>
        /// Clears all <see cref="ISpatial"/>s from the segment.
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove the <see cref="ISpatial"/> from the segment.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        void Remove(ISpatial spatial);
    }
}