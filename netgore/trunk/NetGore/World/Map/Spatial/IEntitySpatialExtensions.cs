using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    public static class IISpatialSpatialExtensions
    {
        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities(this ISpatialCollection c, CollisionBox cb)
        {
            return c.ContainsEntities(cb.ToRectangle());
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <param name="position">The position to use instead of the position provided by the
        /// <paramref name="cb"/>.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities(this ISpatialCollection c, CollisionBox cb, Vector2 position)
        {
            return c.ContainsEntities(new Rectangle((int)position.X, (int)position.Y, (int)cb.Size.X, (int)cb.Size.Y));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public static bool ContainsEntities<T>(this ISpatialCollection c, CollisionBox cb) where T : class, ISpatial
        {
            return c.ContainsEntities<T>(cb.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetEntities(this ISpatialCollection c, CollisionBox cb, Predicate<ISpatial> condition)
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<ISpatial> GetEntities(this ISpatialCollection c, CollisionBox cb)
        {
            return c.GetEntities(cb.ToRectangle());
        }

        public static IEnumerable<ISpatial> GetEntities(this ISpatialCollection c, ISpatial spatial)
        {
            return c.GetEntities(spatial.CB);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, CollisionBox cb, Predicate<T> condition)
            where T : class, ISpatial
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, ISpatial spatial, Predicate<T> condition) where T : class, ISpatial
        {
            return c.GetEntities(spatial.CB, condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <typeparam name="T">Type of ISpatial to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, CollisionBox cb) where T : class, ISpatial
        {
            return c.GetEntities<T>(cb.ToRectangle());
        }

        public static IEnumerable<T> GetEntities<T>(this ISpatialCollection c, ISpatial spatial) where T : class, ISpatial
        {
            return c.GetEntities<T>(spatial.CB);
        }
    }
}