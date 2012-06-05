using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class TimeExpirationQueueTests
    {
        #region Unit tests

        [Test]
        public void AddTest()
        {
            var q = new TestQ();
            Assert.IsTrue(q.Items.IsEmpty());

            q.Add("a", 0, 10);
            var a = q.Items.First(x => x.Key == "a");
            Assert.AreEqual("a", a.Key);
            Assert.AreEqual(10, a.Value);

            q.Add("b", 0, 25);
            a = q.Items.First(x => x.Key == "a");
            var b = q.Items.First(x => x.Key == "b");
            Assert.AreEqual("a", a.Key);
            Assert.AreEqual(10, a.Value);
            Assert.AreEqual("b", b.Key);
            Assert.AreEqual(25, b.Value);

            q.Add("a", 0, 50);
            a = q.Items.First(x => x.Key == "a");
            b = q.Items.First(x => x.Key == "b");
            Assert.AreEqual("a", a.Key);
            Assert.AreEqual(50, a.Value);
            Assert.AreEqual("b", b.Key);
            Assert.AreEqual(25, b.Value);
        }

        [Test]
        public void RemoveTest()
        {
            var q = new TestQ();

            q.Add("abc", 0, 10);
            Assert.AreEqual("abc", q.Items.First().Key);

            q.Remove("abcd");
            Assert.AreEqual("abc", q.Items.First().Key);
            Assert.AreEqual(1, q.Items.Count());

            q.Remove("abc");
            Assert.IsTrue(q.Items.IsEmpty());
        }

        [Test]
        public void UpdateTest()
        {
            var q = new TestQ();
            q.Add("b", 0, 50);
            q.Add("a", 0, 10);
            q.Add("c", 0, 100);
            Assert.AreEqual(3, q.Items.Count());

            q.Update(0);
            Assert.AreEqual(3, q.Items.Count());
            Assert.IsTrue(q.Items.Any(x => x.Key == "a"));
            Assert.IsTrue(q.Items.Any(x => x.Key == "b"));
            Assert.IsTrue(q.Items.Any(x => x.Key == "c"));
            Assert.IsTrue(q.GetRecentExpired().IsEmpty());

            q.Update(10);
            Assert.AreEqual(2, q.Items.Count());
            Assert.IsTrue(q.Items.Any(x => x.Key == "b"));
            Assert.IsTrue(q.Items.Any(x => x.Key == "c"));
            Assert.IsTrue(q.GetRecentExpired().All(x => x == "a"));
            Assert.IsTrue(q.GetRecentExpired().IsEmpty());

            q.Update(49);
            Assert.AreEqual(2, q.Items.Count());
            Assert.IsTrue(q.Items.Any(x => x.Key == "b"));
            Assert.IsTrue(q.Items.Any(x => x.Key == "c"));
            Assert.IsTrue(q.GetRecentExpired().IsEmpty());

            q.Update(51);
            Assert.AreEqual(1, q.Items.Count());
            Assert.IsTrue(q.GetRecentExpired().All(x => x == "b"));
            Assert.IsTrue(q.Items.Any(x => x.Key == "c"));

            q.Update(99);
            Assert.AreEqual(1, q.Items.Count());
            Assert.IsTrue(q.Items.Any(x => x.Key == "c"));

            q.Update(1000);
            Assert.IsTrue(q.GetRecentExpired().All(x => x == "c"));
            Assert.IsTrue(q.Items.IsEmpty());
        }

        #endregion

        class TestQ : TimeExpirationQueue<string>
        {
            readonly List<string> _expired = new List<string>();

            /// <summary>
            /// When overridden in the derived class, gets the minimum amount of time in milliseconds that must elapsed
            /// between calls to Update. If this amount of time has not elapsed, calls to Update will just return 0.
            /// </summary>
            protected override TickCount UpdateRate
            {
                get { return 0; }
            }

            /// <summary>
            /// When overridden in the derived class, handles when an item has expired since it has been in this collection
            /// for longer the allowed time.
            /// </summary>
            /// <param name="item">The item that has expired.</param>
            protected override void ExpireItem(string item)
            {
                _expired.Add(item);
            }

            /// <summary>
            /// Gets the items that have expired since the last call to this method.
            /// </summary>
            /// <returns>The items that have expired since the last call to this method.</returns>
            public IEnumerable<string> GetRecentExpired()
            {
                var ret = _expired.ToArray();
                _expired.Clear();
                return ret;
            }
        }
    }
}