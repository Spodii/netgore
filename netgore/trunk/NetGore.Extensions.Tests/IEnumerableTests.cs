using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Extensions.Tests
{
    [TestFixture]
    public class IEnumerableTests
    {
        [Test]
        public void ImplodeCharTest()
        {
            Random rnd = new Random();
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
        public void ImplodeStringTest()
        {
            Random rnd = new Random();
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
    }
}