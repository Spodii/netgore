using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NetGore.Db.Query;
using NUnit.Framework;

namespace NetGore.Db.MySql.Tests
{
    struct MyReaderValues
    {
        public int A;
        public int B;
        public int C;

        public MyReaderValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    class MyReader : DbQueryReader<MyNonReaderValues>
    {
        const string _commandText = "SELECT @a + @b + @c";
        static readonly IEnumerable<DbParameter> _parameters = new DbParameter[] { new MySqlParameter("@a", null),
            new MySqlParameter("@b", null), new MySqlParameter("@c", null) };

        public MyReader(DbConnectionPool connectionPool)
            : base(connectionPool, _commandText, _parameters)
        {
        }

        protected override void SetParameters(DbParameterCollection parameters, MyNonReaderValues item)
        {
            parameters["@a"].Value = item.A;
            parameters["@b"].Value = item.B;
            parameters["@c"].Value = item.C;
        }
    }

    [TestFixture]
    public class DbQueryReaderTests
    {
        static MyReader CreateReader() { return new MyReader(TestSettings.CreateConnectionPool()); }

        [Test]
        public void SelectTest()
        {
            using (var nonReader = CreateReader())
            {
                var cp = nonReader.ConnectionPool;

                for (int i = 0; i < 100; i++)
                {
                    Assert.AreEqual(0, cp.Count);
                    using (var r = nonReader.Execute(new MyNonReaderValues(5, 10, 15)))
                    {
                        Assert.AreEqual(1, cp.Count);
                        Assert.IsTrue(r.Read());
                        Assert.AreEqual(5 + 10 + 15, r[0]);
                    }
                }
            }
        }

        static void SelectTestRecurse(IDbQueryReader<MyNonReaderValues> reader, int depth, int initialDepth)
        {
            var cp = reader.ConnectionPool;
            var v = new MyNonReaderValues(depth * 2, depth - 2, depth + 57);
            int expectedPoolSize = initialDepth - depth;

            Assert.AreEqual(expectedPoolSize, cp.Count);
            using (var r = reader.Execute(v))
            {
                expectedPoolSize++;

                Assert.AreEqual(expectedPoolSize, cp.Count);
                Assert.IsTrue(r.Read());
                Assert.AreEqual(v.A + v.B + v.C, r[0]);
                if (depth > 0)
                    SelectTestRecurse(reader, depth - 1, initialDepth);
                Assert.AreEqual(expectedPoolSize, cp.Count);

                expectedPoolSize--;
            }
            Assert.AreEqual(expectedPoolSize, cp.Count);
        }

        static void SelectTestRecurse(IDbQueryReader<MyNonReaderValues> reader, int depth)
        {
            SelectTestRecurse(reader, depth, depth);
        }

        [Test]
        public void ConcurrentSelectTest()
        {
            // Tests 3 concurrently open selects 100 times total

            using (var reader = CreateReader())
            {
                for (int i = 0; i < 100; i++)
                {
                    SelectTestRecurse(reader, 3);
                }
            }
        }

        [Test]
        public void DisposeTest()
        {
            MyReader reader = CreateReader();

            using (reader)
            {
                SelectTestRecurse(reader, 1);
            }

            Assert.IsTrue(reader.IsDisposed);
        }

        [Test]
        public void SuperConcurrentSelectTest()
        {
            // Recurses 50 levels deep, resulting in 50 concurrent selects being open

            using (var reader = CreateReader())
            {
                SelectTestRecurse(reader, 50);
            }
        }
    }
}
