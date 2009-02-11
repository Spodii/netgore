using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class CultureTests : BaseTest
    {
        #region Setup/Teardown

        public override void Setup()
        {
            base.Setup();
            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (id INT NOT NULL, name VARCHAR(250), PRIMARY KEY(id))");
        }

        #endregion

        void InternalTestFloats(bool prepared)
        {
            CultureInfo curCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo curUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo c = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = c;
            Thread.CurrentThread.CurrentUICulture = c;

            execSQL("DROP TABLE IF EXISTS Test");
            execSQL("CREATE TABLE Test (fl FLOAT, db DOUBLE, dec1 DECIMAL(5,2))");

            MySqlCommand cmd = new MySqlCommand("INSERT INTO Test VALUES (?fl, ?db, ?dec)", conn);
            cmd.Parameters.Add("?fl", MySqlDbType.Float);
            cmd.Parameters.Add("?db", MySqlDbType.Double);
            cmd.Parameters.Add("?dec", MySqlDbType.Decimal);
            cmd.Parameters[0].Value = 2.3;
            cmd.Parameters[1].Value = 4.6;
            cmd.Parameters[2].Value = 23.82;
            if (prepared)
                cmd.Prepare();
            int count = cmd.ExecuteNonQuery();
            Assert.AreEqual(1, count);

            MySqlDataReader reader = null;
            try
            {
                cmd.CommandText = "SELECT * FROM Test";
                if (prepared)
                    cmd.Prepare();
                reader = cmd.ExecuteReader();
                reader.Read();
                Assert.AreEqual(2.3, (decimal)reader.GetFloat(0));
                Assert.AreEqual(4.6, reader.GetDouble(1));
                Assert.AreEqual(23.82, reader.GetDecimal(2));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                Thread.CurrentThread.CurrentCulture = curCulture;
                Thread.CurrentThread.CurrentUICulture = curUICulture;
            }
        }

        /// <summary>
        /// Bug #29931  	Connector/NET does not handle Saudi Hijri calendar correctly
        /// </summary>
        [Test]
        public void ArabicCalendars()
        {
            execSQL("DROP TABLE IF EXISTS test");
            execSQL("CREATE TABLE test(dt DATETIME)");
            execSQL("INSERT INTO test VALUES ('2007-01-01 12:30:45')");

            CultureInfo curCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo curUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo c = new CultureInfo("ar-SA");
            Thread.CurrentThread.CurrentCulture = c;
            Thread.CurrentThread.CurrentUICulture = c;

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT dt FROM test", conn);
                DateTime dt = (DateTime)cmd.ExecuteScalar();
                Assert.AreEqual(2007, dt.Year);
                Assert.AreEqual(1, dt.Month);
                Assert.AreEqual(1, dt.Day);
                Assert.AreEqual(12, dt.Hour);
                Assert.AreEqual(30, dt.Minute);
                Assert.AreEqual(45, dt.Second);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Thread.CurrentThread.CurrentCulture = curCulture;
            Thread.CurrentThread.CurrentUICulture = curUICulture;
        }

        [Test]
        public void TestFloats()
        {
            InternalTestFloats(false);
        }

        [Test]
        public void TestFloatsPrepared()
        {
            if (version < new Version(4, 1))
                return;

            InternalTestFloats(true);
        }

        /// <summary>
        /// Bug #8228  	turkish character set causing the error
        /// </summary>
        [Test]
        public void Turkish()
        {
            CultureInfo curCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo curUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo c = new CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentCulture = c;
            Thread.CurrentThread.CurrentUICulture = c;

            try
            {
                MySqlConnection newConn = new MySqlConnection(GetConnectionString(true));
                newConn.Open();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Thread.CurrentThread.CurrentCulture = curCulture;
            Thread.CurrentThread.CurrentUICulture = curUICulture;
        }
    }
}