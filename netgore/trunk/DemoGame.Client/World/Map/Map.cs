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
    /// Map object for the client
    /// </summary>
    public class Map : MapBase, IDisposable, IDrawableMap
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

        readonly DrawableSorter _drawableSorter = new DrawableSorter();

        /// <summary>
        /// Graphics device used when building the atlas
        /// </summary>
        readonly GraphicsDevice _graphics;

        /// <summary>
        /// List of map grhs on the map
        /// </summary>
        readonly List<MapGrh> _mapGrhs = new List<MapGrh>(128);

        readonly MapParticleEffectCollection _particleEffects = new MapParticleEffectCollection();

        TextureAtlas _atlas;
        ICamera2D _camera;

        /// <summary>
        /// List of atlas textures used for the graphics for the map
        /// </summary>
        List<Texture2D> _mapAtlases = new List<Texture2D>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="mapIndex">Index of the map.</param>
        /// <param name="camera">The camera used to view the map.</param>
        /// <param name="getTime">The object used to get the current time.</param>
        /// <param name="graphics">GraphicsDevice to use to construct the atlas for the map.</param>
        public Map(MapIndex mapIndex, ICamera2D camera, IGetTime getTime, GraphicsDevice graphics) : base(mapIndex, getTime)
        {
            _camera = camera;
            _graphics = graphics;
        }

        /// <summary>
        /// Gets an IEnumerable of all the BackgroundImages on the Map.
        /// </summary>
        public IEnumerable<BackgroundImage> BackgroundImages
        {
            get { return _backgroundImages; }
        }

        /// <summary>
        /// Gets or sets a filter to be used when determining what components on the map will be drawn. If null,
        /// all components will be drawn.
        /// </summary>
        public Func<IDrawable, bool> DrawFilter { get; set; }

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
        /// Adds the <paramref name="bgImage"/> to the map.
        /// </summary>
        /// <param name="bgImage">The <see cref="BackgroundImage"/> to add.</param>
        public void AddBackgroundImage(BackgroundImage bgImage)
        {
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

            // First, grab the GrhData for each GrhIndex, making sure to skip null GrhDatas. Then,
            // grab all the frames and add them. Stationary GrhDatas will end up adding their self, while
            // animated ones will add all their frames, so this will end up adding them all no matter the type.
            // Finally, use Distinct to ensure we have no duplicates.
            var validGrhDatas = grhIndexes.Select(x => GrhInfo.GetData(x)).Where(x => x != null);
            var atlasItems = validGrhDatas.SelectMany(x => x.Frames);
            atlasItems = atlasItems.Distinct();

            // Dispose of the old atlas if needed
            if (_atlas != null && !_atlas.IsDisposed)
                _atlas.Dispose();

            // Generate the atlas out of all the items
            _atlas = new TextureAtlas(_graphics, atlasItems.Cast<ITextureAtlasable>());
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
        /// Finds all duplicate (same position, size and type) walls
        /// </summary>
        /// <param name="compareTo">Walls to compare against</param>
        /// <returns>A list of all duplicate walls in the map</returns>
        public IEnumerable<WallEntityBase> FindDuplicateWalls(IEnumerable<WallEntityBase> compareTo)
        {
            // List to store all the duplicates
            var ret = new List<WallEntityBase>(32);

            // Loop through each entity in the map
            foreach (var a in Entities.OfType<WallEntityBase>())
            {
                // Loop through each WallEntity in the comparison enum
                foreach (WallEntityBase b in compareTo)
                {
                    // Check for duplicate location and collision type
                    if (a.Position == b.Position && a.Size == b.Size && a.GetType() == b.GetType())
                        ret.Add(a);
                }
            }

            return ret.Distinct();
        }

        /// <summary>
        /// Finds all duplicate <see cref="WallEntityBase"/>s.
        /// </summary>
        /// <returns>A list of all duplicate walls in the map.</returns>
        public IEnumerable<WallEntityBase> FindDuplicateWalls()
        {
            var ret = new List<WallEntityBase>();

            // Initial loop (all walls but the last)
            foreach (var wall in Entities.OfType<WallEntityBase>())
            {
                // Get the overlapping walls
                foreach (var wall2 in Spatial.GetMany<WallEntityBase>(wall))
                {
                    if (wall.Position == wall2.Position && wall.Size == wall2.Size && wall.GetType() == wall2.GetType())
                        ret.Add(wall2);
                }
            }

            return ret.Distinct();
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
            var loadedBGImages = r.ReadManyNodes<BackgroundImage>(_bgImagesNodeName, x => new BackgroundLayer(this, this, x));

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
            int currentTime = GetTime();
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
        /// Removes the <paramref name="bgImage"/> from the map.
        /// </summary>
        /// <param name="bgImage">The <see cref="BackgroundImage"/> to remove.</param>
        /// <returns>True if successfully removed; otherwise false.</returns>
        public bool RemoveBackgroundImage(BackgroundImage bgImage)
        {
            return _backgroundImages.Remove(bgImage);
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

            int currentTime = GetTime();

            // Update the map Grhs
            foreach (MapGrh g in _mapGrhs)
            {
                if (Camera.InView(g.Grh, g.Position))
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

        #region IDrawableMap Members

        /// <summary>
        /// Notifies listeners immediately before a layer has started drawing.
        /// </summary>
        public event MapDrawEventHandler OnBeginDrawLayer;

        /// <summary>
        /// Notifies listeners immediately after a layer has finished drawing.
        /// </summary>
        public event MapDrawEventHandler OnEndDrawLayer;

        /// <summary>
        /// Draws the content of the map to the screen.
        /// </summary>
        /// <param name="sb">SpriteBatch object used for drawing.</param>
        public void Draw(SpriteBatch sb)
        {
            // Find the drawable objects that are in view and pass the filter (if one is provided)
            var viewArea = Camera.GetViewArea();
            IEnumerable<IDrawable> drawableInView;
            if (DrawFilter != null)
                drawableInView = Spatial.GetMany<IDrawable>(viewArea, x => DrawFilter(x));
            else
                drawableInView = Spatial.GetMany<IDrawable>(viewArea);

            // Concat the background images (to the start of the list) since they aren't in any spatials
            var bgInView = _backgroundImages.Cast<IDrawable>().Where(x => x.InView(Camera));
            drawableInView = bgInView.Concat(drawableInView);

            // Sort all the items, then start drawing them layer-by-layer, item-by-item
            foreach (var layer in _drawableSorter.GetSorted(drawableInView))
            {
                if (OnBeginDrawLayer != null)
                    OnBeginDrawLayer(this, layer.Key, sb);

                foreach (var drawable in layer.Value)
                {
                    drawable.Draw(sb);
                }

                if (OnEndDrawLayer != null)
                    OnEndDrawLayer(this, layer.Key, sb);
            }

            // Draw the particle effects
            _particleEffectRenderer.SpriteBatch = sb;
            _particleEffectRenderer.Draw(Camera, ParticleEffects);
        }

        /// <summary>
        /// Gets or sets the <see cref="ICamera2D"/> used to view the map.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ICamera2D Camera
        {
            get { return _camera; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _camera = value;
            }
        }

        #endregion
    }
}