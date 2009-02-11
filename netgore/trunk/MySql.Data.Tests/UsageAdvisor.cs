using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class UsageAdvisorTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            createTable("CREATE TABLE Test (id int, name VARCHAR(200))", "INNODB");
        }

        #endregion

        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            csAdditions = ";Usage Advisor=true;";
            base.FixtureSetup();
        }

        [Test]
        public void NotReadingEveryField()
        {
            execSQL("INSERT INTO Test VALUES (1, 'Test1')");
            execSQL("INSERT INTO Test VALUES (2, 'Test2')");
            execSQL("INSERT INTO Test VALUES (3, 'Test3')");
            execSQL("INSERT INTO Test VALUES (4, 'Test4')");

            Trace.Listeners.Clear();
            GenericListener listener = new GenericListener();
            Trace.Listeners.Add(listener);

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test; SELECT * FROM Test WHERE id > 2", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                reader.GetInt32(0); // access  the first field
                reader.Read();

                Assert.IsTrue(reader.NextResult());
                Assert.IsTrue(listener.Find("Fields not accessed:  name") != 0);

                reader.Read();
                listener.Clear();

                Assert.AreEqual("Test3", reader.GetString(1));
                Assert.IsFalse(reader.NextResult());
                Assert.IsTrue(listener.Find("Fields not accessed:  id") > 0);
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
        public void NotReadingEveryRow()
        {
            execSQL("INSERT INTO Test VALUES (1, 'Test1')");
            execSQL("INSERT INTO Test VALUES (2, 'Test2')");
            execSQL("INSERT INTO Test VALUES (3, 'Test3')");
            execSQL("INSERT INTO Test VALUES (4, 'Test4')");

            Trace.Listeners.Clear();
            GenericListener listener = new GenericListener();
            Trace.Listeners.Add(listener);

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Test; SELECT * FROM Test WHERE id > 2", conn);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                reader.Read();
                reader.Read();

                listener.Clear();
                Assert.IsTrue(reader.NextResult());
                Assert.IsTrue(listener.Find("Reason: Not all rows in resultset were read.") > 0);

                reader.Read();
                reader.Read();
                listener.Clear();

                Assert.IsFalse(reader.NextResult());
                Assert.IsTrue(listener.Find("Reason: Not all rows in resultset were read.") > 0);
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
    }
}