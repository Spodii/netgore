using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// A TreeView specifically for displaying a NPC chat dialog.
    /// </summary>
    public class NPCChatDialogView : TreeView
    {
        /// <summary>
        /// Maps the objects handled by a TreeNode to the TreeNodes that handle it.
        /// </summary>
        readonly Dictionary<object, List<NPCChatDialogViewNode>> _objToTreeNode =
            new Dictionary<object, List<NPCChatDialogViewNode>>();

        Color _nodeForeColorBranch = Color.DarkRed;
        Color _nodeForeColorGoTo = Color.Blue;
        Color _nodeForeColorNormal = Color.Black;
        Color _nodeForeColorResponse = Color.Green;

        EditorNPCChatDialog _npcChatDialog;

        /// <summary>
        /// Gets or sets the NPCChatDialog currently being displayed.
        /// </summary>
        public EditorNPCChatDialog NPCChatDialog
        {
            get { return _npcChatDialog; }
            set
            {
                if (_npcChatDialog == value)
                    return;

                _npcChatDialog = value;

                Nodes.Clear();
                _objToTreeNode.Clear();

                if (_npcChatDialog != null && _npcChatDialog.GetInitialDialogItem() != null)
                    new NPCChatDialogViewNode(this, _npcChatDialog.GetInitialDialogItem());
            }
        }

        /// <summary>
        /// Gets or sets the font color of branch chat dialog nodes.
        /// </summary>
        [Description("The font color of branch chat dialog nodes.")]
        public Color NodeForeColorBranch
        {
            get { return _nodeForeColorBranch; }
            set { _nodeForeColorBranch = value; }
        }

        /// <summary>
        /// Gets or sets the font color of GOTO nodes.
        /// </summary>
        [Description("The font color of GOTO nodes.")]
        public Color NodeForeColorGoTo
        {
            get { return _nodeForeColorGoTo; }
            set { _nodeForeColorGoTo = value; }
        }

        /// <summary>
        /// Gets or sets the font color of normal chat dialog nodes.
        /// </summary>
        [Description("The font color of normal chat dialog nodes.")]
        public Color NodeForeColorNormal
        {
            get { return _nodeForeColorNormal; }
            set { _nodeForeColorNormal = value; }
        }

        /// <summary>
        /// Gets or sets the font color of dialog response value nodes.
        /// </summary>
        [Description("The font color of dialog response value nodes.")]
        public Color NodeForeColorResponse
        {
            get { return _nodeForeColorResponse; }
            set { _nodeForeColorResponse = value; }
        }

        /// <summary>
        /// Handles when a <see cref="EditorNPCChatDialogItem"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="EditorNPCChatDialogItem"/> that changed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void EditorNPCChatDialogItem_Changed(EditorNPCChatDialogItem sender, EventArgs e)
        {
            List<NPCChatDialogViewNode> l;
            if (!_objToTreeNode.TryGetValue(sender, out l))
                return;

            foreach (var node in l)
            {
                node.Update(false);
            }
        }

        /// <summary>
        /// Handles when a <see cref="EditorNPCChatResponse"/> changes.
        /// </summary>
        /// <param name="response">The <see cref="EditorNPCChatResponse"/> that changed.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void EditorNPCChatResponse_Changed(EditorNPCChatResponse response, EventArgs e)
        {
            List<NPCChatDialogViewNode> l;
            if (!_objToTreeNode.TryGetValue(response, out l))
                return;

            foreach (var node in l)
            {
                node.Update(false);
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the TreeNodes from the given <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The root collection of TreeNodes.</param>
        /// <returns>An IEnumerable of all the TreeNodes from the given <paramref name="root"/>.</returns>
        public static IEnumerable<TreeNode> GetAllNodes(IEnumerable root)
        {
            foreach (var node in root.Cast<TreeNode>())
            {
                yield return node;
                foreach (var r in GetAllNodes(node.Nodes))
                {
                    yield return r;
                }
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the TreeNodes in this TreeView.
        /// </summary>
        /// <returns>An IEnumerable of all the TreeNodes in this TreeView.</returns>
        public IEnumerable<TreeNode> GetAllNodes()
        {
            return GetAllNodes(Nodes);
        }

        /// <summary>
        /// Gets the NPCChatDialogViewNode for the given NPCChatDialogItemBase. Nodes that redirect to the
        /// given NPCChatDialogItemBase are not included.
        /// </summary>
        /// <param name="dialogItem">The NPCChatDialogItemBase to find the NPCChatDialogViewNode for.</param>
        /// <returns>The NPCChatDialogViewNode for the given <paramref name="dialogItem"/>, or null if
        /// no NPCChatDialogViewNode was found that handles the given <paramref name="dialogItem"/>.</returns>
        public NPCChatDialogViewNode GetNodeForDialogItem(NPCChatDialogItemBase dialogItem)
        {
            if (NPCChatDialog == null)
                return null;

            List<NPCChatDialogViewNode> ret;
            if (!_objToTreeNode.TryGetValue(dialogItem, out ret))
                return null;

            // Remove dead nodes
            foreach (var deadNode in ret.Where(x => x.IsDead))
            {
                NotifyNodeDestroyed(deadNode);
            }

            Debug.Assert(ret.Count(x => x.ChatItemType == NPCChatDialogViewNodeItemType.DialogItem) <= 1,
                "Was only expected 0 or 1 TreeNodes to be directly for this dialogItem.");

            var r = ret.FirstOrDefault(x => x.ChatItemType == NPCChatDialogViewNodeItemType.DialogItem);

            Debug.Assert(r.ChatItemAsDialogItem == dialogItem);

            return r;
        }

        /// <summary>
        /// Gets the NPCChatDialogViewNode for the given NPCChatDialogItemBase. Nodes that redirect to the
        /// given NPCChatDialogItemBase are not included.
        /// </summary>
        /// <param name="dialogItemID">The index of the NPCChatDialogItemBase to find the NPCChatDialogViewNode for.</param>
        /// <returns>The NPCChatDialogViewNode for the given <paramref name="dialogItemID"/>, or null if
        /// no NPCChatDialogViewNode was found that handles the given <paramref name="dialogItemID"/> or
        /// if the <paramref name="dialogItemID"/> was for an invalid NPCChatDialogItemBase.</returns>
        public NPCChatDialogViewNode GetNodeForDialogItem(NPCChatDialogItemID dialogItemID)
        {
            if (NPCChatDialog == null)
                return null;

            var dialogItem = NPCChatDialog.GetDialogItem(dialogItemID);
            if (dialogItem == null)
                return null;

            return GetNodeForDialogItem(dialogItem);
        }

        /// <summary>
        /// Gets an IEnumerable of all the NPCChatDialogViewNodes that handle the given <paramref name="dialogItem"/>.
        /// This includes any NPCChatDialogViewNodes that redirect to the NPCChatDialogItemBase.
        /// </summary>
        /// <param name="dialogItem">The NPCChatDialogItemBase to find the NPCChatDialogViewNodes for.</param>
        /// <returns>An IEnumerable of all the NPCChatDialogViewNodes that handle the given
        /// <paramref name="dialogItem"/>.</returns>
        public IEnumerable<NPCChatDialogViewNode> GetNodesForDialogItem(NPCChatDialogItemBase dialogItem)
        {
            if (NPCChatDialog == null)
                return null;

            List<NPCChatDialogViewNode> ret;
            if (!_objToTreeNode.TryGetValue(dialogItem, out ret))
                return Enumerable.Empty<NPCChatDialogViewNode>();

            // Remove dead nodes
            foreach (var deadNode in ret.Where(x => x.IsDead))
            {
                NotifyNodeDestroyed(deadNode);
            }

            Debug.Assert(ret.All(x => x.ChatItemAsDialogItem == dialogItem));

            return ret;
        }

        /// <summary>
        /// Notifies the NPCChatDialogView when a NPCChatDialogViewNode was created.
        /// </summary>
        /// <param name="node">The NPCChatDialogViewNode that was created.</param>
        internal void NotifyNodeCreated(NPCChatDialogViewNode node)
        {
            List<NPCChatDialogViewNode> l;
            if (!_objToTreeNode.TryGetValue(node.ChatItem, out l))
            {
                l = new List<NPCChatDialogViewNode>(1);
                _objToTreeNode.Add(node.ChatItem, l);
            }

            if (node.ChatItemType == NPCChatDialogViewNodeItemType.Response)
            {
                node.ChatItemAsResponse.Changed -= EditorNPCChatResponse_Changed;
                node.ChatItemAsResponse.Changed += EditorNPCChatResponse_Changed;
            }
            else
            {
                node.ChatItemAsDialogItem.Changed -= EditorNPCChatDialogItem_Changed;
                node.ChatItemAsDialogItem.Changed += EditorNPCChatDialogItem_Changed;
            }

            l.Add(node);
        }

        /// <summary>
        /// Notifies the NPCChatDialogView when a NPCChatDialogViewNode was destroyed.
        /// </summary>
        /// <param name="node">The NPCChatDialogViewNode that was destroyed.</param>
        internal void NotifyNodeDestroyed(NPCChatDialogViewNode node)
        {
            List<NPCChatDialogViewNode> l;
            if (!_objToTreeNode.TryGetValue(node.ChatItem, out l))
                return;

            l.Remove(node);
        }

        /// <summary>
        /// Handles when the NPCChatDialogView is double-clicked.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                var tagAsTreeNode = e.Node.Tag as TreeNode;
                if (tagAsTreeNode != null)
                    SelectedNode = tagAsTreeNode;
            }

            base.OnNodeMouseDoubleClick(e);
        }

        /// <summary>
        /// Updates the whole TreeView to ensure it is properly synchronized.
        /// </summary>
        public void UpdateTree()
        {
            if (NPCChatDialog == null)
                return;

            foreach (var childNode in Nodes.OfType<NPCChatDialogViewNode>())
            {
                childNode.Update(true);
            }
        }
    }
}