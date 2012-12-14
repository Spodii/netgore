using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.World;
using NUnit.Framework;
using SFML.Graphics;

// ReSharper disable AccessToModifiedClosure

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class SpatialTests
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

        static IEnumerable<ISpatialCollection> GetSpatials()
        {
            var a = new LinearSpatialCollection();
            a.SetAreaSize(SpatialSize);

            var b = new DynamicGridSpatialCollection();
            b.SetAreaSize(SpatialSize);

            var c = new StaticGridSpatialCollection();
            c.SetAreaSize(SpatialSize);

            return new ISpatialCollection[] { a, b };
        }

        #region Unit tests

        [Test]
        public void AddTest()
        {
            foreach (var spatial in GetSpatials())
            {
                var entity = new TestEntity();
                spatial.Add(entity);
                Assert.IsTrue(spatial.CollectionContains(entity), "Current spatial: " + spatial);
            }
        }

        [Test]
        public void GetEntitiesTest()
        {
            const int count = 25;
            var min = new Vector2(32, 64);
            var max = new Vector2(256, 128);
            var diff = max - min;

            foreach (var spatial in GetSpatials())
            {
                var entities = CreateEntities(count, min, max);
                spatial.Add(entities);

                foreach (var entity in entities)
                {
                    Assert.IsTrue(spatial.CollectionContains(entity), "Current spatial: " + spatial);
                }

                var found = spatial.GetMany(new Rectangle(min.X, min.Y, diff.X, diff.Y));

                Assert.AreEqual(count, found.Count());
            }
        }

        [Test]
        public void MoveTest()
        {
            foreach (var spatial in GetSpatials())
            {
                var entity = new TestEntity();
                spatial.Add(entity);
                Assert.IsTrue(spatial.CollectionContains(entity), "Current spatial: " + spatial);

                entity.Position = new Vector2(128, 128);
                Assert.IsTrue(spatial.Contains(new Vector2(128, 128)), "Current spatial: " + spatial);
                Assert.IsFalse(spatial.Contains(new Vector2(256, 128)), "Current spatial: " + spatial);
                Assert.IsFalse(spatial.Contains(new Vector2(128, 256)), "Current spatial: " + spatial);
            }
        }

        [Test(Description = "Puts an emphasis on ensuring Move() works.")]
        public void MovementTest01()
        {
            var moveSize = (int)(Math.Min(SpatialSize.X, SpatialSize.Y) / 4);
            var halfMoveSize = new Vector2(moveSize) / 2f;

            var r = new Random(654987645);
            foreach (var spatial in GetSpatials())
            {
                var entities = new List<TestEntity>();
                for (var i = 0; i < 20; i++)
                {
                    var e = new TestEntity
                    { Position = new Vector2(r.Next((int)SpatialSize.X), r.Next((int)SpatialSize.Y)), Size = new Vector2(32) };
                    entities.Add(e);
                    spatial.Add(e);

                    Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    Assert.IsTrue(spatial.Contains(e.Position, x => x == e));
                }

                for (var i = 0; i < 40; i++)
                {
                    foreach (var e in entities)
                    {
                        var v = new Vector2(r.Next(moveSize), r.Next(moveSize)) - halfMoveSize;
                        var newPos = v + e.Position;
                        if (newPos.X < 0 || newPos.X >= SpatialSize.X - e.Size.X)
                            v.X = 0;
                        if (newPos.Y < 0 || newPos.Y >= SpatialSize.Y - e.Size.Y)
                            v.Y = 0;

                        e.Move(v);

                        Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                        Assert.IsTrue(spatial.Contains(e.Position, x => x == e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    }
                }
            }
        }

        [Test(Description = "Puts an emphasis on ensuring Teleport works.")]
        public void MovementTest02()
        {
            var moveSize = (int)(Math.Min(SpatialSize.X, SpatialSize.Y) / 4);
            var halfMoveSize = new Vector2(moveSize) / 2f;

            var r = new Random(654987645);
            foreach (var spatial in GetSpatials())
            {
                var entities = new List<TestEntity>();
                for (var i = 0; i < 20; i++)
                {
                    var e = new TestEntity
                    { Position = new Vector2(r.Next((int)SpatialSize.X), r.Next((int)SpatialSize.Y)), Size = new Vector2(32) };
                    entities.Add(e);
                    spatial.Add(e);

                    Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    Assert.IsTrue(spatial.Contains(e.Position, x => x == e));
                }

                for (var i = 0; i < 40; i++)
                {
                    foreach (var e in entities)
                    {
                        var v = new Vector2(r.Next(moveSize), r.Next(moveSize)) - halfMoveSize;
                        var newPos = v + e.Position;

                        if (newPos.X < 0)
                            newPos.X = 0;
                        else if (newPos.X >= SpatialSize.X - e.Size.X)
                            newPos.X = SpatialSize.X - e.Size.X;

                        if (newPos.Y < 0)
                            newPos.Y = 0;
                        else if (newPos.Y >= SpatialSize.Y - e.Size.Y)
                            newPos.Y = SpatialSize.Y - e.Size.Y;

                        e.Position = newPos;

                        Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                        Assert.IsTrue(spatial.Contains(e.Position, x => x == e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    }
                }
            }
        }

        [Test(Description = "Tests both Teleport and Move().")]
        public void MovementTest03()
        {
            var moveSize = (int)(Math.Min(SpatialSize.X, SpatialSize.Y) / 4);
            var halfMoveSize = new Vector2(moveSize) / 2f;

            var r = new Random(654987645);
            foreach (var spatial in GetSpatials())
            {
                var entities = new List<TestEntity>();
                for (var i = 0; i < 20; i++)
                {
                    var e = new TestEntity
                    { Position = new Vector2(r.Next((int)SpatialSize.X), r.Next((int)SpatialSize.Y)), Size = new Vector2(32) };
                    entities.Add(e);
                    spatial.Add(e);

                    Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    Assert.IsTrue(spatial.Contains(e.Position, x => x == e));
                }

                for (var i = 0; i < 40; i++)
                {
                    foreach (var e in entities)
                    {
                        var v = new Vector2(r.Next(moveSize), r.Next(moveSize)) - halfMoveSize;
                        var newPos = v + e.Position;

                        var teleport = r.Next(0, 2) == 0;
                        if (teleport)
                        {
                            if (newPos.X < 0)
                                newPos.X = 0;
                            else if (newPos.X >= SpatialSize.X - e.Size.X)
                                newPos.X = SpatialSize.X - e.Size.X;

                            if (newPos.Y < 0)
                                newPos.Y = 0;
                            else if (newPos.Y >= SpatialSize.Y - e.Size.Y)
                                newPos.Y = SpatialSize.Y - e.Size.Y;

                            e.Position = newPos;
                        }
                        else
                        {
                            if (newPos.X < 0 || newPos.X >= SpatialSize.X - e.Size.X)
                                v.X = 0;
                            if (newPos.Y < 0 || newPos.Y >= SpatialSize.Y - e.Size.Y)
                                v.Y = 0;

                            e.Move(v);
                        }

                        Assert.IsTrue(spatial.CollectionContains(e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                        Assert.IsTrue(spatial.Contains(e.Position, x => x == e), "Spatial: {0}, i: {1}", spatial.GetType().Name, i);
                    }
                }
            }
        }

        [Test]
        public void RemoveTest()
        {
            foreach (var spatial in GetSpatials())
            {
                var entity = new TestEntity();
                spatial.Add(entity);
                Assert.IsTrue(spatial.CollectionContains(entity), "Current spatial: " + spatial);

                spatial.Remove(entity);
                Assert.IsFalse(spatial.CollectionContains(entity), "Current spatial: " + spatial);
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