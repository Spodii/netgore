using System.Linq;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class EntityTests
    {
        #region Unit tests

        [Test]
        public void HitTestTest()
        {
            var e = new TestEntity(Vector2.Zero, new Vector2(10, 10));
            Assert.IsTrue(e.Contains(new Vector2(0, 0)));
            Assert.IsTrue(e.Contains(new Vector2(10, 0)));
            Assert.IsTrue(e.Contains(new Vector2(0, 10)));
            Assert.IsTrue(e.Contains(new Vector2(10, 10)));
            Assert.IsTrue(e.Contains(new Vector2(5, 5)));

            Assert.IsFalse(e.Contains(new Vector2(-1, 0)));
            Assert.IsFalse(e.Contains(new Vector2(0, -1)));
            Assert.IsFalse(e.Contains(new Vector2(-1, -1)));
            Assert.IsFalse(e.Contains(new Vector2(11, 0)));
            Assert.IsFalse(e.Contains(new Vector2(0, 11)));
            Assert.IsFalse(e.Contains(new Vector2(11, 11)));
        }

        [Test]
        public void IntersectTest()
        {
            var a = new TestEntity(Vector2.Zero, new Vector2(10, 10));
            var b = new TestEntity(Vector2.Zero, new Vector2(10, 10));

            Assert.IsTrue(a.Intersects(b));

            a.Position = new Vector2(9, 9);
            Assert.IsTrue(a.Intersects(b));

            a.Position = new Vector2(-9, -9);
            Assert.IsTrue(a.Intersects(b));

            a.Position = new Vector2(-9, 0);
            Assert.IsTrue(a.Intersects(b));

            a.Position = new Vector2(0, -9);
            Assert.IsTrue(a.Intersects(b));

            a.Position = new Vector2(-11, -11);
            Assert.IsFalse(a.Intersects(b));

            a.Position = new Vector2(-11, 0);
            Assert.IsFalse(a.Intersects(b));

            a.Position = new Vector2(0, -11);
            Assert.IsFalse(a.Intersects(b));

            a.Position = new Vector2(11, 11);
            Assert.IsFalse(a.Intersects(b));
        }

        [Test]
        public void MoveTest()
        {
            var e = new TestEntity(new Vector2(10, 10), new Vector2(10, 10));
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
            var e = new TestEntity(new Vector2(10, 10), new Vector2(10, 10));
            Assert.AreEqual(10, e.Position.X);
            Assert.AreEqual(10, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);

            e.Position = new Vector2(5, 10);
            Assert.AreEqual(5, e.Position.X);
            Assert.AreEqual(10, e.Position.Y);
            Assert.AreEqual(10, e.Size.X);
            Assert.AreEqual(10, e.Size.Y);
        }

        #endregion

        class TestEntity : Entity
        {
            public TestEntity(Vector2 pos, Vector2 size) : base(pos, size)
            {
            }

            /// <summary>
            /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
            /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
            /// </summary>
            public override bool CollidesAgainstWalls
            {
                get { return true; }
            }
        }
    }
}