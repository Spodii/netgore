using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class ExceptionTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(100))");
        }

        #endregion

        /// <summary>
        /// Bug #27436 Add the MySqlException.Number property value to the Exception.Data Dictionary  
        /// </summary>
        [Test]
        public void ErrorData()
        {
            MySqlCommand cmd = new MySqlCommand("SELEDT 1", conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(1064, ex.Data["Server Error Code"]);
            }
        }

        [Test]
        public void Timeout()
        {
            for (int i = 1; i < 10; i++)
            {
                execSQL("INSERT INTO Test VALUES (" + i + ", 'This is a long text string that I am inserting')");
            }

            // we create a new connection so our base one is not closed
            MySqlConnection c2 = new MySqlConnection(conn.ConnectionString);
            c2.Open();

            KillConnection(c2);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test", c2);
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                reader.Read();
                reader.Close();
                Assert.Fail("We should not reach this code");
            }
            catch (Exception)
            {
                Assert.AreEqual(ConnectionState.Closed, c2.State);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                c2.Close();
            }
        }
    }
}