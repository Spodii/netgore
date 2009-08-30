using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests
{
    [TestFixture]
    public class EnumHelperTests
    {
        // ReSharper disable UnusedMember.Local
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
        // ReSharper restore UnusedMember.Local

        static readonly Random r = new Random();

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
        public void TryParseCaseInsensitiveTests()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            foreach (string name in names)
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper.Parse<TestEnum>(name), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (string name in names.Select(x => x.ToUpper()))
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper.Parse<TestEnum>(name, true), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, true), outValue);
            }

            foreach (string name in names.Select(x => x.ToLower()))
            {
                TestEnum outValue;
                Assert.IsTrue(EnumHelper.TryParse(name, true, out outValue));
                Assert.AreEqual(EnumHelper.Parse<TestEnum>(name, true), outValue);
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
                Assert.IsTrue(EnumHelper.TryParse(name, false, out outValue));
                Assert.AreEqual(EnumHelper.Parse<TestEnum>(name, false), outValue);
                Assert.AreEqual((TestEnum)Enum.Parse(typeof(TestEnum), name, false), outValue);
            }

            foreach (string name in names)
            {
                string nameUpper = name.ToUpper();
                if (name.Equals(nameUpper, StringComparison.CurrentCulture))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper.TryParse(nameUpper, false, out outValue));
            }

            foreach (string name in names)
            {
                string nameLower = name.ToLower();
                if (name.Equals(nameLower, StringComparison.CurrentCulture))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper.TryParse(nameLower, false, out outValue));
            }
        }

        [Test]
        public void TryParseRandomStrings()
        {
            var names = Enum.GetNames(typeof(TestEnum));

            for (int i = 0; i < 1000; i++)
            {
                string s = CreateRandomString();
                if (names.Contains(s))
                    continue;

                TestEnum outValue;
                Assert.IsFalse(EnumHelper.TryParse(s, true, out outValue));
                Assert.IsFalse(EnumHelper.TryParse(s, false, out outValue));
            }
        }
    }
}