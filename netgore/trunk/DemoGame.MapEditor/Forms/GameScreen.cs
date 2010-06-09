using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.EditorTools;
using DemoGame.MapEditor.Forms;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Audio;
using NetGore.Collections;
using NetGore.Content;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using SFML.Graphics;
using CustomUITypeEditors = DemoGame.EditorTools.CustomUITypeEditors;

namespace DemoGame.MapEditor
{
    public partial class GameScreen : Form, IGetTime
    {
        public delegate void MapChangeEventHandler(Map oldMap, Map newMap);

        const float _maxCameraScale = 100.0f;
        const float _minCameraScale = 0.01f;

        readonly FocusedSpatialDrawer _focusedSpatialDrawer = new FocusedSpatialDrawer();
        readonly ScreenGrid _grid = new ScreenGrid();
        readonly MapBorderDrawer _mapBorderDrawer = new MapBorderDrawer();
        readonly MapDrawFilterHelper _mapDrawFilterHelper = new MapDrawFilterHelper();
        readonly MapDrawingExtensionCollection _mapDrawingExtensions = new MapDrawingExtensionCollection();

        /// <summary>
        /// Information on the walls bound to MapGrhs.
        /// </summary>
        readonly MapGrhWalls _mapGrhWalls = new MapGrhWalls(ContentPaths.Dev, CreateWallEntity);

        /// <summary>
        /// Currently selected Grh to draw to the map.
        /// </summary>
        readonly Grh _selectedGrh = new Grh(null, AnimType.Loop, 0);

        /// <summary>
        /// Stopwatch used for calculating the game time.
        /// </summary>
        readonly Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// List of all the active transformation boxes
        /// </summary>
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);

        ICamera2D _camera;

        /// <summary>
        /// Current total time in milliseconds - used as the root of all timing
        /// in external classes through the GetTime method.
        /// </summary>
        TickCount _currentTime = 0;

        DrawingManager _drawingManager;
        WallEntityBase _editGrhSelectedWall = null;
        Direction _editGrhSelectedWallDir;

        /// <summary>
        /// The default font.
        /// </summary>
        Font _font;

        Map _map;
        IMapBoundControl[] _mapBoundControls;

        /// <summary>
        /// Modifier values for the camera moving
        /// </summary>
        Vector2 _moveCamera;

        /// <summary>
        /// The <see cref="IParticleRenderer"/> used when drawing the <see cref="ParticleEmitter"/> only.
        /// </summary>
        SpriteBatchParticleRenderer _particleEmitterRenderer;

        /// <summary>
        /// Currently selected transformation box
        /// </summary>
        TransBox _selTransBox = null;

        SelectedObjectsManager<object> _selectedObjectsManager;

        /// <summary>
        /// Current world - used for reference by the map being edited only.
        /// </summary>
        World _world;

        /// <summary>
        /// All content used by the map editor
        /// </summary>
        IContentManager _content;

        public GameScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Notifies listeners when the map has changed.
        /// </summary>
        public event MapChangeEventHandler MapChanged;

        /// <summary>
        /// Gets the camera used for the game screen.
        /// </summary>
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        public Vector2 MoveCamera
        {
            get { return _moveCamera; }
            set { _moveCamera = value; }
        }

        public RenderWindow RenderWindow
        {
            get { return gameScreenCtrl.RenderWindow; }
        }

        /// <summary>
        /// Gets or sets the position of the cursor in the world.
        /// </summary>
        public Vector2 CursorPos
        {
            get { return gameScreenCtrl.CursorPos; }
            set { gameScreenCtrl.CursorPos = value; }
        }

        public DrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Gets the size of the game screen.
        /// </summary>
        public Vector2 GameScreenSize
        {
            get { return new Vector2(gameScreenCtrl.ClientSize.Width, gameScreenCtrl.ClientSize.Height); }
        }

