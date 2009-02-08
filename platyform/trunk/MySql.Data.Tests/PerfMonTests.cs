using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    /// <summary>
    /// Summary description for StoredProcedure.
    /// </summary>
    [TestFixture]
    public class PerfMonTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test; CREATE TABLE Test (id INT, name VARCHAR(100))");
        }

        #endregion

        public override void FixtureSetup()
        {
            pooling = false;
            csAdditions = ";use performance monitor=true;";
            base.FixtureSetup();
        }

        /// <summary>
        /// This test doesn't work from the CI setup currently
        /// </summary>
        [Test]
        public void ProcedureFromCache()
        {
            /*
            if (version < new Version(5, 0)) return;

            execSQL("DROP PROCEDURE IF EXISTS spTest");
			execSQL("CREATE PROCEDURE spTest(id int) BEGIN END");

			PerformanceCounter hardQuery = new PerformanceCounter(
				 ".NET Data Provider for MySQL", "HardProcedureQueries", true);
			PerformanceCounter softQuery = new PerformanceCounter(
				 ".NET Data Provider for MySQL", "SoftProcedureQueries", true);
			long hardCount = hardQuery.RawValue;
			long softCount = softQuery.RawValue;

			MySqlCommand cmd = new MySqlCommand("spTest", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("?id", 1);
			cmd.ExecuteScalar();

			Assert.AreEqual(hardCount + 1, hardQuery.RawValue);
			Assert.AreEqual(softCount, softQuery.RawValue);
			hardCount = hardQuery.RawValue;

			MySqlCommand cmd2 = new MySqlCommand("spTest", conn);
			cmd2.CommandType = CommandType.StoredProcedure;
			cmd2.Parameters.AddWithValue("?id", 1);
			cmd2.ExecuteScalar();

			Assert.AreEqual(hardCount, hardQuery.RawValue);
			Assert.AreEqual(softCount + 1, softQuery.RawValue);
            */
        }
    }
}