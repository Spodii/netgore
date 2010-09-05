using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Helper methods for the file filters.
    /// </summary>
    public static class FileFilterHelper
    {
        /// <summary>
        /// Creates an <see cref="IFileFilterCollection"/> from a collection of filter strings.
        /// </summary>
        /// <param name="filterStrings">The filter strings.</param>
        /// <returns>The <see cref="IFileFilterCollection"/> created from the <paramref name="filterStrings"/>.</returns>
        public static IFileFilterCollection CreateCollection(IEnumerable<string> filterStrings)
        {
            var filters = filterStrings.Where(x => !string.IsNullOrEmpty(x)).Select(CreateFilter);
            return new FileFilterCollection(filters);
        }

        /// <summary>
        /// Creates an <see cref="IFileFilter"/> from a filter string.
        /// </summary>
        /// <param name="filterString">The filter string.</param>
        /// <returns>The <see cref="IFileFilter"/> instance for the <paramref name="filterString"/>.</returns>
        public static IFileFilter CreateFilter(string filterString)
        {
            if (filterString.StartsWith("/"))
            {
                // Regex
                filterString = filterString.Substring(1);
                return new RegexFileFilter(filterString);
            }
            else
            {
                // Wildcard
                return new WildcardFileFilter(filterString);
            }
        }

        /// <summary>
        /// Sanitizes a file path and prepares it for filtering.
        /// </summary>
        /// <param name="filePath">The file path to sanitize.</param>
        /// <returns>The sanitized <paramref name="filePath"/>.</returns>
        public static string SanitizeFilePath(string filePath)
        {
            // Always start with the path separator
            if (!filePath.StartsWith("\\"))
                filePath = "\\" + filePath;

            // Use the proper path separator
            if (Path.DirectorySeparatorChar != '\\')
                filePath.Replace('\\', Path.DirectorySeparatorChar);

            return filePath;
        }
    }
}