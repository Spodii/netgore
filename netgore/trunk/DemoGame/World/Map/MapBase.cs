using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using DemoGame.DbObjs;
using log4net;
using NetGore;
using NetGore.Audio;
using NetGore.Collections;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// The base map class.
    /// </summary>
    public abstract class MapBase : IMap, IMapTable
    {
        /// <summary>
        /// Flags used to define behavior when saving the maps.
        /// </summary>
        [Flags]
        public enum MapSaveFlags : byte
        {
            /// <summary>
            /// No flags / default behavior.
            /// </summary>
            None = 0,

            /// <summary>
            /// Do not sort collections before saving, so elements may not get saved in a predictable order. Keeps the saved map closer
            /// in similarity to the map in memory, but is less predictable.
            /// </summary>
            DoNotSort = 1 << 0,
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _dynamicEntitiesNodeName = "DynamicEntities";

        /// <summary>
        /// The number of additional pixels to search in each direction around an entity when finding
        /// a valid place to put the entity. Larger values will allow for a better chance of finding a valid position,
        /// but will allow the entity to warp farther away from their initial position.
        /// </summary>
        const int _findValidPlacementPadding = 128;

        const string _headerNodeCustomGravityKey = "CustomGravity";
        const string _headerNodeGravityKey = "Gravity";

        const string _headerNodeHasMusicKey = "HasMusic";
        const string _headerNodeIndoorsKey = "Indoors";
        const string _headerNodeMusicKey = "MusicID";
        const string _headerNodeName = "Header";
        const string _headerNodeNameKey = "Name";
        const string _headerNodeSizeKey = "Size";
        const string _headerNodeParentMapKey = "ParentMapID";
        const string _miscNodeName = "Misc";
        const string _rootNodeName = "Map";
        const string _wallsNodeName = "Walls";

        readonly List<IDelayedMapEvent> _delayedEvents = new List<IDelayedMapEvent>();

        /// <summary>
        /// Collection of DynamicEntities on this map.
        /// </summary>
        readonly ICyclingObjectArray<ushort, DynamicEntity> _dynamicEntities =
            CyclingObjectArray.CreateUsingUShortKey<DynamicEntity>();

        /// <summary>
        /// List of entities in the map
        /// </summary>
        readonly List<Entity> _entities = new List<Entity>();

        /// <summary>
        /// Interface used to get the time
        /// </summary>
        readonly IGetTime _getTime;

        readonly ISpatialCollection _spatialCollection;

        readonly List<IUpdateableEntity> _updateableEntities = new List<IUpdateableEntity>();
        Vector2? _gravity = null;

        MapID _mapID;

        /// <summary>
        /// Name of the map
        /// </summary>
        string _name = string.Empty;

        Vector2 _size = new Vector2(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBase"/> class.
        /// </summary>
        /// <param name="mapID">ID of the map.</param>
        /// <param name="getTime">Interface used to get the time.</param>
        /// <exception cref="ArgumentException"><paramref name="mapID"/> returned false for <see cref="MapBase.MapIDExists"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="getTime"/> is null.</exception>
        protected MapBase(MapID mapID, IGetTime getTime)
        {
            if (getTime == null)
                throw new ArgumentNullException("getTime");

            _getTime = getTime;
            _mapID = mapID;

            _spatialCollection = CreateSpatialManager();
        }

        /// <summary>
        /// Notifies listeners that the Map has been saved.
        /// </summary>
        public event TypedEventHandler<MapBase> Saved;

        /// <summary>
        /// Gets an IEnumerable of all the DynamicEntities on the Map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<DynamicEntity> DynamicEntities
        {
            get { return _dynamicEntities.Values; }
        }

        /// <summary>
        /// Gets or sets if this map represents the map of an area that is indoors.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        [Description("If this map represents an area that is indoors.")]
        [DefaultValue(false)]
        public bool Indoors { get; set; }

        /// <summary>
        /// Gets or sets the ID of the music to play for the map, or empty or null if there is no music.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        [Description("The ID of the music to play on the map.")]
        public MusicID? MusicID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent map.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        [Description("The ID of the parent map.")]
        public MapID ParentMapID { get; set; }

        /// <summary>
        /// Adds a <see cref="DynamicEntity"/> to the <see cref="MapBase"/>, using the pre-determined unique index.
        /// </summary>
        /// <param name="entity"><see cref="DynamicEntity"/> to add to the Map.</param>
        /// <param name="mapEntityIndex">Unique index to assign to the <see cref="DynamicEntity"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public virtual void AddDynamicEntity(DynamicEntity entity, MapEntityIndex mapEntityIndex)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Debug.Assert(!_dynamicEntities.Values.Contains(entity),
                string.Format("`{0}` is already in the DynamicEntity list!", entity));

            var existingDE = _dynamicEntities[(int)mapEntityIndex];
            if (existingDE != null)
            {
                // Entity already exists
                if (existingDE == entity)
                {
                    // But the existing is this entity, so not that big of a deal
                    const string errmsg = "DynamicEntity `{0}` has already been to MapEntityIndex `{1}`.";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, entity, mapEntityIndex);
                    Debug.Fail(string.Format(errmsg, entity, mapEntityIndex));
                    return;
                }
                else
                {
                    // The existing entity is a different entity, which is a big deal
                    const string errmsg = "A DynamicEntity ({0}) already exists at MapEntityIndex `{1}`!";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, existingDE, mapEntityIndex);
                    Debug.Fail(string.Format(errmsg, existingDE, mapEntityIndex));
                    RemoveEntity(existingDE);
                }
            }

            ((IDynamicEntitySetMapEntityIndex)entity).SetMapEntityIndex(mapEntityIndex);
            _dynamicEntities[(int)mapEntityIndex] = entity;

            AddEntityFinish(entity);
        }

        /// <summary>
        /// Adds an <see cref="Entity"/> to the map.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> to add to the map.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public virtual void AddEntity(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // Assign a DynamicEntity its unique map index
            DynamicEntity dynamicEntity;
            if ((dynamicEntity = entity as DynamicEntity) != null)
            {
                Debug.Assert(!_dynamicEntities.Values.Contains(dynamicEntity),
                    string.Format("`{0}` is already in the DynamicEntity list!", entity));

                var indexRaw = _dynamicEntities.Add(dynamicEntity);
                var index = new MapEntityIndex(indexRaw);
                ((IDynamicEntitySetMapEntityIndex)dynamicEntity).SetMapEntityIndex(index);
            }

            // Finish adding the Entity
            AddEntityFinish(entity);
        }

        void AddEntityFinish(Entity entity)
        {
            AddEntityToEntityList(entity);
            ForceEntityInMapBoundaries(entity);
        }

        /// <summary>
        /// Adds an entity to the entity list, which in turn adds the entity to the entity grid
        /// and creates the hooks to the entity's events.
        /// </summary>
        /// <param name="entity">Entity to add to the entity list</param>
        void AddEntityToEntityList(Entity entity)
        {
            // When in debug build, ensure we do not add an entity that is already added
            Debug.Assert(!_entities.Contains(entity), "entity already in the map's entity list!");

            // Add to the one-dimensional entity list
            _entities.Add(entity);

            var updateableEntity = entity as IUpdateableEntity;
            if (updateableEntity != null)
                _updateableEntities.Add(updateableEntity);

            // Check for the IUpdateableMapReference interface
            var updateableMapReference = entity as IUpdateableMapReference;
            if (updateableMapReference != null)
                updateableMapReference.Map = this;

            // Also add the entity to the grid
            Spatial.Add(entity);

            // Add the event hooks
            entity.Disposed -= Entity_Disposed;
            entity.Disposed += Entity_Disposed;

            // Allow for additional processing
            EntityAdded(entity);
        }

        /// <summary>
        /// Changes the gravity of the map.
        /// </summary>
        /// <param name="value">The new gravity for the map, or null to use the default gravity.</param>
        public void ChangeGravity(Vector2? value)
        {
            _gravity = value;
        }

        /// <summary>
        /// Changes the ID of the map. This is generally only something you will want to do in editors.
        /// </summary>
        /// <param name="newID">The new map ID.</param>
        public void ChangeID(MapID newID)
        {
            _mapID = newID;
        }

        /// <summary>
        /// Checks for an entity's collision against other entities (besides walls).
        /// </summary>
        /// <param name="entity">The entity to check the collision for (the entity that has moved).</param>
        void CheckCollisionAgainstEntities(Entity entity)
        {
            // Get the entities we have a rectangular collision with
            var collisionSources = Spatial.GetMany<Entity>(entity, x => !(x is WallEntityBase));

            foreach (var other in collisionSources)
            {
                var displacement = SpatialHelper.MTD(entity, other);
                if (displacement != Vector2.Zero)
                {
                    entity.CollideInto(other, displacement);
                    other.CollideFrom(entity, displacement);
                }
            }
        }

        /// <summary>
        /// Checks for an entity's collision against walls.
        /// </summary>
        /// <param name="entity">The entity to check the collision for (the entity that has moved).</param>
        void CheckCollisionsAgainstWalls(Entity entity)
        {
            if (!entity.CollidesAgainstWalls)
                return;

            Vector2 moveVector = entity.Position - entity.LastPosition;
            Vector2 absMoveVector = moveVector.Abs();

            if (absMoveVector.X > 32 || absMoveVector.Y > 32)
            {
                // If the move vector is too large, use the velocity instead
                moveVector = entity.Velocity;
            }
            else
            {
                // Use velocity for unset coordinates (velocity is most likely also 0, but gives us better reliability)
                if (absMoveVector.X <= float.Epsilon)
                    moveVector.X = entity.Velocity.X;
                if (absMoveVector.Y <= float.Epsilon)
                    moveVector.Y = entity.Velocity.Y;
            }
            
            // Get the entities we have a rectangular collision with
            var collisionSources = Spatial.GetMany<WallEntityBase>(entity);

            // Do real collision detection on the entities, and handle it if the collision test passes
            foreach (var wall in collisionSources)
            {
                // Get the displacement vector if the two entities collided
                var displacement = SpatialHelper.MTD(entity, wall);

                // If there is a displacement value, forward it to the collision notifiers
                if (displacement != Vector2.Zero)
                {
                    WallEntityBase.HandleCollideInto(this, wall, entity, displacement, wall.IsPlatform, moveVector, wall.DirectionalBlock);
                }
            }
        }

        /// <summary>
        /// Clears out all the map's contents.
        /// </summary>
        protected virtual void Clear()
        {
            foreach (var e in Entities.ToArray())
                RemoveEntity(e);

            foreach (var de in DynamicEntities.ToArray())
                RemoveEntity(de);

            Spatial.Clear();
        }

        /// <summary>
        /// Describes how to create the <see cref="ISpatialCollection"/> to be used by the map. This can be overridden
        /// in the derived class to provide a different <see cref="ISpatialCollection"/> implementation.
        /// </summary>
        /// <param name="classTypeTree">The class type tree.</param>
        /// <returns>The <see cref="ISpatialCollection"/> to be used by the map.</returns>
        protected virtual ISpatialCollection CreateSpatialCollection(ClassTypeTree classTypeTree)
        {
            if (classTypeTree.Type == typeof(WallEntityBase))
                return new StaticGridSpatialCollection();
            else
                return new DynamicGridSpatialCollection();
        }

        /// <summary>
        /// Creates the <see cref="ISpatialCollection"/> to be used for the spatial objects on the map.
        /// </summary>
        /// <returns>The <see cref="ISpatialCollection"/> to be used for the spatial objects on the map.</returns>
        protected virtual ISpatialCollection CreateSpatialManager()
        {
            return new SpatialManager(GetSpatialTypes(), CreateSpatialCollection);
        }

        /// <summary>
        /// When overridden in the derived class, creates a new WallEntityBase instance.
        /// </summary>
        /// <returns>WallEntityBase that is to be used on the map.</returns>
        protected abstract WallEntityBase CreateWall(IValueReader r);

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities added to the map.
        /// This is called after the Entity has finished being added to the map.
        /// </summary>
        /// <param name="entity">Entity that was added to the map.</param>
        protected virtual void EntityAdded(Entity entity)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional processing on Entities removed from the map.
        /// This is called after the Entity has finished being removed from the map.
        /// </summary>
        /// <param name="entity">Entity that was removed from the map.</param>
        protected virtual void EntityRemoved(Entity entity)
        {
        }

        /// <summary>
        /// Handles when an <see cref="Entity"/> is disposed while still on the map.
        /// </summary>
        /// <param name="sender">The <see cref="Entity"/> that was disposed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Entity_Disposed(Entity sender, EventArgs e)
        {
            RemoveEntity(sender);
        }

        /// <summary>
        /// Checks if an Entity is in the map's boundaries and, if it is not, moves the Entity into the map's boundaries.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        void ForceEntityInMapBoundaries(Entity entity)
        {
            var min = entity.Position;
            var max = entity.Max;

            if (min.X < 0)
                min.X = 0;
            if (min.Y < 0)
                min.Y = 0;
            if (max.X >= Width)
                min.X = Width - entity.Size.X;
            if (max.Y >= Height)
                min.Y = Height - entity.Size.Y;

            entity.Position = min;
        }

        /// <summary>
        /// Gets the DynamicEntity with the specified index.
        /// </summary>
        /// <param name="mapEntityIndex">Index of the DynamicEntity to get.</param>
        /// <returns>The DynamicEntity with the specified <paramref name="mapEntityIndex"/>, or null if
        /// no <see cref="DynamicEntity"/> was found..</returns>
        public DynamicEntity GetDynamicEntity(MapEntityIndex mapEntityIndex)
        {
            return _dynamicEntities[(int)mapEntityIndex];
        }

        /// <summary>
        /// Gets the DynamicEntity with the specified index.
        /// </summary>
        /// <typeparam name="T">Type of DynamicEntity to get.</typeparam>
        /// <param name="mapEntityIndex">Index of the DynamicEntity to get.</param>
        /// <returns>The DynamicEntity with the specified <paramref name="mapEntityIndex"/>, or null if
        /// no <see cref="DynamicEntity"/> was found.</returns>
        public T GetDynamicEntity<T>(MapEntityIndex mapEntityIndex) where T : DynamicEntity
        {
            var dynamicEntity = GetDynamicEntity(mapEntityIndex);
            if (dynamicEntity == null)
                return null;

            var casted = dynamicEntity as T;

            if (casted == null)
            {
                const string errmsg = "DynamicEntity found, but was of type `{0}`, not type `{1}` like expected!";
                Debug.Fail(string.Format(errmsg, dynamicEntity.GetType(), typeof(T)));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, dynamicEntity.GetType(), typeof(T));
            }

            return casted;
        }

        /// <summary>
        /// Allows an entity to be grabbed by index. Intended only for performance cases where iterating
        /// through the entire entity set would not be optimal (such as comparing the entities to each other).
        /// </summary>
        /// <param name="index">Index of the entity to get</param>
        /// <returns>Entity for the given index, or null if invalid</returns>
        protected Entity GetEntity(int index)
        {
            // Ensure the index is valid
            if (index < 0 || index >= _entities.Count)
            {
                Debug.Fail("Invalid index.");
                return null;
            }

            // Get the entity at the index
            return _entities[index];
        }

        /// <summary>
        /// Gets the path to a map file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/>.</param>
        /// <param name="id">The ID of the map.</param>
        /// <returns>The path to the map file.</returns>
        public static string GetMapFilePath(ContentPaths contentPath, MapID id)
        {
            return contentPath.Maps.Join(id + EngineSettings.DataFileSuffix);
        }

        /// <summary>
        /// Gets the path to a mini-map file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/>.</param>
        /// <param name="id">The ID of the map.</param>
        /// <returns>The path to the mini-map file.</returns>
        public static string GetMiniMapFilePath(ContentPaths contentPath, MapID id)
        {
            return contentPath.Grhs.Join("MiniMap\\" + id + EngineSettings.ImageFileSuffix);
        }

        /// <summary>
        /// Gets an IEnumerable of the path of all the map files in the given ContentPaths.
        /// </summary>
        /// <param name="path">ContentPaths to load the map files from.</param>
        /// <returns>An IEnumerable of all the map files in <paramref name="path"/>.</returns>
        public static IEnumerable<string> GetMapFiles(ContentPaths path)
        {
            // Get all the files
            var allFiles = Directory.GetFiles(path.Maps);

            // Select only valid map files
            var mapFiles = allFiles.Where(IsValidMapFile);

            return mapFiles;
        }

        /// <summary>
        /// Gets MapIds for maps that exist in file.
        /// </summary>
        public static MapID[] GetUsedMapIds(ContentPaths path)
        {
            var mapFiles = GetMapFiles(path);

            var usedIndices = new List<MapID>(mapFiles.Count());
            foreach (var file in mapFiles)
            {
                MapID o;
                if (TryGetIndexFromPath(file, out o))
                    usedIndices.Add(o);
            }

            usedIndices.Sort();
            return usedIndices.ToArray();
        }

        /// <summary>
        /// Gets the next free map index.
        /// </summary>
        /// <param name="path">ContentPaths containing the maps.</param>
        /// <returns>Next free map index.</returns>
        public static MapID GetNextFreeIndex(ContentPaths path)
        {
            // Get the used map indices
            MapID[] usedIndices = GetUsedMapIds(path);

            // Find the first missing map index
            // Check every map index starting at 1, returning the first free value found
            int expected = 1;
            for (var i = 0; i < usedIndices.Length; i++)
            {
                if ((int)usedIndices[i] != expected)
                    return new MapID(expected);

                expected++;
            }

            return new MapID(expected);
        }

        /// <summary>
        /// Gets the possible positions to place the given <see cref="ISpatial"/> around a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to place around.</param>
        /// <param name="r">The <see cref="Rectangle"/> describing the area to be placed.</param>
        /// <returns>An IEnumerable of the min positions to place the <paramref name="r"/> around the
        /// given <paramref name="spatial"/>.</returns>
        static IEnumerable<Vector2> GetPositionsAroundSpatial(ISpatial spatial, Rectangle r)
        {
            // Top
            yield return new Vector2(r.X, spatial.Position.Y - r.Height - 1);

            // Bottom
            yield return new Vector2(r.X, spatial.Max.Y + 1);

            // Left
            yield return new Vector2(spatial.Position.X - r.Width - 1, r.Y);

            // Right
            yield return new Vector2(spatial.Max.X + 1, r.Y);

            // Top, left-aligned
            yield return new Vector2(spatial.Position.X, spatial.Position.Y - r.Height - 1);

            // Top, right-aligned
            yield return new Vector2(spatial.Max.X - r.Width, spatial.Position.Y - r.Height - 1);

            // Bottom, left-aligned
            yield return new Vector2(spatial.Position.X, spatial.Max.Y + 1);

            // Bottom, right-aligned
            yield return new Vector2(spatial.Max.X - r.Width, spatial.Max.Y + 1);

            // Left, top-aligned
            yield return new Vector2(spatial.Position.X - r.Width - 1, spatial.Position.Y);

            // Left, bottom-aligned
            yield return new Vector2(spatial.Position.X - r.Width - 1, spatial.Max.Y - r.Height);

            // Right, top-aligned
            yield return new Vector2(spatial.Max.X + 1, spatial.Position.Y);

            // Right, bottom-aligned
            yield return new Vector2(spatial.Max.X + 1, spatial.Max.Y - r.Height);
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for. This should include
        /// all the <see cref="Type"/>s that are used frequently when querying the map's spatial collection.
        /// </summary>
        /// <returns>An IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual IEnumerable<Type> GetSpatialTypes()
        {
            return new Type[]
            { typeof(Entity), typeof(DynamicEntity), typeof(CharacterEntity), typeof(WallEntityBase), typeof(ItemEntityBase) };
        }

        public bool IsInMapBoundaries(Vector2 min, Vector2 max)
        {
            return IsInMapBoundaries(min) && IsInMapBoundaries(max);
        }

        public bool IsInMapBoundaries(Entity entity)
        {
            return IsInMapBoundaries(entity.ToRectangle());
        }

        public bool IsInMapBoundaries(Rectangle rect)
        {
            var min = new Vector2(rect.X, rect.Y);
            var max = new Vector2(rect.Right, rect.Bottom);
            return IsInMapBoundaries(min, max);
        }

        public bool IsInMapBoundaries(Vector2 point)
        {
            return (point.X >= 0) && (point.Y >= 0) && (point.X < Width) && (point.Y < Height);
        }

        /// <summary>
        /// Checks if a given file is a valid map file by taking a quick look at the file, but not
        /// actually loading it. Files that are validated by this can still fail to load if they
        /// are corrupt.
        /// </summary>
        /// <param name="filePath">Path to the file to check</param>
        /// <returns>True if the file is a valid map file, else false</returns>
        public static bool IsValidMapFile(string filePath)
        {
            // Check if the file exists
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            // Check the suffix
            var expectedSuffix = EngineSettings.DataFileSuffix;
            if (expectedSuffix.Length > 0 && !filePath.EndsWith(expectedSuffix, StringComparison.OrdinalIgnoreCase))
                return false;

            // Check if the file is named properly
            MapID index;
            if (!Parser.Invariant.TryParse(Path.GetFileNameWithoutExtension(filePath), out index))
                return false;

            // Validate the file index
            if (index < 1)
                return false;

            // All checks passed
            return true;
        }

        /// <summary>
        /// Checks if a <see cref="Rectangle"/> is in a place that is inside of the map and does not intersect
        /// any <see cref="WallEntityBase"/>s.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> containing the placement to check.</param>
        /// <returns>True if the <see cref="Rectangle"/> is in an area that does not intersect any
        /// <see cref="WallEntityBase"/>s; otherwise false.</returns>
        public bool IsValidPlacementPosition(Rectangle rect)
        {
            return IsInMapBoundaries(rect) && !Spatial.Contains<WallEntity>(rect);
        }

        public bool IsValidPlacementPosition(Vector2 position, Vector2 size)
        {
            var rect = new Rectangle(position.X, position.Y, size.X, size.Y);
            return IsValidPlacementPosition(rect);
        }

        /// <summary>
        /// Checks if a <see cref="Rectangle"/> is in a place that is inside of the map and does not intersect
        /// any <see cref="WallEntityBase"/>s.
        /// </summary>
        /// <param name="r">The <see cref="Rectangle"/> that represents the area to check.</param>
        /// <param name="closestValidPosition">When this method returns false, contains the closest valid position
        /// that the <see cref="Rectangle"/> can be at to not intersect any <see cref="WallEntityBase"/>s.</param>
        /// <param name="validPositionFound">When this method returns false, contains if the
        /// <see cref="closestValidPosition"/> that was found is valid. If false, no near-by legal position could be
        /// found that the <see cref="Rectangle"/> could occupy without causing any intersections.</param>
        /// <returns>True if the <see cref="Rectangle"/> is in an area that does not intersect any
        /// <see cref="WallEntityBase"/>s; otherwise false.</returns>
        public bool IsValidPlacementPosition(Rectangle r, out Vector2 closestValidPosition, out bool validPositionFound)
        {
            // Perform the initial check to see if we need to even find a new position
            if (IsValidPlacementPosition(r))
            {
                // No intersections - already a valid position
                closestValidPosition = new Vector2(r.X, r.Y);
                validPositionFound = true;
                return true;
            }

            // Intersections were found, so we have to find a valid position
            // First, grab the walls in the region around the cb
            var nearbyWallsRect = r.Inflate(_findValidPlacementPadding);
            var nearbyWalls = Spatial.GetMany<WallEntityBase>(nearbyWallsRect);

            // Next, find the legal positions we can place the cb
            var cbSize = new Vector2(r.Width, r.Height);
            var validPlacementPositions =
                nearbyWalls.SelectMany(wall => GetPositionsAroundSpatial(wall, r)).Where(p => IsValidPlacementPosition(p, cbSize));

            // If there are 0 legal positions, we're F'd in the A
            if (validPlacementPositions.IsEmpty())
            {
                // Failure :(
                validPositionFound = false;
                closestValidPosition = Vector2.Zero;
            }
            else
            {
                // One or more legal positions found, so find the closest one and use that
                validPositionFound = true;
                var cbMin = new Vector2(r.X, r.Y);
                closestValidPosition = validPlacementPositions.MinElement(x => x.QuickDistance(cbMin));
            }

            return false;
        }

        /// <summary>
        /// Confirms that an entity being moved by a given offset will
        /// keep the offset in the map boundaries, modifying the offset
        /// if needed to stay in the map.
        /// </summary>
        /// <param name="entity">Entity to be moved.</param>
        /// <param name="offset">Amount to add the entity's position.</param>
        public void KeepInMap(Entity entity, ref Vector2 offset)
        {
            if (entity == null)
            {
                const string errmsg = "Parameter `entity` is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            if (entity.Position.X + offset.X < 0)
                offset.X = -entity.Position.X;
            else if (entity.Max.X + offset.X > Width)
                offset.X = entity.Max.X - Width;

            if (entity.Position.Y + offset.Y < 0)
                offset.Y = -entity.Position.Y;
            else if (entity.Max.Y + offset.Y > Height)
                offset.Y = entity.Max.Y - Height;
        }

        /// <summary>
        /// Loads the map from the specified content path.
        /// </summary>
        /// <param name="contentPath">ContentPath to load the map from.</param>
        /// <param name="loadDynamicEntities">If true, the DynamicEntities will be loaded from the file. If false,
        /// all DynamicEntities will be skipped.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        public void Load(ContentPaths contentPath, bool loadDynamicEntities, IDynamicEntityFactory dynamicEntityFactory)
        {
            var path = GetMapFilePath(contentPath, ID);
            Load(path, loadDynamicEntities, dynamicEntityFactory);
        }

        /// <summary>
        /// Loads the map.
        /// </summary>
        /// <param name="filePath">Path to the file to load.</param>
        /// <param name="loadDynamicEntities">If true, the DynamicEntities will be loaded from the file. If false,
        /// all DynamicEntities will be skipped.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dynamicEntityFactory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="filePath"/> is invalid or no file exists at the path.</exception>
        protected void Load(string filePath, bool loadDynamicEntities, IDynamicEntityFactory dynamicEntityFactory)
        {
            if (dynamicEntityFactory == null)
                throw new ArgumentNullException("dynamicEntityFactory");

            if (!File.Exists(filePath))
            {
                const string errmsg = "Map file `{0}` does not exist - unable to load map.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath);
                throw new ArgumentException(string.Format(errmsg, filePath), "filePath");
            }

            var r = GenericValueReader.CreateFromFile(filePath, _rootNodeName);
            Load(r, loadDynamicEntities, dynamicEntityFactory);
        }

        public void Load(IValueReader r, bool loadDynamicEntities, IDynamicEntityFactory dynamicEntityFactory)
        {
            Clear();

            LoadHeader(r);
            LoadWalls(r);
            LoadDynamicEntities(r, loadDynamicEntities, dynamicEntityFactory);
            LoadMisc(r.ReadNode(_miscNodeName));
        }

        void LoadDynamicEntities(IValueReader r, bool loadDynamicEntities, IDynamicEntityFactory dynamicEntityFactory)
        {
            var loadedDynamicEntities = r.ReadManyNodes(_dynamicEntitiesNodeName, x => dynamicEntityFactory.Read(x, true));

            // Add the loaded DynamicEntities to the map
            if (loadDynamicEntities)
            {
                foreach (var dynamicEntity in loadedDynamicEntities)
                {
                    AddEntity(dynamicEntity);
                }
            }
        }

        /// <summary>
        /// Loads the header information for the map.
        /// </summary>
        /// <param name="r"><see cref="IValueReader"/> used to load the map file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="r" /> is <c>null</c>.</exception>
        void LoadHeader(IValueReader r)
        {
            if (r == null)
                throw new ArgumentNullException("r");

            var nodeReader = r.ReadNode(_headerNodeName);

            // Read the values
            Name = nodeReader.ReadString(_headerNodeNameKey);

            var hasMusic = nodeReader.ReadBool(_headerNodeHasMusicKey);
            MusicID = nodeReader.ReadMusicID(_headerNodeMusicKey);
            if (!hasMusic)
                MusicID = null;

            ParentMapID = nodeReader.ReadMapID(_headerNodeParentMapKey);

            _size = nodeReader.ReadVector2(_headerNodeSizeKey);
            Indoors = nodeReader.ReadBool(_headerNodeIndoorsKey);

            var customGravity = nodeReader.ReadBool(_headerNodeCustomGravityKey);
            _gravity = nodeReader.ReadVector2(_headerNodeGravityKey);
            if (!customGravity)
                _gravity = null;

            Debug.Assert(Size.X > 0 && Size.Y > 0);

            // Set the size for the spatial
            Spatial.SetAreaSize(Size);
        }

        /// <summary>
        /// Handles loading of custom values.
        /// </summary>
        /// <param name="r">IValueReader to read the misc values from.</param>
        protected virtual void LoadMisc(IValueReader r)
        {
        }

        /// <summary>
        /// Loads the wall information for the map.
        /// </summary>
        /// <param name="r"><see cref="IValueReader"/> used to load the map file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="r" /> is <c>null</c>.</exception>
        void LoadWalls(IValueReader r)
        {
            if (r == null)
                throw new ArgumentNullException("r");

            var loadedWalls = r.ReadManyNodes(_wallsNodeName, CreateWall);

            // Add the loaded walls to the map
            foreach (var wall in loadedWalls)
            {
                AddEntity(wall);
            }
        }

        /// <summary>
        /// Checks if a <see cref="MapID"/> is valid and a map exists with the given ID.
        /// </summary>
        /// <param name="mapID">The ID to check.</param>
        /// <returns>True if the <paramref name="mapID"/> is valid and exists; otherwise false.</returns>
        public static bool MapIDExists(MapID mapID)
        {
            var path = GetMapFilePath(ContentPaths.Build, mapID);
            if (!File.Exists(path))
                return false;

            return true;
        }

        /// <summary>
        /// Removes an <see cref="Entity"/> from the map.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> to remove from the map.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public void RemoveEntity(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            // Remove the listeners
            entity.Disposed -= Entity_Disposed;

            // Remove the entity from the entity list
            if (!_entities.Remove(entity))
            {
                // Entity must have already been removed, since it wasn't in the entities list
                Debug.Fail("entity was not in the entities list");
            }

            var asUpdateable = entity as IUpdateableEntity;
            if (asUpdateable != null && !_updateableEntities.Remove(asUpdateable))
                Debug.Fail("Updateable entity was not in the updateable entities list");

            // If a DynamicEntity, remove it from the DynamicEntities list
            DynamicEntity dynamicEntity;
            if ((dynamicEntity = entity as DynamicEntity) != null)
            {
                Debug.Assert(_dynamicEntities[(int)dynamicEntity.MapEntityIndex] == dynamicEntity,
                    string.Format("DynamicEntity `{0}` is holding an invalid MapEntityIndex!", dynamicEntity));

                _dynamicEntities[(int)dynamicEntity.MapEntityIndex] = null;
            }

            // Remove the entity from the grid
            Spatial.Remove(entity);

            // Allow for additional processing
            EntityRemoved(entity);
        }

        /// <summary>
        /// Resizes the Entity to the supplied size, but forces the Entity to remain in the map's boundaries.
        /// </summary>
        /// <param name="entity">Entity to teleport</param>
        /// <param name="size">New size of the entity</param>
        public void SafeResizeEntity(Entity entity, Vector2 size)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            // Ensure the entity will be in the map
            var newMax = entity.Position + size;
            if (newMax.X > Width)
                size.X = Width - entity.Position.X;
            if (newMax.Y > Height)
                size.Y = Height - entity.Position.Y;

            // Set the size
            entity.Size = size;
        }

        /// <summary>
        /// Performs an Entity.Teleport() call to the supplied position, but forces the Entity to remain
        /// in the map's boundaries.
        /// </summary>
        /// <param name="entity">Entity to teleport</param>
        /// <param name="pos">Position to teleport to</param>
        public void SafeTeleportEntity(Entity entity, Vector2 pos)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            // Ensure the entity will be in the map
            if (pos.X < 0)
                pos.X = 0;
            if (pos.Y < 0)
                pos.Y = 0;

            var max = pos + entity.Size;
            if (max.X > Width)
                pos.X = Width - entity.Size.X;
            if (max.Y > Height)
                pos.Y = Height - entity.Size.Y;

            // Teleport to the altered position
            entity.Position = pos;
        }

        /// <summary>
        /// Gets if the map in its current state differs from the map saved to file.
        /// </summary>
        /// <param name="contentPath">Content path to save the map file to.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        /// <returns>True if this map differs from the one on file; otherwise false.</returns>
        public bool DiffersFromSaved(ContentPaths contentPath, IDynamicEntityFactory dynamicEntityFactory)
        {
            var path = contentPath.Maps.Join(ID + EngineSettings.DataFileSuffix);
            
            // We compare using binary instead of string. Even though we could use string comparison that is more robust at ignoring
            // unimportant diffs, a raw binary comparison gives less false negatives, and will work if switching to save as binary.

            // Get the contents on disk
            if (!File.Exists(path))
                return true; // File didn't exist, so it clearly differs

            byte[] existingMapData = File.ReadAllBytes(path);

            // Get the current map contents
            byte[] currentMapData;
            using (var tmpFile = new TempFile())
            {
                Save(tmpFile.FilePath, dynamicEntityFactory);
                currentMapData = File.ReadAllBytes(tmpFile.FilePath);
            }

            // Compare
            return !ByteArrayEqualityComparer.AreEqual(existingMapData, currentMapData);
        }

        /// <summary>
        /// Saves the map to a file to the specified content path.
        /// </summary>
        /// <param name="contentPath">Content path to save the map file to.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        public void Save(ContentPaths contentPath, IDynamicEntityFactory dynamicEntityFactory)
        {
            Save(ID, contentPath, dynamicEntityFactory);
        }

        /// <summary>
        /// Saves the map to a file to the specified content path.
        /// </summary>
        /// <param name="mapID">Map ID to save as.</param>
        /// <param name="contentPath">Content path to save the map file to.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        public void Save(MapID mapID, ContentPaths contentPath, IDynamicEntityFactory dynamicEntityFactory)
        {
            var path = contentPath.Maps.Join(mapID + EngineSettings.DataFileSuffix);
            Save(path, dynamicEntityFactory);

            if (Saved != null)
                Saved.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Saves the map to a file to the specified file path.
        /// </summary>
        /// <param name="filePath">Path to save the map file at.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to load the
        /// <see cref="DynamicEntity"/>s.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dynamicEntityFactory"/> is null.</exception>
        /// <exception cref="IOException">The directory name could not be found from the <paramref name="filePath"/>.</exception>
        void Save(string filePath, IDynamicEntityFactory dynamicEntityFactory)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            if (dynamicEntityFactory == null)
                throw new ArgumentNullException("dynamicEntityFactory");

            var dirName = Path.GetDirectoryName(filePath);
            if (dirName == null)
            {
                const string errmsg = "Failed to get the directory name for the path `{0}`.";
                throw new IOException(string.Format(errmsg, filePath));
            }

            Directory.CreateDirectory(dirName);
            using (var w = GenericValueWriter.Create(filePath, _rootNodeName))
            {
                Save(w, dynamicEntityFactory);
            }
        }

        /// <summary>
        /// Saves the map to an IValueWriter.
        /// </summary>
        /// <param name="writer">The IValueWriter to save to.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to save the <see cref="DynamicEntity"/>s.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dynamicEntityFactory"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public void Save(IValueWriter writer, IDynamicEntityFactory dynamicEntityFactory, MapSaveFlags saveFlags = MapSaveFlags.None)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (dynamicEntityFactory == null)
                throw new ArgumentNullException("dynamicEntityFactory");

            SaveHeader(writer, saveFlags);
            SaveWalls(writer, saveFlags);
            SaveDynamicEntities(writer, dynamicEntityFactory, saveFlags);

            writer.WriteStartNode(_miscNodeName);
            SaveMisc(writer, saveFlags);
            writer.WriteEndNode(_miscNodeName);
        }

        /// <summary>
        /// Saves the DynamicEntities.
        /// </summary>
        /// <param name="w">The IValueWriter to save to.</param>
        /// <param name="dynamicEntityFactory">The <see cref="IDynamicEntityFactory"/> used to save the <see cref="DynamicEntity"/>s.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        void SaveDynamicEntities(IValueWriter w, IDynamicEntityFactory dynamicEntityFactory, MapSaveFlags saveFlags)
        {
            var entities = DynamicEntities;
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                entities = entities.OrderBy(x => x, new BasicSpatialComparer());
            }
            entities = entities.ToImmutable();

            w.WriteManyNodes(_dynamicEntitiesNodeName, entities, (x, v) => dynamicEntityFactory.Write(x, v));
        }

        /// <summary>
        /// Saves the map header
        /// </summary>
        /// <param name="w"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        /// <exception cref="ArgumentNullException"><paramref name="w" /> is <c>null</c>.</exception>
        void SaveHeader(IValueWriter w, MapSaveFlags saveFlags)
        {
            if (w == null)
                throw new ArgumentNullException("w");

            w.WriteStartNode(_headerNodeName);
            {
                w.Write(_headerNodeNameKey, Name);
                w.Write(_headerNodeParentMapKey, ParentMapID);
                w.Write(_headerNodeHasMusicKey, MusicID.HasValue);
                w.Write(_headerNodeMusicKey, MusicID.HasValue ? MusicID.Value : new MusicID(0));
                w.Write(_headerNodeSizeKey, Size);
                w.Write(_headerNodeIndoorsKey, Indoors);
                w.Write(_headerNodeCustomGravityKey, _gravity.HasValue);
                w.Write(_headerNodeGravityKey, _gravity.HasValue ? _gravity.Value : Vector2.Zero);
            }
            w.WriteEndNode(_headerNodeName);
        }

        /// <summary>
        /// When overridden in the derived class, saves misc map information specific to the derived class.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        protected virtual void SaveMisc(IValueWriter w, MapSaveFlags saveFlags)
        {
        }

        /// <summary>
        /// Saves the map walls
        /// </summary>
        /// <param name="w"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        /// <exception cref="ArgumentNullException"><paramref name="w" /> is <c>null</c>.</exception>
        void SaveWalls(IValueWriter w, MapSaveFlags saveFlags)
        {
            if (w == null)
                throw new ArgumentNullException("w");

            var walls = Entities.OfType<WallEntityBase>();
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                walls = walls.OrderBy(x => x, new BasicSpatialComparer());
            }
            walls = walls.ToImmutable();

            w.WriteManyNodes(_wallsNodeName, walls, ((writer, item) => item.Write(writer)));
        }

        /// <summary>
        /// Sets the new dimensions of the map and trims objects that exceed the new dimension
        /// </summary>
        /// <param name="newSize">New size of the map</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>newSize</c> is out of range.</exception>
        public void SetDimensions(Vector2 newSize)
        {
            if (Size == newSize)
                return;

            if (newSize.X <= 1 || newSize.Y <= 1)
                throw new ArgumentOutOfRangeException("newSize", "Invalid map size.");

            if (newSize.X > ushort.MaxValue || newSize.Y > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("newSize",
                    "Map sizes larger than ushort.MaxValue are not supported by default.");

            // Remove any objects outside of the new dimensions
            if (Size.X > newSize.X || Size.Y > newSize.Y)
            {
                var outOfRangeEntities = Entities.Where(x => x.Max.X > newSize.X || x.Max.Y > newSize.Y).ToImmutable();
                foreach (var entity in outOfRangeEntities)
                {
                    entity.Dispose();
                    RemoveEntity(entity);
                }
            }

            // Update the map's size
            _size = newSize;

            // Update the spatial's size
            Spatial.SetAreaSize(Size);
        }

        /// <summary>
        /// Snaps a entity to any near-by entity
        /// </summary>
        /// <param name="entity">Entity to edit</param>
        /// <param name="maxDiff">Maximum position difference</param>
        /// <returns>New position for the entity</returns>
        public Vector2 SnapToWalls(Entity entity, float maxDiff = 20f)
        {
            var ret = entity.Position;
            var newRect = entity.ToRectangle().Inflate(maxDiff / 2);

            foreach (var w in Spatial.GetMany<WallEntityBase>(newRect, x => x != entity))
            {
                // Selected wall right side to target wall left side
                if (Math.Abs(newRect.Right - w.Position.X) < maxDiff)
                    ret.X = w.Position.X - entity.Size.X;

                // Selected wall left side to target wall right side
                if (Math.Abs(w.Max.X - newRect.X) < maxDiff)
                    ret.X = w.Max.X;

                // Selected wall bottom to target wall top
                if (Math.Abs(newRect.Bottom - w.Position.Y) < maxDiff)
                    ret.Y = w.Position.Y - entity.Size.Y;

                // Selected wall top to target wall bottom
                if (Math.Abs(w.Max.Y - newRect.Y) < maxDiff)
                    ret.Y = w.Max.Y;
            }

            return ret;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var ret = Name ?? "[Unnamed]";
            ret += " [ID: " + ID + "]";
            return ret;
        }

        /// <summary>
        /// Tries to get the map's index from the file path.
        /// </summary>
        /// <param name="path">File path to the map.</param>
        /// <param name="mapID">If this method returns true, contains the index of the map.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null or empty.</exception>
        public static bool TryGetIndexFromPath(string path, out MapID mapID)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            var fileName = Path.GetFileNameWithoutExtension(path);
            return Parser.Invariant.TryParse(fileName, out mapID);
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="deltaTime">The amount of time that elapsed since the last update.</param>
        public void Update(uint deltaTime)
        {
            Update((int)deltaTime);
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="deltaTime">The amount of time that elapsed since the last update.</param>
        public virtual void Update(int deltaTime)
        {
            // Update the Entities
            // We use a for loop because entities might be added/removed when they update
            var i = 0;
            while (true)
            {
                // Check if we hit the end
                if (i >= _updateableEntities.Count)
                    break;

                // Get and update the current entity
                IUpdateableEntity current = _updateableEntities[i];

                if (current != null)
                    current.Update(this, deltaTime);

                // Only increment if the current has not changed
                // This way we can be sure to update everyone even if the entities collection changed
                if (i < _updateableEntities.Count && current == _updateableEntities[i])
                    i++;
            }

            // Update the delayed events
            UpdateDelayedEvents();
        }

        /// <summary>
        /// Updates the <see cref="IDelayedMapEvent"/>s.
        /// </summary>
        void UpdateDelayedEvents()
        {
            var currentTime = TickCount.Now;

            for (var i = 0; i < _delayedEvents.Count; i++)
            {
                var e = _delayedEvents[i];

                // Remove dead elements by swapping them with the last element in the list, then removing the last element
                // This allows us to remove elements without shifting down the whole list
                if (e.IsReady(currentTime))
                {
                    // Swap with last element
                    _delayedEvents[i] = _delayedEvents[_delayedEvents.Count - 1];

                    // Remove last element in the list
                    _delayedEvents.RemoveAt(_delayedEvents.Count - 1);

                    // Finally, execute the event
                    e.Execute();
                }
            }
        }

        #region IMap Members

        /// <summary>
        /// Gets the <see cref="IDelayedMapEvent"/>s on the map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<IDelayedMapEvent> DelayedEvents
        {
            get { return _delayedEvents; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the Entities on the Map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Entity> Entities
        {
            get { return _entities; }
        }

        /// <summary>
        /// Gets the gravity to use on the map.
        /// </summary>
        [Browsable(false)]
        public Vector2 Gravity
        {
            get
            {
                if (_gravity.HasValue)
                    return _gravity.Value;
                else
                    return EngineSettings.Instance.Gravity;
            }
        }

        /// <summary>
        /// Gets the height of the map in pixels
        /// </summary>
        [Browsable(false)]
        public float Height
        {
            get { return Size.Y; }
        }

        /// <summary>
        /// Gets the ID of the map.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        [Description("The ID of the map.")]
        public MapID ID
        {
            get { return _mapID; }
        }

        /// <summary>
        /// Gets the size of the map in pixels.
        /// </summary>
        [Browsable(false)]
        public Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets the <see cref="ISpatialCollection"/> for all the spatial objects on the map.
        /// </summary>
        [Browsable(false)]
        public ISpatialCollection Spatial
        {
            get { return _spatialCollection; }
        }

        /// <summary>
        /// Gets the width of the map in pixels.
        /// </summary>
        [Browsable(false)]
        public float Width
        {
            get { return Size.X; }
        }

        /// <summary>
        /// Adds a <see cref="IDelayedMapEvent"/>.
        /// </summary>
        /// <param name="e">The <see cref="IDelayedMapEvent"/>.</param>
        public void AddDelayedEvent(IDelayedMapEvent e)
        {
            if (e == null)
                return;

            _delayedEvents.Add(e);
        }

        /// <summary>
        /// Checks if an Entity collides with any other entities. For each collision, <paramref name="entity"/>
        /// will call CollideInto(), and the Entity that <paramref name="entity"/> collided into will call
        /// CollideFrom().
        /// </summary>
        /// <param name="entity">Entity to check against other Entities. If the CollisionType is
        /// CollisionType.None, or if <paramref name="entity"/> is null, this will always return 0 and no
        /// collision detection will take place.</param>
        public void CheckCollisions(Entity entity)
        {
            // Check for a null entity
            if (entity == null)
            {
                const string errmsg = "Parameter `entity` is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            // Ensure the entity is in the map still
            var move = Vector2.Zero;
            var entityMin = entity.Position;
            var entityMax = entity.Max;

            if (entityMin.X < 0)
                move.X = -entityMin.X;
            else if (entityMax.X >= Size.X)
                move.X = Size.X - entityMax.X;

            if (entityMin.Y < 0)
                move.Y = -entityMin.Y;
            else if (entityMax.Y >= Size.Y)
                move.Y = Size.Y - entityMax.Y;

            entity.Move(move);

            // Perform collision detetion
            CheckCollisionsAgainstWalls(entity);
            CheckCollisionAgainstEntities(entity);
        }

        /// <summary>
        /// Finds the <see cref="WallEntityBase"/> that the <paramref name="stander"/> is standing on.
        /// </summary>
        /// <param name="stander">The <see cref="ISpatial"/> to check for standing on a <see cref="WallEntityBase"/>.</param>
        /// <returns>The best-fit <see cref="WallEntityBase"/> that the <paramref name="stander"/> is standing on, or
        /// null if they are not standing on any walls.</returns>
        public WallEntityBase FindStandingOn(ISpatial stander)
        {
            var rect = stander.GetStandingAreaRect();
            return Spatial.Get<WallEntityBase>(rect);
        }

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public TickCount GetTime()
        {
            return _getTime.GetTime();
        }

        #endregion

        #region IMapTable Members

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Browsable(true)]
        [Description("The name of the map.")]
        [Category("Map")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _name = value;
            }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        IMapTable IMapTable.DeepCopy()
        {
            return new MapTable(this);
        }

        #endregion

        /// <summary>
        /// Gets a rough (not very reliable) comparison between two spatials. Used to sort before
        /// saving the map to help make the order more consistent (resulting in more reasonable save diffs).
        /// </summary>
        protected class BasicSpatialComparer : IComparer<ISpatial>
        {
            public int Compare(ISpatial a, ISpatial b)
            {
                int tmp = a.Position.X.CompareTo(b.Position.X);
                if (tmp != 0) return tmp;

                tmp = a.Position.Y.CompareTo(b.Position.Y);
                if (tmp != 0) return tmp;

                tmp = a.Size.X.CompareTo(b.Size.X);
                if (tmp != 0) return tmp;

                tmp = a.Size.Y.CompareTo(b.Size.Y);
                if (tmp != 0) return tmp;

                return StringComparer.Ordinal.Compare(a.GetType().FullName ?? string.Empty, b.GetType().FullName ?? string.Empty);
            }
        }
    }
}