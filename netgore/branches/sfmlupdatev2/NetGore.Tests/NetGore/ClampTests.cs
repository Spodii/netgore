using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class ClampTests
    {
        #region Unit tests

        [Test]
        public void ClampByteTest()
        {
            for (byte i = 0; i < 40; i++)
            {
                for (byte j = 0; j < 40; j++)
                {
                    for (byte k = 0; k < 60; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampIntTest()
        {
            for (var i = -20; i < 20; i++)
            {
                for (var j = -20; j < 20; j++)
                {
                    for (var k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampSByteTest()
        {
            for (sbyte i = -20; i < 20; i++)
            {
                for (sbyte j = -20; j < 20; j++)
                {
                    for (sbyte k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampShortTest()
        {
            for (short i = -20; i < 20; i++)
            {
                for (short j = -20; j < 20; j++)
                {
                    for (short k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampUIntTest()
        {
            for (uint i = 0; i < 40; i++)
            {
                for (uint j = 0; j < 40; j++)
                {
                    for (uint k = 0; k < 60; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampULongTest()
        {
            for (ulong i = 0; i < 40; i++)
            {
                for (ulong j = 0; j < 40; j++)
                {
                    for (ulong k = 0; k < 60; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClampUShortTest()
        {
            for (ushort i = 0; i < 40; i++)
            {
                for (ushort j = 0; j < 40; j++)
                {
                    for (ushort k = 0; k < 60; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void DoubleShortTest()
        {
            for (double i = -20; i < 20; i++)
            {
                for (double j = -20; j < 20; j++)
                {
                    for (double k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void FloatShortTest()
        {
            for (float i = -20; i < 20; i++)
            {
                for (float j = -20; j < 20; j++)
                {
                    for (float k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        [Test]
        public void LongShortTest()
        {
            for (long i = -20; i < 20; i++)
            {
                for (long j = -20; j < 20; j++)
                {
                    for (long k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            var l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            var l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }

        #endregion
    }
}