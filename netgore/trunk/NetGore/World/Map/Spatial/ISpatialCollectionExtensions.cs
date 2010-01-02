using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="ISpatialCollection"/> interface.
    /// </summary>
    public static class ISpatialCollectionExtensions
    {
        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities(this ISpatialCollection c, ISpatial spatial)
        {
            return c.ContainsEntities(spatial.ToRectangle());
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <param name="position">The position to use instead of the position provided by the
        /// <paramref name="spatial"/>.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities(this ISpatialCollection c, ISpatial spatial, Vector2 position)
        {
            return c.ContainsEntities(new Rectangle((int)position.X, (int)position.Y, (int)spatial.Size.X, (int)spatial.Size.Y));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities<T>(this ISpatialCollection c, ISpatial spatial)
        {
            return c.ContainsEntities<T>(spatial.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetEntities(this ISpatialCollection c, ISpatial spatial, Predicate<ISpatial> condition)
        {
            return c.GetEntities(spatial.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetEntities(this ISpatialCollection c, ISpatial spatial)
        {
            return c.GetEntities(spatial.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, ISpatial spatial, Predicate<T> condition)
        {
            return c.GetEntities(spatial.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="spatial">The <see cref="ISpatial"/> representing the map area to check.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, ISpatial spatial)
        {
            return c.GetEntities<T>(spatial.ToRectangle());
        }
    }
}