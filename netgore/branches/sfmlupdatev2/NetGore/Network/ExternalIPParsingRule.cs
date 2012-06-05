using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore.Network
{
    /// <summary>
    /// Contains a Url and <see cref="Regex"/> parser for acquiring the external IP from a remote web site.
    /// </summary>
    public struct ExternalIPParser : IEquatable<ExternalIPParser>
    {
        readonly Regex _parser;
        readonly Uri _uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalIPParser"/> struct.
        /// </summary>
        /// <param name="uri">The url.</param>
        /// <param name="parser">The parser.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="parser"/> is not null and does not contain a group
        /// named 'ip'.</exception>
        public ExternalIPParser(Uri uri, Regex parser)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (parser != null && !parser.GetGroupNames().Contains("ip"))
                throw new ArgumentException("The Regex must contain a group named 'ip'.", "parser");

            _uri = uri;
            _parser = parser;
        }

        /// <summary>
        /// Gets the <see cref="Regex"/> used to parse the data retreived from the
        /// <see cref="Uri"/>. Must contain a group named "ip", which will contain the results
        /// to display. Can be null.
        /// </summary>
        public Regex Parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> to get the information from.
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ExternalIPParser other)
        {
            return Equals(other._parser, _parser) && Equals(other._uri, _uri);
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
            return obj is ExternalIPParser && this == (ExternalIPParser)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_parser != null ? _parser.GetHashCode() : 0) * 397) ^ (_uri != null ? _uri.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ExternalIPParser left, ExternalIPParser right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ExternalIPParser left, ExternalIPParser right)
        {
            return !left.Equals(right);
        }
    }
}