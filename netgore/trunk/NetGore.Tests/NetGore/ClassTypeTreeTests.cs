using System;
using System.Linq;
using System.Text;
using NetGore.Graphics;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class ClassTypeTreeTests
    {
        static ClassTypeTree BuildTree()
        {
            return
                new ClassTypeTree(new Type[]
                {
                    typeof(I1), typeof(I2), typeof(A1), typeof(A2), typeof(A1B1), typeof(A1B1C1), typeof(A1B1C2), typeof(A2),
                    typeof(A2B1), typeof(A2B2)
                });
        }

        #region Unit tests

        [Test]
        public void BuildTreeTest()
        {
            /* TEST CLASS LAYOUT:
             *  
             *              C1
             *             /
             *      A1 - B1 - C2
             *     /
             * root
             *     \
             *      A2 - B1
             *       \
             *        B2
             */

            var root = BuildTree();
            Assert.AreEqual(root.Type, null);
            Assert.AreEqual(root.Parent, null);
            Assert.AreEqual(3, root.Children.Count());

            var a1 = root.Children.First(x => x.Type == typeof(A1));
            Assert.AreEqual(root, a1.Parent);
            Assert.AreEqual(2, a1.Children.Count());

            var a2 = root.Children.First(x => x.Type == typeof(A2));
            Assert.AreEqual(root, a2.Parent);
            Assert.AreEqual(3, a2.Children.Count());

            var a1b1 = a1.Children.First(x => x.Type == typeof(A1B1));
            Assert.AreEqual(a1, a1b1.Parent);
            Assert.AreEqual(3, a1b1.Children.Count());

            var a1b1c1 = a1b1.Children.First(x => x.Type == typeof(A1B1C1));
            Assert.AreEqual(a1b1, a1b1c1.Parent);
            Assert.AreEqual(0, a1b1c1.Children.Count());

            var a1b1c2 = a1b1.Children.First(x => x.Type == typeof(A1B1C2));
            Assert.AreEqual(a1b1, a1b1c2.Parent);
            Assert.AreEqual(0, a1b1c2.Children.Count());

            var a2b1 = a2.Children.First(x => x.Type == typeof(A2B1));
            Assert.AreEqual(a2, a2b1.Parent);
            Assert.AreEqual(0, a2b1.Children.Count());

            var a2b2 = a2.Children.First(x => x.Type == typeof(A2B2));
            Assert.AreEqual(a2, a2b2.Parent);
            Assert.AreEqual(0, a2b2.Children.Count());
        }

        [Test]
        public void FindA1B1C1D1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A1B1C1), root.Find(typeof(A1B1C1D1)).Type);
        }

        [Test]
        public void FindA1B1C1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A1B1C1), root.Find(typeof(A1B1C1)).Type);
        }

        [Test]
        public void FindA1B1C2Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A1B1C2), root.Find(typeof(A1B1C2)).Type);
        }

        [Test]
        public void FindA1B1C3Test()
        {
            var root = BuildTree();
            var x = root.Find(typeof(A1B1C3));
            Assert.AreEqual(typeof(A1B1), x.Parent.Type);
        }

        [Test]
        public void FindA1B1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A1B1), root.Find(typeof(A1B1)).Type);
        }

        [Test]
        public void FindA1FromA1B1Test()
        {
            var root = BuildTree();
            var a1b1 = root.Children.First(x => x.Type == typeof(A1)).Children.First(x => x.Type == typeof(A1B1));
            Assert.AreEqual(typeof(A1B1), a1b1.Type);
            Assert.AreEqual(typeof(A1), a1b1.Find(typeof(A1)).Type);
        }

        [Test]
        public void FindA1FromA2B1Test()
        {
            var root = BuildTree();
            var a1b2 = root.Children.First(x => x.Type == typeof(A2)).Children.First(x => x.Type == typeof(A2B1));
            Assert.AreEqual(typeof(A2B1), a1b2.Type);
            Assert.AreEqual(typeof(A1), a1b2.Find(typeof(A1)).Type);
        }

        [Test]
        public void FindA1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A1), root.Find(typeof(A1)).Type);
        }

        [Test]
        public void FindA2B1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A2B1), root.Find(typeof(A2B1)).Type);
        }

        [Test]
        public void FindA2B2Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A2B2), root.Find(typeof(A2B2)).Type);
        }

        [Test]
        public void FindA2B3C1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A2), root.Find(typeof(A2B3C1)).Parent.Type);
        }

        [Test]
        public void FindA2B3Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A2), root.Find(typeof(A2B3)).Parent.Type);
        }

        [Test]
        public void FindA2Test()
        {
            var root = BuildTree();
            Assert.AreEqual(typeof(A2), root.Find(typeof(A2)).Type);
        }

        [Test]
        public void FindI1Test()
        {
            var root = BuildTree();
            Assert.AreEqual(root, root.Find(typeof(I1)));
        }

        [Test]
        public void FindI2Test()
        {
            var root = BuildTree();
            Assert.AreEqual(root, root.Find(typeof(I2)));
        }

        [Test]
        public void FindICamera2DTest()
        {
            var root = BuildTree();
            Assert.AreEqual(root, root.Find(typeof(ICamera2D)));
        }

        [Test]
        public void FindNullTypeTest()
        {
            var root = BuildTree();
            Assert.Throws<ArgumentNullException>(() => root.Find(null));
        }

        [Test]
        public void FindStringBuilderTest()
        {
            var root = BuildTree();
            Assert.AreEqual(root.Children.First(x => x.Type == null), root.Find(typeof(StringBuilder)));
        }

        [Test]
        public void FindTheGapInTree1Test()
        {
            var root = new ClassTypeTree(new Type[] { typeof(A1), typeof(A2), typeof(A1B1C1D1) });
            var match = root.Find(typeof(A1B1));
            Assert.AreEqual(typeof(A1), match.Type);
        }

        [Test]
        public void FindTheGapInTree2Test()
        {
            var root = new ClassTypeTree(new Type[] { typeof(A1), typeof(A2), typeof(A1B1C1D1) });
            var match = root.Find(typeof(A1B1C1));
            Assert.AreEqual(typeof(A1), match.Type);
        }

        [Test]
        public void GetAllNodesTest()
        {
            var root = BuildTree();
            Assert.AreEqual(12, root.GetAllNodes().Count());
            Assert.AreEqual(root.GetAllNodes().Distinct().Count(), root.GetAllNodes().Count());
        }

        [Test]
        public void RootTest()
        {
            var root = BuildTree();
            var a1b1 = root.Children.First(x => x.Type == typeof(A1)).Children.First(x => x.Type == typeof(A1B1));
            var a1b2 = root.Children.First(x => x.Type == typeof(A2)).Children.First(x => x.Type == typeof(A2B1));
            Assert.AreEqual(root, a1b1.Root);
            Assert.AreEqual(root, a1b2.Root);
            Assert.AreEqual(root, root.Root);
        }

        #endregion

        class A1
        {
        }

        class A1B1 : A1
        {
        }

        class A1B1C1 : A1B1
        {
        }

        class A1B1C1D1 : A1B1C1
        {
        }

        class A1B1C2 : A1B1
        {
        }

        class A1B1C3 : A1B1
        {
        }

        class A2 : I1, I2
        {
        }

        class A2B1 : A2
        {
        }

        class A2B2 : A2
        {
        }

        class A2B3 : A2
        {
        }

        class A2B3C1 : A2B3
        {
        }

        interface I1
        {
        }

        interface I2
        {
        }
    }
}