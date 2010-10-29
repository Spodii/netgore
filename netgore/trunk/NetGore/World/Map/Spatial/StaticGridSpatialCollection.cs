using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets an immutable version of the <paramref name="values"/>. Provided as virtual for specialized derived
        /// classes that do not need immutable values since it can guarantee the underlying collection won't change.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="values">The values to get the distinct and immutable copy of.</param>
        /// <returns>The immutable copy of the <paramref name="values"/>.</returns>
        protected override IEnumerable<T> AsImmutable<T>(IEnumerable<T> values)
        {
            return values;
        }
    }
}