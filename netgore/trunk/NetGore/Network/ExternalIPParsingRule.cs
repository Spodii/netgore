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
        readonly string _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalIPParser"/> struct.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="parser">The parser.</param>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="parser"/> is not null and does not contain a group
        /// named 'ip'.</exception>
        public ExternalIPParser(string url, Regex parser)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            if (parser != null && !parser.GetGroupNames().Contains("ip"))
                throw new ArgumentException("The Regex must contain a group named 'ip'.", "parser");

            _url = url;
            _parser = parser;
        }

        /// <summary>
        /// Gets the <see cref="Regex"/> used to parse the data retreived from the
        /// <see cref="ExternalIPParser.Url"/>. Must contain a group named "ip", which will contain the results
        /// to display. Can be null.
        /// </summary>
        public Regex Parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// Gets the Url to get the information from.
        /// </summary>
        public string Url
        {
            get { return _url; }
        }
    }
}