using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Editor.Forms.Dockable;
using DemoGame.Editor.Properties;
using DemoGame.Editor.Tools;
using DemoGame.Editor.UITypeEditors;
using NetGore;
using NetGore.Content;
using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.EditorTool;
using NetGore.Editor.Grhs;
using NetGore.Editor.UI;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Contains the latest <see cref="MainForm"/> instance. Only deal with one instance since we assume only one
        /// instance exists (why would anyone run more than one instance in the same program?).
        /// </summary>
        static MainForm _instance;

        readonly DockContentDeserializer _deserializer;

        BodyEditorForm _frmBodyEditor;
        DbEditorForm _frmDbEditor;
        GrhTreeViewForm _frmGrhTreeView;
        MusicEditorForm _frmMusicEditor;
        NPCChatEditorForm _frmNPCChatEditor;
        SelectedMapObjectsForm _frmSelectedMapObjs;
        SkeletonEditorForm _frmSkeletonEditor;
        SoundEditorForm _frmSoundEditor;
        private GrhTilesetForm _frmGrhTilesetForm;

        public static DockPanel DockPanel { get { return _instance.dockPanel; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            _instance = this;

            InitializeComponent();

            _deserializer = new DockContentDeserializer(this);
        }

        static void SetStatusMessageLoading(string message)
        {
            SetStatusMessage("Loading: " + message + "...");
        }

        /// <summary>
        /// Sets the message in the status toolbar.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void SetStatusMessage(string message)
        {
            try
            {
                var frm = _instance;
                if (frm == null)
                    return;

                frm.tssInfo.Text = message ?? string.Empty;
                frm.tssInfo.GetCurrentParent().Refresh();
            }
            catch (Exception ex)
            {
                // Doesn't matter if we somehow have an error
                Debug.Fail(ex.ToString());
            }
        }

        static string GetDockSettingsFilePath(string settingsName)
        {
            return ContentPaths.Build.Settings.Join("EditorLayout." + settingsName + ".xml");
        }

        void LoadDockSettings(string settingsName)
        {
            var filePath = GetDockSettingsFilePath(settingsName);
            if (File.Exists(filePath))
                dockPanel.LoadFromXml(filePath, _deserializer.Deserialize);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DesignMode)
            {
                base.OnClosing(e);
                return;
            }

            // Save the GrhDatas
            GrhInfo.Save(ContentPaths.Dev);

            // Save body infos
            BodyInfoManager.Instance.Save(ContentPaths.Dev);

            // Save ParticleEffects
            ParticleEffectManager.Instance.Save(ContentPaths.Dev);

            // Save the tool settings
            ToolManager.Instance.SaveSettings();

            // Save docking settings
            SaveDockSettings("User");

            base.OnClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            tssInfo.Text = string.Empty;
            UpdateCursorPos(Vector2.Zero, Vector2.Zero);

            Show();

            // Set the ToolBarVisibility values. Do it here instead of setting the properties to avoid messing up
            // the controls order.
            tbGlobal.ToolBarVisibility = ToolBarVisibility.Global;
            tbMap.ToolBarVisibility = ToolBarVisibility.Map;

            // Auto-update GrhDatas
            SetStatusMessageLoading("Updating GrhData");
            GlobalState.Instance.AutoUpdateGrhDatas();
            GlobalState.Instance.AutoUpdateMusic();
            GlobalState.Instance.AutoUpdateSounds();
         

            SetStatusMessageLoading("Updating cached GrhData sizes");
            AutomaticGrhDataSizeUpdater.Instance.UpdateSizes();

            // Enable the update timer
            GlobalState.Instance.IsTickEnabled = true;

            // Create the child form instances
            SetStatusMessageLoading("Creating child forms");
            _frmGrhTreeView = new GrhTreeViewForm();
            _frmGrhTreeView.VisibleChanged += _frmGrhTreeView_VisibleChanged;

            _frmSelectedMapObjs = new SelectedMapObjectsForm();
            _frmSelectedMapObjs.VisibleChanged += _frmSelectedMapObjs_VisibleChanged;

            _frmNPCChatEditor = new NPCChatEditorForm();
            _frmNPCChatEditor.VisibleChanged += _frmNPCChatEditor_VisibleChanged;

            _frmSkeletonEditor = new SkeletonEditorForm();
            _frmSkeletonEditor.VisibleChanged += _frmSkeletonEditor_VisibleChanged;

            _frmDbEditor = new DbEditorForm();
            _frmDbEditor.VisibleChanged += _frmDbEditor_VisibleChanged;

            _frmMusicEditor = new MusicEditorForm();
            _frmMusicEditor.VisibleChanged += _frmMusicEditor_VisibleChanged;

            _frmBodyEditor = new BodyEditorForm();
            _frmBodyEditor.VisibleChanged += _frmBodyEditor_VisibleChanged;

            _frmSoundEditor = new SoundEditorForm();
            _frmSoundEditor.VisibleChanged += _frmSoundEditor_VisibleChanged;

            _frmGrhTilesetForm = new GrhTilesetForm();
            _frmGrhTilesetForm.VisibleChanged += new EventHandler(_frmGrhTilesetForm_VisibleChanged);

            // Set up some other stuff
            var mapPropertiesTool = ToolManager.Instance.TryGetTool<MapPropertiesTool>();
            if (mapPropertiesTool != null)
                mapPropertiesTool.DockPanel = dockPanel;
            else
            {
                const string errmsg = "Unable to set DockPanel on MapPropertiesTool - couldn't find tool instance.";
                Debug.Fail(errmsg);
            }

            // Load the settings
            SetStatusMessageLoading("Loading user settings");
            LoadDockSettings("User");

            // Set up map state control listeners & display initial values
            SetStatusMessageLoading("Hooking up map events");
            GlobalState.Instance.Map.LayerDepthChanged -= Map_LayerDepthChanged;
            GlobalState.Instance.Map.LayerDepthChanged += Map_LayerDepthChanged;

            GlobalState.Instance.Map.LayerChanged -= Map_LayerChanged;
            GlobalState.Instance.Map.LayerChanged += Map_LayerChanged;

            Map_LayerChanged(GlobalState.Instance, EventArgs.Empty);
            Map_LayerDepthChanged(GlobalState.Instance, EventArgs.Empty);

            // Clean up bad maps
            SetStatusMessageLoading("Validating map ids");
            MapHelper.DeleteMissingMapIds();

            // Load the initial map
            SetStatusMessageLoading("Loading the initial map");
            var validMaps = MapHelper.FindAllMaps().ToArray();
            if (validMaps.Length > 0)
            {
                MapID initialMapId = (MapID)EditorSettings.Default.InitialMapId;
                IMapTable initialMap = validMaps.FirstOrDefault(x => x.ID == initialMapId);

                if (initialMap == null)
                    initialMap = validMaps.MinElement(x => x.ID);

                CreateEditMapForm(initialMap);
            }

            // Global not currently used...
            tbGlobal.Visible = false;

            // Bind help
            SetStatusMessageLoading("Setting up F1 help");
            var helpManager = EditorHelpManager.Instance;
            helpManager.StatusLabel = tssInfo;
            helpManager.Add(cmbLayer, "Map Layers", "Map layers");
            helpManager.Add(trackBarDepth, "Layer Depth", "Layer depth");

            SetStatusMessage(string.Empty);
        }

        void _frmGrhTilesetForm_VisibleChanged(object sender, EventArgs e)
        {
            grhTilesetViewMenuItem.Checked = ((Form)sender).Visible;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Display help (F1)
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.Help)
            {
                if (EditorHelpManager.Instance.RaiseHelp())
                    e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        static MapRenderLayer GetLayerFromComboBoxText(string text)
        {
            switch (text)
            {
                case "Background": return MapRenderLayer.SpriteBackground;
                case "Dynamic": return MapRenderLayer.Dynamic;
                case "Foreground": return MapRenderLayer.SpriteForeground;
                default: throw new ArgumentException("Invalid text: " + text);
            }
        }

        static string GetComboBoxTextFromLayer(MapRenderLayer layer)
        {
            switch (layer)
            {
                case MapRenderLayer.SpriteBackground: return "Background";
                case MapRenderLayer.Dynamic: return "Dynamic";
                case MapRenderLayer.SpriteForeground: return "Foreground";
                default: throw new InvalidEnumArgumentException("layer", (int)layer, typeof(MapRenderLayer));
            }
        }

        void Map_LayerChanged(object sender, EventArgs e)
        {
            // Update layer combobox
            var current = GlobalState.Instance.Map.Layer;
            if (cmbLayer.SelectedItem == null || current != GetLayerFromComboBoxText(cmbLayer.SelectedItem.ToString()))
            {
                cmbLayer.SelectedItem = GetComboBoxTextFromLayer(current);
            }
        }

        void Map_LayerDepthChanged(object sender, EventArgs e)
        {
            // Update depth value
            var current = GlobalState.Instance.Map.LayerDepth;
            if (trackBarDepth.Value != current)
                trackBarDepth.Value = current.Clamp(trackBarDepth.Minimum, trackBarDepth.Maximum);

            lblDepth.Text = current.ToString();
        }

        void SaveDockSettings(string settingsName)
        {
            var filePath = GetDockSettingsFilePath(settingsName);
            dockPanel.SaveAsXml(filePath);
        }

        /// <summary>
        /// Updates the screen and world position status bar texts.
        /// </summary>
        /// <param name="worldPos">The world position.</param>
        /// <param name="screenPos">The screen position.</param>
        public static void UpdateCursorPos(Vector2 worldPos, Vector2 screenPos)
        {
            var instance = _instance;
            if (instance == null)
                return;

            instance.tssWorldPos.Text = string.Format("World: {0}{2}{1}", Parser.Current.ToString(worldPos.X),
                Parser.Current.ToString(worldPos.Y), Parser.Current.NumberFormatInfo.NumberGroupSeparator);

            instance.tssScreenPos.Text = string.Format("Screen: {0}{2}{1}", Parser.Current.ToString(screenPos.X),
                Parser.Current.ToString(screenPos.Y), Parser.Current.NumberFormatInfo.NumberGroupSeparator);
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmBodyEditor"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmBodyEditor_VisibleChanged(object sender, EventArgs e)
        {
            bodyEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmDbEditor"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmDbEditor_VisibleChanged(object sender, EventArgs e)
        {
            dbEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmGrhTreeView"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmGrhTreeView_VisibleChanged(object sender, EventArgs e)
        {
            grhDatasToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmMusicEditor"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmMusicEditor_VisibleChanged(object sender, EventArgs e)
        {
            musicEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmNPCChatEditor"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmNPCChatEditor_VisibleChanged(object sender, EventArgs e)
        {
            npcChatEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmSelectedMapObjs"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmSelectedMapObjs_VisibleChanged(object sender, EventArgs e)
        {
            selectedMapObjectsToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the <see cref="_frmSkeletonEditor"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmSkeletonEditor_VisibleChanged(object sender, EventArgs e)
        {
            skeletonEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmSoundEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmSoundEditor_VisibleChanged(object sender, EventArgs e)
        {
            soundEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the Click event of the <see cref="bodyEditorToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void bodyEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bodyEditorToolStripMenuItem.Checked)
                _frmBodyEditor.Show(dockPanel, DockState.Float);
            else
                _frmBodyEditor.Hide();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="closeToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="dbEditorToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void dbEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbEditorToolStripMenuItem.Checked)
                _frmDbEditor.Show(dockPanel, DockState.Float);
            else
                _frmDbEditor.Hide();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="dockPanel"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void dockPanel_Click(object sender, EventArgs e)
        {
            // Clear the ToolBarVisibility
            ToolBar.CurrentToolBarVisibility = ToolBarVisibility.Global;
        }

        /// <summary>
        /// Handles the Click event of the <see cref="grhDatasToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void grhDatasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grhDatasToolStripMenuItem.Checked)
                _frmGrhTreeView.Show(dockPanel, DockState.Float);
            else
                _frmGrhTreeView.Hide();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="loadMapToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var uiFrm = new MapUITypeEditorForm(null))
            {
                var result = uiFrm.ShowDialog(this);
                if (result != DialogResult.OK && result != DialogResult.Yes)
                    return;

                var map = uiFrm.SelectedItem;
                if (map == null)
                    return;

                CreateEditMapForm(map);
            }
        }

        /// <summary>
        /// Creates an EditMapForm.
        /// </summary>
        /// <param name="map">The map to be edited.</param>
        void CreateEditMapForm(IMapTable map)
        {
            var editorFrm = new EditMapForm();
            editorFrm.MapScreenControl.ChangeMap(map.ID);
            editorFrm.Text = "[" + map.ID + "] " + map.Name;
            editorFrm.Show(dockPanel);

            var settings = EditorSettings.Default;
            settings.InitialMapId = (int)map.ID;
            settings.Save();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="loadPEToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void loadPEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var uiFrm = new ParticleEffectUITypeEditorForm(null))
            {
                var result = uiFrm.ShowDialog(this);
                if (result != DialogResult.OK && result != DialogResult.Yes)
                    return;

                var effect = uiFrm.SelectedItem;
                if (effect == null)
                    return;

                var editorFrm = new ParticleEditorForm { ParticleEffect = effect };
                editorFrm.Show(dockPanel, DockState.Float);
            }
        }

        /// <summary>
        /// Handles the Click event of the <see cref="musicEditorToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void musicEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (musicEditorToolStripMenuItem.Checked)
                _frmMusicEditor.Show(dockPanel, DockState.Float);
            else
                _frmMusicEditor.Hide();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="newMapToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var id = MapHelper.CreateNewMap(true);
            if (!id.HasValue)
                return;

            var editorFrm = new EditMapForm();
            editorFrm.MapScreenControl.ChangeMap(id.Value);

            editorFrm.Show(dockPanel);
        }

        /// <summary>
        /// Handles the Click event of the <see cref="newPEToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void newPEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string confirmMsg = "Are you sure you wish to create a new particle effect?";
            if (MessageBox.Show(confirmMsg, "Create new particle effect?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Create the new ParticleEffect
            var pe = new ParticleEffect();

            // Save the new ParticleEffect
            ParticleEffectManager.Instance.Save(ContentPaths.Dev);

            // Show the editor form
            var editorFrm = new ParticleEditorForm { ParticleEffect = pe };
            editorFrm.Show(dockPanel, DockState.Float);
        }

        /// <summary>
        /// Handles the Click event of the <see cref="npcChatEditorToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void npcChatEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (npcChatEditorToolStripMenuItem.Checked)
                _frmNPCChatEditor.Show(dockPanel, DockState.Float);
            else
                _frmNPCChatEditor.Hide();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the <see cref="selectedMapObjectsToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void selectedMapObjectsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedMapObjectsToolStripMenuItem.Checked)
                _frmSelectedMapObjs.Show(dockPanel, DockState.Float);
            else
                _frmSelectedMapObjs.Hide();
        }

        /// <summary>
        /// Handles the Click event of the <see cref="skeletonEditorToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void skeletonEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (skeletonEditorToolStripMenuItem.Checked)
                _frmSkeletonEditor.Show(dockPanel, DockState.Float);
            else
                _frmSkeletonEditor.Hide();
        }

        /// <summary>
        /// Handles the Click event of the soundEditorToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void soundEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (soundEditorToolStripMenuItem.Checked)
                _frmSoundEditor.Show(dockPanel, DockState.Float);
            else
                _frmSoundEditor.Hide();
        }

        sealed class DockContentDeserializer
        {
            readonly MainForm _form;

            public DockContentDeserializer(MainForm form)
            {
                _form = form;
            }

            MainForm Owner
            {
                get { return _form; }
            }

            public IDockContent Deserialize(string name)
            {
                if (IsNameFor(Owner._frmDbEditor, name))
                    return Owner._frmDbEditor;

                if (IsNameFor(Owner._frmGrhTreeView, name))
                    return Owner._frmGrhTreeView;

                if (IsNameFor(Owner._frmNPCChatEditor, name))
                    return Owner._frmNPCChatEditor;

                if (IsNameFor(Owner._frmSelectedMapObjs, name))
                    return Owner._frmSelectedMapObjs;

                if (IsNameFor(Owner._frmSkeletonEditor, name))
                    return Owner._frmSkeletonEditor;

                if (IsNameFor(Owner._frmMusicEditor, name))
                    return Owner._frmMusicEditor;

                if (IsNameFor(Owner._frmSoundEditor, name))
                    return Owner._frmSoundEditor;

                if (IsNameFor(Owner._frmBodyEditor, name))
                    return Owner._frmBodyEditor;

                return null;
            }

            static bool IsNameFor(IDockContent control, string name)
            {
                return StringComparer.Ordinal.Equals(control.GetType().ToString(), name);
            }
        }

        private void trackBarDepth_Scroll(object sender, EventArgs e)
        {
            GlobalState.Instance.Map.LayerDepth = (short)trackBarDepth.Value;
        }

        private void cmbLayer_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbLayer.SelectedItem != null)
                GlobalState.Instance.Map.Layer = GetLayerFromComboBoxText(cmbLayer.SelectedItem.ToString());
        }

        private void trackBarDepth_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                GlobalState.Instance.Map.LayerDepth = 0;
        }

        private void shiftContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var map = MapHelper.ActiveMap;
            if (map == null)
                return;

            var frm = new ShiftContentsForm(map);
            frm.Show();
        }

        private void mapToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            mapToolStripMenuItem.Enabled = MapHelper.ActiveMap != null;
        }

        private void grhTilesetViewMenuItem_Click(object sender, EventArgs e)
        {
            if (grhTilesetViewMenuItem.Checked)
                _frmGrhTilesetForm.Show(dockPanel, DockState.Float);
            else
                _frmGrhTilesetForm.Hide();
            
        }
    }
}