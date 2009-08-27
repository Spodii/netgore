using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using NetGore.Graphics;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace NetGore.EditorTools
{
    public delegate void GrhTreeViewContextMenuItemClickEvent(object sender, EventArgs e);

    public delegate void GrhTreeViewEvent(object sender, GrhTreeViewEventArgs e);

    public delegate void GrhTreeNodeMouseClickEvent(object sender, GrhTreeNodeMouseClickEventArgs e);

    public delegate void GrhTreeViewCancelEvent(object sender, GrhTreeViewCancelEventArgs e);

    /// <summary>
    /// TreeView used to display Grhs
    /// </summary>
    public class GrhTreeView : TreeView, IComparer
    {
        /// <summary>
        /// Timer to update the animated Grhs in the grh tree
        /// </summary>
        readonly Timer _animTimer = new Timer();

        /// <summary>
        /// List of animated grhs in the tree
        /// </summary>
        readonly List<GrhTreeNode> _animTreeNodes = new List<GrhTreeNode>();

        readonly ContextMenu _contextMenu = new ContextMenu();

        /// <summary>
        /// Image list for the treeGrhs
        /// </summary>
        readonly ImageList _grhImageList = new ImageList();

        /// <summary>
        /// Track the elapsed time for Grh animating
        /// </summary>
        readonly Stopwatch _watch = new Stopwatch();

        /// <summary>
        /// Occurs after a new Grh node is selected
        /// </summary>
        public event GrhTreeViewEvent GrhAfterSelect;

        /// <summary>
        /// Occurs before a new Grh node is selected
        /// </summary>
        public event GrhTreeViewCancelEvent GrhBeforeSelect;

        public event GrhTreeViewContextMenuItemClickEvent GrhContextMenuBatchChangeTextureClick;
        public event GrhTreeViewContextMenuItemClickEvent GrhContextMenuDuplicateClick;

        /// <summary>
        /// Occurs when context menu item "Edit" is clicked.
        /// </summary>
        public event GrhTreeViewContextMenuItemClickEvent GrhContextMenuEditClick;

        public event GrhTreeViewContextMenuItemClickEvent GrhContextMenuNewGrhClick;

        /// <summary>
        /// Occurs when a Grh node is clicked
        /// </summary>
        public event GrhTreeNodeMouseClickEvent GrhMouseClick;

        /// <summary>
        /// Occurs when a Grh node is double-clicked
        /// </summary>
        public event GrhTreeNodeMouseClickEvent GrhMouseDoubleClick;

        /// <summary>
        /// Gets or sets the size of the Grh preview images
        /// </summary>
        [Description("Size of the Grh images in pixels")]
        public Size ImageSize
        {
            get { return ImageList.ImageSize; }
            set { ImageList.ImageSize = value; }
        }

        /// <summary>
        /// Gets the current time
        /// </summary>
        int Time
        {
            get { return (int)_watch.ElapsedMilliseconds; }
        }

        /// <summary>
        /// GrhTreeView constructor
        /// </summary>
        public GrhTreeView()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            AllowDrop = true;

            // Remove all nodes
            Nodes.Clear();

            // Create the animate timer
            _animTimer.Interval = 150;
            _animTimer.Tick += UpdateAnimations;
            _animTimer.Start();

            // Start the elapsed time stopwatch
            _watch.Start();

            // Set the sort method
            TreeViewNodeSorter = this;

            // Create the ImageList containing the Grhs as an image
            ImageList = GrhImageList.ImageList;

            // Event hooks
            AfterSelect += GrhTreeView_AfterSelect;
            BeforeSelect += GrhTreeView_BeforeSelect;
            NodeMouseDoubleClick += GrhTreeView_NodeMouseDoubleClick;
            NodeMouseClick += GrhTreeView_NodeMouseClick;
            ItemDrag += GrhTreeView_ItemDrag;
            DragEnter += GrhTreeView_DragEnter;
            DragDrop += GrhTreeView_DragDrop;
            DragOver += GrhTreeView_DragOver;
            AfterLabelEdit += GrhTreeView_AfterLabelEdit;

            //Set up the context menu for the GrhTreeView
            _contextMenu.MenuItems.Add(new MenuItem("Edit", MenuClickEdit));
            _contextMenu.MenuItems.Add(new MenuItem("New Grh", MenuClickNewGrh));
            _contextMenu.MenuItems.Add(new MenuItem("Duplicate", MenuClickDuplicate));
            _contextMenu.MenuItems.Add(new MenuItem("Batch Change Texture", MenuClickBatchChangeTexture));
            _contextMenu.MenuItems.Add(new MenuItem("Automatic Update", MenuClickAutomaticUpdate));
            ContextMenu = _contextMenu;

            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Adds a Grh to the tree or updates it if it already exists.
        /// </summary>
        /// <param name="gd">Grh to add or update.</param>
        void AddGrhToTree(GrhData gd)
        {
            if (gd == null || gd.GrhIndex == 0)
                return;
            string indexStr = gd.GrhIndex.ToString();

            // Set the categorization information
            string category;
            if (!string.IsNullOrEmpty(gd.Category))
                category = gd.Category;
            else
                category = "Uncategorized";

            string title = string.Format("[{0}]", indexStr);
            if (!string.IsNullOrEmpty(gd.Title))
                title = string.Format("{0} {1}", gd.Title, title);

            // Add to the tree
            string nodePath = string.Format("{0}.{1}", category, title);
            TreeNode node = CreateNode(indexStr, nodePath, '.');

            // Set the preview picture
            if (!string.IsNullOrEmpty(gd.TextureName))
            {
                string imageKey = GrhImageList.GetImageKey(gd);
                node.ImageKey = imageKey;
                node.SelectedImageKey = imageKey;
                node.StateImageKey = imageKey;
            }
            else if (gd.Frames != null)
            {
                // Grh does not contain a valid texture, but it does have frames, so it must be animated
                Grh nodeGrh = new Grh(gd.GrhIndex, AnimType.Loop, Time);
                GrhTreeNode treeNode = new GrhTreeNode(node, nodeGrh);
                _animTreeNodes.Add(treeNode);
            }

            // Set the tooltip text
            node.ToolTipText = GetToolTipText(gd);
        }

        void CheckForMissingTextures(ContentManager cm)
        {
            // We must create the hash collection since its constructor has the updating goodies, and we want
            // to make sure that is called
            TextureHashCollection hashCollection = new TextureHashCollection();

            // Get the GrhDatas with missing textures
            var missing = GrhInfo.FindMissingTextures();
            if (missing.Count() == 0)
                return;

            // Display a form showing which textures need to be fixed
            // The GrhTreeView will be disabled until the MissingTexturesForm is closed
            Enabled = false;
            MissingTexturesForm frm = new MissingTexturesForm(hashCollection, missing, cm);
            frm.FormClosed += delegate
            {
                RebuildTree();
                Enabled = true;
            };
            frm.Show();
        }

        /// <summary>
        /// Adds a node to a tree node collection with support of recursively adding nodes
        /// to pre-existing nodes under the same branch
        /// </summary>
        /// <param name="name">Unique name of the node</param>
        /// <param name="path">Complete path of the node from the root</param>
        /// <param name="separator">Character used to separate the individual nodes of the tree list</param>
        /// <returns>Node added to the tree</returns>
        TreeNode CreateNode(string name, string path, char separator)
        {
            // Split up the category by the separator
            var cats = path.Split(separator);

            // Recursively add the category and all subcategories
            TreeNodeCollection nodeColl = Nodes;
            for (int i = 0; i < cats.Length - 1; i++)
            {
                bool addNew = true;

                // If a node of the same name already exists, append to existing node
                foreach (TreeNode n in nodeColl)
                {
                    if (n.Text == cats[i])
                    {
                        nodeColl = n.Nodes;
                        addNew = false;
                        break;
                    }
                }

                // If no existing node was found, create a new one (defaulting as a folder)
                if (addNew)
                {
                    TreeNode newNode = nodeColl.Add(cats[i]);
                    newNode.ImageKey = GrhImageList.ClosedFolderKey;
                    newNode.SelectedImageKey = GrhImageList.OpenFolderKey;
                    newNode.StateImageKey = GrhImageList.ClosedFolderKey;
                    nodeColl = newNode.Nodes;
                }
            }

            // Check for a node of the same name
            TreeNode ret;
            if (nodeColl.ContainsKey(name))
            {
                // We already have the node created, so just use that
                ret = nodeColl[name];
            }
            else
            {
                // The node didn't already exist, so create it
                ret = nodeColl.Add(cats[cats.Length - 1]);
                ret.Text = cats[cats.Length - 1];
                ret.Name = name;
            }

            return ret;
        }

        /// <summary>
        /// Deletes a node from the tree, along with any node under it
        /// </summary>
        /// <param name="root">Root node to delete</param>
        static void DeleteNode(TreeNode root)
        {
            if (!IsGrhNode(root))
            {
                // Recursively delete all nodes under this one
                // Because the collection will be changed, we create a temporary list that we will use
                // to find the nodes to remove to prevent problems from the changing collection
                // Populate the list
                var nodes = new List<TreeNode>(root.Nodes.Count);
                foreach (TreeNode node in root.Nodes)
                {
                    nodes.Add(node);
                }

                // Delete while iterating through our temporary list
                foreach (TreeNode node in nodes)
                {
                    DeleteNode(node);
                }
            }
            else
            {
                // If the node is a Grh, remove the GrhData associated with it
                GrhData gd = GetGrhData(root);
                if (gd == null)
                    throw new Exception("Failed to find a valid GrhData for node.");
                GrhInfo.Delete(gd);
            }

            // Remove the node from the tree
            root.Remove();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                GrhImageListCache.Save();
        }

        /// <summary>
        /// Creates a duplicate of the tree nodes and structure from the root, giving it a unique name. All GrhDatas
        /// under the node to be duplicated will be duplicated and placed in a new category with a new GrhIndex,
        /// but with the same name and structure.
        /// </summary>
        /// <param name="root">Root node to duplicate from</param>
        /// <returns>GrhTreeNode of the root of the new duplicate</returns>
        public GrhTreeNode DuplicateNodes(TreeNode root)
        {
            // Get a unique name for the new node
            string newName = GetUniqueNodeText(root);
            string oldPath = root.FullPath.Replace(PathSeparator, ".");
            string newPath = root.Parent.FullPath.Replace(PathSeparator, ".") + "." + newName;

            // Loop through each Grh under the root, including the root, 
            // and create a new GrhData for it with the new path
            DuplicateNodes(root, oldPath, newPath);

            return null;
        }

        /// <summary>
        /// Recursively duplicates a root node and all nodes under it, changing the category. Duplication
        /// actually takes place by duplicating the GrhDatas under the new category, then rebuilding the tree
        /// to let the duplication take effect
        /// </summary>
        /// <param name="root">Root node to duplicate from</param>
        /// <param name="oldCategory">Old category name</param>
        /// <param name="newCategory">New category name</param>
        void DuplicateNodes(TreeNode root, string oldCategory, string newCategory)
        {
            // If this is not a leaf, it wont have a GrhData associated with it
            if (!IsGrhNode(root))
            {
                // Recursively loop through the nodes
                foreach (TreeNode child in root.Nodes)
                {
                    DuplicateNodes(child, oldCategory, newCategory);
                }
            }
            else
            {
                // Get the old GrhData
                GrhData oldGrhData = GetGrhData(root);
                if (oldGrhData == null)
                {
                    Debug.Fail("Found a null GrhData for node.");
                    return;
                }

                // Build the new category
                string newCat = oldGrhData.Category.Replace(oldCategory, newCategory);

                // Build the new title (appending " copy" constantly until it is unique) and category
                string newTitle = oldGrhData.Title;
                while (GrhInfo.GetData(newCat, newTitle) != null)
                {
                    newTitle += " copy";
                }

                // Create the new GrhData
                GrhData newGrhData = oldGrhData.Duplicate(newCat, newTitle);

                // Add the GrhData in the tree, which will ensure it is in the display
                AddGrhToTree(newGrhData);
            }
        }

        /// <summary>
        /// Finds the GrhData for a given TreeNode
        /// </summary>
        /// <param name="node">TreeNode to get the GrhData from</param>
        /// <returns>GrhData for the TreeNode, null if none</returns>
        public static GrhData GetGrhData(TreeNode node)
        {
            // If a null node, or the node is not a leaf (which means its a folder), return null
            if (node == null || !IsGrhNode(node))
                return null;

            // Get the index
            GrhIndex grhIndex;
            if (!GrhIndex.TryParse(node.Name, out grhIndex))
            {
                Debug.Fail("Failed to parse GrhIndex of the node.");
                return null;
            }

            // Get the data
            GrhData grhData = GrhInfo.GetData(grhIndex);
            if (grhData == null || grhData.GrhIndex == 0)
            {
                Debug.Fail("Failed to find the GrhData associated with the node.");
                return null;
            }

            return grhData;
        }

        /// <summary>
        /// Creates the tooltip text to use for a GrhData.
        /// </summary>
        /// <param name="gd">GrhData to get the tooltip for.</param>
        /// <returns>The tooltip text to use for a GrhData.</returns>
        static string GetToolTipText(GrhData gd)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (gd.Frames.Length == 1)
                {
                    // Stationary
                    Rectangle sourceRect = gd.SourceRect;

                    sb.AppendLine("Grh: " + gd.GrhIndex);
                    sb.AppendLine("Texture: " + gd.TextureName);
                    sb.AppendLine("Pos: (" + sourceRect.X + "," + sourceRect.Y + ")");
                    sb.Append("Size: " + sourceRect.Width + "x" + sourceRect.Height);
                }
                else
                {
                    // Animated
                    const string framePadding = "  ";
                    const string frameSeperator = ",";

                    sb.AppendLine("Grh: " + gd.GrhIndex);
                    sb.AppendLine("Frames: " + gd.Frames.Length);

                    sb.Append(framePadding);
                    for (int i = 0; i < gd.Frames.Length; i++)
                    {
                        sb.Append(gd.Frames[i].GrhIndex);
                        if (i < gd.Frames.Length - 1)
                        {
                            if ((i + 1) % 6 == 0)
                            {
                                // Add a break every 6 indices
                                sb.AppendLine();
                                sb.Append(framePadding);
                            }
                            else
                            {
                                // Separate the frame indicies
                                sb.Append(frameSeperator);
                            }
                        }
                    }

                    sb.AppendLine();
                    sb.Append("Speed: " + (1f / gd.Speed));

                    return sb.ToString();
                }
            }
            catch (ContentLoadException ex)
            {
                Debug.Fail(ex.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a unique text for a node, based off an existing node
        /// </summary>
        /// <param name="node">Node to base the text off of</param>
        /// <returns>Unique node text</returns>
        static string GetUniqueNodeText(TreeNode node)
        {
            int copyNum = 1;
            TreeNode parent = node.Parent;
            string newText;

            // Loop until we find a unique name
            do
            {
                copyNum++;
                newText = string.Format("{0} ({1})", node.Text, copyNum);
            }
            while (TreeContainsText(parent, newText));

            // Return the unique name
            return newText;
        }

        void GrhTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node != null && e.Label != null)
            {
                e.CancelEdit = true;
                e.Node.Text = e.Label;
                UpdateGrhsToTree(e.Node);
                Sort();
            }
        }

        void GrhTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (GrhAfterSelect == null || e.Node == null || !IsGrhNode(e.Node))
                return;

            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhAfterSelect(this, new GrhTreeViewEventArgs(gd, e));
        }

        void GrhTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (GrhBeforeSelect == null || e.Node == null || !IsGrhNode(e.Node))
                return;

            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhBeforeSelect(this, new GrhTreeViewCancelEventArgs(gd, e));
        }

        void GrhTreeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeView srcView = (TreeView)sender;

            foreach (TreeNode child in Nodes)
            {
                HighlightFolder(child, null);
            }

            if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
                return;

            Point pt = srcView.PointToClient(new Point(e.X, e.Y));
            TreeNode destNode = srcView.GetNodeAt(pt);
            TreeNode newNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            TreeNode addedNode;

            // Don't allow dropping onto itself
            if (newNode == destNode)
                return;

            // If the destination node is a Grh node, move the destination to the folder
            if (TryGetGrhData(destNode))
                destNode = destNode.Parent;

            // Check for a valid destination
            if (destNode == null)
                return;

            if (IsGrhNode(newNode))
            {
                // Move a Grh node
                addedNode = (TreeNode)newNode.Clone();
                destNode.Nodes.Add(addedNode);
                destNode.Expand();
            }
            else
            {
                // Move a folder node

                // Do not allow a node to be moved into its own child
                TreeNode tmp = destNode;
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

        static void GrhTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        void GrhTreeView_DragOver(object sender, DragEventArgs e)
        {
            TreeNode nodeOver = GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (nodeOver != null)
            {
                // Find the folder the node will drop into
                TreeNode folderNode = nodeOver;
                if (!IsGrhNode(folderNode))
                    folderNode = folderNode.Parent;

                // Perform the highlighting
                foreach (TreeNode child in Nodes)
                {
                    HighlightFolder(child, folderNode);
                }
            }
        }

        void GrhTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedNode = (TreeNode)e.Item;
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        void GrhTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Change the SelectedNode to the clicked node (normally, right-click doesn't change the selected)
            if (e.Node != null)
                SelectedNode = e.Node;

            // If there is no GrhMouseClick event, or this was a folder, there is nothing left to do
            if (GrhMouseClick == null || e.Node == null || !IsGrhNode(e.Node))
                return;

            // Get the GrhData for the node clicked, raising the GrhMouseClick event if valid
            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseClick(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        void GrhTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // If there is no GrhMouseDoubleClick event, or this was a folder, there is nothing left to do
            if (GrhMouseDoubleClick == null || e.Node == null || !IsGrhNode(e.Node))
                return;

            // Get the GrhData for the node double-clicked, raising the GrhMouseDoubleClick event if valid
            GrhData gd = GetGrhData(e.Node);
            if (gd != null)
                GrhMouseDoubleClick(this, new GrhTreeNodeMouseClickEventArgs(gd, e));
        }

        void HighlightFolder(TreeNode root, TreeNode folder)
        {
            if (folder != null && (root == folder || (root.Parent == folder && IsGrhNode(root))))
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
        /// Generates the tree information. Must be called after GrhInfo.Load().
        /// </summary>
        public void Initialize(ContentManager cm)
        {
            // Check for missing textures
            CheckForMissingTextures(cm);

            // Iterate through all the grhs
            foreach (GrhData grh in GrhInfo.GrhDatas)
            {
                AddGrhToTree(grh);
            }

            // Perform the initial sort
            Sort();
        }

        /// <summary>
        /// Checks if a node contains a Grh, or is a folder
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>True if a Grh, false if a folder</returns>
        static bool IsGrhNode(TreeNode node)
        {
            return (node.Name.Length > 0);
        }

        void MenuClickAutomaticUpdate(object sender, EventArgs e)
        {
            // HACK: I shouldn't be grabbing the ContentManager like this... but how else should I go about getting it? o.O
            ContentManager cm = GrhInfo.GrhDatas.First(x => x.ContentManager != null).ContentManager;
            if (cm == null)
                throw new Exception("Failed to find a ContentManager to use.");

            var newGDs = AutomaticGrhDataUpdater.UpdateAll(cm, ContentPaths.Dev.Grhs);
            int newCount = newGDs.Count();

            if (newCount > 0)
            {
                UpdateGrhDatas(newGDs);
                MessageBox.Show(newCount + " new GrhDatas have been automatically added.");
            }
            else
                MessageBox.Show("No new GrhDatas automatically added - everything is already up to date.");
        }

        void MenuClickBatchChangeTexture(object sender, EventArgs e)
        {
            GrhContextMenuBatchChangeTextureClick(sender, e);
        }

        void MenuClickDuplicate(object sender, EventArgs e)
        {
            GrhContextMenuDuplicateClick(sender, e);
        }

        void MenuClickEdit(object sender, EventArgs e)
        {
            GrhContextMenuEditClick(sender, e);
        }

        void MenuClickNewGrh(object sender, EventArgs e)
        {
            GrhContextMenuNewGrhClick(sender, e);
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
            int count = 1;

            // Recursively count the children
            foreach (TreeNode child in root.Nodes)
            {
                count += NodeCount(child);
            }

            return count;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Delete)
            {
                const string txt = "Are you sure you wish to delete this node?";
                if (MessageBox.Show(txt, "Delete node?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    DeleteNode(SelectedNode);
            }
        }

        /// <summary>
        /// Completely rebuilds the GrhTreeView.
        /// </summary>
        public void RebuildTree()
        {
            // Clear all nodes
            Nodes.Clear();
            _animTreeNodes.Clear();

            // Re-add every GrhData
            foreach (GrhData grh in GrhInfo.GrhDatas)
            {
                AddGrhToTree(grh);
            }
        }

        /// <summary>
        /// Removes all empty folders
        /// </summary>
        static void RemoveEmptyFolders(TreeNode root)
        {
            if (root == null)
                return;

            if (!IsGrhNode(root) && root.Nodes.Count == 0)
            {
                root.Remove();
                RemoveEmptyFolders(root.Parent);
            }
            else
            {
                foreach (TreeNode node in root.Nodes)
                {
                    RemoveEmptyFolders(node);
                }
            }
        }

        /// <summary>
        /// If a TreeNode contains a node with the given text already
        /// </summary>
        /// <param name="node">Node to check for texts</param>
        /// <param name="text">Text to check against</param>
        /// <returns>True if the text exists in the <paramref name="node"/>, else false</returns>
        static bool TreeContainsText(TreeNode node, string text)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Text == text)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the GrhData for a given TreeNode
        /// </summary>
        /// <param name="node">TreeNode to get the GrhData from</param>
        /// <param name="gd">GrhData returned, or null if invalid</param>
        /// <returns>True if a valid GrhData was found, or false if not</returns>
        public static bool TryGetGrhData(TreeNode node, out GrhData gd)
        {
            // If a null node, or the node is not a leaf (which means its a folder), return null
            if (node == null || !IsGrhNode(node))
            {
                gd = null;
                return false;
            }

            // Get the index
            GrhIndex grhIndex;
            if (!GrhIndex.TryParse(node.Name, out grhIndex))
            {
                Debug.Fail("Failed to parse GrhIndex of the node.");
                gd = null;
                return false;
            }

            // Get the data
            GrhData grhData = GrhInfo.GetData(grhIndex);
            if (grhData == null || grhData.GrhIndex == 0)
            {
                Debug.Fail("Failed to find the GrhData associated with the node.");
                gd = null;
                return false;
            }

            gd = grhData;
            return true;
        }

        /// <summary>
        /// Finds the GrhData for a given TreeNode
        /// </summary>
        /// <param name="node">TreeNode to get the GrhData from</param>
        /// <returns>True if a valid GrhData was found, or false if not</returns>
        public static bool TryGetGrhData(TreeNode node)
        {
            // If a null node, or the node is not a leaf (which means its a folder), return false
            if (node == null || !IsGrhNode(node))
                return false;

            // Get the index
            GrhIndex grhIndex;
            if (!GrhIndex.TryParse(node.Name, out grhIndex))
            {
                Debug.Fail("Failed to parse GrhIndex of the node.");
                return false;
            }

            // Get the data
            GrhData grhData = GrhInfo.GetData(grhIndex);
            if (grhData == null || grhData.GrhIndex == 0)
            {
                Debug.Fail("Failed to find the GrhData associated with the node.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Animates the Grhs in the tree with multiple frames
        /// </summary>
        void UpdateAnimations(object sender, EventArgs e)
        {
            // Loop through every animated Grh we have (no need to update stationary ones)
            foreach (GrhTreeNode atn in _animTreeNodes)
            {
                // Check if the TreeNode for the animated Grh is even visible
                if (!atn.TreeNode.IsVisible)
                    continue;

                // Store the GrhIndex of the animation before updating it to compare if there was a change
                GrhIndex oldGrhIndex = atn.Grh.CurrentGrhData.GrhIndex;

                // Update the Grh
                atn.Grh.Update(Time);

                // Check that the GrhIndex changed from the update
                if (oldGrhIndex != atn.Grh.CurrentGrhData.GrhIndex)
                {
                    // Change the image
                    string imageKey = GrhImageList.GetImageKey(atn.Grh.CurrentGrhData);
                    atn.TreeNode.ImageKey = imageKey;
                    atn.TreeNode.SelectedImageKey = imageKey;
                    atn.TreeNode.StateImageKey = imageKey;
                }
            }
        }

        /// <summary>
        /// Updates a GrhData's information in the tree.
        /// </summary>
        /// <param name="grhData">GrhData to update.</param>
        public void UpdateGrhData(GrhData grhData)
        {
            AddGrhToTree(grhData);
            foreach (TreeNode node in Nodes)
            {
                RemoveEmptyFolders(node);
            }
            Sort();
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
            foreach (GrhData grhData in grhDatas)
            {
                UpdateGrhData(grhData);
            }
        }

        /// <summary>
        /// Updates the GrhData categorizing to match the displayed tree. Used to make the GrhData categorization
        /// be set based on the tree.
        /// </summary>
        /// <param name="root">Root TreeNode to start updating at</param>
        void UpdateGrhsToTree(TreeNode root)
        {
            if (!IsGrhNode(root))
            {
                // Node is a branch, so it should be a folder
                foreach (TreeNode node in root.Nodes)
                {
                    UpdateGrhsToTree(node);
                }
            }
            else
            {
                // Node is a leaf, so check if it has a valid GrhData
                GrhData data = GetGrhData(root);
                if (data == null)
                {
                    Debug.Fail("Found a null GrhData for node.");
                    return;
                }

                // Get the full path of the root node, ensuring the separator is a period
                string path = root.FullPath.Replace(PathSeparator, ".");

                // Remove the [GrhIndex] part, plus the space before it
                path = path.Remove(path.LastIndexOf('[') - 1);

                // Get the category and title
                int lastPeriodIndex = path.LastIndexOf('.');
                string category = path.Remove(lastPeriodIndex);
                string title = path.Substring(lastPeriodIndex + 1);

                // Ensure both the title and category do not start or end with a space or period
                var trimChars = new char[] { ' ', '.' };
                category = category.Trim(trimChars);
                title = title.Trim(trimChars);

                // Update the categorization
                data.SetCategorization(category, title);
            }
        }

        #region IComparer Members

        /// <summary>
        /// Compares two TreeNodes
        /// </summary>
        /// <param name="a">First object</param>
        /// <param name="b">Second object</param>
        /// <returns>-1 if a is first, 1 if b is first, or 0 for no preference</returns>
        int IComparer.Compare(object a, object b)
        {
            TreeNode x = (TreeNode)a;
            TreeNode y = (TreeNode)b;

            // Folders take priority
            if (!IsGrhNode(x) && IsGrhNode(y))
                return -1;
            else if (!IsGrhNode(y) && IsGrhNode(x))
                return 1;

                // Name sort
            else
                return x.Text.CompareTo(y.Text);
        }

        #endregion
    }
}