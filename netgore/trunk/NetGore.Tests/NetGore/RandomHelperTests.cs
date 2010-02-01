using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class RandomHelperTests
    {
        #region Unit tests

        [Test]
        public void Choose1Test()
        {
            var selected = RandomHelper.Choose(1);
            Assert.AreEqual(1, selected);
        }

        [Test]
        public void Choose2Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2);
                Assert.IsTrue(selected == 1 || selected == 2);
            }
        }

        [Test]
        public void Choose3Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3);
                Assert.IsTrue(selected >= 1 && selected <= 3);
            }
        }

        [Test]
        public void Choose4Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4);
                Assert.IsTrue(selected >= 1 && selected <= 4);
            }
        }

        [Test]
        public void Choose5Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4, 5);
                Assert.IsTrue(selected >= 1 && selected <= 5);
            }
        }

        [Test]
        public void Choose6Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4, 5, 6);
                Assert.IsTrue(selected >= 1 && selected <= 6);
            }
        }

        [Test]
        public void NextBoolTest()
        {
            const int it = 1000;

            bool trueFound = false;
            for (int i = 0; i < it; i++)
            {
                if (RandomHelper.NextBool())
                {
                    trueFound = true;
                    break;
                }
            }

            bool falseFound = false;
            for (int i = 0; i < it; i++)
            {
                if (!RandomHelper.NextBool())
                {
                    falseFound = true;
                    break;
                }
            }

            Assert.IsTrue(trueFound, "Failed to find a random 'true' value after {0} iterations.", it);
            Assert.IsTrue(falseFound, "Failed to find a random 'false' value after {0} iterations.", it);
        }

        [Test]
        public void NextFloatTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat();
                Assert.GreaterOrEqual(value, 0.0f);
                Assert.LessOrEqual(value, 1.0f);
            }
        }

        [Test]
        public void NextFloatWithMaxTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat(100);
                Assert.GreaterOrEqual(value, 0.0f);
                Assert.LessOrEqual(value, 100.0f);
            }
        }

        [Test]
        public void NextFloatWithRangeTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat(50, 300);
                Assert.GreaterOrEqual(value, 50.0f);
                Assert.LessOrEqual(value, 300.0f);
            }
        }

        [Test]
        public void NextIntTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt();
                Assert.GreaterOrEqual(value, 0);
            }
        }

        [Test]
        public void NextIntWithMaxTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt(1000);
                Assert.GreaterOrEqual(value, 0);
                Assert.LessOrEqual(value, 1000);
            }
        }

        [Test]
        public void NextIntWithRangeTest()
        {
            for (int i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt(50, 500);
                Assert.GreaterOrEqual(value, 50);
                Assert.LessOrEqual(value, 500);
            }
        }

        #endregion
    }
}