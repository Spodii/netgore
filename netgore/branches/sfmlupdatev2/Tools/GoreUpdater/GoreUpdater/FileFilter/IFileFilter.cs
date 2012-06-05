using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for a file filter.
    /// </summary>
    public interface IFileFilter
    {
        /// <summary>
        /// Checks the given file path against the filter.
        /// </summary>
        /// <param name="filePath">The file path to test.</param>
        /// <returns>True if the filter matches the <paramref name="filePath"/>; otherwise false.</returns>
        bool IsMatch(string filePath);
    }
}