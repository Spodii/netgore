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

        const string _rootNodeName = "DbConnectionSettings";

        /// <summary>
        /// The name and suffix of the settings file.
        /// </summary>
        const string _settingsFileName = "DbSettings.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        /// <param name="filePath">The file path to load the settings from.</param>
        public DbConnectionSettings(string filePath)
        {
            // Copy over the default settings file to the destination settings file
            // This way, project developers don't end up constantly overwriting each other's database settings
            string destSettingsFile = DefaultFilePath;
            if (!File.Exists(destSettingsFile))
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Settings file for local server settings copied to `{0}`.", destSettingsFile);

                File.Copy(filePath, destSettingsFile);
            }

            // Read the values
            var reader = new XmlValueReader(destSettingsFile, _rootNodeName);
            ((IPersistable)this).ReadState(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        public DbConnectionSettings() : this(_settingsFileName)
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
            get { return ContentPaths.Build.Settings.Join(_settingsFileName); }
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
        /// Saves the <see cref="DbConnectionSettings"/> to file.
        /// </summary>
        public void Save()
        {
            using (var writer = new XmlValueWriter(DefaultFilePath, _rootNodeName))
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