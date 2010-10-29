using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using log4net;
using Timer = System.Timers.Timer;

namespace NetGore.IO
{
    /// <summary>
    /// Manages the settings of a collection of <see cref="IPersistable"/> objects by saving the state to file for
    /// all managed objects, along with loading the previous state for objects when they are added to the manager.
    /// </summary>
    public class ObjectStatePersister : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _countValueName = "Count";

        /// <summary>
        /// The initial auto-save rate.
        /// </summary>
        const int _initialAutoSaveRate = 60000;

        const string _itemNodeName = "Item";
        const string _itemsNodeName = "Items";
        const string _keyValueName = "Key";
        const string _valueNodeName = "Values";

        /// <summary>
        /// The <see cref="StringComparer"/> to use for the created dictionaries.
        /// </summary>
        static readonly StringComparer _keyStringComparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// File path being used.
        /// </summary>
        readonly string _filePath;

        /// <summary>
        /// Contains the collection of loaded settings that have not yet been loaded back. Values in here wait until
        /// an <see cref="IPersistable"/>s is added through Add() that has the same key as in this. When a match is found,
        /// the item is removed from this collection. Therefore, if this collection is empty, all settings have been restored.
        /// </summary>
        readonly Dictionary<string, IValueReader> _loadedNodeItems;

        /// <summary>
        /// Dictionary of <see cref="IPersistable"/> objects being tracked, indexed by their unique identifier.
        /// </summary>
        readonly Dictionary<string, IPersistable> _objs = new Dictionary<string, IPersistable>(_keyStringComparer);

        readonly string _rootNode;
        readonly object _saveLock = new object();

