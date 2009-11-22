using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class IEnumerableTests
    {
        static readonly Random rnd = new Random();

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
    }
}