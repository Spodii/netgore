using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class XmlReaderCreationTests
    {
        static XmlReader GetTestXmlValueReaderReader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            sb.AppendLine("<TestValues>");
            {
                sb.AppendLine("<DummyNode>");
                {
                    sb.AppendLine("<DummyValue1>99</DummyValue1>");
                    sb.AppendLine("<DummyValue2>99</DummyValue2>");

                    sb.AppendLine("<Values>");
                    {
                        sb.AppendLine("<MyInt>10</MyInt>");
                        sb.AppendLine("<MyFloat>25.55</MyFloat>");
                        sb.AppendLine("<MyString>Hello!</MyString>");
                    }
                    sb.AppendLine("</Values>");

                    sb.AppendLine("<DummyValue3>99</DummyValue3>");
                    sb.AppendLine("<DummyValue4>99</DummyValue4>");
                }
                sb.AppendLine("</DummyNode>");
            }
            sb.AppendLine("</TestValues>");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var ms = new MemoryStream(bytes, false);

            return XmlReader.Create(ms);
        }

        static void MoveXmlReaderToNode(XmlReader xmlReader, string nodeName)
        {
            while (xmlReader.Read() && xmlReader.Name != nodeName)
            {
            }
        }

        static void TestXmlValueReader(IValueReader reader)
        {
            Assert.AreEqual(10, reader.ReadInt("MyInt"));
            Assert.AreEqual(25.55f, reader.ReadFloat("MyFloat"));
            Assert.AreEqual("Hello!", reader.ReadString("MyString"));
        }

        #region Unit tests

        [Test(Description = "Make sure we can create an XmlValueReader at the start of the node to read.")]
        public void CreateXmlReaderAtNodeTest()
        {
            // Create the reader at the Values node
            var xmlReader = GetTestXmlValueReaderReader();
            MoveXmlReaderToNode(xmlReader, "Values");

            var r = XmlValueReader.Create(xmlReader, "Values");
            TestXmlValueReader(r);
        }

        [Test(Description = "Make sure we can create an XmlValueReader INSIDE the node being read.")]
        public void CreateXmlReaderInsideNodeTest()
        {
            // Create the reader at the MyInt node (first value node)
            var xmlReader = GetTestXmlValueReaderReader();
            MoveXmlReaderToNode(xmlReader, "MyInt");

            var r = XmlValueReader.Create(xmlReader, "Values");
            TestXmlValueReader(r);
        }

        #endregion
    }
}