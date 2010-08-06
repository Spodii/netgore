using System.Linq;
using NetGore.Features.ActionDisplays;
using NetGore.World;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Features.ActionDisplays
{
    [TestFixture]
    public class ActionDisplayScriptTests
    {
        #region Unit tests

        [Test]
        public void CallScriptTest()
        {
            var handler = ActionDisplayScriptManager.GetHandler("__UNITTEST_TEST");
            Assert.IsNotNull(handler);

            TestClass.i = 0;

            handler(null, null, null, null);

            Assert.AreEqual(50, TestClass.i);
        }

        #endregion

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