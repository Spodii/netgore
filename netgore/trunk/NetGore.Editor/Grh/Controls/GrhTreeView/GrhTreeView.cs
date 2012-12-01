using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// TreeView used to display Grhs
    /// </summary>
    public class GrhTreeView : TreeView, IComparer, IComparer<TreeNode>
    {
        /// <summary>
        /// If the EditGrhForm is enabled for individual Grhs. Default is false, and we instead rely on tags in the file names for graphics.
        /// </summary>
        public const bool EnableGrhEditor = false;

        const int _drawImageOffset = 2;

        static readonly IComparer<string> _nodeTextComparer = NaturalStringComparer.Instance;

        string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter == value)
                    return;

                _filter = value;
                RebuildTree();
            }
        }

        /// <summary>
        /// Timer to update the animated <see cref="Grh"/>s in the <see cref="GrhTreeView"/>.
        /// </summary>
        readonly Timer _animTimer = new Timer();

        readonly ContextMenu _contextMenu = new ContextMenu();

        bool _compactMode = true;
        IContentManager _contentManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhTreeView"/> class.
        /// </summary>
        public GrhTreeView()
        {
            ImageList = new ImageList { ImageSize = new Size(GrhImageList.ImageWidth, GrhImageList.ImageHeight) };
            DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        /// <summary>
        /// Notifies listeners when a GrhData is to be edited.
        /// </summary>
        public event TypedEventHandler<GrhTreeView, GrhTreeViewEditGrhDataEventArgs> EditGrhDataRequested;

        /// <summary>
        /// Notofies listeners after a new <see cref="GrhData"/> node is selected.
        /// </summary>
        public event EventHandler<GrhTreeViewEventArgs> GrhAfterSelect;

        /// <summary>
        /// Notifies listeners before a new <see cref="GrhData"/> node is selected.
        /// </summary>
        public event EventHandler<GrhTreeViewCancelEventArgs> GrhBeforeSelect;

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> node is clicked.
        /// </summary>
        public event EventHandler<GrhTreeNodeMouseClickEventArgs> GrhMouseClick;

        /// <summary>
        /// Notifies listeners when a <see cref="GrhData"/> node is double-clicked.
        /// </summary>
        public event EventHandler<GrhTreeNodeMouseClickEventArgs> GrhMouseDoubleClick;

        /// <summary>
        /// Adds a <see cref="GrhData"/> to the tree or updates it if it already exists.
        /// </summary>
        /// <param name="gd"><see cref="GrhData"/> to add or update.</param>
        void AddGrhToTree(GrhData gd)
        {
            GrhTreeViewNode.Create(this, gd);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> containing the <see cref="GrhData"/> to edit.</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        public bool BeginEditGrhData(TreeNode node)
        {
            if (_compactMode)
                return false;

            var gd = GetGrhData(node);
            return BeginEditGrhData(node, gd, false);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        public bool BeginEditGrhData(GrhData gd)
        {
            if (_compactMode)
                return false;

            return BeginEditGrhData(gd, false);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="deleteOnCancel">If true, the <paramref name="gd"/> will be deleted if the edit form is
        /// closed by pressing "Cancel".</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        bool BeginEditGrhData(GrhData gd, bool deleteOnCancel)
        {
            var node = FindGrhDataNode(gd);
            return BeginEditGrhData(node, gd, deleteOnCancel);
        }

        /// <summary>
        /// Attempts to begin the editing of a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> containing the <see cref="GrhData"/> to edit.</param>
        /// <param name="gd">The <see cref="GrhData"/> to edit.</param>
        /// <param name="deleteOnCancel">If true, the <paramref name="gd"/> will be deleted if the edit form is
        /// closed by pressing "Cancel".</param>
        /// <returns>True if the editing started successfully; otherwise false.</returns>
        bool BeginEditGrhData(TreeNode node, GrhData gd, bool deleteOnCancel)
        {
            if (node == null || gd == null)
                return false;

            if (EditGrhDataRequested != null)
                EditGrhDataRequested.Raise(this, new GrhTreeViewEditGrhDataEventArgs(node, gd, deleteOnCancel));

            return true;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.TreeView"/> and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to
        /// release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GrhInfo.Removed -= GrhInfo_Removed;

                try
                {
                    if (_animTimer != null)
                    {
                        _animTimer.Stop();
                        _animTimer.Dispose();
                    }

                    if (_contextMenu != null)
                        _contextMenu.Dispose();

                    Nodes.Clear();
                }
                catch
                {
                }
            }

            base.Dispose(disposing);
        }

        void DuplicateGrhDataNode(GrhTreeViewNode node, string oldCategoryStart, string newCategoryStart)
        {
            var gd = node.GrhData;
            if (gd is AutomaticAnimatedGrhData)
                return;

            // Replace the start of the categorization to the new category
            var newCategory = newCategoryStart;
            if (gd.Categorization.Category.ToString().Length > oldCategoryStart.Length)
                newCategory += gd.Categorization.Category.ToString().Substring(oldCategoryStart.Length);

            // Grab the new title
            var newTitle = GrhInfo.GetUniqueTitle(newCategory, gd.Categorization.Title);

            // Duplicate
            var newGrhData = gd.Duplicate(new SpriteCategorization(newCategory, newTitle));

            // Add the new one to the tree
            UpdateGrhData(newGrhData);
        }

        void DuplicateNode(TreeNode node, string oldCategoryStart, string newCategoryStart)
        {
            if (node is GrhTreeViewNode)
                DuplicateGrhDataNode((GrhTreeViewNode)node, oldCategoryStart, newCategoryStart);
            else if (node is GrhTreeViewFolderNode)
            {
                var grhDataNodes = ((GrhTreeViewFolderNode)node).GetChildGrhDataNodes(true).ToImmutable();

                foreach (var child in grhDataNodes)
                {
                    DuplicateGrhDataNode(child, oldCategoryStart, newCategoryStart);
                }
            }
        }

        /// <summary>
        /// Creates a duplicate of the tree nodes and structure from the root, giving it a unique name. All GrhDatas
        /// under the node to be duplicated will be duplicated and placed in a new category with a new GrhIndex,
        /// but with the same name and structure.
        /// </summary>
        /// <param name="root">Root <see cref="TreeNode"/> to duplicate from.</param>
        public void DuplicateNodes(TreeNode root)
        {
            SpriteCategory category;
            SpriteCategory newCategory;

            if (root is GrhTreeViewNode)
            {
                category = ((GrhTreeViewNode)root).GrhData.Categorization.Category;
                newCategory = category;
            }
            else
            {
                category = ((GrhTreeViewFolderNode)root).FullCategory;
                newCategory = GrhInfo.GetUniqueCategory(category);
            }

            DuplicateNode(root, category.ToString(), newCategory.ToString());
        }

        public GrhTreeViewFolderNode FindFolder(string category)
        {
            var delimiters = new string[] { SpriteCategorization.Delimiter };
            var categoryParts = category.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            GrhTreeViewFolderNode current = null;
            var currentColl = Nodes;

            for (var i = 0; i < categoryParts.Length; i++)
            {
                var subCategory = categoryParts[i];
                current = currentColl.OfType<GrhTreeViewFolderNode>().FirstOrDefault(x => x.SubCategory == subCategory);

                if (current == null)
                    return null;

                currentColl = current.Nodes;
            }

            return current;
        }

        /// <summary>
        /// Finds the <see cref="GrhTreeViewNode"/> for the given <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to find the <see cref="GrhTreeViewNode"/> for.</param>
        /// <returns>The <see cref="GrhTreeViewNode"/> that is for the given <paramref name="grhData"/>, or null
        /// if invalid or not found.</returns>
        public GrhTreeViewNode FindGrhDataNode(GrhData grhData)
        {
            if (grhData == null)
                return null;

            var folder = FindFolder(grhData.Categorization.Category.ToString());
            if (folder != null)
            {
                var existingNode = folder.Nodes.OfType<GrhTreeViewNode>().FirstOrDefault(x => x.GrhData == grhData);
                if (existingNode != null)
                    return existingNode;
            }

            return null;
        }

        /// <summary>
        /// Gets the category to use from the given <paramref name="node"/>.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to get the category from.</param>
        /// <returns>The category to use from the given <paramref name="node"/>.</returns>
        public static SpriteCategory GetCategoryFromTreeNode(TreeNode node)
        {
            // Check for a valid node
            if (node != null)
            {
                if (node is GrhTreeViewNode)
                    return ((GrhTreeViewNode)node).GrhData.Categorization.Category;
                else if (node is GrhTreeViewFolderNode)
                    return ((GrhTreeViewFolderNode)node).FullCategory;
            }

            return "Uncategorized";
        }

        /// <summary>
        /// Finds the <see cref="GrhData"/> for a given <see cref="TreeNode"/>.
        /// </summary>
        /// <param name="node"><see cref="TreeNode"/> to get the <see cref="GrhData"/> from.</param>
        /// <returns><see cref="GrhData"/> for the <see cref="TreeNode"/>, or null if invalid.</returns>
        public static GrhData GetGrhData(TreeNode node)
        {
            var casted = node as GrhTreeViewNode;
            if (casted != null)
                return casted.GrhData;

            return null;
        }

        /// <summary>
        /// Gets all of the visible <see cref="GrhTreeViewNode"/>s in this <see cref="GrhTreeView"/> that are animated.
        /// </summary>
        /// <param name="root">The root node.</param>
        /// <returns>All of the visible <see cref="GrhTreeViewNode"/>s in this <see cref="GrhTreeView"/> that are animated.</returns>
        static IEnumerable<GrhTreeViewNode> GetVisibleAnimatedGrhTreeViewNodes(IEnumerable root)
        {
            foreach (var node in root.OfType<TreeNode>())
            {
                if (!node.IsVisible)
                    continue;

                var asGrhTreeViewNode = node as GrhTreeViewNode;

                if (asGrhTreeViewNode != null && asGrhTreeViewNode.NeedsToUpdateImage)
                {
                    // Return the node
                    yield return asGrhTreeViewNode;
                }
                else if (node.Nodes.Count > 0 && node.IsExpanded)
                {
                    // Recursively add child nodes of this node if it is expanded
                    foreach (var child in GetVisibleAnimatedGrhTreeViewNodes(node.Nodes))
                    {
                        yield return child;
                    }
                }
            }
        }

        void GrhInfo_Removed(GrhData sender, EventArgs e)
        {
            var node = FindGrhDataNode(sender);
            if (node != null)
                node.Update();
        }

        void GrhTreeView_GrhMouseDoubleClick(object sender, GrhTreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !(e.GrhData is AutomaticAnimatedGrhData))
                BeginEditGrhData(e.Node);
        }

        void HighlightFolder(TreeNode root, TreeNode folder)
        {
            if (folder != null && (root == folder || (root.Parent == folder && IsGrhDataNode(root))))
            {
                // Highlight the folder, the Grhs in the folder, but not the folders in the folder
                root.ForeColor = SystemColors.HighlightText;
                root.BackColor = SystemColors.Highlight;
            }
            else
            {
                // Do not highlight anything else / remove old highlighting
                root.ForeColor = ForeColor;
                root.BackColor = BackColor;
            }

            // Recurse through the rest of the nodes
            foreach (TreeNode child in root.Nodes)
            {
                HighlightFolder(child, folder);
            }
        }

        /// <summary>
        /// Initializes the <see cref="GrhTreeView"/> with all features.
        /// </summary>
        /// <param name="cm">The <see cref="IContentManager"/> used for loading content needed by the
        /// <see cref="GrhTreeView"/>.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Grh")]
        public void Initialize(IContentManager cm)
        {
            if (DesignMode)
                return;

            _contentManager = cm;

            // Perform the compact initialization
            InitializeCompact();

            // Perform some extended initialization goodness
            _compactMode = false;

            // Event hooks
            GrhMouseDoubleClick -= GrhTreeView_GrhMouseDoubleClick;
            GrhMouseDoubleClick += GrhTreeView_GrhMouseDoubleClick;

            // Set up the context menu for the GrhTreeView
#pragma warning disable 162
            if (EnableGrhEditor)
            {
                _contextMenu.MenuItems.Add(new MenuItem("Edit", MenuClickEdit));
                _contextMenu.MenuItems.Add(new MenuItem("New Grh", MenuClickNewGrh));
                _contextMenu.MenuItems.Add(new MenuItem("Duplicate", MenuClickDuplicate));
            }
#pragma warning restore 162

            ContextMenu = _contextMenu;

            AllowDrop = true;
        }

        /// <summary>
        /// Initializes the <see cref="GrhTreeView"/>. Requires that the <see cref="GrhData"/>s are already
        /// loaded and won't provide any additional features.
        /// </summary>
        public void InitializeCompact()
        {
            RealInitializeCompact();
        }

        /// <summary>
        /// Checks if a <see cref="TreeNode"/> contains a <see cref="GrhData"/>, or is a folder.
        /// </summary>
        /// <param name="node">The <see cref="TreeNode"/> to check.</param>
        /// <returns>True if a Grh, false if a folder.</returns>
        static bool IsGrhDataNode(TreeNode node)
        {
            return node is GrhTreeViewNode;
        }

        void MenuClickDuplicate(object sender, EventArgs e)
        {
            var node = SelectedNode;

            if (node == null)
                return;

            // Confirm the duplicate request
            var count = NodeCount(node);
            string text;
            if (count <= 0)
            {
                Debug.Fail(string.Format("Somehow, we have a count of `{0}` nodes...", count));
                return;
            }

            if (count == 1)
                text = "Are you sure you wish to duplicate this node?";
            else
                text = string.Format("Are you sure you wish to duplicate these {0} nodes?", NodeCount(node));
            if (MessageBox.Show(text, "Duplicate nodes?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            DuplicateNodes(node);
        }

        void MenuClickEdit(object sender, EventArgs e)
        {
            var node = SelectedNode;

            if (node == null)
                return;

            var gd = GetGrhData(node);
            if (gd is AutomaticAnimatedGrhData)
                return;

            if (gd != null && node.Nodes.Count == 0)
            {
                // The TreeNode is a GrhData
                BeginEditGrhData(node);
            }
            else if (gd == null)
            {
                // The TreeNode is a folder
                node.BeginEdit();
            }
        }

        void MenuClickNewGrh(object sender, EventArgs e)
        {
            if (_contentManager == null)
                return;

            // Create the new GrhData
            var category = GetCategoryFromTreeNode(SelectedNode);
            GrhData gd = GrhInfo.CreateGrhData(_contentManager, category);
            UpdateGrhData(gd);

            // Begin edit
            BeginEditGrhData(gd, true);
        }

        /// <summary>
        /// Counts the number of nodes under the root node, plus the root itself
        /// </summary>
        /// <param name="root">Root node to count from</param>
        /// <returns>Number of nodes under the root node, plus the root itself</returns>
        public static int NodeCount(TreeNode root)
        {
            // No root? No count
            if (root == null)
                return 0;

            // 1 because we are counting ourself
            var count = 1;

            // Recursively count the children
            foreach (TreeNode child in root.Nodes)
            {
                count += NodeCount(child);
            }

            return count;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterExpand"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"/> that contains the event data.</param>
        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);

            var n = e.Node;
            if (n == null)
                return;

            // For each of the GrhTreeViewNodes that are in the node that just expanded, ensure the image has been set for them
            foreach (var c in n.Nodes.OfType<GrhTreeViewNode>())
            {
                c.EnsureImageIsSet();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterLabelEdit"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.NodeLabelEditEventArgs"/> that contains the event data.</param>
        protected override void OnAfterLabelEdit(NodeLabelEditEventArgs e)
        {
            if (_compactMode)
                return;

            base.OnAfterLabelEdit(e);

            if (e.Node != null && e.Label != null)
            {
                e.CancelEdit = true;
                e.Node.Text = e.Label;
                UpdateGrhsToTree(e.Node);
                Sort();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterSelect"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"/> that contains the event data.</param>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);

            if (GrhAfterSelect == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            var gd = GetGrhData(e.Node);
            if (gd != null)
                GrhAfterSelect.Raise(this, new GrhTreeViewEventArgs(gd, e));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeSelect"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs"/> that contains the event data.</param>
        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            base.OnBeforeSelect(e);

            if (GrhBeforeSelect == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            var gd = GetGrhData(e.Node);
            if (gd != null)
                GrhBeforeSelect.Raise(this, new GrhTreeViewCancelEventArgs(gd, e));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"/> that contains the event data.</param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            if (_compactMode)
                return;

            base.OnDragDrop(e);

            foreach (TreeNode child in Nodes)
            {
                HighlightFolder(child, null);
            }

            if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                return;

            var pt = PointToClient(new Point(e.X, e.Y));
            var destNode = GetNodeAt(pt);
            var newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            TreeNode addedNode;

            // Don't allow dropping onto itself
            if (newNode == destNode)
                return;

            // If the destination node is a GrhData node, move the destination to the folder
            if (IsGrhDataNode(destNode))
                destNode = destNode.Parent;

            // Check for a valid destination
            if (destNode == null)
                return;

            if (IsGrhDataNode(newNode))
            {
                // Move a GrhData node
                addedNode = (TreeNode)newNode.Clone();
                destNode.Nodes.Add(addedNode);
                destNode.Expand();
            }
            else
            {
                // Move a folder node

                // Do not allow a node to be moved into its own child
                var tmp = destNode;
                while (tmp.Parent != null)
                {
                    if (tmp.Parent == newNode)
                        return;
                    tmp = tmp.Parent;
                }

                addedNode = (TreeNode)newNode.Clone();
                destNode.Nodes.Add(addedNode);
            }

            // If a node was added, we will want to update
            destNode.Expand();
            newNode.Remove();
            UpdateGrhsToTree(addedNode);
            SelectedNode = addedNode;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"/> event.
        /// </summary>
        /// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"/> that contains the event data.</param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (_compactMode)
                return;

            drgevent.Effect = DragDropEffects.Move;

            base.OnDragEnter(drgevent);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragOver"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"/> that contains the event data.</param>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            if (_compactMode)
                return;

            var nodeOver = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (nodeOver != null)
            {
                // Find the folder the node will drop into
                var folderNode = nodeOver;
                if (!IsGrhDataNode(folderNode))
                    folderNode = folderNode.Parent;

                // Perform the highlighting
                foreach (TreeNode child in Nodes)
                {
                    HighlightFolder(child, folderNode);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.DrawNode"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DrawTreeNodeEventArgs"/> that contains the event data.</param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            // Perform the default drawing
            e.DrawDefault = true;

            // Draw the node's image
            var casted = e.Node as IGrhTreeViewNode;
            if (casted == null)
                return;

            var image = casted.Image;
            if (image == null)
                return;

            var p = new Point(e.Node.Bounds.X - ImageList.ImageSize.Width - _drawImageOffset, e.Node.Bounds.Y);
            e.Graphics.DrawImageUnscaled(image, p);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.ItemDrag"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs"/> that contains the event data.</param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            if (_compactMode)
                return;

            base.OnItemDrag(e);

            if (e.Button == MouseButtons.Left)
            {
                SelectedNode = (TreeNode)e.Item;
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var n = GetNodeAt(e.X, e.Y) as GrhTreeViewFolderNode;
            if (n != null)
                n.UpdateToolTip();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> that contains the
        /// event data.</param>
        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);

            // Change the SelectedNode to the clicked node (normally, right-click doesn't change the selected)
            if (e.Node != null)
                SelectedNode = e.Node;

            // If there is no GrhMouseClick event, or this was a folder, there is nothing left to do
            if (GrhMouseClick == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            // Get the GrhData for the node clicked, raising the GrhMouseClick event if valid
            var gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseClick.Raise(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseDoubleClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"/> that contains the
        /// event data.</param>
        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseDoubleClick(e);

            // If there is no GrhMouseDoubleClick event, or this was a folder, there is nothing left to do
            if (GrhMouseDoubleClick == null || e.Node == null || !IsGrhDataNode(e.Node))
                return;

            // Get the GrhData for the node double-clicked, raising the GrhMouseDoubleClick event if valid
            var gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseDoubleClick.Raise(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        void RealInitializeCompact()
        {
            Enabled = false;

            AllowDrop = false;

            // Set the sort method
            TreeViewNodeSorter = this;

            // Build the tree
            RebuildTree();

            // Listen for GrhDatas being added/removed
            GrhInfo.Removed -= GrhInfo_Removed;
            GrhInfo.Removed += GrhInfo_Removed;

            // Create the animate timer
            _animTimer.Interval = 150;
            _animTimer.Tick -= UpdateAnimations;
            _animTimer.Tick += UpdateAnimations;
            _animTimer.Start();

            Enabled = true;
        }

        /// <summary>
        /// Completely rebuilds the <see cref="GrhTreeView"/>.
        /// </summary>
        public void RebuildTree()
        {
            if (DesignMode)
                return;

            // Suspend the layout and updating while we massively alter the collection
            BeginUpdate();
            SuspendLayout();

            try
            {
                // If there are any nodes already, keep track of which is selected
                GrhTreeViewNode selectedGrhNode = SelectedNode as GrhTreeViewNode;
                GrhData selectedGrhData = selectedGrhNode != null ? selectedGrhNode.GrhData : null;

                // Clear any existing nodes (probably isn't any, but just in case...)
                Nodes.Clear();

                // Set up the filter
                string[] filterWords = (Filter ?? string.Empty).Split(',').Distinct(StringComparer.OrdinalIgnoreCase).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
                if (filterWords.Length == 0)
                    filterWords = null;

                // Iterate through all the GrhDatas
                foreach (var grhData in GrhInfo.GrhDatas)
                {
                    if (filterWords != null)
                    {
                        // With filtering
                        string cat = grhData.Categorization.ToString();
                        if (filterWords.Any(x => cat.Contains(x, StringComparison.OrdinalIgnoreCase)))
                            AddGrhToTree(grhData);
                    }
                    else
                    {
                        // No filtering
                        AddGrhToTree(grhData);
                    }
                }

                // Perform the initial sort
                Sort();

                // If we used filtering, expand all nodes
                if (filterWords != null)
                    ExpandAll();

                // Restore selection
                if (selectedGrhData != null)
                    SelectedNode = FindGrhDataNode(selectedGrhData);
            }
            finally
            {
                // Resume the layout and updating
                ResumeLayout();
                EndUpdate();
            }
        }

        /// <summary>
        /// Forces a node's image to be refreshed.
        /// </summary>
        /// <param name="n">The node to refresh.</param>
        internal void RefreshNodeImage(TreeNode n)
        {
            var w = ImageList.ImageSize.Width;
            var rect = new Rectangle(n.Bounds.X - w - _drawImageOffset, n.Bounds.Y, w, n.Bounds.Height);
            Invalidate(rect);
        }

        /// <summary>
        /// Updates the visible animated <see cref="GrhTreeViewNode"/>s.
        /// </summary>
        void UpdateAnimations(object sender, EventArgs e)
        {
            foreach (var toUpdate in GetVisibleAnimatedGrhTreeViewNodes(Nodes))
            {
                toUpdate.UpdateIconImage();
            }
        }

        /// <summary>
        /// Updates a <see cref="GrhData"/>'s information in the tree.
        /// </summary>
        /// <param name="grhData"><see cref="GrhData"/> to update.</param>
        public void UpdateGrhData(GrhData grhData)
        {
            GrhTreeViewNode.Create(this, grhData);
        }

        /// <summary>
        /// Updates a GrhData's information in the tree
        /// </summary>
        /// <param name="grhIndex">Index of the GrhData to update</param>
        public void UpdateGrhData(GrhIndex grhIndex)
        {
            UpdateGrhData(GrhInfo.GetData(grhIndex));
        }

        public void UpdateGrhDatas(IEnumerable<GrhData> grhDatas)
        {
            foreach (var grhData in grhDatas)
            {
                UpdateGrhData(grhData);
            }
        }

        /// <summary>
        /// Updates the GrhData categorizing to match the displayed tree. Used to make the GrhData categorization
        /// be set based on the tree.
        /// </summary>
        /// <param name="root">Root TreeNode to start updating at</param>
        static void UpdateGrhsToTree(TreeNode root)
        {
            if (!IsGrhDataNode(root))
            {
                // Node is a branch, so it should be a folder
                foreach (TreeNode node in root.Nodes)
                {
                    UpdateGrhsToTree(node);
                }
            }
            else
            {
                var grhDataNode = root as GrhTreeViewNode;
                if (grhDataNode == null)
                    return;

                grhDataNode.SyncGrhData();
            }
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)"/>.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_ERASEBKGND = 0x0014;

            // Prevent the TreeView from erasing its own background, which helps reduce flickering
            if (m.Msg == WM_ERASEBKGND)
                m.Msg = 0x0000;
            base.WndProc(ref m);
        }

        #region IComparer Members

        /// <summary>
        /// Compares two <see cref="TreeNode"/>s.
        /// </summary>
        /// <param name="a">First object.</param>
        /// <param name="b">Second object.</param>
        /// <returns>-1 if a is first, 1 if b is first, or 0 for no preference.</returns>
        int IComparer.Compare(object a, object b)
        {
            return ((IComparer<TreeNode>)this).Compare(a as TreeNode, b as TreeNode);
        }

        #endregion

        #region IComparer<TreeNode> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// Value Condition
        ///     Less than zero
        ///     <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero
        ///     <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero
        ///     <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        int IComparer<TreeNode>.Compare(TreeNode x, TreeNode y)
        {
            // Can't do much if either of the nodes are null... but no biggie, they never should be anyways
            if (x == null || y == null)
                return 0;

            // Folders take priority
            if (!IsGrhDataNode(x) && IsGrhDataNode(y))
                return -1;

            if (!IsGrhDataNode(y) && IsGrhDataNode(x))
                return 1;

            // Text sort
            return _nodeTextComparer.Compare(x.Text, y.Text);
        }

        #endregion
    }
}