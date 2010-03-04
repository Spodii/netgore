using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Extensions;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class Vector2Tests
    {
        #region Unit tests

        [Test]
        public void AbsTest()
        {
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    Vector2 v = new Vector2(x, y);
                    v = v.Abs();
                    Assert.LessOrEqual(0, v.X);
                    Assert.LessOrEqual(0, v.Y);
                }
            }
        }

        [Test]
        public void CeilingTest()
        {
            const int max = 1000;

            Random r = new Random(987);

            for (int i = 0; i < 30; i++)
            {
                var v = new Vector2(r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Ceiling();
                Assert.AreEqual(Math.Ceiling(v.X), c.X);
                Assert.AreEqual(Math.Ceiling(v.Y), c.Y);
            }
        }

        [Test]
        public void FloorTest()
        {
            const int max = 1000;

            Random r = new Random(102);

            for (int i = 0; i < 30; i++)
            {
                var v = new Vector2(r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Floor();
                Assert.AreEqual(Math.Floor(v.X), c.X);
                Assert.AreEqual(Math.Floor(v.Y), c.Y);
            }
        }

        [Test]
        public void IsGreaterOrEqualTest()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Vector2 v1 = new Vector2(x1, y1);
                            Vector2 v2 = new Vector2(x2, y2);

                            bool b1 = (v1.X >= v2.X && v1.Y >= v2.Y);
                            Assert.AreEqual(b1, v1.IsGreaterOrEqual(v2));

                            bool b2 = (v2.X >= v1.X && v2.Y >= v1.Y);
                            Assert.AreEqual(b2, v2.IsGreaterOrEqual(v1));
                        }
                    }
                }
            }
        }

        [Test]
        public void IsGreaterThanTest()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Vector2 v1 = new Vector2(x1, y1);
                            Vector2 v2 = new Vector2(x2, y2);

                            bool b1 = (v1.X > v2.X && v1.Y > v2.Y);
                            Assert.AreEqual(b1, v1.IsGreaterThan(v2));

                            bool b2 = (v2.X > v1.X && v2.Y > v1.Y);
                            Assert.AreEqual(b2, v2.IsGreaterThan(v1));
                        }
                    }
                }
            }
        }

        [Test]
        public void IsLessOrEqualTest()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Vector2 v1 = new Vector2(x1, y1);
                            Vector2 v2 = new Vector2(x2, y2);

                            bool b1 = (v1.X <= v2.X && v1.Y <= v2.Y);
                            Assert.AreEqual(b1, v1.IsLessOrEqual(v2));

                            bool b2 = (v2.X <= v1.X && v2.Y <= v1.Y);
                            Assert.AreEqual(b2, v2.IsLessOrEqual(v1));
                        }
                    }
                }
            }
        }

        [Test]
        public void IsLessThanTest()
        {
            for (int x1 = -10; x1 < 10; x1++)
            {
                for (int y1 = -10; y1 < 10; y1++)
                {
                    for (int x2 = -10; x2 < 10; x2++)
                    {
                        for (int y2 = -10; y2 < 10; y2++)
                        {
                            Vector2 v1 = new Vector2(x1, y1);
                            Vector2 v2 = new Vector2(x2, y2);

                            bool b1 = (v1.X < v2.X && v1.Y < v2.Y);
                            Assert.AreEqual(b1, v1.IsLessThan(v2));

                            bool b2 = (v2.X < v1.X && v2.Y < v1.Y);
                            Assert.AreEqual(b2, v2.IsLessThan(v1));
                        }
                    }
                }
            }
        }

        [Test]
        public void RoundTest()
        {
            const int max = 1000;

            Random r = new Random(578);

            for (int i = 0; i < 30; i++)
            {
                var v = new Vector2(r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Round();
                Assert.AreEqual(Math.Round(v.X), c.X);
                Assert.AreEqual(Math.Round(v.Y), c.Y);
            }
        }

        [Test]
        public void SumTest()
        {
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    Vector2 v = new Vector2(x, y);
                    Assert.AreEqual(x + y, v.Sum());
                }
            }
        }

        #endregion
    }
}