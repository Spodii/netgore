using System;
using System.Linq;
using NetGore;
using NetGore.Globalization;

namespace NetGore.IO
{
    /// <summary>
    /// An individual node item, containing the name of the item and its value.
    /// </summary>
    public struct NodeItem
    {
        const string _nameValueKey = "Name";
        const string _valueValueKey = "Value";
        readonly string _name;
        readonly string _value;

        /// <summary>
        /// Gets the name of the node item.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the value of the node item.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeItem"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NodeItem(string name, IFormattable value)
        {
            _name = name;
            _value = value.ToString(null, Parser.Invariant.CultureInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeItem"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NodeItem(string name, string value)
        {
            _name = name;
            _value = value ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeItem"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public NodeItem(string name, object value)
        {
            _name = name;
            _value = value == null ? string.Empty : value.ToString();
        }

        public NodeItem(IValueReader reader)
        {
            string name = reader.ReadString(_nameValueKey);
            string value = reader.ReadString(_valueValueKey);

            _name = name;
            _value = value;
        }

        public void Write(IValueWriter writer)
        {
            writer.Write(_nameValueKey, Name);
            writer.Write(_valueValueKey, Value);
        }
    }
}