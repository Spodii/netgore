using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class TypeHelperTests
    {
        static Type GetNestedType(Type type, string typeName)
        {
            return type.GetNestedType(typeName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        }

        [Test]
        public void IsClassTypeTreePublicTest()
        {
            var a = typeof(A);
            var aa = GetNestedType(a, "AA");
            var aaa = GetNestedType(aa, "AAA");
            var aab = GetNestedType(aa, "AAB");

            var b = typeof(B);
            var ba = GetNestedType(b, "BA");
            var bb = GetNestedType(b, "BB");
            var bba = GetNestedType(bb, "BBA");

            Assert.IsTrue(TypeHelper.IsClassTypeTreePublic(a));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(aa));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(aaa));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(aab));

            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(b));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(ba));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(bb));
            Assert.IsFalse(TypeHelper.IsClassTypeTreePublic(bba));

            Assert.IsTrue(TypeHelper.IsClassTypeTreePublic(typeof(TypeHelperTests)));
        }

        public class A
        {
            class AA
            {
                public class AAA
                {
                }

                class AAB
                {
                }
            }
        }

        class B
        {
            public class BA
            {
            }

            class BB
            {
                public class BBA
                {
                }
            }
        }
    }
}