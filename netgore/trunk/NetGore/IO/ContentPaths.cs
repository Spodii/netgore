using System;
using System.Collections.Generic;
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
        /// The suffix given to compiled content files. Does not include the prefixed period.
        /// </summary>
        public const string CompiledContentSuffix = "xnb";

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
        /// The relative path to the Settings directory from the Contents directory.
        /// </summary>
        public const string SettingsFolder = "Settings";

        /// <summary>
        /// The relative path to the Skeletons directory from the Contents directory.
        /// </summary>
        public const string SkeletonsFolder = "Skeletons";

        /// <summary>
        /// Suffix for temporary files.
        /// </summary>
        const string _tempFileSuffix = ".ngtmp";

        /// <summary>
        /// The relative path to the Temp directory from the root directory.
        /// </summary>
        const string _tempFolder = "Temp";

        /// <summary>
        /// The relative path to the Particle Effects directory from the Contents directory.
        /// </summary>
        public static readonly string ParticleEffectsFolder = DataFolder + Path.DirectorySeparatorChar + "ParticleEffects";

        static readonly PathString _appRoot;
        static readonly ContentPaths _buildPaths;
        static readonly PathString _temp;

        static ContentPaths _devPaths;

        /// <summary>
        /// The current free file index.
        /// </summary>
        static volatile uint _freeFileIndex;

        readonly PathString _data;
        readonly PathString _engine;
        readonly PathString _fonts;
        readonly PathString _fx;
        readonly PathString _grhs;
        readonly PathString _languages;
        readonly PathString _maps;
        readonly PathString _particleEffects;
        readonly PathString _root;
        readonly PathString _settings;
        readonly PathString _skeletons;

        /// <summary>
        /// Initializes the <see cref="ContentPaths"/> class.
        /// </summary>
        static ContentPaths()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _appRoot = Path.GetFullPath(baseDir);

            // Set the _freeFileIndex to a random initial value
            _freeFileIndex = (uint)new Random().Next();

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
            _particleEffects = GetChildPath(_root, ParticleEffectsFolder);
            _maps = GetChildPath(_root, MapsFolder);
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
        /// Gets the <see cref="ContentPaths"/> for the Development content.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">The <see cref="ContentPaths"/> for the Development
        /// content could not be found.</exception>
        public static ContentPaths Dev
        {
            get
            {
                if (_devPaths == null)
                {
                    var devPath = GetDevContentPath();
                    if (devPath == null)
                    {
                        throw new DirectoryNotFoundException(@"Could not find the directory to the development content path (ContentPaths.Dev)." +
                            @" Please make sure that the path to this directory is defined in the \Content\Data\devpath.txt file in the build directory.");
                    }

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
        /// Gets the file path to the ParticleEffects directory.
        /// </summary>
        public PathString ParticleEffects
        {
            get { return _particleEffects; }
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
            string childPath = "Content" + Path.DirectorySeparatorChar;
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
            string path = Path.Combine(root, child);

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

            uint index = ++_freeFileIndex;

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
    }
}