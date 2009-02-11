using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    /// <summary>
    /// Summary description for BaseTest.
    /// </summary>
    public class BaseTest
    {
        protected static string database0;
        protected static string database1;
        protected static string host;
        protected static int maxPacketSize;
        protected static string memoryName;
        protected static string password;
        protected static string pipeName;
        protected static int port;
        protected static string rootPassword;
        protected static string rootUser;
        protected static string user;
        protected MySqlConnection conn;
        protected string csAdditions = String.Empty;
        protected bool pooling;
        protected MySqlConnection rootConn;
        protected string table;
        protected Version version;

        protected Version Version
        {
            get { return version; }
        }

        public BaseTest()
        {
            if (host == null)
            {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
                LoadStaticConfiguration();
            }
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected void Close()
        {
            try
            {
                // delete the table we created.
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                execSQL("DROP TABLE IF EXISTS Test");
                conn.Close();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        protected int CountProcesses()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SHOW PROCESSLIST", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows.Count;
        }

        protected void createTable(string sql, string engine)
        {
            if (Version >= new Version(4, 1))
                sql += " ENGINE=" + engine;
            else
                sql += " TYPE=" + engine;
            execSQL(sql);
        }

        protected IDataReader execReader(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            return cmd.ExecuteReader();
        }

        protected void execSQL(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        protected DataTable FillTable(string sql)
        {
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            // open up a root connection
            string connStr =
                String.Format(
                    "server={0};user id={1};password={2};database=mysql;" + "persist security info=true;pooling=false;", host,
                    rootUser, rootPassword);
            connStr += GetConnectionInfo();
            rootConn = new MySqlConnection(connStr);
            rootConn.Open();
        }

        protected virtual string GetConnectionInfo()
        {
            return String.Format("protocol=sockets;port={0};use procedure bodies=false", port);
        }

        protected string GetConnectionString(bool includedb)
        {
            string connStr = String.Format("{0};{1}", GetConnectionStringBasic(includedb), csAdditions);
            return connStr;
        }

        protected string GetConnectionStringBasic(bool includedb)
        {
            string connStr =
                String.Format(
                    "server={0};user id={1};password={2};" +
                    "persist security info=true;connection reset=true;allow user variables=true;", host, user, password);
            if (includedb)
                connStr += String.Format("database={0};", database0);
            if (!pooling)
                connStr += ";pooling=false;";
            connStr += GetConnectionInfo();
            return connStr;
        }

        protected string GetConnectionStringEx(string puser, string pw, bool includedb)
        {
            string connStr = String.Format("server={0};user id={1};" + "persist security info=true;{2}", host, puser, csAdditions);
            if (includedb)
                connStr += String.Format("database={0};", database0);
            if (pw != null)
                connStr += String.Format("password={0};", pw);
            connStr += GetConnectionInfo();
            return connStr;
        }

        protected void KillConnection(MySqlConnection c)
        {
            int threadId = c.ServerThread;
            MySqlCommand cmd = new MySqlCommand("KILL " + threadId, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // the kill flag might need a little prodding to do its thing
            try
            {
                cmd.CommandText = "SELECT 1";
                cmd.Connection = c;
                cmd.ExecuteNonQuery();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
            {
            }

            // now wait till the process dies
            bool processStillAlive = false;
            while (true)
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SHOW PROCESSLIST", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Id"].Equals(threadId))
                        processStillAlive = true;
                }
                if (!processStillAlive)
                    break;
                Thread.Sleep(500);
            }
        }

        protected virtual void LoadStaticConfiguration()
        {
            Debug.Assert(host == null);

            user = "root";
            password = "test";
            port = 3306;
            rootUser = "root";
            rootPassword = "test";

            host = ConfigurationManager.AppSettings["host"];
            string strPort = ConfigurationManager.AppSettings["port"];
            pipeName = ConfigurationManager.AppSettings["pipename"];
            memoryName = ConfigurationManager.AppSettings["memory_name"];

            if (strPort != null)
                port = Int32.Parse(strPort);
            if (host == null)
                host = "localhost";
            if (pipeName == null)
                pipeName = "MYSQL";
            if (memoryName == null)
                memoryName = "MYSQL";

            // we don't use FileVersion because it's not available
            // on the compact framework
            if (database0 == null)
            {
                string fullname = Assembly.GetExecutingAssembly().FullName;
// ReSharper disable PossibleNullReferenceException
                var parts = fullname.Split(new char[] { '=' });
// ReSharper restore PossibleNullReferenceException
                var versionParts = parts[1].Split(new char[] { '.' });
                database0 = String.Format("db{0}{1}{2}-a", versionParts[0], versionParts[1], port - 3300);
                database1 = String.Format("db{0}{1}{2}-b", versionParts[0], versionParts[1], port - 3300);
            }
        }

        protected void Open()
        {
            try
            {
                string connString = GetConnectionString(true);
                conn = new MySqlConnection(connString);
                conn.Open();

                string ver = conn.ServerVersion;

                int x = 0;
                foreach (char c in ver)
                {
                    if (!Char.IsDigit(c) && c != '.')
                        break;
                    x++;
                }
                ver = ver.Substring(0, x);
                version = new Version(ver);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
        }

        protected void SetAccountPerms(bool includeProc)
        {
            // now allow our user to access them
            suExecSQL(String.Format(@"GRANT ALL ON `{0}`.* to 'test'@'localhost' 
				identified by 'test'", database0));
            suExecSQL(String.Format(@"GRANT ALL ON `{0}`.* to 'test'@'localhost' 
				identified by 'test'", database1));
            suExecSQL(String.Format(@"GRANT ALL ON `{0}`.* to 'test'@'%' 
				identified by 'test'", database0));
            suExecSQL(String.Format(@"GRANT ALL ON `{0}`.* to 'test'@'%' 
				identified by 'test'", database1));

            if (includeProc)
            {
                // now allow our user to access them
                suExecSQL(@"GRANT ALL ON mysql.proc to 'test'@'localhost' identified by 'test'");
                suExecSQL(@"GRANT ALL ON mysql.proc to 'test'@'%' identified by 'test'");
            }

            suExecSQL("FLUSH PRIVILEGES");
        }

        [SetUp]
        public virtual void Setup()
        {
            try
            {
                // now create our databases
                suExecSQL(String.Format("DROP DATABASE IF EXISTS `{0}`; CREATE DATABASE `{0}`", database0));
                suExecSQL(String.Format("DROP DATABASE IF EXISTS `{0}`; CREATE DATABASE `{0}`", database1));

                SetAccountPerms(false);

                rootConn.ChangeDatabase(database0);

                Open();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        protected void suExecSQL(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, rootConn);
            cmd.ExecuteNonQuery();
        }

        protected bool TableExists(string tableName)
        {
            var restrictions = new string[4];
            restrictions[2] = tableName;
            DataTable dt = conn.GetSchema("Tables", restrictions);
            return dt.Rows.Count > 0;
        }

        [TearDown]
        public virtual void Teardown()
        {
            suExecSQL(String.Format("DROP DATABASE IF EXISTS `{0}`", database0));
            suExecSQL(String.Format("DROP DATABASE IF EXISTS `{0}`", database1));
            Close();
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            rootConn.Close();
        }
    }
}