using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DoxyPacker
{
    public class Packer
    {
        /// <summary>
        /// These file extensions will be auto-deleted.
        /// </summary>
        static readonly IEnumerable<string> _junkExtensions = new string[] { ".md5", ".map" };

        readonly string _rootDir;

        public Packer(string rootDir)
        {
            if (string.IsNullOrEmpty(rootDir))
                throw new ArgumentNullException("rootDir");

            _rootDir = rootDir;
        }

        public string RootDir
        {
            get { return _rootDir; }
        }

        static string GetFileName(string name, FileType type)
        {
            var sb = new StringBuilder(256);
            sb = sb.Append(GetTypePrefix(type));
            sb = sb.Append(name);
            sb = sb.Replace("\\", "-");
            sb = sb.Replace("/", "-");
            sb = sb.Replace(":", string.Empty);
            sb = sb.Append(".html");

            if (type != FileType.File && type != FileType.Directory && type != FileType.SourceFile)
            {
                sb = sb.Replace(" ", string.Empty);
                sb = sb.Replace("-g&lt;", "(");
                sb = sb.Replace("&lt;", "(");
                sb = sb.Replace("&gt;", ")");
            }

            if (type == FileType.Directory)
            {
                if (sb[sb.Length - 1 - ".html".Length] == '-')
                {
                    sb.Length -= 1 + ".html".Length;
                    sb.Append(".html");
                }
            }

            return sb.ToString();
        }

        static readonly Regex _titleGrabber = new Regex(@"\<title\>.*?: (?<name>.+?) (?<type>Class Template|Interface|Class|File|Directory) Reference\</title\>",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        static readonly Regex _titleGrabber2 = new Regex(@"\<title\>.*?: (?<type>Package) (?<name>.+)\</title\>",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        static FileType GetFileType(string html, out string name)
        {
            const int maxTitleSearchLen = 1024;

            var m = _titleGrabber.Match(html, 0, maxTitleSearchLen);

            if (!m.Success)
                m = _titleGrabber2.Match(html, 0, maxTitleSearchLen);

            if (!m.Success)
            {
                name = null;
                return FileType.Unknown;
            }

            name = m.Groups["name"].Value;
            var typeStr = m.Groups["type"].Value;

            if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "Class"))
                return FileType.Class;
            else if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "Interface"))
                return FileType.Interface;
            else if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "Package"))
                return FileType.Package;
            else if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "File"))
                return FileType.File;
            else if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "Directory"))
                return FileType.Directory;
            else if (StringComparer.OrdinalIgnoreCase.Equals(typeStr, "Class Template"))
                return FileType.ClassTemplate;
            else
            {
                Debug.Fail(typeStr);
                return FileType.Unknown;
            }
        }

        static string GetTypePrefix(FileType type)
        {
            switch (type)
            {
                case FileType.Class:
                    return "c";
                case FileType.ClassTemplate:
                    return "t";
                case FileType.Directory:
                    return "d";
                case FileType.File:
                    return "f";
                case FileType.SourceFile:
                    return "s";
                case FileType.Interface:
                    return "i";
                case FileType.Package:
                    return "p";
                default:
                    throw new Exception(type.ToString());
            }
        }

        static void Log(string s, params object[] p)
        {
            if (p != null && p.Length > 0)
                Console.WriteLine(s, p);
            else
                Console.WriteLine(s);
        }

        /// <summary>
        /// The first pass: Process each file, grabbing the type, names, and IDs, along with compacting the HTML.
        /// </summary>
        /// <returns>The changed files names, with the key as the old name and value as the new name.</returns>
        Dictionary<string, string> Pass1()
        {
            Log("***** PASS 1 *****");

            var changedFileNames = new Dictionary<string, string>();
            var htmlComp = new HtmlCompacter(RootDir);

            var files = Directory.GetFiles(RootDir, "*.html", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                // Skip source files for now
                if (f.EndsWith("_source.html"))
                    continue;

                // Read contents
                var html = File.ReadAllText(f);

                // Figure out what type of file this is
                string name;
                var type = GetFileType(html, out name);

                // Compact the HTML and re-save it - using the ID for the name if needed
                html = htmlComp.Compact(html);

                if (type != FileType.Unknown)
                {
                    var dir = Path.GetDirectoryName(f);
                    if (dir == null)
                        throw new Exception();

                    var oldName = Path.GetFileName(f);
                    if (oldName == null)
                        throw new Exception();

                    var newName = GetFileName(name, type);
                    var newPath = Path.Combine(dir, newName);

                    File.WriteAllText(newPath, html);
                    File.Delete(f);

                    changedFileNames.Add(oldName, newName);

                    Log("Moved: {0} -> {1}", oldName, newName);

                    // For file reference files, also change the source file
                    if (type == FileType.File)
                    {
                        var srcFileOldName = oldName.Substring(0, oldName.Length - ".html".Length) + "_source.html";
                        var srcFileNewName = GetTypePrefix(FileType.SourceFile) +
                                             newName.Substring(1, newName.Length - ".html".Length - 1) + ".html";
                        File.Move(Path.Combine(dir, srcFileOldName), Path.Combine(dir, srcFileNewName));
                        changedFileNames.Add(srcFileOldName, srcFileNewName);
                        Log("Moved: {0} -> {1}", srcFileOldName, srcFileNewName);
                    }
                }
                else
                {
                    // No file renaming
                    File.WriteAllText(f, html);
                }
            }

            return changedFileNames;
        }

        /// <summary>
        /// The second pass: Update links inside all files.
        /// </summary>
        /// <param name="changedFileNames">The changed files names, with the key as the old name and value as the new name.</param>
        void Pass2(Dictionary<string, string> changedFileNames)
        {
            var hrefFinder = new Regex(" href=\"(?<filename>\\w+?\\.\\w+?)[#\"]",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var files = Directory.GetFiles(RootDir, "*.html", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                Log("Updating links in: {0}", f);

                // Read contents
                var html = File.ReadAllText(f);

                // Find all file names referenced in the HTML
                var allRefFiles = hrefFinder.Matches(html).OfType<Match>().Select(x => x.Groups["filename"].Value).Distinct();

                // Narrow down to referenced files that have had their names changed
                var changedRefFiles = allRefFiles.Where(changedFileNames.ContainsKey).ToArray();

                // Perform a replace call for every file name change
                var newHtml = new StringBuilder(html);
                foreach (var oldName in changedRefFiles)
                {
                    var newName = changedFileNames[oldName];
                    newHtml = newHtml.Replace(" href=\"" + oldName, " href=\"" + newName);
                }

                // Save file changes
                File.WriteAllText(f, newHtml.ToString());
            }
        }

        /// <summary>
        /// The third pass: Delete junk files (*.map, *.md5).
        /// </summary>
        void Pass3()
        {
            var files = Directory.GetFiles(RootDir, "*", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var ext = Path.GetExtension(f);
                if (StringComparer.OrdinalIgnoreCase.Equals(".html", ext))
                    continue;

                if (_junkExtensions.Any(x => StringComparer.OrdinalIgnoreCase.Equals(ext, x)))
                {
                    File.Delete(f);
                    Log("Deleted junk: {0}", f);
                }
            }
        }

        public void Run()
        {
            var changedFileNames = Pass1();
            Pass2(changedFileNames);
            Pass3();
        }
    }
}