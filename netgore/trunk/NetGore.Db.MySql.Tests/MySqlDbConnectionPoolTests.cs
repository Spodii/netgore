using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace NetGore.Db.MySql.Tests
{
    [TestFixture]
    public class MySqlDbConnectionPoolTests
    {
        [Test]
        public void ConnectionOpenTest()
        {
            var pool = TestSettings.CreateConnectionPool();

            using (var connPool = pool.Create())
            {
                var conn = connPool.Connection;
                Assert.IsNotNull(conn);
                Assert.AreEqual(ConnectionState.Open, conn.State);
            }
        }

        [Test]
        public void ConnectionCloseTest()
        {
            var pool = TestSettings.CreateConnectionPool();

            IDbConnection conn;
            using (var connPool = pool.Create())
            {
                conn = connPool.Connection;
            }
            Assert.AreEqual(ConnectionState.Closed, conn.State);
        }

        [Test]
        public void MultiplePoolItemsTest()
        {
            var pool = TestSettings.CreateConnectionPool();

            Assert.AreEqual(0, pool.Count);
            using (var a = pool.Create())
            {
                Assert.AreEqual(1, pool.Count);
                using (var b = pool.Create())
                {
                    Assert.AreEqual(2, pool.Count);
                    using (var c = pool.Create())
                    {
                        Assert.AreEqual(3, pool.Count);
                        Assert.IsNotNull(a);
                        Assert.IsNotNull(b);
                        Assert.IsNotNull(c);
                    }
                    Assert.AreEqual(2, pool.Count);
                }
                Assert.AreEqual(1, pool.Count);
            }
            Assert.AreEqual(0, pool.Count);
        }

        [Test]
        public void SelectQueryTest()
        {
            var pool = TestSettings.CreateConnectionPool();

            using (var connPool = pool.Create())
            {
                using (var cmd = connPool.Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 100 + 500";
                    using (var r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual(600, r[0]);
                    }
                }
            }
        }
    }
}
