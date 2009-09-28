using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
    public class DbQueryNonReaderTests
    {
        static MyNonReader CreateNonReader()
        {
            return new MyNonReader(TestSettings.CreateConnectionPool());
        }

        [Test]
        public void SelectTest()
        {
            using (MyNonReader nonReader = CreateNonReader())
            {
                DbConnectionPool cp = nonReader.ConnectionPool;

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