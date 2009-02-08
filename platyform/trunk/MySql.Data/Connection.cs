using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Threading;
using System.Transactions;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;
using IsolationLevel=System.Data.IsolationLevel;

namespace MySql.Data.MySqlClient
{
    /// <include file='docs/MySqlConnection.xml' path='docs/ClassSummary/*'/>
    [ToolboxBitmap(typeof(MySqlConnection), "MySqlClient.resources.connection.bmp")]
    [DesignerCategory("Code")]
    [ToolboxItem(true)]
    public sealed class MySqlConnection : DbConnection, ICloneable
    {
        static readonly Cache<string, MySqlConnectionStringBuilder> connectionStringCache =
            new Cache<string, MySqlConnectionStringBuilder>(0, 25);

        static int ia = 0;

        internal ConnectionState connectionState;
        internal Driver driver;
        readonly UsageAdvisor advisor;
        readonly object readerSync = new object();
        string database;
        MySqlDataReader dataReader;
        bool hasBeenOpen;
        bool isExecutingBuggyQuery;
        bool isReaderFree = true;
        PerformanceMonitor perfMonitor;
        ProcedureCache procedureCache;
        SchemaProvider schemaProvider;
        MySqlConnectionStringBuilder settings;

        /// <include file='docs/MySqlConnection.xml' path='docs/InfoMessage/*'/>
        public event MySqlInfoMessageEventHandler InfoMessage;

