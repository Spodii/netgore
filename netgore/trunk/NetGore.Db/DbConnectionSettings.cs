using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Db
{
    /// <summary>
    /// Settings for the database connection.
    /// </summary>
    public class DbConnectionSettings : IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default name of the DbSettings file (no suffix).
        /// </summary>
        const string _dbSettingsFileName = "DbSettings";

        const string _rootNodeName = "DbConnectionSettings";

        readonly string _filePath;

        /// <summary>
        /// Initializes the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        static DbConnectionSettings()
        {
            EncodingFormat = GenericValueIOFormat.Xml;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        /// <param name="fileName">The name of the settings file.</param>
        /// <param name="forceDefaultSettings">If true, the settings file in the settings directory
        /// will be used if they exists. If they don't, they will be copied over from the default
        /// settings path. If false, the default settings will always be used, and will never be copied
        /// over to the settings directory, forcing the default settings to be used.</param>
        public DbConnectionSettings(string fileName, bool forceDefaultSettings)
        {
            string destSettingsFile;

            if (forceDefaultSettings)
            {
                destSettingsFile = Path.GetFullPath(fileName);
                _filePath = destSettingsFile;
            }
            else
            {
                // Copy over the default settings file to the destination settings file
                // This way, project developers don't end up constantly overwriting each other's database settings
                destSettingsFile = DefaultFilePath;
                _filePath = destSettingsFile;

                if (!File.Exists(destSettingsFile))
                {
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Settings file for local server settings copied to `{0}`.", destSettingsFile);

                    var copyPath = Path.GetFullPath(fileName);
                    File.Copy(copyPath, destSettingsFile);
                    _filePath = copyPath;
                }
            }

            // Read the values
            var reader = GenericValueReader.CreateFromFile(destSettingsFile, _rootNodeName);
            ((IPersistable)this).ReadState(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        public DbConnectionSettings() : this(_dbSettingsFileName + EngineSettings.DataFileSuffix, false)
        {
        }

        /// <summary>
        /// Gets or sets the Sql database name containing the game information.
        /// </summary>
        [SyncValue]
        public string Database { get; set; }

        /// <summary>
        /// Gets the default file path for the <see cref="DbConnectionSettings"/>.
        /// </summary>
        public static string DefaultFilePath
        {
            get { return ContentPaths.Build.Settings.Join(_dbSettingsFileName + EngineSettings.DataFileSuffix); }
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is <see cref="GenericValueIOFormat.Xml"/>.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets the path to the file that these settings were loaded from and will be saved to by default.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets or sets the Sql connection host address.
        /// </summary>
        [SyncValue]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the Sql connection password.
        /// </summary>
        [SyncValue]
        public string Pass { get; set; }

        /// <summary>
        /// Gets or sets the Sql server port.
        /// </summary>
        [SyncValue]
        public uint Port { get; set; }

        /// <summary>
        /// Gets or sets the Sql user name.
        /// </summary>
        [SyncValue]
        public string User { get; set; }

        /// <summary>
        /// Opens the settings file for editing.
        /// </summary>
        /// <returns>True if the file was successfully opened; otherwise false.</returns>
        public bool OpenFileForEdit()
        {
            // Try to open the file to edit
            var ex = FileHelper.TryOpenWithNotepad(FilePath);

            // If the file could not be opened, then we obviously can't edit it, can we?
            if (ex != null)
            {
                const string errmsg = "Failed to open DbConnectionSettings file to edit. File path: {0}. Exception: {1}";
                var msg = string.Format(errmsg, FilePath, ex);
                log.Fatal(msg);
                Debug.Fail(msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reloads the settings from the file.
        /// </summary>
        public void Reload()
        {
            var reader = GenericValueReader.CreateFromFile(FilePath, _rootNodeName);
            ((IPersistable)this).ReadState(reader);
        }

        /// <summary>
        /// Saves the <see cref="DbConnectionSettings"/> to file.
        /// </summary>
        public void Save()
        {
            using (var writer = GenericValueWriter.Create(FilePath, _rootNodeName, EncodingFormat))
            {
                ((IPersistable)this).WriteState(writer);
            }
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        void IPersistable.ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        void IPersistable.WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}