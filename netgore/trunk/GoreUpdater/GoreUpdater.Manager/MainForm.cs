using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// TODO: Display the list of file servers on the GUI + be able to edit the entries
// TODO: Create the master server logic
// TODO: Start work on the client

// TODO: [LATER] Add the logging framework and make use of it

namespace GoreUpdater.Manager
{
    public partial class MainForm : Form
    {
        static readonly ManagerSettings _settings = ManagerSettings.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
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
            _settings.NextVersionCreated += _settings_NextVersionCreated;

            btnChangeLiveVersion.Enabled = _settings.DoesNextVersionExist();
            lblLiveVersion.Text = _settings.LiveVersion.ToString();
        }

        void _settings_NextVersionCreated(ManagerSettings sender)
        {
            btnChangeLiveVersion.Enabled = sender.DoesNextVersionExist();
        }

        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            lblLiveVersion.Invoke((Action)(() => lblLiveVersion.Text = sender.LiveVersion.ToString()));
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
            try
            {
                VersionFileList.CreateFromFile(VersionHelper.GetVersionFileListPath(nextVersion));
            }
            catch (Exception ex)
            {
                const string errmsg = "The file listing file for the next version exists, but it is corrupt." +
                    " Please use Create New Version to recreate the next version then try again.{0}Inner Exception:{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, ex), "Corrupt file listing file",  MessageBoxButtons.OK);
                return;
            }

            // Confirm change
            if (
                MessageBox.Show("Are you sure you wish to update the live version?", "Update live version?",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // If the new version is still being uploaded to the servers, warn the user
            bool areServersBusy = _settings.FileServers.Any(x => x.IsBusySyncing);
            if (areServersBusy)
            {
                const string msg = "Are you sure you wish to update the live version? One or more servers (either file or master servers)" +
                    " are still synchronizing. Updating the live version now may leave you in a state where not all of the files" +
                    " are available on all of the servers. It is highly recommended you wait until all synchronization finishes." +
                    "{0}{0}Do you wish to continue? (Canceling is highly recommended!)";
                if (MessageBox.Show(msg, "Continue updating live version?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }

            // Update the version number on the master servers
            _settings.TrySetLiveVersion(_settings.LiveVersion + 1);

            // Done!
            const string doneMsg = "The live version has been successfully updated!" +
                " The master servers will automatically start to update with the new live version number.";
            MessageBox.Show(doneMsg, "Done!", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles the Click event of the btnNewVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnNewVersion_Click(object sender, EventArgs e)
        {
            // If the version already exists, confirm that they want to recreate it
            if (_settings.DoesNextVersionExist())
            {
                const string msg = "The next version already exists, it just has not been made live yet." +
                    " Are you sure you wish to recreate the next version?";
                if (MessageBox.Show(msg, "Recreate next version?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            // Show the form for creating the new version
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