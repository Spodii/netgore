using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using log4net;

namespace NetGore.IO
{
    /// <summary>
    /// An immutable class that contains the paths for the game content.
    /// </summary>
    public class ContentPaths
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the file suffix (including the period, if one exists) for compiled content files.
        /// </summary>
        public const string ContentFileSuffix = "";

        /// <summary>
        /// Gets the file suffix (not including the period) for compiled content files.
        /// </summary>
        public const string ContentFileSuffixNoPeriod = "";

        /// <summary>
        /// The relative path to the Data directory from the Contents directory.
        /// </summary>
        public const string DataFolder = "Data";

        /// <summary>
        /// The relative path to the Engine directory from the Contents directory.
        /// </summary>
        public const string EngineFolder = "Engine";

        /// <summary>
        /// The relative path to the Fonts directory from the Contents directory.
        /// </summary>
        public const string FontsFolder = "Font";

        /// <summary>
        /// The relative path to the Fx directory from the Contents directory.
        /// </summary>
        public const string FxFolder = "Fx";

        /// <summary>
        /// The relative path to the Grhs directory from the Contents directory.
        /// </summary>
        public const string GrhsFolder = "Grh";

        /// <summary>
        /// The relative path to the Languages directory from the Contents directory.
        /// </summary>
        public const string LanguagesFolder = "Languages";

        /// <summary>
        /// The relative path to the Maps directory from the Contents directory.
        /// </summary>
        public const string MapsFolder = "Maps";

        /// <summary>
        /// The relative path to the Music directory from the Contents directory.
        /// </summary>
        public const string MusicFolder = "Music";

        /// <summary>
        /// The relative path to the Recycled directory from the Contents directory.
        /// </summary>
        public const string RecycledFolder = "Recycled";

        /// <summary>
        /// The relative path to the Settings directory from the Contents directory.
        /// </summary>
        public const string SettingsFolder = "Settings";

        /// <summary>
        /// The relative path to the Skeletons directory from the Contents directory.
        /// </summary>
        public const string SkeletonsFolder = "Skeletons";

        /// <summary>
        /// The relative path to the Sounds directory from the Contents directory.
        /// </summary>
        public const string SoundsFolder = "Sounds";

        /// <summary>
        /// Suffix for temporary files.
        /// </summary>
        const string _tempFileSuffix = ".ngtmp";

        /// <summary>
        /// The relative path to the Temp directory from the root directory.
        /// </summary>
        const string _tempFolder = "Temp";

        static readonly PathString _appRoot;
        static readonly ContentPaths _buildPaths;
        static readonly PathString _temp;

        static ContentPaths _devPaths;

        /// <summary>
        /// The current free file index.
        /// </summary>
        static volatile uint _freeFileIndex;

        static bool _isDevPathLoaded = false;

        readonly PathString _data;
        readonly PathString _engine;
        readonly PathString _fonts;
        readonly PathString _fx;
        readonly PathString _grhs;
        readonly PathString _languages;
        readonly PathString _maps;
        readonly PathString _music;
        readonly PathString _root;
        readonly PathString _settings;
        readonly PathString _skeletons;
        readonly PathString _sounds;

        /// <summary>
        /// Initializes the <see cref="ContentPaths"/> class.
        /// </summary>
        static ContentPaths()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _appRoot = Path.GetFullPath(baseDir);

            // Set the _freeFileIndex to a random initial value
            _freeFileIndex = RandomHelper.NextUInt();

            // Set the temp files path
            _temp = _appRoot.Join(_tempFolder);
            if (!Directory.Exists(_temp))
                Directory.CreateDirectory(_temp);

            _buildPaths = new ContentPaths(GetBuildContentPath(_appRoot));

            // Delete temp files
            DeleteTempFiles();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPaths"/> class.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        ContentPaths(string rootPath)
        {
            _root = Path.GetFullPath(rootPath);

            _data = GetChildPath(_root, DataFolder);
            _maps = GetChildPath(_root, MapsFolder);
            _music = GetChildPath(_root, MusicFolder);
            _sounds = GetChildPath(_root, SoundsFolder);
            _engine = GetChildPath(_root, EngineFolder);
            _fonts = GetChildPath(_root, FontsFolder);
            _grhs = GetChildPath(_root, GrhsFolder);
            _skeletons = GetChildPath(_root, SkeletonsFolder);
            _fx = GetChildPath(_root, FxFolder);
            _settings = GetChildPath(_root.Back(), SettingsFolder);
            _languages = GetChildPath(_root, LanguagesFolder);
        }

