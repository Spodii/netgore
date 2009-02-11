using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace DemoGame.Server.Tests
{
    [TestFixture]
    public class SayHandlerTests
    {
        [Test]
        public void SplitCommandFromTextTest()
        {
            string command;
            string remainder;

            SayHandler.SplitCommandFromText("/shout test", out command, out remainder);
            Assert.AreEqual("shout", command);
            Assert.AreEqual("test", remainder);

            SayHandler.SplitCommandFromText("/shout test test asdf 123", out command, out remainder);
            Assert.AreEqual("shout", command);
            Assert.AreEqual("test test asdf 123", remainder);

            SayHandler.SplitCommandFromText("/shout /\"tes!#@t test asdf 12\"3", out command, out remainder);
            Assert.AreEqual("shout", command);
            Assert.AreEqual("/\"tes!#@t test asdf 12\"3", remainder);

            SayHandler.SplitCommandFromText("/shout     test", out command, out remainder);
            Assert.AreEqual("shout", command);
            Assert.AreEqual("    test", remainder);

            SayHandler.SplitCommandFromText("/s", out command, out remainder);
            Assert.AreEqual("s", command);
            Assert.IsTrue(string.IsNullOrEmpty(remainder));

            SayHandler.SplitCommandFromText("/s/", out command, out remainder);
            Assert.AreEqual("s/", command);
            Assert.IsTrue(string.IsNullOrEmpty(remainder));
        }
    }
}