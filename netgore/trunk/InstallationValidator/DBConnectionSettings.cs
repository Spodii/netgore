using System;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// Settings for the database connection.
    /// </summary>
    public class DBConnectionSettings
    {
        readonly string _database;
        readonly string _host;
        readonly string _pass;
        readonly string _user;

        /// <summary>
        /// Gets the Sql database name containing the game information.
        /// </summary>
        public string Database
        {
            get { return _database; }
        }

        /// <summary>
        /// Gets the Sql connection host address.
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Gets the Sql connection password.
        /// </summary>
        public string Pass
        {
            get { return _pass; }
        }

        /// <summary>
        /// Gets the Sql user name.
        /// </summary>
        public string User
        {
            get { return _user; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnectionSettings"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="pass">The pass.</param>
        /// <param name="db">The db.</param>
        /// <param name="host">The host.</param>
        public DBConnectionSettings(string user, string pass, string db, string host)
        {
            _user = user;
            _pass = pass;
            _database = db;
            _host = host;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnectionSettings"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public DBConnectionSettings(string filePath)
        {
            var dic = XmlInfoReader.ReadFile(filePath)[0];
            _user = dic["MySql.User"];
            _host = dic["MySql.Host"];
            _database = dic["MySql.Database"];

            if (dic.ContainsKey("MySql.Pass"))
                _pass = dic["MySql.Pass"];
            else
                _pass = string.Empty;
        }
    }
}