using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.IO;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// A <see cref="TreeNode"/> for the <see cref="GrhTreeView"/> that represents a folder containing one or more
    /// child nodes.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class GrhTreeViewFolderNode : TreeNode, IGrhTreeViewNode
    {
        /// <summary>
        /// The minimum amount of time that must elapse between updates of the ToolTipText.
        /// </summary>
        const int _minUpdateToolTipRate = 5000;

        static readonly Image _closedFolder;
        static readonly Image _openFolder;
        readonly string _subCategory;

        /// <summary>
        /// The ToolTipText will not be updated if the current time is less than this value.
        /// </summary>
        TickCount _nextUpdateToolTipTime = TickCount.MinValue;

        /// <summary>
        /// Initializes the <see cref="GrhTreeViewFolderNode"/> class.
        /// </summary>
        static GrhTreeViewFolderNode()
        {
            _closedFolder = GrhImageList.ClosedFolder;
            _openFolder = GrhImageList.OpenFolder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeViewFolderNode"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="subCategory">The sub category.</param>
        GrhTreeViewFolderNode(TreeNodeCollection parent, string subCategory)
        {
            _subCategory = subCategory;
            Name = FullCategory;
            parent.Add(this);
            Text = SubCategory;
        }

        /// <summary>
        /// Gets the full category name of this <see cref="GrhTreeViewFolderNode"/> represents.
        /// </summary>
        public string FullCategory
        {
            get
            {
                if (Parent == null)
                    return SubCategory;
                else
                    return Parent.FullPath + SpriteCategorization.Delimiter + SubCategory;
            }
        }

        /// <summary>
        /// Gets the subcategory name of the category this <see cref="GrhTreeViewFolderNode"/> represents.
        /// </summary>
        public string SubCategory
        {
            get { return _subCategory; }
        }

        /// <summary>
        /// Creates a <see cref="GrhTreeViewFolderNode"/>.
        /// </summary>
        /// <param name="treeView">The <see cref="TreeView"/> to add the node to.</param>
        /// <param name="category">The category for the node.</param>
        /// <returns>The <see cref="GrhTreeViewFolderNode"/> instance.</returns>
        public static GrhTreeViewFolderNode Create(TreeView treeView, string category)
        {
            var delimiters = new string[] { SpriteCategorization.Delimiter };
            var categoryParts = category.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            GrhTreeViewFolderNode current = null;
            var currentColl = treeView.Nodes;

            for (var i = 0; i < categoryParts.Length; i++)
            {
                var subCategory = categoryParts[i];
                var matches = currentColl.OfType<GrhTreeViewFolderNode>().Where(x => x.SubCategory == subCategory);

                var count = matches.Count();
                if (count == 0)
                {
                    // Create the new folder node for the subcategory
                    current = new GrhTreeViewFolderNode(currentColl, subCategory);
                }
                else if (count == 1)
                {
                    // Use the found match
                    current = matches.First();
                }
                else
                {
                    // Uhm... too many matches?
                    const string errmsg = "Somehow we have more than one node for a single category (`{0}`).";
                    Debug.Fail(string.Format(errmsg, category));
                    return matches.FirstOrDefault();
                }

                currentColl = current.Nodes;
            }

            return current;
        }

        static IEnumerable<TreeNode> GetAllChildren(TreeNode root)
        {
            foreach (var child in root.Nodes.OfType<TreeNode>())
            {
                yield return child;

                foreach (var child2 in GetAllChildren(child))
                {
                    yield return child2;
                }
            }
        }

        public IEnumerable<GrhTreeViewNode> GetChildGrhDataNodes(bool recursive)
        {
            foreach (var node in Nodes.OfType<GrhTreeViewNode>())
            {
                yield return node;
            }

            if (recursive)
            {
                foreach (var folderNode in Nodes.OfType<GrhTreeViewFolderNode>())
                {
                    foreach (var node2 in folderNode.GetChildGrhDataNodes(true))
                    {
                        yield return node2;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the tooltip text to use for a folder node.
        /// </summary>
        /// <returns>The tooltip text to use for a folder node.</returns>
        string GetToolTipText()
        {
            // Count the immediate sub-categories and grhs
            var subCategories = Nodes.OfType<GrhTreeViewFolderNode>().Count();
            var grhs = Nodes.OfType<GrhTreeViewNode>().Count();

            // Count the total number of sub-categories and grhs
            var totalGrhs = 0;
            var totalSubCategories = 0;
            foreach (var child in GetAllChildren(this))
            {
                if (child is GrhTreeViewNode)
                    totalGrhs++;
                else if (child is GrhTreeViewFolderNode)
                    totalSubCategories++;
            }

            // Create the string
            var sb = new StringBuilder();
            sb.Append("Category: ");
            sb.AppendLine(FullCategory);

            sb.Append("Sub-categories: ");
            sb.Append(subCategories);
            sb.Append(" [");
            sb.Append(totalSubCategories);
            sb.AppendLine(" total]");

            sb.Append("Grhs: ");
            sb.Append(grhs);
            sb.Append(" [");
            sb.Append(totalGrhs);
            sb.AppendLine(" total]");

            return sb.ToString();
        }

        /// <summary>
        /// Removes the <see cref="GrhTreeViewFolderNode"/> only if it is empty.
        /// </summary>
        public void RemoveIfEmpty()
        {
            if (Nodes.Count != 0)
                return;

            var parent = Parent as GrhTreeViewFolderNode;
            Remove();
            if (parent != null)
                parent.RemoveIfEmpty();
        }

        /// <summary>
        /// Updates the tooltip text for this node. The text will only update if a certain amount of time has elapsed
        /// since the last attempt to update, so there is no harm in calling this method excessively.
        /// </summary>
        public void UpdateToolTip()
        {
            var time = TickCount.Now;
            if (_nextUpdateToolTipTime > time)
                return;

            _nextUpdateToolTipTime = time + _minUpdateToolTipRate;

            ToolTipText = GetToolTipText();
        }

        #region IGrhTreeViewNode Members

        /// <summary>
        /// Gets the <see cref="Image"/> to use to draw the node.
        /// </summary>
        public Image Image
        {
            get
            {
                if (IsExpanded)
                    return _openFolder;
                else
                    return _closedFolder;
            }
        }

        #endregion
    }
}