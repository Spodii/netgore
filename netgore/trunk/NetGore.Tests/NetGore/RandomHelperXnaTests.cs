using System.Linq;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class RandomHelperXnaTests
    {
        #region Unit tests

        [Test]
        public void RandomVector2Test()
        {
            for (var i = 0; i < 50; i++)
            {
                var value = RandomHelperXna.NextVector2();
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
                var value = RandomHelperXna.NextVector2(100);
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
                var value = RandomHelperXna.NextVector2(50, 100);
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
                var value = RandomHelperXna.NextVector2(50, 150, 100, 200);
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
                var value = RandomHelperXna.NextVector2(new Vector2(50, 100), new Vector2(150, 200));
                Assert.GreaterOrEqual(value.X, 50.0f);
                Assert.GreaterOrEqual(value.Y, 100.0f);
                Assert.LessOrEqual(value.X, 150.0f);
                Assert.LessOrEqual(value.Y, 200.0f);
            }
        }

        #endregion
    }
}