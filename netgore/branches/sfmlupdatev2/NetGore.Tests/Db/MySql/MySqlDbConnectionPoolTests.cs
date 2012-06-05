using System.Data;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture]
    public class MySqlDbConnectionPoolTests
    {
        #region Unit tests

        [Test]
        public void ConnectionCloseTest()
        {
            using (var pool = DbManagerTestSettings.CreateConnectionPool())
            {
                IDbConnection conn;
                using (var connPool = pool.Acquire())
                {
                    conn = connPool.Connection;
                    connPool.QueryRunner.Flush();
                }
                Assert.AreEqual(ConnectionState.Closed, conn.State);
            }
        }

        [Test]
        public void ConnectionOpenTest()
        {
            using (var pool = DbManagerTestSettings.CreateConnectionPool())
            {
                using (var connPool = pool.Acquire())
                {
                    var conn = connPool.Connection;
                    Assert.IsNotNull(conn);
                    Assert.AreEqual(ConnectionState.Open, conn.State);
                }
            }
        }

        [Test]
        public void MultiplePoolItemsTest()
        {
            using (var pool = DbManagerTestSettings.CreateConnectionPool())
            {
                Assert.AreEqual(0, pool.LiveObjects);
                using (var a = pool.Acquire())
                {
                    Assert.AreEqual(1, pool.LiveObjects);
                    using (var b = pool.Acquire())
                    {
                        Assert.AreEqual(2, pool.LiveObjects);
                        using (var c = pool.Acquire())
                        {
                            Assert.AreEqual(3, pool.LiveObjects);
                            Assert.IsNotNull(a);
                            Assert.IsNotNull(b);
                            Assert.IsNotNull(c);
                        }
                        Assert.AreEqual(2, pool.LiveObjects);
                    }
                    Assert.AreEqual(1, pool.LiveObjects);
                }
                Assert.AreEqual(0, pool.LiveObjects);
            }
        }

        [Test]
        public void SelectQueryTest()
        {
            using (var pool = DbManagerTestSettings.CreateConnectionPool())
            {
                using (var connPool = pool.Acquire())
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

        #endregion
    }
}