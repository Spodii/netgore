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
        [Test]
        public void AttributeFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator
            {
                RequireAttributes = true,
                MatchAllAttributes = true,
                Attributes = new Type[] { typeof(DescriptionAttribute), typeof(AttribA) }
            };
            TypeFilterCreator f2 = new TypeFilterCreator
            {
                RequireAttributes = true,
                MatchAllAttributes = true,
                Attributes = new Type[] { typeof(DescriptionAttribute), typeof(AttribB) }
            };
            TypeFilterCreator f3 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(DescriptionAttribute) } };

            Assert.Throws<TypeFilterException>(() => f1.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f2.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f3.GetFilter()(typeof(A)));
        }

        [Test]
        public void AttributePassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA) } };
            TypeFilterCreator f2 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribB) } };
            TypeFilterCreator f3 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA), typeof(AttribB) } };
            TypeFilterCreator f4 = new TypeFilterCreator
            { RequireAttributes = true, MatchAllAttributes = true, Attributes = new Type[] { typeof(AttribA), typeof(AttribB) } };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f3.GetFilter()(typeof(A)));
            Assert.IsTrue(f4.GetFilter()(typeof(A)));
        }

        [Test]
        public void ConstructorFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator
            { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object) } };
            TypeFilterCreator f2 = new TypeFilterCreator
            { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object), typeof(int) } };

            Assert.Throws<TypeFilterException>(() => f1.GetFilter()(typeof(A)));
            Assert.Throws<TypeFilterException>(() => f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void ConstructorPassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator
            { RequireConstructor = true, ConstructorParameters = new Type[] { typeof(object), typeof(string) } };
            TypeFilterCreator f2 = new TypeFilterCreator { RequireConstructor = true, ConstructorParameters = Type.EmptyTypes };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void CustomFilterTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { CustomFilter = (x => !x.IsPublic) };
            TypeFilterCreator f2 = new TypeFilterCreator { CustomFilter = (x => x == typeof(object)) };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsFalse(f1.GetFilter()(typeof(object)));
            Assert.IsFalse(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(object)));
        }

        [Test]
        public void InterfaceFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia) } };
            TypeFilterCreator f2 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(IEnumerable), typeof(Ib) } };
            TypeFilterCreator f3 = new TypeFilterCreator
            {
                RequireInterfaces = true,
                MatchAllInterfaces = true,
                Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia), typeof(Ib) }
            };
            TypeFilterCreator f4 = new TypeFilterCreator
            {
                RequireInterfaces = true,
                MatchAllInterfaces = true,
                Interfaces = new Type[] { typeof(IEnumerable), typeof(Ia), typeof(Ib) }
            };
            TypeFilterCreator f5 = new TypeFilterCreator
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
            TypeFilterCreator f1 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia) } };
            TypeFilterCreator f2 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ib) } };
            TypeFilterCreator f3 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia), typeof(Ib) } };
            TypeFilterCreator f4 = new TypeFilterCreator
            { RequireInterfaces = true, MatchAllInterfaces = true, Interfaces = new Type[] { typeof(Ia), typeof(Ib) } };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
            Assert.IsTrue(f3.GetFilter()(typeof(A)));
            Assert.IsTrue(f4.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsAbstractFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsAbstract = false };
            Assert.IsFalse(f1.GetFilter()(typeof(baseClass)));
        }

        [Test]
        public void IsAbstractNullTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsAbstract = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsAbstractPassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsAbstract = true };
            Assert.IsTrue(f1.GetFilter()(typeof(baseClass)));
        }

        [Test]
        public void IsClassFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsClass = false };
            Assert.IsFalse(f1.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsClassNullTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsClass = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsClassPassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsClass = true };
            Assert.IsTrue(f1.GetFilter()(typeof(A)));
        }

        [Test]
        public void IsInterfaceFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsInterface = false };
            Assert.IsFalse(f1.GetFilter()(typeof(Ia)));
        }

        [Test]
        public void IsInterfaceNullTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsInterface = null };
            Assert.IsTrue(f1.GetFilter()(typeof(int)));
        }

        [Test]
        public void IsInterfacePassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { IsInterface = true };
            Assert.IsTrue(f1.GetFilter()(typeof(Ia)));
        }

        [Test]
        public void SubclassFailTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { Subclass = typeof(StringBuilder) };
            TypeFilterCreator f2 = new TypeFilterCreator { Subclass = typeof(Ia) };

            Assert.IsFalse(f1.GetFilter()(typeof(A)));
            Assert.IsFalse(f2.GetFilter()(typeof(A)));
        }

        [Test]
        public void SubclassPassTest()
        {
            TypeFilterCreator f1 = new TypeFilterCreator { Subclass = typeof(baseClass) };
            TypeFilterCreator f2 = new TypeFilterCreator { Subclass = typeof(object) };

            Assert.IsTrue(f1.GetFilter()(typeof(A)));
            Assert.IsTrue(f2.GetFilter()(typeof(A)));
        }

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

        class AttribA : Attribute
        {
        }

        class AttribB : Attribute
        {
        }

        abstract class baseClass
        {
        }

        interface Ia
        {
        }

        interface Ib
        {
        }
    }
}