using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace NetGore
{
    /// <summary>
    /// Assists in reading Xml files by creating a period-delimited dictionary for all
    /// attributes and values.
    /// </summary>
    public static class XmlInfoReader
    {
        static readonly StringComparer _stringComparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// Combines two dictionaries together.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="dest">Dictionary to add the values to.</param>
        /// <param name="source">Dictionary to get the values from.</param>
        static void CombineDictionaries<TKey, TValue>(IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
        {
            if (dest == null || source == null || source.Count <= 0)
                return;

            foreach (TKey key in source.Keys)
            {
                dest.Add(key, source[key]);
            }
        }

        /// <summary>
        /// Parses a XML file and returns a single-entry list of the base elements containing a
        /// dictionary holding the attribute values of the elements found.
        /// </summary>
        /// <param name="filePath">Path to file to parse.</param>
        public static List<Dictionary<string, string>> ReadFile(string filePath)
        {
            return ReadFile(filePath, false);
        }

        /// <summary>
        /// Parses a XML file and returns a list of the base elements containing a
        /// dictionary holding the attribute values of the elements found.
        /// </summary>
        /// <param name="filePath">Path to file to parse.</param>
        /// <param name="splitList">If true, splits all of the 2nd level nodes into individual list entries. This
        /// should be used when each Xml element directly under the root do not all have unique names.</param>
        public static List<Dictionary<string, string>> ReadFile(string filePath, bool splitList)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("Invalid file specified.", "filePath");

            var ret = new List<Dictionary<string, string>>(1);

            // Load up the xml document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            if (xmlDoc.DocumentElement == null)
                throw new Exception("XmlDocument contains no DocumentElements.");

            if (splitList)
            {
                // There is a delimiter, so split up the 2nd level nodes and add each one to a different list
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    // Get the list as normal
                    var dict = RecursiveReadNodes(node.ChildNodes);

                    // Ensure we grab attributes from the parent if they exist
                    CombineDictionaries(dict, ResolveAttributes(node, string.Empty));

                    // Add the dictionary to the list
                    ret.Add(dict);
                }
            }
            else
            {
                // No delimiter, just add everything to one list
                ret.Add(RecursiveReadNodes(xmlDoc.DocumentElement.ChildNodes));
            }

            return ret;
        }

        /// <summary>
        /// Reads an Xml node and all the child Xml nodes of the node all
        /// the way down to the last child of every child.
        /// </summary>
        /// <param name="nodeList">Root node to read (normally the root Xml file node).</param>
        /// <returns>A dictionary of all the found values.</returns>
        public static Dictionary<string, string> RecursiveReadNodes(XmlNodeList nodeList)
        {
            if (nodeList == null)
                throw new ArgumentNullException("nodeList", "Parameter nodeList is null.");

            var ret = new Dictionary<string, string>(_stringComparer);

            // Read through every node in the list
            foreach (XmlNode node in nodeList)
            {
                // Create the parent list (only needed if theres any attributes or no children)
                string parentList = string.Empty;
                if (node.Attributes != null || node.ChildNodes.Count == 0)
                    parentList = ResolveParentList(node);

                // Read all the attributes (if any)
                CombineDictionaries(ret, ResolveAttributes(node, parentList));

                if (node.HasChildNodes)
                {
                    // Read all the child nodes of the node
                    var childRet = RecursiveReadNodes(node.ChildNodes);
                    if (childRet.Count > 0)
                    {
                        foreach (string key in childRet.Keys)
                        {
                            ret.Add(key, childRet[key]);
                        }
                    }
                }
                else
                {
                    // Read the inner text of the node
                    if (node.InnerText != "")
                        ret.Add(parentList, node.InnerText);
                }
            }

            return ret;
        }

        /// <summary>
        /// Resolves all the attributes in a given node.
        /// </summary>
        /// <param name="node">Node to check.</param>
        /// <param name="parentList">Parent list of the node.</param>
        /// <returns>Dictionary containing the attributes and their values.</returns>
        static Dictionary<string, string> ResolveAttributes(XmlNode node, string parentList)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (parentList == null)
                throw new ArgumentNullException("parentList");

            // Check if the node even contains attributes
            if (node.Attributes == null)
                return null;

            var ret = new Dictionary<string, string>(_stringComparer);

            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.OwnerElement == null)
                    continue;

                // With a parent list length > 0, we must add the "." after the list
                if (parentList.Length > 0)
                    ret.Add(parentList + "." + attr.OwnerElement.Name + "." + attr.Name, attr.InnerText);
                else
                    ret.Add(attr.OwnerElement.Name + "." + attr.Name, attr.InnerText);
            }

            return ret;
        }

        /// <summary>
        /// Returns a formatted list of all the parents in the Xml node.
        /// </summary>
        /// <param name="node">Node to check.</param>
        static string ResolveParentList(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (node.ParentNode == null)
                throw new ArgumentException("XmlNode's parent may not be null", "node");

            string ret = string.Empty;

            // Create the parent list string
            while (node.ParentNode.ParentNode != null && node.ParentNode.ParentNode.NodeType != XmlNodeType.Document)
            {
                ret = node.ParentNode.Name + "." + ret;
                node = node.ParentNode;
            }

            // If the string is not empty, crop off the last character
            if (ret.Length > 0)
                return ret.Substring(0, ret.Length - 1);
            else
                return ret;
        }
    }
}