using System.IO;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class GenericValueReaderWriterTests
    {
        #region Unit tests

        [Test]
        public void BinaryTest()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                using (
                    var writer = GenericValueWriter.Create(format: GenericValueIOFormat.Binary, filePath: filePath,
                        rootNodeName: "Root"))
                {
                    writer.Write("Test", "asdf");
                }

                var reader = GenericValueReader.CreateFromFile(filePath, "Root");
                var s = reader.ReadString("Test");
                Assert.AreEqual("asdf", s);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void XmlTest()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                using (
                    var writer = GenericValueWriter.Create(format: GenericValueIOFormat.Xml, filePath: filePath,
                        rootNodeName: "Root"))
                {
                    writer.Write("Test", "asdf");
                }

                var reader = GenericValueReader.CreateFromFile(filePath, "Root");
                var s = reader.ReadString("Test");
                Assert.AreEqual("asdf", s);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        #endregion
    }
}