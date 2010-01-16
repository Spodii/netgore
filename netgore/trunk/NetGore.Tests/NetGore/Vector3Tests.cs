using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class Vector3Tests
    {
        [Test]
        public void AbsTest()
        {
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    for (int z = -10; z < 10; z++)
                    {
                        Vector3 v = new Vector3(x, y, z);
                        v = v.Abs();
                        Assert.LessOrEqual(0, v.X);
                        Assert.LessOrEqual(0, v.Y);
                        Assert.LessOrEqual(0, v.Z);
                    }
                }
            }
        }

        [Test]
        public void IsGreaterOrEqualTest()
        {
            for (int x1 = -2; x1 < 2; x1++)
            {
                for (int y1 = -2; y1 < 2; y1++)
                {
                    for (int z1 = -5; z1 < 5; z1++)
                    {
                        for (int x2 = -2; x2 < 2; x2++)
                        {
                            for (int y2 = -2; y2 < 2; y2++)
                            {
                                for (int z2 = -5; z2 < 5; z2++)
                                {
                                    Vector3 v1 = new Vector3(x1, y1, z1);
                                    Vector3 v2 = new Vector3(x2, y2, z2);

                                    bool b1 = (v1.X >= v2.X && v1.Y >= v2.Y && v1.Z >= v2.Z);
                                    Assert.AreEqual(b1, v1.IsGreaterOrEqual(v2));

                                    bool b2 = (v2.X >= v1.X && v2.Y >= v1.Y && v2.Z >= v1.Z);
                                    Assert.AreEqual(b2, v2.IsGreaterOrEqual(v1));
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
            for (int x1 = -2; x1 < 2; x1++)
            {
                for (int y1 = -2; y1 < 2; y1++)
                {
                    for (int z1 = -5; z1 < 5; z1++)
                    {
                        for (int x2 = -2; x2 < 2; x2++)
                        {
                            for (int y2 = -2; y2 < 2; y2++)
                            {
                                for (int z2 = -5; z2 < 5; z2++)
                                {
                                    Vector3 v1 = new Vector3(x1, y1, z1);
                                    Vector3 v2 = new Vector3(x2, y2, z2);

                                    bool b1 = (v1.X > v2.X && v1.Y > v2.Y && v1.Z > v2.Z);
                                    Assert.AreEqual(b1, v1.IsGreaterThan(v2));

                                    bool b2 = (v2.X > v1.X && v2.Y > v1.Y && v2.Z > v1.Z);
                                    Assert.AreEqual(b2, v2.IsGreaterThan(v1));
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
            for (int x1 = -2; x1 < 2; x1++)
            {
                for (int y1 = -2; y1 < 2; y1++)
                {
                    for (int z1 = -5; z1 < 5; z1++)
                    {
                        for (int x2 = -2; x2 < 2; x2++)
                        {
                            for (int y2 = -2; y2 < 2; y2++)
                            {
                                for (int z2 = -5; z2 < 5; z2++)
                                {
                                    Vector3 v1 = new Vector3(x1, y1, z1);
                                    Vector3 v2 = new Vector3(x2, y2, z2);

                                    bool b1 = (v1.X <= v2.X && v1.Y <= v2.Y && v1.Z <= v2.Z);
                                    Assert.AreEqual(b1, v1.IsLessOrEqual(v2));

                                    bool b2 = (v2.X <= v1.X && v2.Y <= v1.Y && v2.Z <= v1.Z);
                                    Assert.AreEqual(b2, v2.IsLessOrEqual(v1));
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
            for (int x1 = -2; x1 < 2; x1++)
            {
                for (int y1 = -2; y1 < 2; y1++)
                {
                    for (int z1 = -5; z1 < 5; z1++)
                    {
                        for (int x2 = -2; x2 < 2; x2++)
                        {
                            for (int y2 = -2; y2 < 2; y2++)
                            {
                                for (int z2 = -5; z2 < 5; z2++)
                                {
                                    Vector3 v1 = new Vector3(x1, y1, z1);
                                    Vector3 v2 = new Vector3(x2, y2, z2);

                                    bool b1 = (v1.X < v2.X && v1.Y < v2.Y && v1.Z < v2.Z);
                                    Assert.AreEqual(b1, v1.IsLessThan(v2));

                                    bool b2 = (v2.X < v1.X && v2.Y < v1.Y && v2.Z < v1.Z);
                                    Assert.AreEqual(b2, v2.IsLessThan(v1));
                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void SumTest()
        {
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    for (int z = -10; z < 10; z++)
                    {
                        Vector3 v = new Vector3(x, y, z);
                        Assert.AreEqual(x + y + z, v.Sum());
                    }
                }
            }
        }
    }
}