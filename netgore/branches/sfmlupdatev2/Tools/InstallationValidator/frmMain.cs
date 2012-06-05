using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InstallationValidator.Properties;
using InstallationValidator.Tests;
using Microsoft.Win32;
using NetGore.IO;

namespace InstallationValidator
{
    public partial class frmMain : Form
    {
        ITestable[] _tests;

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            SetTestSuiteStatus(TestSuiteStatus.Waiting);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DoubleBuffered = true;

            SetCurrentDirectory();

            // Find the path to mysql.exe
            if (!MySqlHelper.ValidateFilePathsLoaded())
            {
                Dispose();
                return;
            }

            // Create the tests
            _tests = new ITestable[]
            {
                new LocateMySqlExe(), new DatabaseFileExists(), new LoadDbConnectionSettings(), new ConnectToDatabase(),
                new DatabaseExists(), new DatabaseVersion(), new LoadSchemaFile(), new DatabasePopulated()
            };

            // Populate the test list
            lstTests.Items.AddRange(_tests);
        }

        static void SetCurrentDirectory()
        {
            // Make sure the current directory is always the root, whether we run it using the .bat file or
            // through the IDE or whatever
            var currentDir = Directory.GetCurrentDirectory();
            if (currentDir.EndsWith("bin", StringComparison.OrdinalIgnoreCase))
            {
                // Move two directories down
                var parent1 = Directory.GetParent(currentDir);
                var parent2 = Directory.GetParent(parent1.FullName);
                var parent3 = Directory.GetParent(parent2.FullName);

                // Set the new current directory
                Directory.SetCurrentDirectory(parent3.FullName);
            }
        }

        /// <summary>
        /// Sets the test suite status.
        /// </summary>
        /// <param name="currentTest">The current test.</param>
        /// <param name="totalTests">The total tests.</param>
        void SetTestSuiteStatus(int currentTest, int totalTests)
        {
            SetTestSuiteStatus(TestSuiteStatus.Running);
            lblStatus.Text += string.Format(" {0}/{1}", currentTest, totalTests);
            lblStatus.Refresh();
        }

        /// <summary>
        /// Sets the test suite status.
        /// </summary>
        /// <param name="status">The status.</param>
        void SetTestSuiteStatus(TestSuiteStatus status)
        {
            string txt;
            Color color;

            switch (status)
            {
                case TestSuiteStatus.Pass:
                    txt = "All tests passed! NetGore should be set up correctly and ready to use.";
                    color = Color.Green;
                    break;

                case TestSuiteStatus.Failure:
                    txt = "A test has failed! Select the test to view the details.";
                    color = Color.Red;
                    break;

                case TestSuiteStatus.Running:
                    txt = "Running tests...";
                    color = SystemColors.WindowText;
                    break;

                case TestSuiteStatus.Waiting:
                    txt = "Waiting testing to start...";
                    color = SystemColors.WindowText;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("status");
            }

            lblStatus.ForeColor = color;
            lblStatus.Text = txt;
        }

        /// <summary>
        /// Handles the Click event of the btnDbSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDbSettings_Click(object sender, EventArgs e)
        {
            if (!File.Exists(MySqlHelper.DbSettingsFile))
            {
                MessageBox.Show(
                    string.Format(
                        "Error: Could not find the DbSettings.dat file at the expected location:{0}{0}{1}{0}{0}If you have moved this file, please move it back.",
                        Environment.NewLine, MySqlHelper.DbSettingsFile));
                return;
            }

            var ex = FileHelper.TryOpenWithNotepad(MySqlHelper.DbSettingsFile);

            if (ex != null)
                MessageBox.Show(string.Format("Failed to open file {0}.{1}{1}{2}", MySqlHelper.DbSettingsFile, Environment.NewLine,
                    ex));
        }

        /// <summary>
        /// Handles the Click event of the btnRun control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnRun.Refresh();

            try
            {
                var anyFailed = false;

                btnRun.Image = Resources.idle;
                btnRun.Refresh();

                // Clear the status of all tests
                foreach (var test in _tests)
                {
                    test.ClearStatus();
                }

                lstTests.Refresh();

                // Run the tests in order
                for (var i = 0; i < _tests.Length; i++)
                {
                    SetTestSuiteStatus(i + 1, _tests.Length);

                    var currTest = _tests[i];
                    string s;
                    var currTestFailed = !currTest.Test(out s);
                    anyFailed |= currTestFailed;

                    lstTests.SelectedIndex = i;
                    lstTests.Refresh();

                    if (currTestFailed && currTest.IsVital)
                        break;
                }

                SetTestSuiteStatus(anyFailed ? TestSuiteStatus.Failure : TestSuiteStatus.Pass);
                btnRun.Image = anyFailed ? Resources.fail : Resources.pass;
            }
            finally
            {
                btnRun.Enabled = true;
            }

            btnRun.Refresh();
        }

        /// <summary>
        /// Handles the Click event of the btnSetupGuide control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnSetupGuide_Click(object sender, EventArgs e)
        {
            const string url = "http://www.netgore.com/wiki/setup-guide.html";

            try
            {
                Help.ShowHelp(this, url);
            }
            catch (Exception ex)
            {
                Debug.Fail("Failed to open browser via Help.ShowHelp: " + ex);

                // ShowHelp failed, try to do it manually
                try
                {
                    var key = Registry.ClassesRoot.OpenSubKey(@"htmlfile\shell\open\command", false);
                    var browserPath = ((string)key.GetValue(null, null)).Split('"')[1];
                    Process.Start(browserPath, url);
                }
                catch (Exception ex2)
                {
                    Debug.Fail("Failed to open browser: " + ex2);
                    MessageBox.Show(
                        "Failed to automatically open your default web browser, so please instead manually browse to the following url:\n\n" +
                        url);
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstTests control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lstTests.SelectedItem as ITestable;
            if (item == null)
            {
                txtDesc.Text = "Select a test to view the description.";
                txtStatus.Text = "Select a test to view the status.";
            }
            else
            {
                txtDesc.Text = item.Name + ":\n\n" + item.Description;
                switch (item.LastRunStatus)
                {
                    case TestStatus.NotTested:
                        txtStatus.Text = "Status is not available until the test is run.";
                        break;
                    case TestStatus.Passed:
                        txtStatus.Text = "Test successful!";
                        break;
                    default:
                        txtStatus.Text = item.LastRunError;
                        break;
                }
            }

            txtDesc.Text = txtDesc.Text.Replace('\n'.ToString(), Environment.NewLine);
            txtStatus.Text = txtStatus.Text.Replace('\n'.ToString(), Environment.NewLine);
        }

        /// <summary>
        /// The possible test suite statuses.
        /// </summary>
        enum TestSuiteStatus : byte
        {
            /// <summary>
            /// No tests failed.
            /// </summary>
            Pass,

            /// <summary>
            /// One or more tests failed.
            /// </summary>
            Failure,

            /// <summary>
            /// Tests are waiting to be run.
            /// </summary>
            Waiting,

            /// <summary>
            /// Tests are being run.
            /// </summary>
            Running
        }
    }
}