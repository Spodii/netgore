using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class StringRulesTests
    {
        const int _maxLength = 10000;
        const int _minLength = 1;
        const string _sLower = "abcdefghijklmnopqrstuvwxyz";
        const string _sNumeric = "1234567890";
        const string _sPunctuation = "-_,.!?\"";
        const string _sSentence = "Hello World!";
        const string _sUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string _sUpperLower = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string _sWhitespace = "    ";

        #region Unit tests

        [Test]
        public void AllTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.All);
            Assert.IsTrue(r.IsValid(_sLower));
            Assert.IsTrue(r.IsValid(_sUpper));
            Assert.IsTrue(r.IsValid(_sUpperLower));
            Assert.IsTrue(r.IsValid(_sNumeric));
            Assert.IsTrue(r.IsValid(_sPunctuation));
            Assert.IsTrue(r.IsValid(_sSentence));
            Assert.IsTrue(r.IsValid(_sWhitespace));
        }

        [Test]
        public void AlphaLowerTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.AlphaLower);
            Assert.IsTrue(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void AlphaTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.Alpha);
            Assert.IsTrue(r.IsValid(_sLower));
            Assert.IsTrue(r.IsValid(_sUpper));
            Assert.IsTrue(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void AlphaUpperTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.AlphaUpper);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsTrue(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void EmptyInputTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.All);
            Assert.IsFalse(r.IsValid(string.Empty));
        }

        [Test]
        public void InvalidCharTypesTest()
        {
            Assert.Throws<ArgumentException>(delegate { new StringRules(_minLength, _maxLength, 0); });
        }

        [Test]
        public void InvalidMaxLengthTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate { new StringRules(10, 8, CharType.AlphaLower); });
        }

        [Test]
        public void InvalidMinLengthTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate { new StringRules(-1, _maxLength, CharType.AlphaLower); });
        }

        [Test]
        public void NullInputTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.All);
            Assert.IsFalse(r.IsValid(null));
        }

        [Test]
        public void NumericTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.Numeric);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsTrue(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void PropertyTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.AlphaLower);
            Assert.AreEqual(_minLength, r.MinLength);
            Assert.AreEqual(_maxLength, r.MaxLength);
            Assert.AreEqual(CharType.AlphaLower, r.AllowedChars);
        }

        [Test]
        public void PunctuationTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.Punctuation);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsTrue(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void StringTooLongTest()
        {
            var r = new StringRules(_minLength, _minLength, CharType.Alpha | CharType.Punctuation | CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void StringTooShortTest()
        {
            var r = new StringRules(_maxLength, _maxLength, CharType.Alpha | CharType.Punctuation | CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void WhitespaceTest()
        {
            var r = new StringRules(_minLength, _maxLength, CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsTrue(r.IsValid(_sWhitespace));
        }

        #endregion
    }
}