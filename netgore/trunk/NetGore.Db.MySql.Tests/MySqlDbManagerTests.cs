using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace NetGore.Db.MySql.Tests
{
    [TestFixture]
    public class MySqlDbManagerTests
    {
        readonly string _connectionString = new MySqlConnectionStringBuilder { Server = "localhost", UserID = "root", Database = "demogame", Password = "" }.ToString();

        DbManager CreateDbManager()
        {
            return new DbManager(new MySqlDbConnectionPool(_connectionString));
        }

        [Test]
        public void ConnectionTest()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                IPoolableDbConnection pConn;
                IDbConnection conn;
                using (pConn = manager.GetConnection())
                {
                    conn = pConn.Connection;
                    Assert.IsNotNull(conn);
                    Assert.AreEqual(ConnectionState.Open, conn.State);
                }
                Assert.AreEqual(ConnectionState.Closed, conn.State);
            }
        }

        [Test]
        public void CommandTest()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                using (var cmd = manager.GetCommand())
                {
                    Assert.IsNotNull(cmd);
                    cmd.CommandText = "SELECT 500 + 100";
                    using (var r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual("600", r[0].ToString());
                    }
                }
            }
        }

        [Test]
        public void CommandStringTest()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                using (var cmd = manager.GetCommand("SELECT 500 + 100"))
                {
                    Assert.IsNotNull(cmd);
                    Assert.AreEqual("SELECT 500 + 100", cmd.CommandText);
                    using (var r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual("600", r[0].ToString());
                    }
                }
            }
        }

        [Test]
        public void ExecuteNonQueryTest()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                int ret = manager.ExecuteNonQuery("SELECT 500 + 100");
                Assert.AreEqual(-1, ret);
            }
        }

        [Test]
        public void ExecuteReaderTest()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                using (var r = manager.ExecuteReader("SELECT 500 + 100"))
                {
                    Assert.IsFalse(r.IsClosed);
                    r.Read();
                    Assert.AreEqual("600", r[0].ToString());
                }
            }
        }

        [Test]
        public void ExecuteReader2Test()
        {
            var manager = CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                using (var r = manager.ExecuteReader("SELECT 500 + 100", CommandBehavior.SingleResult))
                {
                    Assert.IsFalse(r.IsClosed);
                    r.Read();
                    Assert.AreEqual("600", r[0].ToString());
                }
            }
        }
    }
}
