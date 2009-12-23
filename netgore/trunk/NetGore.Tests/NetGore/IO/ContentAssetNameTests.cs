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
