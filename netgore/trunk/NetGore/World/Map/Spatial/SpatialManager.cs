using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Manages multiple <see cref="ISpatialCollection"/>s internally while making the internal
    /// <see cref="ISpatialCollection"/>s completely transparent externally.
    /// </summary>
    public class SpatialManager : ISpatialCollection
    {
        readonly ISpatialCollection _rootSpatial;
        readonly IDictionary<Type, ISpatialCollection> _spatialCollectionCache = new Dictionary<Type, ISpatialCollection>();
        readonly SpatialTypeTree _treeRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialManager"/> class.
        /// </summary>
        /// <param name="spatialCollectionTypes">The key <see cref="Type"/>s that will be used to build
        /// <see cref="ISpatialCollection"/>s on. The more <see cref="Type"/>s that are included, the smaller each
        /// <see cref="ISpatialCollection"/> will be which will allow for faster look-up for specific types, but
        /// slower look-up for less specific types since there will be more <see cref="ISpatialCollection"/>s to crawl. It
        /// is recommended to just include <see cref="Type"/>s that you plan to filter by.</param>
        /// <param name="createSpatialCollection">The delegate that describes how to create the
        /// <see cref="ISpatialCollection"/>s.</param>
        public SpatialManager(IEnumerable<Type> spatialCollectionTypes,
                              Func<ClassTypeTree, ISpatialCollection> createSpatialCollection)
        {
            _treeRoot = new SpatialTypeTree(spatialCollectionTypes, createSpatialCollection);
            _rootSpatial = _treeRoot.SpatialCollection;
        }

        /// <summary>
        /// Gets the <see cref="ISpatialCollection"/> that contains all <see cref="ISpatial"/>s of the specified
        /// type.
        /// </summary>
        /// <param name="type">The type of <see cref="ISpatial"/> that the returned <see cref="ISpatialCollection"/>
        /// must contain.</param>
        /// <returns>
        /// The <see cref="ISpatialCollection"/> containing all <see cref="ISpatial"/>s of type <paramref name="type"/>.
        /// The returned <see cref="ISpatialCollection"/> is guaranteed to return all <see cref="ISpatial"/>s of type
        /// <paramref name="type"/>, but is not required contain only that type.
        /// </returns>
        ISpatialCollection GetSpatialCollection(Type type)
        {
            // We know that anything that isn't a class will just return the root node anyways, so don't cache it
            if (!type.IsClass)
                return _rootSpatial;

            // Cache the ISpatialCollection for each Type on the first request since the tree will never change, and
            // this will allow us to avoid a lot of Type-testing and tree crawling
            ISpatialCollection spatialCollection;
            if (!_spatialCollectionCache.TryGetValue(type, out spatialCollection))
            {
                spatialCollection = ((SpatialTypeTree)_treeRoot.Find(type)).SpatialCollection;
                _spatialCollectionCache.Add(type, spatialCollection);
            }

            return spatialCollection;
        }

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
            GetSpatialCollection(spatial.GetType()).Add(spatial);
        }

        /// <summary>
        /// Clears out all objects in this ISpatialCollection.
        /// </summary>
        public void Clear()
        {
            if (_rootSpatial != null)
            {
                _rootSpatial.Clear();
            }

            if (_spatialCollectionCache != null)
            {
                foreach (var x in _spatialCollectionCache.Select(x => x.Value))
                    x.Clear();
            }
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
            return GetSpatialCollection(spatial.GetType()).CollectionContains(spatial);
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
            return GetSpatialCollection(typeof(T)).Contains<T>(point);
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
            return GetSpatialCollection(typeof(ISpatial)).Contains(point);
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
            return GetSpatialCollection(typeof(T)).Contains(condition);
        }

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool Contains(Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).Contains(condition);
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
            return GetSpatialCollection(typeof(T)).Contains(point, condition);
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
            return GetSpatialCollection(typeof(T)).Contains(rect, condition);
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
            return GetSpatialCollection(typeof(ISpatial)).Contains(point, condition);
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
            return GetSpatialCollection(typeof(T)).Contains(rect);
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
            return GetSpatialCollection(typeof(ISpatial)).Contains(rect);
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
            return GetSpatialCollection(typeof(ISpatial)).Contains(rect, condition);
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
            return GetSpatialCollection(typeof(T)).Get(rect, condition);
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
            return GetSpatialCollection(typeof(ISpatial)).Get(rect, condition);
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
            return GetSpatialCollection(typeof(T)).Get(p, condition);
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
            return GetSpatialCollection(typeof(T)).Get(condition);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> matching the given condition.
        /// </summary>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> matching the given condition, or null if none found.</returns>
        public ISpatial Get(Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).Get(condition);
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
            return GetSpatialCollection(typeof(T)).Get<T>(p);
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
            return GetSpatialCollection(typeof(ISpatial)).Get(p);
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
            return GetSpatialCollection(typeof(ISpatial)).Get(p, condition);
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
            return GetSpatialCollection(typeof(ISpatial)).Get(rect);
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
            return GetSpatialCollection(typeof(T)).Get<T>(rect);
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetMany(p);
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
            return GetSpatialCollection(typeof(ISpatial)).GetMany(rect);
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
            return GetSpatialCollection(typeof(T)).GetMany<T>(p);
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
            return GetSpatialCollection(typeof(T)).GetMany<T>(rect);
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
            return GetSpatialCollection(typeof(T)).GetMany<T>();
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p, Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetMany(p, condition);
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Predicate<ISpatial> condition)
        {
            return GetSpatialCollection(typeof(ISpatial)).GetMany(condition);
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<T> GetMany<T>(Predicate<T> condition)
        {
            return GetSpatialCollection(typeof(T)).GetMany(condition);
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
            return GetSpatialCollection(typeof(T)).GetMany(rect, condition);
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
            return GetSpatialCollection(typeof(T)).GetMany(p, condition);
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
            return GetSpatialCollection(typeof(ISpatial)).GetMany(rect, condition);
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
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            _rootSpatial.SetAreaSize(size);
        }

        #endregion

        /// <summary>
        /// Implementation of the <see cref="ClassTypeTree"/> that attaches an <see cref="ISpatialCollection"/> to each
        /// leaf node, and a <see cref="SpatialAggregate"/> to each non-leaf node, resulting in a collection of
        /// <see cref="ISpatialCollection"/>s that do not ever overlap and a <see cref="SpatialAggregate"/> that
        /// only includes the needed types.
        /// </summary>
        sealed class SpatialTypeTree : ClassTypeTree
        {
            ISpatialCollection _spatialCollection;

            /// <summary>
            /// Initializes a new instance of the <see cref="SpatialTypeTree"/> class.
            /// </summary>
            /// <param name="types">The types to build the tree out of.</param>
            /// <param name="createSpatial">The function used to create the <see cref="ISpatialCollection"/>
            /// for the leaf nodes.</param>
            public SpatialTypeTree(IEnumerable<Type> types, Func<ClassTypeTree, ISpatialCollection> createSpatial) : base(types)
            {
                // Only call CreateSpatialCollection() from this constructor since this will be the last constructor
                // to finish (since the tree is built recursively), and it will only be called once since this is always
                // the root, and the root constructor is only called once per tree.
                CreateSpatialCollection(createSpatial);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SpatialTypeTree"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="selfType">Type of the this node.</param>
            /// <param name="types">The possible child types.</param>
            SpatialTypeTree(ClassTypeTree parent, Type selfType, IEnumerable<Type> types) : base(parent, selfType, types)
            {
            }

            public ISpatialCollection SpatialCollection
            {
                get { return _spatialCollection; }
            }

            /// <summary>
            /// Creates a <see cref="ClassTypeTree"/> using the given values. This allows derived types of the
            /// <see cref="ClassTypeTree"/> to have their derived type be used for every node in the tree.
            /// </summary>
            /// <param name="parent">The parent node.</param>
            /// <param name="selfType">The type of this node.</param>
            /// <param name="types">All of the possible child nodes.</param>
            /// <returns>
            /// A <see cref="ClassTypeTree"/> using the given values.
            /// </returns>
            protected override ClassTypeTree CreateNode(ClassTypeTree parent, Type selfType, IEnumerable<Type> types)
            {
                return new SpatialTypeTree(parent, selfType, types);
            }

            void CreateSpatialCollection(Func<ClassTypeTree, ISpatialCollection> createSpatial)
            {
                // First, recurse all the way down to ensure the leaves are built first
                if (!IsLeaf)
                {
                    foreach (var child in Children.Cast<SpatialTypeTree>())
                    {
                        child.CreateSpatialCollection(createSpatial);
                    }

                    // Now build an aggregate to attach all the children
                    _spatialCollection = new SpatialAggregate(Children.Cast<SpatialTypeTree>().Select(x => x.SpatialCollection));
                }
                else
                {
                    // The leaf is easier to handle - just build a single spatial collection (not an aggregate)
                    _spatialCollection = createSpatial(this);
                }
            }
        }
    }
}