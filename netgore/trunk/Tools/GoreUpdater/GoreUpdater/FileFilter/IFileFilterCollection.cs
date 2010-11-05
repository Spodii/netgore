using System.Collections.Generic;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Interface for a collection of <see cref="IFileFilter"/>s.
    /// </summary>
    public interface IFileFilterCollection : ICollection<IFileFilter>
    {
        /// <summary>
        /// Checks if any of the filters in this collection match the given file path.
        /// </summary>
        /// <param name="filePath">The file path to test.</param>
        /// <returns>True if any of the filters match the <paramref name="filePath"/>; otherwise false.</returns>
        bool IsMatch(string filePath);
    }
}