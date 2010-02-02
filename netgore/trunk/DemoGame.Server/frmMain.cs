using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NetGore;

namespace DemoGame.Server
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// The URL of the web site to get the external IP address from.
        /// </summary>
        const string _externalIPSite = "http://www.whatismyip.org/";

        /// <summary>
        /// The maximum number of log entries to display.
        /// </summary>
        const int _maxLogDisplayLines = 150;

        readonly object _consoleOutLock = new object();
        readonly MemoryAppender _logger;
        readonly Thread _serverThread;
        readonly Thread _watchLogThread;

        ConsoleCommands _consoleCommands;

        /// <summary>
        /// Incrementing counter that keeps track of the number of times <see cref="tmrUpdateDisplay_Tick"/> has been called.
        /// </summary>
        int _interfaceUpdateTicker;

        Server _server;

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            _logger = new MemoryAppender();
            BasicConfigurator.Configure(_logger);

            InitializeComponent();

            _serverThread = new Thread(ServerThread) { Name = "Server Thread" };
        }

        /// <summary>
        /// Appends text to the console output.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The text type.</param>
        void AppendToConsole(string text, ConsoleTextType type)
        {
            Color color;

            switch (type)
            {
                case ConsoleTextType.InputReturn:
                    color = Color.Black;
                    break;

                case ConsoleTextType.Info:
                    color = Color.Blue;
                    break;

                default:
                    color = Color.Green;
                    break;
            }

            lock (_consoleOutLock)
            {
                txtConsoleOut.ForeColor = color;
                txtConsoleOut.AppendText(text + Environment.NewLine);
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppendToConsole("Shutting down...", ConsoleTextType.Info);
            txtConsoleOut.Refresh();

            _server.Shutdown();
            _serverThread.Join();
        }

        /// <summary>
        /// Handles the Load event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void frmMain_Load(object sender, EventArgs e)
        {
            Show();
            Update();

            UpdateExternalIP();

            EngineSettingsInitializer.Initialize();

            _serverThread.Start();

            _consoleCommands = new ConsoleCommands(_server);

            AppendToConsole("Server started. Type 'help' for a list of server console commands.", ConsoleTextType.Info);

            _watchLogThread.Start();
        }

        /// <summary>
        /// Handles the KeyDown event of the lbLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void lbLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                lbLog.SelectedIndex = -1;
        }

        /// <summary>
        /// Worker thread for the server.
        /// </summary>
        void ServerThread()
        {
            // Create the server
            using (_server = new Server())
            {
                // Start the main loop (the thread will block here until the server is closed)
                _server.Start();
            }
        }

        /// <summary>
        /// Handles the Tick event of the tmrUpdateDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void tmrUpdateDisplay_Tick(object sender, EventArgs eArgs)
        {
            ++_interfaceUpdateTicker;

            // Get the latest events
            LoggingEvent[] events = _logger.GetEvents();
            _logger.Clear();

            // Ensure there are events
            if (events != null && events.Length > 0)
            {
                lbLog.SuspendLayout();
                try
                {
                    // Add the events
                    lbLog.Items.AddRange(events);

                    // If too long, truncate
                    while (lbLog.Items.Count > _maxLogDisplayLines)
                    {
                        lbLog.Items.RemoveAt(0);
                    }

                    // Scroll down to see the latest item if nothing is selected
                    if (lbLog.SelectedIndex < 0)
                    {
                        lbLog.SelectedIndex = lbLog.Items.Count - 1;
                        lbLog.ClearSelected();
                    }
                }
                finally
                {
                    lbLog.ResumeLayout();
                }
            }

            // Update the CPU and memory usage values
            if (_interfaceUpdateTicker % 4 == 0)
            {
                lblCPU.Text = Math.Round(SystemPerformance.CPU.Usage) + "%";
                lblRAMUsed.Text = SystemPerformance.Memory.ProcessUsageMB + " MB";
                lblRAMFree.Text = SystemPerformance.Memory.AvailableMB + " MB";
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the txtConsoleIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void txtConsoleIn_KeyDown(object sender, KeyEventArgs e)
        {
            string txt = txtConsoleIn.Text;

            if (e.KeyCode != Keys.Return || string.IsNullOrEmpty(txt))
                return;

            txtConsoleIn.Text = string.Empty;

            var resultStr = _consoleCommands.ExecuteCommand(txt);

            AppendToConsole(txt, ConsoleTextType.Input);

            if (!string.IsNullOrEmpty(resultStr))
                AppendToConsole(resultStr, ConsoleTextType.InputReturn);
        }

        /// <summary>
        /// Creates a background thread to find and update the external IP address text.
        /// </summary>
        void UpdateExternalIP()
        {
            string externalIP = string.Empty;
            string hostName = string.Empty;

            var w = new BackgroundWorker();

            // Create the work method
            w.DoWork += delegate
                        {
                            hostName = Dns.GetHostName();

                            try
                            {
                                using (var c = new WebClient())
                                {
                                    externalIP = c.DownloadString(_externalIPSite);
                                }
                            }
                            catch (WebException)
                            {
                                externalIP = "Failed to get IP";
                            }
                        };

            // Create the updater
            w.RunWorkerCompleted += delegate
                                    {
                                        lblIP.Text = string.Format("{0} ({1})", externalIP, hostName);
                                        w.Dispose();
                                    };

            // Run the worker
            w.RunWorkerAsync();
        }

        /// <summary>
        /// Enum of the different console text types.
        /// </summary>
        enum ConsoleTextType
        {
            /// <summary>
            /// Entered input text.
            /// </summary>
            Input,

            /// <summary>
            /// Returned text from input (such as return results from a command).
            /// </summary>
            InputReturn,

            /// <summary>
            /// Informative messages.
            /// </summary>
            Info
        }
    }
}