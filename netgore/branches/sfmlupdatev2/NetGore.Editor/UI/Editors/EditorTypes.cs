using System;
using System.Linq;

namespace NetGore.Editor.UI
{
    /// <summary>
    /// A <see cref="Type"/> pair for a type and the type of editor.
    /// </summary>
    public struct EditorTypes : IEquatable<EditorTypes>
    {
        readonly Type _editorType;
        readonly Type _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTypes"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="editorType">Type of the editor.</param>
        public EditorTypes(Type type, Type editorType)
        {
            _type = type;
            _editorType = editorType;
        }

        /// <summary>
        /// Gets the type of the editor.
        /// </summary>
        public Type EditorType
        {
            get { return _editorType; }
        }

        /// <summary>
        /// Gets the type to add to.
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(EditorTypes other)
        {
            return Equals(other._editorType, _editorType) && Equals(other._type, _type);
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
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(EditorTypes))
                return false;
            return Equals((EditorTypes)obj);
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
                return ((_editorType != null ? _editorType.GetHashCode() : 0) * 397) ^ (_type != null ? _type.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(EditorTypes left, EditorTypes right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(EditorTypes left, EditorTypes right)
        {
            return !left.Equals(right);
        }
    }
}