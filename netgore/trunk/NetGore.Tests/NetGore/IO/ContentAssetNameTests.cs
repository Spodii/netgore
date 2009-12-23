using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class ContentAssetNameTests
    {
        [Test]
        public void GetAbsoluteFilePathTest()
        {
            string s = new ContentAssetName("myasset").GetAbsoluteFilePath(ContentPaths.Build);
            Assert.IsTrue(s.EndsWith("Content\\myasset.xnb", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetAbsoluteFilePathSeparatorPrefixTest()
        {
            string s = new ContentAssetName("\\myasset").GetAbsoluteFilePath(ContentPaths.Build);
            Assert.IsTrue(s.EndsWith("Content\\myasset.xnb", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetAbsoluteFilePathAlternateSeparatorPrefixTest()
        {
            string s = new ContentAssetName("/myasset").GetAbsoluteFilePath(ContentPaths.Build);
            Assert.IsTrue(s.EndsWith("Content\\myasset.xnb", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void SanitizeSeparatorTest()
        {
            const string s = @"asdf\basdf\wer\asdf";
            Assert.AreEqual(s.Replace("\\", ContentAssetName.PathSeparator), ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizePrefixedSeparatorTest()
        {
            const string s = @"\asdf";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizePrefixedAlternateSeparatorTest()
        {
            const string s = @"/asdf";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizeSuffixedSeparatorTest()
        {
            const string s = @"asdf\";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizeSuffixedAlternateSeparatorTest()
        {
            const string s = @"asdf/";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizePrefixedAndSuffixedSeparatorTest()
        {
            const string s = @"\asdf\";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizePrefixedAndSuffixedAlternateSeparatorTest()
        {
            const string s = @"/asdf/";
            Assert.AreEqual("asdf", ContentAssetName.Sanitize(s));
        }

        [Test]
        public void SanitizeAlternateSeparatorTest()
        {
            const string s = @"asdf/basdf/wer/asdf";
            Assert.AreEqual(s.Replace("/", ContentAssetName.PathSeparator), ContentAssetName.Sanitize(s));
        }

        [Test]
        public void FromAbsoluteFilePathTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent", @"C:\whatever\path\to");
            Assert.AreEqual("mycontent", n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathAlternateSeparatorTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:/whatever/path/to/mycontent", @"C:/whatever/path/to");
            Assert.AreEqual("mycontent", n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathDeepTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent\is\super\awesome", @"C:\whatever\path\to");
            Assert.AreEqual(@"mycontent\is\super\awesome".Replace("\\", ContentAssetName.PathSeparator), n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathAlternateSeparatorDeepTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:/whatever/path/to/mycontent/is/super/awesome", @"C:/whatever/path/to");
            Assert.AreEqual(@"mycontent/is/super/awesome".Replace("/", ContentAssetName.PathSeparator), n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathTrailingSlashTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent", @"C:\whatever\path\to\");
            Assert.AreEqual("mycontent", n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathCapsTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent".ToUpper(), @"C:\whatever\path\to");
            Assert.AreEqual("mycontent", n.Value.ToLower());
        }

        [Test]
        public void FromAbsoluteFilePathWithXNBSuffixTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent.xnb", @"C:\whatever\path\to");
            Assert.AreEqual("mycontent", n.Value);
        }

        [Test]
        public void FromAbsoluteFilePathWithXNBSuffixCapsTest()
        {
            ContentAssetName n = ContentAssetName.FromAbsoluteFilePath(@"C:\whatever\path\to\mycontent.xnb".ToUpper(), @"C:\whatever\path\to");
            Assert.AreEqual("mycontent", n.Value.ToLower());
        }
    }
}
