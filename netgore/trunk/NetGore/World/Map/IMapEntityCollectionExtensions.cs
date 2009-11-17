using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="IMapEntityCollection"/> interface.
    /// </summary>
    public static class IMapEntityCollectionExtensions
    {
        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public static bool ContainsEntities(this IMapEntityCollection c, CollisionBox cb)
        {
            return c.ContainsEntities(cb.ToRectangle());
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public static bool ContainsEntities<T>(this IMapEntityCollection c, CollisionBox cb) where T : Entity
        {
            return c.ContainsEntities<T>(cb.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<Entity> GetEntities(this IMapEntityCollection c, CollisionBox cb, Predicate<Entity> condition)
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<Entity> GetEntities(this IMapEntityCollection c, CollisionBox cb)
        {
            return c.GetEntities(cb.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IMapEntityCollection c, CollisionBox cb, Predicate<T> condition)
            where T : Entity
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IMapEntityCollection c, CollisionBox cb) where T : Entity
        {
            return c.GetEntities<T>(cb.ToRectangle());
        }
    }
}