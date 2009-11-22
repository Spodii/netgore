using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;
using NUnit.Framework;

namespace NetGore.Tests.Graphics.GUI
{
    [TestFixture]
    public class StyledTextTests
    {
        [Test]
        public void ConcastTestA()
        {
            StyledText s1 = new StyledText("abcd", Color.Black);
            StyledText s2 = new StyledText("123", Color.Black);
            StyledText s3 = new StyledText("xyz", Color.Black);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 });

            Assert.AreEqual(1, concat.Count());
            Assert.AreEqual(s1.Text + s2.Text + s3.Text, concat.First().Text);
            Assert.IsTrue(s1.HasSameStyle(concat.First()));
        }

        [Test]
        public void ConcastTestB()
        {
            StyledText s1 = new StyledText("abcd", Color.Black);
            StyledText s2 = new StyledText("123", Color.Black);
            StyledText s3 = new StyledText("xyz", Color.White);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 }).ToArray();

            Assert.AreEqual(2, concat.Count());
            Assert.AreEqual(s1.Text + s2.Text, concat[0].Text);
            Assert.AreEqual(s3.Text, concat[1].Text);
            Assert.IsTrue(s1.HasSameStyle(concat[0]));
            Assert.IsTrue(s3.HasSameStyle(concat[1]));
        }

        [Test]
        public void ConcastTestC()
        {
            StyledText s1 = new StyledText("abcd", Color.Black);
            StyledText s2 = new StyledText("123", Color.White);
            StyledText s3 = new StyledText("xyz", Color.Black);
            var concat = StyledText.Concat(new StyledText[] { s1, s2, s3 }).ToArray();

            Assert.AreEqual(3, concat.Count());
            Assert.AreEqual(s1.Text, concat[0].Text);
            Assert.AreEqual(s2.Text, concat[1].Text);
            Assert.AreEqual(s3.Text, concat[2].Text);
            Assert.IsTrue(s1.HasSameStyle(concat[0]));
            Assert.IsTrue(s2.HasSameStyle(concat[1]));
            Assert.IsTrue(s3.HasSameStyle(concat[2]));
        }

        [Test]
        public void ConstructorCopyStyleTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            StyledText s2 = new StyledText("ffjfjfj", s);

            Assert.AreEqual(originalString, s.Text);
            Assert.AreEqual("ffjfjfj", s2.Text);

            Assert.AreEqual(Color.Black, s.Color);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [Test]
        public void ConstructorTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            Assert.AreEqual(originalString, s.Text);
            Assert.AreEqual(Color.Black, s.Color);
        }

        [Test]
        public void OperatorAddTest()
        {
            StyledText s = new StyledText("asdf", Color.Black);
            s += "ff";

            Assert.AreEqual("asdfff", s.Text);
            Assert.AreEqual(Color.Black, s.Color);
        }

        [Test]
        public void OperatorAddTest2()
        {
            StyledText s = new StyledText("asdf", Color.Black);
            StyledText s2 = s + "ff";

            Assert.AreEqual("asdf", s.Text);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual("asdfff", s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [Test]
        public void SplitAtTest()
        {
            StyledText s = new StyledText("abcd", Color.Black);
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

        [Test]
        public void SplitCharsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(' ');
            var expected = originalString.Split(' ');

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SplitCharsWithCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, 2);
            var expected = originalString.Split(new char[] { ' ' }, 2);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SplitCharsWithOptionsAndCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SplitCharsWithOptionsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SplitStringsWithOptionsAndCountTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(new string[] { " " }, 4, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new string[] { " " }, 4, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SplitStringsWithOptionsTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var split = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var expected = originalString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(s.Color, split[i].Color);
                Assert.AreEqual(expected[i], split[i].Text);
            }
        }

        [Test]
        public void SubstringTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            StyledText s2 = s.Substring(5);

            Assert.AreEqual(s.Text, originalString);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual(originalString.Substring(5), s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [Test]
        public void SubstringWithLengthTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            StyledText s2 = s.Substring(5, 4);

            Assert.AreEqual(s.Text, originalString);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual(originalString.Substring(5, 4), s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
        }

        [Test]
        public void ToMultilineMultiInputDifferentLineTest()
        {
            // TODO: $$ Test using \r\n and make sure both chars are removed
            const string originalString1 = "one \ntwo";
            const string originalString2 = "three fou\nr";
            StyledText s1 = new StyledText(originalString1, Color.Black);
            StyledText s2 = new StyledText(originalString2, Color.Black);
            var lines = StyledText.ToMultiline(new StyledText[] { s1, s2 }, true);

            Assert.AreEqual(4, lines.Count);
            Assert.AreEqual("one ", lines[0][0].Text);
            Assert.AreEqual("two", lines[1][0].Text);
            Assert.AreEqual("three fou", lines[2][0].Text);
            Assert.AreEqual("r", lines[3][0].Text);

            foreach (var l in lines)
            {
                Assert.AreEqual(s1.Color, l[0].Color);
                Assert.AreEqual(s2.Color, l[0].Color);
            }
        }

        [Test]
        public void ToMultilineMultiInputSameLineTest()
        {
            const string originalString1 = "one \ntwo";
            const string originalString2 = " three fou\nr";
            StyledText s1 = new StyledText(originalString1, Color.Black);
            StyledText s2 = new StyledText(originalString2, Color.Black);
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

        [Test]
        public void ToMultilineOneInputTest()
        {
            const string originalString = "one two\n three fou\nr";
            StyledText s = new StyledText(originalString, Color.Black);
            var lines = StyledText.ToMultiline(s);

            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("one two", lines[0].Text);
            Assert.AreEqual(" three fou", lines[1].Text);
            Assert.AreEqual("r", lines[2].Text);

            foreach (StyledText l in lines)
            {
                Assert.AreEqual(s.Color, l.Color);
            }
        }
    }
}