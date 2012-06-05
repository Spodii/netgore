using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class MethodInfoHelperTests
    {
        static readonly string[] _instanceMethodNames = new string[] { "A", "C", "E", "G", "K", "L", "M", "N" };
        static readonly string[] _invalidMethodNames = new string[] { "X", "Y", "Z" };
        static readonly string[] _staticMethodNames = new string[] { "B", "D", "F", "H", "I", "J" };

        readonly Type[] _types = new Type[] { typeof(TestA), typeof(TestB), typeof(TestC), typeof(TestD) };

        static bool HasAllInstanceMethods(IEnumerable<MethodInfo> methodInfos)
        {
            return _instanceMethodNames.All(n => methodInfos.Any(m => m.Name == n));
        }

        static bool HasAllStaticMethods(IEnumerable<MethodInfo> methodInfos)
        {
            return _staticMethodNames.All(n => methodInfos.Any(m => m.Name == n));
        }

        static bool HasAnyInstanceMethods(IEnumerable<MethodInfo> methodInfos)
        {
            return _instanceMethodNames.Any(n => methodInfos.Any(m => m.Name == n));
        }

        static bool HasAnyStaticMethods(IEnumerable<MethodInfo> methodInfos)
        {
            return _staticMethodNames.Any(n => methodInfos.Any(m => m.Name == n));
        }

        static bool HasNonAttributeMethods(IEnumerable<MethodInfo> methodInfos)
        {
            return _invalidMethodNames.Any(n => methodInfos.Any(m => m.Name == n));
        }

        #region Unit tests

        [Test]
        public void FindInstanceMethodsWithAttribute2Test()
        {
            var methods = MethodInfoHelper.FindInstanceMethodsWithAttribute<Att>(_types, x => true);
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsFalse(HasAnyStaticMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void FindInstanceMethodsWithAttributeTest()
        {
            var methods = MethodInfoHelper.FindInstanceMethodsWithAttribute<Att>(_types);
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsFalse(HasAnyStaticMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void FindMethodsWithAttribute2Test()
        {
            var methods = MethodInfoHelper.FindMethodsWithAttribute<Att>(_types, x => true);
            Assert.IsTrue(HasAllStaticMethods(methods));
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void FindMethodsWithAttributeTest()
        {
            var methods = MethodInfoHelper.FindMethodsWithAttribute<Att>(_types);
            Assert.IsTrue(HasAllStaticMethods(methods));
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void FindStaticMethodsWithAttribute2Test()
        {
            var methods = MethodInfoHelper.FindStaticMethodsWithAttribute<Att>(_types, x => true);
            Assert.IsTrue(HasAllStaticMethods(methods));
            Assert.IsFalse(HasAnyInstanceMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void FindStaticMethodsWithAttributeTest()
        {
            var methods = MethodInfoHelper.FindStaticMethodsWithAttribute<Att>(_types);
            Assert.IsTrue(HasAllStaticMethods(methods));
            Assert.IsFalse(HasAnyInstanceMethods(methods));
            Assert.IsFalse(HasNonAttributeMethods(methods));
        }

        [Test]
        public void GetInstanceMethodsTest()
        {
            var methods = MethodInfoHelper.GetInstanceMethods(_types);
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsFalse(HasAnyStaticMethods(methods));
            Assert.IsTrue(HasNonAttributeMethods(methods));
        }

        [Test]
        public void GetMethodsTest()
        {
            var methods = MethodInfoHelper.GetMethods(_types);
            Assert.IsTrue(HasAllInstanceMethods(methods));
            Assert.IsTrue(HasAnyStaticMethods(methods));
            Assert.IsTrue(HasNonAttributeMethods(methods));
        }

        [Test]
        public void GetStaticMethodsTest()
        {
            var methods = MethodInfoHelper.GetStaticMethods(_types);
            Assert.IsTrue(HasAllStaticMethods(methods));
            Assert.IsFalse(HasAnyInstanceMethods(methods));
            Assert.IsTrue(HasNonAttributeMethods(methods));
        }

        #endregion

        sealed class Att : Attribute
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class TestA
        {
            [Att]
            public void A()
            {
            }

            [Att]
            public static void B()
            {
            }

            [Att]
            internal void C()
            {
            }

            [Att]
            internal static void D()
            {
            }

            [Att]
            void E()
            {
            }

            [Att]
            static void F()
            {
            }

            [Att]
            protected internal void G()
            {
            }

            [Att]
            protected internal static void H()
            {
            }

            public void X()
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class TestB
        {
            [Att]
            public static void I()
            {
            }

            [Att]
            static void J()
            {
            }

            public static void Y()
            {
            }
        }

        class TestC
        {
            [Att]
            public void K()
            {
            }

            [Att]
            void L()
            {
            }

            void Z()
            {
            }
        }

        internal class TestD
        {
            [Att]
            public void M()
            {
            }

            [Att]
            void N()
            {
            }
        }
    }
}