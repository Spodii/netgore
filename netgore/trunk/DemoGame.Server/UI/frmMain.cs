﻿using System;
using System.Collections.Generic;
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
        /// How many extra items are removed from the log buffer when removing. Allows for less removals to be made.
        /// </summary>
        const int _logBufferRemoveExtra = 50;

        /// <summary>
        /// The maximum number of log events to keep in memory.
        /// </summary>
        const int _logBufferSize = _maxLogDisplayLines * 2;

        /// <summary>
        /// The maximum number of log entries to display.
        /// </summary>
        const int _maxLogDisplayLines = 256;

        readonly object _consoleOutLock = new object();
        readonly List<LoggingEvent> _logBuffer = new List<LoggingEvent>(_logBufferSize);
        readonly MemoryAppender _logger;
        readonly Thread _serverThread;
        bool _formClosing = false;

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
        /// Sets the log display information.
        /// </summary>
        /// <param name="e">The log event.</param>
        void DisplayLogInfo(LoggingEvent e)
        {
            if (!lstLog.Enabled || e == null)
                return;

            txtLogClass.Text = e.LocationInformation.ClassName;
            txtLogMethod.Text = e.LocationInformation.MethodName;
            txtLogLine.Text = e.LocationInformation.LineNumber;
            txtLogMsg.Text = e.RenderedMessage;
            txtLogLevel.Text = e.Level.DisplayName;
            txtLogLevel.ForeColor = e.Level.GetColor();

            tabControl1.SelectTab(tbLogItem);
        }

        /// <summary>
        /// Handles the FormClosing event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formClosing = true;

            AppendToConsole("Shutting down...", ConsoleTextType.Info);
            txtConsoleOut.Refresh();

            _server.Shutdown();
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

            chkDebug.ForeColor = Level.Debug.GetColor();
            chkInfo.ForeColor = Level.Info.GetColor();
            chkWarn.ForeColor = Level.Warn.GetColor();
            chkError.ForeColor = Level.Error.GetColor();
            chkFatal.ForeColor = Level.Fatal.GetColor();

            AppendToConsole("Server started. Type 'help' for a list of server console commands.", ConsoleTextType.Info);
        }

        /// <summary>
        /// Handles the KeyDown event of the lbLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void lbLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lstLog.Enabled = false;
                lstLog.SelectedIndex = lstLog.Items.Count - 1;
                lstLog.SelectedIndex = -1;
                lstLog.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lbLog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lbLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayLogInfo(lstLog.SelectedItem as LoggingEvent);
        }

        /// <summary>
        /// Handles when a console command was executed on the server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="command">The command.</param>
        /// <param name="returnString">The command's return string.</param>
        void Server_ConsoleCommandExecuted(Server server, string command, string returnString)
        {
            var e = new EventHandler(delegate
                                     {
                                         AppendToConsole(command, ConsoleTextType.Input);

                                         if (!string.IsNullOrEmpty(returnString))
                                             AppendToConsole(returnString, ConsoleTextType.InputReturn);
                                     });

            txtConsoleOut.Invoke(e);
        }

        /// <summary>
        /// Worker thread for the server.
        /// </summary>
        void ServerThread()
        {
            // Create the server
            using (_server = new Server())
            {
                _server.ConsoleCommandExecuted += Server_ConsoleCommandExecuted;

                // Start the main loop (the thread will block here until the server is closed)
                _server.Start();
            }

            // Close the form if the server stops
            try
            {
                if (!Disposing && !_formClosing)
                    Invoke(new EventHandler(delegate { Close(); }));
            }
            catch (InvalidOperationException)
            {
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
                lstLog.SuspendLayout();
                lstLog.Enabled = false;
                try
                {
                    // Add the events
                    lstLog.Items.AddRange(events);
                    _logBuffer.AddRange(events);

                    // If too long, truncate
                    while (lstLog.Items.Count > _maxLogDisplayLines)
                    {
                        lstLog.Items.RemoveAt(0);
                    }

                    if (_logBuffer.Count > _logBufferSize)
                        _logBuffer.RemoveRange(0, _logBuffer.Count - _logBufferSize + _logBufferRemoveExtra);

                    // Scroll down to see the latest item if nothing is selected
                    if (lstLog.SelectedIndex < 0)
                    {
                        lstLog.SelectedIndex = lstLog.Items.Count - 1;
                        lstLog.ClearSelected();
                    }
                }
                finally
                {
                    lstLog.Enabled = true;
                    lstLog.ResumeLayout();
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

            _server.EnqueueConsoleCommand(txt);
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