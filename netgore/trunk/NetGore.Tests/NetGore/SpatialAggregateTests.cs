using System.Collections.Generic;
using System.Linq;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class SpatialAggregateTests
    {
        static readonly Vector2 SpatialSize = new Vector2(1024, 512);

        static IEnumerable<Entity> CreateEntities(int amount, Vector2 minPos, Vector2 maxPos)
        {
            var ret = new Entity[amount];
            for (var i = 0; i < amount; i++)
            {
                ret[i] = new TestEntity { Position = RandomHelper.NextVector2(minPos, maxPos) };
            }

            return ret;
        }

        static IEnumerable<SpatialAggregate> GetSpatials(out Entity someEntity)
        {
            var aEntities = CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32));
            var bEntities = CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32));
            var cEntities = CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32));
            someEntity = aEntities.First();

            var a = new LinearSpatialCollection();
            a.SetAreaSize(SpatialSize);
            a.Add(aEntities);

            var b = new DynamicGridSpatialCollection();
            b.SetAreaSize(SpatialSize);
            b.Add(bEntities);

            var c = new DynamicGridSpatialCollection();
            c.SetAreaSize(SpatialSize);
            c.Add(cEntities);

            return new SpatialAggregate[] { new SpatialAggregate(new ISpatialCollection[] { a, b, c }) };
        }

        #region Unit tests

        [Test]
        public void ContainsAllTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                var all = spatial.GetMany(new Rectangle(0, 0, SpatialSize.X, SpatialSize.Y));
                foreach (var a in all)
                {
                    Assert.IsTrue(spatial.CollectionContains(a));
                }
            }
        }

        [Test]
        public void ContainsTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                Assert.IsTrue(spatial.CollectionContains(someEntity));
            }
        }

        [Test]
        public void DistinctTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                var all = spatial.GetMany(new Rectangle(0, 0, SpatialSize.X, SpatialSize.Y));
                var distinct = all.Distinct();
                Assert.AreEqual(distinct.Count(), all.Count());
            }
        }

        #endregion

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
        }
    }
}