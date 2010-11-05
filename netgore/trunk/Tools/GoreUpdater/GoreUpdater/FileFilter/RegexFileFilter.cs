using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// An <see cref="IFileFilter"/> for Regex matching.
    /// </summary>
    public class RegexFileFilter : IFileFilter
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexFileFilter"/> class.
        /// </summary>
        /// <param name="regexPattern">The Regex pattern.</param>
        public RegexFileFilter(string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
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
                log.DebugFormat("Performing IsMatch on `{0}` for filePath `{1}`.", this, filePath);

            var s = FileFilterHelper.SanitizeFilePath(filePath);
            return _regex.IsMatch(s);
        }

        #endregion
    }
}