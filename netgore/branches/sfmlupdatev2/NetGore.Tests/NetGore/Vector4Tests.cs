using System;
using System.Linq;
using NetGore.Extensions;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class Vector4Tests
    {
        #region Unit tests

        [Test]
        public void AbsTest()
        {
            for (var x = -10; x < 10; x++)
            {
                for (var y = -10; y < 10; y++)
                {
                    for (var z = -10; z < 10; z++)
                    {
                        for (var w = -10; w < 10; w++)
                        {
                            var v = new Vector4(x, y, z, w);
                            v = v.Abs();
                            Assert.LessOrEqual(0, v.X);
                            Assert.LessOrEqual(0, v.Y);
                            Assert.LessOrEqual(0, v.Z);
                        }
                    }
                }
            }
        }

        [Test]
        public void CeilingTest()
        {
            const int max = 1000;

            var r = new Random(987);

            for (var i = 0; i < 30; i++)
            {
                var v = new Vector4(r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Ceiling();
                Assert.AreEqual(Math.Ceiling(v.X), c.X);
                Assert.AreEqual(Math.Ceiling(v.Y), c.Y);
                Assert.AreEqual(Math.Ceiling(v.Z), c.Z);
                Assert.AreEqual(Math.Ceiling(v.W), c.W);
            }
        }

        [Test]
        public void FloorTest()
        {
            const int max = 1000;

            var r = new Random(102);

            for (var i = 0; i < 30; i++)
            {
                var v = new Vector4(r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Floor();
                Assert.AreEqual(Math.Floor(v.X), c.X);
                Assert.AreEqual(Math.Floor(v.Y), c.Y);
                Assert.AreEqual(Math.Floor(v.Z), c.Z);
                Assert.AreEqual(Math.Floor(v.W), c.W);
            }
        }

        [Test]
        public void IsGreaterOrEqualTest()
        {
            for (var x1 = -1; x1 < 1; x1++)
            {
                for (var y1 = -1; y1 < 1; y1++)
                {
                    for (var z1 = -1; z1 < 1; z1++)
                    {
                        for (var w1 = -2; w1 < 2; w1++)
                        {
                            for (var x2 = -2; x2 < 2; x2++)
                            {
                                for (var y2 = -2; y2 < 2; y2++)
                                {
                                    for (var z2 = -2; z2 < 2; z2++)
                                    {
                                        for (var w2 = -3; w2 < 3; w2++)
                                        {
                                            var v1 = new Vector4(x1, y1, z1, w1);
                                            var v2 = new Vector4(x2, y2, z2, w2);

                                            var b1 = (v1.X >= v2.X && v1.Y >= v2.Y && v1.Z >= v2.Z && v1.W >= v2.W);
                                            Assert.AreEqual(b1, v1.IsGreaterOrEqual(v2));

                                            var b2 = (v2.X >= v1.X && v2.Y >= v1.Y && v2.Z >= v1.Z && v2.W >= v1.W);
                                            Assert.AreEqual(b2, v2.IsGreaterOrEqual(v1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void IsGreaterThanTest()
        {
            for (var x1 = -1; x1 < 1; x1++)
            {
                for (var y1 = -1; y1 < 1; y1++)
                {
                    for (var z1 = -1; z1 < 1; z1++)
                    {
                        for (var w1 = -2; w1 < 2; w1++)
                        {
                            for (var x2 = -2; x2 < 2; x2++)
                            {
                                for (var y2 = -2; y2 < 2; y2++)
                                {
                                    for (var z2 = -2; z2 < 2; z2++)
                                    {
                                        for (var w2 = -3; w2 < 3; w2++)
                                        {
                                            var v1 = new Vector4(x1, y1, z1, w1);
                                            var v2 = new Vector4(x2, y2, z2, w2);

                                            var b1 = (v1.X > v2.X && v1.Y > v2.Y && v1.Z > v2.Z && v1.W > v2.W);
                                            Assert.AreEqual(b1, v1.IsGreaterThan(v2));

                                            var b2 = (v2.X > v1.X && v2.Y > v1.Y && v2.Z > v1.Z && v2.W > v1.W);
                                            Assert.AreEqual(b2, v2.IsGreaterThan(v1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void IsLessOrEqualTest()
        {
            for (var x1 = -1; x1 < 1; x1++)
            {
                for (var y1 = -1; y1 < 1; y1++)
                {
                    for (var z1 = -1; z1 < 1; z1++)
                    {
                        for (var w1 = -2; w1 < 2; w1++)
                        {
                            for (var x2 = -2; x2 < 2; x2++)
                            {
                                for (var y2 = -2; y2 < 2; y2++)
                                {
                                    for (var z2 = -2; z2 < 2; z2++)
                                    {
                                        for (var w2 = -3; w2 < 3; w2++)
                                        {
                                            var v1 = new Vector4(x1, y1, z1, w1);
                                            var v2 = new Vector4(x2, y2, z2, w2);

                                            var b1 = (v1.X <= v2.X && v1.Y <= v2.Y && v1.Z <= v2.Z && v1.W <= v2.W);
                                            Assert.AreEqual(b1, v1.IsLessOrEqual(v2));

                                            var b2 = (v2.X <= v1.X && v2.Y <= v1.Y && v2.Z <= v1.Z && v2.W <= v1.W);
                                            Assert.AreEqual(b2, v2.IsLessOrEqual(v1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void IsLessThanTest()
        {
            for (var x1 = -1; x1 < 1; x1++)
            {
                for (var y1 = -1; y1 < 1; y1++)
                {
                    for (var z1 = -1; z1 < 1; z1++)
                    {
                        for (var w1 = -2; w1 < 2; w1++)
                        {
                            for (var x2 = -2; x2 < 2; x2++)
                            {
                                for (var y2 = -2; y2 < 2; y2++)
                                {
                                    for (var z2 = -2; z2 < 2; z2++)
                                    {
                                        for (var w2 = -3; w2 < 3; w2++)
                                        {
                                            var v1 = new Vector4(x1, y1, z1, w1);
                                            var v2 = new Vector4(x2, y2, z2, w2);

                                            var b1 = (v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z && v1.W < v2.W);
                                            Assert.AreEqual(b1, v1.IsLessThan(v2));

                                            var b2 = (v2.X < v1.X && v2.Y < v1.Y && v2.Z < v1.Z && v2.W < v1.W);
                                            Assert.AreEqual(b2, v2.IsLessThan(v1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void RoundTest()
        {
            const int max = 1000;

            var r = new Random(578);

            for (var i = 0; i < 30; i++)
            {
                var v = new Vector4(r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max, r.NextFloat() * max);
                var c = v.Round();
                Assert.AreEqual(Math.Round(v.X), c.X);
                Assert.AreEqual(Math.Round(v.Y), c.Y);
                Assert.AreEqual(Math.Round(v.Z), c.Z);
                Assert.AreEqual(Math.Round(v.W), c.W);
            }
        }

        [Test]
        public void SumTest()
        {
            for (var x = -10; x < 10; x++)
            {
                for (var y = -10; y < 10; y++)
                {
                    for (var z = -10; z < 10; z++)
                    {
                        for (var w = -10; w < 10; w++)
                        {
                            var v = new Vector4(x, y, z, w);
                            Assert.AreEqual(x + y + z + w, v.Sum());
                        }
                    }
                }
            }
        }

        #endregion
    }
}