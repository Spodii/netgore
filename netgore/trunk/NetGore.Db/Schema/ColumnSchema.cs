using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore.Db.Schema
{
    /// <summary>
    /// Describes the schema for a database column.
    /// </summary>
    [Serializable]
    public class ColumnSchema
    {
        const string _keyValueName = "Key";
        const string _valueValueName = "Value";
        const string _valuesNodeName = "Values";

        /// <summary>
        /// All the schema value names possible for the <see cref="ColumnSchema"/>.
        /// </summary>
        static readonly string[] _valueNames = new string[]
        {
            "TABLE_NAME", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "CHARACTER_OCTET_LENGTH", "NUMERIC_PRECISION",
            "NUMERIC_SCALE", "COLUMN_TYPE", "CHARACTER_SET_NAME", "COLUMN_KEY", "EXTRA", "COLUMN_DEFAULT", "IS_NULLABLE"
        };

        readonly IDictionary<string, string> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public ColumnSchema(IValueReader reader)
        {
            var values = reader.ReadManyNodes(_valuesNodeName, ReadKVP);
            _values = values.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public ColumnSchema(IDictionary<string, string> values)
        {
            _values = values;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnSchema"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IDataRecord"/> to read the values from.</param>
        public ColumnSchema(IDataRecord r)
        {
            _values = ReadValues(r);
        }

        /// <summary>
        /// Gets the value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the given <paramref name="key"/>, or null if the key does not exist.</returns>
        public string this[string key]
        {
            get
            {
                string ret;
                if (_values.TryGetValue(key, out ret))
                    return ret;

                return null;
            }
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get { return this["COLUMN_NAME"]; }
        }

        /// <summary>
        /// Gets the name of the table this column is on.
        /// </summary>
        public string TableName
        {
            get { return this["TABLE_NAME"]; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the schema value names possible for the <see cref="ColumnSchema"/>.
        /// </summary>
        public static IEnumerable<string> ValueNames
        {
            get { return _valueNames; }
        }

        /// <summary>
        /// Checks if the values of this <see cref="ColumnSchema"/> are equal to the values in the
        /// <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The other <see cref="ColumnSchema"/>.</param>
        /// <returns>True if all values are equal; otherwise false.</returns>
        public bool EqualValues(ColumnSchema other)
        {
            foreach (var kvp in _values)
            {
                var otherValue = other[kvp.Key];

                var v1 = string.IsNullOrEmpty(otherValue) ? null : otherValue;
                var v2 = string.IsNullOrEmpty(kvp.Value) ? null : kvp.Value;

                if (v1 != v2)
                    return false;
            }

            return true;
        }

        static KeyValuePair<string, string> ReadKVP(IValueReader reader)
        {
            var key = reader.ReadString(_keyValueName);
            var value = reader.ReadString(_valueValueName);
            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// Reads the <see cref="ColumnSchema"/> values from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r">The <see cref="IDataRecord"/> to read the values from.</param>
        /// <returns>The read values.</returns>
        public static IDictionary<string, string> ReadValues(IDataRecord r)
        {
            var d = new Dictionary<string, string>(_valueNames.Length);
            foreach (var v in ValueNames)
            {
                var ordinal = r.GetOrdinal(v);
                if (r.IsDBNull(ordinal))
                {
                    // Value is null

                    // ReSharper disable AssignNullToNotNullAttribute
                    d.Add(v, null);
                    // ReSharper restore AssignNullToNotNullAttribute
                }
                else
                {
                    // Value is not null
                    var i = r.GetString(ordinal);
                    d.Add(v, i);
                }
            }

            return d;
        }

        /// <summary>
        /// Writes the <see cref="ColumnSchema"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            var v = _values.OrderBy(x => x.Key).ToArray();
            writer.WriteManyNodes(_valuesNodeName, v, WriteKVP);
        }

        static void WriteKVP(IValueWriter writer, KeyValuePair<string, string> value)
        {
            writer.Write(_keyValueName, value.Key);
            writer.Write(_valueValueName, value.Value ?? string.Empty);
        }
    }
}