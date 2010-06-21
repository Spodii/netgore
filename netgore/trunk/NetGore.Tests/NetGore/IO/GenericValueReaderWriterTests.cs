using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class GenericValueReaderWriterTests
    {
        [Test]
        public void XmlTest()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                using (var writer = new GenericValueWriter(format: GenericValueIOFormat.Xml, filePath: filePath, rootNodeName: "Root"))
                {
                    writer.Write("Test", "asdf");
                }

                var reader = new GenericValueReader(filePath, "Root");
                string s = reader.ReadString("Test");
                Assert.AreEqual("asdf", s);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void BinaryTest()
        {
            var filePath = Path.GetTempFileName();

            try
            {
                using (var writer = new GenericValueWriter(format: GenericValueIOFormat.Binary, filePath: filePath, rootNodeName: "Root"))
                {
                    writer.Write("Test", "asdf");
                }

                var reader = new GenericValueReader(filePath, "Root");
                string s = reader.ReadString("Test");
                Assert.AreEqual("asdf", s);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
