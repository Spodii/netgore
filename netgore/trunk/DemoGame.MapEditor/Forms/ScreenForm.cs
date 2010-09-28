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
using NetGore.Content;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;
using CustomUITypeEditors = DemoGame.EditorTools.CustomUITypeEditors;

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable UnusedParameter.Local

// LATER: Grid-snapping for batch movement
// LATER: When walking down slope, don't count it as falling
// LATER: Add more support for editing Grhs
// LATER: Add a cursor that can work with misc entities

namespace DemoGame.MapEditor
{
    partial class ScreenForm : PersistableForm, IGetTime
    {
        public delegate void MapChangeEventHandler(Map oldMap, Map newMap);

        /// <summary>
        /// Key to move the camera down.
        /// </summary>
        const Keys _cameraDown = Keys.S;

        /// <summary>
        /// Key to move the camera left.
        /// </summary>
        const Keys _cameraLeft = Keys.A;

        /// <summary>
        /// Rate at which the screen scrolls.
        /// </summary>
        const float _cameraMoveRate = 15;

        /// <summary>
        /// Key to move the camera right.
        /// </summary>
        const Keys _cameraRight = Keys.D;

        /// <summary>
        /// Key to move the camera up.
        /// </summary>
        const Keys _cameraUp = Keys.W;

        const float _maxCameraScale = 100.0f;
        const float _minCameraScale = 0.01f;

        /// <summary>
        /// The <see cref="Type"/> of the <see cref="Control"/>s that, when they have focus, the key strokes
        /// will not be sent to the game screen. For instance, TextBoxes can be added to prevent moving the
        /// map all over the place when typing something into them. This also works for any derived type, so
        /// entering the type <see cref="Control"/> will result in the camera never moving.
        /// </summary>
        static readonly Type[] _focusOverrideTypes = new Type[] { typeof(TextBox), typeof(ListBox) };

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

        readonly SettingsManager _settingsManager = new SettingsManager("MapEditor",
                                                                        ContentPaths.Build.Settings.Join("MapEditor" +
                                                                                                         EngineSettings.
                                                                                                             DataFileSuffix));

        /// <summary>
        /// Stopwatch used for calculating the game time.
        /// </summary>
        readonly Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// The switches used when creating this form.
        /// </summary>
        readonly IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> _switches;

        /// <summary>
        /// List of all the active transformation boxes
        /// </summary>
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);

        ICamera2D _camera;

        /// <summary>
        /// All content used by the map editor
        /// </summary>
        IContentManager _content;

        /// <summary>
        /// Used to hold the last set Cursor property value without actually changing the cursor. This way, we only actually change the cursor
        /// once per frame, which helps avoid "flickering" from changing it multiple times per frame.
        /// </summary>
        Cursor _currentCursor = Cursors.Default;

        /// <summary>
        /// Current total time in milliseconds - used as the root of all timing
        /// in external classes through the GetTime method.
        /// </summary>
        TickCount _currentTime = 0;

        EditorCursorManager<ScreenForm> _cursorManager;

        IDbController _dbController;
        DrawingManager _drawingManager;
        WallEntityBase _editGrhSelectedWall = null;
        Direction _editGrhSelectedWallDir;

        /// <summary>
        /// If set to a value other than null, the selected <see cref="ParticleEmitter"/> on this form
        /// will be drawn instead of the map.
        /// </summary>
        ParticleEmitterUITypeEditorForm _emitterSelectionForm = null;

        /// <summary>
        /// The default font.
        /// </summary>
        Font _font;

        KeyEventArgs _keyEventArgs = new KeyEventArgs(Keys.None);
        Map _map;
        IMapBoundControl[] _mapBoundControls;
        MiniMapForm _miniMapForm;

