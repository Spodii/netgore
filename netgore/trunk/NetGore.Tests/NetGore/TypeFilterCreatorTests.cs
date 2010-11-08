using System;
using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class TypeFilterCreatorTests
    {
        #region Unit tests

        [Test]
        public void AttributeFailTest()
        {
            var f1 = new TypeFilterCreator
            {
                RequireAttributes = true,
                MatchAllAttributes = true,
                Attributes = new Type[] { typeof(DescriptionAttribute), typeof(AttribA) }
            };
            var f2 = new TypeFilterCreator
            {
                RequireAttributes = true,
                MatchAllAttributes = true,
                Attributes = new Type[] { typeof(DescriptionAttribute), typeof(AttribB) }
            };
            var f3 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(DescriptionAttribute) } };

            Assert.Throws<TypeFilterException>(() => f1.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f2.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f3.GetFilter()(typeof(A)));
        }

        [Test]
        public void AttributePassTest()
        {
            var f1 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA) } };
            var f2 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribB) } };
            var f3 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA), typeof(AttribB) } };
            var f4 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA), typeof(AttribB) } };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f3.GetFilter()(typeof(A)));
            Assert.IsTrue(f4.GetFilter()(typeof(A)));
        }

        [Test]
        public void ConstructorFailTest()
        {
            var f1 = new TypeFilterCreator { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object) } };
            var f2 = new TypeFilterCreator
            { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object), typeof(int) } };

            Assert.Throws<TypeFilterException>(() => f1.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void ConstructorPassTest()
        {
            var f1 = new TypeFilterCreator
            { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object), typeof(string) } };
            var f2 = new TypeFilterCreator { RequireConstructor = true, ConstructorParameters = Type.EmptyTypes };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void CustomFilterTest()
        {
            var f1 = new TypeFilterCreator { CustomFilter = (x => !x.IsPublic) };
            var f2 = new TypeFilterCreator { CustomFilter = (x => x == typeof(object)) };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsFalse(f1.GetFilter()(typeof(object)));
            Assert.IsFalse(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(object)));
        }

        [Test]
        public void InterfaceFailTest()
        {
            var f1 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia) } };
            var f2 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(IEnumerable), typeof(Ib) } };
            var f3 = new TypeFilterCreator
            {
                RequireInterfaces = true,
                MatchAllInterfaces = true,
                Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia), typeof(Ib) }
            };
            var f4 = new TypeFilterCreator
            {
                RequireInterfaces = true,
                MatchAllInterfaces = true,
                Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia), typeof(Ib) }
            };
            var f5 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = false, Interfaces = new Type[] { typeof(IEnumerable) } };

            Assert.Throws<TypeFilterException>(() => f1.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f2.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f3.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f4.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f5.GetFilter()(typeof(A)));
        }

        [Test]
        public void InterfacePassTest()
        {
            var f1 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia) } };
            var f2 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ib) } };
            var f3 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia), typeof(Ib) } };
            var f4 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia), typeof(Ib) } };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f3.GetFilter()(typeof(A)));
            Assert.IsTrue(f4.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsAbstractFailTest()
        {
            var f1 = new TypeFilterCreator { IsAbstract = false };
            Assert.IsFalse(f1.GetFilter()(typeof(baseClass)));
        }

        [Test]
        public void IsAbstractNullTest()
        {
            var f1 = new TypeFilterCreator { IsAbstract = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsAbstractPassTest()
        {
            var f1 = new TypeFilterCreator { IsAbstract = true };
            Assert.IsTrue(f1.GetFilter()(typeof(baseClass)));
        }

        [Test]
        public void IsClassFailTest()
        {
            var f1 = new TypeFilterCreator { IsClass = false };
            Assert.IsFalse(f1.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsClassNullTest()
        {
            var f1 = new TypeFilterCreator { IsClass = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsClassPassTest()
        {
            var f1 = new TypeFilterCreator { IsClass = true };
            Assert.IsTrue(f1.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsInterfaceFailTest()
        {
            var f1 = new TypeFilterCreator { IsInterface = false };
            Assert.IsFalse(f1.GetFilter()(typeof(Ia)));
        }

        [Test]
        public void IsInterfaceNullTest()
        {
            var f1 = new TypeFilterCreator { IsInterface = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsInterfacePassTest()
        {
            var f1 = new TypeFilterCreator { IsInterface = true };
            Assert.IsTrue(f1.GetFilter()(typeof(Ia)));
        }

        [Test]
        public void SubclassFailTest()
        {
            var f1 = new TypeFilterCreator { Subclass = typeof(StringBuilder) };
            var f2 = new TypeFilterCreator { Subclass = typeof(Ia) };

            Assert.IsFalse(f1.GetFilter()(typeof(A)));
            Assert.IsFalse(f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void SubclassPassTest()
        {
            var f1 = new TypeFilterCreator { Subclass = typeof(baseClass) };
            var f2 = new TypeFilterCreator { Subclass = typeof(object) };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
        }

        #endregion

        [AttribA]
        [AttribB]
        class A : baseClass, Ia, Ib
        {
            public A(object a, string b)
            {
            }

            A()
            {
            }
        }

        sealed class AttribA : Attribute
        {
        }

        sealed class AttribB : Attribute
        {
        }

        interface Ia
        {
        }

        interface Ib
        {
        }

        abstract class baseClass
        {
        }
    }
}