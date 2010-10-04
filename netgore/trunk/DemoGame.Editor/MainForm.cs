using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.World;

namespace DemoGame.Editor
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
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

            // Set the ToolBarVisibility values. Do it here instead of setting the properties to avoid messing up
            // the controls order.
            tbGlobal.ToolBarVisibility = ToolBarVisibility.Global;
            tbMap.ToolBarVisibility = ToolBarVisibility.Map;

            // HACK: Force the ToolManager to initialize. Won't be needed when we load the settings here instead.
            var x = ToolManager.Instance;
        }

        void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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

        void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new EditMapForm();
            frm.MapScreenControl.ChangeMap(new MapID(1));

            frm.Show(dockPanel);
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: !!
        }
    }
}