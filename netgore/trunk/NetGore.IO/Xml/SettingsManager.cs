using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// Manages the settings of a collection of IRestorableSettings objects by saving the settings to file for
    /// all managed objects, along with loading the previous settings for objects when they are added to the manager.
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// File path being used.
        /// </summary>
        readonly string _filePath;

        /// <summary>
        /// Contains the collection of loaded settings that have not yet been loaded back. Values in here wait until
        /// an IRestorableSettings is added through Add() that has the same key as in this. When a match is found,
        /// the item is removed from this collection. Therefore, if this collection is empty, all settings have been restored.
        /// </summary>
        readonly List<NodeItems> _loadedNodeItems;

        /// <summary>
        /// Dictionary of IRestorableSettings objects being tracked, indexed by their unique identifier.
        /// </summary>
        readonly Dictionary<string, IRestorableSettings> _objs =
            new Dictionary<string, IRestorableSettings>(StringComparer.OrdinalIgnoreCase);

        readonly string _rootNode;
        readonly object _syncRoot = new object();

        bool _disposed = false;

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
        /// root node name as this will be able to be loaded by this SettingsManager.
        /// </summary>
        public string RootNode
        {
            get { return _rootNode; }
        }

        /// <summary>
        /// SettingsManager constructor.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        public SettingsManager(string rootNode, string filePath)
        {
            _rootNode = rootNode;
            _filePath = filePath;

            // Load the existing settings
            _loadedNodeItems = LoadSettings(filePath).ToList();
        }

        /// <summary>
        /// SettingsManager constructor.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        /// <param name="secondaryPath">Secondary path to check for settings from. The FilePath will still be
        /// <paramref name="filePath"/>, but the settings can be loaded from somewhere else, like a default
        /// settings file.</param>
        public SettingsManager(string rootNode, string filePath, string secondaryPath)
        {
            _rootNode = rootNode;
            _filePath = filePath;

            // Find the file path to use
            string loadPath = null;
            if (SimpleXmlReader.GetRootNodeName(filePath) == rootNode)
                loadPath = filePath;
            else if (SimpleXmlReader.GetRootNodeName(secondaryPath) == rootNode)
                loadPath = secondaryPath;

            // Load the settings from the selected path
            _loadedNodeItems = LoadSettings(loadPath).ToList();
        }

        /// <summary>
        /// SettingsManager constructor.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        /// <param name="secondaryPaths">Secondary paths to check for settings from. The FilePath will still be
        /// <paramref name="filePath"/>, but the settings can be loaded from somewhere else, like a default
        /// settings file. The first path to contain a file, even if not a valid settings file, is used to
        /// load the settings from.</param>
        public SettingsManager(string rootNode, string filePath, IEnumerable<string> secondaryPaths)
        {
            _rootNode = rootNode;
            _filePath = filePath;

            // Find the file path to use
            string loadPath = null;
            if (SimpleXmlReader.GetRootNodeName(filePath) == rootNode)
                loadPath = filePath;
            else if (secondaryPaths != null)
            {
                foreach (string path in secondaryPaths)
                {
                    if (SimpleXmlReader.GetRootNodeName(path) == rootNode)
                    {
                        loadPath = path;
                        break;
                    }
                }
            }

            // Load the settings from the selected path
            _loadedNodeItems = LoadSettings(loadPath).ToList();
        }

        /// <summary>
        /// Adds an object to be tracked by this GUISettings and loads the previous settings for the object
        /// if possible.
        /// </summary>
        /// <param name="key">Unique identifier of the IRestorableSettings object.</param>
        /// <param name="obj">IRestorableSettings object to load and save the settings for.</param>
        public void Add(string key, IRestorableSettings obj)
        {
            // Add to the gui items to track
            _objs.Add(key, obj);

            // Check if the settings need to be restored
            int index = _loadedNodeItems.FindIndex(x => x.Name == key);

            if (index >= 0)
            {
                // Restore the settings and remove from the collection
                var loadDict = _loadedNodeItems[index].ToDictionary();
                _loadedNodeItems.RemoveAt(index);
                obj.Load(loadDict);
            }
        }

        /// <summary>
        /// Asynchronously saves the settings for all the tracked IRestorableSettings objects.
        /// </summary>
        public void AsyncSave()
        {
            ThreadPool.QueueUserWorkItem(AsyncSaveCallback, this);
        }

        /// <summary>
        /// Callback for saving using the thread pool.
        /// </summary>
        /// <param name="sender">SettingsManager the request came from.</param>
        static void AsyncSaveCallback(object sender)
        {
            ((SettingsManager)sender).Save();
        }

        /// <summary>
        /// Disposes the GUISettings and flushes out the stored settings.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            Save();
        }

        /// <summary>
        /// Loads the existing settings (if they exist) into memory.
        /// </summary>
        /// <param name="path">Path of the file to load the settings from.</param>
        /// <returns>An IEnumerable of all the NodeItems loaded, if any.</returns>
        IEnumerable<NodeItems> LoadSettings(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return Enumerable.Empty<NodeItems>();

            try
            {
                SimpleXmlReader r = new SimpleXmlReader(path);
                if (r.RootNodeName != _rootNode)
                    return Enumerable.Empty<NodeItems>();
                return r.Items;
            }
            catch (XmlException)
            {
                return Enumerable.Empty<NodeItems>();
            }
        }

        /// <summary>
        /// Saves the settings for all the tracked IRestorableSettings objects.
        /// </summary>
        public void Save()
        {
            lock (_syncRoot)
            {
                using (SimpleXmlWriter w = new SimpleXmlWriter(_filePath, _rootNode))
                {
                    foreach (var pair in _objs)
                    {
                        w.Write(pair.Key, pair.Value.Save());
                    }
                }
            }
        }
    }
}