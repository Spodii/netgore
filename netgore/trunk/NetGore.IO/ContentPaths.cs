using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// An immutable class that contains the paths for the game content.
    /// </summary>
    public class ContentPaths
    {
        const string _dataFolder = "Data";
        const string _engineFolder = "Engine";
        const string _fontsFolder = "Font";
        const string _grhsFolder = "Grh";
        const string _languagesFolder = "Languages";
        const string _mapsFolder = "Maps";
        const string _settingsFolder = "Settings";
        const string _skeletonsFolder = "Skeletons";
        const string _texturesFolder = "Texture";

        static readonly string _appRoot;
        static readonly ContentPaths _buildPaths;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static ContentPaths _devPaths;

        readonly PathString _data;
        readonly PathString _engine;
        readonly PathString _fonts;
        readonly PathString _grhs;
        readonly PathString _languages;
        readonly PathString _maps;
        readonly PathString _root;
        readonly PathString _settings;
        readonly PathString _skeletons;
        readonly PathString _textures;

        /// <summary>
        /// Gets the ContentPaths for the Build content.
        /// </summary>
        public static ContentPaths Build
        {
            get { return _buildPaths; }
        }

        /// <summary>
        /// Gets the ContentPaths for the Development content.
        /// </summary>
        public static ContentPaths Dev
        {
            get
            {
                if (_devPaths == null)
                    _devPaths = new ContentPaths(GetDevContentPath(_appRoot));
                return _devPaths;
            }
        }

        /// <summary>
        /// Gets the file path to the Data directory.
        /// </summary>
        public PathString Data
        {
            get { return _data; }
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
        /// Gets the file path to the Textures directory.
        /// </summary>
        public PathString Textures
        {
            get { return _textures; }
        }

        /// <summary>
        /// Initializes the <see cref="ContentPaths"/> class.
        /// </summary>
        static ContentPaths()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _appRoot = Path.GetFullPath(baseDir);

            _buildPaths = new ContentPaths(GetBuildContentPath(_appRoot));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPaths"/> class.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        public ContentPaths(string rootPath)
        {
            _root = Path.GetFullPath(rootPath);

            _data = GetChildPath(_root, _dataFolder);
            _maps = GetChildPath(_root, _mapsFolder);
            _engine = GetChildPath(_root, _engineFolder);
            _fonts = GetChildPath(_root, _fontsFolder);
            _grhs = GetChildPath(_root, _grhsFolder);
            _skeletons = GetChildPath(_root, _skeletonsFolder);
            _textures = GetChildPath(_root, _texturesFolder);
            _settings = GetChildPath(_root, _settingsFolder);
            _languages = GetChildPath(_root, _languagesFolder);
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
        /// <param name="rootPath">Root application path.</param>
        /// <returns>The full file path to the development content directory.</returns>
        static string GetDevContentPath(string rootPath)
        {
            // Check if this directory contains the content directory
            var subDirectories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
            foreach (string subDirectory in subDirectories)
            {
                if (IsDevContentDirectory(subDirectory))
                    return subDirectory;
            }

            // Get the parent directory
            DirectoryInfo parent;
            try
            {
                parent = Directory.GetParent(rootPath);
            }
            catch (Exception)
            {
                parent = null;
            }

            if (parent == null)
            {
                const string errmsg = "Failed to locate the development content directory.";
                Debug.Fail(errmsg);
                log.Fatal(errmsg);
                throw new DirectoryNotFoundException();
            }

            // Recursively crawl down the tree looking for the development content directory
            return GetDevContentPath(parent.FullName);
        }

        /// <summary>
        /// Checks if the given <paramref name="path"/> is the development content directory.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if the development content directory, else false.</returns>
        static bool IsDevContentDirectory(string path)
        {
            // Assume the dev content directory will be the one with a *.contentproj file
            // and contain the Data and Grh folders

            // Check for the *.contentproj file
            var files = Directory.GetFiles(path, "*.contentproj", SearchOption.TopDirectoryOnly);
            if (files.Count() == 0)
                return false;

            // Check for the required child directories
            var reqFolders = new List<string> { _dataFolder, _grhsFolder };
            var req = reqFolders.Select(d => Path.GetFileNameWithoutExtension(d).ToLower());

            var dirs = Directory.GetDirectories(path);
            var dirNames = dirs.Select(d => Path.GetFileNameWithoutExtension(d).ToLower());

            if (dirNames.Intersect(req).Count() != req.Count())
                return false;

            // All conditions met
            return true;
        }
    }
}