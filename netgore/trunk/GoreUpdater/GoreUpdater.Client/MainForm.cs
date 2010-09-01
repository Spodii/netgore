using System;
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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var targetDir=Application.StartupPath;
            var settingsPath =PathHelper.CombineDifferentPaths(Application.StartupPath, "UpdaterSettings");
            var resetAppPath = Application.ExecutablePath;
            var settings = new UpdateClientSettings(targetDir, settingsPath, resetAppPath);

            _uc = new UpdateClient(settings);
            _uc.StateChanged += new UpdateClientStateChangedEventHandler(_uc_StateChanged);
            _uc.LiveVersionFound += new UpdateClientEventHandler(_uc_LiveVersionFound);
            _uc.FileDownloaded += new UpdateClientFileDownloadedEventHandler(_uc_FileDownloaded);
            _uc.FileDownloadFailed += new UpdateClientFileDownloadFailedEventHandler(_uc_FileDownloadFailed);
            _uc.FileMoveFailed += new UpdateClientFileMoveFailedEventHandler(_uc_FileMoveFailed);
            _uc.MasterServerReaderError += new UpdateClientMasterServerReaderErrorEventHandler(_uc_MasterServerReaderError);
            _uc.IsRunningChanged += new UpdateClientEventHandler(_uc_IsRunningChanged);

            _uc.Start();
        }

        void _uc_MasterServerReaderError(UpdateClient sender, string error)
        {
            LogLine("MasterServerReader error: " + error);
        }

        void _uc_IsRunningChanged(UpdateClient sender)
        {
            LogLine("IsRunning set to: " + sender.IsRunning);

            if (sender.IsRunning)
            {
                if (sender.TryExecuteOfflineFileReplacer())
                {
                    MessageBox.Show("Resetting to update files that couldn't be updated...");
                    Close();
                }
            }
        }

        void _uc_FileMoveFailed(UpdateClient sender, string remoteFile, string localFilePath, string targetFilePath)
        {
            LogLine("File move failed: " + remoteFile);
        }

        void _uc_FileDownloadFailed(UpdateClient sender, string remoteFile)
        {
            LogLine("File download failed: " + remoteFile);
        }

        void _uc_FileDownloaded(UpdateClient sender, string remoteFile, string localFilePath)
        {
            LogLine("File downloaded: " + remoteFile);
        }

        void _uc_LiveVersionFound(UpdateClient sender)
        {
            LogLine("Live version found: " + sender.LiveVersion);
        }

        void _uc_StateChanged(UpdateClient sender, UpdateClientState oldState, UpdateClientState newState)
        {
            LogLine("State changed: " + newState);
        }
    }
}