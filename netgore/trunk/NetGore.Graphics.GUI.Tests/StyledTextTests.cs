using System;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace NetGore.Graphics.GUI.Tests
{
    [TestFixture]
    public class StyledTextTests
    {
        [Test]
        public void SubstringTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            var s2 = s.Substring(5);

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
            var s2 = s.Substring(5, 4);

            Assert.AreEqual(s.Text, originalString);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual(originalString.Substring(5, 4), s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
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
            var s2 = s + "ff";

            Assert.AreEqual("asdf", s.Text);
            Assert.AreEqual(Color.Black, s.Color);

            Assert.AreEqual("asdfff", s2.Text);
            Assert.AreEqual(Color.Black, s2.Color);
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
        public void ConstructorTest()
        {
            const string originalString = "asdf werljk xov  .qw 120 xcv;z";
            StyledText s = new StyledText(originalString, Color.Black);
            Assert.AreEqual(originalString, s.Text);
            Assert.AreEqual(Color.Black, s.Color);
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
    }
}