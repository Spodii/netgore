using System.Linq;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class AlignmentHelperTests
    {
        #region Unit tests

        [Test]
        public void FindOffsetBottomLeftTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(0, -2), AlignmentHelper.FindOffset(Alignment.BottomLeft, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetBottomRightTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(-2, -2), AlignmentHelper.FindOffset(Alignment.BottomRight, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetBottomTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(-1, -2), AlignmentHelper.FindOffset(Alignment.Bottom, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetLeftTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(0, -1), AlignmentHelper.FindOffset(Alignment.Left, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetRightTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(-2, -1), AlignmentHelper.FindOffset(Alignment.Right, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetTopLeftTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(0, 0), AlignmentHelper.FindOffset(Alignment.TopLeft, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetTopRightTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(-2, 0), AlignmentHelper.FindOffset(Alignment.TopRight, srcSize, dstSize));
        }

        [Test]
        public void FindOffsetTopTest()
        {
            var srcSize = new Vector2(4, 4);
            var dstSize = new Vector2(2, 2);
            Assert.AreEqual(new Vector2(-1, 0), AlignmentHelper.FindOffset(Alignment.Top, srcSize, dstSize));
        }

        #endregion
    }
}