using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Interface for an object that supports searching entities on a map.
    /// </summary>
    public interface IMapEntityCollection
    {
        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities<T>(Vector2 point) where T : Entity;

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities(Vector2 point);

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities<T>(Vector2 point, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities(Vector2 point, Predicate<Entity> condition);

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities<T>(Rectangle rect) where T : Entity;

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities(Rectangle rect);

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        bool ContainsEntities(Rectangle rect, Predicate<Entity> condition);

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <returns>All of the entities at the given point.</returns>
        IEnumerable<Entity> GetEntities(Vector2 p);

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<Entity> GetEntities(Rectangle rect);

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        IEnumerable<T> GetEntities<T>(Vector2 p) where T : Entity;

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<T> GetEntities<T>(Rectangle rect) where T : Entity;

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All of the entities at the given point.</returns>
        IEnumerable<Entity> GetEntities(Vector2 p, Predicate<Entity> condition);

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        IEnumerable<Entity> GetEntities(Rectangle rect, Predicate<Entity> condition);

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        T GetEntity<T>(Rectangle rect, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        Entity GetEntity(Rectangle rect, Predicate<Entity> condition);

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        T GetEntity<T>(Vector2 p, Predicate<T> condition) where T : Entity;

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        T GetEntity<T>(Vector2 p) where T : Entity;

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        Entity GetEntity(Vector2 p);

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        Entity GetEntity(Vector2 p, Predicate<Entity> condition);

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        Entity GetEntity(Rectangle rect);

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        T GetEntity<T>(Rectangle rect) where T : Entity;
    }
}