using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// An implementation of <see cref="ISpatialCollection"/> that uses a grid internally to reduce the test
    /// set for all the queries.
    /// </summary>
    public abstract class GridSpatialCollectionBase : ISpatialCollection
    {
        /// <summary>
        /// The size of each grid segment.
        /// </summary>
        readonly int _gridSegmentSize;

        Point _gridSize;

        IGridSpatialCollectionSegment[] _gridSegments;

        readonly SpatialMoveEventHandler _spatialMoveHandler;
        readonly SpatialResizeEventHandler _spatialResizeHandler; 

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSpatialCollectionBase"/> class.
        /// </summary>
        /// <param name="gridSegmentSize">Size of the grid segments. Must be greater than or equal to 4.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="gridSegmentSize"/> is less than 4.</exception>
        protected GridSpatialCollectionBase(int gridSegmentSize)
        {
            if (gridSegmentSize < 4)
                throw new ArgumentOutOfRangeException("gridSegmentSize");

            _spatialMoveHandler = Spatial_OnMove;
            _spatialResizeHandler = Spatial_OnResize; 

            _gridSegmentSize = gridSegmentSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSpatialCollectionBase"/> class.
        /// </summary> 
        protected GridSpatialCollectionBase() : this(256)
        {
        }

        /// <summary>
        /// Gets the 0-based size of the grid in each dimension.
        /// </summary>
        protected Point GridSize { get { return _gridSize; } }

        /// <summary>
        /// Gets the index of the grid segment for the given world position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The grid segment index.</returns>
        protected Point WorldPositionToGridSegment(Vector2 worldPosition)
        {
            return new Point((int)(worldPosition.X / _gridSegmentSize), (int)(worldPosition.Y / _gridSegmentSize));
        }

        /// <summary>
        /// Checks if a grid segment index is valid.
        /// </summary>
        /// <param name="gridIndex">The grid segment index.</param>
        /// <returns>True if the grid segment index is valid; otherwise false.</returns>
        protected bool IsLegalGridSegment(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.Y >= 0 && gridIndex.X < GridSize.X && gridIndex.Y < GridSize.Y;
        }

        /// <summary>
        /// Gets the grid segment for the specified grid index.
        /// </summary>
        /// <param name="gridIndex">The grid segment index.</param>
        /// <returns>The grid segment for the specified grid index.</returns>
        /// <exception cref="IndexOutOfRangeException">The <paramref name="gridIndex"/> is invalid.</exception>
        protected IGridSpatialCollectionSegment GetSegment(Point gridIndex)
        {
            return _gridSegments[gridIndex.X + (gridIndex.Y * GridSize.X)];
        }

        /// <summary>
        /// Gets the grid segments for the specified grid index range. Only segments in range are returned, and specifying
        /// values out of range does not throw an exception.
        /// </summary>
        /// <param name="startIndex">The indices to start at.</param>
        /// <param name="length">The number of indices to grab in each direction.</param>
        /// <returns>The grid segments for the specified grid index range.</returns>
        protected IEnumerable<IGridSpatialCollectionSegment> GetSegments(Point startIndex, Point length)
        {
            int maxX = startIndex.X + length.X;
            int maxY = startIndex.Y + length.Y;

            maxX = Math.Max(maxX, GridSize.X - 1);
            maxY = Math.Max(maxY, GridSize.Y - 1);

            for (int x = startIndex.X; x <= maxX; x++)
            {
                for (int y = startIndex.Y; y <= maxY; y++)
                {
                    yield return GetSegment(new Point(x, y));
                }
            }
        }

        /// <summary>
        /// Gets the grid segments for the specified world area. Only segments in range are returned, and specifying
        /// values out of range does not throw an exception.
        /// </summary>
        /// <param name="worldArea">The world area.</param>
        /// <returns>The grid segments for the specified world area.</returns>
        protected IEnumerable<IGridSpatialCollectionSegment> GetSegments(Rectangle worldArea)
        {
            Point min = WorldPositionToGridSegment(new Vector2(worldArea.X, worldArea.Y));
            Point max = WorldPositionToGridSegment(new Vector2(worldArea.Right, worldArea.Bottom));
            Point len = new Point(max.X - min.X, max.Y - min.Y);
            return GetSegments(min, len);
        }

        /// <summary>
        /// Adds multiple <see cref="ISpatial"/>s to the spatial collection.
        /// </summary>
        /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
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
            Debug.Assert(!Contains(spatial), "The spatial was already in this spatial collection!");

            // Add the spatial to the segments
            foreach (var segment in GetSegments(spatial))
                segment.Add(spatial);

            // Hook a listener for movement and resizing
            spatial.OnMove += _spatialMoveHandler;
            spatial.OnResize += _spatialResizeHandler;
        }

        /// <summary>
        /// Handles when a <see cref="ISpatial"/> in this <see cref="ISpatialCollection"/> moves.
        /// </summary>
        /// <param name="sender">The <see cref="ISpatial"/> that moved.</param>
        /// <param name="oldPosition">The old position.</param>
        void Spatial_OnMove(ISpatial sender, Vector2 oldPosition)
        {
            // Get the grid index for the last and current positions to see if the segments have changed
            var minSegment = WorldPositionToGridSegment(sender.Position);
            var oldMinSegment = WorldPositionToGridSegment(sender.Position);

            if (minSegment == oldMinSegment)
                return;

            // FUTURE: Can improve performance by only removing from and adding to the appropriate changed segments

            // The position did change, so we have to remove the spatial from the old segments and add to the new
            foreach (var segment in GetSegments(sender, oldPosition))
                segment.Remove(sender);

            Debug.Assert(!Contains(sender), "spatial was not completely removed from the grid!");

            // Add the spatial back using the new positions
            foreach (var segment in GetSegments(sender))
                segment.Add(sender);
        }

        /// <summary>
        /// Handles when a <see cref="ISpatial"/> in this <see cref="ISpatialCollection"/> resizes.
        /// </summary>
        /// <param name="sender">The <see cref="ISpatial"/> that resized.</param>
        /// <param name="oldSize">The old size.</param>
        void Spatial_OnResize(ISpatial sender, Vector2 oldSize)
        {
            // Get the grid index for the last and current max positions to see if the segments have changed
            var maxSegment = WorldPositionToGridSegment(sender.Position + sender.Size);
            var oldMaxSegment = WorldPositionToGridSegment(sender.Position + oldSize);

            if (maxSegment == oldMaxSegment)
                return;

            // FUTURE: Can improve performance by only removing from and adding to the appropriate changed segments

            // The position did change, so we have to remove the spatial from the old segments and add to the new
            var startIndex = WorldPositionToGridSegment(sender.Position);
            var length = WorldPositionToGridSegment(sender.Position + oldSize);
            foreach (var segment in GetSegments(startIndex, length))
                segment.Remove(sender);

            Debug.Assert(!Contains(sender), "spatial was not completely removed from the grid!");

            // Add the spatial back using the new positions
            foreach (var segment in GetSegments(sender))
                segment.Add(sender);
        }

        /// <summary>
        /// Gets the grid segments that the <paramref name="spatial"/> occupies.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the segments for.</param>
        /// <returns>The grid segments that the <paramref name="spatial"/> occupies.</returns>
        protected IEnumerable<IGridSpatialCollectionSegment> GetSegments(ISpatial spatial)
        {
            return GetSegments(spatial, spatial.Position);
        }

        /// <summary>
        /// Gets the grid segments that the <paramref name="spatial"/> occupies.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the segments for.</param>
        /// <param name="position">The position to treat the <see cref="spatial"/> as being at.</param>
        /// <returns>The grid segments that the <paramref name="spatial"/> occupies.</returns>
        protected IEnumerable<IGridSpatialCollectionSegment> GetSegments(ISpatial spatial, Vector2 position)
        {
            var min = WorldPositionToGridSegment(position);
            var max = WorldPositionToGridSegment(position + spatial.Size);
            var length = new Point(max.X - min.X, max.Y - min.Y);

            return GetSegments(min, length);
        }

        /// <summary>
        /// Checks if this spatial collection contains the given <paramref name="spatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to look for.</param>
        /// <returns>True if this spatial collection contains the given <paramref name="spatial"/>; otherwise false.</returns>
        public bool Contains(ISpatial spatial)
        {
            // First check a segment we know the spatial would occupy for a fast positive
            var gridIndex = WorldPositionToGridSegment(spatial.Position);
            if (IsLegalGridSegment(gridIndex))
            {
                var segment = GetSegment(gridIndex);
                if (segment.Contains(spatial))
                    return true;
            }

            // They weren't in the segment, or the segment was invalid, so just scan the whole grid
            return _gridSegments.Any(x => x.Contains(spatial));
        }

        /// <summary>
        /// Tries to get a <see cref="IGridSpatialCollectionSegment"/> for the given world position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The <see cref="IGridSpatialCollectionSegment"/> for the given world position, or null if the
        /// world position was not valid.</returns>
        protected IGridSpatialCollectionSegment TryGetSegment(Vector2 worldPosition)
        {
            var gridIndex = WorldPositionToGridSegment(worldPosition);
            if (!IsLegalGridSegment(gridIndex))
                return null;

            return GetSegment(gridIndex);
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            return segment.Any(x => x is T && x.HitTest(point));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point)
        {
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            return segment.Any(x => x.HitTest(point));
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            return segment.Any(x => x is T && x.HitTest(point) && condition((T)x));
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
            var segments = GetSegments(rect);
            return segments.Any(seg => seg.Any(x => x is T && x.Intersect(rect) && condition((T)x)));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="point">The map point to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Vector2 point, Predicate<ISpatial> condition)
        {
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            return segment.Any(x => x.HitTest(point) && condition(x));
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
            var segments = GetSegments(rect);
            return segments.Any(seg => seg.Any(x => x is T && x.Intersect(rect)));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect)
        {
            var segments = GetSegments(rect);
            return segments.Any(seg => seg.Any(x => x.Intersect(rect)));
        }

        /// <summary>
        /// Gets if the specified area or location contains any spatials.
        /// </summary>
        /// <param name="rect">The map area to check.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool ContainsEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            var segments = GetSegments(rect);
            return segments.Any(seg => seg.Any(x => x.Intersect(rect) && condition(x)));
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<ISpatial>();

            return AsImmutable(segment.Where(x => x.HitTest(p)));
        }

        /// <summary>
        /// Gets a distinct and immutable version of the <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="values">The values to get the distinct and immutable copy of.</param>
        /// <returns>The distinct and immutable copy of the <paramref name="values"/>.</returns>
        protected IEnumerable<T> AsDistinctAndImmutable<T>(IEnumerable<T> values)
        {
            return AsImmutable(values.Distinct());
        }

        /// <summary>
        /// Gets an immutable version of the <paramref name="values"/>. Provided as virtual for specialized derived
        /// classes that do not need immutable values since it can guarantee the underlying collection won't change.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="values">The values to get the distinct and immutable copy of.</param>
        /// <returns>The immutable copy of the <paramref name="values"/>.</returns>
        protected virtual IEnumerable<T> AsImmutable<T>(IEnumerable<T> values)
        {
            return values.ToImmutable();
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect)
        {
            var segments = GetSegments(rect);
            return AsDistinctAndImmutable(segments.SelectMany(seg => seg.Where(x => x.Intersect(rect))));
        }

        /// <summary>
        /// Gets all spatials at the given point.
        /// </summary>
        /// <param name="p">The point to find the spatials at.</param>
        /// <typeparam name="T">The type of <see cref="ISpatial"/> to look for.</typeparam>
        /// <returns>All spatials containing the given point that are of the given type.</returns>
        public IEnumerable<T> GetEntities<T>(Vector2 p)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<T>();

            return AsImmutable(segment.Where(x => x.HitTest(p)).OfType<T>());
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <typeparam name="T">Type of ISpatial to look for.</typeparam>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<T> GetEntities<T>(Rectangle rect)
        {
            var segments = GetSegments(rect);
            return AsDistinctAndImmutable(segments.SelectMany(seg => seg.Where(x => x.Intersect(rect)).OfType<T>()));
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetEntities(Vector2 p, Predicate<ISpatial> condition)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<ISpatial>();

            return AsImmutable(segment.Where(x => x.HitTest(p) && condition(x)));
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
            var segments = GetSegments(rect);
            return AsDistinctAndImmutable(segments.SelectMany(seg => seg.Where(x => x.Intersect(rect)).OfType<T>().Where(x => condition(x))));
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<T>();

            return AsImmutable(segment.Where(x => x.HitTest(p)).OfType<T>().Where(x => condition(x)));
        }

        /// <summary>
        /// Gets the Entities found intersecting the given region.
        /// </summary>
        /// <param name="rect">Region to check for Entities.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All Entities found intersecting the given region.</returns>
        public IEnumerable<ISpatial> GetEntities(Rectangle rect, Predicate<ISpatial> condition)
        {
            var segments = GetSegments(rect);
            return AsDistinctAndImmutable(segments.SelectMany(seg => seg.Where(x => x.Intersect(rect) && condition(x))));
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment)
                {
                    if (spatial is T && spatial.Intersect(rect) && condition((T)spatial))
                        return (T)spatial;
                }
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment)
                {
                    if (spatial.Intersect(rect) && condition(spatial))
                        return spatial;
                }
            }

            return null;
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return default(T);

            foreach (var spatial in segment)
            {
                if (spatial is T && spatial.HitTest(p) && condition((T)spatial))
                    return (T)spatial;
            }

            return default(T);
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return default(T);

            foreach (var spatial in segment)
            {
                if (spatial is T && spatial.HitTest(p))
                    return (T)spatial;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return null;

            return segment.FirstOrDefault(x => x.HitTest(p));
        }

        /// <summary>
        /// Gets the first <see cref="ISpatial"/> found at the given point.
        /// </summary>
        /// <param name="p">Point to find the spatial at.</param>
        /// <param name="condition">Condition the <see cref="ISpatial"/> must meet.</param>
        /// <returns>First <see cref="ISpatial"/> found at the given point, or null if none found.</returns>
        public ISpatial GetEntity(Vector2 p, Predicate<ISpatial> condition)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return null;

            return segment.FirstOrDefault(x => x.HitTest(p) && condition(x));
        }

        /// <summary>
        /// Gets the first ISpatial found in the given region
        /// </summary>
        /// <param name="rect">Region to check for the ISpatial</param>
        /// <returns>First ISpatial found at the given point, or null if none found</returns>
        public ISpatial GetEntity(Rectangle rect)
        {
            var segments = GetSegments(rect);
            foreach (var segment in segments)
            {
                foreach (var spatial in segment)
                {
                    if (spatial.Intersect(rect))
                        return spatial;
                }
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
            var segments = GetSegments(rect);
            foreach (var segment in segments)
            {
                foreach (var spatial in segment)
                {
                    if (spatial is T && spatial.Intersect(rect))
                        return (T)spatial;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial spatial)
        {
            spatial.OnMove -= _spatialMoveHandler;
            spatial.OnResize -= _spatialResizeHandler;

            // Remove the spatial from the segments
            foreach (var segment in GetSegments(spatial))
                segment.Remove(spatial);

            Debug.Assert(!Contains(spatial), "Didn't fully and completely remove the spatial from all segments...");
        }

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            Point newSize = WorldPositionToGridSegment(size);

            // Don't rebuild the grid of the size didn't change at all
            if (newSize == GridSize)
                return;

            // Grab all the spatials in the grid
            IEnumerable<ISpatial> spatials;
            if (_gridSegments != null)
                spatials = _gridSegments.SelectMany(x => x).Distinct();
            else
                spatials = Enumerable.Empty<ISpatial>();

            // Grab the old segments so we can reuse as many as possible
            var oldSegments = new Stack<IGridSpatialCollectionSegment>(_gridSegments ?? Enumerable.Empty< IGridSpatialCollectionSegment>());

            // Set the new grid
            _gridSize = newSize;
            _gridSegments = new IGridSpatialCollectionSegment[(newSize.X + 1) * (newSize.Y + 1)];

            // Instantiate the segments
            for (int i = 0; i < _gridSegments.Length; i++)
            {
                IGridSpatialCollectionSegment segment;

                if (oldSegments.Count > 0)
                {
                    segment = oldSegments.Pop();
                    segment.Clear();
                }
                else
                {
                    segment = CreateSegment();
                }

                _gridSegments[i] = segment;
            }

            // Re-add all spatials to the grid
            foreach (var spatial in spatials)
            {
                foreach (var segment in GetSegments(spatial))
                    segment.Add(spatial);
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates a new <see cref="IGridSpatialCollectionSegment"/>.
        /// </summary>
        /// <returns>The new <see cref="IGridSpatialCollectionSegment"/> instance.</returns>
        protected abstract IGridSpatialCollectionSegment CreateSegment();
    }
}