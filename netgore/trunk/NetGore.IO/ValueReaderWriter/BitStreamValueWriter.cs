using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public class BitStreamValueWriter : IValueWriter
    {
        readonly BitStream _writer;

        public BitStreamValueWriter(BitStream writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
        }

        public void Write(uint value)
        {
            _writer.Write(value);
        }

        public void Write(short value)
        {
            _writer.Write(value);
        }

        public void Write(ushort value)
        {
            _writer.Write(value);
        }

        public void Write(byte value)
        {
            _writer.Write(value);
        }

        public void Write(sbyte value)
        {
            _writer.Write(value);
        }

        public void Write(string value)
        {
            _writer.Write(value);
        }

        public void Write(int value)
        {
            _writer.Write(value);
        }

        public void Write(float value)
        {
            _writer.Write(value);
        }
    }
}
