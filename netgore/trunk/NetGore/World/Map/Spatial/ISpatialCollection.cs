using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Interface for a collection that keeps track and allows for queries on the spatial components of an
    /// <see cref="ISpatial"/>.
    /// </summary>
    public interface ISpatialCollection
    {
        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        void Add(IEnumerable<ISpatial> spatials);

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/>.</typeparam>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
        void Add<T>(IEnumerable<T> spatials) where T : class, ISpatial;

        /// <summary>
        /// Adds a single <see cref="ISpatial"/> to the <see cref="ISpatialCollection"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
        void Add(ISpatial spatial);

        /// <summary>
        /// Clears out all objects in this ISpatialCollection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Checks if this <see cref="ISpatialCollection"/> contains the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>True if this <see cref="ISpatialCollection"/> contains the given <paramref name="spatial"/>;
        /// otherwise false.</returns>
        bool CollectionContains(ISpatial spatial);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool Contains<T>(Vector2 point);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains(Vector2 point);

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains<T>(Predicate<T> condition);

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains(Predicate<ISpatial> condition);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains<T>(Vector2 point, Predicate<T> condition);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains<T>(Rectangle rect, Predicate<T> condition);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains(Vector2 point, Predicate<ISpatial> condition);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to check against. All other types of
        /// <see cref="ISpatial"/> will be ignored.</typeparam>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool Contains<T>(Rectangle rect);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains(Rectangle rect);

        /// <summary>
        /// Gets if the specified area or location contains any <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        bool Contains(Rectangle rect, Predicate<ISpatial> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        T Get<T>(Rectangle rect, Predicate<T> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found in the given region.
        /// </summary>
        /// <param name="rect">Region to find the <see cref="ISpatial"/> in.</param>
        /// <param name="condition">Additional condition an <see cref="ISpatial"/> must meet.</param>
        /// <returns>The first <see cref="ISpatial"/> found in the given region, or null if none found.</returns>
        ISpatial Get(Rectangle rect, Predicate<ISpatial> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        T Get<T>(Vector2 p, Predicate<T> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        T Get<T>(Predicate<T> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        ISpatial Get(Predicate<ISpatial> condition);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for. Any other type of <see cref="ISpatial"/>
        /// will be ignored.</typeparam>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        T Get<T>(Vector2 p);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        ISpatial Get(Vector2 p);

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        ISpatial Get(Vector2 p, Predicate<ISpatial> condition);

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        ISpatial Get(Rectangle rect);

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        T Get<T>(Rectangle rect);

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        IEnumerable<ISpatial> GetMany(Vector2 p);

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        IEnumerable<ISpatial> GetMany(Rectangle rect);

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        IEnumerable<T> GetMany<T>(Vector2 p);

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        IEnumerable<T> GetMany<T>(Rectangle rect);

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All <see cref="ISpatial"/>s of the given type.</returns>
        IEnumerable<T> GetMany<T>();

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        IEnumerable<ISpatial> GetMany(Vector2 p, Predicate<ISpatial> condition);

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        IEnumerable<ISpatial> GetMany(Predicate<ISpatial> condition);

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        IEnumerable<T> GetMany<T>(Predicate<T> condition);

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        IEnumerable<T> GetMany<T>(Rectangle rect, Predicate<T> condition);

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        IEnumerable<T> GetMany<T>(Vector2 p, Predicate<T> condition);

        /// <summary>
        /// Gets the <see cref="ISpatial"/>s found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for <see cref="ISpatial"/>s.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All <see cref="ISpatial"/>s found intersecting the given region.</returns>
        IEnumerable<ISpatial> GetMany(Rectangle rect, Predicate<ISpatial> condition);

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        void Remove(ISpatial spatial);

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        void SetAreaSize(Vector2 size);
    }
}