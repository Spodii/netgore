using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Server.NPCChat.Conditionals;
using NetGore.EditorTools;
using NetGore.NPCChat;

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

        void AlwaysDisabledControl_EnabledChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c == null)
                return;

            c.Enabled = false;
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

            // NOTE: Temp
            EditorNPCChatManager.SaveDialogs();
        }

        void cmbSelectedDialog_OnChangeDialog(NPCChatDialogComboBox sender, NPCChatDialogBase dialog)
        {
            bool initialDoNotUpdateValue = _doNotUpdateObj;
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
                cmbSelectedDialog.Items.Clear();
                cmbSelectedDialog.AddDialog(EditorNPCChatManager.Dialogs.OfType<NPCChatDialogBase>());

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
            EditorNPCChatManager.SaveDialogs();
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
    }
}