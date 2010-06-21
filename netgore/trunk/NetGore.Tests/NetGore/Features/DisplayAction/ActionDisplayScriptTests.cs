using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Features.ActionDisplays;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Features.ActionDisplays
{
    [TestFixture]
    public class ActionDisplayScriptTests
    {
        [Test]
        public void CallScriptTest()
        {
            var handler = ActionDisplayScriptManager.GetHandler("__UNITTEST_TEST");
            Assert.IsNotNull(handler);

            TestClass.i = 0;

            handler(null, null, null, null);

            Assert.AreEqual(50, TestClass.i);
        }

        [ActionDisplayScriptCollection]
        static class TestClass
        {
            public static int i = 0;

            [ActionDisplayScript("__UNITTEST_TEST")]
            public static void Test(ActionDisplay actionDisplay, IMap map, Entity source, Entity target)
            {
                i = 50;
            }
        }
    }
}
