using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// An Xml reader for reading Xml created by the SimpleXmlWriter.
    /// </summary>
    public class SimpleXmlReader
    {
        readonly List<NodeItems> _items = new List<NodeItems>();

        readonly string _rootNode;

        /// <summary>
        /// IEnumerable of the items read.
        /// </summary>
        public IEnumerable<NodeItems> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the name of the root node.
        /// </summary>
        public string RootNodeName
        {
            get { return _rootNode; }
        }

        /// <summary>
        /// SimpleXmlReader constructor
        /// </summary>
        /// <param name="path">Path to the Xml file to read.</param>
        public SimpleXmlReader(string path)
        {
            XmlReader r = null;

            try
            {
                r = XmlReader.Create(path);

                // Read past the initial element
                while (r.Read() && r.NodeType != XmlNodeType.Element)
                {
                }
                _rootNode = r.Name;

                // Read through the Xml
                while (r.Read())
                {
                    if (r.NodeType != XmlNodeType.Element)
                        continue;

                    // Store the node name and depth
                    string nodeName = r.Name;
                    int depth = r.Depth;

                    // Read all the nodes under this node and store the values
                    var pairs = new List<NodeItem>();
                    while (r.Read() && r.Depth > depth)
                    {
                        if (r.NodeType != XmlNodeType.Element)
                            continue;

                        string name = r.Name;
                        string value = r.ReadElementContentAsString();
                        pairs.Add(new NodeItem(name, value));
                    }

                    // Add to the items list
                    _items.Add(new NodeItems(nodeName, pairs));
                }
            }
            finally
            {
                if (r != null)
                    r.Close();
            }
        }

        /// <summary>
        /// Gets the root Xml node name from the specified Xml file.
        /// </summary>
        /// <param name="filePath">Path to the file to get the root Xml node name of.</param>
        /// <returns>Root Xml node name from the specified file, or null if the file was not found or
        /// an invalid Xml file.</returns>
        public static string GetRootNodeName(string filePath)
        {
            string ret;

            XmlReader r = null;
            try
            {
                r = XmlReader.Create(filePath);

                while (r.Read() && r.NodeType != XmlNodeType.Element)
                {
                }

                ret = r.Name;
            }
            catch (Exception)
            {
                ret = null;
            }
            finally
            {
                if (r != null)
                    r.Close();
            }

            return ret;
        }
    }
}