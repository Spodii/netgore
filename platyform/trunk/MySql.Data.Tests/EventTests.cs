using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class EventTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), PRIMARY KEY(id))");
        }

        #endregion

        static void WarningsInfoMessage(object sender, MySqlInfoMessageEventArgs args)
        {
            Assert.AreEqual(1, args.errors.Length);
        }

        static void StateChangeHandler(object sender, StateChangeEventArgs e)
        {
        }

        [Test]
        public void StateChange()
        {
            MySqlConnection c = new MySqlConnection(GetConnectionString(true));
            c.StateChange += StateChangeHandler;
            c.Open();
            c.Close();
        }

        [Test]
        public void Warnings()
        {
            if (version < new Version(4, 1))
                return;

            conn.InfoMessage += WarningsInfoMessage;

            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test (name VARCHAR(10))");

            MySqlCommand cmd = new MySqlCommand("INSERT INTO test VALUES ('12345678901')", conn);
            MySqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
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