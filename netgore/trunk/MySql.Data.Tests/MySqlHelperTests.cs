using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class MySqlHelperTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), PRIMARY KEY(id))");
        }

        #endregion

        /// <summary>
        /// Bug #11490  	certain incorrect queries trigger connection must be valid and open message
        /// </summary>
        [Test]
        public void Bug11490()
        {
            if (version < new Version(4, 1))
                return;

            MySqlDataReader reader = null;

            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 254; i++)
                {
                    sb.Append('a');
                }
                string sql = "INSERT INTO Test (name) VALUES ('" + sb + "')";
                reader = MySqlHelper.ExecuteReader(GetConnectionString(true), sql);
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