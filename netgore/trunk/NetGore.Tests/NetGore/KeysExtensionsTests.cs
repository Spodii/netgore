using System.Linq;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class KeysExtensionsTests
    {
        #region Unit tests

        [Test]
        public void D0Test()
        {
            Assert.AreEqual(0, Keys.D0.GetNumericKeyAsValue());
        }

        [Test]
        public void D1Test()
        {
            Assert.AreEqual(1, Keys.D1.GetNumericKeyAsValue());
        }

        [Test]
        public void D2Test()
        {
            Assert.AreEqual(2, Keys.D2.GetNumericKeyAsValue());
        }

        [Test]
        public void D3Test()
        {
            Assert.AreEqual(3, Keys.D3.GetNumericKeyAsValue());
        }

        [Test]
        public void D4Test()
        {
            Assert.AreEqual(4, Keys.D4.GetNumericKeyAsValue());
        }

        [Test]
        public void D5Test()
        {
            Assert.AreEqual(5, Keys.D5.GetNumericKeyAsValue());
        }

        [Test]
        public void D6Test()
        {
            Assert.AreEqual(6, Keys.D6.GetNumericKeyAsValue());
        }

        [Test]
        public void D7Test()
        {
            Assert.AreEqual(7, Keys.D7.GetNumericKeyAsValue());
        }

        [Test]
        public void D8Test()
        {
            Assert.AreEqual(8, Keys.D8.GetNumericKeyAsValue());
        }

        [Test]
        public void D9Test()
        {
            Assert.AreEqual(9, Keys.D9.GetNumericKeyAsValue());
        }

        [Test]
        public void DNullTest()
        {
            Assert.AreEqual(null, Keys.A.GetNumericKeyAsValue());
        }

        [Test]
        public void F01Test()
        {
            Assert.AreEqual(1, Keys.F1.GetFunctionKeyAsValue());
        }

        [Test]
        public void F02Test()
        {
            Assert.AreEqual(2, Keys.F2.GetFunctionKeyAsValue());
        }

        [Test]
        public void F03Test()
        {
            Assert.AreEqual(3, Keys.F3.GetFunctionKeyAsValue());
        }

        [Test]
        public void F04Test()
        {
            Assert.AreEqual(4, Keys.F4.GetFunctionKeyAsValue());
        }

        [Test]
        public void F05Test()
        {
            Assert.AreEqual(5, Keys.F5.GetFunctionKeyAsValue());
        }

        [Test]
        public void F06Test()
        {
            Assert.AreEqual(6, Keys.F6.GetFunctionKeyAsValue());
        }

        [Test]
        public void F07Test()
        {
            Assert.AreEqual(7, Keys.F7.GetFunctionKeyAsValue());
        }

        [Test]
        public void F08Test()
        {
            Assert.AreEqual(8, Keys.F8.GetFunctionKeyAsValue());
        }

        [Test]
        public void F09Test()
        {
            Assert.AreEqual(9, Keys.F9.GetFunctionKeyAsValue());
        }

        [Test]
        public void F10Test()
        {
            Assert.AreEqual(10, Keys.F10.GetFunctionKeyAsValue());
        }

        [Test]
        public void F11Test()
        {
            Assert.AreEqual(11, Keys.F11.GetFunctionKeyAsValue());
        }

        [Test]
        public void F12Test()
        {
            Assert.AreEqual(12, Keys.F12.GetFunctionKeyAsValue());
        }

        [Test]
        public void F13Test()
        {
            Assert.AreEqual(13, Keys.F13.GetFunctionKeyAsValue());
        }

        [Test]
        public void F14Test()
        {
            Assert.AreEqual(14, Keys.F14.GetFunctionKeyAsValue());
        }

        [Test]
        public void F15Test()
        {
            Assert.AreEqual(15, Keys.F15.GetFunctionKeyAsValue());
        }

        [Test]
        public void F16Test()
        {
            Assert.AreEqual(16, Keys.F16.GetFunctionKeyAsValue());
        }

        [Test]
        public void F17Test()
        {
            Assert.AreEqual(17, Keys.F17.GetFunctionKeyAsValue());
        }

        [Test]
        public void F18Test()
        {
            Assert.AreEqual(18, Keys.F18.GetFunctionKeyAsValue());
        }

        [Test]
        public void F19Test()
        {
            Assert.AreEqual(19, Keys.F19.GetFunctionKeyAsValue());
        }

        [Test]
        public void F20Test()
        {
            Assert.AreEqual(20, Keys.F20.GetFunctionKeyAsValue());
        }

        [Test]
        public void F21Test()
        {
            Assert.AreEqual(21, Keys.F21.GetFunctionKeyAsValue());
        }

        [Test]
        public void F22Test()
        {
            Assert.AreEqual(22, Keys.F22.GetFunctionKeyAsValue());
        }

        [Test]
        public void F23Test()
        {
            Assert.AreEqual(23, Keys.F23.GetFunctionKeyAsValue());
        }

        [Test]
        public void F24Test()
        {
            Assert.AreEqual(24, Keys.F24.GetFunctionKeyAsValue());
        }

        [Test]
        public void FNullTest()
        {
            Assert.AreEqual(null, Keys.A.GetFunctionKeyAsValue());
        }

        #endregion
    }
}