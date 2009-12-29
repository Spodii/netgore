using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Creates an aggregate of multiple <see cref="ISpatialCollection"/>s so that many spatials can be treated
    /// as just one.
    /// </summary>
    public class SpatialAggregate : ISpatialCollection
    {
        readonly IEnumerable<ISpatialCollection> _spatials;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialAggregate"/> class.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatialCollection"/>s to join together.</param>
        public SpatialAggregate(IEnumerable<ISpatialCollection> spatials)
        {
            _spatials = spatials.ToCompact();
        }

        static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException("This operation is not supported by an aggregate spatial.");
        }

        #region ISpatialCollection Members

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            foreach (var spatial in _spatials)
                spatial.SetAreaSize(size);
        }

        /// <summary>
        /// Adds multiple entities to the spatial collection.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public void Add(IEnumerable<ISpatial> entities)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the spatial collection.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/>.</typeparam>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        public void Add<T>(IEnumerable<T> spatials) where T : class, ISpatial
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Adds a single <see cref="ISpatial"/> to the spatial collection.
        /// </summary>
        /// <param name="entity">The <see cref="ISpatial"/> to add.</param>
        public void Add(ISpatial entity)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Checks if this spatial collection contains the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>True if this spatial collection contains the given <paramref name="entity"/>; otherwise false.</returns>
        public bool Contains(ISpatial entity)
        {
            return _spatials.Any(x => x.Contains(entity));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point)
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
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(point, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(rect, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<ISpatial> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(point, condition));
        }

        /// <summary>
        /// Gets if the specified area or location contains any entities.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect)
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
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any entities; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return _spatials.Any(x => x.ContainsEntities(rect, condition));
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p)
        {
            return _spatials.SelectMany(x => x.GetEntities(p));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect)
        {
            return _spatials.SelectMany(x => x.GetEntities(rect));
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p)
        {
            return _spatials.SelectMany(x => x.GetEntities<T>(p));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect)
        {
            return _spatials.SelectMany(x => x.GetEntities<T>(rect));
        }

        /// <summary>
        /// Gets all entities containing a given point.
        /// </summary>
        /// <param name="p">Point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the entities at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p, Predicate<ISpatial> condition)
        {
            return _spatials.SelectMany(x => x.GetEntities(p, condition));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition)
        {
            return _spatials.SelectMany(x => x.GetEntities(rect, condition));
        }

        /// <summary>
        /// Gets all entities at the given point.
        /// </summary>
        /// <param name="p">The point to find the entities at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All entities containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition)
        {
            return _spatials.SelectMany(x => x.GetEntities(p, condition));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return _spatials.SelectMany(x => x.GetEntities(rect, condition));
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        public T GetEntity<T>(Rectangle rect, Predicate<T> condition)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(rect, condition);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        public ISpatial GetEntity(Rectangle rect, Predicate<ISpatial> condition)
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
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p, Predicate<T> condition)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity(p, condition);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity<T>(p);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p)
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
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the entity at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p, Predicate<ISpatial> condition)
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
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public ISpatial GetEntity(Rectangle rect)
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
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public T GetEntity<T>(Rectangle rect)
        {
            foreach (var spatial in _spatials)
            {
                var ret = spatial.GetEntity<T>(rect);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="entity">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial entity)
        {
            throw GetNotSupportedException();
        }

        #endregion
    }
}