using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics
{
    /// <summary>
    /// A series of joints used to define a stationary skeleton
    /// </summary>
    public class Skeleton
    {
        /// <summary>
        /// Root node of the skeleton
        /// </summary>
        SkeletonNode _rootNode = null;

        /// <summary>
        /// Gets or sets the root node of the skeleton
        /// </summary>
        public SkeletonNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        /// <summary>
        /// Recursively copies the IsModifier property of one set of nodes to another set of nodes
        /// </summary>
        /// <param name="src">Source skeleton to copy from</param>
        /// <param name="dest">Destination skeleton to copy to</param>
        public static void CopyIsModifier(Skeleton src, Skeleton dest)
        {
            CopyIsModifier(src._rootNode, dest._rootNode);
        }

        /// <summary>
        /// Recursively copies the IsModifier property of one set of nodes to another set of nodes
        /// </summary>
        /// <param name="src">Source root SkeletonNode to copy from</param>
        /// <param name="dest">Destination root SkeletonNode to copy to</param>
        public static void CopyIsModifier(SkeletonNode src, SkeletonNode dest)
        {
            if (src == null)
            {
                Debug.Fail("src is null.");
                return;
            }
            if (dest == null)
            {
                Debug.Fail("dest is null.");
                return;
            }

            dest.IsModifier = src.IsModifier;
            for (int i = 0; i < src.Nodes.Count; i++)
            {
                CopyIsModifier(src.Nodes[i], dest.Nodes[i]);
            }
        }

        /// <summary>
        /// Recursively copies the length of one set of nodes to another set of nodes
        /// </summary>
        /// <param name="src">Source skeleton to copy from</param>
        /// <param name="dest">Destination skeleton to copy to</param>
        public static void CopyLength(Skeleton src, Skeleton dest)
        {
            if (src == null)
            {
                Debug.Fail("src is null.");
                return;
            }
            if (dest == null)
            {
                Debug.Fail("dest is null.");
                return;
            }

            CopyIsModifier(src._rootNode, dest._rootNode);
        }

        /// <summary>
        /// Recursively copies the length of one set of nodes to another set of nodes
        /// </summary>
        /// <param name="src">Source root SkeletonNode to copy from</param>
        /// <param name="dest">Destination root SkeletonNode to copy to</param>
        public static void CopyLength(SkeletonNode src, SkeletonNode dest)
        {
            if (src == null)
            {
                Debug.Fail("src is null.");
                return;
            }
            if (dest == null)
            {
                Debug.Fail("dest is null.");
                return;
            }

            dest.SetLength(src.GetLength());
            for (int i = 0; i < src.Nodes.Count; i++)
            {
                CopyIsModifier(src.Nodes[i], dest.Nodes[i]);
            }
        }

        /// <summary>
        /// Creates a deep-copy of the skeleton and all its joints
        /// </summary>
        /// <returns>Deep-copy of the skeleton and all its joints</returns>
        public Skeleton DeepCopy()
        {
            return new Skeleton { RootNode = RootNode.Duplicate() };
        }

        /// <summary>
        /// Finds a SkeletonNode by a given name
        /// </summary>
        /// <param name="name">Name of the node to look for</param>
        /// <param name="nodes">List of SkeletonNodes to search through</param>
        /// <returns>SkeletonNode with the specified name, or null if none found</returns>
        public static SkeletonNode FindNode(List<SkeletonNode> nodes, string name)
        {
            if (nodes == null)
            {
                Debug.Fail("nodes is null.");
                return null;
            }
            if (name == null)
            {
                Debug.Fail("name is null.");
                return null;
            }

            foreach (SkeletonNode node in nodes)
            {
                if (node.Name == name)
                    return node;
            }
            return null;
        }

        /// <summary>
        /// Finds a SkeletonNode by a given name
        /// </summary>
        /// <param name="name">Name of the node to look for</param>
        /// <returns>SkeletonNode with the specified name, or null if none found</returns>
        public SkeletonNode FindNode(string name)
        {
            return FindNode(RootNode, name);
        }

        /// <summary>
        /// Finds a SkeletonNode by a given name
        /// </summary>
        /// <param name="rootNode">Root node to look through</param>
        /// <param name="name">Name of the node to look for</param>
        /// <returns>SkeletonNode with the specified name, or null if none found</returns>
        static SkeletonNode FindNode(SkeletonNode rootNode, string name)
        {
            if (rootNode == null)
            {
                Debug.Fail("rootNode is null.");
                return null;
            }
            if (name == null)
            {
                Debug.Fail("name is null.");
                return null;
            }

            if (rootNode.Name == name)
            {
                // Root node matched the name
                return rootNode;
            }
            else
            {
                // Check the child nodes for a name match
                foreach (SkeletonNode child in rootNode.Nodes)
                {
                    SkeletonNode ret = FindNode(child, name);
                    if (ret != null)
                        return ret;
                }
            }

            // Node with the given name not found
            return null;
        }

        /// <summary>
        /// Checks is a skeleton is valid
        /// </summary>
        /// <returns>True if a valid skeleton, else false</returns>
        public bool IsValid()
        {
            var nodes = RootNode.GetAllNodes();

            // Check for duplicate names
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    if (nodes[i].Name == nodes[j].Name)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Loads a skeleton from a file
        /// </summary>
        /// <param name="filePath">Path to the file to load</param>
        /// <returns>New skeleton object</returns>
        public static Skeleton Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("File not found.", "filePath");

            Skeleton ret = new Skeleton();
            var nodes = new List<SkeletonNode>();

            var fileInfo = XmlInfoReader.ReadFile(filePath, true);

            // Load all the node information except for the parent/child references
            foreach (var dic in fileInfo)
            {
                if (dic.ContainsKey("Nodes"))
                    continue;

                string name = dic["Node.Name"];
                float x = float.Parse(dic["Node.X"]);
                float y = float.Parse(dic["Node.Y"]);
                bool isMod = dic.ContainsKey("Node.IsModifier") && bool.Parse(dic["Node.IsModifier"]);

                SkeletonNode node = new SkeletonNode(new Vector2(x, y));
                nodes.Add(node);
                node.Name = name;
                node.IsModifier = isMod;
            }

            // Now that we have all node objects created, we can properly link the references
            foreach (var dic in fileInfo)
            {
                if (dic.ContainsKey("Nodes"))
                    continue;

                // Find the node this block is for
                SkeletonNode node = FindNode(nodes, dic["Node.Name"]);

                // Check for no parent which means this is the root
                if (!dic.ContainsKey("Node.ParentName"))
                {
                    // This is the root node
                    node.Parent = null;
                    ret.RootNode = node;
                }
                else
                {
                    // Find the parent node by its name
                    SkeletonNode parent = FindNode(nodes, dic["Node.ParentName"]);
                    if (parent == null)
                        throw new Exception("Unable to find parent node with name '" + dic["Node.ParentName"] + "'.");

                    // Set the references
                    node.Parent = parent;
                    parent.Nodes.Add(node);
                }
            }

            return ret;
        }

        /// <summary>
        /// Saves the skeleton to a file
        /// </summary>
        /// <param name="skeleton">Skeleton to save</param>
        /// <param name="filePath">Path to the file to save</param>
        public static void Save(Skeleton skeleton, string filePath)
        {
            if (skeleton == null)
                throw new ArgumentNullException("skeleton");
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            // Validate the skeleton
            if (!skeleton.IsValid())
                throw new Exception("Skeleton returned false for IsValid() - unable to save!");

            // Save the file
            using (Stream s = File.Open(filePath, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter w = XmlWriter.Create(s, settings))
                {
                    skeleton.Save(w);
                }
            }
        }

        /// <summary>
        /// Saves the skeleton to a stream with a binary writer
        /// </summary>
        /// <param name="skeleton">Skeleton to save</param>
        /// <param name="w">XmlWriter to write to</param>
        public static void Save(Skeleton skeleton, XmlWriter w)
        {
            if (skeleton == null)
                throw new ArgumentNullException("skeleton");
            if (w == null)
                throw new ArgumentNullException("w");
            if (w.WriteState == WriteState.Closed || w.WriteState == WriteState.Error)
                throw new Exception("XmlWriter w is in an invalid WriteState.");

            var nodes = skeleton.RootNode.GetAllNodes();
            w.WriteStartElement("Skeleton");

            // Save the node information of each node
            foreach (SkeletonNode node in nodes)
            {
                w.WriteStartElement("Node");
                w.WriteElementString("Name", node.Name);
                w.WriteElementString("X", node.Position.X.ToString());
                w.WriteElementString("Y", node.Position.Y.ToString());
                if (node.IsModifier)
                    w.WriteElementString("IsModifier", true.ToString());
                if (node.Parent != null)
                    w.WriteElementString("ParentName", node.Parent.Name);
                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        /// <summary>
        /// Saves the skeleton to a file
        /// </summary>
        /// <param name="filePath">Path to the file to save</param>
        public void Save(string filePath)
        {
            Save(this, filePath);
        }

        /// <summary>
        /// Saves the skeleton to a stream with a binary writer
        /// </summary>
        /// <param name="w">XmlWriter to write to</param>
        public void Save(XmlWriter w)
        {
            Save(this, w);
        }
    }
}