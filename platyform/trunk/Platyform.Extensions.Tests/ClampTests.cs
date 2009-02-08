using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Platyform.Extensions;

namespace Platyform.Extensions.Tests
{
    [TestFixture]
    public class ClampTests
    {
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
                            byte l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            byte l = k.Clamp(j, i);
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
            for (int i = -20; i < 20; i++)
            {
                for (int j = -20; j < 20; j++)
                {
                    for (int k = -30; k < 30; k++)
                    {
                        if (i < j)
                        {
                            int l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            int l = k.Clamp(j, i);
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
                            sbyte l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            sbyte l = k.Clamp(j, i);
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
                            short l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            short l = k.Clamp(j, i);
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
                            uint l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            uint l = k.Clamp(j, i);
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
                            ulong l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            ulong l = k.Clamp(j, i);
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
                            ushort l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            ushort l = k.Clamp(j, i);
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
                            double l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            double l = k.Clamp(j, i);
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
                            float l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            float l = k.Clamp(j, i);
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
                            long l = k.Clamp(i, j);
                            Assert.GreaterOrEqual(l, i);
                            Assert.LessOrEqual(l, j);
                        }
                        else
                        {
                            long l = k.Clamp(j, i);
                            Assert.GreaterOrEqual(l, j);
                            Assert.LessOrEqual(l, i);
                        }
                    }
                }
            }
        }
    }
}