using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.Docking;
using NetGore.Editor.EditorTool;
using NetGore.World;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    public partial class MainForm : Form
    {
        DbEditorForm _frmDbEditor;
        GrhTreeViewForm _frmGrhTreeView;
        NPCChatEditorForm _frmNPCChatEditor;
        SelectedObjectsForm _frmSelectedObjs;
        SkeletonEditorForm _frmSkeletonEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the NPCChatEditorToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void NPCChatEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NPCChatEditorToolStripMenuItem.Checked)
                _frmNPCChatEditor.Show(dockPanel, DockState.Float);
            else
                _frmNPCChatEditor.Hide();
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

            ToolManager.Instance.SaveSettings();

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

            // Set the ToolBarVisibility values. Do it here instead of setting the properties to avoid messing up
            // the controls order.
            tbGlobal.ToolBarVisibility = ToolBarVisibility.Global;
            tbMap.ToolBarVisibility = ToolBarVisibility.Map;

            // Enable the update timer
            GlobalState.Instance.IsTickEnabled = true;

            // Create the child form instances
            _frmGrhTreeView = new GrhTreeViewForm();
            _frmGrhTreeView.VisibleChanged += _frmGrhTreeView_VisibleChanged;

            _frmSelectedObjs = new SelectedObjectsForm();
            _frmSelectedObjs.VisibleChanged += _frmSelectedObjs_VisibleChanged;

            _frmNPCChatEditor = new NPCChatEditorForm();
            _frmNPCChatEditor.VisibleChanged += _frmNPCChatEditor_VisibleChanged;

            _frmSkeletonEditor = new SkeletonEditorForm();
            _frmSkeletonEditor.VisibleChanged += _frmSkeletonEditor_VisibleChanged;

            _frmDbEditor = new DbEditorForm();
            _frmDbEditor.VisibleChanged += _frmDbEditor_VisibleChanged;

            // Load the first map
            // NOTE: Temp
            var frm = new EditMapForm();
            frm.MapScreenControl.ChangeMap(new MapID(1));
            frm.Show(dockPanel);
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmDbEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmDbEditor_VisibleChanged(object sender, EventArgs e)
        {
            dbEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmGrhTreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmGrhTreeView_VisibleChanged(object sender, EventArgs e)
        {
            grhDatasToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmNPCChatEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmNPCChatEditor_VisibleChanged(object sender, EventArgs e)
        {
            NPCChatEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmSelectedObjs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmSelectedObjs_VisibleChanged(object sender, EventArgs e)
        {
            selectedObjectsToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the _frmSkeletonEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _frmSkeletonEditor_VisibleChanged(object sender, EventArgs e)
        {
            skeletonEditorToolStripMenuItem.Checked = ((Form)sender).Visible;
        }

        /// <summary>
        /// Handles the Click event of the closeToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the dbEditorToolStripMenuItem control.
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
        /// Handles the Click event of the dockPanel control.
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
        /// Handles the Click event of the loadToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new EditMapForm();
            frm.MapScreenControl.ChangeMap(new MapID(1));

            frm.Show(dockPanel);
        }

        /// <summary>
        /// Handles the Click event of the newToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newToolStripMenuItem.Checked)
                _frmNPCChatEditor.Show(dockPanel, DockState.Float);
            else
                _frmNPCChatEditor.Hide();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the <see cref="selectedObjectsToolStripMenuItem"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void selectedObjectsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedObjectsToolStripMenuItem.Checked)
                _frmSelectedObjs.Show(dockPanel, DockState.Float);
            else
                _frmSelectedObjs.Hide();
        }

        /// <summary>
        /// Handles the Click event of the skeletonEditorToolStripMenuItem control.
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
    }
}