        Timer _autoSaveTimer;
        bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectStatePersister"/> class.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        public ObjectStatePersister(string rootNode, string filePath) : this(rootNode, filePath, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectStatePersister"/> class.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        /// <param name="secondaryPath">Secondary path to check for settings from. The FilePath will still be
        /// <paramref name="filePath"/>, but the settings can be loaded from somewhere else, like a default
        /// settings file.</param>
        public ObjectStatePersister(string rootNode, string filePath, string secondaryPath)
            : this(rootNode, filePath, new string[] { secondaryPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectStatePersister"/> class.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        /// <param name="secondaryPaths">Secondary paths to check for settings from. The FilePath will still be
        /// <paramref name="filePath"/>, but the settings can be loaded from somewhere else, like a default
        /// settings file. The first path to contain a file, even if not a valid settings file, is used to
        /// load the settings from.</param>
        public ObjectStatePersister(string rootNode, string filePath, IEnumerable<string> secondaryPaths)
        {
            _rootNode = rootNode;
            _filePath = filePath;

            // Try to load from the primary path, then the secondary paths if the primary fails
            var loadedItems = LoadSettings(filePath);
            if (loadedItems == null)
            {
                foreach (var secondaryPath in secondaryPaths.Where(x => !string.IsNullOrEmpty(x)))
                {
                    loadedItems = LoadSettings(secondaryPath);
                    if (loadedItems != null)
                        break;
                }
            }

            _loadedNodeItems = loadedItems ?? new Dictionary<string, IValueReader>(_keyStringComparer);

            // Set up the auto-save timer
            AutoSaveRate = _initialAutoSaveRate;
        }

        /// <summary>
        /// Gets or sets the rate in milliseconds at which the <see cref="ObjectStatePersister"/> is automatically saved. If the
        /// value is less than or equal to 0, auto-saving will be disabled.
        /// </summary>
        public int AutoSaveRate
        {
            get { return _autoSaveTimer == null ? 0 : (int)_autoSaveTimer.Interval; }
            set
            {
                if (value > 0)
                {
                    // Check if the value is already set
                    if (_autoSaveTimer != null && value == (int)_autoSaveTimer.Interval)
                        return;

                    // Positive value and not what we already have, so check if we need to create the timer
                    if (_autoSaveTimer == null)
                    {
                        _autoSaveTimer = new Timer { AutoReset = false };
                        _autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
                        _autoSaveTimer.Start();
                    }

                    _autoSaveTimer.Interval = value;
                }
                else
                {
                    // Check if the timer is already null
                    if (_autoSaveTimer == null)
                        return;

                    // Dispose of the timer
                    _autoSaveTimer.Dispose();
                    _autoSaveTimer = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Gets the file path used to load and save the settings.
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets if this GUISettings has been disposed of.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Gets the name of the root node used for Xml file. Only an Xml file that contains the same
        /// root node name as this will be able to be loaded by this <see cref="ObjectStatePersister"/>.
        /// </summary>
        public string RootNode
        {
            get { return _rootNode; }
        }

        /// <summary>
        /// Adds an object to be tracked by this <see cref="ObjectStatePersister"/> and loads the previous settings
        /// for the object if possible.
        /// </summary>
        /// <param name="objs">The persistable object and unique key pairs.</param>
        /// <exception cref="ArgumentException">An object with the key provided by any of the <paramref name="objs"/>
        /// already exists in this collection.</exception>
        public void Add(IEnumerable<KeyValuePair<string, IPersistable>> objs)
        {
            foreach (var obj in objs)
            {
                Add(obj.Key, obj.Value);
            }
        }

        /// <summary>
        /// Adds an object to be tracked by this <see cref="ObjectStatePersister"/> and loads the previous settings
        /// for the object if possible.
        /// </summary>
        /// <param name="obj">The persistable object and unique key pair.</param>
        /// <exception cref="ArgumentException">An object with the key provided by the <paramref name="obj"/> already exists
        /// in this collection.</exception>
        public void Add(KeyValuePair<string, IPersistable> obj)
        {
            Add(obj.Key, obj.Value);
        }

        /// <summary>
        /// Adds an object to be tracked by this <see cref="ObjectStatePersister"/> and loads the previous settings
        /// for the object if possible.
        /// </summary>
        /// <param name="key">Unique identifier of the <see cref="IPersistable"/> object.</param>
        /// <param name="obj"><see cref="IPersistable"/> object to load and save the settings for.</param>
        /// <exception cref="ArgumentException">An object with the given <paramref name="key"/> already exists
        /// in this collection.</exception>
        public void Add(string key, IPersistable obj)
        {
            // Add to the gui items to track
            _objs.Add(key, obj);

            // Check if the settings need to be restored
            IValueReader valueReader;
            if (_loadedNodeItems.TryGetValue(key, out valueReader))
            {
                try
                {
                    obj.ReadState(valueReader);
                }
                catch (Exception ex)
                {
                    // If we fail to read the state, just absorb the exception while printing an error
                    const string errmsg = "Failed to load settings for object `{0}` with key `{1}`: {2}";
                    var err = string.Format(errmsg, obj, key, ex);
                    log.Error(err);
                    Debug.Fail(err);
                }

                _loadedNodeItems.Remove(key);
            }
        }

        /// <summary>
        /// Asynchronously saves the settings for all the tracked <see cref="IPersistable"/> objects.
        /// </summary>
        public void AsyncSave()
        {
            ThreadPool.QueueUserWorkItem(AsyncSaveCallback, this);
        }

        /// <summary>
        /// Callback for saving using the thread pool.
        /// </summary>
        /// <param name="sender">The <see cref="ObjectStatePersister"/> the request came from.</param>
        static void AsyncSaveCallback(object sender)
        {
            ((ObjectStatePersister)sender).Save();
        }

        /// <summary>
        /// Handles the Elapsed event of the <see cref="_autoSaveTimer"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        void AutoSaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Ensure the timer is stopped so we don't try saving multiple times at once
            _autoSaveTimer.Stop();

            // Perform the save
            Save();

            // Start the timer back up
            _autoSaveTimer.AutoReset = false;
            _autoSaveTimer.Start();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ObjectStatePersister"/> is reclaimed by garbage collection.
        /// </summary>
        ~ObjectStatePersister()
        {
            Dispose();
        }

        /// <summary>
        /// Loads the existing settings into memory.
        /// </summary>
        /// <param name="path">Path of the file to load the settings from.</param>
        /// <returns>A Dictionary of all the items loaded, if any, or null if there was an error in loading.</returns>
        Dictionary<string, IValueReader> LoadSettings(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            var fi = new FileInfo(path);
            if (fi.Length < 1)
                return null;

            var nodeReaders = new Dictionary<string, IValueReader>(_keyStringComparer);

            try
            {
                var reader = GenericValueReader.CreateFromFile(path, _rootNode);

                reader = reader.ReadNode(_itemsNodeName);
                var count = reader.ReadInt(_countValueName);

                for (var i = 0; i < count; i++)
                {
                    var nodeReader = reader.ReadNode(_itemNodeName + i);
                    var keyName = nodeReader.ReadString(_keyValueName);

                    var valueReader = nodeReader.ReadNode(_valueNodeName);

                    nodeReaders.Add(keyName, valueReader);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load settings from `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.Error(string.Format(errmsg, path, ex));
                Debug.Fail(string.Format(errmsg, path, ex));
                return null;
            }

            return nodeReaders;
        }

        /// <summary>
        /// Saves the settings for all the tracked <see cref="IPersistable"/> objects.
        /// </summary>
        public void Save()
        {
            var objs = _objs.ToArray();

            // Lock to ensure we never try to save from multiple threads at once
            lock (_saveLock)
            {
                using (var w = GenericValueWriter.Create(_filePath, _rootNode, EncodingFormat))
                {
                    w.WriteStartNode(_itemsNodeName);
                    {
                        w.Write(_countValueName, objs.Length);
                        for (var i = 0; i < objs.Length; i++)
                        {
                            w.WriteStartNode(_itemNodeName + i);
                            {
                                var obj = objs[i];
                                w.Write(_keyValueName, obj.Key);
                                w.WriteStartNode(_valueNodeName);
                                {
                                    obj.Value.WriteState(w);
                                }
                                w.WriteEndNode(_valueNodeName);
                            }
                            w.WriteEndNode(_itemNodeName + i);
                        }
                    }
                    w.WriteEndNode(_itemsNodeName);
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the GUISettings and flushes out the stored settings.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            GC.SuppressFinalize(this);

            Save();
        }

        #endregion
    }
}