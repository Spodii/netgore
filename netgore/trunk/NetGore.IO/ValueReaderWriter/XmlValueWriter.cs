using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetGore.IO
{
    public class XmlValueWriter : INamedValueWriter
    {
        readonly XmlWriter _writer;

        public XmlValueWriter(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
        }

        public void Write(string name, string value)
        {
            _writer.WriteElementString(name, value);
        }

        public void Write(string name, int value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, float value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, uint value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, short value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, ushort value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, byte value)
        {
            Write(name, value.ToString());
        }

        public void Write(string name, sbyte value)
        {
            Write(name, value.ToString());
        }
    }
}
