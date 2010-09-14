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
        static readonly SafeRandom r = new SafeRandom();

        static string CreateRandomString()
        {
            var length = r.Next(2, 10);
            var ret = new char[length];

            for (var i = 0; i < ret.Length; i++)
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

        #region Unit tests

        [Test]
        public void BitsRequiredTest()
        {
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum1>.MaxValue), EnumHelper<BitsReqEnum1>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum2>.MaxValue), EnumHelper<BitsReqEnum2>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum3>.MaxValue), EnumHelper<BitsReqEnum3>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum4>.MaxValue), EnumHelper<BitsReqEnum4>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum5>.MaxValue), EnumHelper<BitsReqEnum5>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum6>.MaxValue), EnumHelper<BitsReqEnum6>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum7>.MaxValue), EnumHelper<BitsReqEnum7>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum8>.MaxValue), EnumHelper<BitsReqEnum8>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum9>.MaxValue), EnumHelper<BitsReqEnum9>.BitsRequired);
            Assert.AreEqual(BitOps.RequiredBits((uint)EnumHelper<BitsReqEnum10>.MaxValue), EnumHelper<BitsReqEnum10>.BitsRequired);
        }

        [Test]
        public void ExceptionWhenNotSupportsCastOperationsTest()
        {
#pragma warning disable 219
            object o;
#pragma warning restore 219

            var bs = new BitStream(128);

            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.BitsRequired);
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.MaxValue);
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.MinValue);
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.GetToIntFunc());
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.GetFromIntFunc());
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.FromInt(1));
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.ToInt(EVULong.A));
            Assert.Throws<MethodAccessException>(() => EnumHelper<EVULong>.WriteValue(bs, EVULong.A));
            Assert.Throws<MethodAccessException>(() => EnumHelper<EVULong>.WriteValue(bs, "test", EVULong.A));

            bs.PositionBits = 0;

            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.ReadValue(bs));
            Assert.Throws<MethodAccessException>(() => o = EnumHelper<EVULong>.ReadValue(bs, "test"));
        }

        [Test]
        public void FromIntTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVByte>.FromInt((int)v));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVSByte>.FromInt((int)v));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVShort>.FromInt((int)v));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUShort>.FromInt((int)v));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVInt>.FromInt((int)v));
            }
        }

        [Test]
        public void InvalidEnumTypeTest()
        {
#pragma warning disable 219
            bool b;
#pragma warning restore 219

            Assert.Throws<TypeInitializationException>(() => b = EnumHelper<int>.Values != null);
        }

        [Test]
        public void IsDefinedInvalidValuesTest()
        {
            for (var i = 50; i < 80; i++)
            {
                Assert.IsFalse(EnumHelper<EVByte>.IsDefined((EVByte)i));
                Assert.IsFalse(EnumHelper<EVSByte>.IsDefined((EVSByte)i));
                Assert.IsFalse(EnumHelper<EVShort>.IsDefined((EVShort)i));
                Assert.IsFalse(EnumHelper<EVUShort>.IsDefined((EVUShort)i));
                Assert.IsFalse(EnumHelper<EVInt>.IsDefined((EVInt)i));
                Assert.IsFalse(EnumHelper<EVUInt>.IsDefined((EVUInt)i));
                Assert.IsFalse(EnumHelper<EVLong>.IsDefined((EVLong)i));
                Assert.IsFalse(EnumHelper<EVULong>.IsDefined((EVULong)i));
            }
        }

        [Test]
        public void IsDefinedValidValuesTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.IsTrue(EnumHelper<EVByte>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.IsTrue(EnumHelper<EVSByte>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.IsTrue(EnumHelper<EVShort>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.IsTrue(EnumHelper<EVUShort>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.IsTrue(EnumHelper<EVInt>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                Assert.IsTrue(EnumHelper<EVLong>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                Assert.IsTrue(EnumHelper<EVULong>.IsDefined(v));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                Assert.IsTrue(EnumHelper<EVUInt>.IsDefined(v));
            }
        }

        [Test]
        public void MaxValueTest()
        {
            Assert.AreEqual((int)BitsReqEnum1.a, EnumHelper<BitsReqEnum1>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum2.b, EnumHelper<BitsReqEnum2>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum3.c, EnumHelper<BitsReqEnum3>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum4.d, EnumHelper<BitsReqEnum4>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum5.e, EnumHelper<BitsReqEnum5>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum6.f, EnumHelper<BitsReqEnum6>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum7.g, EnumHelper<BitsReqEnum7>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum8.h, EnumHelper<BitsReqEnum8>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum9.i, EnumHelper<BitsReqEnum9>.MaxValue);
            Assert.AreEqual((int)BitsReqEnum10.j, EnumHelper<BitsReqEnum10>.MaxValue);
        }

        [Test]
        public void MinValueTest()
        {
            Assert.AreEqual((int)BitsReqEnum1.a, EnumHelper<BitsReqEnum1>.MinValue);
            Assert.AreEqual((int)BitsReqEnum2.a, EnumHelper<BitsReqEnum2>.MinValue);
            Assert.AreEqual((int)BitsReqEnum3.a, EnumHelper<BitsReqEnum3>.MinValue);
            Assert.AreEqual((int)BitsReqEnum4.a, EnumHelper<BitsReqEnum4>.MinValue);
            Assert.AreEqual((int)BitsReqEnum5.a, EnumHelper<BitsReqEnum5>.MinValue);
            Assert.AreEqual((int)BitsReqEnum6.a, EnumHelper<BitsReqEnum6>.MinValue);
            Assert.AreEqual((int)BitsReqEnum7.a, EnumHelper<BitsReqEnum7>.MinValue);
            Assert.AreEqual((int)BitsReqEnum8.a, EnumHelper<BitsReqEnum8>.MinValue);
            Assert.AreEqual((int)BitsReqEnum9.a, EnumHelper<BitsReqEnum9>.MinValue);
            Assert.AreEqual((int)BitsReqEnum10.a, EnumHelper<BitsReqEnum10>.MinValue);
        }

        [Test]
        public void ParseValidValuesIgnoreCaseLowerTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVByte>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVSByte>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVShort>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUShort>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVInt>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVLong>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVULong>.Parse(v.ToString().ToLower(), true));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUInt>.Parse(v.ToString().ToLower(), true));
            }
        }

        [Test]
        public void ParseValidValuesIgnoreCaseUpperTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVByte>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVSByte>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVShort>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUShort>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVInt>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVLong>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVULong>.Parse(v.ToString().ToUpper(), true));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUInt>.Parse(v.ToString().ToUpper(), true));
            }
        }

        [Test]
        public void ParseValidValuesInvalidCaseTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVByte>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVSByte>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVShort>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVUShort>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVInt>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVLong>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVULong>.Parse(b.ToString().ToLower()));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                var b = v;
                Assert.Throws<ArgumentException>(() => EnumHelper<EVUInt>.Parse(b.ToString().ToLower()));
            }
        }

        [Test]
        public void ParseValidValuesTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVByte>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVSByte>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVShort>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUShort>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVInt>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVLong>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVULong>.Parse(v.ToString()));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUInt>.Parse(v.ToString()));
            }
        }

        [Test]
        public void SupportsCastOperationsTest()
        {
            Assert.IsTrue(EnumHelper<EVByte>.SupportsCastOperations);
            Assert.IsTrue(EnumHelper<EVSByte>.SupportsCastOperations);
            Assert.IsTrue(EnumHelper<EVShort>.SupportsCastOperations);
            Assert.IsTrue(EnumHelper<EVUShort>.SupportsCastOperations);
            Assert.IsTrue(EnumHelper<EVInt>.SupportsCastOperations);
            Assert.IsFalse(EnumHelper<EVUInt>.SupportsCastOperations);
            Assert.IsFalse(EnumHelper<EVLong>.SupportsCastOperations);
            Assert.IsFalse(EnumHelper<EVULong>.SupportsCastOperations);
        }

        [Test]
        public void ToIntTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual((int)v, EnumHelper<EVByte>.ToInt(v));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual((int)v, EnumHelper<EVSByte>.ToInt(v));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual((int)v, EnumHelper<EVShort>.ToInt(v));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual((int)v, EnumHelper<EVUShort>.ToInt(v));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual((int)v, EnumHelper<EVInt>.ToInt(v));
            }
        }

        [Test]
        public void ToNameTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVByte>.Parse(EnumHelper<EVByte>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVSByte>.Parse(EnumHelper<EVSByte>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVShort>.Parse(EnumHelper<EVShort>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUShort>.Parse(EnumHelper<EVUShort>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVInt>.Parse(EnumHelper<EVInt>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVLong>.Parse(EnumHelper<EVLong>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVULong>.Parse(EnumHelper<EVULong>.ToName(v)));
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                Assert.AreEqual(v, EnumHelper<EVUInt>.Parse(EnumHelper<EVUInt>.ToName(v)));
            }
        }

        [Test]
        public void TryParseCaseInsensitiveTests()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            foreach (var name in names)
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (var name in names.Select(x => x.ToUpper()))
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name, true), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (var name in names.Select(x => x.ToLower()))
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

            foreach (var name in names)
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper<TestEnum>.TryParse(name, false, out outValue));
                Assert.AreEqual(EnumHelper<TestEnum>.Parse(name, false), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, false), outValue);
            }

            foreach (var name in names)
            {
                var nameUpper = name.ToUpper();
                if (name.Equals(nameUpper, StringComparison.CurrentCulture))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(nameUpper, false, out outValue));
            }

            foreach (var name in names)
            {
                var nameLower = name.ToLower();
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

            for (var i = 0; i < 1000; i++)
            {
                var s = CreateRandomString();
                if (names.Contains(s, StringComparer.OrdinalIgnoreCase))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(s, true, out outValue), "String: " + s);
                Assert.IsFalse(EnumHelper<TestEnum>.TryParse(s, false, out outValue), "String: " + s);
            }
        }

        [Test]
        public void TryParseValidValuesIgnoreCaseLowerTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                EVByte o;
                Assert.IsTrue(EnumHelper<EVByte>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                EVSByte o;
                Assert.IsTrue(EnumHelper<EVSByte>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                EVShort o;
                Assert.IsTrue(EnumHelper<EVShort>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                EVUShort o;
                Assert.IsTrue(EnumHelper<EVUShort>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                EVInt o;
                Assert.IsTrue(EnumHelper<EVInt>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                EVLong o;
                Assert.IsTrue(EnumHelper<EVLong>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                EVULong o;
                Assert.IsTrue(EnumHelper<EVULong>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                EVUInt o;
                Assert.IsTrue(EnumHelper<EVUInt>.TryParse(v.ToString().ToLower(), true, out o));
                Assert.AreEqual(v, o);
            }
        }

        [Test]
        public void TryParseValidValuesIgnoreCaseUpperTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                EVByte o;
                Assert.IsTrue(EnumHelper<EVByte>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                EVSByte o;
                Assert.IsTrue(EnumHelper<EVSByte>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                EVShort o;
                Assert.IsTrue(EnumHelper<EVShort>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                EVUShort o;
                Assert.IsTrue(EnumHelper<EVUShort>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                EVInt o;
                Assert.IsTrue(EnumHelper<EVInt>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                EVLong o;
                Assert.IsTrue(EnumHelper<EVLong>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                EVULong o;
                Assert.IsTrue(EnumHelper<EVULong>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                EVUInt o;
                Assert.IsTrue(EnumHelper<EVUInt>.TryParse(v.ToString().ToUpper(), true, out o));
                Assert.AreEqual(v, o);
            }
        }

        [Test]
        public void TryParseValidValuesTest()
        {
            foreach (var v in EnumHelper<EVByte>.Values)
            {
                EVByte o;
                Assert.IsTrue(EnumHelper<EVByte>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVSByte>.Values)
            {
                EVSByte o;
                Assert.IsTrue(EnumHelper<EVSByte>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVShort>.Values)
            {
                EVShort o;
                Assert.IsTrue(EnumHelper<EVShort>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUShort>.Values)
            {
                EVUShort o;
                Assert.IsTrue(EnumHelper<EVUShort>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVInt>.Values)
            {
                EVInt o;
                Assert.IsTrue(EnumHelper<EVInt>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVLong>.Values)
            {
                EVLong o;
                Assert.IsTrue(EnumHelper<EVLong>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVULong>.Values)
            {
                EVULong o;
                Assert.IsTrue(EnumHelper<EVULong>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }

            foreach (var v in EnumHelper<EVUInt>.Values)
            {
                EVUInt o;
                Assert.IsTrue(EnumHelper<EVUInt>.TryParse(v.ToString(), out o));
                Assert.AreEqual(v, o);
            }
        }

        #endregion

        enum BitsReqEnum1
        {
            a
        } ;

        enum BitsReqEnum10
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i,
            j
        } ;

        enum BitsReqEnum2
        {
            a,
            b
        } ;

        enum BitsReqEnum3
        {
            a,
            b,
            c
        } ;

        enum BitsReqEnum4
        {
            a,
            b,
            c,
            d
        } ;

        enum BitsReqEnum5
        {
            a,
            b,
            c,
            d,
            e
        } ;

        enum BitsReqEnum6
        {
            a,
            b,
            c,
            d,
            e,
            f
        } ;

        enum BitsReqEnum7
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g
        } ;

        enum BitsReqEnum8
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g,
            h
        } ;

        enum BitsReqEnum9
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i
        } ;

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

        enum EVLong : long
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

        enum EVUInt : uint
        {
            A = 0,
            B = 1,
            C,
            D,
            E,
            F = 100,
            G,
            H
        }

        enum EVULong : ulong
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