using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Creates an aggregate of multiple <see cref="ISpatialCollection"/>s so that many spatials can be treated
    /// as just one.
    /// </summary>
    public class SpatialAggregate : ISpatialCollection
    {
        readonly IEnumerable<ISpatialCollection> _spatialCollections;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialAggregate"/> class.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatialCollection"/>s to join together.</param>
        public SpatialAggregate(IEnumerable<ISpatialCollection> spatials)
        {
            // Expand any ISpatialCollections that are a SpatialAggregate to prevent the need for recursion
            var expanded = new List<ISpatialCollection>();
            expanded.AddRange(spatials.OfType<SpatialAggregate>().SelectMany(x => x._spatialCollections));
            expanded.AddRange(spatials.Where(x => !(x is SpatialAggregate)));

            _spatialCollections = expanded.Distinct().ToCompact();
        }

        /// <summary>
        /// Gets the <see cref="NotSupportedException"/> to use for when trying to use a method that is not supported.
        /// </summary>
        /// <returns>The <see cref="NotSupportedException"/> to use for when trying to use a method that is not supported.</returns>
        static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException("This operation is not supported by an aggregate spatial.");
        }

        #region ISpatialCollection Members

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        /// <exception cref="NotSupportedException">This operation is not supported by the <see cref="SpatialAggregate"/>.</exception>
        void ISpatialCollection.Add(IEnumerable<ISpatial> spatials)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/>.</typeparam>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        /// <exception cref="NotSupportedException">This operation is not supported by the <see cref="SpatialAggregate"/>.</exception>
        void ISpatialCollection.Add<T>(IEnumerable<T> spatials)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Adds a single <see cref="ISpatial"/> to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        /// <exception cref="NotSupportedException">This operation is not supported by the <see cref="SpatialAggregate"/>.</exception>
        void ISpatialCollection.Add(ISpatial spatial)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Clears out all objects in this ISpatialCollection.
        /// </summary>
        public void Clear()
        {
            if (_spatialCollections == null)
                return;

            foreach (var x in _spatialCollections)
                x.Clear();
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
            return _spatialCollections.Any(x => x.CollectionContains(spatial));
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
            return _spatialCollections.Any(x => x.Contains<T>(point));
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
            return _spatialCollections.Any(x => x.Contains(point));
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
            return _spatialCollections.Any(x => x.Contains(condition));
        }

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool Contains(Predicate<ISpatial> condition)
        {
            return _spatialCollections.Any(x => x.Contains(condition));
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
            return _spatialCollections.Any(x => x.Contains(point, condition));
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
            return _spatialCollections.Any(x => x.Contains(rect, condition));
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
            return _spatialCollections.Any(x => x.Contains(point, condition));
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
            return _spatialCollections.Any(x => x.Contains<T>(rect));
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
            return _spatialCollections.Any(x => x.Contains(rect));
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
            return _spatialCollections.Any(x => x.Contains(rect, condition));
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(rect, condition);
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
        /// <returns>
        /// The first <see cref="ISpatial"/> found in the given region, or null if none found.
        /// </returns>
        public ISpatial Get(Rectangle rect, Predicate<ISpatial> condition)
        {
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(rect, condition);
                if (ret != null)
                    return ret;
            }

            return null;
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(p, condition);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(condition);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        public ISpatial Get(Predicate<ISpatial> condition)
        {
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(condition);
                if (ret != null)
                    return ret;
            }

            return null;
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get<T>(p);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(p);
                if (ret != null)
                    return ret;
            }

            return null;
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(p, condition);
                if (ret != null)
                    return ret;
            }

            return null;
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get(rect);
                if (ret != null)
                    return ret;
            }

            return null;
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
            foreach (var spatial in _spatialCollections)
            {
                var ret = spatial.Get<T>(rect);
                if (!Equals(ret, default(T)))
                    return ret;
            }

            return default(T);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p)
        {
            return _spatialCollections.SelectMany(x => x.GetMany(p));
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
            return _spatialCollections.SelectMany(x => x.GetMany(rect));
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
            return _spatialCollections.SelectMany(x => x.GetMany<T>(p));
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
            return _spatialCollections.SelectMany(x => x.GetMany<T>(rect));
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
            return _spatialCollections.SelectMany(x => x.GetMany<T>());
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p, Predicate<ISpatial> condition)
        {
            return _spatialCollections.SelectMany(x => x.GetMany(p, condition));
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Predicate<ISpatial> condition)
        {
            return _spatialCollections.SelectMany(x => x.GetMany(condition));
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<T> GetMany<T>(Predicate<T> condition)
        {
            return _spatialCollections.SelectMany(x => x.GetMany(condition));
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
            return _spatialCollections.SelectMany(x => x.GetMany(rect, condition));
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
            return _spatialCollections.SelectMany(x => x.GetMany(p, condition));
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
            return _spatialCollections.SelectMany(x => x.GetMany(rect, condition));
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        /// <exception cref="NotSupportedException">This operation is not supported by the <see cref="SpatialAggregate"/>.</exception>
        void ISpatialCollection.Remove(ISpatial spatial)
        {
            throw GetNotSupportedException();
        }

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            foreach (var spatial in _spatialCollections)
            {
                spatial.SetAreaSize(size);
            }
        }

        #endregion
    }
}