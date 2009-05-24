using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetGore.IO
{
    public class XmlValueReader : INamedValueReader
    {
        readonly Dictionary<string, string> _values;

        public XmlValueReader(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _values = ReadNodesIntoDictionary(reader);
        }

        static Dictionary<string, string> ReadNodesIntoDictionary(XmlReader reader)
        {
            if (!reader.MoveToElement())
            {
                // TODO: Error message
                Debug.Fail(":(");
            }

            var ret = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            int initialDepth = reader.Depth;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        string key = reader.Name;
                        string value = reader.ReadElementContentAsString();
                        ret.Add(key, value);
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Depth == initialDepth)
                            return ret;
                        break;
                }
            }

            return ret;
        }

        public int ReadInt(string name)
        {
            return int.Parse(_values[name]);
        }

        public uint ReadUInt(string name)
        {
            return uint.Parse(_values[name]);
        }

        public short ReadShort(string name)
        {
            return short.Parse(_values[name]);
        }

        public ushort ReadUShort(string name)
        {
            return ushort.Parse(_values[name]);
        }

        public byte ReadByte(string name)
        {
            return byte.Parse(_values[name]);
        }

        public sbyte ReadSByte(string name)
        {
            return sbyte.Parse(_values[name]);
        }

        public float ReadFloat(string name)
        {
            return float.Parse(_values[name]);
        }

        public string ReadString(string name)
        {
            return _values[name];
        }
    }
}
