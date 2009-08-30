using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NetGore.Globalization;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the INamedValueWriter using Xml.
    /// </summary>
    public class XmlValueReader : IValueReader
    {
        readonly Dictionary<string, List<string>> _values;

        /// <summary>
        /// XmlValueReader constructor.
        /// </summary>
        /// <param name="reader">XmlReader that the values will be read from.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from.</param>
        public XmlValueReader(XmlReader reader, string rootNodeName) : this(reader, rootNodeName, false)
        {
        }

        /// <summary>
        /// XmlValueReader constructor.
        /// </summary>
        /// <param name="filePath">Path of the file to read.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from.</param>
        public XmlValueReader(string filePath, string rootNodeName)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader r = XmlReader.Create(stream))
                {
                    while (r.Read())
                    {
                        if (r.NodeType == XmlNodeType.Element &&
                            string.Equals(r.Name, rootNodeName, StringComparison.OrdinalIgnoreCase))
                            break;
                    }

                    if (r.EOF)
                        throw new Exception(string.Format("Failed to find the node `{0}` in the file.", rootNodeName));

                    _values = ReadNodesIntoDictionary(r, rootNodeName, true);
                }
            }
        }

        /// <summary>
        /// XmlValueReader constructor.
        /// </summary>
        /// <param name="reader">XmlReader that the values will be read from.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from.</param>
        /// <param name="readAllContent">If true, the XmlReader is expected to be read to the end.</param>
        XmlValueReader(XmlReader reader, string rootNodeName, bool readAllContent)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _values = ReadNodesIntoDictionary(reader, rootNodeName, readAllContent);
        }

        static ArgumentException CreateDuplicateKeysException(string key)
        {
            const string parameterName = "name";
            const string errmsg =
                "Cannot read the value of key `{0}` since multiple values were found with that key." +
                " This method requires that the key's name is unique.";

            return new ArgumentException(string.Format(errmsg, key), parameterName);
        }

        static ArgumentException CreateKeyNotFoundException(string key)
        {
            const string parameterName = "parameterName";
            const string errmsg = "Cannot read the value of key `{0}` since no key with that name was found.";

            return new ArgumentException(string.Format(errmsg, key), parameterName);
        }

        /// <summary>
        /// Reads the contents of each Element node and places it into a dictionary. Attributes will be ignored and
        /// any Element node containing child Element nodes, not a value, will result in an exception being thrown.
        /// </summary>
        /// <param name="reader">XmlReader to read the values from.</param>
        /// <param name="rootNodeName">Name of the root node that is to be read from. Serves as a means of checking
        /// to ensure the XmlValueReader is at the expected location.</param>
        /// <param name="readAllContent">If true, the XmlReader is expected to be read to the end.</param>
        /// <returns>Dictionary containing the content of each Element node.</returns>
        static Dictionary<string, List<string>> ReadNodesIntoDictionary(XmlReader reader, string rootNodeName, bool readAllContent)
        {
            var ret = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            // Read past the first node if it is the root node
            if (string.Equals(reader.Name, rootNodeName, StringComparison.OrdinalIgnoreCase))
                reader.Read();

            // Read all the values
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Read the name and value of the element
                        string key = reader.Name;
                        string value = reader.ReadInnerXml();

                        List<string> l;
                        if (!ret.TryGetValue(key, out l))
                        {
                            l = new List<string>();
                            ret.Add(key, l);
                        }

                        l.Add(value);

                        break;

                    case XmlNodeType.EndElement:
                        // Check if we hit the end of the nodes
                        if (string.Equals(reader.Name, rootNodeName, StringComparison.OrdinalIgnoreCase))
                            return ret;
                        else
                            throw new Exception(string.Format("Was expecting end of element `{0}`, but found `{1}`.", rootNodeName,
                                                              reader.Name));
                }
            }

            if (!readAllContent)
                throw new Exception("XmlReader was read to the end, but this was not expected to happen.");

            return ret;
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
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseInt(values[0]);
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
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseUInt(values[0]);
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
        /// Reads one or more child nodes from the IValueReader.
        /// </summary>
        /// <param name="name">Name of the nodes to read.</param>
        /// <param name="count">The number of nodes to read. Must be greater than 0. An ArgumentOutOfRangeException will
        /// be thrown if this value exceeds the actual number of nodes available.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count is less than 0.</exception>
        public IEnumerable<IValueReader> ReadNodes(string name, int count)
        {
            if (count == 0)
                return Enumerable.Empty<IValueReader>();

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            var valuesAsList = _values[name];
            IEnumerable<string> values;

            if (valuesAsList.Count() < count)
                throw new ArgumentOutOfRangeException("count", "Value is greater than the actual number of available nodes.");

            if (valuesAsList.Count() > count)
                values = valuesAsList.Take(count);
            else
                values = valuesAsList;

            var ret = new List<IValueReader>(count);

            foreach (string value in values.Take(count))
            {
                string trimmed = value.Trim();
                var bytes = UTF8Encoding.UTF8.GetBytes(trimmed);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (XmlReader r = XmlReader.Create(ms, new XmlReaderSettings
                    {
                        ConformanceLevel = ConformanceLevel.Fragment
                    }))
                    {
                        XmlValueReader reader = new XmlValueReader(r, name, true);
                        ret.Add(reader);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public bool ReadBool(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseBool(values[0]);
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public short ReadShort(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseShort(values[0]);
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public ushort ReadUShort(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseUShort(values[0]);
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public byte ReadByte(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseByte(values[0]);
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public sbyte ReadSByte(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseSByte(values[0]);
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public float ReadFloat(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return Parser.Invariant.ParseFloat(values[0]);
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>String read from the reader.</returns>
        public string ReadString(string name)
        {
            var values = _values[name];

            if (values.Count == 0)
                throw CreateKeyNotFoundException(name);

            if (values.Count > 1)
                throw CreateDuplicateKeysException(name);

            return values[0];
        }

        #endregion
    }
}