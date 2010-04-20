using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture]
    public class DbQueryNonReaderTests
    {
        static MyNonReader CreateNonReader()
        {
            return new MyNonReader(DbManagerTestSettings.CreateConnectionPool());
        }

        #region Unit tests

        [Test]
        public void SelectTest()
        {
            using (var nonReader = CreateNonReader())
            {
                var cp = nonReader.ConnectionPool;

                for (var i = 0; i < 100; i++)
                {
                    Assert.AreEqual(0, cp.LiveObjects);
                    nonReader.Execute(new QueryTestValues(5, 10, 15));
                    Assert.AreEqual(0, cp.LiveObjects);
                }
            }
        }

        #endregion

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
    }
}