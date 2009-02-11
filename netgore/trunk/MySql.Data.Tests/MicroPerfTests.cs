using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class MicroPerfTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id int NOT NULL, name VARCHAR(100))");
        }

        #endregion

/*        [Test]
        public void Connect1000Times()
        {
            DateTime start = DateTime.Now;

            for (int i = 0; i < 1000; i++)
            {
                MySqlConnection c = new MySqlConnection(
                    base.GetConnectionString(true));
                c.Open();
                c.Close();
            }
        }*/
    }
}