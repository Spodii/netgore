using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Tools;
using DemoGame.Editor.UITypeEditors;
using NetGore.Editor.Docking;
using NetGore.Editor.EditorTool;
using NetGore.Editor.UI;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;
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

        DbEditorForm _frmDbEditor;
        GrhTreeViewForm _frmGrhTreeView;
        MusicEditorForm _frmMusicEditor;
        NPCChatEditorForm _frmNPCChatEditor;
        SelectedMapObjectsForm _frmSelectedMapObjs;
        SkeletonEditorForm _frmSkeletonEditor;
        SoundEditorForm _frmSoundEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            _instance = this;

            InitializeComponent();

            _deserializer = new DockContentDeserializer(this);
        }

        /// <summary>
        /// Handles the FormLoaded event of the <see cref="EditMapForm"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void EditMapForm_FormLoaded(object sender, EventArgs e)
        {
            if (!_frmSelectedMapObjs.Visible)
                _frmSelectedMapObjs.Show(dockPanel);
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

            Show();

            // Set the ToolBarVisibility values. Do it here instead of setting the properties to avoid messing up
            // the controls order.
            tbGlobal.ToolBarVisibility = ToolBarVisibility.Global;
            tbMap.ToolBarVisibility = ToolBarVisibility.Map;

            // Enable the update timer
            GlobalState.Instance.IsTickEnabled = true;

            // Create the child form instances
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

            _frmSoundEditor = new SoundEditorForm();
            _frmSoundEditor.VisibleChanged += _frmSoundEditor_VisibleChanged;

            // Set up some other stuff
            EditMapForm.FormLoaded += EditMapForm_FormLoaded;

            var mapPropertiesTool = ToolManager.Instance.TryGetTool<MapPropertiesTool>();
            if (mapPropertiesTool != null)
                mapPropertiesTool.DockPanel = dockPanel;
            else
            {
                const string errmsg = "Unable to set DockPanel on MapPropertiesTool - couldn't find tool instance.";
                Debug.Fail(errmsg);
            }

            // Load the settings
            LoadDockSettings("User");
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

            instance.tssWorldPos.Text = string.Format("World: {0},{1}", worldPos.X, worldPos.Y);
            instance.tssScreenPos.Text = string.Format("Screen: {0},{1}", screenPos.X, screenPos.Y);
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
        /// Handles the VisibleChanged event of the _frmMusicEditor control.
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

                var editorFrm = new EditMapForm();
                editorFrm.MapScreenControl.ChangeMap(map.ID);

                editorFrm.Show(dockPanel);
            }
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

                var editorFrm = new ParticleEditorForm();
                editorFrm.ParticleEffect = effect;
                editorFrm.Show(dockPanel, DockState.Float);
            }
        }

        /// <summary>
        /// Handles the Click event of the musicEditorToolStripMenuItem control.
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

                return null;
            }

            static bool IsNameFor(IDockContent control, string name)
            {
                return StringComparer.Ordinal.Equals(control.GetType().ToString(), name);
            }
        }
    }
}