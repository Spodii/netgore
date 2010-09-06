using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        /// Handles the ServerChanged event of a <see cref="FileServerInfo"/> instance.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void FileServerInfo_ServerChanged(ServerInfoBase sender)
        {
            // Refresh the display of the item in the ListBox
            lstFS.Invoke((Action)(() => lstFS.RefreshItem(sender)));
        }

        /// <summary>
        /// Handles the ServerChanged event of a <see cref="MasterServerInfo"/> instance.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void MasterServerInfo_ServerChanged(ServerInfoBase sender)
        {
            // Refresh the display of the item in the ListBox
            lstMS.Invoke((Action)(() => lstMS.RefreshItem(sender)));
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

            _settings.LiveVersionChanged += _settings_LiveVersionChanged;
            _settings.NextVersionCreated += _settings_NextVersionCreated;
            _settings.FileServerListChanged += _settings_FileServerListChanged;
            _settings.MasterServerListChanged += _settings_MasterServerListChanged;

            btnChangeLiveVersion.Enabled = _settings.DoesNextVersionExist();
            lblLiveVersion.Text = _settings.LiveVersion.ToString();

            UpdateServerListBox(lstFS, _settings.FileServers.Cast<ServerInfoBase>(), FileServerInfo_ServerChanged);
            UpdateServerListBox(lstMS, _settings.MasterServers.Cast<ServerInfoBase>(), MasterServerInfo_ServerChanged);
        }

        /// <summary>
        /// Shows the form to add a new server.
        /// </summary>
        /// <param name="isMasterServer">True if it is a master server to add; false for file server.</param>
        void ShowAddNewServerForm(bool isMasterServer)
        {
            // Create the new form, using Invoke() calls to ensure that we have no issues if this is called from another thread
            // for whatever reason (better safe than sorry!)
            Invoke((Action)delegate
            {
                var frm = new AddServerForm(isMasterServer);
                frm.Show();
                frm.Focus();

                btnFSNew.Enabled = false;
                btnMSNew.Enabled = false;

                frm.FormClosed += delegate
                {
                    btnFSNew.Invoke((Action)(() => btnFSNew.Enabled = true));
                    btnMSNew.Invoke((Action)(() => btnMSNew.Enabled = true));
                };
            });
        }

        /// <summary>
        /// Updates the file server listbox.
        /// </summary>
        static void UpdateServerListBox(ServerInfoListBox lb, IEnumerable<ServerInfoBase> servers,
                                        ServerInfoEventHandler changedEventHandler)
        {
            // Use Invoke to ensure we are in the correct thread
            lb.Invoke((Action)delegate
            {
                // Store the selected item so we can restore it when done
                var selected = lb.SelectedItem;

                // Remove the update listener from existing items to make sure we don't add it twice (no harm in removing
                // the event hook if it doesn't exist to begin with)
                foreach (var s in lb.Items.OfType<ServerInfoBase>())
                {
                    s.ServerChanged -= changedEventHandler;
                }

                // Add the update listener to all items (after double-checking that the event listener isn't attached)
                foreach (var s in servers)
                {
                    s.ServerChanged -= changedEventHandler;
                    s.ServerChanged += changedEventHandler;
                }

                // Re-add all items
                lb.Items.Clear();
                lb.Items.AddRange(servers.Cast<object>().ToArray());

                // Restore the selected item if it is still in the list
                if (servers.Any(x => x == selected))
                    lb.SelectedItem = selected;
            });
        }

        /// <summary>
        /// Handles the FileServerListChanged event of the <see cref="_settings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void _settings_FileServerListChanged(ManagerSettings sender)
        {
            UpdateServerListBox(lstFS, sender.FileServers.Cast<ServerInfoBase>(), FileServerInfo_ServerChanged);
        }

        /// <summary>
        /// Handles the LiveVersionChanged event of the <see cref="_settings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void _settings_LiveVersionChanged(ManagerSettings sender)
        {
            lblLiveVersion.Invoke((Action)(() => lblLiveVersion.Text = sender.LiveVersion.ToString()));
        }

        /// <summary>
        /// Handles the MasterServerListChanged event of the <see cref="_settings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void _settings_MasterServerListChanged(ManagerSettings sender)
        {
            UpdateServerListBox(lstMS, sender.MasterServers.Cast<ServerInfoBase>(), MasterServerInfo_ServerChanged);
        }

        /// <summary>
        /// Handles the NextVersionCreated event of the <see cref="_settings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        void _settings_NextVersionCreated(ManagerSettings sender)
        {
            btnChangeLiveVersion.Enabled = sender.DoesNextVersionExist();
        }

        /// <summary>
        /// Handles the Click event of the btnChangeLiveVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void btnChangeLiveVersion_Click(object sender, EventArgs e)
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
                const string errmsg =
                    "The file listing file for the next version exists, but it is corrupt." +
                    " Please use Create New Version to recreate the next version then try again.{0}Inner Exception:{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, ex), "Corrupt file listing file", MessageBoxButtons.OK);
                return;
            }

            // Confirm change
            if (
                MessageBox.Show("Are you sure you wish to update the live version?", "Update live version?",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // If the new version is still being uploaded to the servers, warn the user
            var areServersBusy = _settings.FileServers.Any(x => x.IsBusySyncing);
            if (areServersBusy)
            {
                const string msg =
                    "Are you sure you wish to update the live version? One or more servers (either file or master servers)" +
                    " are still synchronizing. Updating the live version now may leave you in a state where not all of the files" +
                    " are available on all of the servers. It is highly recommended you wait until all synchronization finishes." +
                    "{0}{0}Do you wish to continue? (Canceling is highly recommended!)";
                if (MessageBox.Show(msg, "Continue updating live version?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }

            // Update the version number on the master servers
            _settings.TrySetLiveVersion(_settings.LiveVersion + 1);

            // Done!
            const string doneMsg =
                "The live version has been successfully updated!" +
                " The master servers will automatically start to update with the new live version number.";
            MessageBox.Show(doneMsg, "Done!", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles the Click event of the btnFSDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnFSDelete_Click(object sender, EventArgs e)
        {
            var server = lstFS.SelectedItem as FileServerInfo;
            if (server == null)
                return;

            const string confirmMsg =
                "Are you sure you wish to delete this {0} server?{1} * Host: {2}{1} * User: {3}" +
                "{1}{1}NOTE: This will NOT delete the files off the server!";
            var confirmMsgFormatted = string.Format(confirmMsg, "file", Environment.NewLine, server.Host, server.User);
            if (MessageBox.Show(confirmMsgFormatted, "Delete server?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var removed = _settings.RemoveFileServer(server);
            Debug.Assert(removed, "Why was this server in the ListBox control, but not in the actual server list in the settings?");

            server.Dispose();

            MessageBox.Show("Server successfully deleted.", "Success!", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles the Click event of the btnFSInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnFSInfo_Click(object sender, EventArgs e)
        {
            var server = lstFS.SelectedItem as FileServerInfo;
            if (server == null)
                return;

            var frm = new ModifyServerForm(server);
            frm.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnFSNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnFSNew_Click(object sender, EventArgs e)
        {
            ShowAddNewServerForm(false);
        }

        /// <summary>
        /// Handles the Click event of the btnMSDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMSDelete_Click(object sender, EventArgs e)
        {
            var server = lstMS.SelectedItem as MasterServerInfo;
            if (server == null)
                return;

            const string confirmMsg =
                "Are you sure you wish to delete this {0} server?{1} * Host: {2}{1} * User: {3}" +
                "{1}{1}NOTE: This will NOT delete the files off the server!";
            var confirmMsgFormatted = string.Format(confirmMsg, "master", Environment.NewLine, server.Host, server.User);
            if (MessageBox.Show(confirmMsgFormatted, "Delete server?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var removed = _settings.RemoveMasterServer(server);
            Debug.Assert(removed, "Why was this server in the ListBox control, but not in the actual server list in the settings?");

            server.Dispose();

            MessageBox.Show("Server successfully deleted.", "Success!", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Handles the Click event of the btnMSInfo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMSInfo_Click(object sender, EventArgs e)
        {
            var server = lstMS.SelectedItem as MasterServerInfo;
            if (server == null)
                return;

            var frm = new ModifyServerForm(server);
            frm.Show();
        }

        /// <summary>
        /// Handles the Click event of the btnMSNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnMSNew_Click(object sender, EventArgs e)
        {
            ShowAddNewServerForm(true);
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
                const string msg =
                    "The next version already exists, it just has not been made live yet." +
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
        /// Handles the Click event of the lblChangeLiveVersionHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblChangeLiveVersionHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpChangeLiveVersion);
        }

        /// <summary>
        /// Handles the Click event of the lblCreateNewVersionHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblCreateNewVersionHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpCreateNewVersion);
        }

        /// <summary>
        /// Handles the Click event of the lblLiveVersionHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblLiveVersionHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpLiveVersion);
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