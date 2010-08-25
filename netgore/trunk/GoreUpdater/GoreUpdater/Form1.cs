using System;
using System.Linq;
using System.Windows.Forms;
using GoreUpdater;

namespace GoreUpdater
{
    public partial class Form1 : Form
    {
        DownloadManager _dm;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var tempPath = PathHelper.CombineDifferentPaths(Application.StartupPath, "_temp");
            var targetPath = PathHelper.CombineDifferentPaths(Application.StartupPath, "Downloaded");

            _dm = new DownloadManager(targetPath, tempPath);
            _dm.DownloadFinished += _dm_DownloadFinished;
            _dm.FileMoveFailed += _dm_FileMoveFailed;
            _dm.DownloadFailed += _dm_DownloadFailed;

            _dm.AddSource(new HttpDownloadSource("http://www.netgore.com/docs"));

            _dm.Enqueue(new string[] { "tab_a.png", "tab_b.png", "tab_h.png", "tabs.css" });
        }

        void _dm_DownloadFailed(IDownloadManager sender, string remoteFile)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText("FAIL (DOWNLOAD): " + remoteFile + Environment.NewLine)));

            if (_dm.QueueCount == 0)
                textBox1.Invoke((Action)(() => textBox1.AppendText(" === ALL DONE ===" + Environment.NewLine)));
        }

        void _dm_DownloadFinished(IDownloadManager sender, string remoteFile, string localFilePath)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText("DONE: " + remoteFile + Environment.NewLine)));

            if (_dm.QueueCount == 0)
                textBox1.Invoke((Action)(() => textBox1.AppendText(" === ALL DONE ===" + Environment.NewLine)));
        }

        void _dm_FileMoveFailed(IDownloadManager sender, string remoteFile, string localFilePath, string targetFilePath)
        {
            textBox1.Invoke((Action)(() => textBox1.AppendText("FAIL (MOVE): " + remoteFile + Environment.NewLine)));

            if (_dm.QueueCount == 0)
                textBox1.Invoke((Action)(() => textBox1.AppendText(" === ALL DONE ===" + Environment.NewLine)));
        }
    }
}