using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
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
        public SpatialManager(IEnumerable<Type> spatialCollectionTypes, Func<ISpatialCollection> createSpatialCollection)
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

            /*
            // Cache the ISpatialCollection for each Type on the first request since the tree will never change, and
            // this will allow us to avoid a lot of Type-testing and tree crawling
            ISpatialCollection spatialCollection;
            if (!_spatialCollectionCache.TryGetValue(type, out spatialCollection))
            {
                spatialCollection = ((SpatialTypeTree)_treeRoot.Find(type)).SpatialCollection;
                _spatialCollectionCache.Add(type, spatialCollection);
            }
            */
            var spatialCollection = ((SpatialTypeTree)_treeRoot.Find(type)).SpatialCollection;

            return spatialCollection;
        }

        #region ISpatialCollection Members

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            _rootSpatial.SetAreaSize(size);
        }

        /// <summary>
        /// Adds multiple spatials to the spatial collection.
        /// </summary>
        /// <param name="spatials">The spatials to add.</param>
        public void Add(IEnumerable<ISpatial> spatials)
        {
            foreach (var spatial in spatials)
            {
                Add(spatial);
            }
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the spatial collection.
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
        public bool ContainsEntities<T>(Vector2 point)
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
        public bool ContainsEntities<T>(Vector2 point, Predicate<T> condition)
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
        public bool ContainsEntities<T>(Rectangle rect, Predicate<T> condition)
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
        public bool ContainsEntities<T>(Rectangle rect)
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
        public IEnumerable<T> GetEntities<T>(Vector2 p)
        {
            return GetSpatialCollection(typeof(T)).GetEntities<T>(p);
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect)
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
        public IEnumerable<T> GetEntities<T>(Rectangle rect, Predicate<T> condition)
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
        public IEnumerable<T> GetEntities<T>(Vector2 p, Predicate<T> condition)
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
        public T GetEntity<T>(Rectangle rect, Predicate<T> condition)
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
        public T GetEntity<T>(Vector2 p, Predicate<T> condition)
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
        public T GetEntity<T>(Vector2 p)
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
        public T GetEntity<T>(Rectangle rect)
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
            public SpatialTypeTree(IEnumerable<Type> types, Func<ISpatialCollection> createSpatial) : base(types)
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

            void CreateSpatialCollection(Func<ISpatialCollection> createSpatial)
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
                    _spatialCollection = createSpatial();
                }
            }
        }
    }
}