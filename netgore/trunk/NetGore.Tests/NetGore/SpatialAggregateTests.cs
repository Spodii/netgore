using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class SpatialAggregateTests
    {
        static readonly Vector2 SpatialSize = new Vector2(1024, 512);

        [Test]
        public void ContainsAllTest()
        {
            Entity someEntity;
            foreach (var spatial in GetSpatials(out someEntity))
            {
                var all = spatial.GetEntities(new Rectangle(0, 0, (int)SpatialSize.X, (int)SpatialSize.Y));
                foreach (var a in all)
                {
                    Assert.IsTrue(spatial.Contains(a));
                }
            }
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

        static IEnumerable<Entity> CreateEntities(int amount, Vector2 minPos, Vector2 maxPos)
        {
            Entity[] ret = new Entity[amount];
            for (int i = 0; i < amount; i++)
            {
                ret[i] = new TestEntity { Position = RandomHelperXna.NextVector2(minPos, maxPos) };
            }

            return ret;
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

        static IEnumerable<SpatialAggregate> GetSpatials(out Entity someEntity)
        {
            var aEntities = CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32));
            someEntity = aEntities.First();

            var a = new LinearSpatialCollection();
            a.SetAreaSize(SpatialSize);
            a.Add(aEntities);


            // TODO: !! Add tests back again when I re-add the good spatials
            var b = new LinearSpatialCollection();
            b.SetAreaSize(SpatialSize);
            b.Add(aEntities);

            /*
            var b = new DynamicEntitySpatial();
            b.SetMapSize(SpatialSize);
            b.Add(CreateEntities(64, new Vector2(32), SpatialSize - new Vector2(32)));
            */

            return new SpatialAggregate[]
            {
                new SpatialAggregate(new ISpatialCollection[] { a, b })
            };
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
        }
    }
}