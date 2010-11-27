using System;
using System.IO;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable string that represents a file or directory path. This can be implicitly converted
    /// both to and from a normal string.
    /// </summary>
    public sealed class PathString : IEquatable<PathString>, IComparable<PathString>
    {
        static readonly string _altDirSep = Path.AltDirectorySeparatorChar.ToString();
        static readonly string _dirSep = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// The <see cref="StringComparer"/> used to compare paths.
        /// </summary>
        static readonly StringComparer _pathComparer = StringComparer.Ordinal;

        readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathString"/> class.
        /// </summary>
        /// <param name="path">Path of this PathString.</param>
        public PathString(string path)
        {
            if (path.EndsWith(_dirSep) || path.EndsWith(_altDirSep))
                path = path.Substring(0, path.Length - 1);

            _path = path;
        }

        /// <summary>
        /// Gets the parent path for this path.
        /// </summary>
        /// <returns>The parent path for this path, or the same path if already at the highest level path.</returns>
        public PathString Back()
        {
            var parent = Directory.GetParent(_path);

            if (parent == null)
                return this;

            return new PathString(parent.FullName);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>;
        /// otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is PathString)
                return Equals((PathString)obj);
            else if (obj is string)
                return _pathComparer.Equals(_path, (string)obj);

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            if (_path == null)
                return 0;

            return _pathComparer.GetHashCode(_path);
        }

        /// <summary>
        /// Joins this <see cref="PathString"/> with a sub-path.
        /// </summary>
        /// <param name="subPath">Name of the child file or folder from this PathString.</param>
        /// <returns>A new PathString containing the path joining this PathString with the <paramref name="subPath"/>.</returns>
        public PathString Join(string subPath)
        {
            // Remove prefixed directory separator char
            if (subPath.StartsWith(_dirSep) || subPath.StartsWith(_altDirSep))
                subPath = subPath.Substring(1);

            // Remove suffixed directory separator char
            if (subPath.EndsWith(_dirSep) || subPath.EndsWith(_altDirSep))
                subPath = subPath.Substring(0, subPath.Length - 1);

            // Create the new path
            var newPath = Path.Combine(this, subPath);

            // Check if the path is equal to this path. If it is, we can just return this object.
            if (_pathComparer.Equals(_path, newPath))
                return this;

            // Return the new path
            return new PathString(newPath);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _path;
        }

        #region IComparable<PathString> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(PathString other)
        {
            var otherPath = Equals(other, null) ? null : other._path;
            return _pathComparer.Compare(_path, otherPath);
        }

        #endregion

        #region IEquatable<PathString> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PathString other)
        {
            var otherPath = Equals(other, null) ? null : other._path;
            return _pathComparer.Equals(_path, otherPath);
        }

        #endregion

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PathString a, PathString b)
        {
            if (Equals(a, null))
                return Equals(a, b);

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PathString a, PathString b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PathString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(PathString value)
        {
            if (value == null)
                return null;
            else
                return value._path;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="PathString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PathString(string value)
        {
            return new PathString(value);
        }
    }
}