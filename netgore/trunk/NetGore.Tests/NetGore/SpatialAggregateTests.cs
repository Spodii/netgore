using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class SpatialAggregateTests
    {
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

        static readonly Vector2 SpatialSize = new Vector2(1024, 512);

        static IEnumerable<Entity> CreateEntities(int amount, Vector2 minPos, Vector2 maxPos)
        {
            Entity[] ret = new Entity[amount];
            for (int i = 0; i < amount; i++)
                ret[i] = new TestEntity { Position = RandomHelperXna.NextVector2(minPos, maxPos) };

            return ret;
        }

        static IEnumerable<SpatialAggregate> GetSpatials(out Entity someEntity)
        {
            var aEntities = CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32));
            someEntity = aEntities.First();

            var a = new DynamicEntitySpatial();
            a.SetMapSize(SpatialSize);
            a.Add(aEntities);

            var b = new DynamicEntitySpatial();
            b.SetMapSize(SpatialSize);
            b.Add(CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32)));

            return new SpatialAggregate[] {
            new SpatialAggregate(new IEntitySpatial[] { a, b }, false),
            new SpatialAggregate(new IEntitySpatial[] { a, a }, true),
            new SpatialAggregate(new IEntitySpatial[] { a, a, b }, true),
            new SpatialAggregate(new IEntitySpatial[] { a }, false)};

        }

        [Test]
        public void ContainsTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                Assert.IsTrue(spatial.Contains(someEntity));
            }
        }

        [Test]
        public void DistinctTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                var all = spatial.GetEntities(new Rectangle(0, 0, (int)SpatialSize.X, (int)SpatialSize.Y));
                var distinct = all.Distinct();
                Assert.AreEqual(distinct.Count(), all.Count());
            }
        }

        [Test]
        public void ContainsAllTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                var all = spatial.GetEntities(new Rectangle(0, 0, (int)SpatialSize.X, (int)SpatialSize.Y));
                foreach (var a in all)
                    Assert.IsTrue(spatial.Contains(a));
            }
        }
    }
}
