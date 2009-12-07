using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore
{
    public static class IEntitySpatialExtensions
    {
        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public static bool ContainsEntities(this IEntitySpatial c, CollisionBox cb)
        {
            return c.ContainsEntities(cb.ToRectangle());
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <param name="position">The position to use instead of the position provided by the
        /// <paramref name="cb"/>.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public static bool ContainsEntities(this IEntitySpatial c, CollisionBox cb, Vector2 position)
        {
            return c.ContainsEntities(new Rectangle((int)position.X, (int)position.Y, (int)cb.Width, (int)cb.Height));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public static bool ContainsEntities<T>(this IEntitySpatial c, CollisionBox cb) where T : Entity
        {
            return c.ContainsEntities<T>(cb.ToRectangle());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<Entity> GetEntities(this IEntitySpatial c, CollisionBox cb, Predicate<Entity> condition)
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">CollisionBox to check for Entities in.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<Entity> GetEntities(this IEntitySpatial c, CollisionBox cb)
        {
            return c.GetEntities(cb.ToRectangle());
        }

        public static IEnumerable<Entity> GetEntities(this IEntitySpatial c, Entity entity)
        {
            return c.GetEntities(entity.CB);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IEntitySpatial c, CollisionBox cb, Predicate<T> condition)
            where T : Entity
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        public static IEnumerable<T> GetEntities<T>(this IEntitySpatial c, Entity entity, Predicate<T> condition)
            where T : Entity
        {
            return c.GetEntities(entity.CB, condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IEntitySpatial"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IEntitySpatial c, CollisionBox cb) where T : Entity
        {
            return c.GetEntities<T>(cb.ToRectangle());
        }

        public static IEnumerable<T> GetEntities<T>(this IEntitySpatial c, Entity entity) where T : Entity
        {
            return c.GetEntities<T>(entity.CB);
        }
    }
}
