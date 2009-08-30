using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using DemoGame.Client;
using DemoGame.Server;
using DemoGame.Server.Queries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.EditorTools;
using NetGore.Globalization;
using NetGore.Graphics;
using Point=System.Drawing.Point;

// LATER: Grid-snapping for batch movement
// LATER: When walking down slope, don't count it as falling
// LATER: Add more support for editing Grhs
// LATER: Add a cursor that can work with misc entities

namespace DemoGame.MapEditor
{
    delegate void MapChangeEventHandler(Map oldMap, Map newMap);

    partial class ScreenForm : Form, IGetTime
    {
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
        /// Color of the Grh preview when placing new Grhs.
        /// </summary>
        static readonly Color _drawPreviewColor = new Color(255, 255, 255, 150);

        readonly AddGrhCursor _addGrhCursor = new AddGrhCursor();
        readonly AddWallCursor _addWallCursor = new AddWallCursor();

        /// <summary>
        /// Screen camera
        /// </summary>
        readonly Camera2D _camera = new Camera2D(GameData.ScreenSize);

        /// <summary>
        /// Camera used on the Grh edit display.
        /// </summary>
        readonly Camera2D _editGrhCamera = new Camera2D(GameData.ScreenSize);

        readonly EntityCursor _entityCursor = new EntityCursor();
        readonly GrhCursor _grhCursor = new GrhCursor();

        /// <summary>
        /// Draws the grid.
        /// </summary>
        readonly ScreenGrid _grid = new ScreenGrid(GameData.ScreenSize);

        readonly MapBorderDrawer _mapBorderDrawer = new MapBorderDrawer();

        /// <summary>
        /// Information on the walls bound to MapGrhs.
        /// </summary>
        readonly MapGrhWalls _mapGrhWalls;

        /// <summary>
        /// Currently selected Grh to draw to the map.
        /// </summary>
        readonly Grh _selectedGrh = new Grh(null, AnimType.Loop, 0);

        /// <summary>
        /// Stopwatch used for calculating the game time (cant use XNA's GameTime since the
        /// form does not inherit DrawableGameComponent).
        /// </summary>
        readonly Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// List of all the active transformation boxes
        /// </summary>
        readonly List<TransBox> _transBoxes = new List<TransBox>(9);

        readonly WallCursor _wallCursor = new WallCursor();

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

        DBController _dbController;

        /// <summary>
        /// Grh display of the _editGrhData.
        /// </summary>
        Grh _editGrh = null;

        /// <summary>
        /// GrhData currently being edited with the EditGrhForm.
        /// </summary>
        GrhData _editGrhData = null;

        /// <summary>
        /// ListBox control object collection containing the walls for the Grh being edited.
        /// </summary>
        ListBox.ObjectCollection _editGrhWallItems = null;

        /// <summary>
        /// TreeNode Grh currently being edited with the EditGrhForm.
        /// </summary>
        TreeNode _editNode = null;

        KeyEventArgs _keyEventArgs = new KeyEventArgs(Keys.None);
        Map _map;

        /// <summary>
        /// Currently pressed mouse button
        /// </summary>
        MouseButtons _mouseButton = MouseButtons.None;

        /// <summary>
        /// Modifier values for the camera moving
        /// </summary>
        Vector2 _moveCamera;

        /// <summary>
        /// Global SpriteBatch used by the editor
        /// </summary>
        SpriteBatch _sb;

        /// <summary>
        /// Currently selected cursor from the cursor toolbar
        /// </summary>
        EditorCursorBase _selectedTool = null;

        /// <summary>
        /// Currently selected transformation box
        /// </summary>
        TransBox _selTransBox = null;

        /// <summary>
        /// The Default SpriteFont.
        /// </summary>
        SpriteFont _spriteFont;

        public event MapChangeEventHandler OnChangeMap;

        public AddGrhCursor AddGrhCursor
        {
            get { return _addGrhCursor; }
        }

        public AddWallCursor AddWallCursor
        {
            get { return _addWallCursor; }
        }

