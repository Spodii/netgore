using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void HitTestTest()
        {
            TestEntity e = new TestEntity(Vector2.Zero, new Vector2(10, 10));
            Assert.IsTrue(e.HitTest(new Vector2(0, 0)));
            Assert.IsTrue(e.HitTest(new Vector2(10, 0)));
            Assert.IsTrue(e.HitTest(new Vector2(0, 10)));
            Assert.IsTrue(e.HitTest(new Vector2(10, 10)));
            Assert.IsTrue(e.HitTest(new Vector2(5, 5)));

            Assert.IsFalse(e.HitTest(new Vector2(-1, 0)));
            Assert.IsFalse(e.HitTest(new Vector2(0, -1)));
            Assert.IsFalse(e.HitTest(new Vector2(-1, -1)));
            Assert.IsFalse(e.HitTest(new Vector2(11, 0)));
            Assert.IsFalse(e.HitTest(new Vector2(0, 11)));
            Assert.IsFalse(e.HitTest(new Vector2(11, 11)));
        }

        [Test]
        public void IntersectTest()
        {
            TestEntity a = new TestEntity(Vector2.Zero, new Vector2(10, 10));
            TestEntity b = new TestEntity(Vector2.Zero, new Vector2(10, 10));

            Assert.IsTrue(a.Intersect(b));

            a.Teleport(new Vector2(9, 9));
            Assert.IsTrue(a.Intersect(b));

            a.Teleport(new Vector2(-9, -9));
            Assert.IsTrue(a.Intersect(b));

            a.Teleport(new Vector2(-9, 0));
            Assert.IsTrue(a.Intersect(b));

            a.Teleport(new Vector2(0, -9));
            Assert.IsTrue(a.Intersect(b));

            a.Teleport(new Vector2(-11, -11));
            Assert.IsFalse(a.Intersect(b));

            a.Teleport(new Vector2(-11, 0));
            Assert.IsFalse(a.Intersect(b));

            a.Teleport(new Vector2(0, -11));
            Assert.IsFalse(a.Intersect(b));

            a.Teleport(new Vector2(11, 11));
            Assert.IsFalse(a.Intersect(b));
        }

        [Test]
        public void MoveTest()
        {
            TestEntity e = new TestEntity(new Vector2(10, 10), new Vector2(10, 10));
            Assert.AreEqual(10, e.Position.X);
            Assert.AreEqual(10, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);

            e.Move(new Vector2(5, 10));
            Assert.AreEqual(15, e.Position.X);
            Assert.AreEqual(20, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);
        }

        [Test]
        public void TeleportTest()
        {
            TestEntity e = new TestEntity(new Vector2(10, 10), new Vector2(10, 10));
            Assert.AreEqual(10, e.Position.X);
            Assert.AreEqual(10, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);

            e.Teleport(new Vector2(5, 10));
            Assert.AreEqual(5, e.Position.X);
            Assert.AreEqual(10, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);
        }

        class TestEntity : Entity
        {
            /// <summary>
            /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
            /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
            /// </summary>
            public override bool CollidesAgainstWalls
            {
                get { return true; }
            }

            public TestEntity(Vector2 pos, Vector2 size) : base(pos, size)
            {
            }
        }
    }
}