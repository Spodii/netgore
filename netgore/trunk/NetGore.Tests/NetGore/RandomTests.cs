using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Extensions;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class RandomTests
    {
        [Test]
        public void NextFloatTest()
        {
            var r = new Random(555);
            float last = 0f;

            for (int i = 0; i < 20; i++)
            {
                var f = r.NextFloat();
                Assert.AreNotEqual(last, f);
                Assert.Less(f, 1f);
                Assert.GreaterOrEqual(f, 0f);
                last = f;
            }
        }
    }
}
