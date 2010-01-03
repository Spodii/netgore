using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Handler for Map drawing events.
    /// </summary>
    /// <param name="map">Map that the drawing is taking place on.</param>
    /// <param name="layer">The layer that the drawing event is related to.</param>
    /// <param name="spriteBatch">The SpriteBatch that was used to do the drawing.</param>
    /// <param name="camera">The camera that was used in the drawing.</param>
    /// <param name="isDrawing">If the <paramref name="layer"/> is actually being drawn by the <paramref name="map"/>. If
    /// false, it is time for the <paramref name="layer"/> to be drawn, but the <paramref name="map"/> will not actually
    /// draw the layer.</param>
    public delegate void MapDrawEventHandler(
        Map map, MapRenderLayer layer, SpriteBatch spriteBatch, ICamera2D camera, bool isDrawing);

    /// <summary>
    /// Map object for the client
    /// </summary>
    public class Map : MapBase, IDisposable
    {
        const string _bgImagesNodeName = "BackgroundImages";
        const string _mapGrhsNodeName = "MapGrhs";
        const string _particleEffectsNodeName = "ParticleEffects";
        const string _usedIndiciesNodeName = "UsedIndicies";
        static readonly SpriteBatchRenderer _particleEffectRenderer = new SpriteBatchRenderer();

        /// <summary>
        /// List of BackgroundImages on this map.
        /// </summary>
        readonly List<BackgroundImage> _backgroundImages = new List<BackgroundImage>();

        /// <summary>
        /// Graphics device used when building the atlas
        /// </summary>
        readonly GraphicsDevice _graphics;

        /// <summary>
        /// List of map grhs on the map
        /// </summary>
        readonly List<MapGrh> _mapGrhs = new List<MapGrh>(128);

        readonly MapParticleEffectCollection _particleEffects = new MapParticleEffectCollection();

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
        /// Gets the <see cref="MapParticleEffectCollection"/> containing the particle effects for the map.
        /// </summary>
        public MapParticleEffectCollection ParticleEffects
        {
            get { return _particleEffects; }
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

            // Add to the MapGrh list and spatial
            _mapGrhs.Add(mg);
            Spatial.Add(mg);
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
        /// Gets or sets a filter to be used when determining what components on the map will be drawn. If null,
        /// all components will be drawn.
        /// </summary>
        public Func<IDrawable, bool> DrawFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Draws the content of the map to the screen.
        /// </summary>
        /// <param name="sb">SpriteBatch object used for drawing.</param>
        /// <param name="camera">Camera used to find the view area.</param>
        public void Draw(SpriteBatch sb, ICamera2D camera)
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

            // Find the drawable objects that are in view
            var viewArea = camera.GetViewArea();
            var drawableInView = Spatial.GetEntities<IDrawable>(viewArea);

            // TODO: !! This is a shitty filtering. Would be better if I declare a public predicate to determine the filtering. Much more powerful that way.
            if (!DrawBackground)
                drawableInView = drawableInView.Where(x => x.MapRenderLayer != MapRenderLayer.Background);
            if (!DrawCharacters)
                drawableInView = drawableInView.Where(x => x.MapRenderLayer != MapRenderLayer.Chararacter);
            if (!DrawItems)
                drawableInView = drawableInView.Where(x => x.MapRenderLayer != MapRenderLayer.Item);

            if (!DrawMapGrhs)
            {
                drawableInView =
                    drawableInView.Where(
                        x =>
                        x.MapRenderLayer != MapRenderLayer.SpriteBackground && x.MapRenderLayer != MapRenderLayer.SpriteForeground);
            }

            // Sort the drawable items then draw them one by one
            var sorted = drawableInView.OrderBy(x => x.MapRenderLayer);

            foreach (var drawable in sorted)
            {
                drawable.Draw(sb);
            }

            // Draw the particle effects
            _particleEffectRenderer.SpriteBatch = sb;
            _particleEffectRenderer.Draw(camera, ParticleEffects);
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
                    if (a.Position == b.Position && a.Size == b.Size)
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
                    if (a.Position == b.Position && a.Size == b.Size)
                    {
                        if (!ret.Contains(b))
                            ret.Add(b);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets an IEnumerable of the GrhIndexes used in the map.
        /// </summary>
        /// <returns>An IEnumerable of the GrhIndexes used in the map.</returns>
        IEnumerable<GrhIndex> GetMapGrhList()
        {
            return _mapGrhs.Select(x => x.Grh.GrhData.GrhIndex).Distinct().OrderBy(x => x);
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for. This should include
        /// all the <see cref="Type"/>s that are used frequently when querying the map's spatial collection.
        /// </summary>
        /// <returns>
        /// An IEnumerable of the <see cref="Type"/>s to build <see cref="ISpatialCollection"/>s for.
        /// </returns>
        protected override IEnumerable<Type> GetSpatialTypes()
        {
            return base.GetSpatialTypes().Concat(new Type[] { typeof(MapGrh) });
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
            _particleEffects.Read(reader, _particleEffectsNodeName);
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

            // Remove from the MapGrhs list and spatial
            if (!_mapGrhs.Remove(mg))
            {
                // In debug build, ensure the MapGrh was in the MapGrhs list
                Debug.Fail("mg wasn't in the MapGrhs list.");
            }

            Spatial.Remove(mg);
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
            _particleEffects.Write(w, _particleEffectsNodeName);
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
                if (World.Camera.InView(g.Grh, g.Position))
                    g.Update(currentTime);
            }

            // Update the background images
            foreach (BackgroundImage bgImage in _backgroundImages)
            {
                bgImage.Update(currentTime);
            }

            // Update particle effects
            foreach (var p in ParticleEffects)
            {
                p.Update(currentTime);
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