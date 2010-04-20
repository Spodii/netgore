using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture(Description = "These tests are to make sure the VirtualList behaves just like a normal List.")]
    public class VirtualListTests
    {
        static readonly object[] _testValues = new object[] { new object(), new object(), new object() };

        /// <summary>
        /// Checks that two ILists are the same.
        /// </summary>
        /// <param name="l">The first.</param>
        /// <param name="v">The second.</param>
        static void AssertEquality(IList<object> l, IList<object> v)
        {
            Assert.AreEqual(l.Count, v.Count);
            Assert.AreEqual(((IList)l).Count, ((IList)v).Count);
            Assert.AreEqual(((ICollection)l).Count, ((ICollection)v).Count);

            if (l.Count > 0)
            {
                for (var i = 0; i < l.Count; i++)
                {
                    Assert.AreSame(l[i], v[i]);
                    Assert.AreSame(((IList)l)[i], ((IList)v)[i]);

                    Assert.AreEqual(l.IndexOf(l[i]), v.IndexOf(v[i]));
                    Assert.AreEqual(((IList)l).IndexOf(l[i]), ((IList)v).IndexOf(v[i]));

                    Assert.AreEqual(l.Contains(l[i]), v.Contains(v[i]));
                    Assert.AreEqual(((IList)l).Contains(l[i]), ((IList)v).Contains(v[i]));
                }

                Assert.AreEqual(l.First(), v.First());
                Assert.AreEqual(l.Last(), v.Last());
            }
        }

        /// <summary>
        /// Performs an operation on a List and VirtualList and makes sure the outcome is the same.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        static void CollectionTOperation(Action<ICollection<object>> action)
        {
            var a = new List<object> { _testValues };
            var b = new VirtualList<object> { _testValues };

            action(a);
            action(b);

            AssertEquality(a, b);
        }

        /// <summary>
        /// Performs an operation on a List and VirtualList and makes sure the outcome is the same.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        static void ListOperation(Action<IList> action)
        {
            var a = new List<object> { _testValues };
            var b = new VirtualList<object> { _testValues };

            action(a);
            action(b);

            AssertEquality(a, b);
        }

        /// <summary>
        /// Performs an operation on a List and VirtualList and makes sure the outcome is the same.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        static void ListTOperation(Action<IList<object>> action)
        {
            var a = new List<object> { _testValues };
            var b = new VirtualList<object> { _testValues };

            action(a);
            action(b);

            AssertEquality(a, b);
        }

        #region Unit tests

        [Test]
        public void AddTest()
        {
            var o = new object();
            ListTOperation(x => x.Add(o));
        }

        [Test]
        public void ClearTest()
        {
            ListTOperation(x => x.Clear());
        }

        [Test]
        public void CollectionTAddTest()
        {
            var o = new object();
            CollectionTOperation(x => x.Add(o));
        }

        [Test]
        public void CollectionTClearTest()
        {
            CollectionTOperation(x => x.Clear());
        }

        [Test]
        public void CollectionTRemoveNewTest()
        {
            var o = new object();
            CollectionTOperation(x => x.Remove(o));
        }

        [Test]
        public void CollectionTRemoveTest()
        {
            CollectionTOperation(x => x.Remove(_testValues[0]));
        }

        [Test]
        public void Insert0Test()
        {
            var o = new object();
            ListTOperation(x => x.Insert(0, o));
        }

        [Test]
        public void Insert1Test()
        {
            var o = new object();
            ListTOperation(x => x.Insert(1, o));
        }

        [Test]
        public void Insert2Test()
        {
            var o = new object();
            Assert.Throws<ArgumentOutOfRangeException>(() => ListTOperation(x => x.Insert(2, o)));
        }

        [Test]
        public void Insert3Test()
        {
            var o = new object();
            Assert.Throws<ArgumentOutOfRangeException>(() => ListTOperation(x => x.Insert(3, o)));
        }

        [Test]
        public void ListAddTest()
        {
            var o = new object();
            ListOperation(x => x.Add(o));
        }

        [Test]
        public void ListClearTest()
        {
            ListOperation(x => x.Clear());
        }

        [Test]
        public void ListInsert0Test()
        {
            var o = new object();
            ListOperation(x => x.Insert(0, o));
        }

        [Test]
        public void ListInsert1Test()
        {
            var o = new object();
            ListOperation(x => x.Insert(1, o));
        }

        [Test]
        public void ListInsert2Test()
        {
            var o = new object();
            Assert.Throws<ArgumentOutOfRangeException>(() => ListOperation(x => x.Insert(2, o)));
        }

        [Test]
        public void ListInsert3Test()
        {
            var o = new object();
            Assert.Throws<ArgumentOutOfRangeException>(() => ListOperation(x => x.Insert(3, o)));
        }

        [Test]
        public void ListRemoveAtTest()
        {
            ListOperation(x => x.RemoveAt(0));
        }

        [Test]
        public void ListRemoveNewTest()
        {
            var o = new object();
            ListOperation(x => x.Remove(o));
        }

        [Test]
        public void ListRemoveTest()
        {
            ListOperation(x => x.Remove(_testValues[0]));
        }

        [Test]
        public void RemoveAtTest()
        {
            ListTOperation(x => x.RemoveAt(0));
        }

        [Test]
        public void RemoveNewTest()
        {
            var o = new object();
            ListTOperation(x => x.Remove(o));
        }

        [Test]
        public void RemoveTest()
        {
            ListTOperation(x => x.Remove(_testValues[0]));
        }

        #endregion
    }
}