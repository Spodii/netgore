using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
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

        #region ISpatialCollection Members

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        public void Add(IEnumerable<ISpatial> spatials)
        {
            foreach (var spatial in spatials)
            {
                Add(spatial);
            }
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/>.</typeparam>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        public void Add<T>(IEnumerable<T> spatials) where T : class, ISpatial
        {
            foreach (var spatial in spatials)
            {
                Add(spatial);
            }
        }

        /// <summary>
        /// Adds a single <see cref="ISpatial"/> to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        public void Add(ISpatial spatial)
        {
            _spatials.Add(spatial);
        }

        /// <summary>
        /// Clears out all objects in this ISpatialCollection.
        /// </summary>
        public void Clear()
        {
            foreach (var x in _spatials.ToArray())
                Remove(x);
        }

        /// <summary>
        /// Checks if this <see cref="ISpatialCollection"/> contains the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>
        /// True if this <see cref="ISpatialCollection"/> contains the given <paramref name="spatial"/>;
        /// otherwise false.
        /// </returns>
        public bool CollectionContains(ISpatial spatial)
        {
            return _spatials.Contains(spatial);
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="point">The map point to check.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool Contains<T>(Vector2 point)
        {
            return Contains<T>(point, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains(Vector2 point)
        {
            return Contains(point, x => true);
        }

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool Contains<T>(Predicate<T> condition)
        {
            return _spatials.OfType<T>().Any(x => condition(x));
        }

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool Contains(Predicate<ISpatial> condition)
        {
            return _spatials.Any(x => condition(x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains<T>(Vector2 point, Predicate<T> condition)
        {
            return _spatials.Any(x => x is T && x.Contains(point) && condition((T)x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains<T>(Rectangle rect, Predicate<T> condition)
        {
            return _spatials.Any(x => x is T && x.Intersects(rect) && condition((T)x));
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains(Vector2 point, Predicate<ISpatial> condition)
        {
            return Contains<ISpatial>(point, condition);
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="rect">The map area to check.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool Contains<T>(Rectangle rect)
        {
            return Contains<T>(rect, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains(Rectangle rect)
        {
            return Contains(rect, x => true);
        }

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// True if the specified area or location contains any spatials; otherwise false.
        /// </returns>
        public bool Contains(Rectangle rect, Predicate<ISpatial> condition)
        {
            return Contains<ISpatial>(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>
        /// The first <see cref="ISpatial"/> found in the given region, or null if none found.
        /// </returns>
        public T Get<T>(Rectangle rect, Predicate<T> condition)
        {
            return GetMany(rect, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>
        /// The first <see cref="ISpatial"/> found in the given region, or null if none found.
        /// </returns>
        public ISpatial Get(Rectangle rect, Predicate<ISpatial> condition)
        {
            return Get<ISpatial>(rect, condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>
        /// First <see cref="ISpatial"/> found at the given point, or null if none found.
        /// </returns>
        public T Get<T>(Vector2 p, Predicate<T> condition)
        {
            return GetMany(p, condition).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>
        /// First <see cref="ISpatial"/> found at the given point, or null if none found.
        /// </returns>
        public T Get<T>(Vector2 p)
        {
            return Get<T>(p, x => true);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>
        /// First <see cref="ISpatial"/> found at the given point, or null if none found.
        /// </returns>
        public ISpatial Get(Vector2 p)
        {
            return Get<ISpatial>(p);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>
        /// First <see cref="ISpatial"/> found at the given point, or null if none found.
        /// </returns>
        public ISpatial Get(Vector2 p, Predicate<ISpatial> condition)
        {
            return Get<ISpatial>(p, condition);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>
        /// First ISpatial found at the given point, or null if none found
        /// </returns>
        public ISpatial Get(Rectangle rect)
        {
            return Get<ISpatial>(rect);
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>
        /// First ISpatial found at the given point, or null if none found
        /// </returns>
        public T Get<T>(Rectangle rect)
        {
            return Get<T>(rect, x => true);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        public T Get<T>(Predicate<T> condition)
        {
            return _spatials.OfType<T>().FirstOrDefault(x => condition(x));
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        public ISpatial Get(Predicate<ISpatial> condition)
        {
            return _spatials.FirstOrDefault(x => condition(x));
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p)
        {
            return GetMany<ISpatial>(p);
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <returns>
        /// All <see cref="ISpatial"/>s found intersecting the given region.
        /// </returns>
        public IEnumerable<ISpatial> GetMany(Rectangle rect)
        {
            return GetMany<ISpatial>(rect);
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <param name="p">The point to find the spatials at.</param>
        /// <returns>
        /// All spatials containing the given point that are of the given type.
        /// </returns>
        public IEnumerable<T> GetMany<T>(Vector2 p)
        {
            return GetMany<T>(p, x => true);
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <returns>
        /// All <see cref="ISpatial"/>s found intersecting the given region.
        /// </returns>
        public IEnumerable<T> GetMany<T>(Rectangle rect)
        {
            return GetMany<T>(rect, x => true);
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>
        /// All <see cref="ISpatial"/>s of the given type.
        /// </returns>
        public IEnumerable<T> GetMany<T>()
        {
            return _spatials.OfType<T>().ToImmutable();
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetMany<ISpatial>(p, condition);
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Predicate<ISpatial> condition)
        {
            return _spatials.Where(x => condition(x)).ToImmutable();
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<T> GetMany<T>(Predicate<T> condition)
        {
            return _spatials.OfType<T>().Where(x => condition(x)).ToImmutable();
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// All <see cref="ISpatial"/>s found intersecting the given region.
        /// </returns>
        public IEnumerable<T> GetMany<T>(Rectangle rect, Predicate<T> condition)
        {
            return _spatials.Where(x => x is T && x.Intersects(rect) && condition((T)x)).OfType<T>().ToImmutable();
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <param name="p">The point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// All spatials containing the given point that are of the given type.
        /// </returns>
        public IEnumerable<T> GetMany<T>(Vector2 p, Predicate<T> condition)
        {
            return _spatials.Where(x => x is T && x.Contains(p) && condition((T)x)).OfType<T>().ToImmutable();
        }

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>
        /// All <see cref="ISpatial"/>s found intersecting the given region.
        /// </returns>
        public IEnumerable<ISpatial> GetMany(Rectangle rect, Predicate<ISpatial> condition)
        {
            return GetMany<ISpatial>(rect, condition);
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial spatial)
        {
            _spatials.Remove(spatial);
        }

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
        }

        #endregion
    }
}