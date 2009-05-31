using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the INamedValueWriter using Xml.
    /// </summary>
    public class XmlValueReader : IValueReader
    {
        readonly Dictionary<string, string> _values;

        /// <summary>
        /// XmlValueReader constructor.
        /// </summary>
        /// <param name="reader">XmlReader that the values will be read from.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from.</param>
        public XmlValueReader(XmlReader reader, string rootNodeName)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _values = ReadNodesIntoDictionary(reader, rootNodeName);
        }

        /// <summary>
        /// Reads the contents of each Element node and places it into a dictionary. Attributes will be ignored and
        /// any Element node containing child Element nodes, not a value, will result in an exception being thrown.
        /// </summary>
        /// <param name="reader">XmlReader to read the values from.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from. Serves as a means of checking
        /// to ensure the XmlValueReader is at the expected location.</param>
        /// <returns>Dictionary containing the content of each Element node.</returns>
        static Dictionary<string, string> ReadNodesIntoDictionary(XmlReader reader, string rootNodeName)
        {
            var ret = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Read past the first node if it is the root node
            if (string.Equals(reader.Name, rootNodeName, StringComparison.OrdinalIgnoreCase))
                reader.Read();

            // Read all the values
            while (true)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Read the name and value of the element
                        string key = reader.Name;
                        string value = reader.ReadElementContentAsString();
                        ret.Add(key, value);
                        break;

                    case XmlNodeType.EndElement:
                        // Check if we hit the end of the nodes
                        if (string.Equals(reader.Name, rootNodeName, StringComparison.OrdinalIgnoreCase))
                            return ret;
                        else
                            throw new Exception(string.Format("Was expecting end of element `{0}`, but found `{1}`.", rootNodeName,
                                                              reader.Name));
                }

                // Keep reading
                reader.Read();
            }
        }

        #region IValueReader Members

        /// <summary>
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return true; }
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name)
        {
            return int.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name, int bits)
        {
            return ReadInt(name);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name)
        {
            return uint.Parse(_values[name]);
        }

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name, int bits)
        {
            return ReadUInt(name);
        }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public bool ReadBool(string name)
        {
            return bool.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public short ReadShort(string name)
        {
            return short.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public ushort ReadUShort(string name)
        {
            return ushort.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public byte ReadByte(string name)
        {
            return byte.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public sbyte ReadSByte(string name)
        {
            return sbyte.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public float ReadFloat(string name)
        {
            return float.Parse(_values[name]);
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>String read from the reader.</returns>
        public string ReadString(string name)
        {
            return _values[name];
        }

        #endregion
    }
}