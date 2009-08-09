using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

    class MyReader : DbQueryReader<QueryTestValues>
    {
        const string _commandText = "SELECT @a + @b + @c";

        public MyReader(DbConnectionPool connectionPool) : base(connectionPool, _commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@a", "@b", "@c");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryTestValues item)
        {
            p["@a"] = item.A;
            p["@b"] = item.B;
            p["@c"] = item.C;
        }
    }

    [TestFixture]
    public class DbQueryReaderTests
    {
        static MyReader CreateReader()
        {
            return new MyReader(TestSettings.CreateConnectionPool());
        }

        static void SelectTestRecurse(IDbQueryReader<QueryTestValues> reader, int depth, int initialDepth)
        {
            DbConnectionPool cp = reader.ConnectionPool;
            QueryTestValues v = new QueryTestValues(depth * 2, depth - 2, depth + 57);
            int expectedPoolSize = initialDepth - depth;

            Assert.AreEqual(expectedPoolSize, cp.Count);
            using (IDataReader r = reader.ExecuteReader(v))
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

        static void SelectTestRecurse(IDbQueryReader<QueryTestValues> reader, int depth)
        {
            SelectTestRecurse(reader, depth, depth);
        }

        [Test]
        public void ConcurrentSelectTest()
        {
            // Tests 3 concurrently open selects 100 times total

            using (MyReader reader = CreateReader())
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
        public void SelectTest()
        {
            QueryTestValues testValues = new QueryTestValues(5, 10, 15);

            using (MyReader reader = CreateReader())
            {
                DbConnectionPool cp = reader.ConnectionPool;

                for (int i = 0; i < 100; i++)
                {
                    Assert.AreEqual(0, cp.Count);
                    using (IDataReader r = ((IDbQueryReader<QueryTestValues>)reader).ExecuteReader(testValues))
                    {
                        Assert.AreEqual(1, cp.Count);
                        Assert.IsTrue(r.Read());
                        Assert.AreEqual(5 + 10 + 15, r[0]);
                    }
                }
            }
        }

        [Test]
        public void SuperConcurrentSelectTest()
        {
            // Recurses 50 levels deep, resulting in 50 concurrent selects being open

            using (MyReader reader = CreateReader())
            {
                SelectTestRecurse(reader, 50);
            }
        }
    }
}