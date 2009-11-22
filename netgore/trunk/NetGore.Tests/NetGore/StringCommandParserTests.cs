using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class StringCommandParserTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _parser = new TestParser();
        }

        #endregion

        TestParser _parser;

        static string Implode(params object[] args)
        {
            if (args.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                sb.Append(args[i].ToString());
                sb.Append(" ");
            }
            sb.Length--;
            return sb.ToString();
        }

        [TestCommand("SimpleA")]
        public string SimpleA(string a, string b)
        {
            return Implode(a, b);
        }

        [TestCommand("SimpleB")]
        public string SimpleB(float a, string b, int c)
        {
            return Implode(a, b, c);
        }

        [TestCommand("StaticSimpleA")]
        public string StaticSimpleA(string a, string b)
        {
            return Implode(a, b);
        }

        [TestCommand("StaticSimpleB")]
        public string StaticSimpleB(float a, string b, int c)
        {
            return Implode(a, b, c);
        }

        [Test]
        public void TestSimpleTooFewParameters()
        {
            _parser.TestParseInvalid(this, "SimpleA", "abcd");
            _parser.TestParseInvalid(this, "SimpleB", 10.0, "woot");

            _parser.TestParseInvalid(this, "StaticSimpleA", "abcd");
            _parser.TestParseInvalid(this, "StaticSimpleB", 10.0, "woot", 1005, 10);
        }

        [Test]
        public void TestSimpleValid()
        {
            _parser.TestParse(this, "SimpleA", "abcd", "efgh");
            _parser.TestParse(this, "SimpleB", 10.0, "woot", 1005);

            _parser.TestParse(this, "StaticSimpleA", "abcd", "efgh");
            _parser.TestParse(this, "StaticSimpleB", 10.0, "woot", 1005);
        }

        class TestCommandAttribute : StringCommandBaseAttribute
        {
            public TestCommandAttribute(string command) : base(command)
            {
            }
        }

        class TestParser : StringCommandParser<TestCommandAttribute>
        {
            public TestParser() : base(typeof(StringCommandParserTests))
            {
            }

            public void TestParse(StringCommandParserTests binder, string command, params object[] objs)
            {
                string expected = Implode(objs);
                string actual;
                bool successful = TryParse(binder, command + " " + expected, out actual);

                Assert.IsTrue(successful);
                Assert.AreEqual(expected, actual);
            }

            public void TestParseInvalid(StringCommandParserTests binder, string command, params object[] objs)
            {
                string expected = Implode(objs);
                string actual;
                bool successful = TryParse(binder, command + " " + expected, out actual);
                Assert.IsFalse(successful);
            }
        }
    }
}