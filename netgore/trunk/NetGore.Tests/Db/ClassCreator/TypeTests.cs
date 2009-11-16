using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using NetGore.Db.ClassCreator;
using NetGore.Tests.Properties;
using NUnit.Framework;

namespace NetGore.Tests.Db.ClassCreator
{
    [TestFixture]
    public class TypeTests
    {
        DbConnection _conn;
        IEnumerable<Type> _dbATypes;
        IEnumerable<PropertyInfo> _dbAProperties;

        [TestFixtureSetUp]
        public void Setup()
        {
            TestDb.Execute(Resources.testdb_a);

            _dbATypes = ClassCreatorHelper.GetTableTypes(Resources.testdb_a_name);
            _dbAProperties = ClassCreatorHelper.GetTableTypeProperties(_dbATypes);

            _conn = TestDb.Open();
        }

        [Test]
        public void CreateByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bu", typeof(byte));
        }

        [Test]
        public void CreateSByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "b", typeof(sbyte));
        }

        [Test]
        public void CreateSByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bn", typeof(sbyte?));
        }

        [Test]
        public void CreateByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bun", typeof(byte?));
        }

        [Test]
        public void CreateShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "s", typeof(short));
        }

        [Test]
        public void CreateShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "sn", typeof(short?));
        }

        [Test]
        public void CreateUShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "su", typeof(ushort));
        }

        [Test]
        public void CreateUShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "sun", typeof(ushort?));
        }

        [Test]
        public void CreateIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "i", typeof(int));
        }

        [Test]
        public void CreateIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "in", typeof(int?));
        }

        [Test]
        public void CreateUIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "iu", typeof(uint));
        }

        [Test]
        public void CreateUIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "iun", typeof(uint?));
        }

        [Test]
        public void CreateVarCharTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "vc", typeof(string));
        }

        [Test]
        public void CreateVarCharNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "vcn", typeof(string));
        }

        [Test]
        public void CreateLongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "l", typeof(long));
        }

        [Test]
        public void CreateLongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ln", typeof(long?));
        }

        [Test]
        public void CreateULongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "lu", typeof(ulong));
        }

        [Test]
        public void CreateULongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "lun", typeof(ulong?));
        }

        [Test]
        public void CreateTinyTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ttxt", typeof(string));
        }

        [Test]
        public void CreateTinyTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ttxtn", typeof(string));
        }

        [Test]
        public void CreateTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "txt", typeof(string));
        }

        [Test]
        public void CreateTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "txtn", typeof(string));
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TestDb.Close(_conn);
        }

        [Test]
        public void CreateFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "f", typeof(float));
        }

        [Test]
        public void CreateFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fn", typeof(float?));
        }

        [Test]
        public void CreateUFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fu", typeof(float));
        }

        [Test]
        public void CreateUFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fun", typeof(float?));
        }
    }
}
