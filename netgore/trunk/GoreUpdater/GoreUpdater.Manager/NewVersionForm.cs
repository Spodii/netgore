using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class NewVersionForm : Form
    {
        int _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewVersionForm"/> class.
        /// </summary>>
        public NewVersionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Use the version immediately after the live version
            _version = ManagerSettings.Instance.LiveVersion + 1;

            lblVersion.Text = _version.ToString();

            try
            {
                // Load the filters from the previous version and use that for the default filter
                var prevVersionPath = VersionHelper.GetVersionFileListPath(_version - 1);
                if (File.Exists(prevVersionPath))
                {
                    var pv = VersionFileList.CreateFromFile(prevVersionPath);

                    var sb = new StringBuilder();
                    foreach (var f in pv.Filters)
                    {
                        sb.AppendLine(f);
                    }

                    txtIgnoreFilter.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                txtIgnoreFilter.Text = string.Empty;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to cancel?", "Cancel?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            Close();
        }

        /// <summary>
        /// Handles the Click event of the btnCreate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCreate_Click(object sender, EventArgs e)
        {
            // Create the directory if it does not already exist
            try
            {
                if (!Directory.Exists(txtRootPath.Text))
                    Directory.CreateDirectory(txtRootPath.Text);
            }
            catch (Exception)
            {
            }

            // Ensure there are files in the list
            if (!Directory.Exists(txtRootPath.Text) ||
                Directory.GetFiles(txtRootPath.Text, "*", SearchOption.AllDirectories).Length <= 0)
            {
                const string errmsg = "You must first add the files for this new version to the path:{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, txtRootPath.Text));
                return;
            }

            const string confirmationMsg = "Are you sure you wish to create this new version?";
            if (MessageBox.Show(confirmationMsg, "Create new version?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Create the version file list
            VersionFileList vfl;
            try
            {
                vfl = VersionFileList.Create(txtRootPath.Text, txtIgnoreFilter.Lines);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Failed to create VersionFileList due to an unexpected error." +
                    " Please resolve the error below then try again.{0}{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, ex));
                return;
            }

            // Save the file list
            try
            {
                ManagerSettings.Instance.SetNextVersion(vfl);
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Failed to set the next version due to an unexpected error." +
                    " Please resolve the error below then try again.{0}{1}";
                MessageBox.Show(string.Format(errmsg, Environment.NewLine, ex));
                return;
            }

            MessageBox.Show("Version `" + _version + "` successfully created!", "Success!", MessageBoxButtons.OK);
            Close();
        }

        /// <summary>
        /// Handles the TextChanged event of the lblVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lblVersion_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (!int.TryParse(lblVersion.Text, out i))
                return;

            txtRootPath.Text = VersionHelper.GetVersionPath(i);
        }
    }
}