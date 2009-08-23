using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools;
using NetGore.IO;

namespace DemoGame.NPCChatEditor
{
    public partial class frmMain : Form
    {
        EditorNPCChatDialog _dialog;
        bool _doNotUpdateObj;
        object _editingObj;

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
                    _dialog.Items.FirstOrDefault(x => x.ResponseList.Contains(EditingObjAsResponse));
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

            npcChatDialogView.NPCChatDialog = null;
            npcChatDialogView.NPCChatDialog = _dialog;
            npcChatDialogView.ExpandAll();

            BitStream bs = new BitStream(BitStreamMode.Write, 8192);

            // NOTE: Temp
            string filePath = ContentPaths.Build.Data.Join("TestChat.xml");
            using (XmlValueWriter writer = new XmlValueWriter(filePath, "ChatDialogs"))
            {
                writer.WriteStartNode("ChatDialog");
                _dialog.Write(writer);
                writer.WriteEndNode("ChatDialog");
            }

            XmlValueReader reader = new XmlValueReader(filePath, "ChatDialogs");
            IValueReader chatDialogReader = reader.ReadNodes("ChatDialog", 1).First();
            EditorNPCChatDialog newChatDialog = new EditorNPCChatDialog();
            newChatDialog.Read(chatDialogReader);

            npcChatDialogView.NPCChatDialog = newChatDialog;
            npcChatDialogView.ExpandAll();
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
            _dialog = new EditorNPCChatDialog();

            EditorNPCChatDialogItem haveYouDoneThisQuest = new EditorNPCChatDialogItem(0, "Have you done this quest?");
            haveYouDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "False"), new EditorNPCChatResponse(2, "True"));

            EditorNPCChatDialogItem hasNotDoneThisQuest = new EditorNPCChatDialogItem(1, "Think you can help me out?");
            hasNotDoneThisQuest.AddResponse(new EditorNPCChatResponse(3, "Yes"), new EditorNPCChatResponse(4, "No"));

            EditorNPCChatDialogItem acceptHelp = new EditorNPCChatDialogItem(3, "Sweet, thanks!");

            EditorNPCChatDialogItem declineHelp = new EditorNPCChatDialogItem(4, "Fine. Screw you too, you selfish jerk!");

            EditorNPCChatDialogItem hasDoneThisQuest = new EditorNPCChatDialogItem(2, "Sorry dude, you already did this quest!");
            hasDoneThisQuest.AddResponse(new EditorNPCChatResponse(1, "So? Just let me fucking do it!"),
                new EditorNPCChatResponse("Ok, fine, whatever. Asshole."));

            _dialog.Add(new EditorNPCChatDialogItem[]
            { haveYouDoneThisQuest, hasNotDoneThisQuest, acceptHelp, declineHelp, hasDoneThisQuest });

            npcChatDialogView.NPCChatDialog = _dialog;
            npcChatDialogView.ExpandAll();
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