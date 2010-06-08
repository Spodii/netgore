using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Features.DisplayAction;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Features.DisplayAction
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
        public static class TestClass
        {
            public static int i = 0;

            [ActionDisplayScript("__UNITTEST_TEST")]
            public static void Test(ActionDisplay actionDisplay, Entity source, Entity target)
            {
                i = 50;
            }
        }
    }
}
