using System.Text.RegularExpressions;

namespace GoreUpdater
{
    /// <summary>
    /// An <see cref="IFileFilter"/> for wildcard matching.
    /// </summary>
    public class WildcardFileFilter : IFileFilter
    {
        readonly RegexFileFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexFileFilter"/> class.
        /// </summary>
        /// <param name="pattern">The wildcard pattern.</param>
        public WildcardFileFilter(string pattern)
        {
            // Since there is no native support for globbing in C#, we will just build it using Regex
            var s = Regex.Escape(pattern);
            s = s.Replace(@"\*", ".*");
            s = s.Replace(@"\?", ".?");
            s = "^" + s + "$";

            _filter = new RegexFileFilter(s);
        }

        #region Implementation of IFileFilter

        /// <summary>
        /// Checks the given file path against the filter.
        /// </summary>
        /// <param name="filePath">The file path to test.</param>
        /// <returns>True if the filter matches the <paramref name="filePath"/>; otherwise false.</returns>
        public bool IsMatch(string filePath)
        {
            return _filter.IsMatch(filePath);
        }

        #endregion
    }
}