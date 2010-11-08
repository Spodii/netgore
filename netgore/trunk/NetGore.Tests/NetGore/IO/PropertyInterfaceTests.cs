using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.NetGore.IO
{
    class NonPublicWithNestedPublic
    {
        public class PublicNested
        {
            public int Int { get; set; }
        }
    }

    [TestFixture]
    public class PropertyInterfaceTests
    {
        static IPropertyInterface<U, T> CreatePI<T, U>(string name)
        {
            return PropertyInterface<U, T>.Instance.GetByName(name);
        }

        static IPropertyInterface<TestClass, T> CreatePI<T>(string name)
        {
            return PropertyInterface<TestClass, T>.Instance.GetByName(name);
        }

        static void DoTest<T>(string propertyName, T value, Action<IPropertyInterface<TestClass, T>, TestClass, T> setValue)
        {
            var pi = CreatePI<T>(propertyName);
            var o = new TestClass();

            setValue(pi, o, value);

            Assert.AreEqual(value, pi.Get(o));
        }

        #region Unit tests

        [Test]
        public void ByteTest()
        {
            DoTest<byte>("Byte", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void DoubleTest()
        {
            DoTest<double>("Double", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void FloatTest()
        {
            DoTest<float>("Float", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void IntTest()
        {
            DoTest<int>("Int", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void InternalClassTest()
        {
            var o = new InternalTestClass();
            var pi = CreatePI<int, InternalTestClass>("Int");
            pi.Set(o, 50);
            Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void IntsTest()
        {
            DoTest<int[]>("Ints", new int[] { 55, 52, 23 }, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void LongTest()
        {
            DoTest<long>("Long", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void NonPublicWithNestedPublicTest()
        {
            var o = new NonPublicWithNestedPublic.PublicNested();
            var pi = CreatePI<int, NonPublicWithNestedPublic.PublicNested>("Int");
            pi.Set(o, 50);
            Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void ObjectTest()
        {
            DoTest<object>("Object", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void PrivateClassPrivateGetterSetterTest()
        {
            var o = new PrivateTestClassPrivateGetterSetter();
            var pi = CreatePI<int, PrivateTestClassPrivateGetterSetter>("Int");
            pi.Set(o, 50);
            //Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void PrivateClassPrivateGetterTest()
        {
            var o = new PrivateTestClassPrivateGetter();
            var pi = CreatePI<int, PrivateTestClassPrivateGetter>("Int");
            pi.Set(o, 50);
            //Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void PrivateClassPrivateSetterTest()
        {
            var o = new PrivateTestClassPrivateSetter();
            var pi = CreatePI<int, PrivateTestClassPrivateSetter>("Int");
            pi.Set(o, 50);
            Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void PrivateClassTest()
        {
            var o = new PrivateTestClass();
            var pi = CreatePI<int, PrivateTestClass>("Int");
            pi.Set(o, 50);
            Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void ProtectedClassTest()
        {
            var o = new ProtectedTestClass();
            var pi = CreatePI<int, ProtectedTestClass>("Int");
            pi.Set(o, 50);
            Assert.AreEqual(50, o.Int);
            Assert.AreEqual(50, pi.Get(o));
        }

        [Test]
        public void SByteTest()
        {
            DoTest<sbyte>("SByte", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void ShortTest()
        {
            DoTest<short>("Short", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void StringTest()
        {
            DoTest<string>("String", "55", (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void UIntTest()
        {
            DoTest<uint>("UInt", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void ULongTest()
        {
            DoTest<ulong>("ULong", 55, (pi, o, v) => pi.Set(o, v));
        }

        [Test]
        public void UShortTest()
        {
            DoTest<ushort>("UShort", 55, (pi, o, v) => pi.Set(o, v));
        }

        #endregion

        internal class InternalTestClass
        {
            public int Int { get; set; }
        }

        class PrivateTestClass
        {
            public int Int { get; set; }
        }

        class PrivateTestClassPrivateGetter
        {
            public int Int { private get; set; }
        }

        class PrivateTestClassPrivateGetterSetter
        {
            int Int { get; set; }
        }

        class PrivateTestClassPrivateSetter
        {
            public int Int { get; private set; }
        }

        internal class ProtectedTestClass
        {
            public int Int { get; set; }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class TestClass
        {
            public TestClass()
            {
                Ints = new int[] { 1, 2, 3, 4 };
                Object = new object();
                String = "Test";
            }

            public byte Byte { get; set; }
            public double Double { get; set; }
            public float Float { get; set; }
            public int Int { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
            public int[] Ints { get; set; }

            public long Long { get; set; }
            public object Object { get; set; }
            public sbyte SByte { get; set; }
            public short Short { get; set; }
            public string String { get; set; }
            public uint UInt { get; set; }
            public ulong ULong { get; set; }
            public ushort UShort { get; set; }
        }
    }
}