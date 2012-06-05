using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Handles loading and saving the persistent state for <see cref="Tool"/>s.
    /// </summary>
    public class ToolStateManager : IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _kvpKeyName = "Key";
        const string _kvpValueNodeName = "Value";
        const string _rootNodeName = "ToolStateManager";
        const string _separatorClassName = "|";
        const string _toolBarItemsKeyName = "ToolBarVisibility";
        const string _toolBarItemsNodeName = "ToolBarItems";
        const string _toolBarItemsValueName = "Order";
        const string _toolSettingsNodeName = "ToolSettings";

        /// <summary>
        /// The <see cref="StringComparer"/> to use for comparing a <see cref="Tool"/>'s key.
        /// </summary>
        static readonly StringComparer _keyComp = StringComparer.Ordinal;

        readonly object _saveSync = new object();

        readonly IDictionary<ToolBarVisibility, IEnumerable<string>> _toolBarOrder =
            new TSDictionary<ToolBarVisibility, IEnumerable<string>>(EnumComparer<ToolBarVisibility>.Instance);

        readonly IDictionary<string, IValueReader> _toolSettings = new TSDictionary<string, IValueReader>(_keyComp);

        readonly IDictionary<string, Tool> _tools = new TSDictionary<string, Tool>(_keyComp);

        string _currentSettingsFile;

        /// <summary>
        /// Gets or sets the file path to the current settings file. When this value is changed, the settings of the <see cref="Tool"/>s
        /// will be saved to the old path (if it is value) then loaded from the new path only if the file exists at the new path. If
        /// the new path does not exist, no settings will be loaded and the <see cref="Tool"/>s will be unaltered.
        /// If the value is set to an invalid file path or one that cannot be written to or read from, an <see cref="Exception"/> will
        /// be thrown and this property will not be changed.
        /// If the value is set to a file that does not exist, the current settings will be written to that file.
        /// </summary>
        public string CurrentSettingsFile
        {
            get { return _currentSettingsFile; }
            set
            {
                if (StringComparer.Ordinal.Equals(_currentSettingsFile, value))
                    return;

                // Save out to old file
                if (!string.IsNullOrEmpty(_currentSettingsFile))
                {
                    try
                    {
                        Save(_currentSettingsFile);
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to save to path `{0}`. Exception: {1}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, _currentSettingsFile, ex);
                        Debug.Fail(string.Format(errmsg, _currentSettingsFile, ex));
                    }
                }

                // If the new path does not exist, save the current settings to it
                if (!File.Exists(value))
                    Save(value);

                // Load from the new file
                if (!TryLoad(value))
                {
                    const string errmsg =
                        "Failed to load from the settings file at `{0}`. See previous log entries to see why. Aborting property change.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, value);
                    Debug.Fail(string.Format(errmsg, value));
                    return;
                }

                // Set the new value
                _currentSettingsFile = value;
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
        /// Adds a <see cref="Tool"/> to this <see cref="ToolStateManager"/>. If settings already exist for the
        /// <paramref name="tool"/>, they will be applied automatically. Only one instance of each type can be added.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="tool"/>, or a different instance of the same class, is already
        /// in the collection.</exception>
        public void Add(Tool tool)
        {
            if (tool == null)
                throw new ArgumentNullException("tool");

            var key = GetToolKey(tool);
            _tools.Add(key, tool);

            ApplyToolSettings(tool);
        }

        /// <summary>
        /// Reloads or resets the settings for a specific <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to reset the settings of.</param>
        void ApplyToolSettings(Tool tool)
        {
            var key = GetToolKey(tool);

            IValueReader s;
            if (!_toolSettings.TryGetValue(key, out s))
            {
                // No settings provided, reset the tool
                tool.ResetState();
            }
            else
            {
                // Load from settings
                try
                {
                    tool.ReadState(s);
                }
                catch (Exception ex)
                {
                    const string errmsg =
                        "Failed to restore the settings for tool `{0}`. Resetting tool to default. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, tool, ex);
                    Debug.Fail(string.Format(errmsg, tool, ex));

                    // When loading from settings fails, reset to default values
                    tool.ResetState();
                }
            }
        }

        /// <summary>
        /// Gets the default file path for the <see cref="ToolStateManager"/> settings file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to get the path for.</param>
        /// <param name="profileName">The name of the settings profile to use. Use null for the default profile.</param>
        /// <returns>
        /// The default file path for the <see cref="ToolStateManager"/> settings file.
        /// </returns>
        public static string GetFilePath(ContentPaths contentPath, string profileName)
        {
            var fileName = "EditorToolStates";
            if (!string.IsNullOrEmpty(profileName))
                fileName += "." + profileName;

            fileName += EngineSettings.DataFileSuffix;

            return contentPath.Settings.Join(fileName);
        }

        /// <summary>
        /// Gets the string key for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to get the key for.</param>
        /// <returns>The key for the <paramref name="tool"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
        protected static string GetToolKey(Tool tool)
        {
            if (tool == null)
                throw new ArgumentNullException("tool");

            return tool.GetType().FullName;
        }

        /// <summary>
        /// Reads a <see cref="KeyValuePair{T,U}"/> for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The <see cref="KeyValuePair{T,U}"/>.</returns>
        static KeyValuePair<string, IValueReader> ReadKVP(IValueReader reader)
        {
            var kvpKey = reader.ReadString(_kvpKeyName);
            var kvpValue = reader.ReadNode(_kvpValueNodeName);

            return new KeyValuePair<string, IValueReader>(kvpKey, kvpValue);
        }

        /// <summary>
        /// Reads the items order for a <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The <see cref="KeyValuePair{T,U}"/> where the key is the <see cref="ToolBarVisibility"/> and the value
        /// is an ordered list of the control names.</returns>
        static KeyValuePair<ToolBarVisibility, IEnumerable<string>> ReadToolBarItems(IValueReader reader)
        {
            var kvpKey = reader.ReadEnum<ToolBarVisibility>(_toolBarItemsKeyName);
            var kvpValue = reader.ReadMany(_toolBarItemsValueName, (r, name) => r.ReadString(name));

            return new KeyValuePair<ToolBarVisibility, IEnumerable<string>>(kvpKey, kvpValue);
        }

        /// <summary>
        /// Removes a <see cref="Tool"/> from this <see cref="ToolStateManager"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to add.</param>
        /// <returns>True if the <paramref name="tool"/> was successfully removed; false if it was not in the collection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tool"/> is null.</exception>
        public bool Remove(Tool tool)
        {
            if (tool == null)
                throw new ArgumentNullException("tool");

            var key = GetToolKey(tool);
            return _tools.Remove(key);
        }

        /// <summary>
        /// Resets all of the <see cref="Tool"/>s in this object back to the state defined by the currently loaded settings.
        /// <see cref="Tool"/>s in this object that have no settings specified in the loaded file will have their values
        /// reset to default.
        /// </summary>
        public void ResetTools()
        {
            // Remove everything from the ToolBars first
            foreach (var tb in ToolBar.ToolBars)
            {
                try
                {
                    tb.Items.Clear();
                }
                catch (Exception ex)
                {
                    // Log but ignore when there is an error clearing for some reason
                    const string errmsg = "Unexpected error while trying to clear ToolBar `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, tb, ex);
                    Debug.Fail(string.Format(errmsg, tb, ex));
                    continue;
                }
            }

            // Apply the settings to all the tools
            foreach (var tool in _tools.Values)
            {
                ApplyToolSettings(tool);
            }

            // Re-order the tools one ToolBar at a time
            foreach (var kvp in _toolBarOrder)
            {
                // Get the ToolBar
                var tb = ToolBar.GetToolBar(kvp.Key);
                if (tb == null)
                {
                    const string errmsg = "Could not find ToolBar using ToolBarVisibility `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, kvp.Key);
                    Debug.Fail(string.Format(errmsg, kvp.Key));
                    continue;
                }

                // Loop through the control keys in reverse since we will be pushing everything to the head. This will result
                // in the tools not in our ordering list to appear at the tail.
                foreach (var key in kvp.Value.Reverse())
                {
                    if (StringComparer.Ordinal.Equals(_separatorClassName, key))
                    {
                        // If it is a separator, then add the new separator
                        tb.InsertSeparator(0);
                    }
                    else
                    {
                        // Find the Tool with the same key and, if found, push it to the head
                        Tool toolForKey;
                        if (_tools.TryGetValue(key, out toolForKey))
                        {
                            if (toolForKey.ToolBarControl != null)
                                toolForKey.ToolBarControl.MoveToHead();
                        }
                        else
                        {
                            const string errmsg = "Could not find tool with key `{0}`.";
                            if (log.IsWarnEnabled)
                                log.WarnFormat(errmsg, key);
                            Debug.Fail(string.Format(errmsg, key));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        /// <param name="profileName">The name of the settings profile to use. Use null for the default profile.</param>
        public void Save(ContentPaths contentPath, string profileName)
        {
            var filePath = GetFilePath(contentPath, profileName);
            Save(filePath);
        }

        /// <summary>
        /// Saves the settings to file to the <see cref="CurrentSettingsFile"/>.
        /// </summary>
        public void Save()
        {
            Save(CurrentSettingsFile);
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        public void Save(string filePath)
        {
            var kvps = _tools.ToArray();
            var toolBars = ToolBar.ToolBars;

            lock (_saveSync)
            {
                using (var writer = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
                {
                    writer.WriteManyNodes(_toolSettingsNodeName, kvps, WriteKVP);
                    writer.WriteManyNodes(_toolBarItemsNodeName, toolBars, WriteToolBarItems);
                }
            }
        }

        /// <summary>
        /// Loads the settings from the <see cref="CurrentSettingsFile"/>.
        /// </summary>
        /// <returns>True if the settings were successfully loaded from the file; otherwise false.</returns>
        public bool TryLoad()
        {
            return TryLoad(CurrentSettingsFile);
        }

        /// <summary>
        /// Loads the settings from file, and applies loaded settings to the <see cref="Tool"/>s in this object.
        /// <see cref="Tool"/>s in this object that have no settings specified in the loaded file will have their values
        /// reset to default.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        /// <param name="profileName">The name of the settings profile to use. Use null for the default profile.</param>
        /// <returns>True if the settings were successfully loaded from the file; otherwise false.</returns>
        public bool TryLoad(ContentPaths contentPath, string profileName)
        {
            var filePath = GetFilePath(contentPath, profileName);
            return TryLoad(filePath);
        }

        /// <summary>
        /// Loads the settings from file, and applies loaded settings to the <see cref="Tool"/>s in this object.
        /// <see cref="Tool"/>s in this object that have no settings specified in the loaded file will have their values
        /// reset to default.
        /// </summary>
        /// <param name="filePath">The file to load from.</param>
        /// <returns>True if the settings were successfully loaded from the file; otherwise false.</returns>
        public bool TryLoad(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                var reader = GenericValueReader.CreateFromFile(filePath, _rootNodeName);

                // Read in the settings
                var toolKVPs = reader.ReadManyNodes(_toolSettingsNodeName, ReadKVP);
                var toolBarKVPS = reader.ReadManyNodes(_toolBarItemsNodeName, ReadToolBarItems);

                // Out with the old
                _toolSettings.Clear();
                _toolBarOrder.Clear();

                // And in with the new
                foreach (var kvp in toolKVPs)
                {
                    try
                    {
                        _toolSettings.Add(kvp);
                    }
                    catch (Exception ex)
                    {
                        // When there is an error adding to the collection, just skip the item
                        const string errmsg =
                            "Failed to add item to _toolSettings dictionary. Key: {0}. Value: {1}. Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, kvp.Key, kvp.Value, ex);
                        Debug.Fail(string.Format(errmsg, kvp.Key, kvp.Value, ex));
                    }
                }

                foreach (var kvp in toolBarKVPS)
                {
                    try
                    {
                        _toolBarOrder.Add(kvp);
                    }
                    catch (Exception ex)
                    {
                        // When there is an error adding to the collection, just skip the item
                        const string errmsg =
                            "Failed to add item to _toolBarOrder dictionary. Key: {0}. Value: {1}. Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, kvp.Key, kvp.Value, ex);
                        Debug.Fail(string.Format(errmsg, kvp.Key, kvp.Value, ex));
                    }
                }

                // Refresh all settings
                ResetTools();
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to load settings from file `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath, ex);
                Debug.Fail(string.Format(errmsg, filePath, ex));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Writes a <see cref="KeyValuePair{T,U}"/> for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="kvp">The value to write.</param>
        static void WriteKVP(IValueWriter writer, KeyValuePair<string, Tool> kvp)
        {
            writer.Write(_kvpKeyName, kvp.Key);

            writer.WriteStartNode(_kvpValueNodeName);
            {
                kvp.Value.WriteState(writer);
            }
            writer.WriteEndNode(_kvpValueNodeName);
        }

        /// <summary>
        /// Writes the items in a <see cref="ToolBar"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="tb">The <see cref="ToolBar"/> containing the items to write.</param>
        static void WriteToolBarItems(IValueWriter writer, ToolBar tb)
        {
            var tbItems = tb.GetItems().Select(x => x == null ? _separatorClassName : (GetToolKey(x.Tool) ?? _separatorClassName));

            writer.WriteEnum(_toolBarItemsKeyName, tb.ToolBarVisibility);
            writer.WriteMany(_toolBarItemsValueName, tbItems, writer.Write);
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
        }

        #endregion
    }
}