using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
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
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            IDbConnection conn;
            using (PooledDbConnection connPool = pool.Acquire())
            {
                conn = connPool.Connection;
            }
            Assert.AreEqual(ConnectionState.Closed, conn.State);
        }

        [Test]
        public void ConnectionOpenTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            using (PooledDbConnection connPool = pool.Acquire())
            {
                DbConnection conn = connPool.Connection;
                Assert.IsNotNull(conn);
                Assert.AreEqual(ConnectionState.Open, conn.State);
            }
        }

        [Test]
        public void MultiplePoolItemsTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            Assert.AreEqual(0, pool.LiveObjects);
            using (PooledDbConnection a = pool.Acquire())
            {
                Assert.AreEqual(1, pool.LiveObjects);
                using (PooledDbConnection b = pool.Acquire())
                {
                    Assert.AreEqual(2, pool.LiveObjects);
                    using (PooledDbConnection c = pool.Acquire())
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

        [Test]
        public void SelectQueryTest()
        {
            DbConnectionPool pool = DbManagerTestSettings.CreateConnectionPool();

            using (PooledDbConnection connPool = pool.Acquire())
            {
                using (DbCommand cmd = connPool.Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT 100 + 500";
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual(600, r[0]);
                    }
                }
            }
        }

        #endregion
    }
}