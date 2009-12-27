using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Settings for the database connection.
    /// </summary>
    public class DbConnectionSettings
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The name and suffix of the settings file.
        /// </summary>
        const string _settingsFileName = "DbSettings.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionSettings"/> class.
        /// </summary>
        public DbConnectionSettings()
        {
            // Copy over the default settings file to the destination settings file
            // This way, project developers don't end up constantly overwriting each other's database settings
            string destSettingsFile = ContentPaths.Build.Settings.Join(_settingsFileName);
            if (!File.Exists(destSettingsFile))
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Settings file for local server settings copied to `{0}`.", destSettingsFile);
                File.Copy(_settingsFileName, destSettingsFile);
            }

            // Read the values
            var reader = new XmlValueReader(destSettingsFile, _rootNodeName);
            Read(reader);
        }

        public void Write(IValueWriter writer)
        {
            writer.Write(_databaseValueKey, Database);
            writer.Write(_hostValueKey, Host);
            writer.Write(_passValueKey, Pass);
            writer.Write(_portValueKey, Port);
            writer.Write(_userValueKey, User);
        }

        public static string GetDefaultFilePath(ContentPaths contentPath)
        {
            return contentPath.Settings.Join(_settingsFileName);
        }

        const string _rootNodeName = "DbConnectionSettings";
        const string _databaseValueKey = "Database";
        const string _hostValueKey = "Host";
        const string _passValueKey = "Pass";
        const string _portValueKey = "Port";
        const string _userValueKey = "User";

        public void Save()
        {
            using (var writer = new XmlValueWriter(GetDefaultFilePath(ContentPaths.Build), _rootNodeName))
            {
                Write(writer);
            }
        }

        void Read(IValueReader reader)
        {
            Database = reader.ReadString(_databaseValueKey);
            Host = reader.ReadString(_hostValueKey);
            Pass = reader.ReadString(_passValueKey);
            Port = reader.ReadUInt(_portValueKey);
            User = reader.ReadString(_userValueKey);
        }

        /// <summary>
        /// Gets the Sql database name containing the game information.
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// Gets the Sql connection host address.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Gets the Sql connection password.
        /// </summary>
        public string Pass { get; private set; }

        /// <summary>
        /// Gets the Sql Host Port number, convert to UInt
        /// </summary>
        public uint Port { get; private set; }

        /// <summary>
        /// Gets the Sql user name.
        /// </summary>
        public string User { get; private set; }

        /// <summary>
        /// Makes a Sql connection string using the given settings.
        /// </summary>
        /// <returns>Sql connection string.</returns>
        public string SqlConnectionString()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder
            {
                Database = Database,
                UserID = User,
                Password = Pass,
                Server = Host,
                Port = Port,
                IgnorePrepare = false
            };
            return sb.ToString();
        }
    }
}