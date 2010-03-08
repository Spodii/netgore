using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class PriorityQueueTests
    {

        [Test]
        public void SimpleTest()
        {
            PriorityQueue<int> Test = new PriorityQueue<int>();

            Test.Push(5);
            Test.Push(12);
            Test.Push(1);

            Assert.IsTrue(Test[0] == 1,"Assert1 (should == 1) + " + Test[0].ToString());
            int i = Test.Pop();
            Assert.IsTrue(i == 1, "i should equal 1" + i.ToString());

            Assert.IsTrue(Test[0] == 5, "Assert2 (should == 5) + " + Test[0].ToString());
            i = Test.Pop();
            Assert.IsTrue(i == 5, "i should equal 5" + i.ToString());

            Assert.IsTrue(Test[0] == 12, "Assert3 (should == 12) + " + Test[0].ToString());
            i = Test.Pop();
            Assert.IsTrue(i == 12, "i should = 12" + i.ToString());

            Test.Push(11);
            Test.Push(10);
            Test.Push(9);
            Test.Push(8);

            Test.RemoveLocation(9);
            Assert.IsTrue(Test[0] == 8, "Assert5 (Should equal 8) + " + Test[0].ToString());

            Test.Push(11);
            Test.Push(10);
            Test.Push(9);
            Test.Push(8);

            Assert.IsTrue(Test.Count == 7, "Assert6 (Should equal 7) + " + Test.Count.ToString());
            Assert.IsTrue(Test.Peek() == 8, "Assert7 (Should equal 8) + " + Test.Peek().ToString());
            Assert.IsTrue(Test.Pop() == 8, "Assert8 (Should equal 8)");
            Assert.IsTrue(Test.Pop() == 8, "Assert8++ (should equal 8)");
            Assert.IsTrue(Test.Peek() == 9, "Assert9 (Should equal 9) + " + Test.Peek().ToString());
            Test.RemoveLocation(9);
            Assert.IsTrue(Test.Peek() == 10);

            Test.Clear();

            Assert.IsTrue(Test.Count == 0, "Assert10 (Should equal 0) + " + Test.Count.ToString());
        }
    }
}
