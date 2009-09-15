using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests
{
    [TestFixture]
    public class StringRulesTests
    {
        const string _sLower = "abcdefghijklmnopqrstuvwxyz";
        const string _sUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string _sUpperLower = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string _sNumeric = "1234567890";
        const string _sPunctuation = "-_,.!?\"";
        const string _sSentence = "Hello World!";
        const string _sWhitespace = "    ";

        const int _minLength = 1;
        const int _maxLength = 10000;

        [Test]
        public void AllTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.All);
            Assert.IsTrue(r.IsValid(_sLower));
            Assert.IsTrue(r.IsValid(_sUpper));
            Assert.IsTrue(r.IsValid(_sUpperLower));
            Assert.IsTrue(r.IsValid(_sNumeric));
            Assert.IsTrue(r.IsValid(_sPunctuation));
            Assert.IsTrue(r.IsValid(_sSentence));
            Assert.IsTrue(r.IsValid(_sWhitespace));
        }

        [Test]
        public void WhitespaceTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsTrue(r.IsValid(_sWhitespace));
        }

        [Test]
        public void StringTooShortTest()
        {
            StringRules r = new StringRules(_maxLength, _maxLength, CharType.Alpha | CharType.Punctuation | CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void StringTooLongTest()
        {
            StringRules r = new StringRules(_minLength, _minLength, CharType.Alpha | CharType.Punctuation | CharType.Whitespace);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void AlphaLowerTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.AlphaLower);
            Assert.IsTrue(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void PropertyTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.AlphaLower);
            Assert.AreEqual(_minLength, r.MinLength);
            Assert.AreEqual(_maxLength, r.MaxLength);
            Assert.AreEqual(CharType.AlphaLower, r.AllowedChars);
        }

        [Test]
        public void AlphaTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.Alpha);
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
            StringRules r = new StringRules(_minLength, _maxLength, CharType.AlphaUpper);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsTrue(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void InvalidMinLengthTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate { new StringRules(-1, _maxLength, CharType.AlphaLower); });
        }

        [Test]
        public void InvalidMaxLengthTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate { new StringRules(10, 8, CharType.AlphaLower); });
        }

        [Test]
        public void InvalidCharTypesTest()
        {
            Assert.Throws<ArgumentException>(delegate { new StringRules(_minLength, _maxLength, 0); });
        }

        [Test]
        public void NumericTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.Numeric);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsTrue(r.IsValid(_sNumeric));
            Assert.IsFalse(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }

        [Test]
        public void PunctuationTest()
        {
            StringRules r = new StringRules(_minLength, _maxLength, CharType.Punctuation);
            Assert.IsFalse(r.IsValid(_sLower));
            Assert.IsFalse(r.IsValid(_sUpper));
            Assert.IsFalse(r.IsValid(_sUpperLower));
            Assert.IsFalse(r.IsValid(_sNumeric));
            Assert.IsTrue(r.IsValid(_sPunctuation));
            Assert.IsFalse(r.IsValid(_sSentence));
            Assert.IsFalse(r.IsValid(_sWhitespace));
        }
    }
}