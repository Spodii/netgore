using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Extension methods for the <see cref="ISpatialCollection"/> interface.
    /// </summary>
    public static class ISpatialCollectionExtensions
    {
        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/>s must meet.</param>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetMany(this ISpatialCollection c, ISpatial spatial, Predicate<ISpatial> condition)
        {
            return c.GetMany(spatial.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetMany(this ISpatialCollection c, ISpatial spatial)
        {
            return c.GetMany(spatial.ToRectangle());
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/>s must meet.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all <see cref="ISpatial"/>s found intersecting the given region.</returns>
        public static IEnumerable<T> GetMany<T>(this ISpatialCollection c, ISpatial spatial, Predicate<T> condition)
        {
            return c.GetMany(spatial.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all <see cref="ISpatial"/>s found intersecting the given region.</returns>
        public static IEnumerable<T> GetMany<T>(this ISpatialCollection c, ISpatial spatial)
        {
            return c.GetMany<T>(spatial.ToRectangle());
        }
    }
}