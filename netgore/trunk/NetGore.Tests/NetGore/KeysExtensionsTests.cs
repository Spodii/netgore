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
            Assert.AreEqual(0, Keyboard.Key.Num0.GetNumericKeyAsValue());
        }

        [Test]
        public void D1Test()
        {
            Assert.AreEqual(1, Keyboard.Key.Num1.GetNumericKeyAsValue());
        }

        [Test]
        public void D2Test()
        {
            Assert.AreEqual(2, Keyboard.Key.Num2.GetNumericKeyAsValue());
        }

        [Test]
        public void D3Test()
        {
            Assert.AreEqual(3, Keyboard.Key.Num3.GetNumericKeyAsValue());
        }

        [Test]
        public void D4Test()
        {
            Assert.AreEqual(4, Keyboard.Key.Num4.GetNumericKeyAsValue());
        }

        [Test]
        public void D5Test()
        {
            Assert.AreEqual(5, Keyboard.Key.Num5.GetNumericKeyAsValue());
        }

        [Test]
        public void D6Test()
        {
            Assert.AreEqual(6, Keyboard.Key.Num6.GetNumericKeyAsValue());
        }

        [Test]
        public void D7Test()
        {
            Assert.AreEqual(7, Keyboard.Key.Num7.GetNumericKeyAsValue());
        }

        [Test]
        public void D8Test()
        {
            Assert.AreEqual(8, Keyboard.Key.Num8.GetNumericKeyAsValue());
        }

        [Test]
        public void D9Test()
        {
            Assert.AreEqual(9, Keyboard.Key.Num9.GetNumericKeyAsValue());
        }

        [Test]
        public void DNullTest()
        {
            Assert.AreEqual(null, Keyboard.Key.A.GetNumericKeyAsValue());
        }

        [Test]
        public void F01Test()
        {
            Assert.AreEqual(1, Keyboard.Key.F1.GetFunctionKeyAsValue());
        }

        [Test]
        public void F02Test()
        {
            Assert.AreEqual(2, Keyboard.Key.F2.GetFunctionKeyAsValue());
        }

        [Test]
        public void F03Test()
        {
            Assert.AreEqual(3, Keyboard.Key.F3.GetFunctionKeyAsValue());
        }

        [Test]
        public void F04Test()
        {
            Assert.AreEqual(4, Keyboard.Key.F4.GetFunctionKeyAsValue());
        }

        [Test]
        public void F05Test()
        {
            Assert.AreEqual(5, Keyboard.Key.F5.GetFunctionKeyAsValue());
        }

        [Test]
        public void F06Test()
        {
            Assert.AreEqual(6, Keyboard.Key.F6.GetFunctionKeyAsValue());
        }

        [Test]
        public void F07Test()
        {
            Assert.AreEqual(7, Keyboard.Key.F7.GetFunctionKeyAsValue());
        }

        [Test]
        public void F08Test()
        {
            Assert.AreEqual(8, Keyboard.Key.F8.GetFunctionKeyAsValue());
        }

        [Test]
        public void F09Test()
        {
            Assert.AreEqual(9, Keyboard.Key.F9.GetFunctionKeyAsValue());
        }

        [Test]
        public void F10Test()
        {
            Assert.AreEqual(10, Keyboard.Key.F10.GetFunctionKeyAsValue());
        }

        [Test]
        public void F11Test()
        {
            Assert.AreEqual(11, Keyboard.Key.F11.GetFunctionKeyAsValue());
        }

        [Test]
        public void F12Test()
        {
            Assert.AreEqual(12, Keyboard.Key.F12.GetFunctionKeyAsValue());
        }

        [Test]
        public void F13Test()
        {
            Assert.AreEqual(13, Keyboard.Key.F13.GetFunctionKeyAsValue());
        }

        [Test]
        public void F14Test()
        {
            Assert.AreEqual(14, Keyboard.Key.F14.GetFunctionKeyAsValue());
        }

        [Test]
        public void F15Test()
        {
            Assert.AreEqual(15, Keyboard.Key.F15.GetFunctionKeyAsValue());
        }

        [Test]
        public void FNullTest()
        {
            Assert.AreEqual(null, Keyboard.Key.A.GetFunctionKeyAsValue());
        }

        #endregion
    }
}