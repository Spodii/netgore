using System.Linq;
using NUnit.Framework;
using SFML.Window;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class KeysExtensionsTests
    {
        #region Unit tests

        [Test]
        public void D0Test()
        {
            Assert.AreEqual(0, KeyCode.Num0.GetNumericKeyAsValue());
        }

        [Test]
        public void D1Test()
        {
            Assert.AreEqual(1, KeyCode.Num1.GetNumericKeyAsValue());
        }

        [Test]
        public void D2Test()
        {
            Assert.AreEqual(2, KeyCode.Num2.GetNumericKeyAsValue());
        }

        [Test]
        public void D3Test()
        {
            Assert.AreEqual(3, KeyCode.Num3.GetNumericKeyAsValue());
        }

        [Test]
        public void D4Test()
        {
            Assert.AreEqual(4, KeyCode.Num4.GetNumericKeyAsValue());
        }

        [Test]
        public void D5Test()
        {
            Assert.AreEqual(5, KeyCode.Num5.GetNumericKeyAsValue());
        }

        [Test]
        public void D6Test()
        {
            Assert.AreEqual(6, KeyCode.Num6.GetNumericKeyAsValue());
        }

        [Test]
        public void D7Test()
        {
            Assert.AreEqual(7, KeyCode.Num7.GetNumericKeyAsValue());
        }

        [Test]
        public void D8Test()
        {
            Assert.AreEqual(8, KeyCode.Num8.GetNumericKeyAsValue());
        }

        [Test]
        public void D9Test()
        {
            Assert.AreEqual(9, KeyCode.Num9.GetNumericKeyAsValue());
        }

        [Test]
        public void DNullTest()
        {
            Assert.AreEqual(null, KeyCode.A.GetNumericKeyAsValue());
        }

        [Test]
        public void F01Test()
        {
            Assert.AreEqual(1, KeyCode.F1.GetFunctionKeyAsValue());
        }

        [Test]
        public void F02Test()
        {
            Assert.AreEqual(2, KeyCode.F2.GetFunctionKeyAsValue());
        }

        [Test]
        public void F03Test()
        {
            Assert.AreEqual(3, KeyCode.F3.GetFunctionKeyAsValue());
        }

        [Test]
        public void F04Test()
        {
            Assert.AreEqual(4, KeyCode.F4.GetFunctionKeyAsValue());
        }

        [Test]
        public void F05Test()
        {
            Assert.AreEqual(5, KeyCode.F5.GetFunctionKeyAsValue());
        }

        [Test]
        public void F06Test()
        {
            Assert.AreEqual(6, KeyCode.F6.GetFunctionKeyAsValue());
        }

        [Test]
        public void F07Test()
        {
            Assert.AreEqual(7, KeyCode.F7.GetFunctionKeyAsValue());
        }

        [Test]
        public void F08Test()
        {
            Assert.AreEqual(8, KeyCode.F8.GetFunctionKeyAsValue());
        }

        [Test]
        public void F09Test()
        {
            Assert.AreEqual(9, KeyCode.F9.GetFunctionKeyAsValue());
        }

        [Test]
        public void F10Test()
        {
            Assert.AreEqual(10, KeyCode.F10.GetFunctionKeyAsValue());
        }

        [Test]
        public void F11Test()
        {
            Assert.AreEqual(11, KeyCode.F11.GetFunctionKeyAsValue());
        }

        [Test]
        public void F12Test()
        {
            Assert.AreEqual(12, KeyCode.F12.GetFunctionKeyAsValue());
        }

        [Test]
        public void F13Test()
        {
            Assert.AreEqual(13, KeyCode.F13.GetFunctionKeyAsValue());
        }

        [Test]
        public void F14Test()
        {
            Assert.AreEqual(14, KeyCode.F14.GetFunctionKeyAsValue());
        }

        [Test]
        public void F15Test()
        {
            Assert.AreEqual(15, KeyCode.F15.GetFunctionKeyAsValue());
        }

        [Test]
        public void FNullTest()
        {
            Assert.AreEqual(null, KeyCode.A.GetFunctionKeyAsValue());
        }

        #endregion
    }
}