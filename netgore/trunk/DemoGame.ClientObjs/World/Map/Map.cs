using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Handler for Map drawing events.
    /// </summary>
    /// <param name="map">Map that the drawing is taking place on.</param>
    /// <param name="layer">The layer that the drawing event is related to.</param>
    /// <param name="spriteBatch">The SpriteBatch that was used to do the drawing.</param>
    /// <param name="camera">The Camera2D that was used in the drawing.</param>
    /// <param name="isDrawing">If the <paramref name="layer"/> is actually being drawn by the <paramref name="map"/>. If
    /// false, it is time for the <paramref name="layer"/> to be drawn, but the <paramref name="map"/> will not actually
    /// draw the layer.</param>
    public delegate void MapDrawEventHandler(
        Map map, MapRenderLayer layer, SpriteBatch spriteBatch, Camera2D camera, bool isDrawing);

    /// <summary>
    /// Map object for the client
    /// </summary>
    public class Map : MapBase, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _bgImagesNodeName = "BackgroundImages";
        const string _mapGrhsNodeName = "MapGrhs";
        const string _usedIndiciesNodeName = "UsedIndicies";

        /// <summary>
        /// List of BackgroundImages on this map.
        /// </summary>
        readonly List<BackgroundImage> _backgroundImages = new List<BackgroundImage>();

        /// <summary>
        /// List of IDrawableEntity objects in the background layer
        /// </summary>
        readonly List<IDrawable> _drawLayerBackground = new List<IDrawable>();

        /// <summary>
        /// List of IDrawableEntity objects in the character layer
        /// </summary>
        readonly List<IDrawable> _drawLayerCharacter = new List<IDrawable>();

        /// <summary>
        /// List of IDrawableEntity objects in the foreground layer
        /// </summary>
        readonly List<IDrawable> _drawLayerForeground = new List<IDrawable>();

        /// <summary>
        /// List of IDrawableEntity objects in the item layer
        /// </summary>
        readonly List<IDrawable> _drawLayerItem = new List<IDrawable>();

        /// <summary>
        /// Graphics device used when building the atlas
        /// </summary>
        readonly GraphicsDevice _graphics;

        /// <summary>
        /// List of map grhs on the map
        /// </summary>
        readonly List<MapGrh> _mapGrhs = new List<MapGrh>(128);

        /// <summary>
        /// The world the map belongs to
        /// </summary>
        readonly World _world;

        TextureAtlas _atlas;

        /// <summary>
        /// List of atlas textures used for the graphics for the map
        /// </summary>
        List<Texture2D> _mapAtlases = new List<Texture2D>();

        /// <summary>
        /// Notifies listeners when the Map has finished drawing a layer. This event is raised immediately after
        /// drawing for the layer has finished.
        /// </summary>
        public event MapDrawEventHandler OnEndDrawLayer;

        /// <summary>
        /// Notifies listeners when the Map has started drawing a layer. This event is raised immediately before any
        /// drawing for the layer is actually done.
        /// </summary>
        public event MapDrawEventHandler OnStartDrawLayer;

        static Map()
        {
            DrawBackground = true;
            DrawCharacters = true;
            DrawItems = true;
            DrawMapGrhs = true;
        }

        /// <summary>
        /// Map constructor.
        /// </summary>
        /// <param name="mapIndex">Index of the map.</param>
        /// <param name="parent">World the map belongs to.</param>
        /// <param name="graphics">GraphicsDevice to use to construct the atlas for the map.</param>
        public Map(MapIndex mapIndex, World parent, GraphicsDevice graphics) : base(mapIndex, parent)
        {
            _graphics = graphics;
            _world = parent;
        }

        /// <summary>
        /// Gets an IEnumerable of all the BackgroundImages on the Map.
        /// </summary>
        public IEnumerable<BackgroundImage> BackgroundImages
        {
            get { return _backgroundImages; }
        }

        /// <summary>
        /// Gets or sets if the background items are drawn.
        /// </summary>
        public static bool DrawBackground { get; set; }

        /// <summary>
        /// Gets or sets if the character layer is drawn.
        /// </summary>
        public static bool DrawCharacters { get; set; }

        /// <summary>
        /// Gets or sets if the items layer is drawn.
        /// </summary>
        public static bool DrawItems { get; set; }

        /// <summary>
        /// Gets or sets if the map graphics are drawn.
        /// </summary>
        public static bool DrawMapGrhs { get; set; }

        /// <summary>
        /// Gets an IEnumerable of all the MapGrhs on the Map.
        /// </summary>
        public IEnumerable<MapGrh> MapGrhs
        {
            get { return _mapGrhs; }
        }

        /// <summary>
        /// Gets the World the Map belongs to.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        public void AddBackgroundImage(BackgroundImage bgImage)
        {
            Debug.Assert(!_backgroundImages.Contains(bgImage), "BackgroundImage already in the list!");

            _backgroundImages.Add(bgImage);
        }

        /// <summary>
        /// Adds a MapGrh to the map
        /// </summary>
        /// <param name="mg">MapGrh to add to the map</param>
        public void AddMapGrh(MapGrh mg)
        {
            if (mg == null)
            {
                Debug.Fail("mg is null.");
                return;
            }

            // When in debug mode, ensure there are no duplicates
            Debug.Assert(!_mapGrhs.Contains(mg), "mg is already in the MapGrhs list.");

            // Add to the MapGrh list
            _mapGrhs.Add(mg);

            // Listen to the layer change event
            mg.OnChangeRenderLayer += Entity_OnChangeRenderLayer;

            // Set the initial layer
            AddToRenderList(mg);
        }

        /// <summary>
        /// Adds the <paramref name="drawableEntity"/> to the appropriate render layer list
        /// </summary>
        /// <param name="drawableEntity">IDrawableEntity to update</param>
        void AddToRenderList(IDrawable drawableEntity)
        {
            var layerList = GetLayerList(drawableEntity.MapRenderLayer);

            // In debug build, ensure there are no duplicates
            Debug.Assert(!layerList.Contains(drawableEntity), "drawableEntity already in layer list!");

            // Add to the layer list
            layerList.Add(drawableEntity);
        }

        /// <summary>
        /// Constructs an atlas for the map using the given set of GrhIndexes.
        /// </summary>
        /// <param name="grhIndexes">The GrhIndexes.</param>
        void BuildAtlas(IEnumerable<GrhIndex> grhIndexes)
        {
            if (grhIndexes == null || grhIndexes.Count() == 0)
            {
                _mapAtlases = new List<Texture2D>(0);
                return;
            }

            var atlasItems = new List<ITextureAtlasable>();

            // Loop through each index
            foreach (GrhIndex index in grhIndexes)
            {
                GrhData gd = GrhInfo.GetData(index);

                // Every frame of the GrhData gets added
                // For animations, this is one per each frame
                // For stationary, this will just grab a copy of itself from Frames[0]
                foreach (GrhData frame in gd.Frames)
                {
                    if (!atlasItems.Contains(frame))
                        atlasItems.Add(frame);
                }
            }

            // Dispose of the old atlas if needed
            if (_atlas != null && !_atlas.IsDisposed)
                _atlas.Dispose();

            // Generate the atlas out of all the items
            _atlas = new TextureAtlas(_graphics, atlasItems);
        }

        /// <summary>
        /// When overridden in the derived class, creates a new WallEntityBase instance.
        /// </summary>
        /// <returns>WallEntityBase that is to be used on the map.</returns>
        protected override WallEntityBase CreateWall(IValueReader r)
        {
            return new WallEntity(r);
        }

        /// <summary>
        /// Draws the content of the map to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch object used for drawing</param>
        /// <param name="camera">Camera used to find the view area</param>
        public void Draw(SpriteBatch sb, Camera2D camera)
        {
            // Draw the background
            if (OnStartDrawLayer != null)
                OnStartDrawLayer(this, MapRenderLayer.Background, sb, camera, DrawBackground);

            if (DrawBackground)
            {
                foreach (BackgroundImage bgImage in _backgroundImages)
                {
                    bgImage.Draw(sb, camera, Size);
                }
            }

            if (OnEndDrawLayer != null)
                OnEndDrawLayer(this, MapRenderLayer.Background, sb, camera, DrawBackground);

            // Draw the background map graphics (behind the character)
            DrawLayer(sb, camera, _drawLayerBackground, MapRenderLayer.SpriteBackground, DrawMapGrhs);

            // Draw the characters
            DrawLayer(sb, camera, _drawLayerCharacter, MapRenderLayer.Chararacter, DrawCharacters);

            // Draw the items
            DrawLayer(sb, camera, _drawLayerItem, MapRenderLayer.Item, DrawItems);

            // Draw the foreground map graphics (in front of the character)
            DrawLayer(sb, camera, _drawLayerForeground, MapRenderLayer.SpriteForeground, DrawMapGrhs);
        }

        /// <summary>
        /// Draws an IEnumerable of IDrawableEntities.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="camera">Camera to use to check if in view.</param>
        /// <param name="drawableEntities">List of IDrawableEntity objects to draw.</param>
        /// <param name="layer">The MapRenderLayer that is being drawn.</param>
        /// <param name="draw">If true, the layer will be drawn. If false, no drawing will be done.</param>
        void DrawLayer(SpriteBatch sb, Camera2D camera, IEnumerable<IDrawable> drawableEntities, MapRenderLayer layer, bool draw)
        {
            if (OnStartDrawLayer != null)
                OnStartDrawLayer(this, layer, sb, camera, draw);

            if (draw)
            {
                foreach (IDrawable drawableEntity in drawableEntities)
                {
                    if (drawableEntity.InView(camera))
                        drawableEntity.Draw(sb);
                }
            }

            if (OnEndDrawLayer != null)
                OnEndDrawLayer(this, layer, sb, camera, draw);
        }

        /// <summary>
        /// Delegate for when an IDrawableEntity's MapRenderLayer is changed
        /// </summary>
        /// <param name="drawableEntity">IDrawableEntity that changed their MapRenderLayer</param>
        /// <param name="oldLayer">The previous value of MapRenderLayer</param>
        void Entity_OnChangeRenderLayer(IDrawable drawableEntity, MapRenderLayer oldLayer)
        {
            // Remove the IDrawableEntity from the old layer
            if (!GetLayerList(oldLayer).Remove(drawableEntity))
            {
                const string errmsg = "IDrawableEntity `{0}` not found in old layer list `{1}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, drawableEntity, oldLayer);
                Debug.Fail(string.Format(errmsg, drawableEntity, oldLayer));
            }

            // Set the new layer
            AddToRenderList(drawableEntity);
        }

        protected override void EntityAdded(Entity entity)
        {
            // If the entity implements IDrawableEntity, listen to the 
            // render layer change event and add it to the appropriate layer
            IDrawable drawableEntity = entity as IDrawable;
            if (drawableEntity != null)
            {
                drawableEntity.OnChangeRenderLayer += Entity_OnChangeRenderLayer;
                AddToRenderList(drawableEntity);
            }
        }

        protected override void EntityRemoved(Entity entity)
        {
            // If the entity implements IDrawableEntity, remove it from the render layer and
            // unhook the render layer change event listener
            IDrawable drawableEntity = entity as IDrawable;
            if (drawableEntity != null)
            {
                drawableEntity.OnChangeRenderLayer -= Entity_OnChangeRenderLayer;
                if (!GetLayerList(drawableEntity.MapRenderLayer).Remove(drawableEntity))
                {
                    // In debug mode, ensure that the drawableEntity was in the list
                    Debug.Fail("drawableEntity was not in the render layer list!");
                }
            }
        }

        /// <summary>
        /// Finds all duplicate (same position, size and type) walls
        /// </summary>
        /// <param name="compareTo">Walls to compare against</param>
        /// <returns>A list of all duplicate walls in the map</returns>
        public List<WallEntityBase> FindDuplicateWalls(IEnumerable<WallEntityBase> compareTo)
        {
            // List to store all the duplicates
            var ret = new List<WallEntityBase>(32);

            // Loop through each entity in the map
            foreach (Entity entity in Entities)
            {
                // Check if the map is a WallEntity
                WallEntityBase a = entity as WallEntityBase;
                if (a == null)
                    continue;

                // Loop through each WallEntity in the comparison enum
                foreach (WallEntityBase b in compareTo)
                {
                    if (b == null)
                        continue;

                    // Check for duplicate location and collision type
                    if (a.CB.Min == b.CB.Min && a.CB.Max == b.CB.Max && a.CollisionType == b.CollisionType)
                    {
                        // If the return list doesn't yet contain the wall, add it
                        if (!ret.Contains(a))
                            ret.Add(a);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Finds all duplicate (same position, size and type) walls
        /// </summary>
        /// <returns>A list of all duplicate walls in the map</returns>
        public List<WallEntityBase> FindDuplicateWalls()
        {
            var ret = new List<WallEntityBase>();

            // Initial loop (all walls but the last)
            for (int i = 0; i < Entities.Count() - 1; i++)
            {
                WallEntityBase a = GetEntity(i) as WallEntityBase; // Skip if not a WallEntity
                if (a == null)
                    continue;

                // Comparison loop (all walls after the initial)
                for (int j = i + 1; j < Entities.Count(); j++)
                {
                    WallEntityBase b = GetEntity(j) as WallEntityBase; // Skip if not a WallEntity
                    if (b == null)
                        continue;

                    // Check for a match and the wall not already in the return list
                    if (a.CB.Min == b.CB.Min && a.CB.Max == b.CB.Max && a.CollisionType == b.CollisionType)
                    {
                        if (!ret.Contains(b))
                            ret.Add(b);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the List of IDrawableEntity for a specified layer
        /// </summary>
        /// <param name="layer">Layer for the List we want</param>
        /// <returns>List for the specified MapRenderLayer</returns>
        List<IDrawable> GetLayerList(MapRenderLayer layer)
        {
            switch (layer)
            {
                case MapRenderLayer.SpriteBackground:
                    return _drawLayerBackground;
                case MapRenderLayer.Chararacter:
                    return _drawLayerCharacter;
                case MapRenderLayer.Item:
                    return _drawLayerItem;
                case MapRenderLayer.SpriteForeground:
                    return _drawLayerForeground;
                default:
                    const string errmsg = "Could not get the list for unknown layer `{0}`.";
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, layer);
                    Debug.Fail(string.Format(errmsg, layer));
                    throw new ArgumentOutOfRangeException("layer", string.Format(errmsg, layer));
            }
        }

        /// <summary>
        /// Gets a MapGrh matching specified conditions
        /// </summary>
        /// <param name="p">Point on the map contained in the MapGrh</param>
        /// <returns>First MapGrh meeting the condition, null if none found</returns>
        public MapGrh GetMapGrh(Vector2 p)
        {
            foreach (MapGrh grh in MapGrhs)
            {
                if (grh.HitTest(p))
                    return grh;
            }
            return null;
        }

        /// <summary>
        /// Gets an IEnumerable of the GrhIndexes used in the map.
        /// </summary>
        /// <returns>An IEnumerable of the GrhIndexes used in the map.</returns>
        IEnumerable<GrhIndex> GetMapGrhList()
        {
            return _mapGrhs.Select(x => x.Grh.GrhData.GrhIndex).Distinct();
        }

        void LoadBackgroundImages(IValueReader r)
        {
            int currentTime = World.GetTime();
            var loadedBGImages = r.ReadManyNodes<BackgroundImage>(_bgImagesNodeName, x => new BackgroundLayer(x, currentTime));

            // Add the loaded background images
            foreach (BackgroundImage bgImage in loadedBGImages)
            {
                AddBackgroundImage(bgImage);
            }
        }

        void LoadGrhs(IValueReader r)
        {
            IValueReader nodeReader = r.ReadNode(_mapGrhsNodeName);

            // Used GrhIndexes
            var usedGrhIndexes = nodeReader.ReadMany(_usedIndiciesNodeName, ((reader, key) => reader.ReadGrhIndex(key)));
            BuildAtlas(usedGrhIndexes);

            // MapGrhs
            int currentTime = World.GetTime();
            var loadedMapGrhs = nodeReader.ReadManyNodes(_mapGrhsNodeName, x => new MapGrh(x, currentTime));
            foreach (MapGrh mapGrh in loadedMapGrhs)
            {
                AddMapGrh(mapGrh);
            }
        }

        /// <summary>
        /// Handles loading of custom values.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the misc values from.</param>
        protected override void LoadMisc(IValueReader reader)
        {
            LoadGrhs(reader);
            LoadBackgroundImages(reader);
        }

        /// <summary>
        /// Removes a MapGrh from the map
        /// </summary>
        /// <param name="mg">MapGrh to remove from the map</param>
        public void RemoveMapGrh(MapGrh mg)
        {
            if (mg == null)
            {
                Debug.Fail("mg is null.");
                return;
            }

            // Remove the listener
            mg.OnChangeRenderLayer -= Entity_OnChangeRenderLayer;

            // Remove from the MapGrhs list
            if (!_mapGrhs.Remove(mg))
            {
                // In debug build, ensure the MapGrh was in the MapGrhs list
                Debug.Fail("mg wasn't in the MapGrhs list.");
            }

            // Remove from the render layer
            if (!GetLayerList(mg.MapRenderLayer).Remove(mg))
            {
                // In debug build, ensure the MapGrh was in the render layer list in the first place
                Debug.Fail("mg wasn't in the render layer list.");
            }
        }

        /// <summary>
        /// Writes all the BackgroundImages.
        /// </summary>
        /// <param name="w">IValueWriter to write to..</param>
        void SaveBackgroundImages(IValueWriter w)
        {
            var bgImagesToWrite = _backgroundImages.Where(x => x != null);
            w.WriteManyNodes(_bgImagesNodeName, bgImagesToWrite, ((writer, item) => item.Write(writer)));
        }

        /// <summary>
        /// Writes all the MapGrhs to a XmlWriter.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        void SaveGrhs(IValueWriter w)
        {
            w.WriteStartNode(_mapGrhsNodeName);
            {
                // Used GrhIndexes
                w.WriteMany(_usedIndiciesNodeName, GetMapGrhList(), w.Write);

                // MapGrhs
                w.WriteManyNodes(_mapGrhsNodeName, _mapGrhs, ((writer, item) => item.Write(writer)));
            }
            w.WriteEndNode(_mapGrhsNodeName);
        }

        /// <summary>
        /// When overridden in the derived class, saves misc map information specific to the derived class.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        protected override void SaveMisc(IValueWriter w)
        {
            SaveGrhs(w);
            SaveBackgroundImages(w);
        }

        /// <summary>
        /// Updates the map
        /// </summary>
        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);

            int currentTime = World.GetTime();

            // Update the map Grhs
            foreach (MapGrh g in _mapGrhs)
            {
                if (World.Camera.InView(g.Grh, g.Destination))
                    g.Update(currentTime);
            }

            // Update the background images
            foreach (BackgroundImage bgImage in _backgroundImages)
            {
                bgImage.Update(currentTime);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Unloads the garbage created by the map (ie atlas textures)
        /// </summary>
        public void Dispose()
        {
            if (_atlas != null && !_atlas.IsDisposed)
                _atlas.Dispose();

            foreach (Texture2D atlas in _mapAtlases)
            {
                if (atlas != null && !atlas.IsDisposed)
                    atlas.Dispose();
            }

            _mapAtlases.Clear();
        }

        #endregion
    }
}