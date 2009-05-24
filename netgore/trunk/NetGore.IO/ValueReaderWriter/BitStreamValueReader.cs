using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO.ValueReaderWriter
{
    public class BitStreamValueReader : IValueReader
    {
        readonly BitStream _reader;

        public BitStreamValueReader(BitStream reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _reader = reader;
        }

        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        public float ReadFloat()
        {
            return _reader.ReadFloat();
        }

        public int ReadInt()
        {
            return _reader.ReadInt();
        }

        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        public short ReadShort()
        {
            return _reader.ReadShort();
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }

        public uint ReadUInt()
        {
            return _reader.ReadUInt();
        }

        public ushort ReadUShort()
        {
            return _reader.ReadUShort();
        }
    }
}