        /// <summary>
        /// Modifier values for the camera moving
        /// </summary>
        Vector2 _moveCamera;

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
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        public ScreenForm()
        {
            _switches = Enumerable.Empty<KeyValuePair<CommandLineSwitch, string[]>>();
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        /// <param name="switches">The command-line switches.</param>
        public ScreenForm(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            _switches = switches;
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

        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Cursor"/> that represents the cursor to display when the mouse pointer is over the control.
        /// </returns>
        public override Cursor Cursor
        {
            get { return _currentCursor; }
            set { _currentCursor = value; }
        }

        /// <summary>
        /// Gets the cursor manager.
        /// </summary>
        public EditorCursorManager<ScreenForm> CursorManager
        {
            get { return _cursorManager; }
        }

        /// <summary>
        /// Gets or sets the position of the cursor in the world.
        /// </summary>
        public Vector2 CursorPos
        {
            get { return GameScreen.CursorPos; }
            set { GameScreen.CursorPos = value; }
        }

        IDbController DbController
        {
            get { return _dbController; }
        }

        public DrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        /// <summary>
        /// Gets the size of the game screen.
        /// </summary>
        public Vector2 GameScreenSize
        {
            get { return new Vector2(GameScreen.ClientSize.Width, GameScreen.ClientSize.Height); }
        }

        /// <summary>
        /// Gets the grid used for the game screen
        /// </summary>
        public ScreenGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets the most recent KeyEventArgs
        /// </summary>
        public KeyEventArgs KeyEventArgs
        {
            get { return _keyEventArgs; }
        }

        /// <summary>
        /// Gets or sets the currently loaded map.
        /// </summary>
        public Map Map
        {
            get { return _map; }
            private set
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
            get { return GameScreen.MouseButton; }
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
        /// Gets the <see cref="SettingsManager"/>.
        /// </summary>
        public SettingsManager SettingsManager
        {
            get { return _settingsManager; }
        }

        /// <summary>
        /// Gets the <see cref="ToolTip"/> to use for the form's tooltips.
        /// </summary>
        public ToolTip ToolTip
        {
            get { return tt; }
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
        /// Handles the OnChangeCurrentCursor event of the CursorManager.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void CursorManager_CurrentCursorChanged(EditorCursorManager<ScreenForm> sender)
        {
            _transBoxes.Clear();
            _selTransBox = null;
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="sb">The sprite batch.</param>
        void DrawGame(ISpriteBatch sb)
        {
            // Draw the GrhTreeView if needed
            if (treeGrhs.NeedsToDraw)
            {
                treeGrhs.Draw(sb);
                return;
            }

            // Draw the selected ParticleEmitter if needed
            if (_emitterSelectionForm != null && _emitterSelectionForm.SelectedItem != null &&
                !_emitterSelectionForm.SelectedItem.IsDisposed)
            {
                DrawParticleEmitterOnly(sb, _emitterSelectionForm.SelectedItem);
                return;
            }

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

            // MapGrh bound walls
            if (chkDrawAutoWalls.Checked)
            {
                foreach (var mg in Map.MapGrhs)
                {
                    if (!_camera.InView(mg.Grh, mg.Position))
                        continue;

                    var boundWalls = _mapGrhWalls[mg.Grh.GrhData];
                    if (boundWalls == null)
                        continue;

                    foreach (var wall in boundWalls)
                    {
                        EntityDrawer.Draw(sb, Camera, wall, mg.Position);
                    }
                }
            }

            // Border
            _mapBorderDrawer.Draw(sb, Map, _camera);

            // Selection area
            CursorManager.DrawSelection(sb);

            // Grid
            if (chkDrawGrid.Checked)
                Grid.Draw(sb, Camera);

            // On-screen wall editor
            foreach (var box in _transBoxes)
            {
                box.Draw(sb);
            }

            // Light sources
            if (chkLightSources.Checked)
            {
                var offset = AddLightCursor.LightSprite.Size / 2f;
                foreach (var light in DrawingManager.LightManager)
                {
                    AddLightCursor.LightSprite.Draw(sb, light.Position - offset);
                }
            }

            // Tool interface
            CursorManager.DrawInterface(sb);

            // Focused selected object (don't draw it for lights, though)
            foreach (var selected in SelectedObjs.SelectedObjects.Where(x => !(x is ILight)))
            {
                if (selected == SelectedObjs.Focused)
                    _focusedSpatialDrawer.DrawFocused(selected as ISpatial, sb);
                else
                    FocusedSpatialDrawer.DrawNotFocused(selected as ISpatial, sb);
            }

            // End map rendering
            DrawingManager.EndDrawWorld();

            // Begin GUI rendering
            sb = DrawingManager.BeginDrawGUI();
            if (sb == null)
                return;

            // Cursor position
            var cursorPosText = CursorPos.ToString();
            var cursorPosTextPos = new Vector2(GameScreen.Size.Width, GameScreen.Size.Height) -
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
            // Start drawing
            sb.Begin(BlendMode.Alpha, Camera);

            try
            {
                if (_emitterSelectionForm.SelectedItem != null)
                    _emitterSelectionForm.SelectedItem.Draw(sb);
            }
            finally
            {
                sb.End();
            }
        }

        /// <summary>
        /// Finds which control has focus and no children controls
        /// </summary>
        /// <param name="control">Base control to check</param>
        /// <returns>Lowest-level control with focus</returns>
        static Control FindFocusControl(Control control)
        {
            Control ret = null;

            foreach (Control c in control.Controls)
            {
                if (c.Focused && c.Controls.Count == 0)
                {
                    ret = c;
                    break;
                }

                var tmpRet = FindFocusControl(c);
                if (tmpRet != null)
                {
                    ret = tmpRet;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// Handles the MouseWheel event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void GameScreen_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            var delta = e.Delta > 0 ? 1 : -1;

            if (!treeGrhs.NeedsToDraw)
            {
                if (e.Delta != 0)
                    CursorManager.MoveMouseWheel(delta);
            }
            else
            {
                var v = (delta * 0.1f);
                if (e.Button == MouseButtons.Middle)
                    v *= 10;

                var newScale = treeGrhs.EditGrhForm.Camera.Scale + v;
                treeGrhs.EditGrhForm.Camera.Scale = Math.Max(_minCameraScale, Math.Min(_maxCameraScale, newScale));
            }
        }

        /// <summary>
        /// Handles the Resize event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void GameScreen_Resize(object sender, EventArgs e)
        {
            _camera.Size = GameScreenSize;
        }

        void HandleSwitch_SaveAllMaps(string[] parameters)
        {
            foreach (var file in MapBase.GetMapFiles(ContentPaths.Dev))
            {
                if (!MapBase.IsValidMapFile(file))
                    continue;

                MapID index;
                if (!MapBase.TryGetIndexFromPath(file, out index))
                    continue;

                using (var tempMap = new Map(index, Camera, _world))
                {
                    tempMap.Load(ContentPaths.Dev, true, MapEditorDynamicEntityFactory.Instance);
                    tempMap.Save(index, ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);
                }
            }
        }

        void HandleSwitches(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            if (switches == null || switches.Count() == 0)
                return;

            var willClose = false;

            foreach (var item in switches)
            {
                switch (item.Key)
                {
                    case CommandLineSwitch.SaveAllMaps:
                        HandleSwitch_SaveAllMaps(item.Value);
                        break;

                    case CommandLineSwitch.Close:
                        willClose = true;
                        break;
                }
            }

            // To close, we actually will create a timer to close the form one ms from now
            if (willClose)
            {
                var t = new Timer { Interval = 1 };
                t.Tick += delegate { Close(); };
                t.Start();
            }
        }

        static void HookFormKeyEvents(Control root, KeyEventHandler kehDown, KeyEventHandler kehUp)
        {
            foreach (Control c in root.Controls)
            {
                if (c.Controls.Count > 0)
                    HookFormKeyEvents(c, kehDown, kehUp);
                c.KeyDown += kehDown;
                c.KeyUp += kehUp;
            }
        }

        /// <summary>
        /// Checks if a key is valid to be forwarded
        /// </summary>
        static bool IsKeyToForward(Keys key)
        {
            switch (key)
            {
                case _cameraUp:
                case _cameraDown:
                case _cameraLeft:
                case _cameraRight:
                case Keys.Delete:
                    return true;

                default:
                    return false;
            }
        }

        void Map_Saved(MapBase map)
        {
            DbController.GetQuery<UpdateMapQuery>().Execute(map);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DesignMode)
            {
                base.OnFormClosing(e);
                return;
            }

            GrhInfo.Save(ContentPaths.Dev);
            SettingsManager.Save();

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (DesignMode)
                return;

            var focusControl = FindFocusControl(this);
            if (focusControl != null)
            {
                var focusControlType = focusControl.GetType();
                if (_focusOverrideTypes.Any(x => x.IsAssignableFrom(focusControlType)))
                    return;
            }

            var startMoveCamera = new Vector2(_moveCamera.X, _moveCamera.Y);

            switch (e.KeyCode)
            {
                case _cameraUp:
                    _moveCamera.Y = -_cameraMoveRate;
                    break;
                case _cameraRight:
                    _moveCamera.X = _cameraMoveRate;
                    break;
                case _cameraDown:
                    _moveCamera.Y = _cameraMoveRate;
                    break;
                case _cameraLeft:
                    _moveCamera.X = -_cameraMoveRate;
                    break;

                case Keys.Delete:
                    CursorManager.PressDelete();
                    _selTransBox = null;
                    _transBoxes.Clear();
                    break;
            }

            if (startMoveCamera != _moveCamera)
                e.Handled = true;
        }

        /// <summary>
        /// Forwards special KeyDown events to the form
        /// </summary>
        void OnKeyDownForward(object sender, KeyEventArgs e)
        {
            _keyEventArgs = e;
            _cursorManager.UseAlternateCursor = e.Shift;

            if (IsKeyToForward(e.KeyCode))
                OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (DesignMode)
            {
                base.OnKeyUp(e);
                return;
            }

            var startMoveCamera = new Vector2(_moveCamera.X, _moveCamera.Y);

            switch (e.KeyCode)
            {
                case _cameraUp:
                case _cameraDown:
                    _moveCamera.Y = 0;
                    break;

                case _cameraLeft:
                case _cameraRight:
                    _moveCamera.X = 0;
                    break;
            }

            if (startMoveCamera != _moveCamera)
                e.Handled = true;

            if (e.KeyCode == Keys.F12)
            {
                var previewer = new MapPreviewer();
                var tmpFile = new TempFile();
                previewer.CreatePreview(GameScreen.RenderWindow, Map, _mapDrawingExtensions, tmpFile.FilePath);
            }

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Forwards special KeyUp events to the form
        /// </summary>
        void OnKeyUpForward(object sender, KeyEventArgs e)
        {
            _keyEventArgs = e;
            _cursorManager.UseAlternateCursor = e.Shift;

            if (IsKeyToForward(e.KeyCode))
                OnKeyUp(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Make sure we skip doing all of this loading when in design mode
            if (DesignMode)
                return;

            Show();
            Refresh();

            numZoom.Minimum = (decimal)_minCameraScale * 100;
            numZoom.Maximum = (decimal)_maxCameraScale * 100;

            scTabsAndSelected.Panel2Collapsed = true;

            _camera = new Camera2D(GameScreenSize);

            // Create the database connection
            var settings = new DbConnectionSettings();
            _dbController =
                settings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                                                                 x => settings.PromptEditFileMessageBox(x));

            // Set up the object manager
            _selectedObjectsManager = new SelectedObjectsManager<object>(pgSelected, lstSelected);
            SelectedObjs.SelectedChanged += SelectedObjectsManager_SelectedChanged;
            SelectedObjs.FocusedChanged += SelectedObjectsManager_FocusedChanged;

            // Get the IMapBoundControls
            _mapBoundControls = this.GetControls().OfType<IMapBoundControl>().ToArray();

            // Create and set up the cursor manager
            _cursorManager = new EditorCursorManager<ScreenForm>(this, ToolTip, panToolBar, GameScreen,
                                                                 x => Map != null && !treeGrhs.IsEditingGrhData);
            CursorManager.SelectedCursor = CursorManager.TryGetCursor<EntityCursor>();
            CursorManager.SelectedAltCursor = CursorManager.TryGetCursor<AddEntityCursor>();
            CursorManager.CurrentCursorChanged += CursorManager_CurrentCursorChanged;

            // Create the world
            _world = new World(this, _camera, null);

            // Set up the GameScreenControl
            GameScreen.Camera = _camera;
            GameScreen.UpdateHandler = UpdateGame;
            GameScreen.DrawHandler = DrawGame;
            GameScreen.MouseWheel += GameScreen_MouseWheel;

            // Create the engine objects 
            _content = ContentManager.Create();

            // Read the Grh information
            GrhInfo.Load(ContentPaths.Dev, _content);
            AutomaticGrhDataSizeUpdater.Instance.UpdateSizes();

            _drawingManager = new DrawingManager(GameScreen.RenderWindow);
            DrawingManager.LightManager.DefaultSprite = new Grh(GrhInfo.GetData("Effect", "light"));

            // Grab the audio manager instances, which will ensure that they are property initialized
            // before something that can't pass it an ContentManager (such as the UITypeEditor) tries to get an instance.
            AudioManager.GetInstance(_content);

            if (_dbController == null)
            {
                Close();
                return;
            }

            // Create the font
            _font = _content.LoadFont("Font/Arial", 16, ContentLevel.Global);
            Character.NameFont = RenderFont;

            // Hook all controls to forward camera movement keys Form
            KeyEventHandler kehDown = OnKeyDownForward;
            KeyEventHandler kehUp = OnKeyUpForward;
            HookFormKeyEvents(this, kehDown, kehUp);

            // Set the custom UITypeEditors
            CustomUITypeEditors.AddEditors(_dbController);

            // Populate the SettingsManager
            PopulateSettingsManager();
            SelectedObjs.Clear();

            // Set up the PropertyGrids
            PropertyGridHelper.AttachShrinkerEventHandler(pgMap);
            foreach (var pg in this.GetControls().OfType<PropertyGrid>())
            {
                PropertyGridHelper.AttachRefresherEventHandler(pg);
                PropertyGridHelper.SetContextMenuIfNone(pg);
            }

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

            // Set up the MapDrawExtensionCollection
            CreateMapDrawExtension<MapEntityBoxDrawer>(chkDrawEntities);
            CreateMapDrawExtension<MapWallDrawer>(chkShowWalls);

            var v = CreateMapDrawExtension<MapSpawnDrawer>(chkDrawSpawnAreas);
            lstNPCSpawns.SelectedIndexChanged += ((o, x) => v.MapSpawns = ((NPCSpawnsListBox)o).GetMapSpawnValues());

            _camera.Size = GameScreenSize;

            // Load the Grhs tree
            treeGrhs.Initialize(_content, _camera.Size, CreateWallEntity, _mapGrhWalls);

            // Handle any command-line switches
            HandleSwitches(_switches);
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

            if (oldMap != newMap)
            {
                // Remove all lights for the old map from the light manager
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

                // Remove the refraction effects from the old map
                if (oldMap != null)
                {
                    foreach (var fx in oldMap.RefractionEffects)
                    {
                        DrawingManager.RefractionManager.Remove(fx);
                    }
                }

                // Add the refraction effects for the new map
                foreach (var fx in newMap.RefractionEffects)
                {
                    DrawingManager.RefractionManager.Add(fx);
                }
            }

            pgMap.SelectedObject = newMap;
        }

        /// <summary>
        /// Adds all the <see cref="IPersistable"/>s to the <see cref="SettingsManager"/>.
        /// </summary>
        void PopulateSettingsManager()
        {
            // Add the Controls that implement IPersistable
            var persistableControls = this.GetPersistableControls();
            var keyValuePairs =
                persistableControls.Select(x => new KeyValuePair<string, IPersistable>("Control_" + x.Name, (IPersistable)x));
            SettingsManager.Add(keyValuePairs);

            // Manually add the other things that implement IPersistable
            SettingsManager.Add("Grid", Grid);
        }

        /// <summary>
        /// Saves the current map.
        /// </summary>
        void SaveMap()
        {
            if (Map == null)
                return;

            Cursor = Cursors.WaitCursor;
            Enabled = false;

            // Add the MapGrh-bound walls
            var extraWalls = _mapGrhWalls.CreateWallList(Map.MapGrhs, CreateWallEntity);
            foreach (var wall in extraWalls)
            {
                Map.AddEntity(wall);
            }

            // Write the map
            Map.Save(ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);

            // Remove the extra walls
            foreach (var wall in extraWalls)
            {
                Map.RemoveEntity(wall);
            }

            Enabled = true;
            Cursor = Cursors.Default;
        }

        void SelectedObjectsManager_FocusedChanged(SelectedObjectsManager<object> sender, object newFocused)
        {
            scTabsAndSelected.Panel2Collapsed = (SelectedObjs.SelectedObjects.Count() == 0 && SelectedObjs.Focused == null);
        }

        void SelectedObjectsManager_SelectedChanged(SelectedObjectsManager<object> sender)
        {
            scSelectedItems.Panel2Collapsed = SelectedObjs.SelectedObjects.Count() < 2;
        }

        /// <summary>
        /// Handles updating when the <see cref="EditGrhForm"/> is visible and has taken over the main screen (so it
        /// can draw the preview of the GrhData).
        /// </summary>
        void UpdateEditGrhView()
        {
            var frm = treeGrhs.EditGrhForm;
            if (frm == null)
                return;

            // Check that there are even any walls
            if (frm.BoundWalls.IsEmpty())
            {
                _editGrhSelectedWall = null;
                _editGrhSelectedWallDir = Direction.North;
                Cursor = Cursors.Default;
                return;
            }

            // If the left mouse button is no longer down, unset the _editGrhSelectedWall
            if (MouseButtons != MouseButtons.Left)
                _editGrhSelectedWall = null;

            // Get the world position for the cursor
            var worldPos = frm.Camera.ToWorld(CursorPos).Round();

            // Select a new wall if null, or if not null, check to move the wall
            if (_editGrhSelectedWall == null)
            {
                // Check if the cursor is on the corner of any of the bound walls, with a 1 pixel resolution
                var dir = Direction.North;
                WallEntityBase selWall = null;
                foreach (var wall in frm.BoundWalls)
                {
                    dir = Direction.North;

                    var wallPos = wall.Position.Round();
                    var wallMax = wall.Max.Round();

                    if (wallPos.X == worldPos.X)
                    {
                        if (wallPos.Y == worldPos.Y)
                            dir = Direction.NorthWest;
                        else if (wallMax.Y == worldPos.Y)
                            dir = Direction.SouthWest;
                    }
                    else if (wallMax.X == worldPos.X)
                    {
                        if (wallPos.Y == worldPos.Y)
                            dir = Direction.NorthEast;
                        else if (wallMax.Y == worldPos.Y)
                            dir = Direction.SouthEast;
                    }

                    if (dir != Direction.North)
                    {
                        selWall = wall;
                        break;
                    }
                }

                // Set the selected wall if there is one
                if (selWall != null)
                {
                    _editGrhSelectedWall = selWall;
                    _editGrhSelectedWallDir = dir;
                }
                else
                {
                    _editGrhSelectedWall = null;
                    _editGrhSelectedWallDir = Direction.North;
                }
            }
            else
            {
                // Move/resize the bound wall
                var wall = _editGrhSelectedWall;
                var newPos = wall.Position;
                var newSize = wall.Size;
                switch (_editGrhSelectedWallDir)
                {
                    case Direction.NorthWest:
                        newPos = worldPos;
                        break;

                    case Direction.NorthEast:
                        newPos = new Vector2(wall.Position.X, worldPos.Y);
                        newSize = new Vector2(worldPos.X - wall.Position.X, wall.Size.Y);
                        break;

                    case Direction.SouthEast:
                        newSize = worldPos - wall.Position;
                        break;

                    case Direction.SouthWest:
                        newPos = new Vector2(worldPos.X, wall.Position.Y);
                        newSize = new Vector2(wall.Size.X, worldPos.Y - wall.Position.Y);
                        break;
                }

                newSize += wall.Position - newPos;
                newSize = newSize.Max(Vector2.One);

                wall.Position = newPos;
                wall.Size = newSize;
            }

            // If we do have a wall corner under the cursor, show it by changing the cursor
            switch (_editGrhSelectedWallDir)
            {
                case Direction.NorthWest:
                case Direction.SouthEast:
                    Cursor = Cursors.SizeNWSE;
                    break;

                case Direction.NorthEast:
                case Direction.SouthWest:
                    Cursor = Cursors.SizeNESW;
                    break;

                default:
                    Cursor = Cursors.Default;
                    break;
            }
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        void UpdateGame()
        {
            // Restore to the default cursor
            Cursor = Cursors.Default;

            // Update the time
            var currTime = (TickCount)_stopWatch.ElapsedMilliseconds;
            var deltaTime = currTime - _currentTime;
            _currentTime = currTime;

            // Update stuff in selection forms (moving it to the center of the camera so it draws centered)
            if (_emitterSelectionForm != null && _emitterSelectionForm.SelectedItem != null &&
                !_emitterSelectionForm.SelectedItem.IsDisposed)
            {
                var originalOrigin = _emitterSelectionForm.SelectedItem.Origin;
                _emitterSelectionForm.SelectedItem.Origin = Camera.Center;
                _emitterSelectionForm.SelectedItem.Update(_currentTime);
                _emitterSelectionForm.SelectedItem.Origin = originalOrigin;
            }

            // Check for a map
            if (Map == null)
                return;

            if (treeGrhs.NeedsToDraw)
            {
                UpdateEditGrhView();
                return;
            }

            // Move the camera
            _camera.Min += _moveCamera;

            // Update other stuff
            Map.Update(deltaTime);
            DrawingManager.Update(currTime);
            _selectedGrh.Update(currTime);

            // Update the cursor manager
            CursorManager.Update();

            // Apply the new cursor
            base.Cursor = _currentCursor;
        }

        void btnAddSpawn_Click(object sender, EventArgs e)
        {
            lstNPCSpawns.AddNewItem();
        }

        void btnDeleteBGItem_Click(object sender, EventArgs e)
        {
            var selectedBgImage = lstBGItems.SelectedItem as BackgroundImage;
            if (selectedBgImage != null)
                Map.RemoveBackgroundImage(selectedBgImage);
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteEmitter_Click(object sender, EventArgs e)
        {
            var selectedEmitters = SelectedObjs.SelectedObjects.OfType<ParticleEffectReference>().ToImmutable();
            if (selectedEmitters.IsEmpty())
                return;

            foreach (var emitter in selectedEmitters)
            {
                Map.ParticleEffects.Remove(emitter);
            }

            SelectedObjs.Clear();
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteSpawn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteSpawn_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you wish to delete this spawn? This cannot be undone.", "Delete?",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            lstNPCSpawns.DeleteSelectedItem();
        }

        /// <summary>
        /// Handles the Click event of the btnNewBGLayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNewBGLayer_Click(object sender, EventArgs e)
        {
            var bgLayer = new BackgroundLayer(Map, Map);
            Map.AddBackgroundImage(bgLayer);
        }

        /// <summary>
        /// Handles the Click event of the btnNewEmitter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNewEmitter_Click(object sender, EventArgs e)
        {
            ParticleEffectReference newEmitter = null;

            // Create the selection form
            using (var f = new ParticleEmitterUITypeEditorForm(null))
            {
                _emitterSelectionForm = f;

                try
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        var em = f.SelectedItem;
                        if (em == null)
                            return;

                        newEmitter = new ParticleEffectReference(em.Name);
                    }
                }
                finally
                {
                    // Clear the selected form
                    _emitterSelectionForm = null;
                }
            }

            if (newEmitter == null)
                return;

            // Set up the new emitter and add it to the map
            newEmitter.Position = Camera.Center;
            newEmitter.Update(GetTime());
            Map.ParticleEffects.Add(newEmitter);
        }

        /// <summary>
        /// Handles the Click event of the btnSaveAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            MapID newID;

            using (var f = new InputNewMapIDForm(Map.ID))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                if (!f.Value.HasValue)
                    return;

                newID = f.Value.Value;
            }

            Map.ChangeID(newID);

            SaveMap();
        }

        /// <summary>
        /// Handles the Click event of the btnShowMinimap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnShowMinimap_Click(object sender, EventArgs e)
        {
            if (_miniMapForm != null && _miniMapForm.Visible && !_miniMapForm.IsDisposed)
                return;

            if (_miniMapForm != null && !_miniMapForm.IsDisposed)
                _miniMapForm.Dispose();

            _miniMapForm = new MiniMapForm { Camera = Camera };
            _miniMapForm.Show(this);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkDrawBackground control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkDrawBackground_CheckedChanged(object sender, EventArgs e)
        {
            MapDrawFilterHelper.DrawBackground = chkDrawBackground.Checked;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkShowGrhs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void chkShowGrhs_CheckedChanged(object sender, EventArgs e)
        {
            MapDrawFilterHelper.DrawMapGrhs = chkShowGrhs.Checked;
        }

        /// <summary>
        /// Handles the Click event of the cmdLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmdLoad_Click(object sender, EventArgs e)
        {
            string filePath;
            IMap loadedMap;
            if (FileDialogs.TryOpenMap(CreateMapFromFilePath, out filePath, out loadedMap))
                Map = (Map)loadedMap;
        }

        /// <summary>
        /// Handles the Click event of the cmdNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmdNew_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you wish to create a new map? All changes to the current map will be lost.",
                                "Create new map?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var index = MapBase.GetNextFreeIndex(ContentPaths.Dev);

            var newMap = new Map(index, Camera, _world);
            DbController.GetQuery<ReplaceMapQuery>().Execute(newMap);
            newMap.SetDimensions(new Vector2(30, 20) * 32);
            newMap.Save(index, ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);
            Map = newMap;
        }

        /// <summary>
        /// Handles the Click event of the cmdSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void cmdSave_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstBGItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstBGItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBGItems.SelectedItem != null)
                SelectedObjs.SetSelected(lstBGItems.SelectedItem);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstMapEffects control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstMapEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMapEffects.SelectedItem != null)
                SelectedObjs.SetSelected(lstMapEffects.SelectedItem);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstNPCSpawns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstNPCSpawns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcMenu.SelectedTab != tpNPCs)
                return;

            var selected = lstNPCSpawns.SelectedItemReal;
            if (selected == null)
                return;

            SelectedObjs.SetSelected(new EditorMapSpawnValues(selected, Map));
        }

        /// <summary>
        /// Handles the Click event of the lstSelected control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstSelected_Click(object sender, EventArgs e)
        {
            var current = lstSelected.SelectedItem as ISpatial;
            if (current == null)
                return;

            if (current == _focusedSpatialDrawer.Focused)
                _focusedSpatialDrawer.ResetIndicator();
        }

        void numZoom_ValueChanged(object sender, EventArgs e)
        {
            var value = Convert.ToSingle(numZoom.Value);

            value *= 0.01f;

            if (value <= _minCameraScale || value > _maxCameraScale)
                return;

            Camera.Scale = value;
        }

        /// <summary>
        /// Handles the Enter event of the tabPageGrhs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void tabPageGrhs_Enter(object sender, EventArgs e)
        {
            treeGrhs.Select();
        }

        /// <summary>
        /// Handles the GrhAfterSelect event of the treeGrhs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetGore.EditorTools.GrhTreeViewEventArgs"/> instance containing the event data.</param>
        void treeGrhs_GrhAfterSelect(object sender, GrhTreeViewEventArgs e)
        {
            if (_selectedGrh.GrhData != null && e.GrhData.GrhIndex == _selectedGrh.GrhData.GrhIndex)
                return;

            _selectedGrh.SetGrh(e.GrhData.GrhIndex, AnimType.Loop, _currentTime);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtGridHeight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtGridHeight_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridHeight.Text, out result))
                Grid.Height = result;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtGridWidth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtGridWidth_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridWidth.Text, out result))
                Grid.Width = result;
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