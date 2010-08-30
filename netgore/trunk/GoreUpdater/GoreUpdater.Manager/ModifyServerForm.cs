using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class ModifyServerForm : Form
    {
        static readonly object[] _fileUploaderTypes = Enum.GetValues(typeof(FileUploaderType)).OfType<object>().ToArray();
        static readonly object[] _fileDownloaderTypes = Enum.GetValues(typeof(DownloadSourceType)).OfType<object>().ToArray();

        /// <summary>
        /// The server being modified.
        /// </summary>
        readonly ServerInfoBase _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyServerForm"/> class.
        /// </summary>
        /// <param name="server">The server being modified.</param>
        public ModifyServerForm(ServerInfoBase server)
        {
            _server = server;

            InitializeComponent();
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

            // Change the form text
            if (_server is MasterServerInfo)
                Text = "Modify master server";
            else
                Text = "Modify file server";

            // Add the items to the type combo box
            cmbType.Items.Clear();
            cmbType.Items.AddRange(_fileUploaderTypes);
            cmbType.SelectedIndex = 0;

            // Enter the current values
            txtHost.Text = _server.Host;
            txtPassword.Text = _server.Password;
            txtUser.Text = _server.User;
            cmbType.SelectedItem = _server.FileUploaderType;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            // Confirm
            const string confirmMsg = "Are you sure you wish to cancel modifying this server?";
            if (MessageBox.Show(confirmMsg, "Cancel?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Close the form
            Close();
        }

        /// <summary>
        /// Handles the Click event of the btnCreate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCreate_Click(object sender, EventArgs e)
        {
            // Confirm
            const string confirmMsg = "Are you sure you with to modify this server with the new values?";
            if (MessageBox.Show(confirmMsg, "Modify server?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                var type = (FileUploaderType)cmbType.SelectedItem;
                var host = txtHost.Text;
                var user = txtUser.Text;
                var pass = txtPassword.Text;
                var downloadType = (DownloadSourceType)cmbDownloadType.SelectedItem;
                var downloadHost = txtDownloadHost.Text;

                if (string.IsNullOrEmpty(host))
                {
                    MessageBox.Show("Please enter a valid host.", "Invalid value", MessageBoxButtons.OK);
                    txtHost.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(downloadHost))
                {
                    MessageBox.Show("Please enter a valid download host.", "Invalid value", MessageBoxButtons.OK);
                    txtDownloadHost.Focus();
                    return;
                }

                if (!Enum.IsDefined(typeof(FileUploaderType), type))
                {
                    const string errmsg =
                        "Invalid FileUploaderType type ({0}) supplied - not a known enum value. Please select a valid type.";
                    cmbType.SelectedIndex = 0;
                    throw new InvalidEnumArgumentException(string.Format(errmsg, type));
                }

                if (!Enum.IsDefined(typeof(DownloadSourceType), downloadType))
                {
                    const string errmsg =
                        "Invalid DownloadSourceType type ({0}) supplied - not a known enum value. Please select a valid type.";
                    cmbDownloadType.SelectedIndex = 0;
                    throw new InvalidEnumArgumentException(string.Format(errmsg, downloadType));
                }

                // Update
                _server.ChangeInfo(host, user, pass, type, downloadType, downloadHost);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to modify the server. Reason: {0}";
                MessageBox.Show(string.Format(errmsg, ex));
                return;
            }

            MessageBox.Show("Server successfully modified!", "Success!", MessageBoxButtons.OK);
            Close();
        }

        /// <summary>
        /// Handles the Click event of the btnTest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnTest_Click(object sender, EventArgs e)
        {
            var type = (FileUploaderType)cmbType.SelectedItem;
            var host = txtHost.Text;
            var user = txtUser.Text;
            var pass = txtPassword.Text;

            IFileUploader server;
            try
            {
                server = ServerInfoBase.CreateFileUploader(type, host, user, pass);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Failed to create the IFileUploader to use for testing the connection due to an unexpected error:{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, ex), "Error", MessageBoxButtons.OK);
                return;
            }

            var testForm = new TestServerSettingsForm(server);
            btnTest.Enabled = false;
            testForm.FormClosing += delegate { btnTest.Invoke((Action)(() => btnTest.Enabled = true)); };
            testForm.StartTesting();
        }

        /// <summary>
        /// Handles the Click event of the lblHostHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblHostHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpHost);
        }

        /// <summary>
        /// Handles the Click event of the lblPasswordHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblPasswordHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpAccountPassword);
        }

        /// <summary>
        /// Handles the Click event of the lblTypeHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblTypeHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpServerType);
        }

        /// <summary>
        /// Handles the Click event of the lblUserHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void lblUserHelp_Click(object sender, EventArgs e)
        {
            HelpHelper.DisplayHelp(HelpHelper.HelpAccountUser);
        }

        /// <summary>
        /// Handles the Click event of the lblDownloadTypeHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lblDownloadTypeHelp_Click(object sender, EventArgs e)
        {
            // TODO:
        }

        /// <summary>
        /// Handles the Click event of the lblDownloadHostHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lblDownloadHostHelp_Click(object sender, EventArgs e)
        {
            // TODO:
        }
    }
}