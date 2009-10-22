using NetGore;

namespace NetGore
{
    /// <summary>
    /// Settings for the database connection.
    /// </summary>
    public class DBConnectionSettings
    {
        readonly string _sqlDatabase;
        readonly string _sqlHost;
        readonly string _sqlPass;
        readonly string _sqlUser;

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
        /// Gets the Sql user name.
        /// </summary>
        public string SqlUser
        {
            get { return _sqlUser; }
        }

        /// <summary>
        /// ServerSettings constructor.
        /// </summary>
        public DBConnectionSettings(string filePath)
        {
            var dic = XmlInfoReader.ReadFile(filePath)[0];
            _sqlUser = dic["MySql.User"];
            _sqlHost = dic["MySql.Host"];
            _sqlDatabase = dic["MySql.Database"];

            if (dic.ContainsKey("MySql.Pass"))
                _sqlPass = dic["MySql.Pass"];
            else
                _sqlPass = string.Empty;
        }
    }
}