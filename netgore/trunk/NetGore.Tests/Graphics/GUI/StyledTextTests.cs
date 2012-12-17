using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Graphics.GUI;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.Graphics.GUI
{
    [TestFixture]
    public class StyledTextTests
    {
        #region Unit tests

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "abcd")]
        [Test]
        public void ConcastTestA()
        {
            var s1 = new StyledText("abcd", Color.Black);
            var s2 = new StyledText("123", Color.Black);
            var s3 = new StyledText("xyz", Color.Black);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 });

            Assert.AreEqual(1, concat.Count());
            Assert.AreEqual(s1.Text + s2.Text + s3.Text, concat.First().Text);
            Assert.IsTrue(s1.HasSameStyle(concat.First()));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "abcd")]
        [Test]
        public void ConcastTestB()
        {
            var s1 = new StyledText("abcd", Color.Black);
            var s2 = new StyledText("123", Color.Black);
            var s3 = new StyledText("xyz", Color.White);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 }).ToArray();

            Assert.AreEqual(2, concat.Count());
            Assert.AreEqual(s1.Text + s2.Text, concat[0].Text);
            Assert.AreEqual(s3.Text, concat[1].Text);
            Assert.IsTrue(s1.HasSameStyle(concat[0]));
            Assert.IsTrue(s3.HasSameStyle(concat[1]));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "abcd")]
        [Test]
        public void ConcastTestC()
        {
            var s1 = new StyledText("abcd", Color.Black);
            var s2 = new StyledText("123", Color.White);
            var s3 = new StyledText("xyz", Color.Black);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 }).ToArray();

            Assert.AreEqual(3, concat.Count());
            Assert.AreEqual(s1.Text, concat[0].Text);
            Assert.AreEqual(s2.Text, concat[1].Text);
            Assert.AreEqual(s3.Text, concat[2].Text);
            Assert.IsTrue(s1.HasSameStyle(concat[0]));
            Assert.IsTrue(s2.HasSameStyle(concat[1]));
            Assert.IsTrue(s3.HasSameStyle(concat[2]));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ffjfjfj")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void ConstructorCopyStyleTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var s2 = new StyledText("ffjfjfj", s);

            Assert.AreEqual(originalString, s.Text);
            Assert.AreEqual("ffjfjfj", s2.Text);

            Assert.AreEqual(Color.Black, s.Color);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void ConstructorTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            Assert.AreEqual(originalString, s.Text);
            Assert.AreEqual(Color.Black, s.Color);
        }

        [Test]
        public void EmptyIEnumerableToStringTest()
        {
            Assert.AreEqual(string.Empty, StyledText.ToString(new StyledText[0]));
        }

        [Test]
        public void EmptyIEnumerableToStringWithDelimiterTest()
        {
            Assert.AreEqual(string.Empty, StyledText.ToString(new StyledText[0], "WWW"));
        }

        [Test]
        public void HasSameStyleFalseTest()
        {
            var a = new StyledText("abc", Color.Red);
            var b = new StyledText("123", Color.Green);

            Assert.IsFalse(a.HasSameStyle(b));
            Assert.IsFalse(b.HasSameStyle(a));
        }

        [Test]
        public void HasSameStyleTrueTest()
        {
            var a = new StyledText("abc", Color.Red);
            var b = new StyledText("123", a);

            Assert.IsTrue(a.HasSameStyle(b));
            Assert.IsTrue(b.HasSameStyle(a));
        }

        [Test]
        public void IEnumerableToStringTest()
        {
            var a = new StyledText("abc");
            var b = new StyledText("123");
            var c = new StyledText("xyz");
            var v = new StyledText[] { a, b, c };

            Assert.AreEqual("abc123xyz", StyledText.ToString(v));
        }

        [Test]
        public void IEnumerableToStringWithDelimiterTest()
        {
            var a = new StyledText("abc");
            var b = new StyledText("123");
            var c = new StyledText("xyz");
            var v = new StyledText[] { a, b, c };

            Assert.AreEqual("abcWWW123WWWxyz", StyledText.ToString(v, "WWW"));
        }

        [Test]
        public void NullIEnumerableToStringTest()
        {
            Assert.AreEqual(string.Empty, StyledText.ToString(null));
        }

        [Test]
        public void NullIEnumerableToStringWithDelimiterTest()
        {
            Assert.AreEqual(string.Empty, StyledText.ToString(null, "WWW"));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void OperatorAddTest()
        {
            var s = new StyledText("asdf", Color.Black);
            s += "ff";

            Assert.AreEqual("asdfff", s.Text);
            Assert.AreEqual(Color.Black, s.Color);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void OperatorAddTest2()
        {
            var s = new StyledText("asdf", Color.Black);
            var s2 = s + "ff";

            Assert.AreEqual("asdf", s.Text);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual("asdfff", s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "abcd")]
        [Test]
        public void SplitAtTest()
        {
            var s = new StyledText("abcd", Color.Black);
            StyledText l;
            StyledText r;

            s.SplitAt(0, out l, out r);
            Assert.AreEqual("", l.Text);
            Assert.AreEqual("abcd", r.Text);
            Assert.AreEqual(Color.Black, l.Color);
            Assert.AreEqual(Color.Black, r.Color);

            s.SplitAt(1, out l, out r);
            Assert.AreEqual("a", l.Text);
            Assert.AreEqual("bcd", r.Text);
            Assert.AreEqual(Color.Black, l.Color);
            Assert.AreEqual(Color.Black, r.Color);

            s.SplitAt(2, out l, out r);
            Assert.AreEqual("ab", l.Text);
            Assert.AreEqual("cd", r.Text);
            Assert.AreEqual(Color.Black, l.Color);
            Assert.AreEqual(Color.Black, r.Color);

            s.SplitAt(3, out l, out r);
            Assert.AreEqual("abc", l.Text);
            Assert.AreEqual("d", r.Text);
            Assert.AreEqual(Color.Black, l.Color);
            Assert.AreEqual(Color.Black, r.Color);

            s.SplitAt(4, out l, out r);
            Assert.AreEqual("abcd", l.Text);
            Assert.AreEqual("", r.Text);
            Assert.AreEqual(Color.Black, l.Color);
            Assert.AreEqual(Color.Black, r.Color);

            Assert.Throws<ArgumentOutOfRangeException>(() => s.SplitAt(-1, out l, out r));
            Assert.Throws<ArgumentOutOfRangeException>(() => s.SplitAt(5, out l, out r));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitCharsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(' ');
            var expected = originalString.Split(' ');

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitCharsWithCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, 2);
            var expected = originalString.Split(new char[] { ' ' }, 2);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitCharsWithOptionsAndCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitCharsWithOptionsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitStringsWithOptionsAndCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(new string[] { " " }, 4, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new string[] { " " }, 4, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SplitStringsWithOptionsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var split = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SubstringTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var s2 = s.Substring(5);

            Assert.AreEqual(s.Text, originalString);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual(originalString.Substring(5), s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xov")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "xcv")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "werljk")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "qw")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "asdf")]
        [Test]
        public void SubstringWithLengthTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            var s = new StyledText(originalString, Color.Black);
            var s2 = s.Substring(5, 4);

            Assert.AreEqual(s.Text, originalString);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual(originalString.Substring(5, 4), s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "<>g__initLocal4")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lines")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "r")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "s1")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "s2")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "s3")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "s4")]
        [Test]
        public void ToMultiLineTest()
        {
            // Test removed since Font.DefaultFont in the testing framework seems to cause issues
            return;

#pragma warning disable 162
            var s1 = new StyledText("abcd", Color.Black);
            var s2 = new StyledText("\r123", Color.Black);
            var s3 = new StyledText("\nxyz", Color.Black);
            var s4 = new StyledText("\r\nqwe", Color.Black);

            var lines = new List<StyledText> { s1, s2, s3, s4 };

            var r = StyledText.ToMultiline(lines, false, TestHelper.DefaultFont, 400);

            Assert.AreEqual("abcd", r[0][0].Text);
            Assert.AreEqual("123", r[1][0].Text);
            Assert.AreEqual("xyz", r[2][0].Text);
            Assert.AreEqual("qwe", r[3][0].Text);
#pragma warning restore 162
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ve")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "fou")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "fi")]
        [Test]
        public void ToMultilineMultiInputDifferentLineTest()
        {
            const string originalString1 = "one \ntwo";
            const string originalString2 = "three fou\nr";
            const string originalString3 = "fi\r\nve";
            var s1 = new StyledText(originalString1, Color.Black);
            var s2 = new StyledText(originalString2, Color.Black);
            var s3 = new StyledText(originalString3, Color.Black);
            var lines = StyledText.ToMultiline(new StyledText[] { s1, s2, s3 }, true);

            Assert.AreEqual(6, lines.Count);
            Assert.AreEqual("one ", lines[0][0].Text);
            Assert.AreEqual("two", lines[1][0].Text);
            Assert.AreEqual("three fou", lines[2][0].Text);
            Assert.AreEqual("r", lines[3][0].Text);
            Assert.AreEqual("fi", lines[4][0].Text);
            Assert.AreEqual("ve", lines[5][0].Text);

            foreach (var l in lines)
            {
                Assert.AreEqual(s1.Color, l[0].Color);
                Assert.AreEqual(s2.Color, l[0].Color);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "fou")]
        [Test]
        public void ToMultilineMultiInputSameLineTest()
        {
            const string originalString1 = "one \ntwo";
            const string originalString2 = " three fou\nr";
            var s1 = new StyledText(originalString1, Color.Black);
            var s2 = new StyledText(originalString2, Color.Black);
            var lines = StyledText.ToMultiline(new StyledText[] { s1, s2 }, false);

            Assert.AreEqual(3, lines.Count);

            Assert.AreEqual("one ", lines[0][0].Text);

            Assert.AreEqual("two", lines[1][0].Text);
            Assert.AreEqual(" three fou", lines[1][1].Text);

            Assert.AreEqual("r", lines[2][0].Text);

            foreach (var l in lines)
            {
                Assert.AreEqual(s1.Color, l[0].Color);
                Assert.AreEqual(s2.Color, l[0].Color);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "fou")]
        [Test]
        public void ToMultilineOneInputTest()
        {
            const string originalString = "one two\n three fou\nr";
            var s = new StyledText(originalString, Color.Black);
            var lines = StyledText.ToMultiline(s);

            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("one two", lines[0].Text);
            Assert.AreEqual(" three fou", lines[1].Text);
            Assert.AreEqual("r", lines[2].Text);

            foreach (var l in lines)
            {
                Assert.AreEqual(s.Color, l.Color);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "hij")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "defg")]
        [Test]
        public void ToSingleLineTest()
        {
            var a = new StyledText("abc\rdefg\r\nhij\r\nklm\nn");
            Assert.AreEqual("abcdefghijklmn", a.ToSingleline().Text);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "hij")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "defg")]
        [Test]
        public void ToSingleLineWithReplacementTest()
        {
            var a = new StyledText("abc\rdefg\r\nhij\r\nklm\nn");
            Assert.AreEqual("abcXdefgXhijXklmXn", a.ToSingleline("X").Text);
        }

        [Test]
        public void ToStringTest()
        {
            var a = new StyledText("abc", Color.Red);
            var b = new StyledText("123", a);

            Assert.AreEqual("abc", a.ToString());
            Assert.AreEqual("123", b.ToString());
        }

        #endregion
    }
}