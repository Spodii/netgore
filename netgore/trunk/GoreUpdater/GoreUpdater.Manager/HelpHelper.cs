using System;
using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// Helper method for displaying the individual help messages on the GUI.
    /// </summary>
    public static class HelpHelper
    {
        static readonly HelpMessage _accountPassword;
        static readonly HelpMessage _accountUser;
        static readonly HelpMessage _changeLiveVersion;
        static readonly HelpMessage _createNewVersion;
        static readonly HelpMessage _deleteFilters;
        static readonly HelpMessage _host;
        static readonly HelpMessage _liveVersion;
        static readonly HelpMessage _newVersionNumber;
        static readonly HelpMessage _rootPath;
        static readonly HelpMessage _serverType;

        /// <summary>
        /// Initializes the <see cref="HelpHelper"/> class.
        /// </summary>
        static HelpHelper()
        {
            _serverType = new HelpMessage("Server type",
                                          "The method to use to communicate with the server. This is the method used by the update manager only," +
                                          " and does not affect the method used by the updater clients (the end user).");

            _accountUser = new HelpMessage("Account user", "The user account to use to connect to the server.");

            _accountPassword = new HelpMessage("Account password", "The password for the given user account.");

            _host = new HelpMessage("Host",
                                    "The fully qualified address of the server, including the protocol and the sub-directory to use." +
                                    " For example, for ftp:" + Environment.NewLine + "   ftp://www.mydomain.com/subdirectory/");

            _createNewVersion = new HelpMessage("Create new version",
                                                "Creates the next version, which by default is always the version immediately after the live version." +
                                                " If the next version already exists, this will overwrite it instead of creating another new version." +
                                                " Once the new version is created, the servers will automatically start to synchronize with the new contents so that" +
                                                " when you update the new version to the live version, all the files will be ready for download.");

            _liveVersion = new HelpMessage("Live version",
                                           "The live version is the current version that clients are updated to. It is usually either the latest version," +
                                           " or the version immediately before the latest version.");

            _changeLiveVersion = new HelpMessage("Change live version",
                                                 "This will set the live version to the next version, as long as the next version is available." +
                                                 " It is highly recommended you wait on updating the live version until all of your servers have been" +
                                                 " synchronized. That way, you can be sure people have access to the files that they need when they go to update.");

            _newVersionNumber = new HelpMessage("Version number", "The version number that will be assigned to this new version.");

            _rootPath = new HelpMessage("Root path",
                                        "The root path to the directory containing the files to be used for this version." +
                                        " This directory must contain all files that are to be in the version.");

            _deleteFilters = new HelpMessage("Delete filters",
                                             "The filters that will be used to determine what files to NOT delete when" +
                                             " deleting the left-over files after updating to the latest version. Any file that is not in the list of files for the version" +
                                             " and does not match ANY of the filters will be deleted after updating." +
                                             Environment.NewLine + Environment.NewLine +
                                             " * Default filter: Uses the * and ? wildcards, where * matches 0 or more characters and ? matches 0 or 1 characters." +
                                             Environment.NewLine +
                                             " * Regex filter: Uses .NET's Regex class to match file path strings. Use by starting the line with a single forward slash (/)." +
                                             Environment.NewLine + Environment.NewLine + "Other important notes:" +
                                             Environment.NewLine +
                                             " * All paths are relative to the directory they are being downloaded to" +
                                             Environment.NewLine + " * All paths start with the path separator character" +
                                             Environment.NewLine + " * All filters use a case-insensitive matching" +
                                             Environment.NewLine +
                                             " * Back slashes (\\) should always be used to denote a path separator, never a forward slash");
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the account password.
        /// </summary>
        public static HelpMessage HelpAccountPassword
        {
            get { return _accountPassword; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the account user.
        /// </summary>
        public static HelpMessage HelpAccountUser
        {
            get { return _accountUser; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the changing the live version.
        /// </summary>
        public static HelpMessage HelpChangeLiveVersion
        {
            get { return _changeLiveVersion; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for creating a new version.
        /// </summary>
        public static HelpMessage HelpCreateNewVersion
        {
            get { return _createNewVersion; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the file delete filters.
        /// </summary>
        public static HelpMessage HelpDeleteFilters
        {
            get { return _deleteFilters; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the host.
        /// </summary>
        public static HelpMessage HelpHost
        {
            get { return _host; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the live version.
        /// </summary>
        public static HelpMessage HelpLiveVersion
        {
            get { return _liveVersion; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the new version number.
        /// </summary>
        public static HelpMessage HelpNewVersionNumber
        {
            get { return _newVersionNumber; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the root path.
        /// </summary>
        public static HelpMessage HelpRootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        /// Gets the <see cref="HelpMessage"/> for the server type.
        /// </summary>
        public static HelpMessage HelpServerType
        {
            get { return _serverType; }
        }

        /// <summary>
        /// Displays a help message.
        /// </summary>
        /// <param name="title">The title of the message box.</param>
        /// <param name="message">The message to display.</param>
        public static void DisplayHelp(string title, string message)
        {
            MessageBox.Show(message, "Help: " + title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays a help message.
        /// </summary>
        /// <param name="msg">The help message to display.</param>
        public static void DisplayHelp(HelpMessage msg)
        {
            DisplayHelp(msg.Title, msg.Message);
        }
    }
}