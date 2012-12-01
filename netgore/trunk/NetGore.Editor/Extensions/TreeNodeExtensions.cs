using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor
{
    /// <summary>
    /// Extensions for the TreeNode.
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Gets the 0-based depth of the given TreeNode.
        /// </summary>
        /// <param name="node">The TreeNode to get the depth of.</param>
        /// <returns>The depth of the <paramref name="node"/>, where 0 means this is the root node and it has
        /// no parent nodes.</returns>
        public static int GetDepth(this TreeNode node)
        {
            var depth = 0;
            while (node.Parent != null)
            {
                depth++;
                node = node.Parent;
            }

            return depth;
        }

        /// <summary>
        /// Gets an IEnumerable of all the parent TreeNodes for the given <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The TreeNode to find the parents for.</param>
        /// <returns>An IEnumerable of all the parent TreeNodes for the given <paramref name="node"/>.</returns>
        public static IEnumerable<TreeNode> GetParents(this TreeNode node)
        {
            var current = node;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the TreeNodes that share the same Parent and are on the same depth
        /// as the given <paramref name="node"/>. That is, all TreeNodes that this <paramref name="node"/>
        /// is immediately grouped with.
        /// </summary>
        /// <param name="node">The TreeNode to fid the sister nodes for.</param>
        /// <returns>The sister nodes for the given <paramref name="node"/>, but not the <paramref name="node"/>
        /// itself.</returns>
        public static IEnumerable<TreeNode> GetSisterNodes(this TreeNode node)
        {
            IEnumerable<TreeNode> sisterNodes;

            if (node.Parent != null)
                sisterNodes = node.Parent.Nodes.Cast<TreeNode>();
            else
                sisterNodes = node.TreeView.Nodes.Cast<TreeNode>();

            Debug.Assert(sisterNodes.Contains(node));

            return sisterNodes.Where(x => x != node);
        }

        public static IEnumerable<TreeNode> GetChildren(this TreeNode root)
        {
            if (root != null)
            {
                foreach (var c in root.Nodes.OfType<TreeNode>())
                {
                    yield return c;
                    foreach (var c2 in GetChildren(c))
                        yield return c2;
                }
            }
        }

        /// <summary>
        /// Swaps this TreeNode with another TreeNode.
        /// </summary>
        /// <param name="node">This TreeNode.</param>
        /// <param name="other">The TreeNode to swap with.</param>
        /// <param name="swapChildren">If true, the child nodes are also swapped.</param>
        public static void SwapNode(this TreeNode node, TreeNode other, bool swapChildren)
        {
            var a = node;
            var b = other;

            var aParentNodes = a.Parent != null ? a.Parent.Nodes : a.TreeView.Nodes;
            var bParentNodes = b.Parent != null ? b.Parent.Nodes : b.TreeView.Nodes;

            var aChildNodes = a.Nodes.ToArray<TreeNode>();
            var bChildNodes = b.Nodes.ToArray<TreeNode>();

            a.Nodes.Clear();
            b.Nodes.Clear();

            a.Remove();
            b.Remove();

            // Swap child nodes
            if (swapChildren)
            {
                a.Nodes.AddRange(bChildNodes);
                b.Nodes.AddRange(aChildNodes);
            }
            else
            {
                a.Nodes.AddRange(aChildNodes);
                b.Nodes.AddRange(bChildNodes);
            }

            // Swap parents
            aParentNodes.Add(b);
            bParentNodes.Add(a);
        }
    }
}