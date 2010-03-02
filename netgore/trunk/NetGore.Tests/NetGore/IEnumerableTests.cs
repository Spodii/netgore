using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class IEnumerableTests
    {
        static readonly Random rnd = new Random();

        #region Unit tests

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
            List<int> l = null;
            Assert.IsTrue(l.IsEmpty());
        }

        [Test]
        public void ImplodeSplitWithCharTest()
        {
            var l = new List<int>(50);
            for (int i = 0; i < 50; i++)
            {
                l.Add(rnd.Next(0, 100));
            }

            string implode = l.Implode(',');

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (int i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i].ToString(), elements[i]);
            }
        }

        [Test]
        public void MinElementTest()
        {
            string[] s = new string[] { "asdf", "f", "asfkdljas", "sdf" };
            var r = s.MinElement(x => x.Length);
            Assert.AreEqual("f", r);
        }

        [Test]
        public void MaxElementTest()
        {
            string[] s = new string[] { "asdf", "f", "asfkdljas", "sdf" };
            var r = s.MaxElement(x => x.Length);
            Assert.AreEqual("asfkdljas", r);
        }

        [Test]
        public void MaxElementEmptyTest()
        {
            string[] s = new string[0];
            Assert.Throws<ArgumentException>(() => s.MaxElement(x => x.Length));
        }

        [Test]
        public void MinElementEmptyTest()
        {
            string[] s = new string[0];
            Assert.Throws<ArgumentException>(() => s.MinElement(x => x.Length));
        }

        [Test]
        public void ImplodeSplitWithStringTest()
        {
            var l = new List<int>(50);
            for (int i = 0; i < 50; i++)
            {
                l.Add(rnd.Next(0, 100));
            }

            string implode = l.Implode(",");

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (int i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i].ToString(), elements[i]);
            }
        }

        [Test]
        public void ImplodeStringsSplitWithCharTest()
        {
            var l = new List<string>(50);
            for (int i = 0; i < 50; i++)
            {
                l.Add(Parser.Current.ToString(rnd.Next(0, 100)));
            }

            string implode = l.Implode(',');

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (int i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i], elements[i]);
            }
        }

        [Test]
        public void ImplodeStringsSplitWithStringTest()
        {
            var l = new List<string>(50);
            for (int i = 0; i < 50; i++)
            {
                l.Add(Parser.Current.ToString(rnd.Next(0, 100)));
            }

            string implode = l.Implode(",");

            var elements = implode.Split(',');

            Assert.AreEqual(l.Count, elements.Length);

            for (int i = 0; i < l.Count; i++)
            {
                Assert.AreEqual(l[i], elements[i]);
            }
        }

        [Test]
        public void ToImmutableTest()
        {
            int[] i = new int[] { 1, 2, 3 };
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