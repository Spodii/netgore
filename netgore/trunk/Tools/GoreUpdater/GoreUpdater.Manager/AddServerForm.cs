using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class AddServerForm : Form
    {
        static readonly object[] _fileDownloaderTypes = Enum.GetValues(typeof(DownloadSourceType)).OfType<object>().ToArray();
        static readonly object[] _fileUploaderTypes = Enum.GetValues(typeof(FileUploaderType)).OfType<object>().ToArray();

        /// <summary>
        /// If this form is for adding a master server. If false, assumes its for adding a file server.
        /// </summary>
        readonly bool _isMasterServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddServerForm"/> class.
        /// </summary>
        /// <param name="isMasterServer">If this form is for adding a master server. If false, assumes its for adding a
        /// file server.</param>
        public AddServerForm(bool isMasterServer)
        {
            _isMasterServer = isMasterServer;

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
            if (_isMasterServer)
                Text = "Add master server";
            else
                Text = "Add file server";

            // Add the items to the uploader type combo box
            cmbType.Items.Clear();
            cmbType.Items.AddRange(_fileUploaderTypes);
            cmbType.SelectedIndex = 0;

            // Add the items to the downloader type combo box
            cmbDownloadType.Items.Clear();
            cmbDownloadType.Items.AddRange(_fileDownloaderTypes);
            cmbDownloadType.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            // Confirm
            const string confirmMsg = "Are you sure you wish to cancel adding a server?";
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
            const string confirmMsg =
                "Are you sure you wish to create this server?{0}It is highly recommended that you test the settings" +
                " first and make sure they pass, but not required. Although you can change the settings later, it is best to avoid" +
                " doing so whenever possible.";

            if (MessageBox.Show(string.Format(confirmMsg, Environment.NewLine), "Create server?", MessageBoxButtons.YesNo) ==
                DialogResult.No)
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

                // Create
                if (_isMasterServer)
                {
                    var server = new MasterServerInfo(type, host, user, pass, downloadType, downloadHost);
                    ManagerSettings.Instance.AddMasterServer(server);
                }
                else
                {
                    var server = new FileServerInfo(type, host, user, pass, downloadType, downloadHost);
                    ManagerSettings.Instance.AddFileServer(server);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create the server instance. Reason: {0}";
                MessageBox.Show(string.Format(errmsg, ex));
                return;
            }

            MessageBox.Show("Server successfully created!", "Success!", MessageBoxButtons.OK);
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
        /// Handles the Click event of the lblDownloadHostHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lblDownloadHostHelp_Click(object sender, EventArgs e)
        {
            // TODO:
        }

        /// <summary>
        /// Handles the Click event of the lblDownloadTypeHelp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lblDownloadTypeHelp_Click(object sender, EventArgs e)
        {
            // TODO:
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
    }
}