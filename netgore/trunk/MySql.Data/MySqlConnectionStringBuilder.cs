using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient.Properties;

#pragma warning disable 1591

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/MySqlConnectionStringBuilder.xml' path='docs/Class/*'/>
    public sealed class MySqlConnectionStringBuilder : DbConnectionStringBuilder
    {
        static readonly Dictionary<Keyword, object> defaultValues = new Dictionary<Keyword, object>();

        readonly string originalConnectionString;
        readonly StringBuilder persistConnString;
        bool allowBatch;
        bool allowUserVariables;
        bool allowZeroDatetime;
        bool autoEnlist;
        string blobAsUtf8ExcludePattern;
        Regex blobAsUtf8ExcludeRegex;
        string blobAsUtf8IncludePattern;
        Regex blobAsUtf8IncludeRegex;
        string charSet;
        bool clearing;
        bool compress;
        uint connectionLifetime;
        bool connectionReset;
        uint connectionTimeout;
        bool convertZeroDatetime;
        string database;
        uint defaultCommandTimeout;
        MySqlDriverType driverType;
        bool functionsReturnString;
        bool ignorePrepare;
        bool interactiveSession;
        bool logging;
        uint maxPoolSize;
        uint minPoolSize;
        bool oldSyntax;
        string password;
        bool persistSI;
        string pipeName;
        bool pooling;
        uint port;
        uint procCacheSize;
        MySqlConnectionProtocol protocol;
        bool respectBinaryFlags;
        string server;
        string sharedMemName;
        bool treatBlobsAsUTF8;
        bool treatTinyAsBoolean;
        bool usePerfMon;
        bool useProcedureBodies;
        string userId;
        bool useSSL;
        bool useUsageAdvisor;

        /// <summary>
        /// Gets or sets a boolean value that indicates whether this connection will allow
        /// commands to send multiple SQL statements in one execution.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Allow Batch")]
        [Description("Allows execution of multiple SQL commands in a single statement")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool AllowBatch
        {
            get { return allowBatch; }
            set
            {
                SetValue("Allow Batch", value);
                allowBatch = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Allow User Variables")]
        [Description("Should the provider expect user variables to appear in the SQL.")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool AllowUserVariables
        {
            get { return allowUserVariables; }
            set
            {
                SetValue("Allow User Variables", value);
                allowUserVariables = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates if zero date time values are supported.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Allow Zero Datetime")]
        [Description("Should zero datetimes be supported")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool AllowZeroDateTime
        {
            get { return allowZeroDatetime; }
            set
            {
                SetValue("Allow Zero Datetime", value);
                allowZeroDatetime = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Auto Enlist")]
        [Description("Should the connetion automatically enlist in the active connection, if there are any.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool AutoEnlist
        {
            get { return autoEnlist; }
            set
            {
                SetValue("Auto Enlist", value);
                autoEnlist = value;
            }
        }

        /// <summary>
        /// Gets or sets the pattern that matches the columns that should not be treated as UTF8
        /// </summary>
        [DisplayName("BlobAsUTF8ExcludePattern")]
        [Category("Advanced")]
        [Description("Pattern that matches columns that should not be treated as UTF8")]
        [RefreshProperties(RefreshProperties.All)]
        public string BlobAsUTF8ExcludePattern
        {
            get { return blobAsUtf8ExcludePattern; }
            set
            {
                SetValue("BlobAsUTF8ExcludePattern", value);
                blobAsUtf8ExcludePattern = value;
                blobAsUtf8ExcludeRegex = null;
            }
        }

        internal Regex BlobAsUTF8ExcludeRegex
        {
            get
            {
                if (blobAsUtf8ExcludePattern == null)
                    return null;
                if (blobAsUtf8ExcludeRegex == null)
                    blobAsUtf8ExcludeRegex = new Regex(blobAsUtf8ExcludePattern);
                return blobAsUtf8ExcludeRegex;
            }
        }

        /// <summary>
        /// Gets or sets the pattern that matches the columns that should be treated as UTF8
        /// </summary>
        [DisplayName("BlobAsUTF8IncludePattern")]
        [Category("Advanced")]
        [Description("Pattern that matches columns that should be treated as UTF8")]
        [RefreshProperties(RefreshProperties.All)]
        public string BlobAsUTF8IncludePattern
        {
            get { return blobAsUtf8IncludePattern; }
            set
            {
                SetValue("BlobAsUTF8IncludePattern", value);
                blobAsUtf8IncludePattern = value;
                blobAsUtf8IncludeRegex = null;
            }
        }

        internal Regex BlobAsUTF8IncludeRegex
        {
            get
            {
                if (blobAsUtf8IncludePattern == null)
                    return null;
                if (blobAsUtf8IncludeRegex == null)
                    blobAsUtf8IncludeRegex = new Regex(blobAsUtf8IncludePattern);
                return blobAsUtf8IncludeRegex;
            }
        }

        /// <summary>
        /// Gets or sets the character set that should be used for sending queries to the server.
        /// </summary>
        [DisplayName("Character Set")]
        [Category("Advanced")]
        [Description("Character set this connection should use")]
        [RefreshProperties(RefreshProperties.All)]
        public string CharacterSet
        {
            get { return charSet; }
            set
            {
                SetValue("Character Set", value);
                charSet = value;
            }
        }

        /// <summary>
        /// Gets or sets the lifetime of a pooled connection.
        /// </summary>
        [Category("Pooling")]
        [DisplayName("Connection Lifetime")]
        [Description(
            "The minimum amount of time (in seconds) for this connection to " + "live in the pool before being destroyed.")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public uint ConnectionLifeTime
        {
            get { return connectionLifetime; }
            set
            {
                SetValue("Connection Lifetime", value);
                connectionLifetime = value;
            }
        }

        /// <summary>
        /// Gets or sets the protocol that should be used for communicating
        /// with MySQL.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Connection Protocol")]
        [Description("Protocol to use for connection to MySQL")]
        [DefaultValue(MySqlConnectionProtocol.Sockets)]
        [RefreshProperties(RefreshProperties.All)]
        public MySqlConnectionProtocol ConnectionProtocol
        {
            get { return protocol; }
            set
            {
                SetValue("Connection Protocol", value);
                protocol = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if the connection should be reset when retrieved
        /// from the pool.
        /// </summary>
        [Category("Pooling")]
        [DisplayName("Connection Reset")]
        [Description("When true, indicates the connection state is reset when " + "removed from the pool.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool ConnectionReset
        {
            get { return connectionReset; }
            set
            {
                SetValue("Connection Reset", value);
                connectionReset = value;
            }
        }

        /// <summary>
        /// Gets or sets the connection timeout.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Connect Timeout")]
        [Description(
            "The length of time (in seconds) to wait for a connection " +
            "to the server before terminating the attempt and generating an error.")]
        [DefaultValue(15)]
        [RefreshProperties(RefreshProperties.All)]
        public uint ConnectionTimeout
        {
            get { return connectionTimeout; }
            set
            {
                SetValue("Connect Timeout", value);
                connectionTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if zero datetime values should be 
        /// converted to DateTime.MinValue.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Convert Zero Datetime")]
        [Description("Should illegal datetime values be converted to DateTime.MinValue")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool ConvertZeroDateTime
        {
            get { return convertZeroDatetime; }
            set
            {
                SetValue("Convert Zero Datetime", value);
                convertZeroDatetime = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the database the connection should 
        /// initially connect to.
        /// </summary>
        [Category("Connection")]
        [Description("Database to use initially")]
        [RefreshProperties(RefreshProperties.All)]
        public string Database
        {
            get { return database; }
            set
            {
                SetValue("Database", value);
                database = value;
            }
        }

        /// <summary>
        /// Gets or sets the default command timeout.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Default Command Timeout")]
        [Description(@"The default timeout that MySqlCommand objects will use
                     unless changed.")]
        [DefaultValue(30)]
        [RefreshProperties(RefreshProperties.All)]
        public uint DefaultCommandTimeout
        {
            get { return defaultCommandTimeout; }
            set
            {
                SetValue("Default Command Timeout", value);
                defaultCommandTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the driver type that should be used for this connection.
        /// </summary>
        /// <remarks>
        /// There is only one valid value for this setting currently.
        /// </remarks>
        [Category("Connection")]
        [DisplayName("Driver Type")]
        [Description("Specifies the type of driver to use for this connection")]
        [DefaultValue(MySqlDriverType.Native)]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        public MySqlDriverType DriverType
        {
            get { return driverType; }
            set
            {
                SetValue("Driver Type", value);
                driverType = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Functions Return String")]
        [Description("Should all server functions be treated as returning string?")]
        [DefaultValue(false)]
        public bool FunctionsReturnString
        {
            get { return functionsReturnString; }
            set
            {
                SetValue("Functions Return String", value);
                functionsReturnString = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if calls to Prepare() should be ignored.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Ignore Prepare")]
        [Description("Instructs the provider to ignore any attempts to prepare a command.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool IgnorePrepare
        {
            get { return ignorePrepare; }
            set
            {
                SetValue("Ignore Prepare", value);
                ignorePrepare = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Interactive Session")]
        [Description("Should this session be considered interactive?")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool InteractiveSession
        {
            get { return interactiveSession; }
            set
            {
                SetValue("Interactive Session", value);
                interactiveSession = value;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key. In C#, this property 
        /// is the indexer. 
        /// </summary>
        /// <param name="keyword">The key of the item to get or set.</param>
        /// <returns>The value associated with the specified key. </returns>
        public override object this[string keyword]
        {
            get
            {
                Keyword kw = GetKey(keyword);
                return GetValue(kw);
            }
            set
            {
                if (value == null)
                    Remove(keyword);
                else
                    SetValue(keyword, value);
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether logging is enabled.
        /// </summary>
        [Category("Connection")]
        [Description("Enables output of diagnostic messages")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool Logging
        {
            get { return logging; }
            set
            {
                SetValue("Logging", value);
                logging = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum connection pool setting.
        /// </summary>
        [Category("Pooling")]
        [DisplayName("Maximum Pool Size")]
        [Description("The maximum number of connections allowed in the pool.")]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.All)]
        public uint MaximumPoolSize
        {
            get { return maxPoolSize; }
            set
            {
                SetValue("Maximum Pool Size", value);
                maxPoolSize = value;
            }
        }

        /// <summary>
        /// Gets the minimum connection pool size.
        /// </summary>
        [Category("Pooling")]
        [DisplayName("Minimum Pool Size")]
        [Description("The minimum number of connections allowed in the pool.")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        public uint MinimumPoolSize
        {
            get { return minPoolSize; }
            set
            {
                SetValue("Minimum Pool Size", value);
                minPoolSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the password that should be used to connect with.
        /// </summary>
        [Category("Security")]
        [Description("Indicates the password to be used when connecting to the data source.")]
        [PasswordPropertyText(true)]
        [RefreshProperties(RefreshProperties.All)]
        public string Password
        {
            get { return password; }
            set
            {
                SetValue("Password", value);
                password = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the password should be persisted
        /// in the connection string.
        /// </summary>
        [Category("Security")]
        [DisplayName("Persist Security Info")]
        [Description(
            "When false, security-sensitive information, such as the password, " +
            "is not returned as part of the connection if the connection is open or " + "has ever been in an open state.")]
        [RefreshProperties(RefreshProperties.All)]
        public bool PersistSecurityInfo
        {
            get { return persistSI; }
            set
            {
                SetValue("Persist Security Info", value);
                persistSI = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the named pipe that should be used
        /// for communicating with MySQL.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Pipe Name")]
        [Description("Name of pipe to use when connecting with named pipes (Win32 only)")]
        [DefaultValue("MYSQL")]
        [RefreshProperties(RefreshProperties.All)]
        public string PipeName
        {
            get { return pipeName; }
            set
            {
                SetValue("Pipe Name", value);
                pipeName = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if connection pooling is enabled.
        /// </summary>
        [Category("Pooling")]
        [Description(
            "When true, the connection object is drawn from the appropriate " +
            "pool, or if necessary, is created and added to the appropriate pool.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool Pooling
        {
            get { return pooling; }
            set
            {
                SetValue("Pooling", value);
                pooling = value;
            }
        }

        /// <summary>
        /// Gets or sets the port number that is used when the socket
        /// protocol is being used.
        /// </summary>
        [Category("Connection")]
        [Description("Port to use for TCP/IP connections")]
        [DefaultValue(3306)]
        [RefreshProperties(RefreshProperties.All)]
        public uint Port
        {
            get { return port; }
            set
            {
                SetValue("Port", value);
                port = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the stored procedure cache.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Procedure Cache Size")]
        [Description(
            "Indicates how many stored procedures can be cached at one time. " +
            "A value of 0 effectively disables the procedure cache.")]
        [DefaultValue(25)]
        [RefreshProperties(RefreshProperties.All)]
        public uint ProcedureCacheSize
        {
            get { return procCacheSize; }
            set
            {
                SetValue("Procedure Cache Size", value);
                procCacheSize = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Respect Binary Flags")]
        [Description("Should binary flags on column metadata be respected.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool RespectBinaryFlags
        {
            get { return respectBinaryFlags; }
            set
            {
                SetValue("Respect Binary Flags", value);
                respectBinaryFlags = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The server.</value>
        [Category("Connection")]
        [Description("Server to connect to")]
        [RefreshProperties(RefreshProperties.All)]
        public string Server
        {
            get { return server; }
            set
            {
                SetValue("Server", value);
                server = value;
            }
        }

        /// <summary>
        /// Gets or sets the base name of the shared memory objects used to 
        /// communicate with MySQL when the shared memory protocol is being used.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Shared Memory Name")]
        [Description("Name of the shared memory object to use")]
        [DefaultValue("MYSQL")]
        [RefreshProperties(RefreshProperties.All)]
        public string SharedMemoryName
        {
            get { return sharedMemName; }
            set
            {
                SetValue("Shared Memory Name", value);
                sharedMemName = value;
            }
        }

        /// <summary>
        /// Indicates whether the driver should treat binary blobs as UTF8
        /// </summary>
        [DisplayName("Treat Blobs As UTF8")]
        [Category("Advanced")]
        [Description("Should binary blobs be treated as UTF8")]
        [RefreshProperties(RefreshProperties.All)]
        public bool TreatBlobsAsUTF8
        {
            get { return treatBlobsAsUTF8; }
            set
            {
                SetValue("TreatBlobsAsUTF8", value);
                treatBlobsAsUTF8 = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Treat Tiny As Boolean")]
        [Description("Should the provider treat TINYINT(1) columns as boolean.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool TreatTinyAsBoolean
        {
            get { return treatTinyAsBoolean; }
            set
            {
                SetValue("Treat Tiny As Boolean", value);
                treatTinyAsBoolean = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether this connection
        /// should use compression.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Use Compression")]
        [Description("Should the connection ues compression")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool UseCompression
        {
            get { return compress; }
            set
            {
                SetValue("Use Compression", value);
                compress = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether this connection uses
        /// the old style (@) parameter markers or the new (?) style.
        /// </summary>
        [Category("Connection")]
        [DisplayName("Use Old Syntax")]
        [Description("Allows the use of old style @ syntax for parameters")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        [Obsolete("Use Old Syntax is no longer needed.  See documentation")]
        public bool UseOldSyntax
        {
            get { return oldSyntax; }
            set
            {
                SetValue("Use Old Syntax", value);
                oldSyntax = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if the permon hooks should be enabled.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Use Performance Monitor")]
        [Description("Indicates that performance counters should be updated during execution.")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool UsePerformanceMonitor
        {
            get { return usePerfMon; }
            set
            {
                SetValue("Use Performance Monitor", value);
                usePerfMon = value;
            }
        }

        [Category("Advanced")]
        [DisplayName("Use Procedure Bodies")]
        [Description("Indicates if stored procedure bodies will be available for parameter detection.")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool UseProcedureBodies
        {
            get { return useProcedureBodies; }
            set
            {
                SetValue("Use Procedure Bodies", value);
                useProcedureBodies = value;
            }
        }

        /// <summary>
        /// Gets or sets the user id that should be used to connect with.
        /// </summary>
        [Category("Security")]
        [DisplayName("User Id")]
        [Description("Indicates the user ID to be used when connecting to the data source.")]
        [RefreshProperties(RefreshProperties.All)]
        public string UserID
        {
            get { return userId; }
            set
            {
                SetValue("User Id", value);
                userId = value;
            }
        }

        [Category("Authentication")]
        [Description("Should the connection use SSL.  This currently has no effect.")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        internal bool UseSSL
        {
            get { return useSSL; }
            set
            {
                SetValue("UseSSL", value);
                useSSL = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating if the Usage Advisor should be enabled.
        /// </summary>
        [Category("Advanced")]
        [DisplayName("Use Usage Advisor")]
        [Description("Logs inefficient database operations")]
        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        public bool UseUsageAdvisor
        {
            get { return useUsageAdvisor; }
            set
            {
                SetValue("Use Usage Advisor", value);
                useUsageAdvisor = value;
            }
        }

        static MySqlConnectionStringBuilder()
        {
            defaultValues.Add(Keyword.ConnectionTimeout, 15);
            defaultValues.Add(Keyword.Pooling, true);
            defaultValues.Add(Keyword.Port, 3306);
            defaultValues.Add(Keyword.Server, "");
            defaultValues.Add(Keyword.PersistSecurityInfo, false);
            defaultValues.Add(Keyword.ConnectionLifetime, 0);
            defaultValues.Add(Keyword.ConnectionReset, false);
            defaultValues.Add(Keyword.MinimumPoolSize, 0);
            defaultValues.Add(Keyword.MaximumPoolSize, 100);
            defaultValues.Add(Keyword.UserID, "");
            defaultValues.Add(Keyword.Password, "");
            defaultValues.Add(Keyword.UseUsageAdvisor, false);
            defaultValues.Add(Keyword.CharacterSet, "");
            defaultValues.Add(Keyword.Compress, false);
            defaultValues.Add(Keyword.PipeName, "MYSQL");
            defaultValues.Add(Keyword.Logging, false);
            defaultValues.Add(Keyword.OldSyntax, false);
            defaultValues.Add(Keyword.SharedMemoryName, "MYSQL");
            defaultValues.Add(Keyword.AllowBatch, true);
            defaultValues.Add(Keyword.ConvertZeroDatetime, false);
            defaultValues.Add(Keyword.Database, "");
            defaultValues.Add(Keyword.DriverType, MySqlDriverType.Native);
            defaultValues.Add(Keyword.Protocol, MySqlConnectionProtocol.Sockets);
            defaultValues.Add(Keyword.AllowZeroDatetime, false);
            defaultValues.Add(Keyword.UsePerformanceMonitor, false);
            defaultValues.Add(Keyword.ProcedureCacheSize, 25);
            defaultValues.Add(Keyword.UseSSL, false);
            defaultValues.Add(Keyword.IgnorePrepare, true);
            defaultValues.Add(Keyword.UseProcedureBodies, true);
            defaultValues.Add(Keyword.AutoEnlist, true);
            defaultValues.Add(Keyword.RespectBinaryFlags, true);
            defaultValues.Add(Keyword.BlobAsUTF8ExcludePattern, null);
            defaultValues.Add(Keyword.BlobAsUTF8IncludePattern, null);
            defaultValues.Add(Keyword.TreatBlobsAsUTF8, false);
            defaultValues.Add(Keyword.DefaultCommandTimeout, 30);
            defaultValues.Add(Keyword.TreatTinyAsBoolean, true);
            defaultValues.Add(Keyword.AllowUserVariables, false);
            defaultValues.Add(Keyword.InteractiveSession, false);
            defaultValues.Add(Keyword.FunctionsReturnString, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnectionStringBuilder"/> class. 
        /// </summary>
        public MySqlConnectionStringBuilder()
        {
            persistConnString = new StringBuilder();
            Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnectionStringBuilder"/> class. 
        /// The provided connection string provides the data for the instance's internal 
        /// connection information. 
        /// </summary>
        /// <param name="connectionString">The basis for the object's internal connection 
        /// information. Parsed into name/value pairs. Invalid key names raise 
        /// <see cref="KeyNotFoundException"/>.
        /// </param>
        public MySqlConnectionStringBuilder(string connectionString) : this()
        {
            originalConnectionString = connectionString;
            persistConnString = new StringBuilder();
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Clears the contents of the <see cref="MySqlConnectionStringBuilder"/> instance. 
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            persistConnString.Remove(0, persistConnString.Length);

            clearing = true;
            // set all the proper defaults
            foreach (var k in defaultValues)
            {
                SetValue(k.Key, k.Value);
            }
            clearing = false;
        }

        static bool ConvertToBool(object value)
        {
            if (value is string)
            {
                string s = value.ToString().ToLower();
                if (s == "yes" || s == "true")
                    return true;
                if (s == "no" || s == "false")
                    return false;
                throw new ArgumentException(Resources.ImproperValueFormat, (string)value);
            }
            try
            {
                return ((IConvertible)value).ToBoolean(CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(Resources.ImproperValueFormat, value.ToString());
            }
        }

        static MySqlDriverType ConvertToDriverType(object value)
        {
            if (value is MySqlDriverType)
                return (MySqlDriverType)value;
            return (MySqlDriverType)Enum.Parse(typeof(MySqlDriverType), value.ToString(), true);
        }

        static MySqlConnectionProtocol ConvertToProtocol(object value)
        {
            try
            {
                if (value is MySqlConnectionProtocol)
                    return (MySqlConnectionProtocol)value;
                return (MySqlConnectionProtocol)Enum.Parse(typeof(MySqlConnectionProtocol), value.ToString(), true);
            }
            catch (Exception)
            {
                if (value is string)
                {
                    string lowerString = (value as string).ToLower();
                    if (lowerString == "socket" || lowerString == "tcp")
                        return MySqlConnectionProtocol.Sockets;
                    if (lowerString == "pipe")
                        return MySqlConnectionProtocol.NamedPipe;
                    if (lowerString == "unix")
                        return MySqlConnectionProtocol.UnixSocket;
                    if (lowerString == "memory")
                        return MySqlConnectionProtocol.SharedMemory;
                }
            }
            throw new ArgumentException(Resources.ImproperValueFormat, value.ToString());
        }

        static uint ConvertToUInt(object value)
        {
            try
            {
                uint uValue = ((IConvertible)value).ToUInt32(CultureInfo.InvariantCulture);
                return uValue;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(Resources.ImproperValueFormat, value.ToString());
            }
        }

        /// <summary>
        /// Takes a given connection string and returns it, possible
        /// stripping out the password info
        /// </summary>
        /// <returns></returns>
        internal string GetConnectionString(bool includePass)
        {
            if (includePass)
                return originalConnectionString;
            string connStr = persistConnString.ToString();
            return connStr.Remove(connStr.Length - 1, 1);
        }

        static Keyword GetKey(string key)
        {
            string lowerKey = key.ToLower(CultureInfo.InvariantCulture);
            switch (lowerKey)
            {
                case "uid":
                case "username":
                case "user id":
                case "user name":
                case "userid":
                case "user":
                    return Keyword.UserID;
                case "host":
                case "server":
                case "data source":
                case "datasource":
                case "address":
                case "addr":
                case "network address":
                    return Keyword.Server;
                case "password":
                case "pwd":
                    return Keyword.Password;
                case "useusageadvisor":
                case "usage advisor":
                case "use usage advisor":
                    return Keyword.UseUsageAdvisor;
                case "character set":
                case "charset":
                    return Keyword.CharacterSet;
                case "use compression":
                case "compress":
                    return Keyword.Compress;
                case "pipe name":
                case "pipe":
                    return Keyword.PipeName;
                case "logging":
                    return Keyword.Logging;
                case "use old syntax":
                case "old syntax":
                case "oldsyntax":
                    return Keyword.OldSyntax;
                case "shared memory name":
                    return Keyword.SharedMemoryName;
                case "allow batch":
                    return Keyword.AllowBatch;
                case "convert zero datetime":
                case "convertzerodatetime":
                    return Keyword.ConvertZeroDatetime;
                case "persist security info":
                    return Keyword.PersistSecurityInfo;
                case "initial catalog":
                case "database":
                    return Keyword.Database;
                case "connection timeout":
                case "connect timeout":
                    return Keyword.ConnectionTimeout;
                case "port":
                    return Keyword.Port;
                case "pooling":
                    return Keyword.Pooling;
                case "min pool size":
                case "minimum pool size":
                    return Keyword.MinimumPoolSize;
                case "max pool size":
                case "maximum pool size":
                    return Keyword.MaximumPoolSize;
                case "connection lifetime":
                    return Keyword.ConnectionLifetime;
                case "driver":
                    return Keyword.DriverType;
                case "protocol":
                case "connection protocol":
                    return Keyword.Protocol;
                case "allow zero datetime":
                case "allowzerodatetime":
                    return Keyword.AllowZeroDatetime;
                case "useperformancemonitor":
                case "use performance monitor":
                    return Keyword.UsePerformanceMonitor;
                case "procedure cache size":
                case "procedurecachesize":
                case "procedure cache":
                case "procedurecache":
                    return Keyword.ProcedureCacheSize;
                case "connection reset":
                    return Keyword.ConnectionReset;
                case "ignore prepare":
                    return Keyword.IgnorePrepare;
                case "encrypt":
                    return Keyword.UseSSL;
                case "procedure bodies":
                case "use procedure bodies":
                    return Keyword.UseProcedureBodies;
                case "auto enlist":
                    return Keyword.AutoEnlist;
                case "respect binary flags":
                    return Keyword.RespectBinaryFlags;
                case "blobasutf8excludepattern":
                    return Keyword.BlobAsUTF8ExcludePattern;
                case "blobasutf8includepattern":
                    return Keyword.BlobAsUTF8IncludePattern;
                case "treatblobsasutf8":
                case "treat blobs as utf8":
                    return Keyword.TreatBlobsAsUTF8;
                case "default command timeout":
                    return Keyword.DefaultCommandTimeout;
                case "treat tiny as boolean":
                    return Keyword.TreatTinyAsBoolean;
                case "allow user variables":
                    return Keyword.AllowUserVariables;
                case "interactive":
                case "interactive session":
                    return Keyword.InteractiveSession;
                case "functions return string":
                    return Keyword.FunctionsReturnString;
            }
            throw new ArgumentException(Resources.KeywordNotSupported, key);
        }

        ///<summary>
        ///
        ///                    Fills a supplied <see cref="T:System.Collections.Hashtable" /> with information about all the properties of this <see cref="T:System.Data.Common.DbConnectionStringBuilder" />.
        ///                
        ///</summary>
        ///
        ///<param name="propertyDescriptors">
        ///                    The <see cref="T:System.Collections.Hashtable" /> to be filled with information about this <see cref="T:System.Data.Common.DbConnectionStringBuilder" />.
        ///                </param>
        protected override void GetProperties(Hashtable propertyDescriptors)
        {
            base.GetProperties(propertyDescriptors);

            // use a custom type descriptor for connection protocol
            PropertyDescriptor pd = (PropertyDescriptor)propertyDescriptors["Connection Protocol"];
            var myAttr = new Attribute[pd.Attributes.Count];
            pd.Attributes.CopyTo(myAttr, 0);
            ConnectionProtocolDescriptor mypd = new ConnectionProtocolDescriptor(pd.Name, myAttr);
            propertyDescriptors["Connection Protocol"] = mypd;
        }

        object GetValue(Keyword kw)
        {
            switch (kw)
            {
                case Keyword.UserID:
                    return UserID;
                case Keyword.Password:
                    return Password;
                case Keyword.Port:
                    return Port;
                case Keyword.Server:
                    return Server;
                case Keyword.UseUsageAdvisor:
                    return UseUsageAdvisor;
                case Keyword.CharacterSet:
                    return CharacterSet;
                case Keyword.Compress:
                    return UseCompression;
                case Keyword.PipeName:
                    return PipeName;
                case Keyword.Logging:
                    return Logging;
                case Keyword.OldSyntax:
#pragma warning disable 618,612
                    return UseOldSyntax;
#pragma warning restore 618,612
                case Keyword.SharedMemoryName:
                    return SharedMemoryName;
                case Keyword.AllowBatch:
                    return AllowBatch;
                case Keyword.ConvertZeroDatetime:
                    return ConvertZeroDateTime;
                case Keyword.PersistSecurityInfo:
                    return PersistSecurityInfo;
                case Keyword.Database:
                    return Database;
                case Keyword.ConnectionTimeout:
                    return ConnectionTimeout;
                case Keyword.Pooling:
                    return Pooling;
                case Keyword.MinimumPoolSize:
                    return MinimumPoolSize;
                case Keyword.MaximumPoolSize:
                    return MaximumPoolSize;
                case Keyword.ConnectionLifetime:
                    return ConnectionLifeTime;
                case Keyword.DriverType:
                    return DriverType;
                case Keyword.Protocol:
                    return ConnectionProtocol;
                case Keyword.ConnectionReset:
                    return ConnectionReset;
                case Keyword.ProcedureCacheSize:
                    return ProcedureCacheSize;
                case Keyword.AllowZeroDatetime:
                    return AllowZeroDateTime;
                case Keyword.UsePerformanceMonitor:
                    return UsePerformanceMonitor;
                case Keyword.IgnorePrepare:
                    return IgnorePrepare;
                case Keyword.UseSSL:
                    return UseSSL;
                case Keyword.UseProcedureBodies:
                    return UseProcedureBodies;
                case Keyword.AutoEnlist:
                    return AutoEnlist;
                case Keyword.RespectBinaryFlags:
                    return RespectBinaryFlags;
                case Keyword.TreatBlobsAsUTF8:
                    return TreatBlobsAsUTF8;
                case Keyword.BlobAsUTF8ExcludePattern:
                    return blobAsUtf8ExcludePattern;
                case Keyword.BlobAsUTF8IncludePattern:
                    return blobAsUtf8IncludePattern;
                case Keyword.DefaultCommandTimeout:
                    return defaultCommandTimeout;
                case Keyword.TreatTinyAsBoolean:
                    return treatTinyAsBoolean;
                case Keyword.AllowUserVariables:
                    return allowUserVariables;
                case Keyword.InteractiveSession:
                    return interactiveSession;
                case Keyword.FunctionsReturnString:
                    return functionsReturnString;
                default:
                    return null; /* this will never happen */
            }
        }

        /// <summary>
        /// Removes the entry with the specified key from the <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see> instance.
        /// </summary>
        /// <param name="keyword">The key of the key/value pair to be removed from the connection string in this <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see>.</param>
        /// <returns>
        /// true if the key existed within the connection string and was removed; false if the key did not exist.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see> is read-only, or the <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see> has a fixed size.</exception>
        /// <exception cref="T:System.ArgumentNullException">keyword is null (Nothing in Visual Basic)</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
        public override bool Remove(string keyword)
        {
            // first we need to set this keys value to the default
            Keyword kw = GetKey(keyword);
            SetValue(kw, defaultValues[kw]);

            // then we remove this keyword from the base collection
            return base.Remove(keyword);
        }

        void SetValue(string keyword, object value)
        {
            if (value == null)
                throw new ArgumentException(Resources.KeywordNoNull, keyword);
            object out_obj;
            TryGetValue(keyword, out out_obj);

            Keyword kw = GetKey(keyword);
            SetValue(kw, value);
            base[keyword] = value;
            if (kw != Keyword.Password)
            {
                /* Nothing bad happens if the substring is not found */
                persistConnString.Replace(keyword + "=" + out_obj + ";", "");
                persistConnString.AppendFormat(CultureInfo.InvariantCulture, "{0}={1};", keyword, value);
            }
        }

        void SetValue(Keyword kw, object value)
        {
            switch (kw)
            {
                case Keyword.UserID:
                    userId = (string)value;
                    break;
                case Keyword.Password:
                    password = (string)value;
                    break;
                case Keyword.Port:
                    port = ConvertToUInt(value);
                    break;
                case Keyword.Server:
                    server = (string)value;
                    break;
                case Keyword.UseUsageAdvisor:
                    useUsageAdvisor = ConvertToBool(value);
                    break;
                case Keyword.CharacterSet:
                    charSet = (string)value;
                    break;
                case Keyword.Compress:
                    compress = ConvertToBool(value);
                    break;
                case Keyword.PipeName:
                    pipeName = (string)value;
                    break;
                case Keyword.Logging:
                    logging = ConvertToBool(value);
                    break;
                case Keyword.OldSyntax:
                    oldSyntax = ConvertToBool(value);
                    if (!clearing)
                        Logger.LogWarning("Use Old Syntax is now obsolete.  Please see documentation");
                    break;
                case Keyword.SharedMemoryName:
                    sharedMemName = (string)value;
                    break;
                case Keyword.AllowBatch:
                    allowBatch = ConvertToBool(value);
                    break;
                case Keyword.ConvertZeroDatetime:
                    convertZeroDatetime = ConvertToBool(value);
                    break;
                case Keyword.PersistSecurityInfo:
                    persistSI = ConvertToBool(value);
                    break;
                case Keyword.Database:
                    database = (string)value;
                    break;
                case Keyword.ConnectionTimeout:
                    connectionTimeout = ConvertToUInt(value);
                    break;
                case Keyword.Pooling:
                    pooling = ConvertToBool(value);
                    break;
                case Keyword.MinimumPoolSize:
                    minPoolSize = ConvertToUInt(value);
                    break;
                case Keyword.MaximumPoolSize:
                    maxPoolSize = ConvertToUInt(value);
                    break;
                case Keyword.ConnectionLifetime:
                    connectionLifetime = ConvertToUInt(value);
                    break;
                case Keyword.DriverType:
                    driverType = ConvertToDriverType(value);
                    break;
                case Keyword.Protocol:
                    protocol = ConvertToProtocol(value);
                    break;
                case Keyword.ConnectionReset:
                    connectionReset = ConvertToBool(value);
                    break;
                case Keyword.UsePerformanceMonitor:
                    usePerfMon = ConvertToBool(value);
                    break;
                case Keyword.AllowZeroDatetime:
                    allowZeroDatetime = ConvertToBool(value);
                    break;
                case Keyword.ProcedureCacheSize:
                    procCacheSize = ConvertToUInt(value);
                    break;
                case Keyword.IgnorePrepare:
                    ignorePrepare = ConvertToBool(value);
                    break;
                case Keyword.UseSSL:
                    useSSL = ConvertToBool(value);
                    break;
                case Keyword.UseProcedureBodies:
                    useProcedureBodies = ConvertToBool(value);
                    break;
                case Keyword.AutoEnlist:
                    autoEnlist = ConvertToBool(value);
                    break;
                case Keyword.RespectBinaryFlags:
                    respectBinaryFlags = ConvertToBool(value);
                    break;
                case Keyword.TreatBlobsAsUTF8:
                    treatBlobsAsUTF8 = ConvertToBool(value);
                    break;
                case Keyword.BlobAsUTF8ExcludePattern:
                    blobAsUtf8ExcludePattern = (string)value;
                    break;
                case Keyword.BlobAsUTF8IncludePattern:
                    blobAsUtf8IncludePattern = (string)value;
                    break;
                case Keyword.DefaultCommandTimeout:
                    defaultCommandTimeout = ConvertToUInt(value);
                    break;
                case Keyword.TreatTinyAsBoolean:
                    treatTinyAsBoolean = ConvertToBool(value);
                    break;
                case Keyword.AllowUserVariables:
                    allowUserVariables = ConvertToBool(value);
                    break;
                case Keyword.InteractiveSession:
                    interactiveSession = ConvertToBool(value);
                    break;
                case Keyword.FunctionsReturnString:
                    functionsReturnString = ConvertToBool(value);
                    break;
            }
        }

        /// <summary>
        /// Retrieves a value corresponding to the supplied key from this <see cref="T:System.Data.Common.DbConnectionStringBuilder"></see>.
        /// </summary>
        /// <param name="keyword">The key of the item to retrieve.</param>
        /// <param name="value">The value corresponding to the key.</param>
        /// <returns>
        /// true if keyword was found within the connection string, false otherwise.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">keyword contains a null value (Nothing in Visual Basic).</exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
        public override bool TryGetValue(string keyword, out object value)
        {
            try
            {
                Keyword kw = GetKey(keyword);
                value = GetValue(kw);
                return true;
            }
            catch (ArgumentException)
            {
            }
            value = null;
            return false;
        }
    }

    class ConnectionProtocolDescriptor : PropertyDescriptor
    {
        public override Type ComponentType
        {
            get { return typeof(MySqlConnectionStringBuilder); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(MySqlConnectionProtocol); }
        }

        public ConnectionProtocolDescriptor(string name, Attribute[] attr) : base(name, attr)
        {
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            MySqlConnectionStringBuilder cb = (MySqlConnectionStringBuilder)component;
            return cb.ConnectionProtocol;
        }

        public override void ResetValue(object component)
        {
            MySqlConnectionStringBuilder cb = (MySqlConnectionStringBuilder)component;
            cb.ConnectionProtocol = MySqlConnectionProtocol.Sockets;
        }

        public override void SetValue(object component, object value)
        {
            MySqlConnectionStringBuilder cb = (MySqlConnectionStringBuilder)component;
            cb.ConnectionProtocol = (MySqlConnectionProtocol)value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            MySqlConnectionStringBuilder cb = (MySqlConnectionStringBuilder)component;
            return cb.ConnectionProtocol != MySqlConnectionProtocol.Sockets;
        }
    }

    enum Keyword
    {
        UserID,
        Password,
        Server,
        Port,
        UseUsageAdvisor,
        CharacterSet,
        Compress,
        PipeName,
        Logging,
        OldSyntax,
        SharedMemoryName,
        AllowBatch,
        ConvertZeroDatetime,
        PersistSecurityInfo,
        Database,
        ConnectionTimeout,
        Pooling,
        MinimumPoolSize,
        MaximumPoolSize,
        ConnectionLifetime,
        DriverType,
        Protocol,
        ConnectionReset,
        AllowZeroDatetime,
        UsePerformanceMonitor,
        ProcedureCacheSize,
        IgnorePrepare,
        UseSSL,
        UseProcedureBodies,
        AutoEnlist,
        RespectBinaryFlags,
        TreatBlobsAsUTF8,
        BlobAsUTF8IncludePattern,
        BlobAsUTF8ExcludePattern,
        DefaultCommandTimeout,
        TreatTinyAsBoolean,
        AllowUserVariables,
        InteractiveSession,
        FunctionsReturnString
    }
}