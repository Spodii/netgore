using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="IMapEntityCollection"/> interface.
    /// </summary>
    public static class IMapEntityCollectionExtensions
    {
        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public static IEnumerable<T> GetEntities<T>(this IMapEntityCollection c, Vector2 p, Predicate<T> condition) where T : Entity
        {
            return c.GetEntities<T>(p).Where(x => condition(x));
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
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public static IEnumerable<Entity> GetEntities(this IMapEntityCollection c, Rectangle rect, Predicate<Entity> condition)
        {
            return c.GetEntities(rect).Where(x => condition(x));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb">Region to check for Entities in.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IMapEntityCollection c, CollisionBox cb, Predicate<T> condition) where T : Entity
        {
            return c.GetEntities(cb.ToRectangle(), condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <typeparam name="T">Type of Entity to convert to.</typeparam>
        /// <returns>List of all Entities found intersecting the given region.</returns>
        public static IEnumerable<T> GetEntities<T>(this IMapEntityCollection c, Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return c.GetEntities<T>(rect).Where(x => condition(x));
        }


        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public static T GetEntity<T>(this IMapEntityCollection c, Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return c.GetEntities(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public static Entity GetEntity(this IMapEntityCollection c, Rectangle rect, Predicate<Entity> condition)
        {
            return c.GetEntities(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public static T GetEntity<T>(this IMapEntityCollection c, Vector2 p, Predicate<T> condition) where T : Entity
        {
            return c.GetEntities(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="p">Point to find the entity at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public static T GetEntity<T>(this IMapEntityCollection c, Vector2 p) where T : Entity
        {
            return c.GetEntities<T>(p).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="p">Point to find the entity at.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public static Entity GetEntity(this IMapEntityCollection c, Vector2 p)
        {
            return c.GetEntities(p).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public static Entity GetEntity(this IMapEntityCollection c, Vector2 p, Predicate<Entity> condition)
        {
            return c.GetEntities(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect">Region to check for the Entity</param>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public static Entity GetEntity(this IMapEntityCollection c, Rectangle rect)
        {
            return c.GetEntities(rect).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect">Region to check for the Entity</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public static T GetEntity<T>(this IMapEntityCollection c, Rectangle rect) where T : Entity
        {
            return c.GetEntities<T>(rect).FirstOrDefault();
        }

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
    }
}