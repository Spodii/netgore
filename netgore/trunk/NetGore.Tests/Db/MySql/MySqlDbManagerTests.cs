using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NetGore.Db;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture]
    public class MySqlDbManagerTests
    {
        #region Unit tests

        [Test]
        public void CleanupCommandsTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();
            var stack = new Stack<IDbCommand>();

            Assert.AreEqual(0, manager.ConnectionPool.LiveObjects);
            for (int i = 1; i < 20; i++)
            {
                IDbCommand item = manager.GetCommand();
                stack.Push(item);
                Assert.AreEqual(i, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
            }

            while (stack.Count > 0)
            {
                IDbCommand item = stack.Pop();
                int start = manager.ConnectionPool.LiveObjects;
                item.Dispose();
                Assert.AreEqual(start - 1, manager.ConnectionPool.LiveObjects);
            }

            Assert.AreEqual(0, manager.ConnectionPool.LiveObjects);
        }

        [Test]
        public void CleanupConnectionsTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();
            var stack = new Stack<IPoolableDbConnection>();

            Assert.AreEqual(0, manager.ConnectionPool.LiveObjects);
            for (int i = 1; i < 20; i++)
            {
                IPoolableDbConnection item = manager.GetConnection();
                stack.Push(item);
                Assert.AreEqual(i, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
            }

            while (stack.Count > 0)
            {
                IPoolableDbConnection item = stack.Pop();
                int start = manager.ConnectionPool.LiveObjects;
                item.Dispose();
                Assert.AreEqual(start - 1, manager.ConnectionPool.LiveObjects);
            }

            Assert.AreEqual(0, manager.ConnectionPool.LiveObjects);
        }

        [Test]
        public void CommandParametersTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));

                IDbCommand cmd;
                using (cmd = manager.GetCommand())
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    Assert.IsNotNull(cmd);

                    cmd.CommandText = "SELECT @left + @right";

                    MySqlParameter left = new MySqlParameter("@left", null) { Value = 500 };
                    MySqlParameter right = new MySqlParameter("@right", null) { Value = 100 };
                    cmd.Parameters.Add(left);
                    cmd.Parameters.Add(right);

                    using (IDataReader r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual("600", r[0].ToString());
                    }

                    Assert.AreNotEqual(0, cmd.Parameters.Count);
                    Assert.IsFalse(string.IsNullOrEmpty(cmd.CommandText));
                }
            }
        }

        [Test]
        public void CommandStringTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                using (IDbCommand cmd = manager.GetCommand("SELECT 500 + 100"))
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    Assert.IsNotNull(cmd);
                    Assert.AreEqual("SELECT 500 + 100", cmd.CommandText);
                    using (IDataReader r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual("600", r[0].ToString());
                    }
                }
            }
        }

        [Test]
        public void CommandTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                IDbCommand cmd;
                using (cmd = manager.GetCommand())
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    Assert.IsNotNull(cmd);
                    cmd.CommandText = "SELECT 500 + 100";
                    using (IDataReader r = cmd.ExecuteReader())
                    {
                        r.Read();
                        Assert.AreEqual("600", r[0].ToString());
                    }
                }
            }
        }

        [Test]
        public void ConnectionTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                IPoolableDbConnection pConn;
                IDbConnection conn;
                using (pConn = manager.GetConnection())
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    conn = pConn.Connection;
                    Assert.IsNotNull(conn);
                    Assert.AreEqual(ConnectionState.Open, conn.State);
                }
                Assert.AreEqual(ConnectionState.Closed, conn.State);
            }
        }

        [Test]
        public void ExecuteNonQueryTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                int ret = manager.ExecuteNonQuery("SELECT 500 + 100");
                Assert.AreEqual(-1, ret);
            }
        }

        [Test]
        public void ExecuteReader2Test()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                using (DataReaderContainer r = manager.ExecuteReader("SELECT 500 + 100", CommandBehavior.SingleResult))
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    Assert.IsFalse(r.IsClosed);
                    r.Read();
                    Assert.AreEqual("600", r[0].ToString());
                }
            }
        }

        [Test]
        public void ExecuteReaderTest()
        {
            DbManager manager = DbManagerTestSettings.CreateDbManager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                using (DataReaderContainer r = manager.ExecuteReader("SELECT 500 + 100"))
                {
                    Assert.AreEqual(1, manager.ConnectionPool.LiveObjects, string.Format("Iteration {0}", i));
                    Assert.IsFalse(r.IsClosed);
                    r.Read();
                    Assert.AreEqual("600", r[0].ToString());
                }
            }
        }

        #endregion
    }
}