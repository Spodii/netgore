using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using NetGore.Scripting;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class JScriptAssemblyCreatorTests
    {
        #region Unit tests

        [Test]
        public void AddEmptyMethodTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", null, null, "");

            Assembly asm;
            c.Compile(out asm);

            Assert.IsNotNull(asm);
        }

        [Test]
        public void AddMethodNoParametersTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", null, "return \"hi\";");

            Assembly asm;
            c.Compile(out asm);

            Assert.IsNotNull(asm);
        }

        [Test]
        public void AddMethodOneParameterTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", "a", "return \"hi \" + a;");

            Assembly asm;
            c.Compile(out asm);

            Assert.IsNotNull(asm);
        }

        #endregion
    }
}