        /// <summary>
        /// Gets the <see cref="ContentPaths"/> for the Build content.
        /// </summary>
        public static ContentPaths Build
        {
            get { return _buildPaths; }
        }

        /// <summary>
        /// Gets the file path to the Data directory.
        /// </summary>
        public PathString Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Gets the <see cref="ContentPaths"/> for the Development content. Will return null if the path cannot be found.
        /// </summary>
        public static ContentPaths Dev
        {
            get
            {
                if (!_isDevPathLoaded)
                {
                    _isDevPathLoaded = true;
                    var devPath = GetDevContentPath();

                    if (string.IsNullOrEmpty(devPath))
                        _devPaths = null;
                    else
                        _devPaths = new ContentPaths(devPath);
                }

                return _devPaths;
            }
        }

        /// <summary>
        /// Gets the file path to the Engine directory.
        /// </summary>
        public PathString Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Gets the file path to the Fonts directory.
        /// </summary>
        public PathString Fonts
        {
            get { return _fonts; }
        }

        /// <summary>
        /// Gets the file path to the Fx (pixel shader effects) directory.
        /// </summary>
        public PathString Fx
        {
            get { return _fx; }
        }

        /// <summary>
        /// Gets the file path to the Grhs directory.
        /// </summary>
        public PathString Grhs
        {
            get { return _grhs; }
        }

        /// <summary>
        /// Gets the file path to the Languages directory.
        /// </summary>
        public PathString Languages
        {
            get { return _languages; }
        }

        /// <summary>
        /// Gets the file path to the Maps directory.
        /// </summary>
        public PathString Maps
        {
            get { return _maps; }
        }

        /// <summary>
        /// Gets the file path to the Music directory.
        /// </summary>
        public PathString Music
        {
            get { return _music; }
        }

        /// <summary>
        /// Gets the <see cref="PathString"/> to the recycled content folder. Will return null if the <see cref="ContentPaths.Dev"/>
        /// path cannot be found.
        /// </summary>
        public static PathString Recycled
        {
            get
            {
                var dev = Dev;
                if (dev == null)
                    return null;

                return dev.Root.Join(RecycledFolder);
            }
        }

        /// <summary>
        /// Gets the file path to the root Content directory.
        /// </summary>
        public PathString Root
        {
            get { return _root; }
        }

        /// <summary>
        /// Gets the file path to the Settings directory.
        /// </summary>
        public PathString Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the file path to the Skeletons directory.
        /// </summary>
        public PathString Skeletons
        {
            get { return _skeletons; }
        }

        /// <summary>
        /// Gets the file path to the Sounds directory.
        /// </summary>
        public PathString Sounds
        {
            get { return _sounds; }
        }

        /// <summary>
        /// Gets the file path to the Temp directory.
        /// </summary>
        public static PathString Temp
        {
            get { return _temp; }
        }

        /// <summary>
        /// Deletes all of the temporary files.
        /// </summary>
        static void DeleteTempFiles()
        {
            try
            {
                // Find all the temp files and free them
                // FreeTempFile() should suppress any minor exceptions
                var tempFiles = Directory.GetFiles(Temp, "*" + _tempFileSuffix, SearchOption.TopDirectoryOnly);
                foreach (var file in tempFiles)
                {
                    FreeTempFile(file);
                }
            }
            catch (DirectoryNotFoundException)
            {
                // Meh, whatever. Directory doesn't exist, so its not like we need to delete.
                // It really SHOULD exist, but something else will complain about that later.
            }
        }

