using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class CollisionBoxTests
    {
        [Test]
        public void ContainsTest()
        {
            CollisionBox cb = new CollisionBox(new Vector2(10, 10), 16, 16);

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Rectangle r = new Rectangle(x, y, 2, 3);

                    bool hitTestPassed = cb.Contains(r);
                    bool shouldPass = cb.Contains(r);

                    Assert.AreEqual(shouldPass, hitTestPassed, "Position: {0},{1}", x, y);
                }
            }
        }

        [Test]
        public void HitTestTest()
        {
            CollisionBox cb = new CollisionBox(new Vector2(10, 10), 16, 16);

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    bool hitTestPassed = cb.HitTest(new Vector2(x, y));
                    bool shouldPass = true;
                    if (x < cb.Min.X)
                        shouldPass = false;
                    if (x > cb.Max.X)
                        shouldPass = false;
                    if (y < cb.Min.Y)
                        shouldPass = false;
                    if (y > cb.Max.Y)
                        shouldPass = false;

                    Assert.AreEqual(shouldPass, hitTestPassed, "Position: {0},{1}", x, y);
                }
            }
        }

        [Test]
        public void IntersectsTest()
        {
            CollisionBox cb = new CollisionBox(new Vector2(10, 10), 16, 16);

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Rectangle r = new Rectangle(x, y, 2, 3);

                    bool hitTestPassed = cb.Intersect(r);
                    bool shouldPass = true;
                    if (r.Right < cb.Min.X)
                        shouldPass = false;
                    if (r.Left > cb.Max.X)
                        shouldPass = false;
                    if (r.Bottom < cb.Min.Y)
                        shouldPass = false;
                    if (r.Top > cb.Max.Y)
                        shouldPass = false;

                    Assert.AreEqual(shouldPass, hitTestPassed, "Position: {0},{1}", x, y);
                }
            }
        }

        [Test]
        public void PropertyLogicTest()
        {
            CollisionBox cb = new CollisionBox(new Vector2(10, 10), 16, 16);
            Assert.AreEqual(cb.Size, cb.Max - cb.Min);
            Assert.AreEqual(cb.Size.X, cb.Width);
            Assert.AreEqual(cb.Size.Y, cb.Height);
        }

        [Test]
        public void ToRectangleTest()
        {
            CollisionBox cb = new CollisionBox(new Vector2(10, 10), 16, 16);
            Rectangle r = cb.ToRectangle();
            Assert.AreEqual(r.X, 10);
            Assert.AreEqual(r.Y, 10);
            Assert.AreEqual(r.Width, 16);
            Assert.AreEqual(r.Width, 16);
        }
    }
}