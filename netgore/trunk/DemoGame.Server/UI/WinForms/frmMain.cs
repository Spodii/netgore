using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server.UI
{
    public partial class frmMain : Form
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How many extra items are removed from the log buffer when removing. Allows for less removals to be made.
        /// </summary>
        const int _logBufferRemoveExtra = 64;

        /// <summary>
        /// The maximum number of log events to keep in memory.
        /// </summary>
        const int _logBufferSize = _maxLogDisplayLines * 2 + _logBufferRemoveExtra;

        /// <summary>
        /// The maximum number of log entries to display.
        /// </summary>
        const int _maxLogDisplayLines = 128;

        readonly object _consoleOutLock = new object();
        readonly List<LoggingEvent> _logBuffer = new List<LoggingEvent>(_logBufferSize);
        readonly MemoryAppender _logger;
        readonly Thread _serverThread;

        Regex _filterRegex;

        /// <summary>
        /// Incrementing counter that keeps track of the number of times <see cref="tmrUpdateDisplay_Tick"/> has been called.
        /// </summary>
        int _interfaceUpdateTicker;

        Server _server;
        bool _serverDisposed = false;
        bool _shutdownRequested = false;

        /// <summary>
        /// The cpu performance counter.
        /// </summary>
        readonly PerformanceCounter _cpuCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            var cwh = ConsoleWindowHider.TryCreate();
            if (cwh != null)
                cwh.Hide();

            _logger = new MemoryAppender();
            BasicConfigurator.Configure(_logger);

            InitializeComponent();

            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create Processor performance counter: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                _cpuCounter = null;
            }

            _serverThread = new Thread(ServerThread) { Name = "Server Thread", IsBackground = true };
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

            var t = e.TimeStamp.TimeOfDay;
            txtLogTime.Text = string.Format("{0}:{1}:{2}", t.Hours, t.Minutes, t.Seconds);
            txtLogClass.Text = e.LocationInformation.ClassName;
            txtLogMethod.Text = e.LocationInformation.MethodName;
            txtLogLine.Text = e.LocationInformation.LineNumber;
            txtLogMsg.Text = e.RenderedMessage;
            txtLogLevel.Text = e.Level.DisplayName;
            txtLogLevel.ForeColor = e.Level.GetSystemColor();

            tabControl1.SelectTab(tbLogItem);
        }

        /// <summary>
        /// Filters only the enabled log events.
        /// </summary>
        /// <param name="events">The unfiltered log events.</param>
        /// <returns>The enabled log events.</returns>
        LoggingEvent[] GetFilteredEvents(IEnumerable<LoggingEvent> events)
        {
            var ret = new List<LoggingEvent>();

            foreach (var e in events)
            {
                if (e == null)
                    continue;

                if (!GetLogCheckBox(e.Level).Checked)
                    continue;

                if (_filterRegex != null && !_filterRegex.IsMatch(e.RenderedMessage))
                    continue;

                ret.Add(e);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Gets the <see cref="CheckBox"/> for a log <see cref="Level"/>.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <returns>The <see cref="CheckBox"/> for a log <see cref="Level"/>, or null if invalid.</returns>
        CheckBox GetLogCheckBox(Level level)
        {
            if (level == Level.Debug)
                return chkDebug;

            if (level == Level.Info)
                return chkInfo;

            if (level == Level.Warn)
                return chkWarn;

            if (level == Level.Error)
                return chkError;

            if (level == Level.Fatal)
                return chkFatal;

            Debug.Fail(string.Format("No CheckBox available for log level `{0}`.", level));

            return null;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the log CheckBoxes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void LogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RebuildLogList();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _shutdownRequested = true;

            // Wait for the server to be created first, otherwise we might not close it properly. It has likely already
            // started to be created, but just not assigned to the variable yet (constructor is still running).
            if (_server == null)
            {
                e.Cancel = true;
                base.OnFormClosing(e);
                return;
            }

            // Don't actually close if _serverDisposed is not set. This will happen in the _serverThread after the server
            // loop has terminated and the server has been fully shut down
            if (!_serverDisposed)
            {
                e.Cancel = true;

                // But if the server is running, we do still need to start the shutdown
                if (_server.IsRunning)
                {
                    txtConsoleOut.Refresh();
                    _server.Shutdown();
                }
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Show();
            Update();

            UpdateExternalIP();

            _serverThread.Start();

            GetLogCheckBox(Level.Debug).ForeColor = Level.Debug.GetSystemColor();
            GetLogCheckBox(Level.Info).ForeColor = Level.Info.GetSystemColor();
            GetLogCheckBox(Level.Warn).ForeColor = Level.Warn.GetSystemColor();
            GetLogCheckBox(Level.Error).ForeColor = Level.Error.GetSystemColor();
            GetLogCheckBox(Level.Fatal).ForeColor = Level.Fatal.GetSystemColor();

            AppendToConsole("Server started. Type 'help' for a list of server console commands.", ConsoleTextType.Info);
        }

        /// <summary>
        /// Fully rebuilds the log list.
        /// </summary>
        void RebuildLogList()
        {
            var events = GetFilteredEvents(_logBuffer);

            lstLog.SuspendLayout();
            lstLog.Enabled = false;
            try
            {
                lstLog.Items.Clear();
                lstLog.Items.AddRange(events.Cast<object>().ToArray());

                // If too long, truncate
                while (lstLog.Items.Count > _maxLogDisplayLines)
                {
                    lstLog.Items.RemoveAt(0);
                }

                lstLog.SelectedIndex = lstLog.Items.Count - 1;
                lstLog.ClearSelected();
            }
            finally
            {
                lstLog.Enabled = true;
                lstLog.ResumeLayout();
            }
        }

        /// <summary>
        /// Worker thread for the server.
        /// </summary>
        void ServerThread()
        {
            // Create the server
            _server = new Server();
            _server.ConsoleCommandExecuted += Server_ConsoleCommandExecuted;

            // Check if a shutdown request was made before the server even got a chance to finish being constructed
            if (_shutdownRequested)
                _server.Shutdown();

            // Start the main loop (the thread will block here until the server is closed)
            _server.Start();

            // No longer blocking, so the server loop has terminated
            _serverDisposed = true;

            // Close the form when the server stops
            try
            {
                // Do not call close if we are already closing
                if (!Disposing && !IsDisposed)
                    Invoke(new EventHandler(delegate { Close(); }));
            }
            catch (InvalidOperationException ex)
            {
                const string errmsg = "Error occured while trying to shut down the server. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }
        }

        /// <summary>
        /// Handles when a console command was executed on the server.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServerConsoleCommandEventArgs"/> instance containing the event data.</param>
        void Server_ConsoleCommandExecuted(Server sender, ServerConsoleCommandEventArgs e)
        {
            var eh = new EventHandler(delegate
            {
                AppendToConsole(e.Command, ConsoleTextType.Input);

                if (!string.IsNullOrEmpty(e.ReturnString))
                    AppendToConsole(e.ReturnString, ConsoleTextType.InputReturn);
                txtConsoleOut.ScrollToCaret();
            });

            txtConsoleOut.Invoke(eh);
        }

        /// <summary>
        /// Creates a background thread to find and update the external IP address text.
        /// </summary>
        void UpdateExternalIP()
        {
            var externalIP = string.Empty;
            var hostName = string.Empty;

            var w = new BackgroundWorker();

            // Create the work method
            w.DoWork += delegate
            {
                hostName = Dns.GetHostName();
                externalIP = IPAddressHelper.GetExternalIP();
                if (string.IsNullOrEmpty(externalIP))
                    externalIP = "[Failed to get external IP]";
            };

            // Create the updater
            w.RunWorkerCompleted += delegate
            {
                lblIP.Invoke((Action)delegate { lblIP.Text = string.Format("{0} ({1})", externalIP, hostName); });
                w.Dispose();
            };

            // Run the worker
            w.RunWorkerAsync();
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
        /// Handles the Tick event of the tmrUpdateDisplay control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="eArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void tmrUpdateDisplay_Tick(object sender, EventArgs eArgs)
        {
            ++_interfaceUpdateTicker;

            // Get the latest events
            LoggingEvent[] events;
            try
            {
                events = _logger.GetEvents();
            }
            catch (ArgumentException)
            {
                // There is some bug in the _logger.GetEvents() that can throw this exception...
                return;
            }

            _logger.Clear();

            // Ensure there are events
            if (events != null && events.Length > 0)
            {
                _logBuffer.AddRange(events);

                if (_logBuffer.Count > _logBufferSize)
                    _logBuffer.RemoveRange(0, _logBuffer.Count - _logBufferSize + _logBufferRemoveExtra);

                events = GetFilteredEvents(events);

                if (events != null && events.Length > 0)
                {
                    lstLog.SuspendLayout();
                    lstLog.Enabled = false;
                    try
                    {
                        // Add the events
                        lstLog.Items.AddRange(events.Cast<object>().ToArray());

                        // If too long, truncate
                        while (lstLog.Items.Count > _maxLogDisplayLines)
                        {
                            lstLog.Items.RemoveAt(0);
                        }

                        // Scroll down to see the latest item if nothing is selected
                        if (lstLog.SelectedIndex < 0)
                        {
                            lstLog.SelectedIndex = lstLog.Items.Count - 1;
                            lstLog.ClearSelected();

                            // Auto-scroll
                            var numItems = lstLog.ClientSize.Height / lstLog.ItemHeight;
                            lstLog.TopIndex = Math.Max(0, lstLog.Items.Count - numItems + 1);
                        }
                        else
                            lstLog.TopIndex = lstLog.SelectedIndex;
                    }
                    finally
                    {
                        lstLog.Enabled = true;
                        lstLog.ResumeLayout();
                    }
                }
            }

            // Update the CPU and memory usage values
            if (_interfaceUpdateTicker % 8 == 0)
            {
                // Bandwidth
                if (_server != null && _server.ServerSockets != null)
                {
                    try
                    {
                        var totalBytes = _server.ServerSockets.Statistics.ReceivedBytes + _server.ServerSockets.Statistics.SentBytes;
                        var totalMegabytes = totalBytes / 1000f / 1000f;
                        lblBandwidth.Text = string.Format("{0:0.##} MB", totalMegabytes);
                    }
                    catch
                    {
                        lblBandwidth.Text = "ERROR";
                    }
                }
                else
                {
                    lblBandwidth.Text = "";
                }

                // User count
                if (_server != null)
                {
                    try
                    {
                        lblUserCount.Text = _server.World.GetUsers().Count().ToString();
                    }
                    catch
                    {
                        lblUserCount.Text = "ERROR";
                    }
                }
                else
                {
                    lblUserCount.Text = "0";
                }

                // CPU
                if (_cpuCounter != null)
                {
                    try
                    {
                        lblCPU.Text = Math.Round(_cpuCounter.NextValue()) + "%";
                    }
                    catch
                    {
                        lblCPU.Text = "ERROR";
                    }
                }
                else
                {
                    lblCPU.Text = "?";
                }

                // Memory used
                try
                {
                    lblRAMUsed.Text = SystemPerformance.Memory.ProcessUsageMB + " MB";
                }
                catch
                {
                    lblRAMUsed.Text = "ERROR";
                }

                // Memory free
                try
                {
                    lblRAMFree.Text = SystemPerformance.Memory.AvailableMB + " MB";
                }
                catch
                {
                    lblRAMFree.Text = "ERROR";
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the txtConsoleIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void txtConsoleIn_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = txtConsoleIn.Text;

            if (e.KeyCode != Keys.Return || string.IsNullOrEmpty(txt))
                return;

            txtConsoleIn.Text = string.Empty;

            _server.EnqueueConsoleCommand(txt);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtFilterRegex control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtFilterRegex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _filterRegex = new Regex(txtFilterRegex.Text);
                txtFilterRegex.BackColor = SystemColors.Window;
                RebuildLogList();
            }
            catch (ArgumentException)
            {
                _filterRegex = null;
                txtFilterRegex.BackColor = Color.Red;
            }
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