        /// <summary>
        /// Frees a file path acquired by <see cref="GetTempFilePath"/>.
        /// </summary>
        /// <param name="filePath">The file path to free.</param>
        internal static void FreeTempFile(PathString filePath)
        {
            const string errmsg = "Temp file deletion failed: {0}";

            // Delete... or not, doesn't really matter
            // Its just a damn temporary file
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, ex);
            }
            catch (IOException ex)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, ex);
            }
        }

        /// <summary>
        /// Gets the full file path to the build content directory.
        /// </summary>
        /// <param name="rootPath">Root application path.</param>
        /// <returns>The full file path to the development content directory.</returns>
        static string GetBuildContentPath(string rootPath)
        {
            var childPath = "Content" + Path.DirectorySeparatorChar;
            return Path.Combine(rootPath, childPath);
        }

        /// <summary>
        /// Combines the <paramref name="root"/> and <paramref name="child"/> directory.
        /// </summary>
        /// <param name="root">The root (base) directory.</param>
        /// <param name="child">The child directory.</param>
        /// <returns>The <see cref="PathString"/> for the <paramref name="root"/> and <paramref name="child"/>
        /// directory concatenated.</returns>
        static PathString GetChildPath(string root, string child)
        {
            // Create the desired path
            var path = Path.Combine(root, child);

            // Ensure the directory exists
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>
        /// Gets the full file path to the development content directory.
        /// </summary>
        /// <returns>The full file path to the development content directory, or null if a valid
        /// path could not be found.</returns>
        static string GetDevContentPath()
        {
            // Get the devpath.txt file path
            var devPathFile = Build.Data.Join("devpath.txt");
            if (!File.Exists(devPathFile))
                return null;

            // Read all the lines in the file
            var lines = File.ReadAllLines(devPathFile);

            // Parse each of the lines
            foreach (var rawLine in lines)
            {
                // Skip empty lines
                if (string.IsNullOrEmpty(rawLine))
                    continue;

                // Trim down the line
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                // Try to get the full path, ignoring when it generates an exception
                string fullPath = null;
                try
                {
                    fullPath = Path.GetFullPath(line);

                    // Ensure the directory exists
                    if (!Directory.Exists(fullPath))
                        continue;
                }
                catch (ArgumentException)
                {
                }
                catch (SecurityException)
                {
                }
                catch (IOException)
                {
                }

                // If not null, a valid path has been found
                if (fullPath != null)
                    return fullPath;
            }

            return null;
        }

        /// <summary>
        /// Gets the file path to a free temporary file.
        /// </summary>
        /// <returns>The file path to a free temporary file.</returns>
        internal static PathString GetTempFilePath()
        {
            const string errmsg = "Temp file deletion failed: {0}";

            var index = ++_freeFileIndex;

            string filePath = _temp.Join(index + _tempFileSuffix);

            // Delete the file if it already exists (like if the temp files didn't clean out properly from the last
            // time the program ran). For some exceptions, recursively retry with the next temp file until
            // we get it right (or the stack overflows :] ).
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, ex);
                return GetTempFilePath();
            }
            catch (IOException ex)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, ex);
                return GetTempFilePath();
            }

            return filePath;
        }

        /// <summary>
        /// Runs the CopyContent program to copy content from the development directory to the build directory.
        /// </summary>
        /// <param name="dev">The path to the development directory. If null, <see cref="ContentPaths.Dev"/> will be used.</param>
        /// <param name="build">The path to the build directory. If null, <see cref="ContentPaths.Build"/> will be used.</param>
        /// <param name="copyContentFile">The path to the CopyContent.exe program file. If null, the file will be searched
        /// for in the <paramref name="dev"/> path.</param>
        /// <param name="userArgs">The additional arguments to pass to the CopyContent program..</param>
        /// <returns>
        /// True if the process was successfully run; otherwise false.
        /// </returns>
        public static bool TryCopyContent(ContentPaths dev = null, ContentPaths build = null, string copyContentFile = null,
                                          string userArgs = null)
        {
            try
            {
                // Use the default dev/build paths if not defined
                if (dev == null)
                    dev = Dev;

                if (build == null)
                    build = Build;

                if (dev == null || build == null)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("Failed to run CopyContent: Could not acquire the dev ContentPath.");
                    return false;
                }

                // Get the CopyContent binary file path
                if (string.IsNullOrEmpty(copyContentFile))
                    copyContentFile = dev.Root.Join("CopyContent.exe");

                // Ensure the file exists
                if (!File.Exists(copyContentFile))
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("Failed to run CopyContent: Could not find the CopyContent program at `{0}`",
                            copyContentFile);
                    return false;
                }

                // Try to run the program
                var arguments = string.Format("\"{0}\" \"{1}\"", dev.Root, build.Root);
                if (!string.IsNullOrEmpty(userArgs))
                    arguments += " " + userArgs;

                var pi = new ProcessStartInfo(copyContentFile, arguments)
                { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };

                try
                {
                    var proc = Process.Start(pi);
                    if (proc == null)
                    {
                        if (log.IsErrorEnabled)
                            log.ErrorFormat("Failed to run CopyContent process: Process.Start returned null.");
                        return false;
                    }

                    proc.WaitForExit(30000);
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("Failed to run CopyContent process: {0}", ex);

                    return false;
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "TryCopyContent() failed to unknown and unexpected exception. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }

            return true;
        }
    }
}