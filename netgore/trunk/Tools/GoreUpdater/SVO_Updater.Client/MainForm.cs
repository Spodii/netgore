using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater
{
    public partial class MainForm : Form
    {
        UpdateClient _uc;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        void LogLine(string s)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText(s + Environment.NewLine)));
        }
        void Status(string s)
        {
            txtStatus.Invoke((Action)(() => txtStatus.ResetText()));
            txtStatus.Invoke((Action)(() => txtStatus.AppendText(s + Environment.NewLine)));
        }
        void Progress(string s)
        {
            txtProgress.Invoke((Action)(() => txtProgress.ResetText()));
            txtProgress.Invoke((Action)(() => txtProgress.AppendText(s + Environment.NewLine)));
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var targetDir = Application.StartupPath;
            var settingsPath = PathHelper.CombineDifferentPaths(Application.StartupPath, "UpdaterSettings");
            var resetAppPath = Application.ExecutablePath;
            var settings = new UpdateClientSettings(targetDir, settingsPath, resetAppPath);

            _uc = new UpdateClient(settings);
            _uc.StateChanged += _uc_StateChanged;
            _uc.LiveVersionFound += _uc_LiveVersionFound;
            _uc.FileDownloaded += _uc_FileDownloaded;
            _uc.FileDownloadFailed += _uc_FileDownloadFailed;
            _uc.FileMoveFailed += _uc_FileMoveFailed;
            _uc.MasterServerReaderError += _uc_MasterServerReaderError;
            _uc.IsRunningChanged += _uc_IsRunningChanged;
            _uc.HasErrorsChanged += _uc_HasErrorsChanged;

            _uc.Start();
        }

        void _uc_FileDownloadFailed(UpdateClient sender, string remoteFile)
        {
            LogLine("Download failed: " + remoteFile);
            Progress("Download failed: " + remoteFile);
        }

        void _uc_FileDownloaded(UpdateClient sender, string remoteFile, string localFilePath)
        {
            LogLine("Downloaded: " + remoteFile);
            Progress("Downloaded: " + remoteFile);
        }

        void _uc_FileMoveFailed(UpdateClient sender, string remoteFile, string localFilePath, string targetFilePath)
        {
            LogLine("File move failed: " + remoteFile);
            Progress("Move Failed: " + remoteFile);
        }

        void _uc_HasErrorsChanged(UpdateClient sender)
        {
            LogLine("Errors: " + sender.HasErrors);
        }

        void _uc_IsRunningChanged(UpdateClient sender)
        {
            button1.Enabled = sender.IsRunning ? false : true;
            
            LogLine("Running: " + sender.IsRunning);

            if (sender.IsRunning)
            {
                if (sender.TryExecuteOfflineFileReplacer())
                {
                    MessageBox.Show("Resetting to update files that couldn't be updated...");
                    Close();
                }
            }
        }

        void _uc_LiveVersionFound(UpdateClient sender)
        {
            LogLine("Live version found: " + sender.LiveVersion);
            Status("Version found");
            Progress(sender.LiveVersion.ToString());
        }

        void _uc_MasterServerReaderError(UpdateClient sender, string error)
        {
            LogLine("MasterServerReader error: " + error);
            Status("Error");
            Progress(error.ToString());
        }

        void _uc_StateChanged(UpdateClient sender, UpdateClientState oldState, UpdateClientState newState)
        {
            LogLine("State: " + newState);
            Status(newState.ToString());
            if (newState.ToString() == "Completed")
            {
                if (checkBox1.Checked == true)
                {
                    Process.Start("SVOGame.exe","+startclient");
                    CloseAllForms();
                   
                    
                }
                button1.Invoke((Action)(() => button1.Enabled = true));
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void CloseAllForms()
        {
            // get array because collection changes as we close forms
            Form[] forms = OpenForms;

            // close every open form
            foreach (Form form in forms)
            {
                CloseForm(form);
            }
        }
        public Form[] OpenForms
        {
            get
            {
                Form[] forms = null;
                int count = Application.OpenForms.Count;
                forms = new Form[count];
                if (count > 0)
                {
                    int index = 0;
                    foreach (Form form in Application.OpenForms)
                    {
                        forms[index++] = form;
                    }
                }
                return forms;
            }
        }
        delegate void CloseMethod(Form form);
        private void CloseForm(Form form)
        {

            if (!form.IsDisposed)
            {
                if (form.InvokeRequired)
                {
                    CloseMethod method = new CloseMethod(CloseForm);
                    form.Invoke(method, new object[] { form });
                }
                else
                {
                    form.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("SVOGame.exe", "+startclient");
            CloseAllForms();
        }
    }
}