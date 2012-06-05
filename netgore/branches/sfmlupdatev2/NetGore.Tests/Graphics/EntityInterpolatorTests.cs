using System;
using System.Linq;
using NetGore.Graphics;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.Graphics
{
    [TestFixture]
    public class EntityInterpolatorTests
    {
        #region Unit tests

        [Test]
        public void InterpolateNoVelocityRecordTest()
        {
            var e = new TestEntity();
            var i = new EntityInterpolator();

            e.Position = new Vector2(0);
            i.Update(e, 1);
            Assert.AreEqual(Vector2.Zero, i.DrawPosition);

            e.Move(new Vector2(16));
            i.Update(e, 1);
            Assert.AreEqual(new Vector2(16), i.DrawPosition);

            e.Move(new Vector2(100));
            i.Update(e, 1);
            Assert.AreEqual(new Vector2(116), i.DrawPosition);
        }

        [Test]
        public void InterpolateStoppedTest()
        {
            var e = new TestEntity();
            var i = new EntityInterpolator();

            e.Position = new Vector2(0);
            i.Update(e, 1);
            Assert.AreEqual(Vector2.Zero, i.DrawPosition);

            e.Move(new Vector2(16));
            e.SetVelocity(new Vector2(1));
            i.Update(e, 1);
            Assert.AreNotEqual(new Vector2(0), i.DrawPosition);

            e.Move(new Vector2(100));
            e.SetVelocity(new Vector2(0));
            i.Update(e, 1);
            Assert.AreNotEqual(new Vector2(16), i.DrawPosition);

            var last = i.DrawPosition;
            e.Move(new Vector2(50));
            i.Update(e, 10);
            Assert.AreNotEqual(last, i.DrawPosition);
        }

        #endregion

        /// <summary>
        /// An implementation of <see cref="Entity"/> for testing the <see cref="EntityInterpolator"/>.
        /// </summary>
        class TestEntity : Entity
        {
            /// <summary>
            /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
            /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
            /// </summary>
            /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
            public override bool CollidesAgainstWalls
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}