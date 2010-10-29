using System;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// An implementation of the <see cref="GridSpatialCollectionBase"/> that is optimized for having as little impact
    /// on performance for <see cref="ISpatial"/>s that move and resize.
    /// </summary>
    public class DynamicGridSpatialCollection : GridSpatialCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicGridSpatialCollection"/> class.
        /// </summary>
        /// <param name="gridSegmentSize">Size of the grid segments. Must be greater than or equal to 4.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="gridSegmentSize"/> is less than 4.</exception>
        public DynamicGridSpatialCollection(int gridSegmentSize) : base(gridSegmentSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicGridSpatialCollection"/> class.
        /// </summary>
        public DynamicGridSpatialCollection()
        {
        }
    }
}