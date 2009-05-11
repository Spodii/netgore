using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

#pragma warning disable 219
#pragma warning disable 168
// ReSharper disable RedundantAssignment

namespace NetGore.Collections.Tests
{
    [TestFixture]
    public class BoolArrayTests
    {
        static void ResizeTestValueValidate(BoolArray b, int newSize)
        {
            b.Resize(newSize - 1);
            b.SetAll(true);
            b.Resize(newSize);

            // Make sure the new element is false
            Assert.IsFalse(b[newSize - 1]);

            // Make sure all the existing elements remain true
            for (int i = 0; i < newSize - 1; i++)
            {
                Assert.IsTrue(b[i]);
            }
        }

        [Test]
        public void ConstructorTest()
        {
            BoolArray b = new BoolArray(1);
            b = new BoolArray(7);
            b = new BoolArray(8);
            b = new BoolArray(9);
            b = new BoolArray(10000);

            try
            {
                b = new BoolArray(0);
                Assert.Fail("Failed to generate ArgumentOutOfRangeException for negative starting sizes.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                b = new BoolArray(-1);
                Assert.Fail("Failed to generate ArgumentOutOfRangeException for negative starting sizes.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                b = new BoolArray(-100);
                Assert.Fail("Failed to generate ArgumentOutOfRangeException for negative starting sizes.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [Test]
        public void GetSetTest()
        {
            const int iterations = 1024;

            BoolArray b = new BoolArray(iterations);

            for (int i = 0; i < iterations; i++)
            {
                b[i] = (i % 2 == 0);
                Assert.AreEqual(b[i], i % 2 == 0);

                b[i] = (i % 2 != 0);
                Assert.AreEqual(b[i], i % 2 != 0);
            }

            for (int i = 0; i < iterations; i++)
            {
                b[i] = false;
                Assert.AreEqual(b[i], false);

                b[i] = true;
                Assert.AreEqual(b[i], true);
            }
        }

        [Test]
        public void IEnumerableTest()
        {
            const int size = 100;
            BoolArray b = new BoolArray(size);

            // Tests on an all-false collection
            b.SetAll(false);

            var values = b.Where(value => value);
            Assert.AreEqual(0, values.Count());

            values = b.Where(value => !value);
            Assert.AreEqual(size, values.Count());

            // All-true collection
            b.SetAll(true);

            values = b.Where(value => !value);
            Assert.AreEqual(0, values.Count());

            values = b.Where(value => value);
            Assert.AreEqual(size, values.Count());

            // Some-true, some-false collection
            int numTrue = 0;
            int numFalse = 0;

            for (int i = 0; i < size; i++)
            {
                if ((i % 3) == 0)
                {
                    b[i] = true;
                    numTrue++;
                }
                else
                {
                    b[i] = false;
                    numFalse++;
                }
            }

            values = b.Where(value => value);
            Assert.AreEqual(numTrue, values.Count());

            values = b.Where(value => !value);
            Assert.AreEqual(numFalse, values.Count());
        }

        [Test]
        public void IndexOutOfRangeExceptionTest()
        {
            BoolArray b = new BoolArray(8);
            var testValues = new int[] { -10000, -100, -1, 8, 9, 100, 1000, 100000 };

            foreach (int value in testValues)
            {
                try
                {
                    bool r = b[value];
                    const string errmsg = "Failed to generate IndexOutOfRangeException for BoolArray[{0}].";
                    Assert.Fail(errmsg, value);
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
        }

        [Test]
        public void ModifiedCollectionTest()
        {
            BoolArray b = new BoolArray(100);

            foreach (bool value in b)
            {
            }

            try
            {
                foreach (bool value in b)
                {
                    b[0] = true;
                }
                Assert.Fail("Failed to generate InvalidOperationException for modified collection.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void ResizeTest()
        {
            BoolArray b = new BoolArray(1);

            // Validate the length
            b.Resize(10);
            Assert.AreEqual(10, b.Length);
            b.Resize(20);
            Assert.AreEqual(20, b.Length);

            // Make sure that when we resize, all new elements are false and existing elements are unaltered
            for (int i = 4; i < 40; i++)
            {
                ResizeTestValueValidate(b, i);
            }

            // Make sure existing values are not lost on scaling down
            b.Resize(20);
            b.SetAll(true);
            b.Resize(5);
            Assert.IsTrue(b.All(value => value));
        }

        [Test]
        public void SetAllTest()
        {
            const int size = 100;
            BoolArray b = new BoolArray(size);

            for (int loops = 0; loops < 3; loops++)
            {
                b.SetAll(true);
                for (int i = 0; i < size; i++)
                {
                    Assert.AreEqual(b[i], true);
                }

                b.SetAll(false);
                for (int i = 0; i < size; i++)
                {
                    Assert.AreEqual(b[i], false);
                }
            }
        }

        [Test]
        public void SizeOneTest()
        {
            BoolArray b = new BoolArray(1);
            b[0] = true;
            b[0] = b[0];

            try
            {
                b[1] = false;
                Assert.Fail("Failed to generate IndexOutOfRangeException.");
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        [Test]
        public void ToArrayTest()
        {
            const int size = 99;
            BoolArray b = new BoolArray(size);
            b.SetAll(true);

            var a = b.ToArray();

            // Validate length
            Assert.AreEqual(b.Length, a.Length);

            // Validate values
            for (int i = 0; i < a.Length; i++)
            {
                Assert.AreEqual(b[i], a[i]);
            }
        }
    }
}