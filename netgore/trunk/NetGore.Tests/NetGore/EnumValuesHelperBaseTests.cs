using System.Linq;
using NetGore.IO;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests
{
    [TestFixture]
    public class EnumValuesHelperBaseTests
    {
        enum EVByte : byte
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        class EVHByte : EnumValuesHelperBase<EVByte>
        {
            static readonly EVHByte _instance;

            public static EVHByte Instance
            {
                get { return _instance; }
            }

            static EVHByte()
            {
                _instance = new EVHByte();
            }

            EVHByte()
            {
            }

            protected override EVByte FromInt(int value)
            {
                return (EVByte)value;
            }

            protected override int ToInt(EVByte value)
            {
                return (int)value;
            }
        }

        enum EVSByte : byte
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        class EVHSByte : EnumValuesHelperBase<EVSByte>
        {
            static readonly EVHSByte _instance;

            public static EVHSByte Instance
            {
                get { return _instance; }
            }

            static EVHSByte()
            {
                _instance = new EVHSByte();
            }

            EVHSByte()
            {
            }

            protected override EVSByte FromInt(int value)
            {
                return (EVSByte)value;
            }

            protected override int ToInt(EVSByte value)
            {
                return (int)value;
            }
        }

        enum EVShort : byte
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        class EVHShort : EnumValuesHelperBase<EVShort>
        {
            static readonly EVHShort _instance;

            public static EVHShort Instance
            {
                get { return _instance; }
            }

            static EVHShort()
            {
                _instance = new EVHShort();
            }

            EVHShort()
            {
            }

            protected override EVShort FromInt(int value)
            {
                return (EVShort)value;
            }

            protected override int ToInt(EVShort value)
            {
                return (int)value;
            }
        }

        enum EVUShort : byte
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        class EVHUShort : EnumValuesHelperBase<EVUShort>
        {
            static readonly EVHUShort _instance;

            public static EVHUShort Instance
            {
                get { return _instance; }
            }

            static EVHUShort()
            {
                _instance = new EVHUShort();
            }

            EVHUShort()
            {
            }

            protected override EVUShort FromInt(int value)
            {
                return (EVUShort)value;
            }

            protected override int ToInt(EVUShort value)
            {
                return (int)value;
            }
        }

        class EVHInt : EnumValuesHelperBase<EVInt>
        {
            static readonly EVHInt _instance;

            public static EVHInt Instance
            {
                get { return _instance; }
            }

            static EVHInt()
            {
                _instance = new EVHInt();
            }

            EVHInt()
            {
            }

            protected override EVInt FromInt(int value)
            {
                return (EVInt)value;
            }

            protected override int ToInt(EVInt value)
            {
                return (int)value;
            }
        }

        enum EVInt
        {
            A = -100,
            B = 0,
            C,
            D,
            E,
            F = 100,
            G,
            H
        }

        const int _initialBufferSize = 512;

        [Test]
        public void ByteIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHByte i = EVHByte.Instance;
            var testValues = new EVByte[] { EVByte.A, EVByte.D, EVByte.H, EVByte.C, EVByte.A, EVByte.B, EVByte.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.Write(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.Read(bs));
            }
        }

        [Test]
        public void IntIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHInt i = EVHInt.Instance;
            var testValues = new EVInt[] { EVInt.A, EVInt.D, EVInt.H, EVInt.C, EVInt.A, EVInt.B, EVInt.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.Write(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.Read(bs));
            }
        }

        [Test]
        public void SByteIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHSByte i = EVHSByte.Instance;
            var testValues = new EVSByte[] { EVSByte.A, EVSByte.D, EVSByte.H, EVSByte.C, EVSByte.A, EVSByte.B, EVSByte.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.Write(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.Read(bs));
            }
        }

        [Test]
        public void ShortIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHShort i = EVHShort.Instance;
            var testValues = new EVShort[] { EVShort.A, EVShort.D, EVShort.H, EVShort.C, EVShort.A, EVShort.B, EVShort.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.Write(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.Read(bs));
            }
        }

        [Test]
        public void UShortIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHUShort i = EVHUShort.Instance;
            var testValues = new EVUShort[] { EVUShort.A, EVUShort.D, EVUShort.H, EVUShort.C, EVUShort.A, EVUShort.B, EVUShort.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.Write(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.Read(bs));
            }
        }
    }
}