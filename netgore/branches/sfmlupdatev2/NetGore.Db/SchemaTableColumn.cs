using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Contains the database schema, table, and column for a fully qualified column reference.
    /// </summary>
    public struct SchemaTableColumn : IEquatable<SchemaTableColumn>
    {
        readonly string _schema;
        readonly string _column;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaTableColumn"/> struct.
        /// </summary>
        /// <param name="schema">The name of the schema that the <see cref="_table"/> belongs to.</param>
        /// <param name="table">The name of the table the <paramref name="column"/> belongs to.</param>
        /// <param name="column">The name of the database column.</param>
        public SchemaTableColumn(string schema, string table, string column)
        {
            _schema = schema;
            _table = table;
            _column = column;
        }

        /// <summary>
        /// Gets the name of the database column.
        /// </summary>
        public string Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the name of the table the <see cref="_column"/> belongs to.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// Gets the name of the schema that the <see cref="_table"/> belongs to.
        /// </summary>
        public string Schema
        {
            get { return _schema; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(SchemaTableColumn other)
        {
            return Equals(other._schema, _schema) && Equals(other._column, _column) && Equals(other._table, _table);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is SchemaTableColumn && this == (SchemaTableColumn)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = (_schema != null ? _schema.GetHashCode() : 0);
                result = (result * 397) ^ (_column != null ? _column.GetHashCode() : 0);
                result = (result * 397) ^ (_table != null ? _table.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SchemaTableColumn left, SchemaTableColumn right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SchemaTableColumn left, SchemaTableColumn right)
        {
            return !left.Equals(right);
        }
    }
}