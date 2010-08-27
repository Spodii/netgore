using System;
using System.Diagnostics;
using System.Linq;

namespace GoreUpdater.Manager
{
    /// <summary>
    /// The different types of <see cref="IFileUploader"/>s.
    /// </summary>
    public enum FileUploaderType
    {
        Ftp
    }

    public class MasterServerInfo
    {
        readonly object _syncRoot = new object();
        IFileUploader _fileUploader;
        FileUploaderType _fileUploaderType;

        string _host;
        string _password;
        string _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterServerInfo"/> class.
        /// </summary>
        /// <param name="host">The host address.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public MasterServerInfo(string host, string user, string password)
        {
            _host = host;
            _user = user;
            _password = password;
        }

        /// <summary>
        /// Gets the type of <see cref="IFileUploader"/> to use.
        /// </summary>
        public FileUploaderType FileUploaderType
        {
            get { return _fileUploaderType; }
        }

        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Gets the password used to use to log into the server.
        /// </summary>
        public string Password
        {
            get { return _password; }
        }

        /// <summary>
        /// Gets the user name to use to log into the server.
        /// </summary>
        public string User
        {
            get { return _user; }
        }

        /// <summary>
        /// Changes the server information.
        /// </summary>
        /// <param name="newHost">The new host.</param>
        /// <param name="newUser">The new user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="newType">The new type of <see cref="IFileUploader"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="newHost"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newUser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newPassword"/> is null.</exception>
        public void ChangeInfo(string newHost, string newUser, string newPassword, FileUploaderType newType)
        {
            if (newHost == null)
                throw new ArgumentNullException("newHost");
            if (newUser == null)
                throw new ArgumentNullException("newUser");
            if (newPassword == null)
                throw new ArgumentNullException("newPassword");

            lock (_syncRoot)
            {
                // Check if any values are different
                var sc = StringComparer.Ordinal;

                if (sc.Equals(newHost, Host) && sc.Equals(newUser, User) && sc.Equals(newPassword, Password) &&
                    newType == FileUploaderType)
                    return;

                // Set the new values
                _host = newHost;
                _user = newUser;
                _password = newPassword;
                _fileUploaderType = newType;

                // Recreate the uploader
                RecreateFileUploader();
            }
        }

        /// <summary>
        /// Recreates the <see cref="IFileUploader"/> and starts updating again.
        /// </summary>
        void RecreateFileUploader()
        {
            // Dispose of the old uploader
            if (_fileUploader != null)
                _fileUploader.Dispose();

            // Create the new uploader
            switch (FileUploaderType)
            {
                case FileUploaderType.Ftp:
                    _fileUploader = new FtpFileUploader(Host, User, Password);
                    break;

                default:
                    const string errmsg = "FileUploaderType `{0}` not supported.";
                    Debug.Fail(string.Format(errmsg, FileUploaderType));
                    throw new NotSupportedException(string.Format(errmsg, FileUploaderType));
            }

            // TODO: Start checking if the server is up-to-date
        }
    }
}