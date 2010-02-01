using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

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

        /// <summary>
        /// The <see cref="Type"/> of the <see cref="Control"/>s that, when they have focus, the key strokes
        /// will not be sent to the game screen. For instance, TextBoxes can be added to prevent moving the
        /// map all over the place when typing something into them. This also works for any derived type, so
        /// entering the type <see cref="Control"/> will result in the camera never moving.
        /// </summary>
        static readonly Type[] _focusOverrideTypes = new Type[] { typeof(TextBox), typeof(ListBox) };

        readonly ICamera2D _camera;
        readonly EditorCursorManager<ScreenForm> _cursorManager;
        readonly FocusedSpatialDrawer _focusedSpatialDrawer = new FocusedSpatialDrawer();
        readonly ScreenGrid _grid;

        readonly MapBorderDrawer _mapBorderDrawer = new MapBorderDrawer();
        readonly IMapBoundControl[] _mapBoundControls;
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

        readonly SelectedObjectsManager<object> _selectedObjectsManager;

        readonly SettingsManager _settingsManager = new SettingsManager("MapEditor",
                                                                        ContentPaths.Build.Settings.Join("MapEditor.xml"));

        /// <summary>
        /// Stopwatch used for calculating the game time (cant use XNA's GameTime since the
        /// form does not inherit DrawableGameComponent).
        /// </summary>
        readonly Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// The switches used when creating this form.
        /// </summary>
        readonly IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> _switches;

        readonly ToolTip _toolTip = new ToolTip();

        /// <summary>
        /// List of all the active transformation boxes
        /// </summary>
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);

        /// <summary>
        /// Current world - used for reference by the map being edited only.
        /// </summary>
        readonly World _world;

        /// <summary>
        /// All content used by the map editor
        /// </summary>
        ContentManager _content;

        /// <summary>
        /// Current total time in milliseconds - used as the root of all timing
        /// in external classes through the GetTime method.
        /// </summary>
        int _currentTime = 0;

        /// <summary>
        /// World position of the cursor.
        /// </summary>
        Vector2 _cursorPos = Vector2.Zero;

        IDbController _dbController;

        KeyEventArgs _keyEventArgs = new KeyEventArgs(Keys.None);
        Map _map;

        /// <summary>
        /// Modifier values for the camera moving
        /// </summary>
        Vector2 _moveCamera;

        /// <summary>
        /// Currently selected transformation box
        /// </summary>
        TransBox _selTransBox = null;

        /// <summary>
        /// The Default SpriteFont.
        /// </summary>
        SpriteFont _spriteFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        /// <param name="switches">The command-line switches.</param>
        public ScreenForm(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            _switches = switches;

            InitializeComponent();
            scTabsAndSelected.Panel2Collapsed = true;

            _camera = new Camera2D(GameScreenSize);
            _grid = new ScreenGrid(GameScreenSize);

            // Set up the object manager
            _selectedObjectsManager = new SelectedObjectsManager<object>(pgSelected, lstSelected);
            _selectedObjectsManager.SelectedChanged += SelectedObjectsManager_SelectedChanged;
            _selectedObjectsManager.FocusedChanged += SelectedObjectsManager_FocusedChanged;

            // Get the IMapBoundControls
            _mapBoundControls = this.GetControls().OfType<IMapBoundControl>().ToArray();

            // Create and set up the cursor manager
            _cursorManager = new EditorCursorManager<ScreenForm>(this, ToolTip, panToolBar, GameScreen,
                                                                 x => Map != null && !treeGrhs.IsEditingGrhData);
            CursorManager.SelectedCursor = CursorManager.TryGetCursor<EntityCursor>();
            CursorManager.SelectedAltCursor = CursorManager.TryGetCursor<AddEntityCursor>();
            CursorManager.CurrentCursorChanged += CursorManager_CurrentCursorChanged;

            // Create the world
            _world = new World(this, _camera);

            // Set up the GameScreenControl
            GameScreen.Camera = _camera;
            GameScreen.UpdateHandler = UpdateGame;
            GameScreen.DrawHandler = DrawGame;
            GameScreen.MouseWheel += GameScreen_MouseWheel;
            GameScreen.Resize += GameScreen_Resize;
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

                Map oldMap = _map;

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
                txtMapWidth.Text = Map.Width.ToString();
                txtMapHeight.Text = Map.Height.ToString();

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
        /// Gets the SpriteFont used to draw text to the screen.
        /// </summary>
        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        public ToolTip ToolTip
        {
            get { return _toolTip; }
        }

        /// <summary>
        /// Gets the transformation box created.
        /// </summary>
        public List<TransBox> TransBoxes
        {
            get { return _transBoxes; }
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
        /// Handles the Click event of the btnDeletePersistentNPC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeletePersistentNPC_Click(object sender, EventArgs e)
        {
            var selectedChar = lstPersistentNPCs.SelectedItem as MapEditorCharacter;
            if (selectedChar == null)
                return;

            string s = string.Format("Are you sure you wish to delete Character `{0}`? This cannot be undone.", selectedChar);
            if (MessageBox.Show(s) == DialogResult.No)
                return;

            if (DbController.GetQuery<DeletePersistentNPCQuery>().Execute(selectedChar.CharacterID) == 0)
            {
                s = string.Format("Failed to delete Character `{0}` from the database.", selectedChar);
                MessageBox.Show(s);
                return;
            }

            lstPersistentNPCs.RemoveItemAndReselect(selectedChar);
        }

        void btnDeleteSpawn_Click(object sender, EventArgs e)
        {
            lstNPCSpawns.DeleteSelectedItem();
        }

        void btnNewBGLayer_Click(object sender, EventArgs e)
        {
            BackgroundLayer bgLayer = new BackgroundLayer(Map, Map);
            Map.AddBackgroundImage(bgLayer);
        }

        void chkDrawBackground_CheckedChanged(object sender, EventArgs e)
        {
            MapDrawFilterHelper.DrawBackground = chkDrawBackground.Checked;
        }

        void chkShowGrhs_CheckedChanged(object sender, EventArgs e)
        {
            MapDrawFilterHelper.DrawMapGrhs = chkShowGrhs.Checked;
        }

        void cmdApplySize_Click(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            uint width;
            uint height;

            if (Parser.Current.TryParse(txtMapWidth.Text, out width) && Parser.Current.TryParse(txtMapHeight.Text, out height))
                Map.SetDimensions(new Vector2(width, height));

            txtMapWidth_TextChanged(null, null);
            txtMapHeight_TextChanged(null, null);
        }

        void cmdLoad_Click(object sender, EventArgs e)
        {
            string filePath;
            IMap loadedMap;
            if (FileDialogs.TryOpenMap(CreateMapFromFilePath, MapBase.MapFileSuffix, out filePath, out loadedMap))
                Map = (Map)loadedMap;
        }

        void cmdNew_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you wish to create a new map? All changes to the current map will be lost.",
                                "Create new map?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            MapIndex index = MapBase.GetNextFreeIndex(ContentPaths.Dev);

            var newMap = new Map(index, Camera, _world, GameScreen.GraphicsDevice);
            DbController.GetQuery<InsertMapQuery>().Execute(newMap);
            newMap.SetDimensions(new Vector2(30, 20) * 32);
            newMap.Save(index, ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);
            Map = newMap;
        }

        void cmdSave_Click(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            Cursor = Cursors.WaitCursor;
            Enabled = false;

            // Add the MapGrh-bound walls
            var extraWalls = _mapGrhWalls.CreateWallList(Map.MapGrhs, CreateWallEntity);
            foreach (WallEntityBase wall in extraWalls)
            {
                Map.AddEntity(wall);
            }

            // Write the map
            Map.Save(Map.Index, ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);

            // Remove the extra walls
            foreach (WallEntityBase wall in extraWalls)
            {
                Map.RemoveEntity(wall);
            }

            Enabled = true;
            Cursor = Cursors.Default;
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
            T instance = new T { Enabled = checkBox.Checked };

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

            MapIndex index;
            if (!MapBase.TryGetIndexFromPath(filePath, out index))
            {
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, filePath));
                return null;
            }

            return new Map(index, Camera, _world, GameScreen.GraphicsDevice);
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
        void DrawGame(SpriteBatch sb)
        {
            // Clear the background
            GameScreen.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the GrhTreeView if needed
            if (treeGrhs.NeedsToDraw)
            {
                treeGrhs.Draw(sb);
                return;
            }

            // Check for a valid map
            if (Map == null)
                return;

            // Begin the rendering
            sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _camera.Matrix);

            // Map
            Map.Draw(sb);

            // MapGrh bound walls
            if (chkDrawAutoWalls.Checked)
            {
                foreach (MapGrh mg in Map.MapGrhs)
                {
                    if (!_camera.InView(mg.Grh, mg.Position))
                        continue;

                    var boundWalls = _mapGrhWalls[mg.Grh.GrhData];
                    if (boundWalls == null)
                        continue;

                    foreach (WallEntityBase wall in boundWalls)
                    {
                        EntityDrawer.Draw(sb, wall, mg.Position);
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
            foreach (TransBox box in _transBoxes)
            {
                box.Draw(sb);
            }

            // Tool interface
            CursorManager.DrawInterface(sb);

            // Focused selected object
            _focusedSpatialDrawer.Draw(SelectedObjs.Focused as ISpatial, sb);

            // End map rendering
            sb.End();

            // Begin GUI rendering
            sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Cursor position
            Vector2 cursorPosText = new Vector2(GameScreen.Size.Width, GameScreen.Size.Height);
            cursorPosText -= new Vector2(100, 30);
            sb.DrawStringShaded(SpriteFont, _cursorPos.ToString(), cursorPosText, Color.White, Color.Black);

            // End GUI rendering
            sb.End();
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

                Control tmpRet = FindFocusControl(c);
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
            CursorManager.MoveMouseWheel(e.Delta > 0 ? 1 : -1);
        }

        /// <summary>
        /// Handles the Resize event of the GameScreen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void GameScreen_Resize(object sender, EventArgs e)
        {
            _camera.Size = GameScreenSize;
            _grid.Size = GameScreenSize;
        }

        void HandleSwitch_SaveAllMaps(string[] parameters)
        {
            foreach (string file in MapBase.GetMapFiles(ContentPaths.Dev))
            {
                if (!MapBase.IsValidMapFile(file))
                    continue;

                MapIndex index;
                if (!MapBase.TryGetIndexFromPath(file, out index))
                    continue;

                using (Map tempMap = new Map(index, Camera, _world, GameScreen.GraphicsDevice))
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

            bool willClose = false;

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
                Timer t = new Timer { Interval = 1 };
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

        void LoadEditor()
        {
            // Read the database connection
            DbConnectionSettings settings = new DbConnectionSettings();
            _dbController = new ServerDbController(settings.GetMySqlConnectionString());

            // Create the engine objects 
            _content = new ContentManager(GameScreen.Services, ContentPaths.Build.Root);

            // Font
            _spriteFont = _content.Load<SpriteFont>(ContentPaths.Build.Fonts.Join("Game"));
            Character.NameFont = SpriteFont;

            // Read the Grh information
            GrhInfo.Load(ContentPaths.Dev, _content);
            treeGrhs.Initialize(_content, _camera.Size, CreateWallEntity, _mapGrhWalls);
            TransBox.Initialize(GrhInfo.GetData("System", "Move"), GrhInfo.GetData("System", "Resize"));

            // Start the stopwatch for the elapsed time checking
            _stopWatch.Start();

            // Hook all controls to forward camera movement keys Form
            KeyEventHandler kehDown = OnKeyDownForward;
            KeyEventHandler kehUp = OnKeyUpForward;
            HookFormKeyEvents(this, kehDown, kehUp);

            // Read the first map
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                Map = new Map(new MapIndex(1), Camera, _world, GameScreen.GraphicsDevice);
            }
            catch (Exception)
            {
                // Doesn't matter if we fail to load the first map...
            }
            // ReSharper restore EmptyGeneralCatchClause

            // Set up the MapDrawExtensionCollection
            CreateMapDrawExtension<MapEntityBoxDrawer>(chkDrawEntities);
            CreateMapDrawExtension<MapWallDrawer>(chkShowWalls);

            MapSpawnDrawer v = CreateMapDrawExtension<MapSpawnDrawer>(chkDrawSpawnAreas);
            lstNPCSpawns.SelectedIndexChanged += ((o, e) => v.MapSpawns = ((NPCSpawnsListBox)o).GetMapSpawnValues());

            _mapDrawingExtensions.Add(new MapPersistentNPCDrawer(lstPersistentNPCs));

            // Populate the SettingsManager
            Show();
            Refresh();
            PopulateSettingsManager();

            SelectedObjs.Clear();
        }

        void lstAvailableParticleEffects_RequestCreateEffect(ParticleEffectListBox sender, string effectName)
        {
            ParticleEmitter effect;

            try
            {
                effect = ParticleEmitterFactory.LoadEmitter(ContentPaths.Dev, effectName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load particle emitter: " + ex);
                return;
            }

            effect.Origin = Camera.Center;
            effect.SetEmitterLife(0, 0);
            Map.ParticleEffects.Add(effect);
        }

        void lstBGItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstBGItems.SelectedItem != null)
                _selectedObjectsManager.SetSelected(lstBGItems.SelectedItem);
        }

        void lstMapParticleEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMapParticleEffects.SelectedItem != null)
                _selectedObjectsManager.SetSelected(lstMapParticleEffects.SelectedItem);
        }

        void lstSelected_Click(object sender, EventArgs e)
        {
            var current = lstSelected.SelectedItem as ISpatial;
            if (current == null)
                return;

            if (current == _focusedSpatialDrawer.Focused)
                _focusedSpatialDrawer.ResetIndicator();
        }

        void Map_Saved(MapBase map)
        {
            DbController.GetQuery<UpdateMapQuery>().Execute(map);
        }

        void numZoom_ValueChanged(object sender, EventArgs e)
        {
            float value = Convert.ToSingle(numZoom.Value);

            value *= 0.01f;

            if (value <= float.Epsilon || value > 100000)
                return;

            Camera.Scale = value;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            Control focusControl = FindFocusControl(this);
            if (focusControl != null)
            {
                var focusControlType = focusControl.GetType();
                if (_focusOverrideTypes.Any(x => x.IsAssignableFrom(focusControlType)))
                    return;
            }

            Vector2 startMoveCamera = new Vector2(_moveCamera.X, _moveCamera.Y);

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

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Vector2 startMoveCamera = new Vector2(_moveCamera.X, _moveCamera.Y);

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

            // Handle the change on some controls manually
            txtMapName.Text = newMap.Name ?? string.Empty;
            txtMusic.Text = newMap.Music ?? string.Empty;
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

        void ScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GrhInfo.Save(ContentPaths.Dev);
            SettingsManager.Save();
        }

        void ScreenForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEditor();
                HandleSwitches(_switches);
            }
            catch (Exception ex)
            {
                // Stupid hack we have to do to get the exceptions to even show at all from this event
                string errmsg = "Exception: " + ex;
                Debug.Fail(errmsg);
                MessageBox.Show(errmsg);
                Dispose();
                throw;
            }

            _camera.Size = GameScreenSize;
            _grid.Size = GameScreenSize;
        }

        void SelectedObjectsManager_FocusedChanged(SelectedObjectsManager<object> sender, object newFocused)
        {
            scTabsAndSelected.Panel2Collapsed = (_selectedObjectsManager.SelectedObjects.Count() == 0 &&
                                                 _selectedObjectsManager.Focused == null);
        }

        void SelectedObjectsManager_SelectedChanged(SelectedObjectsManager<object> sender)
        {
            scSelectedItems.Panel2Collapsed = _selectedObjectsManager.SelectedObjects.Count() < 2;
        }

        void tabPageGrhs_Enter(object sender, EventArgs e)
        {
            treeGrhs.Select();
        }

        void treeGrhs_GrhAfterSelect(object sender, GrhTreeViewEventArgs e)
        {
            if (_selectedGrh.GrhData != null && e.GrhData.GrhIndex == _selectedGrh.GrhData.GrhIndex)
                return;

            _selectedGrh.SetGrh(e.GrhData.GrhIndex, AnimType.Loop, _currentTime);
        }

        void txtGridHeight_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridHeight.Text, out result))
                Grid.Height = result;
        }

        void txtGridWidth_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridWidth.Text, out result))
                Grid.Width = result;
        }

        void txtMapHeight_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (Parser.Current.TryParse(txtMapHeight.Text, out o))
            {
                if (o == Map.Height)
                    txtMapHeight.BackColor = EditorColors.Normal;
                else
                    txtMapHeight.BackColor = EditorColors.Changed;
            }
            else
                txtMapHeight.BackColor = EditorColors.Error;
        }

        void txtMapName_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            Map.Name = txtMapName.Text;
        }

        void txtMapWidth_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (Parser.Current.TryParse(txtMapWidth.Text, out o))
            {
                if (o == Map.Width)
                    txtMapWidth.BackColor = EditorColors.Normal;
                else
                    txtMapWidth.BackColor = EditorColors.Changed;
            }
            else
                txtMapWidth.BackColor = EditorColors.Error;
        }

        void txtMusic_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            Map.Music = txtMusic.Text;
        }

        /// <summary>
        /// Updates the cursor based on the transformation box the cursor is over
        /// or the currently selected transformation box
        /// </summary>
        void UpdateCursor()
        {
            // Don't do anything if we have an unknown cursor
            if (Cursor != Cursors.Default && Cursor != Cursors.SizeAll && Cursor != Cursors.SizeNESW && Cursor != Cursors.SizeNS &&
                Cursor != Cursors.SizeNWSE && Cursor != Cursors.SizeWE)
                return;

            CursorManager.Update();

            // Set to default if it wasn't yet set
            Cursor = Cursors.Default;
        }

        void UpdateGame()
        {
            // Update the time
            int currTime = (int)_stopWatch.ElapsedMilliseconds;
            int deltaTime = currTime - _currentTime;
            _currentTime = currTime;

            // Check for a map
            if (Map == null)
                return;

            // Move the camera
            _camera.Min += _moveCamera;

            // Update the map
            Map.Update(deltaTime);

            // Update the cursor
            UpdateCursor();

            // Update the selected grh
            _selectedGrh.Update(_currentTime);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public int GetTime()
        {
            return _currentTime;
        }

        #endregion
    }
}