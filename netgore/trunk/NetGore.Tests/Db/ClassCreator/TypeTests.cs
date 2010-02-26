using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using NetGore.Tests.Properties;
using NUnit.Framework;

namespace NetGore.Tests.Db.ClassCreator
{
    [TestFixture]
    public class TypeTests
    {
        DbConnection _conn;
        IEnumerable<PropertyInfo> _dbAProperties;
        IEnumerable<Type> _dbATypes;
        IEnumerable<PropertyInfo> _dbAViewProperties;
        IEnumerable<Type> _dbAViewTypes;

        [TestFixtureSetUp]
        public void Setup()
        {
            TestDb.Execute(Resources.testdb_a);
            TestDb.Execute(Resources.testdb_a_view);

            _dbATypes = ClassCreatorHelper.GetTableTypes(Resources.testdb_a_name);
            _dbAProperties = ClassCreatorHelper.GetTableTypeProperties(_dbATypes);

            _dbAViewTypes = ClassCreatorHelper.GetTableTypes(Resources.testdb_a_view_name);
            _dbAViewProperties = ClassCreatorHelper.GetTableTypeProperties(_dbAViewTypes);

            _conn = TestDb.Open();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            TestDb.Close(_conn);
        }

        #region Unit tests

        [Test]
        public void CreateBoolFromUnsignedTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "boou", typeof(bool));
        }

        [Test]
        public void CreateBoolTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "boo", typeof(bool));
        }

        [Test]
        public void CreateByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bun", typeof(byte?));
        }

        [Test]
        public void CreateByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bu", typeof(byte));
        }

        [Test]
        public void CreateDoubleNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "dn", typeof(double?));
        }

        [Test]
        public void CreateDoubleTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "d", typeof(double));
        }

        [Test]
        public void CreateFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fn", typeof(float?));
        }

        [Test]
        public void CreateFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "f", typeof(float));
        }

        [Test]
        public void CreateIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "in", typeof(int?));
        }

        [Test]
        public void CreateIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "i", typeof(int));
        }

        [Test]
        public void CreateLongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ln", typeof(long?));
        }

        [Test]
        public void CreateLongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "l", typeof(long));
        }

        [Test]
        public void CreateSByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "bn", typeof(sbyte?));
        }

        [Test]
        public void CreateSByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "b", typeof(sbyte));
        }

        [Test]
        public void CreateShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "sn", typeof(short?));
        }

        [Test]
        public void CreateShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "s", typeof(short));
        }

        [Test]
        public void CreateTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "txtn", typeof(string));
        }

        [Test]
        public void CreateTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "txt", typeof(string));
        }

        [Test]
        public void CreateTinyTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ttxtn", typeof(string));
        }

        [Test]
        public void CreateTinyTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "ttxt", typeof(string));
        }

        [Test]
        public void CreateUFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fun", typeof(float?));
        }

        [Test]
        public void CreateUFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "fu", typeof(float));
        }

        [Test]
        public void CreateUIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "iun", typeof(uint?));
        }

        [Test]
        public void CreateUIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "iu", typeof(uint));
        }

        [Test]
        public void CreateULongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "lun", typeof(ulong?));
        }

        [Test]
        public void CreateULongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "lu", typeof(ulong));
        }

        [Test]
        public void CreateUnsignedDoubleNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "dun", typeof(double?));
        }

        [Test]
        public void CreateUnsignedDoubleTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "du", typeof(double));
        }

        [Test]
        public void CreateUShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "sun", typeof(ushort?));
        }

        [Test]
        public void CreateUShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "su", typeof(ushort));
        }

        [Test]
        public void CreateVarCharNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "vcn", typeof(string));
        }

        [Test]
        public void CreateVarCharTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAProperties, "vc", typeof(string));
        }

        [Test]
        public void CreateViewByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "bun", typeof(byte?));
        }

        [Test]
        public void CreateViewByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "bu", typeof(byte));
        }

        [Test]
        public void CreateViewDoubleNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "dn", typeof(double?));
        }

        [Test]
        public void CreateViewDoubleTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "d", typeof(double));
        }

        [Test]
        public void CreateViewFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "fn", typeof(float?));
        }

        [Test]
        public void CreateViewFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "f", typeof(float));
        }

        [Test]
        public void CreateViewIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "in", typeof(int?));
        }

        [Test]
        public void CreateViewIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "i", typeof(int));
        }

        [Test]
        public void CreateViewLongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "ln", typeof(long?));
        }

        [Test]
        public void CreateViewLongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "l", typeof(long));
        }

        [Test]
        public void CreateViewSByteNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "bn", typeof(sbyte?));
        }

        [Test]
        public void CreateViewSByteTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "b", typeof(sbyte));
        }

        [Test]
        public void CreateViewShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "sn", typeof(short?));
        }

        [Test]
        public void CreateViewShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "s", typeof(short));
        }

        [Test]
        public void CreateViewTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "txtn", typeof(string));
        }

        [Test]
        public void CreateViewTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "txt", typeof(string));
        }

        [Test]
        public void CreateViewTinyTextNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "ttxtn", typeof(string));
        }

        [Test]
        public void CreateViewTinyTextTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "ttxt", typeof(string));
        }

        [Test]
        public void CreateViewUFloatNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "fun", typeof(float?));
        }

        [Test]
        public void CreateViewUFloatTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "fu", typeof(float));
        }

        [Test]
        public void CreateViewUIntNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "iun", typeof(uint?));
        }

        [Test]
        public void CreateViewUIntTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "iu", typeof(uint));
        }

        [Test]
        public void CreateViewULongNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "lun", typeof(ulong?));
        }

        [Test]
        public void CreateViewULongTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "lu", typeof(ulong));
        }

        [Test]
        public void CreateViewUnsignedDoubleNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "dun", typeof(double?));
        }

        [Test]
        public void CreateViewUnsignedDoubleTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "du", typeof(double));
        }

        [Test]
        public void CreateViewUShortNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "sun", typeof(ushort?));
        }

        [Test]
        public void CreateViewUShortTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "su", typeof(ushort));
        }

        [Test]
        public void CreateViewVarCharNullableTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "vcn", typeof(string));
        }

        [Test]
        public void CreateViewVarCharTest()
        {
            ClassCreatorHelper.AssertContainsProperty(_dbAViewProperties, "vc", typeof(string));
        }

        #endregion
    }
}