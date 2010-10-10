using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db;
using NUnit.Framework;

namespace NetGore.Tests.Db
{
    [TestFixture]
    public class DbQueryBaseTests
    {
        [Test]
        public void FormatParametersIntoValuesStringTest()
        {
            const string expected = "(`a`,`b`,`c`) VALUES (@a,@b,@c)";
            var fields= new string[] { "a", "b", "c" };
            var s=DbQueryBase.FormatParametersIntoValuesString(fields);

            Assert.AreEqual(expected, s);
        }
    }
}
