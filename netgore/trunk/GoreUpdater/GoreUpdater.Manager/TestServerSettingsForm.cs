using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public partial class TestServerSettingsForm : Form
    {
        readonly IFileUploader _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestServerSettingsForm"/> class.
        /// </summary>
        /// <param name="server">The <see cref="IFileUploader"/> to test.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server"/> is null.</exception>
        public TestServerSettingsForm(IFileUploader server)
        {
            _server = server;

            InitializeComponent();

            Show();
        }

        void _server_TestConnectionMessage(IFileUploader sender, string message, object userState)
        {
            // Ensure the message is for us
            if (userState != this)
                return;

            // Log it
            Log(message);
        }

        bool _isStarted = false;

        Thread _workerThread;

        /// <summary>
        /// Starts the testing.
        /// </summary>
        public void StartTesting()
        {
            if (_isStarted)
                return;

            _isStarted = true;

            _server.TestConnectionMessage += _server_TestConnectionMessage;

            _workerThread = new Thread(WorkerThreadLoop) { IsBackground = true };

            try
            {
                _workerThread.Name = "TestServerSettingsForm worker thread";
            }
            catch (InvalidOperationException)
            {
            }

            _workerThread.Start();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data. </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                if (_workerThread != null && _workerThread.IsAlive)
                {
                    Log("Aborting...");
                    _workerThread.Abort();
                }
            }
            catch (ThreadStateException)
            {
            }
            catch (SecurityException)
            {
            }

            if (!e.Cancel)
                _server.TestConnectionMessage -= _server_TestConnectionMessage;
        }

        /// <summary>
        /// Logs a message to the testing screen.
        /// </summary>
        /// <param name="msg">The message.</param>
        void Log(string msg)
        {
            try
            {
                txtLog.Invoke((Action)delegate
            {
                txtLog.Text += msg + Environment.NewLine;
                txtLog.Select(txtLog.TextLength - 1, 0);
                txtLog.ScrollToCaret();
            });
            }
            catch (InvalidOperationException)
            {
                // InvalidOperationException can happen if we try to log too early or before the form is shown
            }
        }

        /// <summary>
        /// The worker thread that does the actual testing.
        /// </summary>
        void WorkerThreadLoop()
        {
            Log("Server type: " + _server.GetType());
            Log("Starting test...");

            try
            {
                string errmsg;
                bool success = _server.TestConnection(this, out errmsg);

                if (!success)
                {
                    Log("FAILURE: TestConnection returned false, indicating failure. Reason: " + errmsg ?? "[Unknown]");
                    Log("Testing completed UNSUCCESSFULLY. Please close this form when done reading the log.");
                }
                else
                {
                    Log("Testing completed SUCCESSFULLY! Please close this form when done reading the log.");
                }
            }
            catch (Exception ex)
            {
                Log("FAILURE: Exception thrown during testing. Exception:" + Environment.NewLine + ex);
                Log("Testing completed UNSUCCESSFULLY. Please close this form when done reading the log.");
                return;
            }
        }
    }
}
