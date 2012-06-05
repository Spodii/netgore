using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeReleasePreparer
{
    /// <summary>
    /// A collection of <see cref="Regex"/>s.
    /// </summary>
    public class RegexCollection
    {
        /// <summary>
        /// The <see cref="RegexOptions"/> to use on all the <see cref="Regex"/>es in this collection.
        /// </summary>
        const RegexOptions _options =
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        readonly Regex[] _regexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexCollection"/> class.
        /// </summary>
        /// <param name="patterns">The <see cref="Regex"/> patterns.</param>
        public RegexCollection(IEnumerable<string> patterns)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            _regexes = patterns.Select(p => new Regex(p, _options)).ToArray();
        }

        /// <summary>
        /// Checks if a string matches any of the <see cref="Regex"/>es in this collection.
        /// </summary>
        /// <param name="s">The string to check a match for.</param>
        /// <returns>True if any of the <see cref="Regex"/>es in this collection match <paramref name="s"/>; otherwise false.</returns>
        public bool Matches(string s)
        {
            return _regexes.Any(x => x.IsMatch(s));
        }

        /// <summary>
        /// Replaces all matches with the given replacement string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <returns>The string with all the replacements made to it.</returns>
        public string ReplaceMatches(string input, string replacement)
        {
            var s = input;

            foreach (var r in _regexes)
            {
                s = r.Replace(s, replacement);
            }

            return s;
        }
    }
}