        /// <summary>
        /// Gets the camera used for the game screen
        /// </summary>
        public Camera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets the cursor position, taking the camera position into consideration
        /// </summary>
        public Vector2 CursorPos
        {
            get { return _cursorPos; }
            set { _cursorPos = value; }
        }

        DBController DBController
        {
            get { return _dbController; }
        }

        public EntityCursor EntityCursor
        {
            get { return _entityCursor; }
        }

        public GameScreenControl GameScreenControl
        {
            get { return GameScreen; }
        }

        public GrhCursor GrhCursor
        {
            get { return _grhCursor; }
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
                if (_map == value)
                    return;
                if (value == null)
                    throw new ArgumentNullException("value");

                Map oldMap = _map;

                // Remove old map
                if (oldMap != null)
                    oldMap.OnSave -= Map_OnSave;

                // Set new map
                _map = value;
                _map.Load(ContentPaths.Dev, true);
                _map.OnSave += Map_OnSave;

                // Remove all of the walls previously created from the MapGrhs
                var grhWalls = _mapGrhWalls.CreateWallList(Map.MapGrhs);
                var dupeWalls = Map.FindDuplicateWalls(grhWalls);
                foreach (WallEntityBase dupeWall in dupeWalls)
                {
                    Map.RemoveEntity(dupeWall);
                }

                // Reset some of the variables
                _camera.Min = Vector2.Zero;
                txtMapWidth.Text = Map.Width.ToString();
                txtMapHeight.Text = Map.Height.ToString();

                // Notify listeners
                if (OnChangeMap != null)
                    OnChangeMap(oldMap, _map);
            }
        }

        /// <summary>
        /// Gets the most recently pressed mouse button
        /// </summary>
        public MouseButtons MouseButton
        {
            get { return _mouseButton; }
        }

        /// <summary>
        /// Gets the currently selected Grh
        /// </summary>
        public Grh SelectedGrh
        {
            get { return _selectedGrh; }
        }

        /// <summary>
        /// Gets or sets the selected transformation box
        /// </summary>
        public TransBox SelectedTransBox
        {
            get { return _selTransBox; }
            set { _selTransBox = value; }
        }

