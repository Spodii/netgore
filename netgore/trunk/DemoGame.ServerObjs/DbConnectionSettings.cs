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

        readonly string _sqlDatabase;
        readonly string _sqlHost;
        readonly string _sqlPass;
        readonly string _sqlPort;
        readonly string _sqlUser;

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
            var dic = XmlInfoReader.ReadFile(destSettingsFile).First();
            _sqlUser = dic["MySql.User"];
            _sqlHost = dic.AsString("MySql.Host", "localhost");
            _sqlDatabase = dic["MySql.Database"];
            _sqlPort = dic.AsString("MySql.Port", "3306");
            _sqlPass = dic.AsString("MySql.Pass", string.Empty);
        }

        /// <summary>
        /// Gets the Sql database name containing the game information.
        /// </summary>
        public string SqlDatabase
        {
            get { return _sqlDatabase; }
        }

        /// <summary>
        /// Gets the Sql connection host address.
        /// </summary>
        public string SqlHost
        {
            get { return _sqlHost; }
        }

        /// <summary>
        /// Gets the Sql connection password.
        /// </summary>
        public string SqlPass
        {
            get { return _sqlPass; }
        }

        /// <summary>
        /// Gets the Sql Host Port number, convert to UInt
        /// </summary>
        public uint SqlPort
        {
            get { return Convert.ToUInt32(_sqlPort); }
        }

        /// <summary>
        /// Gets the Sql user name.
        /// </summary>
        public string SqlUser
        {
            get { return _sqlUser; }
        }

        /// <summary>
        /// Makes a Sql connection string for the given settings.
        /// </summary>
        /// <returns>Sql connection string.</returns>
        public string SqlConnectionString()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder
            {
                Database = SqlDatabase,
                UserID = SqlUser,
                Password = SqlPass,
                Server = SqlHost,
                Port = SqlPort,
                IgnorePrepare = false
            };
            return sb.ToString();
        }
    }
}