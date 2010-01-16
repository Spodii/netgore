using System;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class EnumHelperTests
    {
        const int _initialBufferSize = 512;

        static readonly Random r = new Random();

        [Test]
        public void ByteIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHByte i = EVHByte.Instance;
            var testValues = new EVByte[] { EVByte.A, EVByte.D, EVByte.H, EVByte.C, EVByte.A, EVByte.B, EVByte.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.WriteValue(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.ReadValue(bs));
            }
        }

        static string CreateRandomString()
        {
            int length = r.Next(2, 10);
            var ret = new char[length];

            for (int i = 0; i < ret.Length; i++)
            {
                int j;
                if (i == 0)
                    j = r.Next(1, 3); // Don't let the first character be numeric
                else
                    j = r.Next(0, 3);

                int min;
                int max;

                // ReSharper disable RedundantCast
                switch (j)
                {
                    case 0:
                        min = (int)'0';
                        max = (int)'9';
                        break;

                    case 1:
                        min = (int)'a';
                        max = (int)'z';
                        break;

                    default:
                        min = (int)'A';
                        max = (int)'Z';
                        break;
                }
                // ReSharper restore RedundantCast

                ret[i] = (char)r.Next(min, max + 1);
            }

            return new string(ret);
        }

        [Test]
        public void IntIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, _initialBufferSize);
            EVHInt i = EVHInt.Instance;
            var testValues = new EVInt[] { EVInt.A, EVInt.D, EVInt.H, EVInt.C, EVInt.A, EVInt.B, EVInt.B };

            for (int j = 0; j < testValues.Length; j++)
            {
                i.WriteValue(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.ReadValue(bs));
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
                i.WriteValue(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.ReadValue(bs));
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
                i.WriteValue(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.ReadValue(bs));
            }
        }

        [Test]
        public void TryParseCaseInsensitiveTests()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            foreach (string name in names)
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (string name in names.Select(x => x.ToUpper()))
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name, true), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (string name in names.Select(x => x.ToLower()))
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name, true), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }
        }

        [Test]
        public void TryParseCaseSensitiveTests()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            foreach (string name in names)
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, false, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name, false), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, false), outValue);
            }

            foreach (string name in names)
            {
                string nameUpper = name.ToUpper();
                if (name.Equals(nameUpper, StringComparison.CurrentCulture))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(nameUpper, false, out outValue));
            }

            foreach (string name in names)
            {
                string nameLower = name.ToLower();
                if (name.Equals(nameLower, StringComparison.CurrentCulture))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(nameLower, false, out outValue));
            }
        }

        [Test]
        public void TryParseRandomStrings()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            for (int i = 0; i < 5000; i++)
            {
                string s = CreateRandomString();
                if (names.Contains(s, StringComparer.OrdinalIgnoreCase))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(s, true, out outValue), "String: " + s);
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(s, false, out outValue), "String: " + s);
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
                i.WriteValue(bs, testValues[j]);
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < testValues.Length; j++)
            {
                Assert.AreEqual(testValues[j], i.ReadValue(bs));
            }
        }

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

        sealed class EVHByte : EnumIOHelper<EVByte>
        {
            static readonly EVHByte _instance;

            static EVHByte()
            {
                _instance = new EVHByte();
            }

            EVHByte()
            {
            }

            public static EVHByte Instance
            {
                get { return _instance; }
            }

            public override EVByte FromInt(int value)
            {
                return (EVByte)value;
            }

            public override int ToInt(EVByte value)
            {
                return (int)value;
            }
        }

        sealed class EVHInt : EnumIOHelper<EVInt>
        {
            static readonly EVHInt _instance;

            static EVHInt()
            {
                _instance = new EVHInt();
            }

            EVHInt()
            {
            }

            public static EVHInt Instance
            {
                get { return _instance; }
            }

            public override EVInt FromInt(int value)
            {
                return (EVInt)value;
            }

            public override int ToInt(EVInt value)
            {
                return (int)value;
            }
        }

        sealed class EVHSByte : EnumIOHelper<EVSByte>
        {
            static readonly EVHSByte _instance;

            static EVHSByte()
            {
                _instance = new EVHSByte();
            }

            EVHSByte()
            {
            }

            public static EVHSByte Instance
            {
                get { return _instance; }
            }

            public override EVSByte FromInt(int value)
            {
                return (EVSByte)value;
            }

            public override int ToInt(EVSByte value)
            {
                return (int)value;
            }
        }

        sealed class EVHShort : EnumIOHelper<EVShort>
        {
            static readonly EVHShort _instance;

            static EVHShort()
            {
                _instance = new EVHShort();
            }

            EVHShort()
            {
            }

            public static EVHShort Instance
            {
                get { return _instance; }
            }

            public override EVShort FromInt(int value)
            {
                return (EVShort)value;
            }

            public override int ToInt(EVShort value)
            {
                return (int)value;
            }
        }

        sealed class EVHUShort : EnumIOHelper<EVUShort>
        {
            static readonly EVHUShort _instance;

            static EVHUShort()
            {
                _instance = new EVHUShort();
            }

            EVHUShort()
            {
            }

            public static EVHUShort Instance
            {
                get { return _instance; }
            }

            public override EVUShort FromInt(int value)
            {
                return (EVUShort)value;
            }

            public override int ToInt(EVUShort value)
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

        enum TestEnum
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            h,
            i,
            jk,
            lmno,
            p
        }
    }
}