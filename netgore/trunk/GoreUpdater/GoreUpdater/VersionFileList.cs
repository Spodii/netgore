using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GoreUpdater
{
    /// <summary>
    /// Provides information about every file for a single update version, along with the deletion ignore filters.
    /// </summary>
    public class VersionFileList
    {
        /// <summary>
        /// The string used to delimit items in the listing.
        /// </summary>
        public const string Delimiter = "|";

        /// <summary>
        /// The header used to denote the start of a listing of file patterns to ignore when deleting files.
        /// </summary>
        public const string IgnoreHeader = "IGNORE";

        /// <summary>
        /// The header used to denote the start of a listing of files to include.
        /// </summary>
        public const string IncludeHeader = "INCLUDE";

        /// <summary>
        /// A string array for the <see cref="Delimiter"/>.
        /// </summary>
        static readonly string[] _delimiterArray = new string[] { Delimiter };

        static readonly StringComparer _pathComparer = StringComparer.Ordinal;

        readonly List<VersionFileInfo> _files;
        readonly Dictionary<string, VersionFileInfo> _filesDict;
        readonly List<string> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionFileList"/> class.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="filters">The filters.</param>
        VersionFileList(IEnumerable<VersionFileInfo> files, IEnumerable<string> filters)
        {
            // Store the files in a sorted form
            _files = new List<VersionFileInfo>(files.OrderBy(x => x.FilePath, _pathComparer));
            _filters = new List<string>(filters.Select(x => x.Trim()).Where(x => x.Length > 0).OrderBy(x => x, _pathComparer));

            // Add to the dictionary, too
            _filesDict = new Dictionary<string, VersionFileInfo>(_pathComparer);
            foreach (var file in _files)
            {
                _filesDict.Add(file.FilePath, file);
            }
        }

        /// <summary>
        /// Gets all of the files included in this version.
        /// </summary>
        public IEnumerable<VersionFileInfo> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Gets all of the file deletion ignore filters in this version.
        /// </summary>
        public IEnumerable<string> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Gets if the given relative file path is included in this version.
        /// </summary>
        /// <param name="relativeFile">The relative file path.</param>
        /// <returns>True if the <paramref name="relativeFile"/> is included in this version; otherwise false.</returns>
        public bool ContainsFile(string relativeFile)
        {
            // Make sure it doesn't start with the directory seperator character
            if (relativeFile.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                relativeFile.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                Debug.Fail("You shouldn't have the directory seperator character prefixed...");
                relativeFile = relativeFile.Substring(1);
            }

            return _filesDict.ContainsKey(relativeFile);
        }

        /// <summary>
        /// Creates a new <see cref="VersionFileList"/>.
        /// </summary>
        /// <param name="rootDir">The root directory containing the files to include.</param>
        /// <param name="filters">The filters to include.</param>
        /// <returns>The <see cref="VersionFileList"/> created from the given parameters.</returns>
        public static VersionFileList Create(string rootDir, IEnumerable<string> filters)
        {
            var addFiles = new List<VersionFileInfo>();

            var files = Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);

            // Get the length of the root directory so we know how much to chop off so that each file starts relative to
            // the rootDir, and does not start with a path separator
            var rootDirLen = rootDir.Length;
            if (!rootDir.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                !rootDir.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                rootDirLen++;

            foreach (var f in files)
            {
                // Get the file information
                var fi = new FileInfo(f);

                // Generate the properties we are interested in
                var relativePath = fi.FullName.Substring(rootDirLen);
                var hash = Hasher.GetFileHash(fi.FullName);
                var size = fi.Length;

                // Add to the list
                var vfi = new VersionFileInfo(relativePath, hash, size);
                addFiles.Add(vfi);
            }

            var vfl = new VersionFileList(addFiles, filters);
            return vfl;
        }

        /// <summary>
        /// Creates the <see cref="VersionFileList"/> from file.
        /// </summary>
        /// <param name="filePath">The path to the file containing the version file list.</param>
        /// <returns>The <see cref="VersionFileList"/> loaded from the <paramref name="filePath"/>.</returns>
        public static VersionFileList CreateFromFile(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            return CreateFromString(fileContents);
        }

        /// <summary>
        /// Creates the <see cref="VersionFileList"/> from a string.
        /// </summary>
        /// <param name="contents">The version file list contents.</param>
        /// <returns>The <see cref="VersionFileList"/> loaded from the <paramref name="contents"/>.</returns>
        /// <exception cref="InvalidDataException">Any of the lines are invalid.</exception>
        public static VersionFileList CreateFromString(string contents)
        {
            var lines = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Create the lists
            var files = new List<VersionFileInfo>();
            var filters = new List<string>();

            // Add to file list instead of the filters by default
            var addToFiles = true;

            var strComp = StringComparer.OrdinalIgnoreCase;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var l = line.Trim();

                // Check for a header
                if (strComp.Equals(IncludeHeader, l))
                {
                    addToFiles = true;
                    continue;
                }
                else if (strComp.Equals(IgnoreHeader, l))
                {
                    addToFiles = false;
                    continue;
                }

                // Not a header, so split according to the type
                if (addToFiles)
                {
                    var vfi = VersionFileInfo.FromString(l);
                    files.Add(vfi);
                }
                else
                {
                    // Expect a line for a filter
                    if (l.Contains(Delimiter))
                        throw new InvalidDataException("Was expecting a line containing a filter on line: " + line);

                    filters.Add(l);
                }
            }

            var vfl = new VersionFileList(files, filters);
            return vfl;
        }

        /// <summary>
        /// Attempts to get the <see cref="VersionFileInfo"/> for a file.
        /// </summary>
        /// <param name="relativeFile">The relative file to get the <see cref="VersionFileInfo"/> for.</param>
        /// <param name="fileInfo">When this method returns true, contains the <see cref="VersionFileInfo"/> for the
        /// <paramref name="relativeFile"/>.</param>
        /// <returns>True if the <paramref name="relativeFile"/> exists in this version; otherwise false.</returns>
        public bool TryGetFileInfo(string relativeFile, out VersionFileInfo fileInfo)
        {
            // Make sure it doesn't start with the directory seperator character
            if (relativeFile.StartsWith(Path.DirectorySeparatorChar.ToString()) ||
                relativeFile.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                Debug.Fail("You shouldn't have the directory seperator character prefixed...");
                relativeFile = relativeFile.Substring(1);
            }

            return _filesDict.TryGetValue(relativeFile, out fileInfo);
        }

        /// <summary>
        /// Writes the <see cref="VersionFileList"/> to file.
        /// </summary>
        /// <param name="outputFile">The output file.</param>
        public void Write(string outputFile)
        {
            // Create the destination directory, if needed
            try
            {
                var p = Path.GetDirectoryName(outputFile);
                if (p != null)
                {
                    if (!Directory.Exists(p))
                        Directory.CreateDirectory(p);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }

            var sb = new StringBuilder();

            // Add the files
            sb.AppendLine(IncludeHeader);
            foreach (var f in _files)
            {
                var creationString = f.GetCreationString();
                sb.AppendLine(creationString);
            }

            // Add the filters
            sb.AppendLine(IgnoreHeader);
            foreach (var f in _filters)
            {
                sb.AppendLine(f);
            }

            File.WriteAllText(outputFile, sb.ToString());
        }

        /// <summary>
        /// Contains the information for a single file in a <see cref="VersionFileList"/>.
        /// </summary>
        public class VersionFileInfo
        {
            readonly string _filePath;
            readonly string _hash;
            readonly long _size;

            /// <summary>
            /// Initializes a new instance of the <see cref="VersionFileInfo"/> struct.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <param name="hash">The file hash.</param>
            /// <param name="size">The file size.</param>
            public VersionFileInfo(string filePath, string hash, long size)
            {
                _filePath = filePath;
                _hash = hash;
                _size = size;
            }

            /// <summary>
            /// Gets the relative path to the file.
            /// </summary>
            public string FilePath
            {
                get { return _filePath; }
            }

            /// <summary>
            /// Gets the hash of the file.
            /// </summary>
            public string Hash
            {
                get { return _hash; }
            }

            /// <summary>
            /// Gets the size of the file in bytes.
            /// </summary>
            public long Size
            {
                get { return _size; }
            }

            /// <summary>
            /// Creates a <see cref="VersionFileInfo"/> from a string.
            /// </summary>
            /// <param name="line">The string.</param>
            /// <returns>The <see cref="VersionFileInfo"/>.</returns>
            /// <exception cref="InvalidDataException">The <paramref name="line"/> is not in the expected format.</exception>
            public static VersionFileInfo FromString(string line)
            {
                // Expect a line for a file to add
                var split = line.Split(_delimiterArray, StringSplitOptions.None);

                if (split.Length != 3)
                {
                    if (line.Contains(Delimiter))
                        throw new InvalidDataException("Invalid number of columns for include file on line: " + line);
                }

                var filePath = split[0];
                var hash = split[1];
                long size;
                if (!long.TryParse(split[2], out size))
                    throw new InvalidDataException("Invalid size parameter for include file on line: " + line);

                return new VersionFileInfo(filePath, hash, size);
            }

            /// <summary>
            /// Gets a string that can be used to create this <see cref="VersionFileList"/> by calling
            /// <see cref="VersionFileList.VersionFileInfo.FromString"/>.
            /// </summary>
            /// <returns>The creation string.</returns>
            public string GetCreationString()
            {
                var ret = FilePath + Delimiter + Hash + Delimiter + Size;
                return ret;
            }
        }
    }
}