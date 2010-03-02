using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class MathStringTests
    {
        #region Unit tests

        [Test]
        public void Add3Test()
        {
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        const string op = "+";
                        int target = i + j + k;

                        string s = string.Format("{1}{0}{2}{0}{3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1} {0}{2}{0}   {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}{0} {2}  {0}  {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}   {0}    {2}   {0}     {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                    }
                }
            }
        }

        [Test]
        public void AddTest()
        {
            for (int i = -100; i < 100; i += 9)
            {
                for (int j = -100; j < 100; j += 9)
                {
                    const string op = "+";
                    int target = i + j;

                    string s = string.Format("{1}{0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1} {0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}{0} {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}   {0}    {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                }
            }
        }

        [Test]
        public void Divide3Test()
        {
            for (int i = -7; i < 7; i++)
            {
                for (int j = -7; j < 7; j++)
                {
                    for (int k = -7; k < 7; k++)
                    {
                        if (j == 0 || k == 0)
                            continue;

                        const string op = "/";
                        int target = i / j / k;

                        string s = string.Format("{1}{0}{2}{0}{3}", op, i, j, k);
                        Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                        s = string.Format("{1} {0}{2}{0}   {3}", op, i, j, k);
                        Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                        s = string.Format("{1}{0} {2}  {0}  {3}", op, i, j, k);
                        Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                        s = string.Format("{1}   {0}    {2}   {0}     {3}", op, i, j, k);
                        Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                    }
                }
            }
        }

        [Test]
        public void DivideTest()
        {
            for (int i = -70; i < 70; i += 3)
            {
                for (int j = -70; j < 70; j += 3)
                {
                    if (j == 0)
                        continue;

                    const string op = "/";
                    int target = i / j;

                    string s = string.Format("{1}{0}{2}", op, i, j);
                    Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                    s = string.Format("{1} {0}{2}", op, i, j);
                    Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                    s = string.Format("{1}{0} {2}", op, i, j);
                    Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                    s = string.Format("{1}   {0}    {2}", op, i, j);
                    Assert.AreEqual(target, (int)MathString.Parse(s, null), s);
                }
            }
        }

        [Test]
        public void Exp3Test()
        {
            for (int i = -5; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (j == 0 || k == 0)
                            continue;

                        const string op = "^";
                        double target = Math.Pow(Math.Pow(i, j), k);

                        string s = string.Format("{1}{0}{2}{0}{3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1} {0}{2}{0}   {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}{0} {2}  {0}  {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}   {0}    {2}   {0}     {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                    }
                }
            }
        }

        [Test]
        public void ExpTest()
        {
            for (int i = -20; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    const string op = "^";
                    double target = Math.Pow(i, j);

                    string s = string.Format("{1}{0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1} {0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}{0} {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}   {0}    {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                }
            }
        }

        [Test]
        public void Multiply3Test()
        {
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        const string op = "*";
                        int target = i * j * k;

                        string s = string.Format("{1}{0}{2}{0}{3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1} {0}{2}{0}   {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}{0} {2}  {0}  {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}   {0}    {2}   {0}     {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                    }
                }
            }
        }

        [Test]
        public void MultiplyTest()
        {
            for (int i = -70; i < 70; i += 3)
            {
                for (int j = -70; j < 70; j += 3)
                {
                    const string op = "*";
                    int target = i * j;

                    string s = string.Format("{1}{0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1} {0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}{0} {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}   {0}    {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                }
            }
        }

        [Test]
        public void Subtract3Test()
        {
            for (int i = -7; i < 7; i++)
            {
                for (int j = -7; j < 7; j++)
                {
                    for (int k = -7; k < 7; k++)
                    {
                        const string op = "-";
                        int target = i - j - k;

                        string s = string.Format("{1}{0}{2}{0}{3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1} {0}{2}{0}   {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}{0} {2}  {0}  {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                        s = string.Format("{1}   {0}    {2}   {0}     {3}", op, i, j, k);
                        Assert.AreEqual(target, MathString.Parse(s, null), s);
                    }
                }
            }
        }

        [Test]
        public void SubtractTest()
        {
            for (int i = -70; i < 70; i += 3)
            {
                for (int j = -70; j < 70; j += 3)
                {
                    const string op = "-";
                    int target = i - j;

                    string s = string.Format("{1}{0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1} {0}{2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}{0} {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                    s = string.Format("{1}   {0}    {2}", op, i, j);
                    Assert.AreEqual(target, MathString.Parse(s, null), s);
                }
            }
        }

        #endregion
    }
}