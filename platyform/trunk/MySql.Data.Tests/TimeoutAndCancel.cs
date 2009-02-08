using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class TimeoutAndCancel : BaseTest
    {
        delegate void CommandInvokerDelegate(MySqlCommand cmdToRun);

        void CommandRunner(MySqlCommand cmdToRun)
        {
            try
            {
                object o = cmdToRun.ExecuteScalar();
                Assert.IsNull(o);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        int stateChangeCount;

        void c_StateChange(object sender, StateChangeEventArgs e)
        {
            stateChangeCount++;
        }

/*        [Test]
        public void TimeoutExpiring()
        {
            if (version < new Version(5, 0)) return;

            // first we need a routine that will run for a bit
            execSQL(@"CREATE PROCEDURE spTest(duration INT) 
                BEGIN 
                    SELECT SLEEP(duration);
                END");

            DateTime start = DateTime.Now;
            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.Parameters.AddWithValue("duration", 60);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5;
                cmd.ExecuteNonQuery();
                Assert.Fail("Should not get to this point");
            }
            catch (MySqlException ex)
            {
                TimeSpan ts = DateTime.Now.Subtract(start);
                Assert.IsTrue(ts.TotalSeconds <= 10);
                Assert.IsTrue(ex.Message.StartsWith("Timeout expired"), "Message is wrong");
            }
        }
        */

        [Test]
        public void CancelSelect()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE TABLE Test (id INT AUTO_INCREMENT PRIMARY KEY, name VARCHAR(20))");
            for (int i = 0; i < 10000; i++)
            {
                execSQL("INSERT INTO Test VALUES (NULL, 'my string')");
            }

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", conn);
            cmd.CommandTimeout = 0;
            int rows = 0;
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();

                cmd.Cancel();

                try
                {
                    while (reader.Read())
                    {
                        rows++;
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
            }
            Assert.IsTrue(rows < 10000);
        }

        [Test]
        public void CancelSingleQuery()
        {
            if (version < new Version(5, 0))
                return;

            // first we need a routine that will run for a bit
            execSQL(
                @"CREATE PROCEDURE spTest(duration INT) 
                BEGIN 
                    SELECT SLEEP(duration);
                END");

            MySqlCommand cmd = new MySqlCommand("spTest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("duration", 60);

            // now we start execution of the command
            CommandInvokerDelegate d = CommandRunner;
            d.BeginInvoke(cmd, null, null);

            // sleep 5 seconds
            Thread.Sleep(5000);

            // now cancel the command
            cmd.Cancel();
        }

        /// <summary>
        /// Bug #40091	mysql driver 5.2.3.0 connection pooling issue
        /// </summary>
        [Test]
        public void ConnectionStringModifiedAfterCancel()
        {
            if (version < new Version(5, 0))
                return;

            bool isPooling = pooling;
            pooling = true;
            string connStr = GetConnectionString(true);
            pooling = isPooling;
            connStr = connStr.Replace("persist security info=true", "persist security info=false");

            using (MySqlConnection c = new MySqlConnection(connStr))
            {
                c.Open();
                string connStr1 = c.ConnectionString;

                MySqlCommand cmd = new MySqlCommand("SELECT SLEEP(10)", c);
                cmd.CommandTimeout = 5;

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    string connStr2 = c.ConnectionString.ToLower(CultureInfo.InvariantCulture);
                    Assert.AreEqual(-1, connStr2.IndexOf("pooling=true"));
                    Assert.AreEqual(-1, connStr2.IndexOf("pooling=false"));
                    reader.Read();
                }
            }
        }

        [Test]
        public void TimeoutDuringBatch()
        {
            if (version < new Version(5, 0))
                return;

            execSQL(
                @"CREATE PROCEDURE spTest(duration INT) 
                BEGIN 
                    SELECT SLEEP(duration);
                END");

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (id INT)");

            MySqlCommand cmd = new MySqlCommand("call spTest(60);INSERT INTO test VALUES(4)", conn);
            cmd.CommandTimeout = 5;
            try
            {
                cmd.ExecuteNonQuery();
                Assert.Fail("This should have timed out");
            }
            catch (MySqlException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("Timeout expired"), "Message is wrong");
            }

            cmd.CommandText = "SELECT COUNT(*) FROM test";
            Assert.AreEqual(0, cmd.ExecuteScalar());
        }

        [Test]
        public void TimeoutNotExpiring()
        {
            if (version < new Version(5, 0))
                return;

            // first we need a routine that will run for a bit
            suExecSQL(
                @"CREATE PROCEDURE spTest(duration INT) 
                BEGIN 
                    SELECT SLEEP(duration);
                END");

            try
            {
                MySqlCommand cmd = new MySqlCommand("spTest", conn);
                cmd.Parameters.AddWithValue("duration", 10);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 15;
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void WaitTimeoutExpiring()
        {
            MySqlConnection c = new MySqlConnection(GetConnectionString(true));
            c.Open();
            c.StateChange += c_StateChange;

            // set the session wait timeout on this new connection
            MySqlCommand cmd = new MySqlCommand("SET SESSION interactive_timeout=10", c);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SET SESSION wait_timeout=10";
            cmd.ExecuteNonQuery();

            stateChangeCount = 0;
            // now wait 10 seconds
            Thread.Sleep(15000);

            try
            {
                cmd.CommandText = "SELECT now()";
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("Fatal"));
            }

            Assert.AreEqual(1, stateChangeCount);
            Assert.AreEqual(ConnectionState.Closed, c.State);

            c = new MySqlConnection(GetConnectionString(true));
            c.Open();
            cmd = new MySqlCommand("SELECT now() as thetime, database() as db", c);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                Assert.IsTrue(r.Read());
            }
        }
    }
}