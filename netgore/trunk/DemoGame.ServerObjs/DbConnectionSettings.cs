using System.Linq;
using System;
using MySql.Data.MySqlClient;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Settings for the database connection.
    /// </summary>
    public class DbConnectionSettings
    {
        readonly string _sqlDatabase;
        readonly string _sqlHost;
        readonly string _sqlPass;
        readonly string _sqlUser;
        readonly string _sqlPort;

        /// <summary>
        /// ServerSettings constructor.
        /// </summary>
        public DbConnectionSettings()
        {
            var dic = XmlInfoReader.ReadFile("DbSettings.xml")[0];
            _sqlUser = dic["MySql.User"];
            _sqlHost = dic["MySql.Host"];
            _sqlDatabase = dic["MySql.Database"];

            if (dic.ContainsKey("MySql.Port"))
                _sqlPort = dic["MySql.Port"];
            else
                _sqlPort = "3306";
            
            if (dic.ContainsKey("MySql.Pass"))
                _sqlPass = dic["MySql.Pass"];
            else
                _sqlPass = string.Empty;
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
        /// Gets the Sql Host Port number, convert to UInt
        /// </summary>
        public uint SqlPort
        {
            get { return System.Convert.ToUInt32(_sqlPort); }
        }
        
        /// <summary>
        /// Gets the Sql connection password.
        /// </summary>
        public string SqlPass
        {
            get { return _sqlPass; }
        }

        /// <summary>
        /// Gets the Sql user name.
        /// </summary>
        public string SqlUser
        {
            get { return _sqlUser; }
        }

        /// <summary>
        /// Makes a Sql connection string for the given settings
        /// </summary>
        /// <returns>Sql connection string</returns>
        public string SqlConnectionString()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder
            { Database = SqlDatabase, UserID = SqlUser, Password = SqlPass, Server = SqlHost, Port = SqlPort, IgnorePrepare = false };
            return sb.ToString();
        }
    }
}