using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class ByteArrayEqualityComparerTests
    {
        [Test]
        public void DifferentLengthTest()
        {
            byte[] a = new byte [] { 1, 2, 3, 4, 5 };
            byte[] b = new byte [] { 1, 2, 3, 4, 5, 6 };
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void NullTest1()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5 };
            byte[] b = null;
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void NullTest2()
        {
            byte[] a = null;
            byte[] b = new byte[] { 1, 2, 3, 4, 5, 6 };
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void NullTest3()
        {
            byte[] a = null;
            byte[] b = null;
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsTrue(actual);
        }

        [Test]
        public void EmptyTest1()
        {
            byte[] a = new byte[0];
            byte[] b = new byte[] { 1, 2, 3, 4, 5, 6 };
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void EmptyTest2()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5 };
            byte[] b = new byte[0]; 
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void EmptyTest3()
        {
            byte[] a = new byte[0];
            byte[] b = new byte[0]; 
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsTrue(actual);
        }

        [Test]
        public void SameLengthTest()
        {
            byte[] a = new byte[] { 1, 2, 3, 4 };
            byte[] b = new byte[] { 1, 2, 4, 4 };
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsFalse(actual);
        }

        [Test]
        public void SameTest1()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5, 6, 1, 3, 5 };
            byte[] b = a;
            bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
            Assert.IsTrue(actual);
        }

        [Test]
        public void RandomEqualTest()
        {
            SafeRandom rnd = new SafeRandom();
            for (int loop = 0; loop < 100000; loop++)
            {
                byte[] a = new byte[rnd.Next(1, 100)];
                rnd.NextBytes(a);

                byte[] b = DeepCopy(a);

                bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
                Assert.IsTrue(actual);
            }
        }

        [Test]
        public void RandomNotEqualTest()
        {
            SafeRandom rnd = new SafeRandom();
            for (int loop = 0; loop < 1000000; loop++)
            {
                byte[] a = new byte[rnd.Next(1, 100)];
                rnd.NextBytes(a);

                byte[] b = DeepCopy(a);

                b[rnd.Next(0, b.Length)]++; // Increment at a random index

                bool actual = ByteArrayEqualityComparer.AreEqual(a, b);
                Assert.IsFalse(actual);
            }
        }

        static byte[] DeepCopy(byte[] a)
        {
            byte[] b = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
                b[i] = a[i];
            return b;
        }
    }
}
