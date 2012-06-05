using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class IEnumerableTests
    {
        static readonly SafeRandom rnd = new SafeRandom();

        #region Unit tests

        [Test]
        public void ContainsSameElementsTest01()
        {
            var a = new int[] { };
            var b = new int[] { };
            Assert.IsTrue(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest02()
        {
            var a = new int[] { 0 };
            var b = new int[] { };
            Assert.IsFalse(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest03()
        {
            var a = new int[] { };
            var b = new int[] { 0 };
            Assert.IsFalse(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest04()
        {
            var a = new int[] { 0 };
            var b = new int[] { 0 };
            Assert.IsTrue(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest05()
        {
            var a = new int[] { 4, 3, 2, 1 };
            var b = new int[] { 1, 3, 2, 4 };
            Assert.IsTrue(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest06()
        {
            var a = new int[] { 4, 3, 2, 1, 4 };
            var b = new int[] { 1, 3, 2, 4 };
            Assert.IsFalse(a.ContainSameElements(b));
        }

        [Test]
        public void ContainsSameElementsTest07()
        {
            var a = new int[] { 4, 4, 3, 3 };
            var b = new int[] { 4, 4, 4, 3 };
            Assert.IsFalse(a.ContainSameElements(b));
        }

        [Test]
        public void HasDuplicatesTest01()
        {
            var a = new int[] { 0, 1, 2, 3 };
            Assert.IsFalse(a.HasDuplicates());
        }

        [Test]
        public void HasDuplicatesTest02()
        {
            var a = new int[] { 0 };
            Assert.IsFalse(a.HasDuplicates());
        }

        [Test]
        public void HasDuplicatesTest03()
        {
            var a = new int[] { 0, 0 };
            Assert.IsTrue(a.HasDuplicates());
        }

        [Test]
        public void HasDuplicatesTest04()
        {
            var a = new int[] { 0, 0, 1 };
            Assert.IsTrue(a.HasDuplicates());
        }

        [Test]
        public void HasDuplicatesTest05()
        {
            var a = new int[] { 0, 1, 2, 0 };
            Assert.IsTrue(a.HasDuplicates());
        }

        [Test]
        public void HasDuplicatesTest06()
        {
            var a = new int[] { 0, 1, 2, 0, 2, 2 };
            Assert.IsTrue(a.HasDuplicates());
        }

        [Test]
        public void ImplodeSplitWithCharTest()
        {
            var l = new List<int>(50);
            for (var i = 0; i < 50; i++)
            {
                l.Add(rnd.Next(0, 100));
            }

            var implode = l.Implode(',');

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (var i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i].ToString(), elements[i]);
            }
        }

        [Test]
        public void ImplodeSplitWithStringTest()
        {
            var l = new List<int>(50);
            for (var i = 0; i < 50; i++)
            {
                l.Add(rnd.Next(0, 100));
            }

            var implode = l.Implode(",");

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (var i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i].ToString(), elements[i]);
            }
        }

        [Test]
        public void ImplodeStringsSplitWithCharTest()
        {
            var l = new List<string>(50);
            for (var i = 0; i < 50; i++)
            {
                l.Add(Parser.Current.ToString(rnd.Next(0, 100)));
            }

            var implode = l.Implode(',');

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (var i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i], elements[i]);
            }
        }

        [Test]
        public void ImplodeStringsSplitWithStringTest()
        {
            var l = new List<string>(50);
            for (var i = 0; i < 50; i++)
            {
                l.Add(Parser.Current.ToString(rnd.Next(0, 100)));
            }

            var implode = l.Implode(",");

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (var i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i], elements[i]);
            }
        }

        [Test]
        public void IsEmptyOnEmptyCollectionsTest()
        {
            Assert.IsTrue(new List<int>().IsEmpty());
            Assert.IsTrue(new int[0].IsEmpty());
            Assert.IsTrue(string.Empty.IsEmpty());
            Assert.IsTrue(new Dictionary<int, int>().IsEmpty());
            Assert.IsTrue(new HashSet<int>().IsEmpty());
            Assert.IsTrue(new Stack<int>().IsEmpty());
            Assert.IsTrue(new Queue<int>().IsEmpty());
        }

        [Test]
        public void IsEmptyOnNonEmptyCollectionsTest()
        {
            Assert.IsFalse(new List<int> { 1, 2, 3 }.IsEmpty());
            Assert.IsFalse(new int[] { 1, 2, 3 }.IsEmpty());
            Assert.IsFalse("asdlkfjasldkfj".IsEmpty());
            Assert.IsFalse(new Dictionary<int, int> { { 5, 1 }, { 3, 4 } }.IsEmpty());
            Assert.IsFalse(new HashSet<int> { 1, 2, 3, 4, 5 }.IsEmpty());
        }

        [Test]
        public void IsEmptyOnNullTest()
        {
            const List<int> l = null;
            Assert.IsTrue(l.IsEmpty());
        }

        [Test]
        public void MaxElementEmptyTest()
        {
            var s = new string[0];
            Assert.Throws<ArgumentException>(() => s.MaxElement(x => x.Length));
            Assert.IsNull(s.MaxElementOrDefault(x => x.Length));
        }

        [Test]
        public void MaxElementTest()
        {
            var s = new string[] { "asdf", "f", "asfkdljas", "sdf" };
            var r = s.MaxElement(x => x.Length);
            Assert.AreEqual("asfkdljas", r);
        }

        [Test]
        public void MinElementEmptyTest()
        {
            var s = new string[0];
            Assert.Throws<ArgumentException>(() => s.MinElement(x => x.Length));
            Assert.IsNull(s.MinElementOrDefault(x => x.Length));
        }

        [Test]
        public void MinElementTest()
        {
            var s = new string[] { "asdf", "f", "asfkdljas", "sdf" };
            var r = s.MinElement(x => x.Length);
            Assert.AreEqual("f", r);
        }

        [Test]
        public void NextFreeValueEmptyTestA()
        {
            var values = new int[0];
            Assert.AreEqual(0, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueEmptyTestB()
        {
            var values = new int[0];
            Assert.AreEqual(10, values.NextFreeValue(10));
        }

        [Test]
        public void NextFreeValueTestA()
        {
            var values = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(7, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestB()
        {
            var values = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(0, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestC()
        {
            var values = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(7, values.NextFreeValue(1));
        }

        [Test]
        public void NextFreeValueTestD()
        {
            var values = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(10, values.NextFreeValue(10));
        }

        [Test]
        public void NextFreeValueTestE()
        {
            var values = new int[] { 0, 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(-10, values.NextFreeValue(-10));
        }

        [Test]
        public void NextFreeValueTestF()
        {
            var values = new int[] { 0, 1, 2, 4, 5, 6 };
            Assert.AreEqual(3, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestG()
        {
            var values = new int[] { 0, 1, 2, 4, 6 };
            Assert.AreEqual(3, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestH()
        {
            var values = new int[] { 0, 1, 2, 3, 4, 5 };
            Assert.AreEqual(6, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestI()
        {
            var values = new int[] { 0, 0, 0, 0, 1, 1, 1, 2, 5, 6 };
            Assert.AreEqual(3, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestJ()
        {
            var values = new int[] { 0, 0, 0, 2 };
            Assert.AreEqual(1, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestK()
        {
            var values = new int[] { 0, 1, 1, 1, 3, 3, 3 };
            Assert.AreEqual(2, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestL()
        {
            var values = new int[] { 0, 1, 1, 1, 3 };
            Assert.AreEqual(2, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestM()
        {
            var values = new int[] { 0, 1, 1, 1 };
            Assert.AreEqual(2, values.NextFreeValue());
        }

        [Test]
        public void NextFreeValueTestN()
        {
            var values = new int[] { 5, 4, 3, 2, 1, 0 };
            Assert.AreEqual(6, values.NextFreeValue());
        }

        [Test]
        public void ToImmutableTest()
        {
            var i = new List<int> { 1, 2, 3 };
            var e = i.ToImmutable();

            i[0] = 50;

            Assert.AreEqual(1, e.ToArray()[0]);
            Assert.AreEqual(2, e.ToArray()[1]);
            Assert.AreEqual(3, e.ToArray()[2]);

            Assert.AreEqual(50, i[0]);

            i[2] = 22;

            Assert.AreEqual(1, e.ToArray()[0]);
            Assert.AreEqual(2, e.ToArray()[1]);
            Assert.AreEqual(3, e.ToArray()[2]);

            Assert.AreEqual(22, i[2]);
        }

        #endregion
    }
}