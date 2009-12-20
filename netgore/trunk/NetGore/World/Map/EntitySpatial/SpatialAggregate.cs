using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Creates an aggregate of multiple <see cref="IEntitySpatial"/>s so that many spatials can be treated
    /// as just one.
    /// </summary>
    public class SpatialAggregate : IEntitySpatial
    {
        readonly bool _canIntersect;
        readonly IEnumerable<IEntitySpatial> _spatials;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialAggregate"/> class.
        /// </summary>
        /// <param name="spatials">The <see cref="IEntitySpatial"/>s to join together.</param>
        /// <param name="canIntersect">Whether or not the <paramref name="spatials"/> can intersect with one another.
        /// That is, if any single <see cref="Entity"/> can be found in more than one of the <paramref name="spatials"/>
        /// at a single time.</param>
        public SpatialAggregate(IEnumerable<IEntitySpatial> spatials, bool canIntersect)
        {
            _canIntersect = canIntersect;
            _spatials = spatials.ToCompact();
        }

        static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException("This operation is not supported by an aggregate spatial.");
        }

        IEnumerable<T> HandleIntersect<T>(IEnumerable<T> collection)
        {
            if (_canIntersect)
                return collection.Distinct();
            else
                return collection;
        }

        #region IEntitySpatial Members

        /// <summary>
        /// Adds multiple entities to the spatial collection.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public void Add(IEnumerable<Entity> entities)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Adds a single <see cref="Entity"/> to the spatial collection.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to add.</param>
        public void Add(Entity entity)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Checks if this spatial collection contains the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to look for.</param>
        /// <returns>True if this spatial collection contains the given <paramref name="entity"/>; otherwise false.</returns>
        public bool Contains(Entity entity)
        {
            return _spatials.Any(x => x.Contains(entity));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point) where T : Entity
        {
            return _spatials.Any(x => x.ContainsEntities<T>(point));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point)
        {
            return _spatials.Any(x => x.ContainsEntities(point));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition) where T : Entity
        {
            return _spatials.Any(x => x.ContainsEntities(point, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return _spatials.Any(x => x.ContainsEntities(rect, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<Entity> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(point, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to check against. All other types of
        /// <see cref="Entity"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect) where T : Entity
        {
            return _spatials.Any(x => x.ContainsEntities<T>(rect));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect)
        {
            return _spatials.Any(x => x.ContainsEntities(rect));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<Entity> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(rect, condition));
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<Entity> GetEntities(Vector2 p)
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(p)));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<Entity> GetEntities(Rectangle rect)
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(rect)));
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p) where T : Entity
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities<T>(p)));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect) where T : Entity
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities<T>(rect)));
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<Entity> GetEntities(Vector2 p, Predicate<Entity> condition)
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(p, condition)));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">Type of Entity to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(rect, condition)));
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition) where T : Entity
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(p, condition)));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="Entity"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<Entity> GetEntities(Rectangle rect, Predicate<Entity> condition)
        {
            return HandleIntersect(_spatials.SelectMany(x => x.GetEntities(rect, condition)));
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public T GetEntity<T>(Rectangle rect, Predicate<T> condition) where T : Entity
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(rect, condition);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="Entity"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="Entity"/> must meet.</param>
        /// <returns>The first <see cref="Entity"/> found in the given region, or null if none found.</returns>
        public Entity GetEntity(Rectangle rect, Predicate<Entity> condition)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(rect, condition);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p, Predicate<T> condition) where T : Entity
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(p, condition);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <typeparam name="T">The type of <see cref="Entity"/> to look for. Any other type of <see cref="Entity"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p) where T : Entity
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity<T>(p);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public Entity GetEntity(Vector2 p)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(p);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first <see cref="Entity"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="Entity"/> must meet.</param>
        /// <returns>First <see cref="Entity"/> found at the given point, or null if none found.</returns>
        public Entity GetEntity(Vector2 p, Predicate<Entity> condition)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(p, condition);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public Entity GetEntity(Rectangle rect)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(rect);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Gets the first Entity found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the Entity</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First Entity found at the given point, or null if none found</returns>
        public T GetEntity<T>(Rectangle rect) where T : Entity
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity<T>(rect);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        /// <summary>
        /// Removes an <see cref="Entity"/> from the spatial collection.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to remove.</param>
        public void Remove(Entity entity)
        {
            throw GetNotSupportedException();
        }

        #endregion
    }
}