using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// An Xml writer that creates a very basic Xml document with minimal effort.
    /// </summary>
    public class SimpleXmlWriter : IDisposable
    {
        /// <summary>
        /// Default settings to use if no settings or invalid settings are specified.
        /// </summary>
        static readonly XmlWriterSettings _defaultSettings = new XmlWriterSettings { Indent = true };

        readonly XmlWriter _w;
        bool _isDisposed = false;

        /// <summary>
        /// Gets if the SimpleXmlWriter has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// SimpleXmlWriter constructor.
        /// </summary>
        /// <param name="path">Path to the Xml file to write to.</param>
        /// <param name="rootNodeName">Root Xml node name.</param>
        public SimpleXmlWriter(string path, string rootNodeName) : this(path, rootNodeName, null)
        {
        }

        /// <summary>
        /// SimpleXmlWriter constructor.
        /// </summary>
        /// <param name="path">Path to the Xml file to write to.</param>
        /// <param name="rootNodeName">Root Xml node name.</param>
        /// <param name="settings">Settings to use for writing the Xml file.</param>
        public SimpleXmlWriter(string path, string rootNodeName, XmlWriterSettings settings)
        {
            // Use the default settings if invalid settings were specified
            if (settings == null)
                settings = _defaultSettings;

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Create the writer
            _w = XmlWriter.Create(path, settings);
            if (_w == null)
                throw new Exception("Failed to create XmlWriter.");

            _w.WriteStartDocument();
            _w.WriteStartElement(rootNodeName);
        }

        /// <summary>
        /// Writes a node and set of item names/values.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="items">Collection of items names and values to write.</param>
        public void Write(string nodeName, params NodeItem[] items)
        {
            WriteNode(nodeName, items);
        }

        /// <summary>
        /// Writes a node and set of item names/values.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="items">Collection of items names and values to write.</param>
        public void Write(string nodeName, IEnumerable<NodeItem> items)
        {
            WriteNode(nodeName, items);
        }

        /// <summary>
        /// Writes a node for an IRestorableSettings.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="obj">IRestorableSettings object to write.</param>
        public void Write(string nodeName, IRestorableSettings obj)
        {
            WriteNode(nodeName, obj.Save());
        }

        /// <summary>
        /// Writes a node and set of item names/values.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="items">Collection of items names and values to write.</param>
        void WriteNode(string nodeName, IEnumerable<NodeItem> items)
        {
            _w.WriteStartElement(nodeName);

            foreach (NodeItem value in items)
            {
                _w.WriteElementString(value.Name, value.Value);
            }

            _w.WriteEndElement();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the SimpleXmlWriter and flushes out all the Xml to the file.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            _w.WriteEndElement();
            _w.WriteEndDocument();

            _w.Flush();
            _w.Close();
        }

        #endregion
    }
}