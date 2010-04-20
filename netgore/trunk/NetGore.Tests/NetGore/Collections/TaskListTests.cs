using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class TaskListTests
    {
        static void AssertContainSameElements<T>(TestTaskList<T> taskList, IEnumerable<T> expected)
        {
            var taskListItems = new List<T>();
            taskList.Perform(delegate(T item)
            {
                taskListItems.Add(item);
                return false;
            });

            Assert.IsTrue(taskListItems.ContainSameElements(expected));
        }

        #region Unit tests

        [Test]
        public void AddTest()
        {
            var t = new TestTaskList<int>();
            var expected = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                t.Add(i);
                expected.Add(i);
            }

            AssertContainSameElements(t, expected);
        }

        [Test]
        public void Remove2Test()
        {
            var t = new TestTaskList<int>();
            var expected = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                t.Add(i);
                expected.Add(i);
            }

            AssertContainSameElements(t, expected);

            t.Perform(x => x != 5);
            expected.RemoveAll(x => x != 5);

            AssertContainSameElements(t, expected);
        }

        [Test]
        public void RemoveFirstTest()
        {
            var t = new TestTaskList<int>();
            var expected = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                t.Add(i);
                expected.Add(i);
            }

            AssertContainSameElements(t, expected);

            t.Perform(x => x == 9);
            expected.RemoveAll(x => x == 9);

            AssertContainSameElements(t, expected);
        }

        [Test]
        public void RemoveLastTest()
        {
            var t = new TestTaskList<int>();
            var expected = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                t.Add(i);
                expected.Add(i);
            }

            AssertContainSameElements(t, expected);

            t.Perform(x => x == 0);
            expected.RemoveAll(x => x == 0);

            AssertContainSameElements(t, expected);
        }

        [Test]
        public void RemoveTest()
        {
            var t = new TestTaskList<int>();
            var expected = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                t.Add(i);
                expected.Add(i);
            }

            AssertContainSameElements(t, expected);

            t.Perform(x => x == 5);
            expected.Remove(5);

            AssertContainSameElements(t, expected);
        }

        #endregion

        class TestTaskList<T> : TaskList<T>
        {
            Func<T, bool> _func;

            public void Perform(Func<T, bool> f)
            {
                _func = f;
                Process();
                _func = null;
            }

            /// <summary>
            /// When overridden in the derived class, handles processing the given task.
            /// </summary>
            /// <param name="item">The value of the task to process.</param>
            /// <returns>True if the <paramref name="item"/> is to be removed from the collection; otherwise false.</returns>
            protected override bool ProcessItem(T item)
            {
                return _func(item);
            }
        }
    }
}