using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using NetGore.Db.ClassCreator;
using NetGore.Tests.Properties;
using NUnit.Framework;

namespace NetGore.Tests.Db.ClassCreator
{
    public enum TestCollEnum
    {
        A,
        B,
        C
    }

    [TestFixture]
    public class AliasingTests
    {
        public enum TestCollNestedEnum
        {
            A,
            B,
            C
        }

        DbConnection _conn;
        IEnumerable<PropertyInfo> _dbProperties;
        IEnumerable<Type> _dbTypes;

        [Test]
        public void BulkRenameTest()
        {
            Assert.IsNotNull(_dbProperties.FirstOrDefault(x => x.Name == "AbCdEfGhIj"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "abcdEFGhij"));
        }

        [Test]
        public void CollectionTest()
        {
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "asdfA"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "asdfB"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "asdfC"));

            Assert.IsNotNull(_dbProperties.FirstOrDefault(x => x.Name == "TestColls"));
        }

        [Test]
        public void CollectionWithNestedKeyTest()
        {
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "bbbbA"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "bbbbB"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "bbbbC"));

            Assert.IsNotNull(_dbProperties.FirstOrDefault(x => x.Name == "TestCollTwos"));
        }

        [Test]
        public void RenameTest()
        {
            Assert.IsNotNull(_dbProperties.FirstOrDefault(x => x.Name == "TestAlias1"));
            Assert.IsNull(_dbProperties.FirstOrDefault(x => x.Name == "a"));
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            TestDb.Execute(Resources.testdb_b);

            _dbTypes = ClassCreatorHelper.GetTableTypes(Resources.testdb_b_name, delegate(MySqlClassGenerator x)
                                                                                 {
                                                                                     x.Formatter.AddAlias("a", "TestAlias1");
                                                                                     x.Formatter.AddAlias("AbCdEfGhIj");
                                                                                     x.AddColumnCollection("TestColl",
                                                                                                           typeof(TestCollEnum),
                                                                                                           typeof(int),
                                                                                                           Resources.testdb_b_name,
                                                                                                           new ColumnCollectionItem
                                                                                                               []
                                                                                                           {
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "asdfA",
                                                                                                                   TestCollEnum.A)
                                                                                                               ,
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "asdfB",
                                                                                                                   TestCollEnum.B)
                                                                                                               ,
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "asdfC",
                                                                                                                   TestCollEnum.C)
                                                                                                           });

                                                                                     x.AddColumnCollection("TestCollTwo",
                                                                                                           typeof(
                                                                                                               TestCollNestedEnum),
                                                                                                           typeof(int),
                                                                                                           Resources.testdb_b_name,
                                                                                                           new ColumnCollectionItem
                                                                                                               []
                                                                                                           {
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "bbbbA",
                                                                                                                   TestCollNestedEnum
                                                                                                                       .A),
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "bbbbB",
                                                                                                                   TestCollNestedEnum
                                                                                                                       .B),
                                                                                                               ColumnCollectionItem
                                                                                                                   .FromEnum(
                                                                                                                   x.Formatter,
                                                                                                                   "bbbbC",
                                                                                                                   TestCollNestedEnum
                                                                                                                       .C)
                                                                                                           });
                                                                                 });
            _dbProperties = ClassCreatorHelper.GetTableTypeProperties(_dbTypes);

            _conn = TestDb.Open();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TestDb.Close(_conn);
        }
    }
}