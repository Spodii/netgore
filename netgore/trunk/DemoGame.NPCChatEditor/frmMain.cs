using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools;
using NetGore.EditorTools.NPCChat;
using NetGore.Globalization;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

// TODO: Do not allow responses to be added to a EditorNPCChatDialogItem where IsBranch is set
// TODO: Do not allow responses immediately under a branch to be altered in any ways
// TODO: Ability to change the order of the actions
// TODO: An indicator for when a dialog characterID or response has conditionals
// TODO: An indicator for when a response has actions

namespace DemoGame.NPCChatEditor
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// Contains the <see cref="NPCChatConditionalBase"/>s available.
        /// </summary>
        static readonly NPCChatConditionalBase[] _npcChatConditionals;

        bool _doNotUpdateObj;
        object _editingObj;

        /// <summary>
        /// Initializes the <see cref="frmMain"/> class.
        /// </summary>
        static frmMain()
        {
            _npcChatConditionals = NPCChatConditionalBase.Conditionals.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMain"/> class.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the EditorNPCChatDialog currently being edited.
        /// </summary>
        EditorNPCChatDialog CurrentDialog
        {
            get { return npcChatDialogView.NPCChatDialog; }
        }

        /// <summary>
        /// Gets the object currently being edited as an EditorNPCChatDialogItem.
        /// </summary>
        EditorNPCChatDialogItem EditingObjAsDialogItem
        {
            get { return _editingObj as EditorNPCChatDialogItem; }
        }

        /// <summary>
        /// Gets the object currently being edited as an EditorNPCChatResponse.
        /// </summary>
        EditorNPCChatResponse EditingObjAsResponse
        {
            get { return _editingObj as EditorNPCChatResponse; }
        }

        /// <summary>
        /// Gets the object currently being edited as a TreeNode.
        /// </summary>
        TreeNode EditingObjAsTreeNode
        {
            get { return _editingObj as TreeNode; }
        }

        void btnAddAction_Click(object sender, EventArgs e)
        {
            if (EditingObjAsResponse == null)
                return;

            var item = cmbAddAction.SelectedItem as NPCChatResponseActionBase;
            if (item == null)
                return;

            EditingObjAsResponse.ActionsList.Add(item);
            UpdateActionsList();
        }

        /// <summary>
        /// Handles the Click event of the btnAddConditional control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddConditional_Click(object sender, EventArgs e)
        {
            var newItem = new EditorNPCChatConditionalCollectionItem(_npcChatConditionals[0]);
            lstConditionals.TryAddToConditionalCollection(newItem);
        }

        /// <summary>
        /// Handles the Click event of the btnAddDialog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddDialog_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsResponse == null)
                return;

            // Make sure there isn't already a dialog characterID
            if (EditingObjAsResponse.Page != EditorNPCChatResponse.EndConversationPage)
            {
                MessageBox.Show("This response already has a dialog characterID.");
                return;
            }

            // Create the new dialog characterID
            var index = CurrentDialog.GetFreeDialogItemIndex();
            var newDialogItem = new EditorNPCChatDialogItem(index, "New dialog characterID");
            CurrentDialog.Add(newDialogItem);

            // Hook it to the response
            EditingObjAsResponse.SetPage(index);

            // Update the tree
            npcChatDialogView.UpdateTree();

            // TODO: Select the new dialog characterID in the tree
        }

        /// <summary>
        /// Handles the Click event of the btnAddRedirect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddRedirect_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsResponse == null)
                return;

            // Make sure there isn't already a dialog characterID
            if (EditingObjAsResponse.Page != EditorNPCChatResponse.EndConversationPage)
            {
                MessageBox.Show("This response already has a dialog characterID.");
                return;
            }

            // Set the redirection
            EditingObjAsResponse.SetPage(0);

            // TODO: Properly update the tree
            button1_Click(null, null);

            // TODO: Select the new dialog characterID in the tree
        }

        /// <summary>
        /// Handles the Click event of the btnAddResponse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnAddResponse_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem == null)
                return;

            var response = new EditorNPCChatResponse("<New Response>");
            EditingObjAsDialogItem.ResponseList.Add(response);
            npcChatDialogView.UpdateTree();

            // TODO: Auto-select the new node for the response
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteConditional control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteConditional_Click(object sender, EventArgs e)
        {
            lstConditionals.TryDeleteSelectedConditionalItem();
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteDialog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteDialog_Click(object sender, EventArgs e)
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
                MessageBox.Show("Cannot delete the root dialog characterID.");
                return;
            }

            // Make sure nothing is redirecting to this node
            var responsesToThisDialog = CurrentDialog.GetSourceResponses(EditingObjAsDialogItem);

            var redirectCount = responsesToThisDialog.Count() - 1;
            if (redirectCount > 0)
            {
                MessageBox.Show(string.Format("Cannot delete this dialog because there are {0} redirects to it.", redirectCount));
                return;
            }

            // Grab the child nodes
            var children = GetChildNodes(npcChatDialogView.SelectedNode);
            var redirectNodes = children.Where(x => x.Tag is TreeNode);
            var dialogNodes = children.Where(x => x.Tag is EditorNPCChatDialogItem);
            var responseNodes = children.Where(x => x.Tag is EditorNPCChatResponse);

            // Make sure none of the child nodes are being redirected to
            var redirectedToItems =
                dialogNodes.Select(x => (EditorNPCChatDialogItem)x.Tag).Where(x => CurrentDialog.GetSourceResponses(x).Count() > 1);

            if (redirectedToItems.Count() > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Cannot delete this node because the following child node(s) are being redirect to:");
                foreach (var item in redirectedToItems)
                {
                    sb.AppendLine(" " + item.Index + ": " + item.Title);
                }
                MessageBox.Show(sb.ToString());
                return;
            }

            // Ask for confirmation to delete
            const string dialogInfoMsgBase =
                "This dialog contains the following:" + "{0}Redirects: {1}{0}Dialogs: {2}{0}Responses: {3}" +
                "{0}{0}Are you sure you wish to delete it?";
            var dialogInfoMsg = string.Format(dialogInfoMsgBase, Environment.NewLine, redirectNodes.Count(), dialogNodes.Count(),
                                              responseNodes.Count());

            if (MessageBox.Show(dialogInfoMsg, "Delete dialog?", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // Delete
            CurrentDialog.RemoveDialogItem(EditingObjAsDialogItem);
            foreach (var item in dialogNodes.Select(x => (EditorNPCChatDialogItem)x.Tag))
            {
                CurrentDialog.RemoveDialogItem(item);
            }

            // TODO: Refresh correctly instead of refreshing the whole thing
            button1_Click(null, null);
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteRedirect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteRedirect_Click(object sender, EventArgs e)
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

            // TODO: Properly update the view
            button1_Click(null, null);
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteResponse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnDeleteResponse_Click(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsResponse != null)
            {
                var responseSource = CurrentDialog.Items.FirstOrDefault(x => x.ResponseList.Contains(EditingObjAsResponse));
                if (responseSource != null)
                {
                    responseSource.ResponseList.Remove(EditingObjAsResponse);
                    npcChatDialogView.UpdateTree();
                }

                // TODO: Even though the node is deleted from the view, it still exists in the NPCChatDialog
            }
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void button1_Click(object sender, EventArgs e)
        {
            // This button is just here for debugging purposes. Ideally, we won't even actually "need" it.
            var dialog = CurrentDialog;
            npcChatDialogView.NPCChatDialog = null;
            npcChatDialogView.NPCChatDialog = dialog;
            npcChatDialogView.ExpandAll();
        }

        void chkIsBranch_CheckedChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;
            if (EditingObjAsDialogItem == null)
                return;

            if (EditingObjAsDialogItem.IsBranch == chkIsBranch.Checked)
            {
                Debug.Fail("Uh-oh - an inconsistency!");
                return;
            }

            _doNotUpdateObj = true;

            const string msgSetAsNonBranch =
                "Are you sure you wish to change this dialog to a normal dialog?" +
                " Changing to a normal dialog will result in all conditionals for this dialog characterID to be lost.";

            const string msgSetAsBranch =
                "Are you sure you wish to change this dialog to a conditional branch?" +
                " Changing to a conditional branch will result in some alterations being made to your existing responses.";

            try
            {
                string error;
                bool success;

                if (EditingObjAsDialogItem.IsBranch)
                {
                    if (MessageBox.Show(msgSetAsNonBranch, "Accept changes?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    success = EditingObjAsDialogItem.TrySetAsNonBranch(out error);
                }
                else
                {
                    if (MessageBox.Show(msgSetAsBranch, "Accept changes?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    success = EditingObjAsDialogItem.TrySetAsBranch(out error);
                }

                if (!success)
                {
                    const string errmsg = "Failed to apply changes. Reason: {0}";
                    MessageBox.Show(string.Format(errmsg, error));
                }
            }
            finally
            {
                chkIsBranch.Checked = EditingObjAsDialogItem.IsBranch;
                _doNotUpdateObj = false;
            }

            // TODO: Proper updating
            button1_Click(this, null);
        }

        /// <summary>
        /// CMBs the selected dialog_ on change dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="dialog">The dialog.</param>
        void cmbSelectedDialog_OnChangeDialog(NPCChatDialogComboBox sender, NPCChatDialogBase dialog)
        {
            var initialDoNotUpdateValue = _doNotUpdateObj;
            _doNotUpdateObj = false;

            EditorNPCChatManager.SaveDialogs();

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
            var dialog = new EditorNPCChatDialog();

            var haveYouDoneThisQuest = new EditorNPCChatDialogItem(0, "Have you done this quest?");
            haveYouDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "False"), new EditorNPCChatResponse(2, "True"));

            var hasNotDoneThisQuest = new EditorNPCChatDialogItem(1, "Think you can help me out?");
            hasNotDoneThisQuest.AddResponse(new EditorNPCChatResponse(3, "Yes"), new EditorNPCChatResponse(4, "No"));

            var acceptHelp = new EditorNPCChatDialogItem(3, "Sweet, thanks!");

            var declineHelp = new EditorNPCChatDialogItem(4, "Fine. Screw you too, you selfish jerk!");

            var hasDoneThisQuest = new EditorNPCChatDialogItem(2, "Sorry dude, you already did this quest!");
            hasDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "So? Just let me fucking do it!"),
                                         new EditorNPCChatResponse("Ok, fine, whatever. Asshole."));

            dialog.Add(new EditorNPCChatDialogItem[]
            { haveYouDoneThisQuest, hasNotDoneThisQuest, acceptHelp, declineHelp, hasDoneThisQuest });

            return dialog;
        }

        /// <summary>
        /// Disables all tabs in the TabControl except for the given page.
        /// </summary>
        /// <param name="tabControl">The TabControl.</param>
        /// <param name="enabledTab">The TabPage to leave enabled.</param>
        void DisableAllTabsExcept(TabControl tabControl, TabPage enabledTab)
        {
            txtDialogText.Text = string.Empty;

            foreach (var tab in tabControl.TabPages.Cast<TabPage>())
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

        /// <summary>
        /// Handles the FormClosing event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            EditorNPCChatManager.SaveDialogs();
        }

        /// <summary>
        /// Handles the Load event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void frmMain_Load(object sender, EventArgs e)
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

                // Disable conditional controls by default
                SetConditionalsEnabled(false);

                // Add the dialogs
                cmbSelectedDialog.Items.Clear();
                cmbSelectedDialog.AddDialog(EditorNPCChatManager.Dialogs.OfType<NPCChatDialogBase>());

                // Select the first one
                if (cmbSelectedDialog.Items.Count > 0)
                    cmbSelectedDialog.SelectedIndex = 0;

                // Populate the evaluation types
                var evaluationTypes = EnumHelper<NPCChatConditionalEvaluationType>.Values;
                cmbEvaluateType.Items.Clear();
                cmbEvaluateType.Items.AddRange(evaluationTypes.Select(x => (object)x).ToArray());

                // Population the response action types
                var actionTypes = NPCChatResponseActionBase.Conditionals.OrderBy(x => x.Name);
                cmbAddAction.Items.Clear();
                cmbAddAction.Items.AddRange(actionTypes.Select(x => (object)x).ToArray());

                // Perform the initial resize
                frmMain_Resize(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Handles the Resize event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void frmMain_Resize(object sender, EventArgs e)
        {
            gbSelectedNode.Top = ClientSize.Height - gbSelectedNode.Height - gbSelectedNode.Left;
            gbSelectedNode.Width = ClientSize.Width - (gbSelectedNode.Left * 2);

            npcChatDialogView.Width = ClientSize.Width - (npcChatDialogView.Left * 2);
            npcChatDialogView.Height = gbSelectedNode.Top - npcChatDialogView.Top - 6;

            button1.Left = ClientSize.Width - button1.Width - 3;
        }

        /// <summary>
        /// Gets all the Controls from the given <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The root Control.</param>
        /// <returns>All the Controls from the given <paramref name="root"/>.</returns>
        static IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (var child in root.Controls.Cast<Control>())
            {
                yield return child;
                foreach (var c2 in GetAllControls(child))
                {
                    yield return c2;
                }
            }
        }

        /// <summary>
        /// Gets all of the child TreeNodes from the given <paramref name="root"/>.
        /// </summary>
        /// <param name="root">The root TreeNode.</param>
        /// <returns>All of the child TreeNodes from the given <paramref name="root"/>.</returns>
        static IEnumerable<TreeNode> GetChildNodes(TreeNode root)
        {
            foreach (var node in root.Nodes.Cast<TreeNode>())
            {
                yield return node;
                foreach (var n2 in GetChildNodes(node))
                {
                    yield return n2;
                }
            }
        }

        void lstActions_KeyDown(object sender, KeyEventArgs e)
        {
            if (_doNotUpdateObj)
                return;
            if (EditingObjAsResponse == null)
                return;

            if (e.KeyCode == Keys.Delete)
            {
                var item = cmbAddAction.SelectedItem as NPCChatResponseActionBase;
                if (item == null)
                    return;

                EditingObjAsResponse.ActionsList.RemoveAt(lstActions.SelectedIndex);
                lstActions.RemoveItemAtAndReselect(lstActions.SelectedIndex);
            }
        }

        /// <summary>
        /// Handles the DoubleClick event of the lstConditionals control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void lstConditionals_DoubleClick(object sender, EventArgs e)
        {
            lstConditionals.EditCurrentItem(_npcChatConditionals);
        }

        /// <summary>
        /// Handles the NodeMouseClick event of the npcChatDialogView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        void npcChatDialogView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetEditingObject(e.Node.Tag);
        }

        /// <summary>
        /// Sets the enabled value of the child controls from the given <paramref name="controls"/>.
        /// </summary>
        /// <param name="controls">The root control collection.</param>
        /// <param name="enabled">True to set the controls as enabled; false for disabled.</param>
        static void SetAllChildrenEnabled(IEnumerable controls, bool enabled)
        {
            foreach (var child in controls.OfType<Control>())
            {
                SetAllChildrenEnabled(child.Controls, enabled);
                child.Enabled = enabled;
            }
        }

        /// <summary>
        /// Enables the Conditionals list.
        /// </summary>
        /// <param name="enabled">True to enable; false to disable.</param>
        void SetConditionalsEnabled(bool enabled)
        {
            gbConditionals.Enabled = enabled;

            if (enabled == false)
                lstConditionals.ConditionalCollection = null;
        }

        /// <summary>
        /// Sets the NPC chat object being edited.
        /// </summary>
        /// <param name="obj">The NPC chat object.</param>
        void SetEditingObject(object obj)
        {
            if (_editingObj == obj)
                return;

            _doNotUpdateObj = true;
            _editingObj = obj;

            SetConditionalsEnabled(false);

            if (obj is EditorNPCChatDialogItem)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpDialog);
                txtTitle.Enabled = true;

                txtTitle.Text = EditingObjAsDialogItem.Title;
                txtDialogText.Text = EditingObjAsDialogItem.Text;
                txtDialogPage.Text = EditingObjAsDialogItem.Index.ToString();
                chkIsBranch.Checked = EditingObjAsDialogItem.IsBranch;

                if (EditingObjAsDialogItem.IsBranch)
                {
                    SetConditionalsEnabled(true);
                    lstConditionals.SetConditionalCollection(EditingObjAsDialogItem.Conditionals);
                    EditingObjAsDialogItem.SetConditionals(lstConditionals.ConditionalCollection);
                }
            }
            else if (obj is EditorNPCChatResponse)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpResponse);
                txtTitle.Enabled = true;

                txtTitle.Text = EditingObjAsResponse.Text;
                txtDialogText.Text = EditingObjAsResponse.Text;
                txtResponseIndex.Text = EditingObjAsResponse.Page.ToString();
                txtResponseValue.Text = EditingObjAsResponse.Value.ToString();

                SetConditionalsEnabled(true);
                lstConditionals.SetConditionalCollection(EditingObjAsResponse.Conditionals);
                EditingObjAsResponse.SetConditionals(lstConditionals.ConditionalCollection);

                lstActions.Items.Clear();
                lstActions.Items.AddRange(EditingObjAsResponse.Actions.ToArray());
            }
            else if (obj is TreeNode)
            {
                DisableAllTabsExcept(tcChatDialogItem, tpRedirect);
                txtTitle.Enabled = false;

                var redirectTo = (EditorNPCChatDialogItem)EditingObjAsTreeNode.Tag;
                txtTitle.Text = redirectTo.Text;
                txtRedirectIndex.Text = redirectTo.Index.ToString();
            }
            else
                SetAllChildrenEnabled(tcChatDialogItem.Controls, false);

            _doNotUpdateObj = false;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtDialogText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtDialogText_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem != null)
                EditingObjAsDialogItem.SetText(txtDialogText.Text);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtDialogTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtDialogTitle_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (CurrentDialog != null)
                CurrentDialog.SetTitle(txtDialogTitle.Text);
        }

        /// <summary>
        /// Handles the KeyDown event of the txtRedirectIndex control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void txtRedirectIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                txtRedirectIndex_Leave(this, e);
        }

        /// <summary>
        /// Handles the Leave event of the txtRedirectIndex control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtRedirectIndex_Leave(object sender, EventArgs e)
        {
            if (EditingObjAsTreeNode == null || npcChatDialogView.SelectedNode == null ||
                npcChatDialogView.SelectedNode.Tag != _editingObj)
                return;

            if (npcChatDialogView.SelectedNode.Parent == null)
                return;

            var response = npcChatDialogView.SelectedNode.Parent.Tag as EditorNPCChatResponse;
            if (response == null)
                return;

            if (_doNotUpdateObj)
            {
                txtRedirectIndex.Text = response.Page.ToString();
                return;
            }

            ushort newIndex;
            if (!Parser.Current.TryParse(txtRedirectIndex.Text, out newIndex))
            {
                MessageBox.Show("Invalid value entered.");
                txtRedirectIndex.Focus();
                return;
            }

            var newNode = CurrentDialog.GetDialogItem(newIndex);
            if (newNode == null)
            {
                MessageBox.Show("Invalid node page index entered.");
                txtRedirectIndex.Focus();
                return;
            }

            response.SetPage(newIndex);

            // TODO: Properly update the view
            button1_Click(null, null);
        }

        /// <summary>
        /// Handles the TextChanged event of the txtTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (_doNotUpdateObj)
                return;

            if (EditingObjAsDialogItem != null)
                EditingObjAsDialogItem.SetTitle(txtTitle.Text);
            else if (EditingObjAsResponse != null)
                EditingObjAsResponse.SetText(txtTitle.Text);
        }

        void UpdateActionsList()
        {
            if (EditingObjAsResponse == null)
            {
                lstActions.Items.Clear();
                return;
            }

            lstActions.SynchronizeItemList(EditingObjAsResponse.Actions);
        }
    }
}