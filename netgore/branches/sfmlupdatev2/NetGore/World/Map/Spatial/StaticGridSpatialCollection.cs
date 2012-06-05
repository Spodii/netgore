using System;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// An implementation of the <see cref="GridSpatialCollectionBase"/> that is optimized for <see cref="ISpatial"/>s
    /// that do not move or resize, and are infrequently (if ever) added or removed.
    /// </summary>
    public class StaticGridSpatialCollection : GridSpatialCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticGridSpatialCollection"/> class.
        /// </summary>
        /// <param name="gridSegmentSize">Size of the grid segments. Must be greater than or equal to 4.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="gridSegmentSize"/> is less than 4.</exception>
        public StaticGridSpatialCollection(int gridSegmentSize) : base(gridSegmentSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticGridSpatialCollection"/> class.
        /// </summary>
        public StaticGridSpatialCollection()
        {
        }
    }
}