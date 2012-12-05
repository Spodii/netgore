using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.IO;

namespace DemoGame.Editor
{
    public partial class BodyEditorForm : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodyEditorForm"/> class.
        /// </summary>
        public BodyEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Save();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            lstBodies.UpdateList();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            Save();
        }

        /// <summary>
        /// Saves the <see cref="BodyInfo"/>s.
        /// </summary>
        static void Save()
        {
            // Save to dev path, then copy it over to build path
            BodyInfoManager.Instance.Save(ContentPaths.Dev);

            var src = BodyInfoManager.GetDefaultFilePath(ContentPaths.Dev);
            var dest = BodyInfoManager.GetDefaultFilePath(ContentPaths.Build);
            File.Copy(src, dest, true);
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            var selected = lstBodies.SelectedItem as BodyInfo;
            if (selected == null)
                return;

            // Show confirmation
            const string deleteMsg = "Are you sure you wish to delete body `{0}`?";
            if (MessageBox.Show(string.Format(deleteMsg, selected), "Delete body?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Delete
            lstBodies.RemoveItemAtAndReselect(lstBodies.SelectedIndex);
            BodyInfoManager.Instance.RemoveBody(selected.ID);
            lstBodies.UpdateList();

            if (pg.SelectedObject == selected)
                pg.SelectedObject = null;
        }

        /// <summary>
        /// Handles the Click event of the <see cref="btnNew"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNew_Click(object sender, EventArgs e)
        {
            // Create a new BodyInfo
            var newBody = BodyInfoManager.Instance.CreateBody();
            lstBodies.UpdateList();
            lstBodies.SelectedItem = newBody;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the <see cref="lstBodies"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            pg.SelectedObject = lstBodies.SelectedItem;
        }
    }
}