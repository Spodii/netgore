using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class DirectionHelperTests
    {
        [Test]
        public void DirectionNorthTest()
        {
            var v = new Vector2(0, -1);
            Assert.AreEqual(Direction.North, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionNorthEastTest()
        {
            var v = new Vector2(1, -1);
            Assert.AreEqual(Direction.NorthEast, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionNorthWestTest()
        {
            var v = new Vector2(-1, -1);
            Assert.AreEqual(Direction.NorthWest, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionSouthTest()
        {
            var v = new Vector2(0, 1);
            Assert.AreEqual(Direction.South, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionSouthEastTest()
        {
            var v = new Vector2(1, 1);
            Assert.AreEqual(Direction.SouthEast, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionSouthWestTest()
        {
            var v = new Vector2(-1, 1);
            Assert.AreEqual(Direction.SouthWest, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionWestTest()
        {
            var v = new Vector2(-1, 0);
            Assert.AreEqual(Direction.West, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionEastTest()
        {
            var v = new Vector2(1, 0);
            Assert.AreEqual(Direction.East, DirectionHelper.FromVector(v));
        }

        [Test]
        public void DirectionNullTest()
        {
            var v = new Vector2(0, 0);
            Assert.AreEqual(null, DirectionHelper.FromVector(v));
        }
    }
}
