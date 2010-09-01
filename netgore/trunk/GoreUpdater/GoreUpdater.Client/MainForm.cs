using System;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater
{
    public partial class MainForm : Form
    {
        DownloadManager _dm;
        BatchOfflineFileReplacer _fileRep;
        MasterServerReader _msr;

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
        }
    }
}