using System;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater
{
    public partial class Form1 : Form
    {
        DownloadManager _dm;
        BatchOfflineFileReplacer _fileRep;
        MasterServerReader _msr;

        public Form1()
        {
            InitializeComponent();
        }

        void LogLine(string s)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText(s + Environment.NewLine)));
        }

        void MSRCallback(IMasterServerReader sender, IMasterServerReadInfo info, object userState)
        {
            LogLine("Done reading from master servers.");
            LogLine(" * Latest version: " + info.Version);

            var tempPath = PathHelper.CombineDifferentPaths(Application.StartupPath, "_temp");
            var targetPath = PathHelper.CombineDifferentPaths(Application.StartupPath, "Downloaded");

            var replacerFilePath = PathHelper.CombineDifferentPaths(Application.StartupPath, GlobalSettings.ReplacerFileName);
            _fileRep = new BatchOfflineFileReplacer(replacerFilePath, Application.ExecutablePath);

            _dm = new DownloadManager(targetPath, tempPath, 1);
            _dm.DownloadFinished += _dm_DownloadFinished;
            _dm.FileMoveFailed += _dm_FileMoveFailed;
            _dm.DownloadFailed += _dm_DownloadFailed;
            _dm.Finished += _dm_Finished;

            var sources = info.DownloadSources.Select(x => x.Instantiate());
            _dm.AddSources(sources);

            _dm.Enqueue(new string[] { "tab_a.png", "tab_b.png", "tab_h.png", "tabs.css" });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var sourceListPath = PathHelper.CombineDifferentPaths(Application.StartupPath, MasterServerReader.CurrentDownloadSourcesFilePath);
            var masterListPath = PathHelper.CombineDifferentPaths(Application.StartupPath, MasterServerReader.CurrentMasterServersFilePath);
            _msr = new MasterServerReader(sourceListPath, masterListPath);

            LogLine("Reading from master servers...");

            _msr.BeginRead(MSRCallback, null);
        }

        void _dm_DownloadFailed(IDownloadManager sender, string remoteFile)
        {
            LogLine("FAIL (DOWNLOAD): " + remoteFile);

            if (_dm.QueueCount == 0)
                LogLine(" === ALL DONE ===");
        }

        void _dm_DownloadFinished(IDownloadManager sender, string remoteFile, string localFilePath)
        {
            LogLine("DONE: " + remoteFile);

            if (_dm.QueueCount == 0)
                LogLine(" === ALL DONE ===");
        }

        void _dm_FileMoveFailed(IDownloadManager sender, string remoteFile, string localFilePath, string targetFilePath)
        {
            _fileRep.AddJob(localFilePath, targetFilePath);

            LogLine("FAIL (MOVE): " + remoteFile);

            if (_dm.QueueCount == 0)
                LogLine(" === ALL DONE ===");
        }

        void _dm_Finished(IDownloadManager sender)
        {
            // If we have any failed file moves, invoke the file replacer script and close the program 
            if (_fileRep.JobCount > 0)
            {
                if (OfflineFileReplacerHelper.TryExecute(_fileRep.FilePath))
                    Invoke((Action)(Close));
            }
        }
    }
}