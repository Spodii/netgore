using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace MySql.Data.MySqlClient.Tests
{
    [TestFixture]
    public class ConnectionStringBuilder : BaseTest
    {
        /// <summary>
        /// Bug #37955 Connector/NET keeps adding the same option to the connection string
        /// </summary>
        [Test]
        public void SettingValueMultipeTimes()
        {
            MySqlConnectionStringBuilder s = new MySqlConnectionStringBuilder();
            s["database"] = "test";
            s["database"] = "test2";
            Assert.AreEqual("database=test2", s.ConnectionString);
        }

        [Test]
        public void Simple()
        {
            MySqlConnectionStringBuilder sb = null;
            try
            {
                sb = new MySqlConnectionStringBuilder
                     {
                         ConnectionString =
                             ("server=localhost;uid=reggie;pwd=pass;port=1111;" +
                              "connection timeout=23; pooling=true; min pool size=33; " + "max pool size=66")
                     };
            }
            catch (ArgumentException ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.AreEqual("localhost", sb.Server);
            Assert.AreEqual("reggie", sb.UserID);
            Assert.AreEqual("pass", sb.Password);
            Assert.AreEqual(1111, sb.Port);
            Assert.AreEqual(23, sb.ConnectionTimeout);
            Assert.IsTrue(sb.Pooling);
            Assert.AreEqual(33, sb.MinimumPoolSize);
            Assert.AreEqual(66, sb.MaximumPoolSize);

            try
            {
                sb.ConnectionString = "server=localhost;badkey=badvalue";
                Assert.Fail("This should not work");
            }
            catch (ArgumentException)
            {
            }
            catch (Exception)
            {
                Assert.Fail("Wrong exception type");
            }

            sb.Clear();
            Assert.AreEqual(15, sb.ConnectionTimeout);
            Assert.AreEqual(true, sb.Pooling);
            Assert.AreEqual(3306, sb.Port);
            Assert.AreEqual(String.Empty, sb.Server);
            Assert.AreEqual(false, sb.PersistSecurityInfo);
            Assert.AreEqual(0, sb.ConnectionLifeTime);
            Assert.IsFalse(sb.ConnectionReset);
            Assert.AreEqual(0, sb.MinimumPoolSize);
            Assert.AreEqual(100, sb.MaximumPoolSize);
            Assert.AreEqual(String.Empty, sb.UserID);
            Assert.AreEqual(String.Empty, sb.Password);
            Assert.AreEqual(false, sb.UseUsageAdvisor);
            Assert.AreEqual(String.Empty, sb.CharacterSet);
            Assert.AreEqual(false, sb.UseCompression);
            Assert.AreEqual("MYSQL", sb.PipeName);
            Assert.IsFalse(sb.Logging);
#pragma warning disable 618,612
            Assert.IsFalse(sb.UseOldSyntax);
#pragma warning restore 618,612
            Assert.IsTrue(sb.AllowBatch);
            Assert.IsFalse(sb.ConvertZeroDateTime);
            Assert.AreEqual("MYSQL", sb.SharedMemoryName);
            Assert.AreEqual(String.Empty, sb.Database);
            Assert.AreEqual(MySqlDriverType.Native, sb.DriverType);
            Assert.AreEqual(MySqlConnectionProtocol.Sockets, sb.ConnectionProtocol);
            Assert.IsFalse(sb.AllowZeroDateTime);
            Assert.IsFalse(sb.UsePerformanceMonitor);
            Assert.AreEqual(25, sb.ProcedureCacheSize);
        }
    }
}