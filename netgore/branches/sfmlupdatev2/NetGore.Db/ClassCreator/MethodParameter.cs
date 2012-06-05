using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes a parameter on a method.
    /// </summary>
    public struct MethodParameter : IEquatable<MethodParameter>
    {
        static readonly MethodParameter[] _empty = new MethodParameter[0];

        readonly string _name;
        readonly string _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="formatter">The formatter.</param>
        public MethodParameter(string name, Type type, CodeFormatter formatter)
        {
            _name = name;
            _type = formatter.GetTypeString(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodParameter"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public MethodParameter(string name, string type)
        {
            _name = name;
            _type = type;
        }

        /// <summary>
        /// Gets an empty collection of <see cref="MethodParameter"/>s.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static MethodParameter[] Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets the parameter type.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MethodParameter other)
        {
            return Equals(other._name, _name) && Equals(other._type, _type);
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
            return obj is MethodParameter && this == (MethodParameter)obj;
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
                return ((_name != null ? _name.GetHashCode() : 0) * 397) ^ (_type != null ? _type.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MethodParameter left, MethodParameter right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MethodParameter left, MethodParameter right)
        {
            return !left.Equals(right);
        }
    }
}