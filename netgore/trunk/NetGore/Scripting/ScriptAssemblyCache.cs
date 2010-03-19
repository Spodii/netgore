using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using log4net;
using NetGore.IO;

namespace NetGore.Scripting
{
    /// <summary>
    /// Contains the caches of assemblies that have been compiled by source code so they don't have to be
    /// recompiled again as long as the cached version exists.
    /// </summary>
    public class ScriptAssemblyCache
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How often to check if the assembly cache needs to be saved and to check if items need to be removed since
        /// the cache has filled up.
        /// </summary>
        const int _autoSaveRate = 10 * 1000;

        const int _cacheCleanupAmount = 128;

        /// <summary>
        /// The file name suffix for cache files. The period delimiter is not included.
        /// </summary>
        const string _cacheFileSuffix = "asm";

        const string _cacheItemNodeName = "CacheItem";
        const int _maxCacheItems = 1024;
        const string _rootNodeName = "CacheItems";
        static readonly ScriptAssemblyCache _instance;

        readonly Dictionary<byte[], CacheItem> _cache = new Dictionary<byte[], CacheItem>(ByteArrayEqualityComparer.Instance);
        readonly string _cacheDir;
        readonly object _cacheSync = new object();
        readonly Timer _saveAndCleanTimer;

        bool _isDirty = false;
        bool _isOverflowing = false;

        /// <summary>
        /// Initializes the <see cref="ScriptAssemblyCache"/> class.
        /// </summary>
        static ScriptAssemblyCache()
        {
            _instance = new ScriptAssemblyCache();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAssemblyCache"/> class.
        /// </summary>
        ScriptAssemblyCache()
        {
            // Get the cache directory
            _cacheDir = ContentPaths.Build.Data.Join("ScriptCache");
            if (!_cacheDir.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                !_cacheDir.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                _cacheDir += Path.DirectorySeparatorChar;

            // Ensure the directory exists
            if (!Directory.Exists(_cacheDir))
                Directory.CreateDirectory(_cacheDir);

            // Load the cache
            Load();

            // Create the saving timer
            _saveAndCleanTimer = new Timer { Interval = _autoSaveRate, AutoReset = true };
            _saveAndCleanTimer.Elapsed += _saveTimer_Elapsed;
            _saveAndCleanTimer.Start();

            // Spawn a new background timer to clean up the junk files a few seconds from now
            Timer cleanTimer = new Timer { Interval = 5 * 1000, AutoReset = false };
            cleanTimer.Elapsed += delegate
            {
                cleanTimer.Dispose();
                RemoveUnusedCacheFiles();
            };
            cleanTimer.Start();
        }

        /// <summary>
        /// Gets the file path to the cache data file.
        /// </summary>
        protected string CacheDataFilePath
        {
            get { return CacheDirectory + "data.xml"; }
        }

        /// <summary>
        /// Gets the path to the cache directory.
        /// </summary>
        public string CacheDirectory
        {
            get { return _cacheDir; }
        }

        /// <summary>
        /// Gets the <see cref="ScriptAssemblyCache"/> instance.
        /// </summary>
        public static ScriptAssemblyCache Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Handles the Elapsed event of the _saveTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        void _saveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isOverflowing)
                TrimCache();

            if (_isDirty)
                Save();
        }

        /// <summary>
        /// Calculates the hash from the source code.
        /// </summary>
        /// <param name="code">The source code.</param>
        /// <returns>The byte array representation of the hash.</returns>
        protected static byte[] CalculateHash(string code)
        {
            MD5 hasher = MD5.Create();
            return hasher.ComputeHash(Encoding.ASCII.GetBytes(code));
        }

