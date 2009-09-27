using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void SortAscTest()
        {
            Random rnd = new Random();
            var s = new Stack<int>();

            for (int i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.Sort();

            Assert.AreEqual(100, s.Count);

            int last = s.Pop();
            while (s.Count > 0)
            {
                int popped = s.Pop();
                Assert.LessOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortAscWhereTest()
        {
            Random rnd = new Random();
            var s = new Stack<int>();

            for (int i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.Sort(item => item > 500);

            int last = s.Pop();

            Assert.LessOrEqual(500, last);

            while (s.Count > 0)
            {
                int popped = s.Pop();
                Assert.LessOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortDescTest()
        {
            Random rnd = new Random();
            var s = new Stack<int>();

            for (int i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.SortDescending();

            Assert.AreEqual(100, s.Count);

            int last = s.Pop();
            while (s.Count > 0)
            {
                int popped = s.Pop();
                Assert.GreaterOrEqual(last, popped);
                last = popped;
            }
        }

        [Test]
        public void SortDescWhereTest()
        {
            Random rnd = new Random();
            var s = new Stack<int>();

            for (int i = 0; i < 100; i++)
            {
                s.Push(rnd.Next(0, 1000));
            }

            s.SortDescending(item => item < 500);

            int last = s.Pop();

            Assert.GreaterOrEqual(500, last);

            while (s.Count > 0)
            {
                int popped = s.Pop();
                Assert.GreaterOrEqual(last, popped);
                last = popped;
            }
        }
    }
}