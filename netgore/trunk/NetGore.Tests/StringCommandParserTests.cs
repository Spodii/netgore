using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetGore.Collections.Tests
{
    [TestFixture]
    public class StringCommandParserTests
    {
        TestParser _parser;

        class TestCommandAttribute : StringCommandBaseAttribute
        {
            public TestCommandAttribute(string command) : base(command)
            {
            }
        }

        class TestParser : StringCommandParser<TestCommandAttribute>
        {
            public TestParser()
                : base(typeof(StringCommandParserTests))
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
        }

        [SetUp]
        public void Setup()
        {
            _parser = new TestParser();
        }

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

        [TestCommandAttribute("SimpleA")]
        public string SimpleA(string a, string b)
        {
            return Implode(a, b);
        }

        [TestCommandAttribute("SimpleB")]
        public string SimpleB(float a, string b, int c)
        {
            return Implode(a, b, c);
        }

        [Test]
        public void TestSimpleValid()
        {
            _parser.TestParse(this, "SimpleA", "abcd", "efgh");
        }
    }
}