        /// <include file='docs/MySqlConnection.xml' path='docs/ConnectionString/*'/>
        [Editor("MySql.Data.MySqlClient.Design.ConnectionStringTypeEditor,MySqlClient.Design", typeof(UITypeEditor))]
        [Browsable(true)]
        [Category("Data")]
        [Description("Information used to connect to a DataSource, such as 'Server=xxx;UserId=yyy;Password=zzz;Database=dbdb'.")]
        public override string ConnectionString
        {
            get
            {
                // Always return exactly what the user set.
                // Security-sensitive information may be removed.
                return settings.GetConnectionString(!hasBeenOpen || settings.PersistSecurityInfo);
            }
            set
            {
                if (State != ConnectionState.Closed)
                    throw new MySqlException(
                        "Not allowed to change the 'ConnectionString' property while the connection (state=" + State + ").");

                MySqlConnectionStringBuilder newSettings;
                lock (connectionStringCache)
                {
// ReSharper disable ConditionIsAlwaysTrueOrFalse
                    if (value == null)
                    {
// ReSharper restore ConditionIsAlwaysTrueOrFalse
                        newSettings = new MySqlConnectionStringBuilder();
                    }
                    else
                    {
                        newSettings = connectionStringCache[value];
                        if (null == newSettings)
                        {
                            newSettings = new MySqlConnectionStringBuilder(value);
                            connectionStringCache.Add(value, newSettings);
                        }
                    }
                }

                settings = newSettings;

                if (!string.IsNullOrEmpty(settings.Database))
                    database = settings.Database;

                if (driver != null)
                    driver.Settings = newSettings;
            }
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/ConnectionTimeout/*'/>
        [Browsable(true)]
        public override int ConnectionTimeout
        {
            get { return (int)settings.ConnectionTimeout; }
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/Database/*'/>
        [Browsable(true)]
        public override string Database
        {
            get { return database; }
        }

        /// <summary>
        /// Gets the name of the MySQL server to which to connect.
        /// </summary>
        [Browsable(true)]
        public override string DataSource
        {
            get { return settings.Server; }
        }

        internal Encoding Encoding
        {
            get
            {
                if (driver == null)
                    return Encoding.Default;
                else
                    return driver.Encoding;
            }
        }

        internal bool IsExecutingBuggyQuery
        {
            get { return isExecutingBuggyQuery; }
            set { isExecutingBuggyQuery = value; }
        }

        internal PerformanceMonitor PerfMonitor
        {
            get { return perfMonitor; }
        }

        internal ProcedureCache ProcedureCache
        {
            get { return procedureCache; }
        }

        internal MySqlDataReader Reader
        {
            get { return dataReader; }
            set { dataReader = value; }
        }

        /// <summary>
        /// Returns the id of the server thread this connection is executing on
        /// </summary>
        [Browsable(false)]
        public int ServerThread
        {
            get { return driver.ThreadID; }
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/ServerVersion/*'/>
        [Browsable(false)]
        public override string ServerVersion
        {
            get { return driver.Version.ToString(); }
        }

        internal MySqlConnectionStringBuilder Settings
        {
            get { return settings; }
        }

        internal bool SoftClosed
        {
            get { return (State == ConnectionState.Closed) && driver != null && driver.CurrentTransaction != null; }
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/State/*'/>
        [Browsable(false)]
        public override ConnectionState State
        {
            get { return connectionState; }
        }

        [Browsable(false)]
        internal UsageAdvisor UsageAdvisor
        {
            get { return advisor; }
        }

        /// <summary>
        /// Indicates if this connection should use compression when communicating with the server.
        /// </summary>
        [Browsable(false)]
        public bool UseCompression
        {
            get { return settings.UseCompression; }
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/DefaultCtor/*'/>
        public MySqlConnection()
        {
            settings = new MySqlConnectionStringBuilder();
            advisor = new UsageAdvisor(this);
            database = String.Empty;
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/Ctor1/*'/>
        public MySqlConnection(string connectionString) : this()
        {
            ConnectionString = connectionString;
        }

        internal void Abort()
        {
            try
            {
                if (settings.Pooling)
                    MySqlPoolManager.ReleaseConnection(driver);
                else
                    driver.Close();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
            }
            SetState(ConnectionState.Closed, true);
        }

        ///<summary>
        ///
        ///                    Starts a database transaction.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    An object representing the new transaction.
        ///                
        ///</returns>
        ///
        ///<param name="isolationLevel">
        ///                    Specifies the isolation level for the transaction.
        ///                </param>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            if (isolationLevel == IsolationLevel.Unspecified)
                return BeginTransaction();
            return BeginTransaction(isolationLevel);
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/BeginTransaction/*'/>
        public new MySqlTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.RepeatableRead);
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/BeginTransaction1/*'/>
        public new MySqlTransaction BeginTransaction(IsolationLevel iso)
        {
            if (State != ConnectionState.Open)
                throw new InvalidOperationException(Resources.ConnectionNotOpen);

            // First check to see if we are in a current transaction
            if ((driver.ServerStatus & ServerStatusFlags.InTransaction) != 0)
                throw new InvalidOperationException(Resources.NoNestedTransactions);

            MySqlTransaction t = new MySqlTransaction(this, iso);

            MySqlCommand cmd = new MySqlCommand("", this) { CommandText = "SET SESSION TRANSACTION ISOLATION LEVEL " };

            switch (iso)
            {
                case IsolationLevel.ReadCommitted:
                    cmd.CommandText += "READ COMMITTED";
                    break;
                case IsolationLevel.ReadUncommitted:
                    cmd.CommandText += "READ UNCOMMITTED";
                    break;
                case IsolationLevel.RepeatableRead:
                    cmd.CommandText += "REPEATABLE READ";
                    break;
                case IsolationLevel.Serializable:
                    cmd.CommandText += "SERIALIZABLE";
                    break;
                case IsolationLevel.Chaos:
                    throw new NotSupportedException(Resources.ChaosNotSupported);
            }

            cmd.ExecuteNonQuery();

            cmd.CommandText = "BEGIN";
            cmd.ExecuteNonQuery();

            return t;
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/ChangeDatabase/*'/>
        public override void ChangeDatabase(string databaseName)
        {
            if (databaseName == null || databaseName.Trim().Length == 0)
                throw new ArgumentException(Resources.ParameterIsInvalid, "databaseName");

            if (State != ConnectionState.Open)
                throw new InvalidOperationException(Resources.ConnectionNotOpen);

            driver.SetDatabase(databaseName);
            database = databaseName;
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/ClearAllPools/*'/>
        public static void ClearAllPools()
        {
            MySqlPoolManager.ClearAllPools();
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/ClearPool/*'/>
        public static void ClearPool(MySqlConnection connection)
        {
            MySqlPoolManager.ClearPool(connection.Settings);
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/Close/*'/>
        public override void Close()
        {
            if (State == ConnectionState.Closed)
                return;

            if (dataReader != null)
                dataReader.Close();

            // if the reader was opened with CloseConnection then driver
            // will be null on the second time through
            if (driver != null)
            {
                if (driver.CurrentTransaction == null)
                    CloseFully();
                else
                    driver.IsInActiveUse = false;
            }

            SetState(ConnectionState.Closed, true);
        }

        internal void CloseFully()
        {
            if (settings.Pooling && driver.IsOpen)
            {
                // if we are in a transaction, roll it back
                if ((driver.ServerStatus & ServerStatusFlags.InTransaction) != 0)
                {
                    MySqlTransaction t = new MySqlTransaction(this, IsolationLevel.Unspecified);
                    t.Rollback();
                }

                MySqlPoolManager.ReleaseConnection(driver);
            }
            else
                driver.Close();
            driver = null;
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/CreateCommand/*'/>
        public new MySqlCommand CreateCommand()
        {
            // Return a new instance of a command object.
            MySqlCommand c = new MySqlCommand { Connection = this };
            return c;
        }

        ///<summary>
        ///
        ///                    Creates and returns a <see cref="T:System.Data.Common.DbCommand" /> object associated with the current connection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    A <see cref="T:System.Data.Common.DbCommand" /> object.
        ///                
        ///</returns>
        ///
        protected override DbCommand CreateDbCommand()
        {
            return CreateCommand();
        }

        internal string CurrentDatabase()
        {
            if (!string.IsNullOrEmpty(Database))
                return Database;
            MySqlCommand cmd = new MySqlCommand("SELECT database()", this);
            return cmd.ExecuteScalar().ToString();
        }

        ///<summary>
        ///
        ///                    Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally releases the managed resources.
        ///                
        ///</summary>
        ///
        ///<param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        ///                </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && State == ConnectionState.Open)
                Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Enlists in the specified transaction. 
        /// </summary>
        /// <param name="transaction">
        /// A reference to an existing <see cref="System.Transactions.Transaction"/> in which to enlist.
        /// </param>
        public override void EnlistTransaction(Transaction transaction)
        {
            // enlisting in the null transaction is a noop
            if (transaction == null)
                return;

            // guard against trying to enlist in more than one transaction
            if (driver.CurrentTransaction != null)
            {
                if (driver.CurrentTransaction.BaseTransaction == transaction)
                    return;

                throw new MySqlException("Already enlisted");
            }

            // now see if we need to swap out drivers.  We would need to do this since
            // we have to make sure all ops for a given transaction are done on the
            // same physical connection.
            Driver existingDriver = DriverTransactionManager.GetDriverInTransaction(transaction);
            if (existingDriver != null)
            {
                // we can't allow more than one driver to contribute to the same connection
                if (existingDriver.IsInActiveUse)
                    throw new NotSupportedException(Resources.MultipleConnectionsInTransactionNotSupported);

                // there is an existing driver and it's not being currently used.
                // now we need to see if it is using the same connection string
                string text1 = existingDriver.Settings.GetConnectionString(true);
                string text2 = Settings.GetConnectionString(true);
                if (String.Compare(text1, text2, true) != 0)
                    throw new NotSupportedException(Resources.MultipleConnectionsInTransactionNotSupported);

                // close existing driver
                // set this new driver as our existing driver
                CloseFully();
                driver = existingDriver;
            }

            if (driver.CurrentTransaction == null)
            {
                MySqlPromotableTransaction t = new MySqlPromotableTransaction(this, transaction);
                if (!transaction.EnlistPromotableSinglePhase(t))
                    throw new NotSupportedException(Resources.DistributedTxnNotSupported);

                driver.CurrentTransaction = t;
                DriverTransactionManager.SetDriverInTransaction(driver);
                driver.IsInActiveUse = true;
            }
        }

        internal void EnsureReleaseReader(MySqlDataReader obj)
        {
            lock (readerSync)
            {
                if (dataReader == null || ReferenceEquals(obj, dataReader))
                    isReaderFree = true;
                else
                {
                }
            }
        }

        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/>. 
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that contains schema information. </returns>
        public override DataTable GetSchema()
        {
            return GetSchema(null);
        }

        /// <summary>
        /// Returns schema information for the data source of this 
        /// <see cref="DbConnection"/> using the specified string for the schema name. 
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return. </param>
        /// <returns>A <see cref="DataTable"/> that contains schema information. </returns>
        public override DataTable GetSchema(string collectionName)
        {
            if (collectionName == null)
                collectionName = SchemaProvider.MetaCollection;

// ReSharper disable AssignNullToNotNullAttribute
            return GetSchema(collectionName, null);
// ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Returns schema information for the data source of this <see cref="DbConnection"/>
        /// using the specified string for the schema name and the specified string array 
        /// for the restriction values. 
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return.</param>
        /// <param name="restrictionValues">Specifies a set of restriction values for the requested schema.</param>
        /// <returns>A <see cref="DataTable"/> that contains schema information.</returns>
        public override DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            if (collectionName == null)
                collectionName = SchemaProvider.MetaCollection;

            string[] restrictions = null;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (restrictionValues != null)
// ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                restrictions = (string[])restrictionValues.Clone();

                for (int x = 0; x < restrictions.Length; x++)
                {
                    string s = restrictions[x];
                    if (s != null)
                    {
                        if (s.StartsWith("`"))
                            s = s.Substring(1);
                        if (s.EndsWith("`"))
                            s = s.Substring(0, s.Length - 1);
                        restrictions[x] = s;
                    }
                }
            }

            DataTable dt = schemaProvider.GetSchema(collectionName, restrictions);
            return dt;
        }

        internal void OnInfoMessage(MySqlInfoMessageEventArgs args)
        {
            if (InfoMessage != null)
                InfoMessage(this, args);
        }

        /// <include file='docs/MySqlConnection.xml' path='docs/Open/*'/>
        public override void Open()
        {
            if (State == ConnectionState.Open)
                throw new InvalidOperationException(Resources.ConnectionAlreadyOpen);

            SetState(ConnectionState.Connecting, true);

            // if we are auto enlisting in a current transaction, then we will be
            // treating the connection as pooled
            if (settings.AutoEnlist && Transaction.Current != null)
            {
                driver = DriverTransactionManager.GetDriverInTransaction(Transaction.Current);
                if (driver != null && (driver.IsInActiveUse || !driver.Settings.EquivalentTo(Settings)))
                    throw new NotSupportedException(Resources.MultipleConnectionsInTransactionNotSupported);
            }

            try
            {
                if (settings.Pooling)
                {
                    MySqlPool pool = MySqlPoolManager.GetPool(settings);
                    if (driver == null)
                        driver = pool.GetConnection();
                    procedureCache = pool.ProcedureCache;
                }
                else
                {
                    if (driver == null)
                        driver = Driver.Create(settings);
                    procedureCache = new ProcedureCache((int)settings.ProcedureCacheSize);
                }
            }
            catch (Exception)
            {
                SetState(ConnectionState.Closed, true);
                throw;
            }

            // if the user is using old syntax, let them know
#pragma warning disable 618,612
            if (driver.Settings.UseOldSyntax)
            {
#pragma warning restore 618,612
                Logger.LogWarning("You are using old syntax that will be removed in future versions");
            }

            SetState(ConnectionState.Open, false);
            driver.Configure(this);
            if (!string.IsNullOrEmpty(settings.Database))
                ChangeDatabase(settings.Database);

            // setup our schema provider
            if (driver.Version.isAtLeast(5, 0, 0))
                schemaProvider = new ISSchemaProvider(this);
            else
                schemaProvider = new SchemaProvider(this);

            perfMonitor = new PerformanceMonitor(this);

            // if we are opening up inside a current transaction, then autoenlist
            if (Transaction.Current != null && settings.AutoEnlist)
                EnlistTransaction(Transaction.Current);

            hasBeenOpen = true;
            SetState(ConnectionState.Open, true);
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns></returns>
        public bool Ping()
        {
            if (driver != null && driver.Ping())
                return true;
            driver = null;
            SetState(ConnectionState.Closed, true);
            return false;
        }

        internal void ReleaseReader(MySqlDataReader obj)
        {
            lock (readerSync)
            {
                if (ReferenceEquals(obj, dataReader))
                {
                    if (isReaderFree)
                        throw new Exception("Oh shit, the reader was already free! Who freed it!? I kill you!");

                    isReaderFree = true;
                }
                else if (dataReader != null)
                    throw new Exception("Wrong object trying to release the reader!");
                else if (dataReader == null)
                    isReaderFree = true;
            }
        }

        internal void ReserveReader()
        {
            while (true)
            {
                lock (readerSync)
                {
                    if (isReaderFree)
                    {
                        ia++;
                        isReaderFree = false;
                        return;
                    }
                }
                Thread.Sleep(1);
            }
        }

        internal void SetState(ConnectionState newConnectionState, bool broadcast)
        {
            if (newConnectionState == connectionState && !broadcast)
                return;
            ConnectionState oldConnectionState = connectionState;
            connectionState = newConnectionState;
            if (broadcast)
                OnStateChange(new StateChangeEventArgs(oldConnectionState, connectionState));
        }

        #region ICloneable Members

        /// <summary>
        /// Creates a new MySqlConnection object with the exact same ConnectionString value
        /// </summary>
        /// <returns>A cloned MySqlConnection object</returns>
        object ICloneable.Clone()
        {
            MySqlConnection clone = new MySqlConnection();
            string connectionString = settings.GetConnectionString(true);
            if (connectionString != null)
                clone.ConnectionString = connectionString;
            return clone;
        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="MySqlConnection.InfoMessage"/> event of a 
    /// <see cref="MySqlConnection"/>.
    /// </summary>
    public delegate void MySqlInfoMessageEventHandler(object sender, MySqlInfoMessageEventArgs args);

    /// <summary>
    /// Provides data for the InfoMessage event. This class cannot be inherited.
    /// </summary>
    public class MySqlInfoMessageEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public MySqlError[] errors;
    }
}