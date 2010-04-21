using System.Linq;
using NUnit.Framework;
using SFML.Graphics;

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
            for (var i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2);
                Assert.IsTrue(selected == 1 || selected == 2);
            }
        }

        [Test]
        public void Choose3Test()
        {
            for (var i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3);
                Assert.IsTrue(selected >= 1 && selected <= 3);
            }
        }

        [Test]
        public void Choose4Test()
        {
            for (var i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4);
                Assert.IsTrue(selected >= 1 && selected <= 4);
            }
        }

        [Test]
        public void Choose5Test()
        {
            for (var i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4, 5);
                Assert.IsTrue(selected >= 1 && selected <= 5);
            }
        }

        [Test]
        public void Choose6Test()
        {
            for (var i = 0; i < 10; i++)
            {
                var selected = RandomHelper.Choose(1, 2, 3, 4, 5, 6);
                Assert.IsTrue(selected >= 1 && selected <= 6);
            }
        }

        [Test]
        public void NextBoolTest()
        {
            const int it = 1000;

            var trueFound = false;
            for (var i = 0; i < it; i++)
            {
                if (RandomHelper.NextBool())
                {
                    trueFound = true;
                    break;
                }
            }

            var falseFound = false;
            for (var i = 0; i < it; i++)
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
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat();
                Assert.GreaterOrEqual(value, 0.0f);
                Assert.LessOrEqual(value, 1.0f);
            }
        }

        [Test]
        public void NextFloatWithMaxTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat(100);
                Assert.GreaterOrEqual(value, 0.0f);
                Assert.LessOrEqual(value, 100.0f);
            }
        }

        [Test]
        public void NextFloatWithRangeTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextFloat(50, 300);
                Assert.GreaterOrEqual(value, 50.0f);
                Assert.LessOrEqual(value, 300.0f);
            }
        }

        [Test]
        public void NextIntTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt();
                Assert.GreaterOrEqual(value, 0);
            }
        }

        [Test]
        public void NextIntWithMaxTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt(1000);
                Assert.GreaterOrEqual(value, 0);
                Assert.LessOrEqual(value, 1000);
            }
        }

        [Test]
        public void NextIntWithRangeTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextInt(50, 500);
                Assert.GreaterOrEqual(value, 50);
                Assert.LessOrEqual(value, 500);
            }
        }

        [Test]
        public void RandomVector2Test()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextVector2();
                Assert.GreaterOrEqual(value.X, 0.0f);
                Assert.GreaterOrEqual(value.Y, 0.0f);
                Assert.LessOrEqual(value.X, 1.0f);
                Assert.LessOrEqual(value.Y, 1.0f);
            }
        }

        [Test]
        public void RandomVector2WithMaxTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextVector2(100);
                Assert.GreaterOrEqual(value.X, 0.0f);
                Assert.GreaterOrEqual(value.Y, 0.0f);
                Assert.LessOrEqual(value.X, 100.0f);
                Assert.LessOrEqual(value.Y, 100.0f);
            }
        }

        [Test]
        public void RandomVector2WithMinMaxTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextVector2(50, 100);
                Assert.GreaterOrEqual(value.X, 50.0f);
                Assert.GreaterOrEqual(value.Y, 50.0f);
                Assert.LessOrEqual(value.X, 100.0f);
                Assert.LessOrEqual(value.Y, 100.0f);
            }
        }

        [Test]
        public void RandomVector2WithRange2Test()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextVector2(50, 150, 100, 200);
                Assert.GreaterOrEqual(value.X, 50.0f);
                Assert.GreaterOrEqual(value.Y, 100.0f);
                Assert.LessOrEqual(value.X, 150.0f);
                Assert.LessOrEqual(value.Y, 200.0f);
            }
        }

        [Test]
        public void RandomVector2WithRangeTest()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelper.NextVector2(new Vector2(50, 100), new Vector2(150, 200));
                Assert.GreaterOrEqual(value.X, 50.0f);
                Assert.GreaterOrEqual(value.Y, 100.0f);
                Assert.LessOrEqual(value.X, 150.0f);
                Assert.LessOrEqual(value.Y, 200.0f);
            }
        }

        #endregion
    }
}