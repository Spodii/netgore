using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeReleasePreparer
{
    public class RegexCollection
    {
        const RegexOptions _options =
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        readonly Regex[] _regexes;

        public RegexCollection(string[] patterns)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            _regexes = new Regex[patterns.Length];

            for (int i = 0; i < patterns.Length; i++)
            {
                _regexes[i] = new Regex(patterns[i], _options);
            }
        }

        public bool Matches(string s)
        {
            return _regexes.Any(x => x.IsMatch(s));
        }
    }
}