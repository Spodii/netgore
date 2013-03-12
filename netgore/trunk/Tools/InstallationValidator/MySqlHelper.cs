using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NetGore;
using NetGore.Db;
using NetGore.IO;

namespace InstallationValidator
{
    /// <summary>
    /// Helper methods for MySQL.
    /// </summary>
    public static class MySqlHelper
    {
        /// <summary>
        /// The name of the database dump file.
        /// </summary>
        public const string DbSqlFile = "db.sql";

        /// <summary>
        /// The relative path to the database schema file.
        /// </summary>
        public static readonly string DbSchemaFile = string.Format("Tools{0}InstallationValidator{0}dbschema{1}",
            Path.DirectorySeparatorChar, EngineSettings.DataFileSuffix);

        /// <summary>
        /// The relative path to the database settings file.
        /// </summary>
        public static readonly string DbSettingsFile = string.Format("DemoGame.Server{0}DbSettings{1}",
            Path.DirectorySeparatorChar, EngineSettings.DataFileSuffix);

        /// <summary>
        /// The database connection settings.
        /// </summary>
        public static DbConnectionSettings ConnectionSettings;

        /// <summary>
        /// The path to the MySql command-line binary (mysql.exe).
        /// </summary>
        public static string MySqlPath = FindMySqlFile("mysql.exe");

