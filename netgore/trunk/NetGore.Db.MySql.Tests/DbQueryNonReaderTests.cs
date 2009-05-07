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
    struct QueryTestValues
    {
        public int A;
        public int B;
        public int C;

        public QueryTestValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    class MyNonReader : DbQueryNonReader<QueryTestValues>
    {
        const string _commandText = "SELECT @a + @b + @c";

        public MyNonReader(DbConnectionPool connectionPool) : base(connectionPool, _commandText)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@a", "@b", "@c");
        }

        protected override void SetParameters(DbParameterValues p, QueryTestValues item)
        {
            p["@a"] = item.A;
            p["@b"] = item.B;
            p["@c"] = item.C;
        }
    }

    [TestFixture]
    public class DbQueryNonReaderTests
    {
        static MyNonReader CreateNonReader() { return new MyNonReader(TestSettings.CreateConnectionPool()); }

        [Test]
        public void SelectTest()
        {
            using (var nonReader = CreateNonReader())
            {
                var cp = nonReader.ConnectionPool;

                for (int i = 0; i < 100; i++)
                {
                    Assert.AreEqual(0, cp.Count);
                    nonReader.Execute(new QueryTestValues(5, 10, 15));
                    Assert.AreEqual(0, cp.Count);
                }
            }
        }
    }
}
