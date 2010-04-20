using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class PriorityQueueTests
    {
        #region Unit tests

        [Test]
        public void SimpleTest()
        {
            var Test = new PriorityQueue<int>();

            Test.Push(5);
            Test.Push(12);
            Test.Push(1);

            Assert.IsTrue(Test[0] == 1, "Assert1 (should == 1) + " + Test[0]);
            var i = Test.Pop();
            Assert.IsTrue(i == 1, "i should equal 1" + i);

            Assert.IsTrue(Test[0] == 5, "Assert2 (should == 5) + " + Test[0]);
            i = Test.Pop();
            Assert.IsTrue(i == 5, "i should equal 5" + i);

            Assert.IsTrue(Test[0] == 12, "Assert3 (should == 12) + " + Test[0]);
            i = Test.Pop();
            Assert.IsTrue(i == 12, "i should = 12" + i);

            Test.Push(11);
            Test.Push(10);
            Test.Push(9);
            Test.Push(8);

            Test.RemoveLocation(9);
            Assert.IsTrue(Test[0] == 8, "Assert5 (Should equal 8) + " + Test[0]);

            Test.Push(11);
            Test.Push(10);
            Test.Push(9);
            Test.Push(8);

            Assert.IsTrue(Test.Count == 7, "Assert6 (Should equal 7) + " + Test.Count);
            Assert.IsTrue(Test.Peek() == 8, "Assert7 (Should equal 8) + " + Test.Peek());
            Assert.IsTrue(Test.Pop() == 8, "Assert8 (Should equal 8)");
            Assert.IsTrue(Test.Pop() == 8, "Assert8++ (should equal 8)");
            Assert.IsTrue(Test.Peek() == 9, "Assert9 (Should equal 9) + " + Test.Peek());
            Test.RemoveLocation(9);
            Assert.IsTrue(Test.Peek() == 10);

            Test.Clear();

            Assert.IsTrue(Test.Count == 0, "Assert10 (Should equal 0) + " + Test.Count);
        }

        #endregion
    }
}