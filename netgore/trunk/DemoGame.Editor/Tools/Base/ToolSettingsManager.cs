using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Collections;
using NetGore.IO;

namespace DemoGame.Editor
{
    /// <summary>
    /// Handles loading and saving the persistent settings for <see cref="Tool"/>s.
    /// </summary>
    public class ToolSettingsManager : IPersistable
    {
        const string _kvpKeyName = "Key";
        const string _kvpValueNodeName = "Value";
        const string _rootNodeName = "ToolSettingsManager";
        const string _toolSettingsNodeName = "ToolSettings";

        /// <summary>
        /// The <see cref="StringComparer"/> to use for comparing a <see cref="Tool"/>'s key.
        /// </summary>
        static readonly StringComparer _keyComp = StringComparer.Ordinal;

        readonly IDictionary<string, string> _toolSettings = new TSDictionary<string, string>(_keyComp);
        readonly IDictionary<string, Tool> _tools = new TSDictionary<string, Tool>(_keyComp);

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

        /// <summary>
        /// Adds a <see cref="Tool"/> to this <see cref="ToolSettingsManager"/>. If settings already exist for the
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
        /// Attempts to apply the settings for a <see cref="Tool"/> to it.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> apply the settings to.</param>
        void ApplyToolSettings(Tool tool)
        {
            var key = GetToolKey(tool);
            string settings;

            // Get the settings
            if (!_toolSettings.TryGetValue(key, out settings))
            {
                // Couldn't load the settings, so reset the tool to the default settings
                tool.ResetValues();
                return;
            }

            // Load the settings into an IValueReader and apply them
            // TODO: !!
            //using (var reader = GenericValueReader.CreateFromFile(settings, 
        }

        /// <summary>
        /// Gets the default file path for the <see cref="ToolSettingsManager"/> settings file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to get the path for.</param>
        /// <returns>The default file path for the <see cref="ToolSettingsManager"/> settings file.</returns>
        public static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Settings.Join("EditorToolSettings" + EngineSettings.DataFileSuffix);
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
        /// Removes a <see cref="Tool"/> from this <see cref="ToolSettingsManager"/>.
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
        /// Saves the settings to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        public void Save(ContentPaths contentPath)
        {
            var filePath = GetFilePath(contentPath);
            Save(filePath);
        }

        /// <summary>
        /// Saves the settings to file.
        /// </summary>
        /// <param name="filePath">The file path to save to.</param>
        public void Save(string filePath)
        {
            var kvps = _tools.ToArray();

            using (var writer = new GenericValueWriter(filePath, _rootNodeName, EncodingFormat))
            {
                writer.WriteManyNodes(_toolSettingsNodeName, kvps, WriteKVP);
            }
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