        /// <summary>
        /// Allow the user to create the database and import the values.
        /// </summary>
        public static void AskToImportDatabase(bool dbExists)
        {
            var sb = new StringBuilder();

            if (!dbExists)
                sb.AppendFormat("Database `{0}` does not exist. Do you wish to create it (Y/N)?", ConnectionSettings.Database);
            else
                sb.AppendFormat("Database `{0}` is not populated. Do you wish to populate it (Y/N)?", ConnectionSettings.Database);

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("Additional information:");
            sb.AppendLine("1. You MUST create the database and import the data at some point.");
            sb.AppendLine("2. You can always re-run this program and create the database later.");
            sb.AppendLine("3. Expert users might want to create the database manually.");
            sb.AppendLine("4. Since the database is empty, no data will be lost, so 'Y' is recommended.");

            if (MessageBox.Show(sb.ToString(), "Import database?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ImportDatabaseContents();
        }

        /// <summary>
        /// Finds the path to a MySql file.
        /// </summary>
        /// <param name="fileName">The MySql file to search for.</param>
        /// <returns>The path to the MySql file.</returns>
        public static string FindMySqlFile(string fileName)
        {
            var regPath = TryGetMySqlPathFromRegistry();
            var path1 = Environment.GetEnvironmentVariable("ProgramFiles") + Path.DirectorySeparatorChar + "MySql";
            var path2 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + Path.DirectorySeparatorChar + "MySql";

            if (path1 == path2)
                path1 = path2.Replace(" (x86)", string.Empty);

            var filePath = FileFinder.Find(fileName, regPath) ??
                           FileFinder.Find(fileName, path1) ?? FileFinder.Find(fileName, path2);

            return filePath;
        }

        /// <summary>
        /// Gets a formatted error string from the MySql command line.
        /// </summary>
        /// <param name="mysqlOut">The output string.</param>
        /// <param name="mysqlError">The error string.</param>
        /// <param name="errmsg">The error.</param>
        /// <param name="p">The parameters.</param>
        /// <returns>The formatted error string.</returns>
        static string GetMySqlCommandErrorStr(string mysqlOut, string mysqlError, string errmsg, params string[] p)
        {
            const string div1 = "=====";
            const string div2 = "===";

            string err;
            if (p != null && p.Length > 0)
                err = string.Format(errmsg, p);
            else
                err = errmsg;

            var errDetails = string.Empty;

            if (mysqlOut != null)
            {
                mysqlOut = mysqlOut.Trim();
                if (mysqlOut.Length > 0)
                    errDetails += string.Format("{0}{1} mysql.exe output {1}{0}{2}", "\n", div2, mysqlOut);
            }

            if (mysqlError != null)
            {
                mysqlError = mysqlError.Trim();
                if (mysqlError.Length > 0)
                    errDetails += string.Format("{0}{1} mysql.exe error output {1}{0}{2}", "\n", div2, mysqlError);
            }

            if (errDetails.Length > 0)
            {
                err += string.Format("{0}{0}{1} ERROR DETAILS {1}{0}", "\n", div1);
                err += errDetails;
            }

            return err + "\n";
        }

        /// <summary>
        /// Imports the database contents into the database.
        /// </summary>
        static void ImportDatabaseContents()
        {
            var dbFile = Path.GetFullPath(DbSqlFile);

            const string title = "Failed to find db.sql";
            const string msg =
                "Failed to find the db.sql file. Make sure you didn't move/delete it, or run this program from a different directory than the default.";

            while (!File.Exists(dbFile))
            {
                if (MessageBox.Show(msg, title, MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                    return;
            }

            var commands = new string[]
            {
                "DROP DATABASE IF EXISTS " + ConnectionSettings.Database + ";",
                "CREATE DATABASE " + ConnectionSettings.Database + ";", "USE " + ConnectionSettings.Database + ";",
                "SOURCE " + dbFile, "exit"
            };

            string errmsg;
            TestMySqlCommand(null, commands, out errmsg);
        }

        /// <summary>
        /// Runs the mysql.exe process.
        /// </summary>
        /// <param name="command">The parameters to use when running the mysql.exe process.</param>
        /// <param name="output">A string of the text mysql.exe sent to the Standard Out stream.</param>
        /// <param name="error">A string of the text mysql.exe sent ot the Standard Error stream.</param>
        /// <param name="cmds">The additional commands to input when running the process.</param>
        public static void MySqlCommand(string command, out string output, out string error, params string[] cmds)
        {
            using (var tmpFile = new TempFile())
            {
                // Build the default command string
                if (string.IsNullOrEmpty(command))
                {
                    // Since v5.6, MySQL started being a little bitch about passing the password via the command line. So we instead set the credentials
                    // in a temporary file and pass it that instead.
                    var username = ConnectionSettings.User;
                    var host = ConnectionSettings.Host;
                    var password = ConnectionSettings.Pass;

                    File.WriteAllText(tmpFile.FilePath, string.Format(@"
[client]
user={0}
password={1}
host={2}
", username, password, host));

                    command = string.Format("--defaults-extra-file=\"{0}\"", tmpFile.FilePath);
                }

                var psi = new ProcessStartInfo(MySqlPath, command)
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var p = new Process { StartInfo = psi };
                p.Start();

                if (cmds != null)
                {
                    for (var i = 0; i < cmds.Length; i++)
                    {
                        p.StandardInput.WriteLine(cmds[i]);
                    }
                }
                p.WaitForExit();

                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
            }
        }

        /// <summary>
        /// Provides a wrapper for a test that does a test on the MySql.exe process. This will call Test()
        /// and perform internal checks for known errors. All expected errors should be defined in here.
        /// </summary>
        /// <param name="command">The primary command (command line command). Leave null or empty to use
        /// the default connection string.</param>
        /// <param name="cmds">The additional commands to execute.</param>
        /// <param name="retStr">When this method returns false, contains the error string. When this method returns true,
        /// contains an empty string.</param>
        /// <returns>
        /// True if the test was successful; false if the test failed.
        /// </returns>
        public static bool TestMySqlCommand(string command, string[] cmds, out string retStr)
        {
            // Check for a valid application path
            if (string.IsNullOrEmpty(MySqlPath) || !File.Exists(MySqlPath))
            {
                retStr = "Unknown or invalid path to mysql.exe. Cannot run command.";
                if (!string.IsNullOrEmpty(MySqlPath))
                    retStr += "\nSupplied path: " + MySqlPath;
                return false;
            }

            // Run the mysql.exe process
            string output;
            string error;

            try
            {
                MySqlCommand(command, out output, out error, cmds);
            }
            catch (Exception ex)
            {
                // Display any exceptions found from trying to run the process
                // Generally this means that the process itself crashed or completely failed to run
                retStr = string.Format("Failed to launch mysql.exe. Internal error: {0}", ex);
                return false;
            }

            // Check for a variety of failure messages from the mysql.exe process
            if (error == null)
                error = string.Empty;
            else
                error = error.Trim();

            if (error.Length > 0)
            {
                var errorLower = error.ToLower();
                if (errorLower.Contains("access denied"))
                {
                    const string failInfo = "Access denied. Ensure the connection string contains valid account info: `{0}`";
                    retStr = GetMySqlCommandErrorStr(output, error, failInfo, command);
                    return false;
                }
                else if (errorLower.Contains("error 2005"))
                {
                    const string failInfo =
                        "Unknown service host `{0}`. Make sure the host used in the connection string is correct" +
                        " and that the MySql server service has been started: `{0}`";
                    retStr = GetMySqlCommandErrorStr(output, error, failInfo, command);
                    return false;
                }
                else if (errorLower.Contains("error 1146"))
                {
                    const string failInfo =
                        "The target database is not populated. Make sure you import the db.sql file." +
                        " See http://netgore.com/wiki/setup-guide.html for more information.";
                    retStr = GetMySqlCommandErrorStr(output, error, failInfo);
                    return false;
                }
                else if (errorLower.Contains("error 1049"))
                {
                    const string failInfo =
                        "Specified database does not exist. Make sure you create the MySql database" +
                        " and import the db.sql file. If you use a database name other than" +
                        " `demogame`, make sure you updated the database settings file located at `{0}`." +
                        " See http://netgore.com/wiki/setup-guide.html for more information.";
                    retStr = GetMySqlCommandErrorStr(output, error, failInfo, DbSettingsFile);
                    return false;
                }
                else if (errorLower.Contains("failed to open file"))
                {
                    const string failInfo =
                        "Failed to open the db.sql file. Please make sure it is in the default location." +
                        " If you continue to get this message, you may have to manually import the database's contents";
                    retStr = GetMySqlCommandErrorStr(output, error, failInfo, DbSettingsFile);
                    return false;
                }
                else
                {
                    retStr = error;
                    return false;
                }
            }

            retStr = string.Empty;
            return true;
        }

        static string TryGetMySqlPathFromRegistry()
        {
            try
            {
                var software = Registry.LocalMachine.OpenSubKey("Software");
                if (software == null)
                    return null;

                var mysqlab = software.OpenSubKey("MYSQL AB");
                if (mysqlab == null)
                    return null;

                var mysql51 = mysqlab.OpenSubKey("MySql Server 5.1");
                if (mysql51 == null)
                    return null;

                return mysql51.GetValue("Location").ToString();
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Checks if that the <see cref="MySqlPath"/> is set and valid. If not valid, opens up a prompt to allow the user
        /// to select the correct path.
        /// </summary>
        /// <returns>True if valid; otherwise false.</returns>
        public static bool ValidateFilePathsLoaded()
        {
            if (string.IsNullOrEmpty(MySqlPath) || !File.Exists(MySqlPath))
            {
                // Create the error message
                var sb = new StringBuilder();

                sb.AppendLine("Failed to automatically find the path to mysql.exe.");
                sb.AppendLine(
                    "After clicking \"OK\", please locate mysql.exe, which should be located in your MySQL installation directory under the sub-directory \\bin\\.");
                sb.AppendLine(@"Example: C:\Program Files\MySQL\MySQL Server 5.1\bin\mysql.exe");

                MessageBox.Show(sb.ToString(), "Failed to find mysql.exe", MessageBoxButtons.OK);

                // Open a prompt to allow the user to select the correct path
                while (true)
                {
                    using (var fs = new OpenFileDialog())
                    {
                        fs.Multiselect = false;
                        fs.Filter = "mysql.exe|*.exe";
                        fs.CheckFileExists = true;
                        fs.CheckPathExists = true;
                        fs.AutoUpgradeEnabled = true;
                        fs.AddExtension = true;
                        fs.ValidateNames = true;
                        if (fs.ShowDialog() != DialogResult.OK)
                            return false;

                        MySqlPath = fs.FileName;
                    }

                    if (MySqlPath != null && MySqlPath.Length > 2 && File.Exists(MySqlPath))
                        break;

                    if (
                        MessageBox.Show("The selected file was invalid. Please select mysql.exe.", "Select mysql.exe",
                            MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return false;
                }
            }

            if (string.IsNullOrEmpty(MySqlPath) || !File.Exists(MySqlPath))
                return false;

            return true;
        }
    }
}