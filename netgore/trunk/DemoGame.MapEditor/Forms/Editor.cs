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
    public partial class Editor : Form
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

        readonly SettingsManager _settingsManager = new SettingsManager("MapEditor",
                                                                        ContentPaths.Build.Settings.Join("MapEditor.xml"));

        /// <summary>
        /// The switches used when creating this form.
        /// </summary>
        readonly IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> _switches;

        EditorCursorManager<ScreenForm> _cursorManager;

        IDbController _dbController;

        KeyEventArgs _keyEventArgs = new KeyEventArgs(Keys.None);

        MiniMapForm _miniMapForm;

        GameScreen _gameScreen;


        /// <summary>
        /// Used to hold the last set Cursor property value without actually changing the cursor. This way, we only actually change the cursor
        /// once per frame, which helps avoid "flickering" from changing it multiple times per frame.
        /// </summary>
        Cursor _currentCursor = Cursors.Default;

        public Editor(IEnumerable<KeyValuePair<CommandLineSwitch, string[]>> switches)
        {
            _switches = switches;
            InitializeComponent();
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

        IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the most recent KeyEventArgs
        /// </summary>
        public KeyEventArgs KeyEventArgs
        {
            get { return _keyEventArgs; }
        }

        /// <summary>
        /// Gets the <see cref="SettingsManager"/>.
        /// </summary>
        public SettingsManager SettingsManager
        {
            get { return _settingsManager; }
        }

        /// <summary>
        /// Handles the OnChangeCurrentCursor event of the CursorManager.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void CursorManager_CurrentCursorChanged(EditorCursorManager<ScreenForm> sender)
        {
            _gameScreen.TransBoxes.Clear();
            _gameScreen.SelectedTransBox = null;
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

                using (var tempMap = new Map(index, _gameScreen.Camera, _gameScreen.World))
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
                return;

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
            if (DesignMode)
                return;

            base.OnKeyDown(e);


            var startMoveCamera = new Vector2(_gameScreen.MoveCamera.X, _gameScreen.MoveCamera.Y);

            Vector2 temp = _gameScreen.MoveCamera;

            switch (e.KeyCode)
            {
                case _cameraUp:

                    //HACK: This was an annoying thing to have to do as when trying to assign straight to the Y value it didnt recognize it as being a variable.
                    temp.Y = -_cameraMoveRate;
                    _gameScreen.MoveCamera = temp;
                    break;

                case _cameraRight:

                    temp.X = -_cameraMoveRate;
                    _gameScreen.MoveCamera = temp;
                    break;
                case _cameraDown:
                    temp.Y = -_cameraMoveRate;
                    _gameScreen.MoveCamera = temp;
                    break;
                case _cameraLeft:
                    temp.X = -_cameraMoveRate;
                    _gameScreen.MoveCamera = temp;
                    break;

                case Keys.Delete:
                    CursorManager.PressDelete();
                    _gameScreen.SelectedTransBox = null;
                    _gameScreen.TransBoxes.Clear();
                    break;
            }

            if (startMoveCamera != _gameScreen.MoveCamera)
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
                return;

            var startMoveCamera = new Vector2(_gameScreen.MoveCamera.X, _gameScreen.MoveCamera.Y);

            Vector2 temp = _gameScreen.MoveCamera;

            switch (e.KeyCode)
            {
                case _cameraUp:
                case _cameraDown:
                    temp.Y = 0;
                    _gameScreen.MoveCamera = temp;
                    break;

                case _cameraLeft:
                case _cameraRight:
                    temp.X = 0;
                    _gameScreen.MoveCamera = temp;
                    break;
            }

            if (startMoveCamera != _gameScreen.MoveCamera)
                e.Handled = true;

            if (e.KeyCode == Keys.F12)
            {
                var previewer = new MapPreviewer();
                var tmpFile = new TempFile();
                previewer.CreatePreview(_gameScreen.RenderWindow, _gameScreen.Map, _gameScreen.MapDrawingExtensions, tmpFile.FilePath);
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

            Show();
            Refresh();

            // Make sure we skip doing all of this loading when in design mode
            if (DesignMode)
                return;

            // Create and set up the cursor manager
           // _cursorManager = new EditorCursorManager<ScreenForm>(this, ToolTip, panToolBar, GameScreen,
                                                                // x => Map != null && !treeGrhs.IsEditingGrhData);
            CursorManager.SelectedCursor = CursorManager.TryGetCursor<EntityCursor>();
            CursorManager.SelectedAltCursor = CursorManager.TryGetCursor<AddEntityCursor>();
            CursorManager.CurrentCursorChanged += CursorManager_CurrentCursorChanged;

            // Create the database connection
            var settings = new DbConnectionSettings();
            _dbController =
                settings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                                                                 x => settings.PromptEditFileMessageBox(x));

            if (_dbController == null)
            {
                Close();
                return;
            }

            // Hook all controls to forward camera movement keys Form
            KeyEventHandler kehDown = OnKeyDownForward;
            KeyEventHandler kehUp = OnKeyUpForward;
            HookFormKeyEvents(this, kehDown, kehUp);

            // Set the custom UITypeEditors
            CustomUITypeEditors.AddEditors(_dbController);

            // Populate the SettingsManager
            PopulateSettingsManager();

            _gameScreen = new GameScreen();
            _gameScreen.Show();

            // Handle any command-line switches
            HandleSwitches(_switches);
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
            //SettingsManager.Add("Grid", Grid);
        }














    }
}
