using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Globalization;
using NUnit.Framework;

namespace NetGore.Tests
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
    }
}