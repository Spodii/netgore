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

        public frmMain()
        {
            InitializeComponent();
            SetTestSuiteStatus(TestSuiteStatus.Waiting);
        }

        void btnDbSettings_Click(object sender, EventArgs e)
        {
            if (!File.Exists(MySqlHelper.DbSettingsFile))
            {
                MessageBox.Show(
                    string.Format(
                        "Error: Could not find the DbSettings.xml file at the expected location:{0}{0}{1}{0}{0}If you have moved this file, please move it back.",
                        Environment.NewLine, MySqlHelper.DbSettingsFile));
                return;
            }

            var ex = FileHelper.TryOpenWithNotepad(MySqlHelper.DbSettingsFile);

            if (ex != null)
                MessageBox.Show(string.Format("Failed to open file {0}.{1}{1}{2}", MySqlHelper.DbSettingsFile,
                                              Environment.NewLine, ex));
        }

        void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            btnRun.Refresh();

            try
            {
                bool anyFailed = false;

                btnRun.Image = Resources.idle;
                btnRun.Refresh();

                // Clear the status of all tests
                foreach (var test in _tests)
                {
                    test.ClearStatus();
                }

                lstTests.Refresh();

                // Run the tests in order
                for (int i = 0; i < _tests.Length; i++)
                {
                    SetTestSuiteStatus(i + 1, _tests.Length);

                    string s;
                    anyFailed |= !_tests[i].Test(out s);

                    lstTests.SelectedIndex = i;
                    lstTests.Refresh();

                    if (anyFailed)
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
                    RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"htmlfile\shell\open\command", false);
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

        void frmMain_Load(object sender, EventArgs e)
        {
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
                new LocateMySqlExe(),
                new DatabaseFileExists(), new LoadDbConnectionSettings(), new ConnectToDatabase(), new DatabaseExists(),
                new DatabaseVersion(),
                new LoadSchemaFile(), new DatabasePopulated()
            };

            // Populate the test list
            lstTests.Items.AddRange(_tests);
        }

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

        static void SetCurrentDirectory()
        {
            // Make sure the current directory is always the root, whether we run it using the .bat file or
            // through the IDE or whatever
            var currentDir = Directory.GetCurrentDirectory();
            if (currentDir.EndsWith("bin", StringComparison.OrdinalIgnoreCase))
            {
                // ReSharper disable PossibleNullReferenceException
                // Move two directories down
                var parent1 = Directory.GetParent(currentDir);
                var parent2 = Directory.GetParent(parent1.FullName);

                // Set the new current directory
                Directory.SetCurrentDirectory(parent2.FullName);
                // ReSharper restore PossibleNullReferenceException
            }
        }

        void SetTestSuiteStatus(int currentTest, int totalTests)
        {
            SetTestSuiteStatus(TestSuiteStatus.Running);
            lblStatus.Text += string.Format(" {0}/{1}", currentTest, totalTests);
            lblStatus.Refresh();
        }

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

        enum TestSuiteStatus
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