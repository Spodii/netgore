using System.IO;
using System.Linq;
using System.Text;

namespace CodeReleasePreparer
{
    /// <summary>
    /// Manages the cleaning of the version control references.
    /// </summary>
    static class VersionControlCleaner
    {
        /// <summary>
        /// Regex of the .csproj file items to delete.
        /// </summary>
        static readonly RegexCollection _projRegexes =
            new RegexCollection(new string[]
            {
                @"<SccProjectName>.+?</SccProjectName>", @"<SccLocalPath>.+?</SccLocalPath>", @"<SccAuxPath>.+?</SccAuxPath>",
                @"<SccProvider>.+?</SccProvider>"
            });

        /// <summary>
        /// Regex of the .sln file items to delete.
        /// </summary>
        static readonly RegexCollection _slnRegexes =
            new RegexCollection(new string[] { @"GlobalSection\(SubversionScc\).+?EndGlobalSection" });

        /// <summary>
        /// Replaces text in a file.
        /// </summary>
        /// <param name="filePath">The path to the file to replace the text in.</param>
        /// <param name="regexes">The regexes to use to replace the contents.</param>
        static void DoReplace(string filePath, RegexCollection regexes)
        {
            var txt = File.ReadAllText(filePath, Encoding.UTF8);
            txt = regexes.ReplaceMatches(txt, string.Empty);
            File.WriteAllText(filePath, txt, Encoding.UTF8);
        }

        /// <summary>
        /// Runs the version control cleaning, cleaning up anything under the given root directory.
        /// </summary>
        /// <param name="rootDir">The root directory to clean from.</param>
        public static void Run(string rootDir)
        {
            var projFiles = Directory.GetFiles(rootDir, "*.csproj", SearchOption.AllDirectories);
            var slnFiles = Directory.GetFiles(rootDir, "*.sln", SearchOption.AllDirectories);

            foreach (var f in projFiles)
            {
                DoReplace(f, _projRegexes);
            }

            foreach (var f in slnFiles)
            {
                DoReplace(f, _slnRegexes);
            }
        }
    }
}