using System.CodeDom.Compiler;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class JScriptAssemblyCreatorTests
    {
        [Test]
        public void AddMethodNoParametersTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", null, "return \"hi\";");

            CompilerResults r;
            c.Compile(out r);

            Assert.IsNotNull(r.CompiledAssembly);
            Assert.AreEqual(0, r.Errors.Count);
        }

        [Test]
        public void AddMethodOneParameterTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", "String", "a", "return \"hi \" + a;");

            CompilerResults r;
            c.Compile(out r);

            Assert.IsNotNull(r.CompiledAssembly);
            Assert.AreEqual(0, r.Errors.Count);
        }

        [Test]
        public void AddEmptyMethodTest()
        {
            JScriptAssemblyCreator c = new JScriptAssemblyCreator { ClassName = "TestClass" };
            c.AddMethod("TestM", "public", null, null, "");

            CompilerResults r;
            c.Compile(out r);

            Assert.IsNotNull(r.CompiledAssembly);
            Assert.AreEqual(0, r.Errors.Count);
        }
    }
}