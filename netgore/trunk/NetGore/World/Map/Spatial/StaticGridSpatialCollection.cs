using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
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
        /// When overridden in the derived class, creates a new <see cref="IGridSpatialCollectionSegment"/>.
        /// </summary>
        /// <returns>The new <see cref="IGridSpatialCollectionSegment"/> instance.</returns>
        protected override IGridSpatialCollectionSegment CreateSegment()
        {
            return new CollectionSegment();
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

        /// <summary>
        /// An implementation of the <see cref="IGridSpatialCollectionSegment"/> for the
        /// <see cref="StaticGridSpatialCollection"/> that focuses on a small footprint.
        /// </summary>
        class CollectionSegment : IGridSpatialCollectionSegment
        {
            ISpatial[] _spatials;

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<ISpatial> GetEnumerator()
            {
                if (_spatials != null)
                {
                    foreach (var spatial in _spatials)
                        yield return spatial;
                }
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
            /// Adds the <see cref="ISpatial"/> to the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
            public void Add(ISpatial spatial)
            {
                if (_spatials == null)
                {
                    _spatials = new ISpatial[1];
                }
                else
                {
                    Array.Resize(ref _spatials, _spatials.Length + 1);
                }

                _spatials[_spatials.Length - 1] = spatial;
            }

            /// <summary>
            /// Remove the <see cref="ISpatial"/> from the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
            public void Remove(ISpatial spatial)
            {
                if (_spatials == null)
                    return;

                if (_spatials.Length == 1)
                {
                    if (_spatials[0] == spatial)
                    {
                        _spatials = null;
                        return;
                    }
                }
                else
                {
                    _spatials = _spatials.Where(x => x != spatial).ToArray();
                }
            }

            /// <summary>
            /// Clears all <see cref="ISpatial"/>s from the segment.
            /// </summary>
            public void Clear()
            {
                _spatials = null;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticGridSpatialCollection"/> class.
        /// </summary>
        public StaticGridSpatialCollection()
        {
        }
    }
}