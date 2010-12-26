using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        /// The file suffix used for the <see cref="Skeleton"/> when writing to file.
        /// </summary>
        public const string FileSuffix = ".skel";

        const string _nodesNodeName = "Nodes";
        const string _rootNodeName = "SkeletonFrame";

        /// <summary>
        /// Root node of the skeleton
        /// </summary>
        SkeletonNode _rootNode = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Skeleton"/> class.
        /// </summary>
        public Skeleton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skeleton"/> class.
        /// </summary>
        /// <param name="skeletonName">Name of the skeleton.</param>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        public Skeleton(string skeletonName, ContentPaths contentPath)
        {
            var filePath = GetFilePath(skeletonName, contentPath);
            var reader = GenericValueReader.CreateFromFile(filePath, _rootNodeName);
            Read(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skeleton"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to use to load.</param>
        public Skeleton(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for when an instance of this class
        /// writes itself out to a new <see cref="GenericValueWriter"/>. If null, the format to use
        /// will be inherited from <see cref="GenericValueWriter.DefaultFormat"/>.
        /// Default value is null.
        /// </summary>
        public static GenericValueIOFormat? EncodingFormat { get; set; }

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
            for (var i = 0; i < src.Nodes.Count(); i++)
            {
                CopyIsModifier(src.Nodes.ElementAt(i), dest.Nodes.ElementAt(i));
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
            for (var i = 0; i < src.Nodes.Count(); i++)
            {
                CopyLength(src.Nodes.ElementAt(i), dest.Nodes.ElementAt(i));
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
        public static SkeletonNode FindNode(IEnumerable<SkeletonNode> nodes, string name)
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

            foreach (var node in nodes)
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
                foreach (var child in rootNode.Nodes)
                {
                    var ret = FindNode(child, name);
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
            if (nodes.Select(x => x.Name).HasDuplicates(StringComparer.Ordinal))
                return false;

            return true;
        }

        /// <summary>
        /// Reads a <see cref="Skeleton"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The loaded <see cref="Skeleton"/> was not properly structured.</exception>
        public void Read(IValueReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var nodesWithParents = new List<KeyValuePair<SkeletonNode, string>>();

            var loadedNodes = reader.ReadManyNodes(_nodesNodeName, x => SkeletonNodeReadHandler(x, nodesWithParents));

            // Add the root node (which should be the only one without a parent)
            var nodesWithoutParents = loadedNodes.Except(nodesWithParents.Select(x => x.Key));
            if (nodesWithoutParents.Count() != 1)
                throw new InvalidOperationException("Invalid number of parentless nodes. Was only expected one!");

            _rootNode = nodesWithoutParents.First();

            // Set up the parents
            foreach (var nodeWithParent in nodesWithParents)
            {
                var node = nodeWithParent.Key;
                var parentName = nodeWithParent.Value;

                var parentNode = loadedNodes.FirstOrDefault(x => parentName.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

                if (parentNode == null)
                {
                    const string errmsg = "Unable to find parent node `{0}` for node `{1}`.";
                    var err = string.Format(errmsg, parentName, node.Name);
                    throw new InvalidOperationException(err);
                }

                node.Parent = parentNode;
                parentNode.internalNodes.Add(node);
            }
        }

        static SkeletonNode SkeletonNodeReadHandler(IValueReader r,
                                                    ICollection<KeyValuePair<SkeletonNode, string>> nodesWithParents)
        {
            string parentName;
            var node = new SkeletonNode(r, out parentName);

            if (parentName != null)
            {
                var kvp = new KeyValuePair<SkeletonNode, string>(node, parentName);
                nodesWithParents.Add(kvp);
            }

            return node;
        }

        /// <summary>
        /// Writes the <see cref="Skeleton"/> to file.
        /// </summary>
        /// <param name="filePath">The file to write to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filePath" /> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Skeleton returned false for IsValid().</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IsValid")]
        public void Write(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            // Validate the skeleton
            if (!IsValid())
                throw new InvalidOperationException("Skeleton returned false for IsValid().");

            // Write the file
            using (var writer = GenericValueWriter.Create(filePath, _rootNodeName, EncodingFormat))
            {
                Write(writer);
            }
        }

        /// <summary>
        /// Writes the <see cref="Skeleton"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer" /> is <c>null</c>.</exception>
        public void Write(IValueWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.WriteManyNodes(_nodesNodeName, RootNode.GetAllNodes(), ((w, item) => item.Write(w)));
        }
    }
}