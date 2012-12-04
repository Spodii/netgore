using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// An implementation of <see cref="ISpatialCollection"/> that uses a grid internally to reduce the test
    /// set for all the queries.
    /// </summary>
    public abstract class GridSpatialCollectionBase : ISpatialCollection
    {
        /// <summary>
        /// The minimum allowed segment size allowed. A segment size below this value is considered invalid.
        /// </summary>
        public const int MinSegmentSize = 4;

        /// <summary>
        /// The default segment size to use for when the segment size is not specified.
        /// </summary>
        const int _defaultSegmentSize = 384;

        /// <summary>
        /// The size of each grid segment.
        /// </summary>
        readonly int _gridSegmentSize;

        readonly TypedEventHandler<ISpatial, EventArgs<Vector2>> _spatialMoveHandler;
        readonly TypedEventHandler<ISpatial, EventArgs<Vector2>> _spatialResizeHandler;

        IGridSpatialCollectionSegment[] _gridSegments;
        Point _gridSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSpatialCollectionBase"/> class.
        /// </summary>
        /// <param name="gridSegmentSize">Size of the grid segments. Must be greater than or equal to
        /// <see cref="GridSpatialCollectionBase.MinSegmentSize"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="gridSegmentSize"/> is less than
        /// <see cref="GridSpatialCollectionBase.MinSegmentSize"/>.</exception>
        protected GridSpatialCollectionBase(int gridSegmentSize)
        {
            if (gridSegmentSize < MinSegmentSize)
                throw new ArgumentOutOfRangeException("gridSegmentSize");

            _spatialMoveHandler = Spatial_Moved;
            _spatialResizeHandler = Spatial_Resized;

            _gridSegmentSize = gridSegmentSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSpatialCollectionBase"/> class.
        /// </summary> 
        protected GridSpatialCollectionBase() : this(_defaultSegmentSize)
        {
        }

        /// <summary>
        /// Gets the size of the grid in each dimension.
        /// </summary>
        protected Point GridSize
        {
            get { return _gridSize; }
        }

        /// <summary>
        /// Creates a new <see cref="IGridSpatialCollectionSegment"/>.
        /// </summary>
        /// <returns>The new <see cref="IGridSpatialCollectionSegment"/> instance.</returns>
        protected virtual IGridSpatialCollectionSegment CreateSegment()
        {
            return new CollectionSegment();
        }

        /// <summary>
        /// Gets the grid segment for the specified grid index.
        /// </summary>
        /// <param name="gridIndex">The grid segment index.</param>
        /// <returns>The grid segment for the specified grid index.</returns>
        /// <exception cref="IndexOutOfRangeException">The <paramref name="gridIndex"/> is invalid.</exception>
        protected IGridSpatialCollectionSegment GetSegment(Point gridIndex)
        {
            var i = gridIndex.X + (gridIndex.Y * GridSize.X);
            return _gridSegments[i];
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
            var maxX = startIndex.X + length.X + 1;
            var maxY = startIndex.Y + length.Y + 1;

            for (var y = Math.Max(0, startIndex.Y); y < Math.Min(maxY, GridSize.Y); y++)
            {
                for (var x = Math.Max(0, startIndex.X); x < Math.Min(maxX, GridSize.X); x++)
                {
                    var p = new Point(x, y);
                    var segment = GetSegment(p);
                    yield return segment;
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
            var min = WorldPositionToGridSegment(new Vector2(worldArea.X, worldArea.Y));
            var max = WorldPositionToGridSegment(new Vector2(worldArea.Right, worldArea.Bottom));
            var len = new Point(max.X - min.X, max.Y - min.Y);
            return GetSegments(min, len);
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
        /// Checks if a grid segment index is valid.
        /// </summary>
        /// <param name="gridIndex">The grid segment index.</param>
        /// <returns>True if the grid segment index is valid; otherwise false.</returns>
        protected bool IsLegalGridSegment(Point gridIndex)
        {
            return gridIndex.X >= 0 && gridIndex.Y >= 0 && gridIndex.X < GridSize.X && gridIndex.Y < GridSize.Y;
        }

        /// <summary>
        /// Handles when a <see cref="ISpatial"/> in this <see cref="ISpatialCollection"/> moves.
        /// </summary>
        /// <param name="sender">The <see cref="ISpatial"/> that moved.</param>
        /// <param name="e">The <see cref="EventArgs{Vector2}"/> instance containing the event data.</param>
        void Spatial_Moved(ISpatial sender, EventArgs<Vector2> e)
        {
            // Get the grid index for the last and current positions to see if the segments have changed
            var minSegment = WorldPositionToGridSegment(sender.Position);
            var maxSegment = WorldPositionToGridSegment(sender.Max);
            var oldMinSegment = WorldPositionToGridSegment(e.Item1);
            var oldMaxSegment = WorldPositionToGridSegment(e.Item1 + sender.Size);

            if (minSegment == oldMinSegment && maxSegment == oldMaxSegment)
                return;

            // The position did change, so remove the ISpatial from all segments
            foreach (var segment in GetSegments(sender, e.Item1))
            {
                segment.Remove(sender);
            }

            Debug.Assert(!_gridSegments.Any(x => x.Contains(sender)), "spatial was not completely removed from the grid!");

            // Add the spatial back using the new positions
            foreach (var segment in GetSegments(sender))
            {
                segment.Add(sender);
            }
        }

        /// <summary>
        /// Handles when a <see cref="ISpatial"/> in this <see cref="ISpatialCollection"/> resizes.
        /// </summary>
        /// <param name="sender">The <see cref="ISpatial"/> that resized.</param>
        /// <param name="e">The <see cref="EventArgs{Vector2}"/> instance containing the event data.</param>
        void Spatial_Resized(ISpatial sender, EventArgs<Vector2> e)
        {
            // Get the grid index for the last and current max positions to see if the segments have changed
            var maxSegment = WorldPositionToGridSegment(sender.Position + sender.Size);
            var oldMaxSegment = WorldPositionToGridSegment(sender.Position + e.Item1);

            if (maxSegment == oldMaxSegment)
                return;

            // The position did change, so we have to remove the spatial from the old segments and add to the new
            var min = WorldPositionToGridSegment(sender.Position);
            var max = WorldPositionToGridSegment(sender.Position + e.Item1);
            var length = new Point(max.X - min.X, max.Y - min.Y);

            foreach (var segment in GetSegments(min, length))
            {
                segment.Remove(sender);
            }

            Debug.Assert(!_gridSegments.Any(x => x.Contains(sender)), "spatial was not completely removed from the grid!");

            // Add the spatial back using the new positions
            foreach (var segment in GetSegments(sender))
            {
                segment.Add(sender);
            }
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
        /// Gets the index of the grid segment for the given world position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The grid segment index.</returns>
        protected Point WorldPositionToGridSegment(Vector2 worldPosition)
        {
            return new Point((int)(worldPosition.X / _gridSegmentSize), (int)(worldPosition.Y / _gridSegmentSize));
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
            Debug.Assert(!CollectionContains(spatial), "The spatial was already in this spatial collection!");

            // Add the spatial to the segments
            var segments = GetSegments(spatial);
            foreach (var segment in segments)
            {
                segment.Add(spatial);
            }

            // Hook a listener for movement and resizing
            spatial.Moved -= _spatialMoveHandler;
            spatial.Moved += _spatialMoveHandler;

            spatial.Resized -= _spatialResizeHandler;
            spatial.Resized += _spatialResizeHandler;
        }

        /// <summary>
        /// Clears out all objects in this ISpatialCollection.
        /// </summary>
        public void Clear()
        {
            if (_gridSegments == null)
                return;

            ISpatial[] items = _gridSegments.SelectMany(x => x.Items).Distinct().ToArray();
            foreach (var item in items)
                Remove(item);
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
            bool ret = false;

            // First check a segment we know the spatial would occupy for a fast positive
            var gridIndex = WorldPositionToGridSegment(spatial.Position);
            if (IsLegalGridSegment(gridIndex))
            {
                var segment = GetSegment(gridIndex);
                if (segment.Contains(spatial))
                    ret = true;
            }

            // In debug mode, make sure our check we performed above returned the correct result
            Debug.Assert(ret == _gridSegments.Any(x => x.Contains(spatial)),
                "CollectionContains() returned false when the spatial really was in the collection. May be a position updating issue...");

            return ret;
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            foreach (var x in segment.Items)
            {
                if (x is T && x.Contains(point))
                    return true;
            }

            return false;
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            foreach (var x in segment.Items)
            {
                if (x.Contains(point))
                    return true;
            }

            return false;
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
            if (_gridSegments == null)
                return false;

            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T && condition((T)spatial))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets if any of the <see cref="ISpatial"/>s match the given condition.
        /// </summary>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>True if the specified area or location contains any spatials; otherwise false.</returns>
        public bool Contains(Predicate<ISpatial> condition)
        {
            if (_gridSegments == null)
                return false;

            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (condition(spatial))
                        return true;
                }
            }

            return false;
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            foreach (var spatial in segment.Items)
            {
                if (spatial is T && spatial.Contains(point) && condition((T)spatial))
                    return true;
            }

            return false;
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T && spatial.Intersects(rect) && condition((T)spatial))
                        return true;
                }
            }

            return false;
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
            var segment = TryGetSegment(point);
            if (segment == null)
                return false;

            foreach (var spatial in segment.Items)
            {
                if (spatial.Contains(point) && condition(spatial))
                    return true;
            }

            return false;
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T && spatial.Intersects(rect))
                        return true;
                }
            }

            return false;
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect))
                        return true;
                }
            }

            return false;
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect) && condition(spatial))
                        return true;
                }
            }

            return false;
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
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T && spatial.Intersects(rect) && condition((T)spatial))
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
        /// <returns>
        /// The first <see cref="ISpatial"/> found in the given region, or null if none found.
        /// </returns>
        public ISpatial Get(Rectangle rect, Predicate<ISpatial> condition)
        {
            var segments = GetSegments(rect);

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect) && condition(spatial))
                        return spatial;
                }
            }

            return null;
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
            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items.OfType<T>())
                {
                    if (condition(spatial))
                        return spatial;
                }
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
            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (condition(spatial))
                        return spatial;
                }
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return default(T);

            foreach (var spatial in segment.Items)
            {
                if (spatial is T && spatial.Contains(p) && condition((T)spatial))
                    return (T)spatial;
            }

            return default(T);
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return default(T);

            foreach (var spatial in segment.Items)
            {
                if (spatial is T && spatial.Contains(p))
                    return (T)spatial;
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return null;

            return segment.Items.FirstOrDefault(x => x.Contains(p));
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return null;

            return segment.Items.FirstOrDefault(x => x.Contains(p) && condition(x));
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
            var segments = GetSegments(rect);
            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect))
                        return spatial;
                }
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
            var segments = GetSegments(rect);
            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T && spatial.Intersects(rect))
                        return (T)spatial;
                }
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<ISpatial>();

            var ret = new List<ISpatial>();

            foreach (var spatial in segment.Items)
            {
                if (spatial.Contains(p))
                    ret.Add(spatial);
            }

            return ret;
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
            var segments = GetSegments(rect);

            var ret = new HashSet<ISpatial>();

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect))
                        ret.Add(spatial);
                }
            }

            return ret;
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<T>();

            var ret = new List<T>();

            foreach (var spatial in segment.Items)
            {
                if (spatial is T && spatial.Contains(p))
                    ret.Add((T)spatial);
            }

            return ret;
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
            var segments = GetSegments(rect);

            var ret = new HashSet<T>();

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T)
                    {
                        var asT = (T)spatial;
                        if (spatial.Intersects(rect))
                            ret.Add(asT);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Predicate<ISpatial> condition)
        {
            var ret = new HashSet<ISpatial>();

            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (!ret.Contains(spatial) && condition(spatial))
                        ret.Add(spatial);
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets all spatials matching the given condition.
        /// </summary>
        /// <param name="condition">The condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<T> GetMany<T>(Predicate<T> condition)
        {
            var ret = new HashSet<T>();

            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T)
                    {
                        var asT = (T)spatial;
                        if (!ret.Contains(asT) && condition(asT))
                            ret.Add(asT);
                    }
                }
            }

            return ret;
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
            var ret = new HashSet<T>();

            foreach (var segment in _gridSegments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T)
                    {
                        var asT = (T)spatial;
                        ret.Add(asT);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets all spatials containing a given point.
        /// </summary>
        /// <param name="p">Point to find the spatials at.</param>
        /// <param name="condition">The additional condition an <see cref="ISpatial"/> must match to be included.</param>
        /// <returns>All of the spatials at the given point.</returns>
        public IEnumerable<ISpatial> GetMany(Vector2 p, Predicate<ISpatial> condition)
        {
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<ISpatial>();

            var ret = new List<ISpatial>();

            foreach (var spatial in segment.Items)
            {
                if (spatial.Contains(p) && condition(spatial))
                    ret.Add(spatial);
            }

            return ret;
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
            var segments = GetSegments(rect);

            var ret = new HashSet<T>();

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial is T)
                    {
                        var asT = (T)spatial;
                        if (spatial.Intersects(rect) && !ret.Contains(asT) && condition(asT))
                            ret.Add(asT);
                    }
                }
            }

            return ret;
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
            var segment = TryGetSegment(p);
            if (segment == null)
                return Enumerable.Empty<T>();

            var ret = new List<T>();

            foreach (var spatial in segment.Items)
            {
                if (spatial is T)
                {
                    var asT = (T)spatial;
                    if (spatial.Contains(p) && condition(asT))
                        ret.Add(asT);
                }
            }

            return ret;
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
            var segments = GetSegments(rect);

            var ret = new HashSet<ISpatial>();

            foreach (var segment in segments)
            {
                foreach (var spatial in segment.Items)
                {
                    if (spatial.Intersects(rect) && !ret.Contains(spatial) && condition(spatial))
                        ret.Add(spatial);
                }
            }

            return ret;
        }

        /// <summary>
        /// Removes an <see cref="ISpatial"/> from the spatial collection.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
        public void Remove(ISpatial spatial)
        {
            spatial.Moved -= _spatialMoveHandler;
            spatial.Resized -= _spatialResizeHandler;

            // Remove the spatial from the segments
            // We're removing from every segment here, when we could just remove from only the segments the object intersects. But removals are so infrequent in-game
            // that it is better to play it safe and do it this way.
            foreach (var segment in _gridSegments)
            {
                segment.Remove(spatial);
            }

            // In debug mode, make sure we got all instances of the object
            Debug.Assert(!CollectionContains(spatial),
                "Didn't fully and completely remove the spatial from all segments." +
                " It most likely failed to be removed from segments at some other time...");
        }

        /// <summary>
        /// Sets the size of the area to keep track of <see cref="ISpatial"/> objects in.
        /// </summary>
        /// <param name="size">The size of the area to keep track of <see cref="ISpatial"/> objects in.</param>
        public void SetAreaSize(Vector2 size)
        {
            var newSize = WorldPositionToGridSegment(size);
            newSize = new Point(newSize.X + 1, newSize.Y + 1);

            // Don't rebuild the grid of the size didn't change at all
            if (_gridSegments != null && newSize == GridSize)
                return;

            // Grab all the spatials in the grid
            IEnumerable<ISpatial> spatials;
            if (_gridSegments != null)
                spatials = _gridSegments.SelectMany(x => x.Items).Distinct().ToImmutable();
            else
                spatials = Enumerable.Empty<ISpatial>();

            // Grab the old segments so we can reuse as many as possible
            var oldSegments =
                new Stack<IGridSpatialCollectionSegment>(_gridSegments ?? Enumerable.Empty<IGridSpatialCollectionSegment>());

            // Set the new grid
            _gridSize = newSize;
            _gridSegments = new IGridSpatialCollectionSegment[newSize.X * newSize.Y];

            // Instantiate the segments
            for (var i = 0; i < _gridSegments.Length; i++)
            {
                IGridSpatialCollectionSegment segment;

                if (oldSegments.Count > 0)
                {
                    segment = oldSegments.Pop();
                    segment.Clear();
                }
                else
                    segment = CreateSegment();

                _gridSegments[i] = segment;
            }

            // Re-add all spatials to the grid
            foreach (var spatial in spatials)
            {
                foreach (var segment in GetSegments(spatial))
                {
                    segment.Add(spatial);
                }
            }
        }

        #endregion

        /// <summary>
        /// A straight-forward implementation of the <see cref="IGridSpatialCollectionSegment"/>.
        /// </summary>
        class CollectionSegment : IGridSpatialCollectionSegment
        {
            readonly HashSet<ISpatial> _spatials = new HashSet<ISpatial>();

            #region IGridSpatialCollectionSegment Members

            /// <summary>
            /// Adds the <see cref="ISpatial"/> to the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to add.</param>
            public void Add(ISpatial spatial)
            {
                _spatials.Add(spatial);
            }

            /// <summary>
            /// Adds multiple <see cref="ISpatial"/>s to the segment.
            /// </summary>
            /// <param name="spatials">The <see cref="ISpatial"/>s to add.</param>
            public void AddRange(IEnumerable<ISpatial> spatials)
            {
                foreach (var spatial in spatials)
                {
                    Add(spatial);
                }
            }

            /// <summary>
            /// Clears all <see cref="ISpatial"/>s from the segment.
            /// </summary>
            public void Clear()
            {
                _spatials.Clear();
            }

            /// <summary>
            /// Remove the <see cref="ISpatial"/> from the segment.
            /// </summary>
            /// <param name="spatial">The <see cref="ISpatial"/> to remove.</param>
            public void Remove(ISpatial spatial)
            {
                _spatials.Remove(spatial);
            }

            /// <summary>
            /// Gets if the given ISpatial is in this collection.
            /// </summary>
            public bool Contains(ISpatial spatial)
            {
                return _spatials.Contains(spatial);
            }

            /// <summary>
            /// Gets the ISpatials in this collection.
            /// </summary>
            public IEnumerable<ISpatial> Items { get { return _spatials; } }

            #endregion
        }
    }
}