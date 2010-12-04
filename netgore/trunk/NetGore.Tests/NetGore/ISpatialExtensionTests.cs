using System;
using System.Linq;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class ISpatialExtensionTests
    {
        #region Unit tests

        [Test]
        public void ContainsHardlyContainedTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(33, 33, 30, 30);
            Assert.IsTrue(s.Contains(r));
            Assert.IsFalse(r.Contains(s));
        }

        [Test]
        public void ContainsOverlapTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(32, 32, 32, 32);
            Assert.IsTrue(s.Contains(r));
            Assert.IsTrue(r.Contains(s));
        }

        [Test]
        public void ContainsTrueTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(40, 40, 3, 3);
            Assert.IsTrue(s.Contains(r));
            Assert.IsFalse(r.Contains(s));
        }

        [Test]
        public void GetDistanceAboveTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(32, -32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 32);
        }

        [Test]
        public void GetDistanceBelowTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(32, 96, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 32);
        }

        [Test]
        public void GetDistanceDownLeftTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(-32, 96, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 64);
        }

        [Test]
        public void GetDistanceDownRightTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(96, 96, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 64);
        }

        [Test]
        public void GetDistanceLeftTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(-32, 32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 32);
        }

        [Test]
        public void GetDistanceOverlapTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(32, 32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 0);
        }

        [Test]
        public void GetDistanceRightTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(96, 32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 32);
        }

        [Test]
        public void GetDistanceUpLeftTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(-32, -32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 64);
        }

        [Test]
        public void GetDistanceUpRightTest()
        {
            var s = new TestSpatial(32, 32, 32, 32);
            var r = new TestSpatial(96, -32, 32, 32);
            var d = r.GetDistance(s);
            Assert.AreEqual(Math.Abs(d), Math.Abs(s.GetDistance(r)));
            Assert.AreEqual(d, 64);
        }

        [Test]
        public void GetStandingAreaRectTest()
        {
            var s = new TestSpatial(0, 0, 32, 32);
            var r = s.GetStandingAreaRect();
            Assert.IsTrue(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseAboveTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(32, 0, 32, 31);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseBelowTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(32, 65, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseBottomBorderTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(32, 64, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseLeftBorderTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(0, 32, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseLeftTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(0, 32, 31, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseRightBorderTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(64, 32, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseRightTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(65, 32, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsFalseTopBorderTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(32, 0, 32, 32);
            Assert.IsFalse(s.Intersects(r));
        }

        [Test]
        public void IntersectsTrueContained2Test()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(34, 34, 6, 6);
            Assert.IsTrue(r.Intersects(s));
        }

        [Test]
        public void IntersectsTrueContainedTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(34, 34, 6, 6);
            Assert.IsTrue(s.Intersects(r));
        }

        [Test]
        public void IntersectsTrueOverlapTest()
        {
            var r = new TestSpatial(32, 32, 32, 32);
            var s = new TestSpatial(32, 32, 32, 32);
            Assert.IsTrue(s.Intersects(r));
        }

        #endregion

        /// <summary>
        /// An implementation of <see cref="ISpatial"/> to perform tests on.
        /// </summary>
        class TestSpatial : ISpatial
        {
            readonly Vector2 _position;
            readonly Vector2 _size;

            /// <summary>
            /// Initializes a new instance of the <see cref="TestSpatial"/> class.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            public TestSpatial(int x, int y, int width, int height) : this(new Vector2(x, y), new Vector2(width, height))
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TestSpatial"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="size">The size.</param>
            TestSpatial(Vector2 position, Vector2 size)
            {
                _position = position;
                _size = size;
            }

            #region ISpatial Members

            /// <summary>
            /// Notifies listeners when this <see cref="ISpatial"/> has moved.
            /// </summary>
            event TypedEventHandler<ISpatial, EventArgs<Vector2>> ISpatial.Moved
            {
                add { }
                remove { }
            }

            /// <summary>
            /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
            /// </summary>
            event TypedEventHandler<ISpatial, EventArgs<Vector2>> ISpatial.Resized
            {
                add { }
                remove { }
            }

            /// <summary>
            /// Gets the center position of the <see cref="ISpatial"/>.
            /// </summary>
            public Vector2 Center
            {
                get { return Position + (Size / 2f); }
            }

            /// <summary>
            /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
            /// </summary>
            public Vector2 Max
            {
                get { return Position + Size; }
            }

            /// <summary>
            /// Gets the world coordinates of the top-left corner of this <see cref="ISpatial"/>.
            /// </summary>
            public Vector2 Position
            {
                get { return _position; }
            }

            /// <summary>
            /// Gets the size of this <see cref="ISpatial"/>.
            /// </summary>
            public Vector2 Size
            {
                get { return _size; }
            }

            /// <summary>
            /// Gets if this <see cref="ISpatial"/> can ever be moved with <see cref="ISpatial.TryMove"/>.
            /// </summary>
            bool ISpatial.SupportsMove
            {
                get { return false; }
            }

            /// <summary>
            /// Gets if this <see cref="ISpatial"/> can ever be resized with <see cref="ISpatial.TryResize"/>.
            /// </summary>
            bool ISpatial.SupportsResize
            {
                get { return false; }
            }

            /// <summary>
            /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
            /// </summary>
            /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
            /// occupies.</returns>
            public Rectangle ToRectangle()
            {
                return SpatialHelper.ToRectangle(this);
            }

            /// <summary>
            /// Tries to move the <see cref="ISpatial"/>.
            /// </summary>
            /// <param name="newPos">The new position.</param>
            /// <returns>True if the <see cref="ISpatial"/> was moved to the <paramref name="newPos"/>; otherwise false.</returns>
            bool ISpatial.TryMove(Vector2 newPos)
            {
                return false;
            }

            /// <summary>
            /// Tries to resize the <see cref="ISpatial"/>.
            /// </summary>
            /// <param name="newSize">The new size.</param>
            /// <returns>True if the <see cref="ISpatial"/> was resized to the <paramref name="newSize"/>; otherwise false.</returns>
            bool ISpatial.TryResize(Vector2 newSize)
            {
                return false;
            }

            #endregion
        }
    }
}