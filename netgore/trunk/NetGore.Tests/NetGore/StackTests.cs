using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class StackTests
    {
        #region Unit tests

        [Test]
        public void SortAscTest()
        {
            var rnd = new Random();
            var s = new Stack<int>();

            for (var i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.Sort();

            Assert.AreEqual(100, s.Count);

            var last = s.Pop();
            while (s.Count > 0)
            {
                var popped = s.Pop();
                Assert.LessOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortAscWhereTest()
        {
            var rnd = new Random();
            var s = new Stack<int>();

            for (var i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.Sort(item => item > 500);

            var last = s.Pop();

            Assert.LessOrEqual(500, last);

            while (s.Count > 0)
            {
                var popped = s.Pop();
                Assert.LessOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortDescTest()
        {
            var rnd = new Random();
            var s = new Stack<int>();

            for (var i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.SortDescending();

            Assert.AreEqual(100, s.Count);

            var last = s.Pop();
            while (s.Count > 0)
            {
                var popped = s.Pop();
                Assert.GreaterOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortDescWhereTest()
        {
            var rnd = new Random();
            var s = new Stack<int>();

            for (var i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.SortDescending(item => item < 500);

            var last = s.Pop();

            Assert.GreaterOrEqual(500, last);

            while (s.Count > 0)
            {
                var popped = s.Pop();
                Assert.GreaterOrEqual(last, popped);
                last = popped;
            }
        }

        #endregion
    }
}