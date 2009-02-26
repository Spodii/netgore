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
        readonly string _connectionString = new MySqlConnectionStringBuilder { Server = "localhost", UserID = "root", Database = "demogame", Password = "" }.ToString();

        MySqlDbConnectionPool CreateConnectionPool()
        {
            return new MySqlDbConnectionPool(_connectionString);
        }

        [Test]
        public void ConnectionOpenTest()
        {
            var pool = CreateConnectionPool();

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
            var pool = CreateConnectionPool();

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
            var pool = CreateConnectionPool();

            using (var a = pool.Create())
            {
                using (var b = pool.Create())
                {
                    using (var c = pool.Create())
                    {
                        Assert.IsNotNull(a);
                        Assert.IsNotNull(b);
                        Assert.IsNotNull(c);
                    }
                }
            }
        }

        [Test]
        public void SelectQueryTest()
        {
            var pool = CreateConnectionPool();

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
