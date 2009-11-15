using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="TreeNode"/> for the <see cref="GrhTreeView"/> that represents a folder containing one or more
    /// child nodes.
    /// </summary>
    public class GrhTreeViewFolderNode : TreeNode
    {
        readonly string _subCategory;

        GrhTreeViewFolderNode(TreeNodeCollection parent, string subCategory)
        {
            _subCategory = subCategory;
            Name = FullCategory;
            parent.Add(this);
            Text = SubCategory;

            // Set the images
            ImageKey = GrhImageList.ClosedFolderKey;
            SelectedImageKey = GrhImageList.OpenFolderKey;
            StateImageKey = GrhImageList.ClosedFolderKey;
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
                    return Parent.FullPath + GrhDataCategorization.Delimiter + SubCategory;
            }
        }

        /// <summary>
        /// Gets the subcategory name of the category this <see cref="GrhTreeViewFolderNode"/> represents.
        public string SubCategory
        {
            get { return _subCategory; }
        }

        public static GrhTreeViewFolderNode Create(GrhTreeView grhTreeView, string category)
        {
            var delimiters = new string[] { GrhDataCategorization.Delimiter };
            var categoryParts = category.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            GrhTreeViewFolderNode current = null;
            var currentColl = grhTreeView.Nodes;

            for (int i = 0; i < categoryParts.Length; i++)
            {
                string subCategory = categoryParts[i];
                var matches = currentColl.OfType<GrhTreeViewFolderNode>().Where(x => x.SubCategory == subCategory);

                int count = matches.Count();
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
                    throw new Exception("Somehow we have more than one node for a single category!");
                }

                currentColl = current.Nodes;
            }

            return current;
        }

        public IEnumerable<GrhTreeViewNode> GetChildGrhDataNodes(bool recursive)
        {
            foreach (var node in Nodes.OfType<GrhTreeViewNode>())
            {
                yield return node;

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
    }
}