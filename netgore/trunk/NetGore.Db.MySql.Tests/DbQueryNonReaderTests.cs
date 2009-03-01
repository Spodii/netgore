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
    struct MyNonReaderValues
    {
        public int A;
        public int B;
        public int C;

        public MyNonReaderValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    class MyNonReader : DbQueryNonReader<MyNonReaderValues>
    {
        const string _commandText = "SELECT @a + @b + @c";
        static readonly IEnumerable<DbParameter> _parameters = new DbParameter[] { new MySqlParameter("@a", null),
            new MySqlParameter("@b", null), new MySqlParameter("@c", null) };

        public MyNonReader(DbConnectionPool connectionPool) : base(connectionPool, _commandText, _parameters)
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
                    nonReader.Execute(new MyNonReaderValues(5, 10, 15));
                    Assert.AreEqual(0, cp.Count);
                }
            }
        }
    }
}
