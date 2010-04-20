using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class NaturalStringComparerTests
    {
        /// <summary>
        /// Runs a sorting test.
        /// </summary>
        /// <param name="unsorted">The unsorted list.</param>
        /// <param name="expected">The expected sorting.</param>
        static void RunTest(List<string> unsorted, IList<string> expected)
        {
            unsorted.Sort(NaturalStringComparer.Instance);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], unsorted[i]);
            }
        }

        #region Unit tests

        [Test]
        public void AlphaNumericSort1Test()
        {
            var unsorted = new List<string> { "101 asdf", "51 fd", "1 a" };
            var expected = new List<string> { "1 a", "51 fd", "101 asdf" };
            RunTest(unsorted, expected);
        }

        [Test]
        public void AlphaNumericSort2Test()
        {
            var unsorted = new List<string> { "asd 41", "asd 5", "as 1" };
            var expected = new List<string> { "as 1", "asd 5", "asd 41" };
            RunTest(unsorted, expected);
        }

        [Test]
        public void AlphaOnlySortTest()
        {
            var unsorted = new List<string> { "a", "f", "e", "ww", "er" };
            var expected = new List<string> { "a", "e", "er", "f", "ww" };
            RunTest(unsorted, expected);
        }

        [Test]
        public void LargeNumbersTest()
        {
            var nums = new List<string>
            {
                "9",
                "99",
                "999",
                "9999",
                "99999",
                "999999",
                "9999999",
                "99999999",
                "999999999",
                "9999999999",
                "99999999999",
                "999999999999",
                "9999999999999",
                "99999999999999",
                "999999999999999",
                "9999999999999999",
                "99999999999999999",
                "999999999999999999",
                "9999999999999999999",
                "99999999999999999999",
                "999999999999999999999",
                "9999999999999999999999",
                "99999999999999999999999",
                "999999999999999999999999",
                "9999999999999999999999999",
                "99999999999999999999999999",
                "999999999999999999999999999",
                "9999999999999999999999999999",
                "99999999999999999999999999999",
                "999999999999999999999999999999",
                "9999999999999999999999999999999",
                "99999999999999999999999999999999",
                "999999999999999999999999999999999",
                "9999999999999999999999999999999999",
                "99999999999999999999999999999999999",
                "999999999999999999999999999999999999",
                "9999999999999999999999999999999999999",
                "99999999999999999999999999999999999999",
                "999999999999999999999999999999999999999",
                "9999999999999999999999999999999999999999",
            };

            nums.Sort(NaturalStringComparer.Instance);
        }

        [Test]
        public void NumericOnlySortTest()
        {
            var unsorted = new List<string> { "5", "1", "10", "51", "502", "123" };
            var expected = new List<string> { "1", "5", "10", "51", "123", "502" };
            RunTest(unsorted, expected);
        }

        #endregion
    }
}