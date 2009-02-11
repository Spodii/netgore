using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class AsyncTests : BaseTest
    {
        [Test]
        public void ExecuteNonQuery()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE TABLE test (id int)");
            execSQL("CREATE PROCEDURE spTest() BEGIN SET @x=0; REPEAT INSERT INTO test VALUES(@x); " +
                    "SET @x=@x+1; UNTIL @x = 300 END REPEAT; END");

            try
            {
                MySqlCommand proc = new MySqlCommand("spTest", conn) { CommandType = CommandType.StoredProcedure };
                IAsyncResult iar = proc.BeginExecuteNonQuery();
                int count = 0;
                while (!iar.IsCompleted)
                {
                    count++;
                    Thread.Sleep(20);
                }
                proc.EndExecuteNonQuery(iar);
                Assert.IsTrue(count > 0);

                proc.CommandType = CommandType.Text;
                proc.CommandText = "SELECT COUNT(*) FROM test";
                object cnt = proc.ExecuteScalar();
                Assert.AreEqual(300, cnt);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ExecuteReader()
        {
            if (version < new Version(5, 0))
                return;

            execSQL("CREATE TABLE test (id int)");
            execSQL("CREATE PROCEDURE spTest() BEGIN INSERT INTO test VALUES(1); " + "SELECT SLEEP(2); SELECT 'done'; END");

            MySqlDataReader reader = null;
            try
            {
                MySqlCommand proc = new MySqlCommand("spTest", conn) { CommandType = CommandType.StoredProcedure };
                IAsyncResult iar = proc.BeginExecuteReader();
                int count = 0;
                while (!iar.IsCompleted)
                {
                    count++;
                    Thread.Sleep(20);
                }

                reader = proc.EndExecuteReader(iar);
                Assert.IsNotNull(reader);
                Assert.IsTrue(count > 0, "count > 0");
                Assert.IsTrue(reader.Read(), "can read");
                Assert.IsTrue(reader.NextResult());
                Assert.IsTrue(reader.Read());
                Assert.AreEqual("done", reader.GetString(0));
                reader.Close();

                proc.CommandType = CommandType.Text;
                proc.CommandText = "SELECT COUNT(*) FROM test";
                object cnt = proc.ExecuteScalar();
                Assert.AreEqual(1, cnt);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        [Test]
        public void ThrowingExceptions()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT xxx", conn);
            IAsyncResult r = cmd.BeginExecuteReader();
            try
            {
                MySqlDataReader reader = cmd.EndExecuteReader(r);
                if (reader != null)
                    reader.Close();
                Assert.Fail("EndExecuteReader should have thrown an exception");
            }
            catch (MySqlException)
            {
            }
        }
    }
}