using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using NetGore;

namespace NetGore.IO
{
    /// <summary>
    /// Manages the settings of a collection of IRestorableSettings objects by saving the settings to file for
    /// all managed objects, along with loading the previous settings for objects when they are added to the manager.
    /// </summary>
    public class SettingsManager : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _itemsNodeName = "Items";
        const string _objectValueKey = "Object";
        const string _valuesNodeName = "Values";

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
        bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        /// <param name="rootNode">Name of the root node. Used to ensure the correct file is loaded when
        /// loading settings. Not required to be unique, but recommended.</param>
        /// <param name="filePath">Primary file path to use, and first place to check for settings.</param>
        public SettingsManager(string rootNode, string filePath)
        {
            _rootNode = rootNode;
            _filePath = filePath;

            // Load the existing settings
            var loadedItems = LoadSettings(filePath);
            if (loadedItems != null)
                _loadedNodeItems = loadedItems.ToList();
            else
                _loadedNodeItems = new List<NodeItems>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
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

            // Try to load from the primary path, then the secondary if the primary fails
            var loadedItems = LoadSettings(filePath);
            if (loadedItems == null && !string.IsNullOrEmpty(secondaryPath))
                loadedItems = LoadSettings(secondaryPath);

            // If the values are null, we have to just make a new list, otherwise use the values
            if (loadedItems != null)
                _loadedNodeItems = loadedItems.ToList();
            else
                _loadedNodeItems = new List<NodeItems>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
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

            // Try to load from the primary path, then the secondary paths if the primary fails
            var loadedItems = LoadSettings(filePath);
            if (loadedItems == null)
            {
                foreach (string secondaryPath in secondaryPaths.Where(x => !string.IsNullOrEmpty(x)))
                {
                    loadedItems = LoadSettings(secondaryPath);
                    if (loadedItems != null)
                        break;
                }
            }

            // If the values are null, we have to just make a new list, otherwise use the values
            if (loadedItems != null)
                _loadedNodeItems = loadedItems.ToList();
            else
                _loadedNodeItems = new List<NodeItems>();
        }

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
        /// Adds an object to be tracked by this GUISettings and loads the previous settings for the object
        /// if possible.
        /// </summary>
        /// <param name="key">Unique identifier of the IRestorableSettings object.</param>
        /// <param name="obj">IRestorableSettings object to load and save the settings for.</param>
        /// <exception cref="ArgumentException">An object with the given <paramref name="key"/> already exists
        /// in this collection.</exception>
        public void Add(string key, IRestorableSettings obj)
        {
            // Add to the gui items to track
            _objs.Add(key, obj);

            // Check if the settings need to be restored
            int index = _loadedNodeItems.FindIndex(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

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
        /// Loads the existing settings into memory.
        /// </summary>
        /// <param name="path">Path of the file to load the settings from.</param>
        /// <returns>An IEnumerable of all the NodeItems loaded, if any, or null if there was an error in loading.</returns>
        IEnumerable<NodeItems> LoadSettings(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            FileInfo fi = new FileInfo(path);
            if (fi.Length < 1)
                return null;

            IEnumerable<NodeItems> nodeItems;

            try
            {
                IValueReader reader = new XmlValueReader(path, _rootNode);
                nodeItems = reader.ReadManyNodes(_itemsNodeName, x => Read(x));
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load settings from `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.Error(string.Format(errmsg, path, ex));
                Debug.Fail(string.Format(errmsg, path, ex));
                return null;
            }

            return nodeItems;
        }

        /// <summary>
        /// Reads the NodeItems from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>The NodeItems from the IValueReader.</returns>
        static NodeItems Read(IValueReader reader)
        {
            string obj = reader.ReadString(_objectValueKey);
            var values = reader.ReadManyNodes(_valuesNodeName, x => new NodeItem(x));

            return new NodeItems(obj, values);
        }

        /// <summary>
        /// Saves the settings for all the tracked <see cref="IRestorableSettings"/> objects.
        /// </summary>
        public void Save()
        {
            using (IValueWriter w = new XmlValueWriter(_filePath, _rootNode))
            {
                w.WriteManyNodes(_itemsNodeName, _objs, Write);
            }
        }

        /// <summary>
        /// Writes the items to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="item">Items to write.</param>
        static void Write(IValueWriter writer, KeyValuePair<string, IRestorableSettings> item)
        {
            var values = item.Value.Save();

            writer.Write(_objectValueKey, item.Key);
            writer.WriteManyNodes(_valuesNodeName, values, ((pwriter, pitem) => pitem.Write(pwriter)));
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

            Save();
        }

        #endregion
    }
}