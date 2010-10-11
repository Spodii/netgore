using System.Linq;
using NetGore.Db;
using NUnit.Framework;

namespace NetGore.Tests.Db
{
    [TestFixture]
    public class DbQueryBaseTests
    {
        #region Unit tests

        [Test]
        public void FormatParametersIntoValuesStringTest()
        {
            const string expected = "(`a`,`b`,`c`) VALUES (@a,@b,@c)";
            var fields = new string[] { "a", "b", "c" };
            var s = DbQueryBase.FormatParametersIntoValuesString(fields);

            Assert.AreEqual(expected, s);
        }

        #endregion
    }
}