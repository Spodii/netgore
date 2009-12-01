using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A series of joints used to define a stationary skeleton
    /// </summary>
    public class Skeleton
    {
        /// <summary>
        /// The file suffix used for the Skeleton.
        /// </summary>
        public const string FileSuffix = ".skel";

        const string _nodesNodeName = "Nodes";
        const string _rootNodeName = "SkeletonFrame";

        /// <summary>
        /// Root node of the skeleton
        /// </summary>
        SkeletonNode _rootNode = null;

        public Skeleton()
        {
        }

        public Skeleton(string skeletonName, ContentPaths contentPath)
        {
            string filePath = GetFilePath(skeletonName, contentPath);
            XmlValueReader reader = new XmlValueReader(filePath, _rootNodeName);
            Read(reader);
        }

        public Skeleton(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Gets or sets the root node of the skeleton
        /// </summary>
        public SkeletonNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        /// <summary>
        /// Recursively copies the IsModifier property of the nodes in the <paramref name="source"/> Skeleton
        /// to this Skeleton.
        /// </summary>
        /// <param name="source">Source skeleton to copy the IsModifier property values from.</param>
        public void CopyIsModifier(Skeleton source)
        {
            CopyIsModifier(source._rootNode, _rootNode);
        }

        /// <summary>
        /// Recursively copies the IsModifier property of one set of nodes to another set of nodes
        /// </summary>
        /// <param name="src">Source root SkeletonNode to copy from</param>
        /// <param name="dest">Destination root SkeletonNode to copy to</param>
        static void CopyIsModifier(SkeletonNode src, SkeletonNode dest)
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
        /// <param name="name">Name of the n to look for</param>
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
        /// <param name="name">Name of the n to look for</param>
        /// <returns>SkeletonNode with the specified name, or null if none found</returns>
        public SkeletonNode FindNode(string name)
        {
            return FindNode(RootNode, name);
        }

        /// <summary>
        /// Finds a SkeletonNode by a given name
        /// </summary>
        /// <param name="rootNode">Root n to look through</param>
        /// <param name="name">Name of the n to look for</param>
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
                // Root n matched the name
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
        /// Gets the absolute file path for a Skeleton file.
        /// </summary>
        /// <param name="skeletonName">The name of the Skeleton.</param>
        /// <param name="contentPath">The content path to use.</param>
        /// <returns>The absolute file path for a Skeleton file.</returns>
        public static string GetFilePath(string skeletonName, ContentPaths contentPath)
        {
            return contentPath.Skeletons.Join(skeletonName + FileSuffix);
        }

        /// <summary>
        /// Checks is the Skeleton is valid.
        /// </summary>
        /// <returns>True if a valid Skeleton; otherwise false.</returns>
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

        public void Read(IValueReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var nodesWithParents = new List<KeyValuePair<SkeletonNode, string>>();

            var loadedNodes = reader.ReadManyNodes(_nodesNodeName, x => SkeletonNodeReadHandler(x, nodesWithParents));

            // Add the root n (which should be the only one without a parent)
            var nodesWithoutParents = loadedNodes.Except(nodesWithParents.Select(x => x.Key));
            if (nodesWithoutParents.Count() != 1)
                throw new Exception("Invalid number of parentless nodes. Was only expected one!");

            _rootNode = nodesWithoutParents.First();

            // Set up the parents
            foreach (var nodeWithParent in nodesWithParents)
            {
                SkeletonNode node = nodeWithParent.Key;
                string parentName = nodeWithParent.Value;

                SkeletonNode parentNode =
                    loadedNodes.FirstOrDefault(x => parentName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

                if (parentNode == null)
                {
                    const string errmsg = "Unable to find parent n `{0}` for n `{1}`.";
                    string err = string.Format(errmsg, parentName, node.Name);
                    throw new Exception(err);
                }

                node.Parent = parentNode;
                parentNode.Nodes.Add(node);
            }
        }

        static SkeletonNode SkeletonNodeReadHandler(IValueReader r,
                                                    ICollection<KeyValuePair<SkeletonNode, string>> nodesWithParents)
        {
            string parentName;
            SkeletonNode node = new SkeletonNode(r, out parentName);

            if (parentName != null)
            {
                var kvp = new KeyValuePair<SkeletonNode, string>(node, parentName);
                nodesWithParents.Add(kvp);
            }

            return node;
        }

        public void Write(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            // Validate the skeleton
            if (!IsValid())
                throw new Exception("Skeleton returned false for IsValid() - unable to save!");

            // Write the file
            using (XmlValueWriter writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer);
            }
        }

        public void Write(IValueWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteManyNodes(_nodesNodeName, RootNode.GetAllNodes(), ((w, item) => item.Write(w)));
        }
    }
}