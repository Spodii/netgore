using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// An <see cref="IFileFilter"/> for wildcard matching.
    /// </summary>
    public class WildcardFileFilter : IFileFilter
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

            if (log.IsDebugEnabled)
                log.DebugFormat("Creating WildcardFileFilter. Input pattern: {0} Output Regex pattern: {1}", pattern, s);

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
            if (log.IsDebugEnabled)
                log.DebugFormat("Performing IsMatch on `{0}` for filePath `{1}`. Note: Will end up calling RegexFileFilter.", this,
                                filePath);

            return _filter.IsMatch(filePath);
        }

        #endregion
    }
}