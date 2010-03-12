using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class AssemblyClassInvokerTests
    {
        #region Unit tests

        [Test]
        public void InvalidMethodTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            var i = c.Compile();
            Assert.Throws<MissingMethodException>(() => i.Invoke("asdf"));
        }

        [Test]
        public void InvokeAsStringTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", null, "return \"hi\";");
            var i = c.Compile();

            Assert.AreEqual("hi", i.InvokeAsString("TestM"));
        }

        [Test]
        public void InvokeAsStringWithParamsTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", "a", "return \"hi \" + a;");
            var i = c.Compile();

            Assert.AreEqual("hi loser", i.InvokeAsString("TestM", "loser"));
        }

        [Test]
        public void InvokeEmptyMethodTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", null, null, "");
            var i = c.Compile();
            i.Invoke("TestM");
        }

        #endregion
    }
}