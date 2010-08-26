using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class MainForm : Form
    {
        static readonly ManagerSettings _settings = ManagerSettings.Instance;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _settings.LiveVersionChanged += _settings_LiveVersionChanged;
            lblLiveVersion.Text = _settings.LiveVersion.ToString();
        }

        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            lblLiveVersion.Invoke((Action)(() => lblLiveVersion.Text = _settings.LiveVersion.ToString()));
        }

        /// <summary>
        /// Handles the Click event of the btnChangeLiveVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnChangeLiveVersion_Click(object sender, EventArgs e)
        {
            // Check that the next version even exists
            var nextVersion = _settings.LiveVersion + 1;

            if (!Directory.Exists(VersionHelper.GetVersionPath(nextVersion)) ||
                !File.Exists(VersionHelper.GetVersionFileListPath(nextVersion)))
            {
                MessageBox.Show(
                    "Cannot change the live version since you are already at the latest version." + Environment.NewLine +
                    "If you want to change the live version, create a new version first.", "Error", MessageBoxButtons.OK);
                return;
            }

            // Ensure the next version's contents are valid
            VersionFileList vfl;
            try
            {
                vfl = VersionFileList.CreateFromFile(VersionHelper.GetVersionFileListPath(nextVersion));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The file listing file for the next version exists, but it is corrupt. Please use Create New Version to recreate the next version then try again." +
                    Environment.NewLine + "Inner exception:" + Environment.NewLine + ex, "Corrupt file listing file",
                    MessageBoxButtons.OK);
                return;
            }

            // Confirm change
            if (MessageBox.Show("Are you sure you wish to update the live version?", "Update live version?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;

            // If the new version is still being uploaded to the servers, warn the user
            // TODO: ...

            // Check that the file hashes match up
            // TODO: ...

            // Update the version number on the master servers
            // TODO: ...

            // Done!
            MessageBox.Show("The live version has been successfully updated!", "Done!", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles the Click event of the btnNewVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNewVersion_Click(object sender, EventArgs e)
        {
            var newVersionForm = new NewVersionForm();
            btnNewVersion.Enabled = false;
            newVersionForm.FormClosed += newVersionForm_FormClosed;

            newVersionForm.Show();
        }

        /// <summary>
        /// Handles the FormClosed event of the newVersionForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        void newVersionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnNewVersion.Enabled = true;
        }
    }
}