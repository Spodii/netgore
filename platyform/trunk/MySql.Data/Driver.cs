using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using MySql.Data.Common;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for BaseDriver.
    /// </summary>
    abstract class Driver : IDisposable
    {
        protected Hashtable charSets;
        protected MySqlConnection connection;
        protected MySqlConnectionStringBuilder connectionString;
        protected DateTime creationTime;
        protected MySqlPromotableTransaction currentTransaction;
        protected Encoding encoding;
        protected bool hasWarnings;
        protected bool inActiveUse;
        protected bool isOpen;
        protected long maxPacketSize;
        protected MySqlPool pool;
        protected ClientFlags serverCaps;
        protected string serverCharSet;
        protected int serverCharSetIndex;
        protected Hashtable serverProps;
        protected ServerStatusFlags serverStatus;
        protected int threadId;
        protected DBVersion version;

        public MySqlConnection Connection
        {
            get { return connection; }
        }

        internal int ConnectionCharSetIndex
        {
            get { return serverCharSetIndex; }
        }

        public MySqlPromotableTransaction CurrentTransaction
        {
            get { return currentTransaction; }
            set { currentTransaction = value; }
        }

        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public bool HasWarnings
        {
            get { return hasWarnings; }
        }

        public bool IsInActiveUse
        {
            get { return inActiveUse; }
            set { inActiveUse = value; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        public long MaxPacketSize
        {
            get { return maxPacketSize; }
        }

        public MySqlPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        public ServerStatusFlags ServerStatus
        {
            get { return serverStatus; }
        }

        public MySqlConnectionStringBuilder Settings
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public abstract bool SupportsBatch { get; }

        public int ThreadID
        {
            get { return threadId; }
        }

        public DBVersion Version
        {
            get { return version; }
        }

        protected Driver(MySqlConnectionStringBuilder settings)
        {
            encoding = Encoding.GetEncoding(1252);
            connectionString = settings;
            threadId = -1;
            serverCharSetIndex = -1;
            serverCharSet = null;
            hasWarnings = false;
            maxPacketSize = 1024;
        }

        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void CloseStatement(int id);

        public virtual void Configure(MySqlConnection pconnection)
        {
            connection = pconnection;

            bool firstConfigure = false;
            // if we have not already configured our server variables
            // then do so now
            if (serverProps == null)
            {
                firstConfigure = true;
                // load server properties
                serverProps = new Hashtable();
                MySqlCommand cmd = new MySqlCommand("SHOW VARIABLES", pconnection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            string key = reader.GetString(0);
                            string value = reader.GetString(1);
                            serverProps[key] = value;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        throw;
                    }
                }

                if (serverProps.Contains("max_allowed_packet"))
                    maxPacketSize = Convert.ToInt64(serverProps["max_allowed_packet"]);

                LoadCharacterSets();
            }

            // if the user has indicated that we are not to reset
            // the pconnection and this is not our first time through,
            // then we are done.
            if (!Settings.ConnectionReset && !firstConfigure)
                return;

            string charSet = connectionString.CharacterSet;
            if (string.IsNullOrEmpty(charSet))
            {
                if (!version.isAtLeast(4, 1, 0))
                {
                    if (serverProps.Contains("character_set"))
                        charSet = serverProps["character_set"].ToString();
                }
                else
                {
                    if (serverCharSetIndex >= 0)
                        charSet = (string)charSets[serverCharSetIndex];
                    else
                        charSet = serverCharSet;
                }
            }

            // now tell the server which character set we will send queries in and which charset we
            // want results in
            if (version.isAtLeast(4, 1, 0))
            {
                MySqlCommand cmd = new MySqlCommand("SET character_set_results=NULL", pconnection);
                object clientCharSet = serverProps["character_set_client"];
                object connCharSet = serverProps["character_set_connection"];
                if ((clientCharSet != null && clientCharSet.ToString() != charSet) ||
                    (connCharSet != null && connCharSet.ToString() != charSet))
                    cmd.CommandText = "SET NAMES " + charSet + ";" + cmd.CommandText;
                cmd.ExecuteNonQuery();
            }

            if (charSet != null)
                Encoding = CharSetMap.GetEncoding(version, charSet);
            else
                Encoding = CharSetMap.GetEncoding(version, "latin1");
        }

        public static Driver Create(MySqlConnectionStringBuilder settings)
        {
            Driver d = null;
            if (settings.DriverType == MySqlDriverType.Native)
                d = new NativeDriver(settings);
            d.Open();
            return d;
        }

        protected virtual void Dispose(bool disposing)
        {
            // if we are pooling, then release ourselves
            if (connectionString.Pooling)
                MySqlPoolManager.RemoveConnection(this);

            isOpen = false;
        }

        public abstract void ExecuteStatement(byte[] bytes);

        public abstract bool FetchDataRow(int statementId, int pageSize, int columns);

        public bool IsTooOld()
        {
            TimeSpan ts = DateTime.Now.Subtract(creationTime);
            if (Settings.ConnectionLifeTime != 0 && ts.TotalSeconds > Settings.ConnectionLifeTime)
                return true;
            return false;
        }

        /// <summary>
        /// Loads all the current character set names and ids for this server 
        /// into the charSets hashtable
        /// </summary>
        void LoadCharacterSets()
        {
            if (!version.isAtLeast(4, 1, 0))
                return;

            MySqlCommand cmd = new MySqlCommand("SHOW COLLATION", connection);

            // now we load all the currently active collations
            try
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    charSets = new Hashtable();
                    while (reader.Read())
                    {
                        charSets[Convert.ToInt32(reader["id"], NumberFormatInfo.InvariantInfo)] =
                            reader.GetString(reader.GetOrdinal("charset"));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public virtual void Open()
        {
            creationTime = DateTime.Now;
        }

        public abstract bool Ping();

        public abstract int PrepareStatement(string sql, ref MySqlField[] parameters);

        public string Property(string key)
        {
            return (string)serverProps[key];
        }

        public abstract void Query(byte[] bytes, int length);

        public abstract MySqlField[] ReadColumnMetadata(int count);

        public abstract IMySqlValue ReadColumnValue(int index, MySqlField field, IMySqlValue value);

        public abstract long ReadResult(ref ulong affectedRows, ref long lastInsertId);

        public void ReportWarnings()
        {
            ArrayList errors = new ArrayList();

            MySqlCommand cmd = new MySqlCommand("SHOW WARNINGS", connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    errors.Add(new MySqlError(reader.GetString(0), reader.GetInt32(1), reader.GetString(2)));
                }

                hasWarnings = false;
                // MySQL resets warnings before each statement, so a batch could indicate
                // warnings when there aren't any
                if (errors.Count == 0)
                    return;

                MySqlInfoMessageEventArgs args = new MySqlInfoMessageEventArgs
                                                 { errors = ((MySqlError[])errors.ToArray(typeof(MySqlError))) };
                if (connection != null)
                    connection.OnInfoMessage(args);
            }
        }

        public abstract void Reset();

        public virtual void SafeClose()
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
            }
        }

        public abstract void SetDatabase(string dbName);

        public abstract void SkipColumnValue(IMySqlValue valObject);

        public abstract bool SkipDataRow();

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}