        /// <summary>
        /// Gets the SpriteBatch used for rendering
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _sb; }
        }

        /// <summary>
        /// Gets the SpriteFont used to draw text to the screen
        /// </summary>
        public SpriteFont SpriteFont
        {
            get { return _spriteFont; }
        }

        /// <summary>
        /// Gets the transformation box created
        /// </summary>
        public List<TransBox> TransBoxes
        {
            get { return _transBoxes; }
        }

        public WallCursor WallCursor
        {
            get { return _wallCursor; }
        }

        public ScreenForm(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            InitializeComponent();
            GameScreen.Screen = this;

            // Set up some of the OnChangeMap events for objects that need to reference the Map
            OnChangeMap += ((oldMap, newMap) => _camera.Map = newMap);
            OnChangeMap += ((oldMap, newMap) => lstNPCSpawns.SetMap(DBController, newMap));
            OnChangeMap += ((oldMap, newMap) => lstPersistentNPCs.SetMap(DBController, newMap));

            // Set up the EntityTypes
            cmbEntityTypes.Items.Clear();
            cmbEntityTypes.Items.AddRange(MapFileEntityAttribute.GetTypes().ToArray());
            cmbEntityTypes.SelectedIndex = 0;

            // Load the settings
            LoadSettings();
            _mapGrhWalls = new MapGrhWalls(ContentPaths.Dev.Data.Join("grhdatawalls.xml"));

            // Create the world
            _world = new World(this, _camera);

            HandleSwitches(switches);
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
                        break;

                    case CommandLineSwitch.Close:
                        willClose = true;
                        break;
                }
            }

            // To close, we actually will create a timer to close the form one ms from now
            if (willClose)
            {
                Timer t = new Timer
                {
                    Interval = 1
                };
                t.Tick += delegate { Close(); };
                t.Start();
            }
        }

        void BeginEditGrhData(TreeNode node, GrhData gd)
        {
            if (_editNode != null || gd == null)
                return;

            _editNode = node;
            _editGrhData = gd;

            Vector2 pos;
            try
            {
                pos = _editGrhData.Size / 2f;
            }
            catch (ContentLoadException)
            {
                pos = Vector2.Zero;
            }
            _editGrhCamera.Zoom(pos, GameData.ScreenSize, 4f);
            _editGrh = new Grh(_editGrhData.GrhIndex, AnimType.Loop, GetTime());

            EditGrhForm f = new EditGrhForm(gd, _mapGrhWalls)
            {
                Location = new Point(0, 0)
            };
            f.FormClosed += EditGrhForm_Close;
            AddOwnedForm(f);
            f.Show();
            _editGrhWallItems = f.lstWalls.Items;
        }

        void btnAddSpawn_Click(object sender, EventArgs e)
        {
            lstNPCSpawns.AddNewItem();
        }

        void btnDeleteBGItem_Click(object sender, EventArgs e)
        {
            // TODO: Add background item deletion support
            Debug.Fail("Not implemented...");
        }

        void btnDeleteSpawn_Click(object sender, EventArgs e)
        {
            lstNPCSpawns.DeleteSelectedItem();
        }

        void btnNewBGLayer_Click(object sender, EventArgs e)
        {
            BackgroundLayer bgLayer = new BackgroundLayer();
            Map.AddBackgroundImage(bgLayer);
        }

        void btnNewBGSprite_Click(object sender, EventArgs e)
        {
            // TODO: Add background sprite support...
            Debug.Fail("Not implemented...");
        }

        void btnNewEntity_Click(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            // Get the selected type
            Type selectedType = cmbEntityTypes.SelectedItem as Type;
            if (selectedType == null)
                return;

            // Create the Entity
            Entity entity = (Entity)Activator.CreateInstance(selectedType);
            Map.AddEntity(entity);

            // Move to the center of the screen
            Vector2 size = new Vector2(64);
            entity.Position = Camera.Min + (Camera.Size / 2) - (size / 2);
            entity.Size = size;
        }

        void chkDrawBackground_CheckedChanged(object sender, EventArgs e)
        {
            Map.DrawBackground = chkDrawBackground.Checked;
        }

        void chkShowGrhs_CheckedChanged(object sender, EventArgs e)
        {
            Map.DrawMapGrhs = chkShowGrhs.Checked;
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
            using (FileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Load map";
                ofd.InitialDirectory = ContentPaths.Dev.Maps;
                ofd.RestoreDirectory = true;

                DialogResult result = ofd.ShowDialog();
                if (result != DialogResult.Cancel)
                    SetMap(ofd.FileName);
            }
        }

        void cmdNew_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you wish to create a new map? All changes to the current map will be lost.",
                                "Create new map?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            MapIndex index = Map.GetNextFreeIndex(ContentPaths.Dev);

            string newMapPath = ContentPaths.Dev.Maps.Join(index + "." + Map.MapFileSuffix);
            if (File.Exists(newMapPath))
            {
                MessageBox.Show(string.Format("Map.GetNextFreeIndex() returned the index [{0}] of an existing map!", index));
                return;
            }

            Map = new Map(index, _world, GameScreen.GraphicsDevice);
            DBController.GetQuery<InsertMapQuery>().Execute(Map);
            Map.OnSave += Map_OnSave;
            Map.SetDimensions(new Vector2(30 * 32, 20 * 32));
            Map.Save(index, ContentPaths.Dev);
            SetMap(newMapPath);
        }

        void cmdSave_Click(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            Cursor = Cursors.WaitCursor;
            Enabled = false;

            // Add the MapGrh-bound walls
            var extraWalls = _mapGrhWalls.CreateWallList(Map.MapGrhs);
            foreach (WallEntityBase wall in extraWalls)
            {
                Map.AddEntity(wall);
            }

            // Save the map
            Map.Save(Map.Index, ContentPaths.Dev);

            // Remove the extra walls
            foreach (WallEntityBase wall in extraWalls)
            {
                Map.RemoveEntity(wall);
            }

            Enabled = true;
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Creates an instance of a MapDrawExtensionBase that automatically updates the Map and Enabled properties.
        /// </summary>
        /// <typeparam name="T">Type of MapDrawExtensionBase to create.</typeparam>
        /// <param name="checkBox">The CheckBox that is used to set the Enabled property.</param>
        /// <returns>The instanced MapDrawExtensionBase of type <typeparamref name="T"/>.</returns>
        // ReSharper disable UnusedMethodReturnValue.Local
        T CreateMapDrawExtensionBase<T>(CheckBox checkBox) where T : MapDrawExtensionBase, new()
            // ReSharper restore UnusedMethodReturnValue.Local
        {
            // Create the instance of the MapDrawExtensionBase
            T instance = new T
            {
                Map = Map,
                Enabled = checkBox.Checked
            };

            // Handle when the CheckBox value changes
            checkBox.CheckedChanged += ((obj, e) => instance.Enabled = ((CheckBox)obj).Checked);

            // Handle when the Map changes
            OnChangeMap += ((oldMap, newMap) => instance.Map = newMap);

            return instance;
        }

        public void DrawGame()
        {
            // Clear the background
            GameScreen.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw a Grh if its being edited
            if (_editGrh != null)
            {
                DrawGrhPreview();
                return;
            }

            // Check for a valid map
            if (Map == null)
                return;

            // Begin the rendering
            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _camera.Matrix);

            // Map
            Map.Draw(_sb, _camera);

            // MapGrh bound walls
            if (chkDrawAutoWalls.Checked)
            {
                foreach (MapGrh mg in Map.MapGrhs)
                {
                    if (!_camera.InView(mg.Grh, mg.Destination))
                        continue;

                    var boundWalls = _mapGrhWalls[mg.Grh.GrhData];
                    if (boundWalls == null)
                        continue;

                    foreach (WallEntityBase wall in boundWalls)
                    {
                        EntityDrawer.Draw(_sb, wall, mg.Destination);
                    }
                }
            }

            // Border
            _mapBorderDrawer.Draw(_sb, Map, _camera);

            // Selection area
            if (_selectedTool != null)
                _selectedTool.DrawSelection(this);

            // Grid
            if (chkDrawGrid.Checked)
                _grid.Draw(_sb, _camera);

            // On-screen wall editor
            foreach (TransBox box in _transBoxes)
            {
                box.Draw(_sb);
            }

            // Selected Grh
            if (_selectedTool is AddGrhCursor && _selectedGrh.GrhData != null)
            {
                Vector2 drawPos;
                if (chkSnapGrhGrid.Checked)
                    drawPos = _grid.AlignDown(_cursorPos);
                else
                    drawPos = _cursorPos;

                // If we fail to draw the selected Grh, just ignore it
                try
                {
                    _selectedGrh.Draw(_sb, drawPos, _drawPreviewColor);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }

            // Tool interface
            if (_selectedTool != null)
                _selectedTool.DrawInterface(this);

            // End map rendering
            _sb.End();

            // Begin GUI rendering
            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            // Cursor position
            Vector2 cursorPosText = new Vector2(GameScreen.Size.Width, GameScreen.Size.Height);
            cursorPosText -= new Vector2(100, 30);
            _sb.DrawStringShaded(SpriteFont, _cursorPos.ToString(), cursorPosText, Color.White, Color.Black);

            // End GUI rendering and present
            _sb.End();
            GameScreen.GraphicsDevice.Present();
        }

        void DrawGrhPreview()
        {
            // Begin rendering
            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, _editGrhCamera.Matrix);

            // Grh - try/catch since invalid texture will throw an exception
#if !DEBUG
            try
            {
#endif
            _editGrh.Draw(_sb, Vector2.Zero);
#if !DEBUG
            }
            catch
            {
            }
#endif

            // Walls
            foreach (object o in _editGrhWallItems)
            {
                WallEntity wall = o as WallEntity;
                if (wall != null)
                    EntityDrawer.Draw(_sb, wall);
            }

            // End rendering
            _sb.End();
            GameScreen.GraphicsDevice.Present();
        }

        void EditGrhForm_Close(object sender, FormClosedEventArgs e)
        {
            if (_editGrhData != null && _editNode != null)
            {
                _editNode.Remove();
                treeGrhs.UpdateGrhData(_editGrhData);
            }

            _editNode = null;
            _editGrh = null;
            _editGrhData = null;
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

        void GameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseButton = e.Button;
            if (Map == null)
                return;

            GameScreen.Focus();

            if (_editGrh != null)
                return;

            // Forward to the corresponding tool's reaction to the screen's MouseDown
            _selectedTool.MouseDown(this, e);
        }

        void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseButton = e.Button;
            if (Map == null)
                return;

            // Update the cursor position
            _cursorPos = _camera.ToWorld(e.X, e.Y);

            if (_editGrh != null)
                return;

            // Forward to the corresponding tool's reaction to the screen's MouseMove
            _selectedTool.MouseMove(this, e);
        }

        void GameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseButton = e.Button;
            if (Map == null)
                return;

            if (_editGrh != null)
                return;

            // Forward to the corresponding tool's reaction to the screen's MouseUp
            _selectedTool.MouseUp(this, e);
        }

        static IEnumerable<Control> GetAllControls(Control root)
        {
            var ret = new List<Control>
            {
                root
            };

            foreach (Control child in root.Controls)
            {
                ret.AddRange(GetAllControls(child));
            }

            return ret;
        }

        string GetCategoryFromTreeNode(TreeNode node)
        {
            string category = "Uncategorized";

            // Check for a valid node
            if (node != null)
            {
                // Try and get the GrhData
                GrhData tmpGrhData = GrhTreeView.GetGrhData(treeGrhs.SelectedNode);

                if (tmpGrhData != null)
                {
                    // GrhData found, so use the category from that
                    category = tmpGrhData.Category;
                }
                else if (treeGrhs.SelectedNode.Name.Length == 0)
                {
                    // No GrhData found, so if the node has no name (is a folder), use its path
                    category = treeGrhs.SelectedNode.FullPath.Replace(treeGrhs.PathSeparator, ".");
                }
            }

            return category;
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
            // Load the database connection
            DBConnectionSettings settings = new DBConnectionSettings();
            _dbController = new DBController(settings.SqlConnectionString());
            AllianceManager.Initialize(DBController);
            ItemTemplateManager.Initialize(DBController);
            CharacterTemplateManager.Initialize(DBController);

            // Create the engine objects 
            _content = new ContentManager(GameScreen.Services, ContentPaths.Build.Root);
            _sb = new SpriteBatch(GameScreen.GraphicsDevice);

            // Font
            _spriteFont = _content.Load<SpriteFont>(ContentPaths.Build.Fonts.Join("Game"));
            Character.NameFont = SpriteFont;

            // Load the Grh information
            GrhInfo.Load(ContentPaths.Dev.Data.Join("grhdata.xml"), _content);
            treeGrhs.Initialize(_content);
            TransBox.Initialize(GrhInfo.GetData("System", "Move"), GrhInfo.GetData("System", "Resize"));

            // Hook GrhTreeView context menu click events
            treeGrhs.GrhContextMenuEditClick += treeGrhs_mnuEdit;
            treeGrhs.GrhContextMenuDuplicateClick += treeGrhs_mnuDuplicate;
            treeGrhs.GrhContextMenuBatchChangeTextureClick += treeGrhs_mnuBatchChangeTexture;
            treeGrhs.GrhContextMenuNewGrhClick += treeGrhs_mnuNewGrh;

            // Start the stopwatch for the elapsed time checking
            _stopWatch.Start();

            // Set the wall types
            cmbWallType.Items.Clear();
            foreach (CollisionType item in EnumHelper.GetValues<CollisionType>())
            {
                cmbWallType.Items.Add(item);
            }
            cmbWallType.SelectedItem = CollisionType.Full;

            // Hook the toolbar visuals
            foreach (PictureBox pic in panToolBar.Controls)
            {
                pic.MouseClick += toolBarItem_Click;
            }
            toolBarItem_Click(picToolSelect, null);

            // Hook all controls to forward camera movement keys Form
            KeyEventHandler kehDown = OnKeyDownForward;
            KeyEventHandler kehUp = OnKeyUpForward;
            HookFormKeyEvents(this, kehDown, kehUp);

            // Load the first map
            SetMap(ContentPaths.Dev.Maps.Join("1." + Map.MapFileSuffix));

            // Set up the MapItemListBoxes
            foreach (MapItemListBox lb in GetAllControls(this).OfType<MapItemListBox>())
            {
                lb.IMap = Map;
                lb.Camera = Camera;
            }

            // Set up the MapExtensionBases
            CreateMapDrawExtensionBase<MapEntityBoxDrawer>(chkDrawEntities);
            CreateMapDrawExtensionBase<MapWallDrawer>(chkShowWalls);
            MapSpawnDrawer v = CreateMapDrawExtensionBase<MapSpawnDrawer>(chkDrawSpawnAreas);
            // NOTE: Using SelectedIndexChanged for this may be a stupid idea...
            lstNPCSpawns.SelectedIndexChanged += ((o, e) => v.MapSpawns = ((NPCSpawnsListBox)o).GetMapSpawnValues());
        }

        /// <summary>
        /// Loads the map editor settings
        /// </summary>
        void LoadSettings()
        {
            if (!File.Exists("MapEditorSettings.xml"))
                return;

            var data = XmlInfoReader.ReadFile("MapEditorSettings.xml");
            if (data == null)
                return;

            foreach (var d in data)
            {
                foreach (string key in d.Keys)
                {
                    string value;
                    if (!d.TryGetValue(key, out value))
                        continue;

                    switch (key)
                    {
                        case "Grid.Width":
                            txtGridWidth.Text = value;
                            break;
                        case "Grid.Height":
                            txtGridHeight.Text = value;
                            break;
                        case "Walls.SnapWallsToWalls":
                            chkSnapWallWall.Checked = bool.Parse(value);
                            break;
                        case "Walls.SnapWallsToGrid":
                            chkSnapWallGrid.Checked = bool.Parse(value);
                            break;
                        case "Display.Grid":
                            chkDrawGrid.Checked = bool.Parse(value);
                            break;
                        case "Display.Grhs":
                            chkShowGrhs.Checked = bool.Parse(value);
                            break;
                        case "Display.Walls":
                            chkShowWalls.Checked = bool.Parse(value);
                            break;
                        case "Display.AutoWalls":
                            chkDrawAutoWalls.Checked = bool.Parse(value);
                            break;
                        case "Display.Entities":
                            chkDrawEntities.Checked = bool.Parse(value);
                            break;
                        case "Display.Background":
                            chkDrawBackground.Checked = bool.Parse(value);
                            break;
                        case "Display.SpawnAreas":
                            chkDrawSpawnAreas.Checked = bool.Parse(value);
                            break;
                        case "Display.PersistentNPCs":
                            chkDrawPersistentNPCs.Checked = bool.Parse(value);
                            break;
                    }
                }
            }
        }

        void lstBGItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pgBGItem.SelectedObject != lstBGItems.SelectedItem)
                pgBGItem.SelectedObject = lstBGItems.SelectedItem;
        }

        void lstEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pgEntity.SelectedObject != lstEntities.SelectedItem)
                pgEntity.SelectedObject = lstEntities.SelectedItem;
        }

        void lstSelectedWalls_SelectedIndexChanged(object sender, EventArgs e)
        {
            WallEntityBase selected = lstSelectedWalls.SelectedItem as WallEntityBase;
            if (selected == null)
                return;

            if (pgWall.SelectedObject != selected)
                pgWall.SelectedObject = selected;
        }

        void lstSelectedWalls_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            WallCursor.SelectedWalls.Clear();
            foreach (ListboxWall lbw in lstSelectedWalls.SelectedItems)
            {
                WallCursor.SelectedWalls.Add(lbw);
            }
        }

        void Map_OnSave(MapBase map)
        {
            DBController.GetQuery<UpdateMapQuery>().Execute(map);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            Control focusControl = FindFocusControl(this);
            if (focusControl != null && focusControl.GetType() == typeof(TextBox))
                return;

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
                    if (_selectedTool != null)
                        _selectedTool.PressDelete(this);
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
            if (IsKeyToForward(e.KeyCode))
                OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Vector2 startMoveCamera = new Vector2(_moveCamera.X, _moveCamera.Y);

            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                case Keys.S:
                case Keys.Down:
                    _moveCamera.Y = 0;
                    break;

                case Keys.Right:
                case Keys.D:
                case Keys.A:
                case Keys.Left:
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
            if (IsKeyToForward(e.KeyCode))
                OnKeyUp(e);
        }

        void picToolGrhsAdd_Click(object sender, EventArgs e)
        {
            tcMenu.SelectTab(tabPageGrhs);
        }

        /// <summary>
        /// Saves the map editor settings
        /// </summary>
        void SaveSettings()
        {
            // TODO: Use the IRestorableSettings instead

            using (FileStream stream = new FileStream("MapEditorSettings.xml", FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };
                using (XmlWriter w = XmlWriter.Create(stream, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter to save settings.");

                    w.WriteStartDocument();
                    w.WriteStartElement("Settings");

                    w.WriteStartElement("Walls");
                    w.WriteElementString("SnapWallsToWalls", chkSnapWallWall.Checked.ToString());
                    w.WriteElementString("SnapWallsToGrid", chkSnapWallGrid.Checked.ToString());
                    w.WriteEndElement();

                    w.WriteStartElement("Grid");
                    w.WriteElementString("Width", _grid.Width.ToString());
                    w.WriteElementString("Height", _grid.Height.ToString());
                    w.WriteEndElement();

                    w.WriteStartElement("Display");
                    w.WriteElementString("Grid", chkDrawGrid.Checked.ToString().ToLower());
                    w.WriteElementString("Grhs", chkShowGrhs.Checked.ToString().ToLower());
                    w.WriteElementString("Walls", chkShowWalls.Checked.ToString().ToLower());
                    w.WriteElementString("AutoWalls", chkDrawAutoWalls.Checked.ToString().ToLower());
                    w.WriteElementString("Entities", chkDrawEntities.Checked.ToString().ToLower());
                    w.WriteElementString("Background", chkDrawBackground.Checked.ToString().ToLower());
                    w.WriteElementString("SpawnAreas", chkDrawSpawnAreas.Checked.ToString().ToLower());
                    w.WriteElementString("PersistentNPCs", chkDrawPersistentNPCs.Checked.ToString().ToLower());
                    w.WriteEndElement();

                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                }
            }
        }

        void ScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GrhInfo.Save(ContentPaths.Dev.Data.Join("grhdata.xml"));
            SaveSettings();
        }

        void ScreenForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEditor();
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
        }

        /// <summary>
        /// Sets the map being used. Use this instead of directly setting Map.
        /// </summary>
        /// <param name="filePath">Path to the map to use</param>
        void SetMap(string filePath)
        {
            MapIndex index = Map.GetIndexFromPath(filePath);
            Map = new Map(index, _world, GameScreen.GraphicsDevice);
        }

        void tabPageGrhs_Enter(object sender, EventArgs e)
        {
            treeGrhs.Select();
        }

        internal void toolBarItem_Click(object sender, EventArgs e)
        {
            // Set the background colors for the tools
            foreach (PictureBox pic in panToolBar.Controls)
            {
                pic.BackColor = System.Drawing.Color.White;
                pic.BorderStyle = BorderStyle.None;
            }

            PictureBox src = (PictureBox)sender;
            if (src == null)
                return;

            src.BackColor = System.Drawing.Color.LightGreen;
            src.BorderStyle = BorderStyle.FixedSingle;

            // Set the selected tool
            if (src == picToolGrhs)
                _selectedTool = GrhCursor;
            else if (src == picToolSelect)
                _selectedTool = EntityCursor;
            else if (src == picToolGrhsAdd)
                _selectedTool = AddGrhCursor;
            else if (src == picToolWalls)
                _selectedTool = WallCursor;
            else if (src == picToolWallsAdd)
                _selectedTool = AddWallCursor;

            // Clear some selection stuff
            WallCursor.SelectedWalls.Clear();
            _transBoxes.Clear();
            _selTransBox = null;
        }

        void treeGrhs_DoubleClickGrh(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                BeginEditGrhData(e.Node, e.GrhData);
        }

        void treeGrhs_mnuBatchChangeTexture(object sender, EventArgs e)
        {
            TreeNode node = treeGrhs.SelectedNode;
            if (node == null)
                return;

            // Show the new form
            BatchRenameTextureForm frm = new BatchRenameTextureForm(node, _content);

            // Disable this form until the rename one closes
            Enabled = false;
            frm.FormClosed += delegate
                              {
                                  Enabled = true;
                              };
            frm.Show();
        }

        void treeGrhs_mnuDuplicate(object sender, EventArgs e)
        {
            TreeNode node = treeGrhs.SelectedNode;

            // Confirm the duplicate request
            string text = string.Format("Are you sure you wish to duplicate these {0} nodes?", GrhTreeView.NodeCount(node));
            if (MessageBox.Show(text, "Duplicate nodes?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            treeGrhs.DuplicateNodes(node);
        }

        void treeGrhs_mnuEdit(object sender, EventArgs e)
        {
            TreeNode node = treeGrhs.SelectedNode;
            GrhData gd = GrhTreeView.GetGrhData(node);

            if (node == null)
                return;

            if (gd != null && node.Nodes.Count == 0)
            {
                // Grh node
                BeginEditGrhData(node, gd);
            }
            else if (gd == null)
            {
                // Folder node
                node.BeginEdit();
            }
        }

        void treeGrhs_mnuNewGrh(object sender, EventArgs e)
        {
            // Create the new GrhData
            string category = GetCategoryFromTreeNode(treeGrhs.SelectedNode);
            GrhData gd = GrhInfo.CreateGrhData(_content, category);
            treeGrhs.UpdateGrhData(gd);

            // Begin edit
            var nodes = treeGrhs.Nodes.Find(gd.GrhIndex.ToString(), true);
            if (nodes.Length == 0)
            {
                Debug.Fail("Failed to find node.");
                return;
            }

            BeginEditGrhData(nodes[0], gd);
        }

        void treeGrhs_SelectGrh(object sender, GrhTreeViewEventArgs e)
        {
            if (_selectedGrh.GrhData == null || e.GrhData.GrhIndex != _selectedGrh.GrhData.GrhIndex)
            {
                _selectedGrh.SetGrh(e.GrhData.GrhIndex, AnimType.Loop, _currentTime);
                toolBarItem_Click(picToolGrhsAdd, null);
                picToolGrhsAdd_Click(this, null);
            }
        }

        void txtGridHeight_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridHeight.Text, out result))
                _grid.Height = result;
        }

        void txtGridWidth_TextChanged(object sender, EventArgs e)
        {
            if (Map == null)
                return;

            float result;
            if (Parser.Current.TryParse(txtGridWidth.Text, out result))
                _grid.Width = result;
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

            if (_selectedTool != null)
                _selectedTool.UpdateCursor(this);

            // Set to default if it wasn't yet set
            Cursor = Cursors.Default;
        }

        public void UpdateGame()
        {
            // Update the time
            int currTime = (int)_stopWatch.ElapsedMilliseconds;
            int deltaTime = currTime - _currentTime;
            _currentTime = (int)_stopWatch.ElapsedMilliseconds;

            // Edited Grh
            if (_editGrh != null)
                _editGrh.Update(_currentTime);

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

        /// <summary>
        /// Updates the list of selected walls
        /// </summary>
        public void UpdateSelectedWallsList(List<WallEntityBase> selectedWalls)
        {
            lstSelectedWalls.Items.Clear();
            var tmpSelectedWalls = selectedWalls.ToArray();
            foreach (WallEntityBase wall in tmpSelectedWalls)
            {
                lstSelectedWalls.AddItemAndReselect(new ListboxWall(wall));
            }

            tcMenu.SelectTab(tabPageWalls);
            lstSelectedWalls.Focus();

            for (int i = 0; i < lstSelectedWalls.Items.Count; i++)
            {
                lstSelectedWalls.SetSelected(i, true);
            }

            if (selectedWalls.Count == 1)
                pgWall.SelectedObject = selectedWalls[0];
            else
                pgWall.SelectedObject = null;
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