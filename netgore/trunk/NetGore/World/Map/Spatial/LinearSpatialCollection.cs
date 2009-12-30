using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// A very basic, primitive <see cref="ISpatialCollection"/> that contains all objects in a single linear collection.
    /// This is mostly provided for a stable and reliable testing platform and not for real-world usage.
    /// </summary>
    public class LinearSpatialCollection : ISpatialCollection
    {
        // You may notice most methods redirect to another method. The idea here is to keep it as simple as possible
        // by having as few methods implement actual "search" code as possible

        readonly List<ISpatial> _spatials = new List<ISpatial>();

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
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
            _spatials.Add(spatial);
        }

        /// <summary>
        /// Checks if this spatial collection contains the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>True if this spatial collection contains the given <paramref name="spatial"/>; otherwise false.</returns>
        public bool Contains(ISpatial spatial)
        {
            return _spatials.Contains(spatial);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point)
        {
            return ContainsEntities<T>(point, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point)
        {
            return ContainsEntities(point, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition)
        {
            return _spatials.Any(x => x is T && x.HitTest(point) && condition((T)x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition)
        {
            return _spatials.Any(x => x is T && x.Intersect(rect) && condition((T)x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<ISpatial> condition)
        {
            return ContainsEntities<ISpatial>(point, condition);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities<T>(Rectangle rect)
        {
            return ContainsEntities<T>(rect, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect)
        {
            return ContainsEntities(rect, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return ContainsEntities<ISpatial>(rect, condition);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p)
        {
            return GetEntities<ISpatial>(p);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect)
        {
            return GetEntities<ISpatial>(rect);
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p)
        {
            return GetEntities<T>(p, x => true);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect)
        {
            return GetEntities<T>(rect, x => true);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetEntities<ISpatial>(p, condition);
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
            return _spatials.Where(x => x is T && x.Intersect(rect) && condition((T)x)).OfType<T>().ToImmutable();
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition)
        {
            return _spatials.Where(x => x is T && x.HitTest(p) && condition((T)x)).OfType<T>().ToImmutable();
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetEntities<ISpatial>(rect, condition);
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
            return GetEntities(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        public ISpatial GetEntity(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetEntity<ISpatial>(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p, Predicate<T> condition)
        {
            return GetEntities(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public T GetEntity<T>(Vector2 p)
        {
            return GetEntity<T>(p, x=> true);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p)
        {
            return GetEntity<ISpatial>(p);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetEntity<ISpatial>(p, condition);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public ISpatial GetEntity(Rectangle rect)
        {
            return GetEntity<ISpatial>(rect);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public T GetEntity<T>(Rectangle rect)
        {
            return GetEntity<T>(rect, x => true);
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial spatial)
        {
            _spatials.Remove(spatial);
        }
    }
}
