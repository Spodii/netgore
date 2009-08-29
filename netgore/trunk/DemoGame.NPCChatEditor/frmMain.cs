using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoGame.Server.NPCChat.Conditionals;
using NetGore.EditorTools;
using NetGore.NPCChat;
using System.Collections.Generic;

namespace DemoGame.NPCChatEditor
{
    public partial class frmMain : Form
    {
        bool _doNotUpdateObj;
        object _editingObj;

        EditorNPCChatDialog CurrentDialog
        {
            get { return npcChatDialogView.NPCChatDialog; }
        }

        EditorNPCChatDialogItem EditingObjAsDialogItem
        {
            get { return _editingObj as EditorNPCChatDialogItem; }
        }

        EditorNPCChatResponse EditingObjAsResponse
        {
            get { return _editingObj as EditorNPCChatResponse; }
        }

        TreeNode EditingObjAsTreeNode
        {
            get { return _editingObj as TreeNode; }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        void btnAddResponse_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem != null)
            {
                EditorNPCChatResponse response = new EditorNPCChatResponse("<New Response>");
                EditingObjAsDialogItem.ResponseList.Add(response);
                npcChatDialogView.UpdateItems();

                // TODO: Auto-select the new node for the response
            }
        }

        void btnDeleteResponse_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsResponse != null)
            {
                EditorNPCChatDialogItem responseSource =
                    CurrentDialog.Items.FirstOrDefault(x => x.ResponseList.Contains(EditingObjAsResponse));
                if (responseSource != null)
                {
                    responseSource.ResponseList.Remove(EditingObjAsResponse);
                    npcChatDialogView.UpdateItems();
                }

                // TODO: Even though the node is deleted from the view, it still exists in the NPCChatDialog
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            // This button is just here for debugging purposes. Ideally, we won't even actually "need" it.
            EditorNPCChatDialog dialog = CurrentDialog;
            npcChatDialogView.NPCChatDialog = null;
            npcChatDialogView.NPCChatDialog = dialog;
            npcChatDialogView.ExpandAll();
        }

        void cmbSelectedDialog_OnChangeDialog(NPCChatDialogComboBox sender, NPCChatDialogBase dialog)
        {
            bool initialDoNotUpdateValue = _doNotUpdateObj;
            _doNotUpdateObj = false;

            // NOTE: Temp removal of saving
            // EditorNPCChatManager.SaveDialogs();

            npcChatDialogView.NPCChatDialog = (EditorNPCChatDialog)dialog;
            npcChatDialogView.ExpandAll();

            txtDialogTitle.Text = CurrentDialog.Title;

            _doNotUpdateObj = initialDoNotUpdateValue;
        }

        /// <summary>
        /// Creates the test dialog.
        /// </summary>
        /// <returns>The test dialog.</returns>
        // ReSharper disable UnusedMember.Local
        static EditorNPCChatDialog CreateTestDialog() // ReSharper restore UnusedMember.Local
        {
            EditorNPCChatDialog dialog = new EditorNPCChatDialog();

            EditorNPCChatDialogItem haveYouDoneThisQuest = new EditorNPCChatDialogItem(0, "Have you done this quest?");
            haveYouDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "False"), new EditorNPCChatResponse(2, "True"));

            EditorNPCChatDialogItem hasNotDoneThisQuest = new EditorNPCChatDialogItem(1, "Think you can help me out?");
            hasNotDoneThisQuest.AddResponse(new EditorNPCChatResponse(3, "Yes"), new EditorNPCChatResponse(4, "No"));

            EditorNPCChatDialogItem acceptHelp = new EditorNPCChatDialogItem(3, "Sweet, thanks!");

            EditorNPCChatDialogItem declineHelp = new EditorNPCChatDialogItem(4, "Fine. Screw you too, you selfish jerk!");

            EditorNPCChatDialogItem hasDoneThisQuest = new EditorNPCChatDialogItem(2, "Sorry dude, you already did this quest!");
            hasDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "So? Just let me fucking do it!"),
                                         new EditorNPCChatResponse("Ok, fine, whatever. Asshole."));

            dialog.Add(new EditorNPCChatDialogItem[]
            {
                haveYouDoneThisQuest, hasNotDoneThisQuest, acceptHelp, declineHelp, hasDoneThisQuest
            });

            return dialog;
        }

        void DisableAllTabsExcept(TabControl tabControl, TabPage enabledTab)
        {
            txtDialogText.Text = string.Empty;

            foreach (TabPage tab in tabControl.TabPages.Cast<TabPage>())
            {
                if (tab == enabledTab)
                {
                    tabControl.SelectedTab = tab;
                    SetAllChildrenEnabled(tab.Controls, true);
                }
                else
                    SetAllChildrenEnabled(tab.Controls, false);
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Set controls that are initially disabled in the tab control to always be disabled
                var controls = GetAllControls(tcChatDialogItem);
                foreach (var control in controls)
                {
                    if (control.Enabled == false)
                        control.EnabledChanged += ((obj, eArgs) => ((Control)obj).Enabled = false);
                }

                // Add the dialogs
                cmbSelectedDialog.Items.Clear();
                cmbSelectedDialog.AddDialog(EditorNPCChatManager.Dialogs.OfType<NPCChatDialogBase>());

                // Select the first one
                if (cmbSelectedDialog.Items.Count > 0)
                    cmbSelectedDialog.SelectedIndex = 0;

                comboBox1.Items.Clear();
                foreach (var item in NPCChatConditional.Conditionals)
                    comboBox1.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // NOTE: Temp removal of saving
            // EditorNPCChatManager.SaveDialogs();
        }

        void frmMain_Resize(object sender, EventArgs e)
        {
            gbSelectedNode.Top = ClientSize.Height - gbSelectedNode.Height - gbSelectedNode.Left;
            gbSelectedNode.Width = ClientSize.Width - (gbSelectedNode.Left * 2);

            npcChatDialogView.Width = ClientSize.Width - (npcChatDialogView.Left * 2);
            npcChatDialogView.Height = gbSelectedNode.Top - (npcChatDialogView.Top * 2) + 6;

            button1.Left = ClientSize.Width - button1.Width - 3;
        }

        void gbSelectedNode_Resize(object sender, EventArgs e)
        {
            txtTitle.Width = gbSelectedNode.ClientSize.Width - txtTitle.Left - 6;
        }

        void npcChatDialogView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetEditingObject(e.Node.Tag);
        }

        static System.Collections.Generic.IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (var child in root.Controls.Cast<Control>())
            {
                yield return child;
                foreach (var c2 in GetAllControls(child))
                    yield return c2;
            }
        }

        static void SetAllChildrenEnabled(IEnumerable controls, bool enabled)
        {
            foreach (Control child in controls.OfType<Control>())
            {
                SetAllChildrenEnabled(child.Controls, enabled);
                child.Enabled = enabled;
            }
        }

        void SetEditingObject(object obj)
        {
            if (_editingObj == obj)
                return;

            _doNotUpdateObj = true;
            _editingObj = obj;

            if (obj is EditorNPCChatDialogItem)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpDialog);
                txtTitle.Enabled = true;

                txtTitle.Text = EditingObjAsDialogItem.Title;
                txtDialogText.Text = EditingObjAsDialogItem.Text;
                txtDialogPage.Text = EditingObjAsDialogItem.Index.ToString();
            }
            else if (obj is EditorNPCChatResponse)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpResponse);
                txtTitle.Enabled = true;

                txtTitle.Text = EditingObjAsResponse.Text;
                txtDialogText.Text = EditingObjAsResponse.Text;
                txtResponseIndex.Text = EditingObjAsResponse.Page.ToString();
                txtResponseValue.Text = EditingObjAsResponse.Value.ToString();
            }
            else if (obj is TreeNode)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpRedirect);
                txtTitle.Enabled = false;

                EditorNPCChatDialogItem redirectTo = (EditorNPCChatDialogItem)EditingObjAsTreeNode.Tag;
                txtTitle.Text = redirectTo.Text;
                txtRedirectIndex.Text = redirectTo.Index.ToString();
            }
            else
                SetAllChildrenEnabled(tcChatDialogItem.Controls, false);

            _doNotUpdateObj = false;
        }

        void txtDialogText_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem != null)
                EditingObjAsDialogItem.SetText(txtDialogText.Text);
        }

        void txtDialogTitle_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (CurrentDialog != null)
                CurrentDialog.SetTitle(txtDialogTitle.Text);
        }

        void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem != null)
                EditingObjAsDialogItem.SetTitle(txtTitle.Text);
            else if (EditingObjAsResponse != null)
                EditingObjAsResponse.SetText(txtTitle.Text);
        }

        private void btnDeleteDialog_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem == null)
                return;

            if (npcChatDialogView.SelectedNode == null || npcChatDialogView.SelectedNode.Tag != _editingObj)
            {
                Debug.Fail("...");
                return;
            }

            // Make sure this isn't the dialog
            if (EditingObjAsDialogItem.Index == 0)
            {
                MessageBox.Show("Cannot delete the root dialog item.", "Cannot delete", MessageBoxButtons.OK);
                return;
            }

            // Make sure nothing is redirecting to this node
            var responsesToThisDialog = CurrentDialog.GetSourceResponses(EditingObjAsDialogItem);

            int redirectCount = responsesToThisDialog.Count() - 1;
            if (redirectCount > 0)
            {
                string redirectToThisMsg = string.Format("Cannot delete this dialog because there are {0} redirects to it.", redirectCount);
                MessageBox.Show(redirectToThisMsg, "Cannot delete", MessageBoxButtons.OK);
                return;
            }

            // Grab the child nodes
            var children = GetChildNodes(npcChatDialogView.SelectedNode);
            var redirectNodes = children.Where(x => x.Tag is TreeNode);
            var dialogNodes = children.Where(x => x.Tag is EditorNPCChatDialogItem);
            var responseNodes = children.Where(x => x.Tag is EditorNPCChatResponse);

            // Make sure none of the child nodes are being redirected to
            var redirectedToItems = dialogNodes.Select(x => (EditorNPCChatDialogItem)x.Tag).Where(x => CurrentDialog.GetSourceResponses(x).Count() > 1);

            if (redirectedToItems.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Cannot delete this node because the following child node(s) are being redirect to:");
                foreach (var item in redirectedToItems)
                    sb.AppendLine(" " + item.Index + ": " + item.Title);
                MessageBox.Show(sb.ToString(), "Cannot delete", MessageBoxButtons.OK);
                return;
            }

            // Ask for confirmation to delete
            const string dialogInfoMsgBase = "This dialog contains the following:" +
                "{0}Redirects: {1}{0}Dialogs: {2}{0}Responses: {3}" +
                "{0}{0}Are you sure you wish to delete it?";
            string dialogInfoMsg = string.Format(dialogInfoMsgBase, Environment.NewLine, redirectNodes.Count(), dialogNodes.Count(), responseNodes.Count());

            if (MessageBox.Show(dialogInfoMsg, "Delete dialog?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Delete
            CurrentDialog.RemoveDialogItem(EditingObjAsDialogItem);
            foreach (var item in dialogNodes.Select(x => (EditorNPCChatDialogItem)x.Tag))
                CurrentDialog.RemoveDialogItem(item);

            // TODO: Refresh correctly instead of refreshing the whole thing
            button1_Click(null, null);
        }

        static IEnumerable<TreeNode> GetChildNodes(TreeNode root)
        {
            foreach (var node in root.Nodes.Cast<TreeNode>())
            {
                yield return node;
                foreach (var n2 in GetChildNodes(node))
                    yield return n2;
            }
        }

        private void btnDeleteRedirect_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsTreeNode == null)
                return;

            var parentNode = npcChatDialogView.SelectedNode.Parent;
            if (parentNode == null)
                return;

            var parent = parentNode.Tag as EditorNPCChatResponse;
            if (parent == null)
                return;

            parent.SetPage(EditorNPCChatResponse.EndConversationPage);

            // TODO: Refresh correctly instead of refreshing the whole thing
            npcChatDialogView.UpdateItems();
            //button1_Click(null, null);
        }

        private void btnAddDialog_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsResponse == null)
                return;


        }
    }
}