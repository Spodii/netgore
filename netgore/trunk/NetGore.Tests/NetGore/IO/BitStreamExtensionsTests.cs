using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class BitStreamExtensionsTests
    {
        static void AreArraysEqual<T>(T[] a, T[] b)
        {
            Assert.AreEqual(a.Length, b.Length);
            for (var i = 0; i < a.Length; i++)
            {
                Assert.AreEqual(a[i], b[i]);
            }
        }

        #region Unit tests

        [Test]
        public void ByteTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new byte[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadBytes(v.Length));
        }

        [Test]
        public void DoubleTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new double[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadDoubles(v.Length));
        }

        [Test]
        public void FloatTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new float[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadFloats(v.Length));
        }

        [Test]
        public void IntTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new int[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadInts(v.Length));
        }

        [Test]
        public void LongTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new long[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadLongs(v.Length));
        }

        [Test]
        public void SByteTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new sbyte[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadSBytes(v.Length));
        }

        [Test]
        public void ShortTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new short[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadShorts(v.Length));
        }

        [Test]
        public void StringTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new string[] { "1", "5", "81", "12", "52" };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadStrings(v.Length));
        }

        [Test]
        public void UIntTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new uint[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadUInts(v.Length));
        }

        [Test]
        public void UShortTest()
        {
            var bs = new BitStream(BitStreamMode.Write, 512) { Mode = BitStreamMode.Write };
            var v = new ushort[] { 1, 5, 81, 12, 52 };
            bs.Write(v);
            bs.Mode = BitStreamMode.Read;
            AreArraysEqual(v, bs.ReadUShorts(v.Length));
        }

        #endregion
    }
}