        /// <summary>
        /// Gets the grid used for the game screen
        /// </summary>
        public ScreenGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets or sets the currently loaded map.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            set
            {
                if (Map == value)
                    return;
                if (value == null)
                    throw new ArgumentNullException("value");

                var oldMap = _map;

                // Remove old map
                if (oldMap != null)
                    oldMap.Saved -= Map_Saved;

                // Set new map
                _map = value;
                Map.Load(ContentPaths.Dev, true, MapEditorDynamicEntityFactory.Instance);
                Map.Saved += Map_Saved;

                // Remove all of the walls previously created from the MapGrhs
                var grhWalls = _mapGrhWalls.CreateWallList(Map.MapGrhs, CreateWallEntity);
                var dupeWalls = Map.FindDuplicateWalls(grhWalls);
                foreach (var dupeWall in dupeWalls)
                {
                    Map.RemoveEntity(dupeWall);
                }

                // Reset some of the variables
                _camera.Min = Vector2.Zero;

                // Notify listeners
                OnMapChanged(oldMap, Map);

                if (MapChanged != null)
                    MapChanged(oldMap, Map);
            }
        }

        public MapDrawFilterHelper MapDrawFilterHelper
        {
            get { return _mapDrawFilterHelper; }
        }

        /// <summary>
        /// Gets the <see cref="MapDrawingExtensionCollection"/>.
        /// </summary>
        public MapDrawingExtensionCollection MapDrawingExtensions
        {
            get { return _mapDrawingExtensions; }
        }

        /// <summary>
        /// Gets the most recently pressed mouse button
        /// </summary>
        public MouseButtons MouseButton
        {
            get { return gameScreenCtrl.MouseButton; }
        }

        /// <summary>
        /// Gets the <see cref="Font"/> used to draw text to the screen.
        /// </summary>
        public Font RenderFont
        {
            get { return _font; }
        }

        /// <summary>
        /// Gets the currently selected Grh
        /// </summary>
        public Grh SelectedGrh
        {
            get { return _selectedGrh; }
        }

        /// <summary>
        /// Gets the manager for selected objects.
        /// </summary>
        public SelectedObjectsManager<object> SelectedObjs
        {
            get { return _selectedObjectsManager; }
        }

        /// <summary>
        /// Gets or sets the selected transformation box.
        /// </summary>
        public TransBox SelectedTransBox
        {
            get { return _selTransBox; }
            set { _selTransBox = value; }
        }

        /// <summary>
        /// Gets the transformation box created.
        /// </summary>
        public List<TransBox> TransBoxes
        {
            get { return _transBoxes; }
        }

        /// <summary>
        /// Creates an instance of a <see cref="MapDrawingExtension"/> that is toggled on and off from a <see cref="CheckBox"/>,
        /// and adds it to the <see cref="MapDrawingExtensionCollection"/>.
        /// </summary>
        /// <typeparam name="T">Type of MapDrawExtensionBase to create.</typeparam>
        /// <param name="checkBox">The CheckBox that is used to set the Enabled property.</param>
        /// <returns>The instanced MapDrawExtensionBase of type <typeparamref name="T"/>.</returns>
        T CreateMapDrawExtension<T>(CheckBox checkBox) where T : IMapDrawingExtension, new()
        {
            // Create the instance of the MapDrawExtensionBase
            var instance = new T { Enabled = checkBox.Checked };

            // Handle when the CheckBox value changes
            checkBox.CheckedChanged += ((obj, e) => instance.Enabled = ((CheckBox)obj).Checked);

            // Add to the collection
            _mapDrawingExtensions.Add(instance);

            return instance;
        }


        Map CreateMapFromFilePath(string filePath)
        {
            const string errmsg = "Invalid map file selected:{0}{1}";

            if (!MapBase.IsValidMapFile(filePath))
            {
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, filePath));
                return null;
            }

            MapID index;
            if (!MapBase.TryGetIndexFromPath(filePath, out index))
            {
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, filePath));
                return null;
            }

            return new Map(index, Camera, _world);
        }

        /// <summary>
        /// Creates a <see cref="WallEntity"/>. This method is purely for convenience in using Lambdas.
        /// </summary>
        /// <param name="position">The position to give the <see cref="WallEntityBase"/>.</param>
        /// <param name="size">The size to give the <see cref="WallEntityBase"/>.</param>
        /// <returns>A <see cref="WallEntityBase"/> created with the specified parameters.</returns>
        static WallEntityBase CreateWallEntity(Vector2 position, Vector2 size)
        {
            return new WallEntity(position, size);
        }

        /// <summary>
        /// Creates a <see cref="WallEntity"/>. This method is purely for convenience in using Lambdas.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the creation values from.</param>
        /// <returns>A <see cref="WallEntityBase"/> created with the specified parameters.</returns>
        static WallEntityBase CreateWallEntity(IValueReader reader)
        {
            return new WallEntity(reader);
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The sprite batch.</param>
        void DrawMap(ISpriteBatch sb)
        {

            // Check for a valid map
            if (Map == null)
                return;

            // Begin the rendering
            DrawingManager.LightManager.Ambient = Map.AmbientLight;
            sb = DrawingManager.BeginDrawWorld(_camera);
            if (sb == null)
                return;

            // Map
            Map.Draw(sb);

            
            // End drawing with lighting, start drawing world without lighting
            DrawingManager.EndDrawWorld();
            sb = DrawingManager.BeginDrawWorld(Camera, false, true);
            if (sb == null)
                return;

            // Border
            _mapBorderDrawer.Draw(sb, Map, _camera);

            
            // End map rendering
            DrawingManager.EndDrawWorld();

            // Begin GUI rendering
            sb = DrawingManager.BeginDrawGUI();
            if (sb == null)
                return;

            // Cursor position
            var cursorPosText = CursorPos.ToString();
            var cursorPosTextPos = new Vector2(gameScreenCtrl.Size.Width, gameScreenCtrl.Size.Height) -
                                   RenderFont.MeasureString(cursorPosText) - new Vector2(4);
            sb.DrawStringShaded(RenderFont, cursorPosText, cursorPosTextPos, Color.White, Color.Black);

            // End GUI rendering
            DrawingManager.EndDrawGUI();
        }

        /// <summary>
        /// Draws a <see cref="ParticleEmitter"/> as the only item on the screen.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to draw.</param>
        void DrawParticleEmitterOnly(ISpriteBatch sb, ParticleEmitter emitter)
        {
            // Set up the particle renderer
            if (_particleEmitterRenderer == null)
                _particleEmitterRenderer = new SpriteBatchParticleRenderer();

            _particleEmitterRenderer.SpriteBatch = sb;

            // Start drawing
            sb.Begin(BlendMode.Alpha, Camera);

            try
            {
                _particleEmitterRenderer.Draw(Camera, new ParticleEmitter[] { _emitterSelectionForm.SelectedItem });
            }
            finally
            {
                sb.End();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);

            Show();
            Refresh();

            // Make sure we skip doing all of this loading when in design mode
            if (DesignMode)
                return;

            _camera = new Camera2D(GameScreenSize);

            // Get the IMapBoundControls
            _mapBoundControls = this.GetControls().OfType<IMapBoundControl>().ToArray();

            // Create the world
            _world = new World(this, _camera, null);

            // Set up the GameScreenControl
            gameScreenCtrl.Camera = _camera;
            gameScreenCtrl.UpdateHandler = UpdateMap;
            gameScreenCtrl.DrawHandler = DrawMap;

            // Create the engine objects 
            _content = ContentManager.Create();

            // Read the Grh information
            GrhInfo.Load(ContentPaths.Dev, _content);
            AutomaticGrhDataSizeUpdater.Instance.UpdateSizes();

            _drawingManager = new DrawingManager(gameScreenCtrl.RenderWindow);
            DrawingManager.LightManager.DefaultSprite = new Grh(GrhInfo.GetData("Effect", "light"));

            // Grab the audio manager instances, which will ensure that they are property initialized
            // before something that can't pass it an ContentManager (such as the UITypeEditor) tries to get an instance.
            AudioManager.GetInstance(_content);

            // Create the font
            _font = _content.LoadFont("Font/Arial", 16, ContentLevel.Global);
            Character.NameFont = RenderFont;

            SelectedObjs.Clear();


            // Read the first map
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                Map = new Map(new MapID(1), Camera, _world);
            }
            catch (Exception)
            {
                // Doesn't matter if we fail to load the first map...
            }
            // ReSharper restore EmptyGeneralCatchClause

            // Start the stopwatch for the elapsed time checking
            _stopWatch.Start();

            _camera.Size = GameScreenSize;
        }

        /// <summary>
        /// Allows for handling when the map has changed. Use this instead of the <see cref="ScreenForm.MapChanged"/>
        /// event when possible.
        /// </summary>
        protected virtual void OnMapChanged(Map oldMap, Map newMap)
        {
            // Clear the selected item
            SelectedObjs.Clear();

            // Forward the change to some controls manually
            Camera.Map = newMap;
            MapDrawingExtensions.Map = newMap;

            // Automatically update all the controls that implement IMapBoundControl
            foreach (var c in _mapBoundControls)
            {
                c.IMap = newMap;
            }

            // Remove all lights for the old map from the light manager
            if (oldMap != newMap)
            {
                if (oldMap != null)
                {
                    foreach (var light in oldMap.Lights)
                    {
                        DrawingManager.LightManager.Remove(light);
                    }
                }

                // Add the lights from the new map
                foreach (var light in newMap.Lights)
                {
                    DrawingManager.LightManager.Add(light);
                }
            }

            
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        void UpdateMap()
        {

            // Update the time
            var currTime = (TickCount)_stopWatch.ElapsedMilliseconds;
            var deltaTime = currTime - _currentTime;
            _currentTime = currTime;

            // Check for a map
            if (Map == null)
                return;

            // Move the camera
            _camera.Min += _moveCamera;

            // Update other stuff
            Map.Update(deltaTime);
            DrawingManager.Update(currTime);
            _selectedGrh.Update(currTime);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public TickCount GetTime()
        {
            return _currentTime;
        }

        #endregion


    }
}
