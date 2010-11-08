using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetGore.Network
{
    /// <summary>
    /// Contains a Url and <see cref="Regex"/> parser for acquiring the external IP from a remote web site.
    /// </summary>
    public struct ExternalIPParser
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
    }
}