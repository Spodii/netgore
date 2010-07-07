using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="IGridSpatialCollectionSegment"/>.
        /// </summary>
        /// <returns>The new <see cref="IGridSpatialCollectionSegment"/> instance.</returns>
        protected override IGridSpatialCollectionSegment CreateSegment()
        {
            return new CollectionSegment();
        }

        /// <summary>
        /// An implementation of the <see cref="IGridSpatialCollectionSegment"/> for the
        /// <see cref="DynamicGridSpatialCollection"/> that focuses on fast adding and removing of a small number of values.
        /// </summary>
        class CollectionSegment : IGridSpatialCollectionSegment
        {
            readonly List<ISpatial> _spatials = new List<ISpatial>();

            #region IGridSpatialCollectionSegment Members

            /// <summary>
            /// Adds the <see cref="ISpatial"/> to the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
            public void Add(ISpatial spatial)
            {
                _spatials.Add(spatial);
            }

            /// <summary>
            /// Clears all <see cref="ISpatial"/>s from the segment.
            /// </summary>
            public void Clear()
            {
                _spatials.Clear();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<ISpatial> GetEnumerator()
            {
                return _spatials.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Remove the <see cref="ISpatial"/> from the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
            public void Remove(ISpatial spatial)
            {
                _spatials.Remove(spatial);
            }

            #endregion
        }
    }
}