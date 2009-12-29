using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Manages multiple <see cref="ISpatialCollection"/>s internally while making the internal
    /// <see cref="ISpatialCollection"/>s completely transparent externally.
    /// </summary>
    public class SpatialManager : ISpatialCollection
    {
        readonly ISpatialCollection _defaultCollection = new LinearSpatialCollection();

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            foreach (var spatialCollection in GetSpatialCollections())
                spatialCollection.SetAreaSize(size);
        }

        /// <summary>
        /// Adds multiple spatials to the spatial collection.
        /// </summary>
        /// <param name="spatials">The spatials to add.</param>
        public void Add(IEnumerable<ISpatial> spatials)
        {
            foreach (var spatial in spatials)
                Add(spatial);
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the spatial collection.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/>.</typeparam>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        public void Add<T>(IEnumerable<T> spatials) where T : class, ISpatial
        {
            foreach (var spatial in spatials)
                Add(spatial);
        }

        /// <summary>
        /// Adds a single <see cref="ISpatial"/> to the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        public void Add(ISpatial spatial)
        {
            GetSpatialCollection(spatial.GetType()).Add(spatial);
        }

        /// <summary>
        /// Checks if this spatial collection contains the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>True if this spatial collection contains the given <paramref name="spatial"/>; otherwise false.</returns>
        public bool Contains(ISpatial spatial)
        {
            return GetSpatialCollection(spatial.GetType()).Contains(spatial);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).ContainsEntities<T>(point);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point)
        {
            return GetSpatialCollection(typeof(ISpatial)).ContainsEntities(point);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).ContainsEntities(point, condition);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).ContainsEntities(rect, condition);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).ContainsEntities(point, condition);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).ContainsEntities(rect);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect)
        {
            return GetSpatialCollection(typeof(ISpatial)).ContainsEntities(rect);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).ContainsEntities(rect, condition);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntities(p);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntities(rect);
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntities<T>(p);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntities<T>(rect);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntities(p, condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntities(rect, condition);
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntities(p, condition);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntities(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <param name="condition">Condition the Entities must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        public T GetEntity<T>(Rectangle rect, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntity(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        public ISpatial GetEntity(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntity(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p, Predicate<T> condition) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntity(p, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntity<T>(p);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntity(p);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntity(p, condition);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public ISpatial GetEntity(Rectangle rect)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetEntity(rect);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public T GetEntity<T>(Rectangle rect) where T : class, ISpatial
        {
            return GetSpatialCollection(typeof(T)).GetEntity<T>(rect);
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial spatial)
        {
            GetSpatialCollection(spatial.GetType()).Remove(spatial);
        }

        /// <summary>
        /// Gets the <see cref="ISpatialCollection"/> that contains all <see cref="ISpatial"/>s of the specified
        /// type. When overriding this method in a derived class, be sure to also override
        /// <see cref="SpatialManager.GetSpatialCollections"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="ISpatial"/> that the returned <see cref="ISpatialCollection"/>
        /// must contain.</param>
        /// <returns>
        /// The <see cref="ISpatialCollection"/> containing all <see cref="ISpatial"/>s of type <paramref name="type"/>.
        /// The returned <see cref="ISpatialCollection"/> is guaranteed to return all <see cref="ISpatial"/>s of type
        /// <paramref name="type"/>, but is not required contain only that type.
        /// </returns>
        protected virtual ISpatialCollection GetSpatialCollection(Type type)
        {
            // It really doesn't matter that the default we provide is the linear collection. It is the lightest-weight
            // spatial, requires no extra steps to set up, and shouldn't even get touched if people override this method
            // like they should in the first place
            return _defaultCollection;
        }

        /// <summary>
        /// Gets all of the <see cref="ISpatialCollection"/>s.
        /// </summary>
        /// <returns>All of the <see cref="ISpatialCollection"/>s.</returns>
        protected virtual IEnumerable<ISpatialCollection> GetSpatialCollections()
        {
            yield return _defaultCollection;
        }
    }
}
