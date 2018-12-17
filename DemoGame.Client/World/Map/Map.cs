using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Features.GameTime;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Map object for the client
    /// </summary>
    public class Map : MapBase, IDisposable, IDrawableMap
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _bgImagesNodeName = "BackgroundImages";
        const string _lightingNodeName = "Lighting";
        const string _lightsNodeName = "Lights";
        const string _mapGrhsNodeName = "MapGrhs";
        const string _particleEffectsNodeName = "ParticleEffects";
        const string _refractionEffectListNodeName = "RefractionEffects";
        const string _refractionEffectsNodeName = "Refraction";
        const string _usedIndiciesNodeName = "UsedIndicies";

        static readonly GameTimeSettings _gameTimeSettings = GameTimeSettings.Instance;

        readonly List<BackgroundImage> _backgroundImages = new List<BackgroundImage>();
        readonly DrawableSorter _drawableSorter = new DrawableSorter();
        readonly List<ILight> _lights = new List<ILight>();
        readonly List<ITemporaryMapEffect> _mapEffects = new List<ITemporaryMapEffect>(32);
        readonly MapParticleEffectCollection _particleEffects = new MapParticleEffectCollection();
        readonly List<IRefractionEffect> _refractionEffects = new List<IRefractionEffect>();

        Color _ambientLight = Color.White;
        TextureAtlas _atlas;
        ICamera2D _camera;

        /// <summary>
        /// List of atlas textures used for the graphics for the map
        /// </summary>
        List<Texture> _mapAtlases = new List<Texture>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="mapID">The ID of the map.</param>
        /// <param name="camera">The camera used to view the map.</param>
        /// <param name="getTime">The object used to get the current time.</param>
        public Map(MapID mapID, ICamera2D camera, IGetTime getTime) : base(mapID, getTime)
        {
            _camera = camera;

            DrawParticles = true;
        }

        /// <summary>
        /// Notifies listeners when this object has been disposed.
        /// </summary>
        [Browsable(false)]
        public event TypedEventHandler<Map> Disposed;

        /// <summary>
        /// Gets or sets the ambient light color.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        [DefaultValue(typeof(Color), "{255, 255, 255, 255}")]
        [Description("The default ambient light color. The alpha value is unused and will always be 255.")]
        public Color AmbientLight
        {
            get { return _ambientLight; }
            set { _ambientLight = new Color(value.R, value.G, value.B, 255); }
        }

        /// <summary>
        /// Gets an IEnumerable of all the BackgroundImages on the Map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<BackgroundImage> BackgroundImages
        {
            get { return _backgroundImages; }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets the lights on the map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<ILight> Lights
        {
            get { return _lights; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="ITemporaryMapEffect"/>s.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<ITemporaryMapEffect> MapEffects
        {
            get { return _mapEffects; }
        }

        /// <summary>
        /// Gets the <see cref="MapParticleEffectCollection"/> containing the particle effects for the map.
        /// </summary>
        [Browsable(true)]
        [Description("The particle effects attached to the map.")]
        [DisplayName("Particle Effects")]
        [Category("Map")]
        public MapParticleEffectCollection ParticleEffects
        {
            get { return _particleEffects; }
        }

        /// <summary>
        /// Gets the refraction effects on the map.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<IRefractionEffect> RefractionEffects
        {
            get { return _refractionEffects; }
        }

        /// <summary>
        /// Gets or sets the size of the map. This properly is only added for usage in editors.
        /// </summary>
        [Browsable(true)]
        [Description("The size of the map in pixels.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DisplayName("Size")]
        [Category("Map")]
        public Vector2 _Size
        {
            get { return Size; }
            set { SetDimensions(value); }
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
        /// Adds a light to the map as long as it does not already exist in the map's light collection.
        /// </summary>
        /// <param name="light">The <see cref="ILight"/> to add.</param>
        public void AddLight(ILight light)
        {
            if (!_lights.Contains(light))
                _lights.Add(light);
        }

        /// <summary>
        /// Adds a MapGrh to the map
        /// </summary>
        /// <param name="mg">MapGrh to add to the map</param>
        public void AddMapGrh(MapGrh mg)
        {
            if (mg == null)
            {
                const string errmsg = "Parameter mg is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return;
            }

            // When in debug mode, ensure there are no duplicates
            Debug.Assert(!Spatial.CollectionContains(mg), string.Format("MapGrh `{0}` is already in the spatial collection.", mg));

            // Add to the spatial
            Spatial.Add(mg);
        }

        /// <summary>
        /// Adds a refraction effect to the map as long as it does not already exist in the map's refraction effect collection.
        /// </summary>
        /// <param name="fx">The <see cref="IRefractionEffect"/> to add.</param>
        public void AddRefractionEffect(IRefractionEffect fx)
        {
            if (!_refractionEffects.Contains(fx))
                _refractionEffects.Add(fx);
        }

        /// <summary>
        /// Constructs an atlas for the map using the given set of GrhIndexes.
        /// </summary>
        /// <param name="grhIndexes">The GrhIndexes.</param>
        void BuildAtlas(IEnumerable<GrhIndex> grhIndexes)
        {
            if (grhIndexes == null || grhIndexes.IsEmpty())
            {
                _mapAtlases = new List<Texture>(0);
                return;
            }

            // First, grab the GrhData for each GrhIndex, making sure to skip null GrhDatas. Then,
            // grab all the frames and add them. Stationary GrhDatas will end up adding their self, while
            // animated ones will add all their frames, so this will end up adding them all no matter the type.
            // Finally, use Distinct to ensure we have no duplicates.
            var validGrhDatas = grhIndexes.Select(GrhInfo.GetData).Where(x => x != null);
            var atlasItems = validGrhDatas.SelectMany(x => x.Frames);
            atlasItems = atlasItems.Distinct();

            // Dispose of the old atlas if needed
            if (_atlas != null && !_atlas.IsDisposed)
                _atlas.Dispose();

            // Generate the atlas out of all the items
            _atlas = new TextureAtlas(atlasItems);
        }

        /// <summary>
        /// Clears out all the map's contents.
        /// </summary>
        protected override void Clear()
        {
            _particleEffects.Clear();
            _refractionEffects.Clear();

            foreach (var light in Lights.ToArray())
                RemoveLight(light);

            foreach (var bgImage in BackgroundImages.ToArray())
                RemoveBackgroundImage(bgImage);

            base.Clear();
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
        /// Finds all <see cref="WallEntityBase"/>s where <see cref="WallEntityBase.AreValuesEqual"/> are true with another
        /// set of <see cref="WallEntityBase"/>s.
        /// </summary>
        /// <param name="compareTo">Walls to compare against.</param>
        /// <returns>A list of all duplicate walls in the map. The <see cref="WallEntityBase"/> on the map is the one returned, not
        /// the references in the <paramref name="compareTo"/> parameter.</returns>
        public IEnumerable<WallEntityBase> FindDuplicateWalls(IEnumerable<WallEntityBase> compareTo)
        {
            // List to store all the duplicates
            var ret = new List<WallEntityBase>(32);

            // Loop through the supplied list of walls
            foreach (var cmpToTemp in compareTo)
            {
                // Copy the iterated variable to a local... ReSharper told me to ;)
                var cmpTo = cmpToTemp;

                // Get the area of walls to look for, adding some padding just in case there are rounding issues
                var cmpToArea = new Rectangle(cmpTo.Position.X - 2, cmpTo.Position.Y - 2, 4, 4);

                // Check for any matches by comparing to the map walls in the area
                var matches = Spatial.GetMany<WallEntityBase>(cmpToArea, x => x.AreValuesEqual(cmpTo));

                // Add all matches to the return list
                ret.AddRange(matches);
            }

            // Return the distinct list of matches
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
            return Spatial.GetMany<MapGrh>().Select(x => x.Grh.GrhData.GrhIndex).OrderBy(x => x).Distinct().ToImmutable();
        }

        /// <summary>
        /// Gets the ambient light color for the map based on whether or not the map is indoors
        /// and the current game-time.
        /// </summary>
        /// <returns>The ambient light color for the map based on whether or not the map is indoors
        /// and the current game-time.</returns>
        public Color GetModifiedAmbientLight()
        {
            // Indoor maps are not affected by time
            if (Indoors)
                return AmbientLight;

            return _gameTimeSettings.GetModifiedAmbientLight(AmbientLight);
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
            // Remove existing
            _backgroundImages.Clear();

            var loadedBGImages = r.ReadManyNodes<BackgroundImage>(_bgImagesNodeName, x => new BackgroundLayer(this, this, x));

            // Add the loaded background images
            foreach (var bgImage in loadedBGImages)
            {
                AddBackgroundImage(bgImage);
            }
        }

        void LoadGrhs(IValueReader reader)
        {
            reader = reader.ReadNode(_mapGrhsNodeName);

            // Used GrhIndexes
            var usedGrhIndexes = reader.ReadMany(_usedIndiciesNodeName, ((r, key) => r.ReadGrhIndex(key)));
            BuildAtlas(usedGrhIndexes);

            // MapGrhs
            var currentTime = GetTime();
            var loadedMapGrhs = reader.ReadManyNodes(_mapGrhsNodeName, x => new MapGrh(x, currentTime));
            foreach (var mapGrh in loadedMapGrhs)
            {
                AddMapGrh(mapGrh);
            }
        }

        void LoadLighting(IValueReader reader)
        {
            reader = reader.ReadNode(_lightingNodeName);

            AmbientLight = reader.ReadColor("Ambient");

            var loadedLights = reader.ReadManyNodes(_lightsNodeName, x => new Light(x));
            _lights.AddRange(loadedLights);

            foreach (var light in loadedLights)
            {
                light.Tag = this;
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
            LoadLighting(reader);
            _particleEffects.Read(reader, _particleEffectsNodeName);
            LoadRefractionEffects(reader);
        }

        void LoadRefractionEffects(IValueReader reader)
        {
            reader = reader.ReadNode(_refractionEffectsNodeName);

            var loadedFx = reader.ReadManyNodes(_refractionEffectListNodeName, RefractionEffectFactory.Read);

            _refractionEffects.AddRange(loadedFx);

            foreach (var fx in loadedFx)
            {
                fx.Tag = this;
            }
        }

        public void RemoveBackgroundImage(IEnumerable<BackgroundImage> bgImages)
        {
            foreach (var bgImage in bgImages)
                RemoveBackgroundImage(bgImage);
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
        /// Removes a light from the map.
        /// </summary>
        /// <param name="light">The <see cref="ILight"/> to add.</param>
        /// <returns>True if the <paramref name="light"/> was removed; false if the <paramref name="light"/> was not
        /// in the map's light collection and thus not removed.</returns>
        public bool RemoveLight(ILight light)
        {
            return _lights.Remove(light);
        }

        public void RemoveLight(IEnumerable<IRefractionEffect> fxs)
        {
            foreach (var fx in fxs)
                RemoveLight(fx);
        }

        /// <summary>
        /// Removes a refraction effect from the map.
        /// </summary>
        /// <param name="fx">The <see cref="IRefractionEffect"/> to add.</param>
        /// <returns>True if the <paramref name="fx"/> was removed; false if the <paramref name="fx"/> was not
        /// in the map's refraction effect collection and thus not removed.</returns>
        public bool RemoveLight(IRefractionEffect fx)
        {
            return _refractionEffects.Remove(fx);
        }

        public void RemoveMapGrh(IEnumerable<MapGrh> mgs)
        {
            foreach (var mg in mgs)
                RemoveMapGrh(mg);
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

            Debug.Assert(Spatial.CollectionContains(mg), string.Format("MapGrh `{0}` isn't in the spatial collection.", mg));

            // Remove from the spatial
            Spatial.Remove(mg);
        }

        /// <summary>
        /// Writes all the BackgroundImages.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        void SaveBackgroundImages(IValueWriter w, MapSaveFlags saveFlags)
        {
            var bgImages = _backgroundImages.Where(x => x != null);
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                bgImages = bgImages.OrderBy(x => x.Sprite.GrhData.GrhIndex).ThenBy(x => (int)x.MapRenderLayer);
            }
            bgImages = bgImages.ToImmutable();

            w.WriteManyNodes(_bgImagesNodeName, bgImages, ((writer, item) => item.Write(writer)));
        }

        /// <summary>
        /// Writes all the MapGrhs to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        void SaveGrhs(IValueWriter w, MapSaveFlags saveFlags)
        {
            var mapGrhs = Spatial.GetMany<MapGrh>();
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                mapGrhs = mapGrhs.OrderBy(x => x, new BasicMapGrhComparer());
            }
            mapGrhs = mapGrhs.ToImmutable();

            w.WriteStartNode(_mapGrhsNodeName);
            {
                // Used GrhIndexes
                w.WriteMany(_usedIndiciesNodeName, GetMapGrhList(), w.Write);

                // MapGrhs
                w.WriteManyNodes(_mapGrhsNodeName, mapGrhs, ((writer, mapGrh) => mapGrh.WriteState(writer)));
            }
            w.WriteEndNode(_mapGrhsNodeName);
        }

        /// <summary>
        /// Writes all the lighting to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        void SaveLighting(IValueWriter w, MapSaveFlags saveFlags)
        {
            IEnumerable<ILight> lights = _lights;
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                lights = lights.OrderBy(x => x, new BasicSpatialComparer());
            }
            lights = lights.ToImmutable();

            w.WriteStartNode(_lightingNodeName);
            {
                w.Write("Ambient", AmbientLight);
                w.WriteManyNodes(_lightsNodeName, lights, (wr, l) => l.WriteState(wr));
            }
            w.WriteEndNode(_lightingNodeName);
        }

        /// <summary>
        /// When overridden in the derived class, saves misc map information specific to the derived class.
        /// </summary>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="saveFlags">The flags to modify the map save behavior.</param>
        protected override void SaveMisc(IValueWriter w, MapSaveFlags saveFlags)
        {
            SaveGrhs(w, saveFlags);
            SaveBackgroundImages(w, saveFlags);
            SaveLighting(w, saveFlags);
            SaveParticleEffects(w, saveFlags);
            SaveRefractionEffects(w, saveFlags);
        }

        void SaveRefractionEffects(IValueWriter w, MapSaveFlags saveFlags)
        {
            var validFx = _refractionEffects.Where(x => RefractionEffectFactory.IsValidType(x.GetType()));
            if ((saveFlags & MapSaveFlags.DoNotSort) == 0)
            {
                validFx = validFx.OrderBy(x => x, new BasicSpatialComparer());
            }
            validFx = validFx.ToImmutable();

            w.WriteStartNode(_refractionEffectsNodeName);
            {
                w.WriteManyNodes(_refractionEffectListNodeName, validFx, RefractionEffectFactory.Write);
            }
            w.WriteEndNode(_refractionEffectsNodeName);
        }

        void SaveParticleEffects(IValueWriter w, MapSaveFlags saveFlags)
        {
            _particleEffects.Write(w, _particleEffectsNodeName);
        }

        /// <summary>
        /// Updates the map
        /// </summary>
        /// <param name="deltaTime">The elapsed time since the last update.</param>
        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);

            var currentTime = GetTime();

            // Grab the view area
            var viewRect = Camera.GetViewArea();

            // Expand the view area a bit to update stuff just out of view
            viewRect.Inflate(32, 32);

            // Update the map Grhs in view
            foreach (var mg in Spatial.GetMany<MapGrh>(viewRect))
            {
                mg.Update(currentTime);
            }

            // Update the temporary map effects
            UpdateTempMapEffects(currentTime);

            // Update the background images
            foreach (var bgImage in _backgroundImages)
            {
                bgImage.Update(currentTime);
            }

            // Update particle effects
            foreach (var p in ParticleEffects)
            {
                p.Update(currentTime);
            }
        }

        /// <summary>
        /// Updates the <see cref="ITemporaryMapEffect"/>s.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void UpdateTempMapEffects(TickCount currentTime)
        {
            // Loop through all the live effects
            for (var i = 0; i < _mapEffects.Count; i++)
            {
                var e = _mapEffects[i];
                e.Update(currentTime);

                // Remove dead elements by swapping them with the last element in the list, then removing the last element
                // This allows us to remove elements without shifting down the whole list
                if (!e.IsAlive)
                {
                    // Swap with last element
                    _mapEffects[i] = _mapEffects[_mapEffects.Count - 1];

                    // Remove last element in the list
                    _mapEffects.RemoveAt(_mapEffects.Count - 1);
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            try
            {
                // Clear the atlas (if one exists)
                if (_atlas != null && !_atlas.IsDisposed)
                {
                    try
                    {
                        _atlas.Dispose();
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to dispose `{0}`. Exception: {1}";
                        if (log.IsWarnEnabled)
                            log.WarnFormat(errmsg, _atlas, ex);
                        Debug.Fail(string.Format(errmsg, _atlas, ex));
                    }
                }

                if (_mapAtlases != null)
                {
                    // Clear the map atlas images
                    foreach (var atlas in _mapAtlases)
                    {
                        try
                        {
                            if (atlas != null && !atlas.IsDisposed)
                                atlas.Dispose();
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Failed to dispose `{0}`. Exception: {1}";
                            if (log.IsWarnEnabled)
                                log.WarnFormat(errmsg, atlas, ex);
                            Debug.Fail(string.Format(errmsg, atlas, ex));
                        }
                    }

                    // Clear the atlas list
                    _mapAtlases.Clear();
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to dispose `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
            }

            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region IDrawableMap Members

        /// <summary>
        /// Notifies listeners immediately before any of the map's layers are drawn.
        /// </summary>
        public event TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> BeginDrawMap;

        /// <summary>
        /// Notifies listeners immediately before a layer has started drawing.
        /// </summary>
        public event TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> BeginDrawMapLayer;

        /// <summary>
        /// Notifies listeners immediately after any of the map's layers are drawn.
        /// </summary>
        public event TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> EndDrawMap;

        /// <summary>
        /// Notifies listeners immediately after a layer has finished drawing.
        /// </summary>
        public event TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> EndDrawMapLayer;

        /// <summary>
        /// Gets or sets the <see cref="ICamera2D"/> used to view the map.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        [Browsable(false)]
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

        /// <summary>
        /// Gets or sets a filter to be used when determining what components on the map will be drawn.
        /// If null, all components will be drawn (same as returning true for each value).
        /// </summary>
        [Browsable(false)]
        public Func<IDrawable, bool> DrawFilter { get; set; }

        /// <summary>
        /// Gets or sets if the particle effects should be drawn.
        /// </summary>
        [Browsable(false)]
        public bool DrawParticles { get; set; }

        /// <summary>
        /// Adds a <see cref="ITemporaryMapEffect"/> to the map.
        /// </summary>
        /// <param name="e">The <see cref="ITemporaryMapEffect"/> to add.</param>
        public void AddTemporaryMapEffect(ITemporaryMapEffect e)
        {
            if (e == null)
            {
                Debug.Fail("e is null.");
                return;
            }

            // When in debug mode, ensure there are no duplicates
            Debug.Assert(!_mapEffects.Contains(e), "e is already in the MapEffects list.");

            // Add to the MapEffects list and the spatial
            _mapEffects.Add(e);
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw with.</param>
        [Browsable(false)]
        public void Draw(ISpriteBatch sb)
        {
            var cam = Camera;
            if (cam == null)
            {
                const string errmsg = "The camera on map `{0}` was null - cannot draw map.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            // Find the drawable objects that are in view and pass the filter (if one is provided)
            var viewArea = cam.GetViewArea();

            IEnumerable<IDrawable> drawableInView;
            IEnumerable<IDrawable> bgInView;
            if (DrawFilter != null)
            {
                drawableInView = Spatial.GetMany<IDrawable>(viewArea, x => DrawFilter(x));
                bgInView = _backgroundImages.Cast<IDrawable>().Where(x => DrawFilter(x) && x.InView(cam));
            }
            else
            {
                drawableInView = Spatial.GetMany<IDrawable>(viewArea);
                bgInView = _backgroundImages.Cast<IDrawable>().Where(x => x.InView(cam));
            }

            // Concat the background images (to the start of the list) since they aren't in any spatials
            drawableInView = bgInView.Concat(drawableInView);

            if (BeginDrawMap != null)
                BeginDrawMap.Raise(this, new DrawableMapDrawEventArgs(sb, cam));

            // Sort all the items, then start drawing them layer-by-layer, item-by-item
            foreach (var tmpLayer in _drawableSorter.GetSorted(drawableInView))
            {
                var layer = tmpLayer;

                var layerEventArgs = new DrawableMapDrawLayerEventArgs(layer.Key, sb, cam);

                // Notify the layer has started drawing
                if (BeginDrawMapLayer != null)
                    BeginDrawMapLayer.Raise(this, layerEventArgs);

                // Draw the normal map objects
                foreach (var drawable in layer.Value)
                {
                    drawable.Draw(sb);
                }

                // Get the effects to draw, then draw them (if possible)
                foreach (var mapEffect in _mapEffects)
                {
                    if (mapEffect.MapRenderLayer == layer.Key && mapEffect.InView(cam))
                        mapEffect.Draw(sb);
                }

                // Notify the layer has finished drawing
                if (EndDrawMapLayer != null)
                    EndDrawMapLayer.Raise(this, layerEventArgs);
            }

            // Draw the particle effects
            if (DrawParticles)
            {
                foreach (var pe in ParticleEffects)
                {
                    if (pe.InView(cam))
                        pe.Draw(sb);
                }
            }

            if (EndDrawMap != null)
                EndDrawMap.Raise(this, new DrawableMapDrawEventArgs(sb, cam));
        }

        #endregion

        /// <summary>
        /// Gets a rough (not very reliable) comparison between two MapGrhs. Used to sort before
        /// saving the map to help make the order more consistent (resulting in more reasonable save diffs).
        /// </summary>
        protected class BasicMapGrhComparer : IComparer<MapGrh>
        {
            public int Compare(MapGrh a, MapGrh b)
            {
                int tmp = a.Position.X.CompareTo(b.Position.X);
                if (tmp != 0) return tmp;

                tmp = a.Position.Y.CompareTo(b.Position.Y);
                if (tmp != 0) return tmp;

                tmp = a.Size.X.CompareTo(b.Size.X);
                if (tmp != 0) return tmp;

                tmp = a.Size.Y.CompareTo(b.Size.Y);
                if (tmp != 0) return tmp;

                tmp = a.Grh.GrhData.GrhIndex.CompareTo(b.Grh.GrhData.GrhIndex);
                if (tmp != 0) return tmp;

                return StringComparer.Ordinal.Compare(a.GetType().FullName ?? string.Empty, b.GetType().FullName ?? string.Empty);
            }
        }
    }
}