        /// <summary>
        /// Creates an <see cref="Assembly"/> inside of the cache. This will only actually generate the assembly
        /// if a cached version of the source code could not be found.
        /// </summary>
        /// <param name="code">The source code to compile.</param>
        /// <param name="assemblyCreator">A <see cref="Func{T1,T2,TResult}"/> describing how to compile the <see cref="Assembly"/>.
        /// The first argument is the source code to compile, and the second argument is the file path to give
        /// the generated <see cref="Assembly"/>.</param>
        /// <returns>The <see cref="Assembly"/>, or null if the <see cref="Assembly"/> failed to compile or load.</returns>
        public Assembly CreateInCache(string code, Func<string, string, Assembly> assemblyCreator)
        {
            CacheItem item;

            var hash = CalculateHash(code);
            bool createdNew = false;

            // Get the cache item, or adding it if it does not already exist
            lock (_cacheSync)
            {
                if (!_cache.TryGetValue(hash, out item))
                {
                    item = new CacheItem(hash, GetFreeFileName());
                    _cache.Add(hash, item);

                    createdNew = true;

                    _isDirty = true;
                    _isOverflowing = _cache.Count > _maxCacheItems;
                }
            }

            // Get the file path for the cache item
            string filePath = GetCacheFilePath(item.FileName);

            // Get the assembly
            Assembly asm = null;
            if (createdNew)
            {
                // If we had to create the file, we will have to generate the assembly
                try
                {
                    asm = assemblyCreator(code, filePath);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to generate assembly: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));
                }
            }
            else
            {
                // Otherwise, we just load the assembly from the file
                try
                {
                    asm = LoadAssembly(filePath);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to load assembly from file: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));
                }
            }

            // If the assembly failed to load, we need to remove it from the cache
            if (asm == null)
            {
                bool wasRemoved = false;

                lock (_cacheSync)
                {
                    // Remove the item from the cache after we make sure that the item hasn't changed
                    // since we last looked at it
                    CacheItem freshCacheItem;
                    if (_cache.TryGetValue(hash, out freshCacheItem) && item == freshCacheItem)
                    {
                        _cache.Remove(hash);
                        wasRemoved = true;
                    }
                }

                // Delete the item while outside of the lock (doesn't really matter if it fails to delete, either)
                if (wasRemoved)
                {
                    try
                    {
                        File.Delete(GetCacheFilePath(item.FileName));
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to delete assembly file: {0}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);
                    }
                }
            }

            return asm;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ScriptAssemblyCache"/> is reclaimed by garbage collection.
        /// </summary>
        ~ScriptAssemblyCache()
        {
            Save();
        }

        /// <summary>
        /// Gets the file path for a cache item.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The file path for the cache item.</returns>
        protected string GetCacheFilePath(string fileName)
        {
            return CacheDirectory + fileName + "." + _cacheFileSuffix;
        }

        /// <summary>
        /// Gets a random free file name for the cache directory. The file will be created before
        /// this method returns to reserve the file.
        /// </summary>
        /// <returns>A random free file name for the cache directory.</returns>
        protected string GetFreeFileName()
        {
            // Find a free file name
            string ret;
            do
            {
                ret = Path.GetRandomFileName();
            }
            while (File.Exists(CacheDirectory + ret));

            // Create the file to reserve it
            File.WriteAllBytes(ret, new byte[0]);

            return ret;
        }

        /// <summary>
        /// Loads the cache from file.
        /// </summary>
        void Load()
        {
            lock (_cacheSync)
            {
                _cache.Clear();

                if (!File.Exists(CacheDataFilePath))
                    return;

                XmlValueReader r = new XmlValueReader(CacheDataFilePath, _rootNodeName);
                var items = r.ReadManyNodes(_cacheItemNodeName, x => new CacheItem(x));

                foreach (var item in items)
                {
                    _cache.Add(item.Hash, item);
                }

                _isDirty = false;
                _isOverflowing = _cache.Count > _maxCacheItems;
            }
        }

        /// <summary>
        /// Loads an <see cref="Assembly"/> from file.
        /// </summary>
        /// <param name="filePath">The file path of the <see cref="Assembly"/> to load.</param>
        /// <returns>The loaded <see cref="Assembly"/>, or null if it failed to load.</returns>
        static Assembly LoadAssembly(string filePath)
        {
            Assembly ret = null;

            try
            {
                ret = Assembly.LoadFile(filePath);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load script assembly cache item from file. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }

            return ret;
        }

        /// <summary>
        /// Removes all of the cache files that are not being used.
        /// </summary>
        void RemoveUnusedCacheFiles()
        {
            IEnumerable<string> toRemove;

            // Perform the discovery operations while inside of a lock just to be safe
            lock (_cacheSync)
            {
                var fileNames =
                    Directory.GetFiles(CacheDirectory, "*." + _cacheFileSuffix, SearchOption.TopDirectoryOnly).Select(
                        x => Path.GetFileNameWithoutExtension(x)).ToArray();
                var usedNames = _cache.Select(x => x.Value.FileName).ToArray();
                toRemove = fileNames.Except(usedNames).ToImmutable();
            }

            // The actual deletion doesn't need to take place inside of a lock
            foreach (var fileName in toRemove)
            {
                try
                {
                    File.Delete(CacheDirectory + fileName + "." + _cacheFileSuffix);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to delete cache file while removing unused files: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                }
            }
        }

        /// <summary>
        /// Saves the cache data.
        /// </summary>
        void Save()
        {
            lock (_cacheSync)
            {
                if (!_isDirty)
                    return;

                var items = _cache.Values.ToArray();
                using (var w = new XmlValueWriter(CacheDataFilePath, _rootNodeName))
                {
                    w.WriteManyNodes(_cacheItemNodeName, items, (x, y) => y.Write(x));
                }
            }
        }

        /// <summary>
        /// Trims down the cache if needed.
        /// </summary>
        void TrimCache()
        {
            IEnumerable<CacheItem> removedItems;

            if (!_isOverflowing)
                return;

            lock (_cacheSync)
            {
                // Ensure the cache really is overflowing
                if (!_isOverflowing)
                    return;

                _isOverflowing = false;

                if (_cache.Count < _maxCacheItems)
                    return;

                // Get the number of items to remove
                var removeCount = _cache.Count - (_maxCacheItems - _cacheCleanupAmount);
                if (removeCount <= 0)
                    return;

                // Order the items by date
                var sortedItems = _cache.Values.OrderBy(x => x.LastUsed);

                // Get the actual items to remove
                removedItems = sortedItems.Take(removeCount).ToImmutable();

                // Remove the items
                foreach (var item in removedItems)
                {
                    _cache.Remove(item.Hash);
                }
            }

            // No more need for syncing the cache from here on

            // Delete the files for the removed items
            foreach (var item in removedItems)
            {
                var filePath = CacheDirectory + item.FileName;

                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                catch (IOException)
                {
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to delete cached assembly file `{0}`. Exception: {1}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, filePath, ex);
                }
            }
        }

        /// <summary>
        /// An item in the <see cref="ScriptAssemblyCache"/>.
        /// </summary>
        class CacheItem
        {
            const string _fileNameValueKey = "FileName";
            const string _hashValueKey = "Hash";
            const string _lastUsedValueKey = "LastUsed";

            string _fileName;
            byte[] _hash;
            DateTime _lastUsed;

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheItem"/> class.
            /// </summary>
            /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
            public CacheItem(IValueReader r)
            {
                Read(r);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheItem"/> class.
            /// </summary>
            /// <param name="hash">The hash.</param>
            /// <param name="fileName">Name of the file.</param>
            public CacheItem(byte[] hash, string fileName)
            {
                _hash = hash;
                _fileName = fileName;
                _lastUsed = DateTime.Now;
            }

            /// <summary>
            /// Gets the name of the file.
            /// </summary>
            /// <value>The name of the file.</value>
            public string FileName
            {
                get { return _fileName; }
            }

            /// <summary>
            /// Gets the hash.
            /// </summary>
            /// <value>The hash.</value>
            public byte[] Hash
            {
                get { return _hash; }
            }

            /// <summary>
            /// Gets the time the item was last used.
            /// </summary>
            /// <value>The time the item was last used.</value>
            public DateTime LastUsed
            {
                get { return _lastUsed; }
            }

            /// <summary>
            /// Gets the string for a hash.
            /// </summary>
            /// <param name="bytes">The hash.</param>
            /// <returns>The string for the hash.</returns>
            static string HashToString(byte[] bytes)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString());
                    sb.Append("-");
                }

                if (sb.Length > 0)
                    sb.Length -= 1;

                return sb.ToString();
            }

            /// <summary>
            /// Reads the values from an <see cref="IValueReader"/>.
            /// </summary>
            /// <param name="r">The <see cref="IValueReader"/> to read the values from.</param>
            void Read(IValueReader r)
            {
                var hashString = r.ReadString(_hashValueKey);
                _hash = StringToHash(hashString);

                _fileName = r.ReadString(_fileNameValueKey);

                var lastUsedRaw = r.ReadLong(_lastUsedValueKey);
                _lastUsed = DateTime.FromFileTimeUtc(lastUsedRaw);
            }

            /// <summary>
            /// Gets the hash from a string.
            /// </summary>
            /// <param name="s">The string.</param>
            /// <returns>The hash.</returns>
            static byte[] StringToHash(string s)
            {
                var byteStrs = s.Split('-');
                var bytes = new byte[byteStrs.Length];

                for (int i = 0; i < byteStrs.Length; i++)
                {
                    bytes[i] = byte.Parse(byteStrs[i]);
                }

                return bytes;
            }

            /// <summary>
            /// Writes the values to an <see cref="IValueWriter"/>.
            /// </summary>
            /// <param name="w">The <see cref="IValueWriter"/> to write to.</param>
            public void Write(IValueWriter w)
            {
                w.Write(_hashValueKey, HashToString(Hash));
                w.Write(_fileNameValueKey, FileName);
                w.Write(_lastUsedValueKey, LastUsed.ToFileTimeUtc());
            }
        }
    }
}