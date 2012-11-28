using NetGore.Features.NPCChat;

namespace DemoGame.Editor
{
    partial class NPCChatEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.cmbSelectedDialog = new NetGore.Features.NPCChat.NPCChatDialogComboBox();
            this.txtDialogTitle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.npcChatDialogView = new NetGore.Features.NPCChat.NPCChatDialogView();
            this.gbSelectedNode = new System.Windows.Forms.GroupBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcChatDialogItem = new System.Windows.Forms.TabControl();
            this.tpDialog = new System.Windows.Forms.TabPage();
            this.chkIsBranch = new System.Windows.Forms.CheckBox();
            this.btnDeleteDialog = new System.Windows.Forms.Button();
            this.btnAddResponse = new System.Windows.Forms.Button();
            this.txtDialogPage = new System.Windows.Forms.TextBox();
            this.txtDialogText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tpResponse = new System.Windows.Forms.TabPage();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.cmbAddAction = new System.Windows.Forms.ComboBox();
            this.lstActions = new System.Windows.Forms.ListBox();
            this.btnAddRedirect = new System.Windows.Forms.Button();
            this.btnAddDialog = new System.Windows.Forms.Button();
            this.txtResponseValue = new System.Windows.Forms.TextBox();
            this.txtResponseIndex = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeleteResponse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tpRedirect = new System.Windows.Forms.TabPage();
            this.btnDeleteRedirect = new System.Windows.Forms.Button();
            this.txtRedirectID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbConditionals = new System.Windows.Forms.GroupBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.lstConditionals = new NetGore.Features.NPCChat.NPCChatConditionalsListBox();
            this.cmbEvaluateType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAddConditional = new System.Windows.Forms.Button();
            this.btnDeleteConditional = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbSelectedNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.tcChatDialogItem.SuspendLayout();
            this.tpDialog.SuspendLayout();
            this.tpResponse.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.tpRedirect.SuspendLayout();
            this.gbConditionals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            this.splitContainer1.Panel1.Controls.Add(this.btnNew);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnDelete);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(930, 623);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.TabIndex = 12;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.cmbSelectedDialog);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtDialogTitle);
            this.splitContainer3.Panel2.Controls.Add(this.label6);
            this.splitContainer3.Size = new System.Drawing.Size(698, 25);
            this.splitContainer3.SplitterDistance = 278;
            this.splitContainer3.TabIndex = 41;
            // 
            // cmbSelectedDialog
            // 
            this.cmbSelectedDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSelectedDialog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSelectedDialog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectedDialog.FormattingEnabled = true;
            this.cmbSelectedDialog.Location = new System.Drawing.Point(0, 0);
            this.cmbSelectedDialog.Name = "cmbSelectedDialog";
            this.cmbSelectedDialog.Size = new System.Drawing.Size(278, 21);
            this.cmbSelectedDialog.TabIndex = 13;
            this.cmbSelectedDialog.SelectedValueChanged += new System.EventHandler(this.cmbSelectedDialog_SelectedValueChanged);
            // 
            // txtDialogTitle
            // 
            this.txtDialogTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDialogTitle.Location = new System.Drawing.Point(30, 0);
            this.txtDialogTitle.Name = "txtDialogTitle";
            this.txtDialogTitle.Size = new System.Drawing.Size(386, 20);
            this.txtDialogTitle.TabIndex = 27;
            this.txtDialogTitle.TextChanged += new System.EventHandler(this.txtDialogTitle_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label6.Size = new System.Drawing.Size(30, 16);
            this.label6.TabIndex = 26;
            this.label6.Text = "Title:";
            // 
            // btnNew
            // 
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNew.Location = new System.Drawing.Point(698, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(58, 25);
            this.btnNew.TabIndex = 40;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(756, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 25);
            this.btnSave.TabIndex = 39;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(814, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(58, 25);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefresh.Location = new System.Drawing.Point(872, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(58, 25);
            this.btnRefresh.TabIndex = 37;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.npcChatDialogView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gbSelectedNode);
            this.splitContainer2.Size = new System.Drawing.Size(930, 594);
            this.splitContainer2.SplitterDistance = 345;
            this.splitContainer2.TabIndex = 13;
            // 
            // npcChatDialogView
            // 
            this.npcChatDialogView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.npcChatDialogView.Location = new System.Drawing.Point(0, 0);
            this.npcChatDialogView.Name = "npcChatDialogView";
            this.npcChatDialogView.NodeForeColorBranch = System.Drawing.Color.DarkRed;
            this.npcChatDialogView.NodeForeColorGoTo = System.Drawing.Color.Blue;
            this.npcChatDialogView.NodeForeColorNormal = System.Drawing.Color.Black;
            this.npcChatDialogView.NodeForeColorResponse = System.Drawing.Color.Green;
            this.npcChatDialogView.NPCChatDialog = null;
            this.npcChatDialogView.Size = new System.Drawing.Size(930, 345);
            this.npcChatDialogView.TabIndex = 1;
            this.npcChatDialogView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.npcChatDialogView_NodeMouseClick);
            // 
            // gbSelectedNode
            // 
            this.gbSelectedNode.Controls.Add(this.splitContainer5);
            this.gbSelectedNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelectedNode.Location = new System.Drawing.Point(0, 0);
            this.gbSelectedNode.Name = "gbSelectedNode";
            this.gbSelectedNode.Size = new System.Drawing.Size(930, 245);
            this.gbSelectedNode.TabIndex = 3;
            this.gbSelectedNode.TabStop = false;
            this.gbSelectedNode.Text = "Selected Node";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(3, 16);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.gbConditionals);
            this.splitContainer5.Size = new System.Drawing.Size(924, 226);
            this.splitContainer5.SplitterDistance = 466;
            this.splitContainer5.TabIndex = 16;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer6.IsSplitterFixed = true;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.txtTitle);
            this.splitContainer6.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.tcChatDialogItem);
            this.splitContainer6.Size = new System.Drawing.Size(466, 226);
            this.splitContainer6.SplitterDistance = 25;
            this.splitContainer6.TabIndex = 7;
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Enabled = false;
            this.txtTitle.Location = new System.Drawing.Point(30, 0);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(436, 20);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Title:";
            // 
            // tcChatDialogItem
            // 
            this.tcChatDialogItem.Controls.Add(this.tpDialog);
            this.tcChatDialogItem.Controls.Add(this.tpResponse);
            this.tcChatDialogItem.Controls.Add(this.tpRedirect);
            this.tcChatDialogItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcChatDialogItem.Location = new System.Drawing.Point(0, 0);
            this.tcChatDialogItem.Name = "tcChatDialogItem";
            this.tcChatDialogItem.SelectedIndex = 0;
            this.tcChatDialogItem.Size = new System.Drawing.Size(466, 197);
            this.tcChatDialogItem.TabIndex = 7;
            // 
            // tpDialog
            // 
            this.tpDialog.Controls.Add(this.chkIsBranch);
            this.tpDialog.Controls.Add(this.btnDeleteDialog);
            this.tpDialog.Controls.Add(this.btnAddResponse);
            this.tpDialog.Controls.Add(this.txtDialogPage);
            this.tpDialog.Controls.Add(this.txtDialogText);
            this.tpDialog.Controls.Add(this.label5);
            this.tpDialog.Controls.Add(this.label3);
            this.tpDialog.Location = new System.Drawing.Point(4, 22);
            this.tpDialog.Name = "tpDialog";
            this.tpDialog.Padding = new System.Windows.Forms.Padding(3);
            this.tpDialog.Size = new System.Drawing.Size(458, 171);
            this.tpDialog.TabIndex = 0;
            this.tpDialog.Text = "Dialog";
            this.tpDialog.UseVisualStyleBackColor = true;
            // 
            // chkIsBranch
            // 
            this.chkIsBranch.AutoSize = true;
            this.chkIsBranch.Location = new System.Drawing.Point(98, 139);
            this.chkIsBranch.Name = "chkIsBranch";
            this.chkIsBranch.Size = new System.Drawing.Size(71, 17);
            this.chkIsBranch.TabIndex = 9;
            this.chkIsBranch.Text = "Is Branch";
            this.chkIsBranch.UseVisualStyleBackColor = true;
            this.chkIsBranch.CheckedChanged += new System.EventHandler(this.chkIsBranch_CheckedChanged);
            // 
            // btnDeleteDialog
            // 
            this.btnDeleteDialog.Location = new System.Drawing.Point(367, 136);
            this.btnDeleteDialog.Name = "btnDeleteDialog";
            this.btnDeleteDialog.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteDialog.TabIndex = 8;
            this.btnDeleteDialog.Text = "Delete";
            this.btnDeleteDialog.UseVisualStyleBackColor = true;
            this.btnDeleteDialog.Click += new System.EventHandler(this.btnDeleteDialog_Click);
            // 
            // btnAddResponse
            // 
            this.btnAddResponse.Location = new System.Drawing.Point(272, 136);
            this.btnAddResponse.Name = "btnAddResponse";
            this.btnAddResponse.Size = new System.Drawing.Size(89, 27);
            this.btnAddResponse.TabIndex = 6;
            this.btnAddResponse.Text = "Add Response";
            this.btnAddResponse.UseVisualStyleBackColor = true;
            this.btnAddResponse.Click += new System.EventHandler(this.btnAddResponse_Click);
            // 
            // txtDialogPage
            // 
            this.txtDialogPage.Enabled = false;
            this.txtDialogPage.Location = new System.Drawing.Point(47, 136);
            this.txtDialogPage.Name = "txtDialogPage";
            this.txtDialogPage.Size = new System.Drawing.Size(45, 20);
            this.txtDialogPage.TabIndex = 5;
            // 
            // txtDialogText
            // 
            this.txtDialogText.Location = new System.Drawing.Point(6, 19);
            this.txtDialogText.Multiline = true;
            this.txtDialogText.Name = "txtDialogText";
            this.txtDialogText.Size = new System.Drawing.Size(413, 111);
            this.txtDialogText.TabIndex = 1;
            this.txtDialogText.TextChanged += new System.EventHandler(this.txtDialogText_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Page:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text:";
            // 
            // tpResponse
            // 
            this.tpResponse.Controls.Add(this.gbActions);
            this.tpResponse.Controls.Add(this.btnAddRedirect);
            this.tpResponse.Controls.Add(this.btnAddDialog);
            this.tpResponse.Controls.Add(this.txtResponseValue);
            this.tpResponse.Controls.Add(this.txtResponseIndex);
            this.tpResponse.Controls.Add(this.label7);
            this.tpResponse.Controls.Add(this.btnDeleteResponse);
            this.tpResponse.Controls.Add(this.label2);
            this.tpResponse.Location = new System.Drawing.Point(4, 22);
            this.tpResponse.Name = "tpResponse";
            this.tpResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tpResponse.Size = new System.Drawing.Size(458, 171);
            this.tpResponse.TabIndex = 1;
            this.tpResponse.Text = "Response";
            this.tpResponse.UseVisualStyleBackColor = true;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnAddAction);
            this.gbActions.Controls.Add(this.cmbAddAction);
            this.gbActions.Controls.Add(this.lstActions);
            this.gbActions.Location = new System.Drawing.Point(6, 6);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(198, 157);
            this.gbActions.TabIndex = 12;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // btnAddAction
            // 
            this.btnAddAction.Location = new System.Drawing.Point(153, 130);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(39, 21);
            this.btnAddAction.TabIndex = 8;
            this.btnAddAction.Text = "Add";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // cmbAddAction
            // 
            this.cmbAddAction.FormattingEnabled = true;
            this.cmbAddAction.Location = new System.Drawing.Point(6, 130);
            this.cmbAddAction.Name = "cmbAddAction";
            this.cmbAddAction.Size = new System.Drawing.Size(141, 21);
            this.cmbAddAction.TabIndex = 1;
            // 
            // lstActions
            // 
            this.lstActions.FormattingEnabled = true;
            this.lstActions.Location = new System.Drawing.Point(6, 16);
            this.lstActions.Name = "lstActions";
            this.lstActions.Size = new System.Drawing.Size(186, 108);
            this.lstActions.TabIndex = 0;
            this.lstActions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstActions_KeyDown);
            // 
            // btnAddRedirect
            // 
            this.btnAddRedirect.Location = new System.Drawing.Point(210, 136);
            this.btnAddRedirect.Name = "btnAddRedirect";
            this.btnAddRedirect.Size = new System.Drawing.Size(77, 27);
            this.btnAddRedirect.TabIndex = 11;
            this.btnAddRedirect.Text = "Add Redirect";
            this.btnAddRedirect.UseVisualStyleBackColor = true;
            this.btnAddRedirect.Click += new System.EventHandler(this.btnAddRedirect_Click);
            // 
            // btnAddDialog
            // 
            this.btnAddDialog.Location = new System.Drawing.Point(293, 136);
            this.btnAddDialog.Name = "btnAddDialog";
            this.btnAddDialog.Size = new System.Drawing.Size(68, 27);
            this.btnAddDialog.TabIndex = 10;
            this.btnAddDialog.Text = "Add Dialog";
            this.btnAddDialog.UseVisualStyleBackColor = true;
            this.btnAddDialog.Click += new System.EventHandler(this.btnAddDialog_Click);
            // 
            // txtResponseValue
            // 
            this.txtResponseValue.Enabled = false;
            this.txtResponseValue.Location = new System.Drawing.Point(253, 6);
            this.txtResponseValue.Name = "txtResponseValue";
            this.txtResponseValue.Size = new System.Drawing.Size(48, 20);
            this.txtResponseValue.TabIndex = 9;
            // 
            // txtResponseIndex
            // 
            this.txtResponseIndex.Enabled = false;
            this.txtResponseIndex.Location = new System.Drawing.Point(280, 32);
            this.txtResponseIndex.Name = "txtResponseIndex";
            this.txtResponseIndex.Size = new System.Drawing.Size(48, 20);
            this.txtResponseIndex.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Value:";
            // 
            // btnDeleteResponse
            // 
            this.btnDeleteResponse.Location = new System.Drawing.Point(367, 136);
            this.btnDeleteResponse.Name = "btnDeleteResponse";
            this.btnDeleteResponse.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteResponse.TabIndex = 7;
            this.btnDeleteResponse.Text = "Delete";
            this.btnDeleteResponse.UseVisualStyleBackColor = true;
            this.btnDeleteResponse.Click += new System.EventHandler(this.btnDeleteResponse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Page Index:";
            // 
            // tpRedirect
            // 
            this.tpRedirect.Controls.Add(this.btnDeleteRedirect);
            this.tpRedirect.Controls.Add(this.txtRedirectID);
            this.tpRedirect.Controls.Add(this.label4);
            this.tpRedirect.Location = new System.Drawing.Point(4, 22);
            this.tpRedirect.Name = "tpRedirect";
            this.tpRedirect.Size = new System.Drawing.Size(458, 171);
            this.tpRedirect.TabIndex = 2;
            this.tpRedirect.Text = "Redirect";
            this.tpRedirect.UseVisualStyleBackColor = true;
            // 
            // btnDeleteRedirect
            // 
            this.btnDeleteRedirect.Location = new System.Drawing.Point(367, 139);
            this.btnDeleteRedirect.Name = "btnDeleteRedirect";
            this.btnDeleteRedirect.Size = new System.Drawing.Size(52, 27);
            this.btnDeleteRedirect.TabIndex = 10;
            this.btnDeleteRedirect.Text = "Delete";
            this.btnDeleteRedirect.UseVisualStyleBackColor = true;
            this.btnDeleteRedirect.Click += new System.EventHandler(this.btnDeleteRedirect_Click);
            // 
            // txtRedirectID
            // 
            this.txtRedirectID.Location = new System.Drawing.Point(45, 6);
            this.txtRedirectID.Name = "txtRedirectID";
            this.txtRedirectID.Size = new System.Drawing.Size(48, 20);
            this.txtRedirectID.TabIndex = 6;
            this.txtRedirectID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRedirectID_KeyDown);
            this.txtRedirectID.Leave += new System.EventHandler(this.txtRedirectID_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Index:";
            // 
            // gbConditionals
            // 
            this.gbConditionals.Controls.Add(this.splitContainer7);
            this.gbConditionals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbConditionals.Location = new System.Drawing.Point(0, 0);
            this.gbConditionals.Name = "gbConditionals";
            this.gbConditionals.Size = new System.Drawing.Size(454, 226);
            this.gbConditionals.TabIndex = 16;
            this.gbConditionals.TabStop = false;
            this.gbConditionals.Text = "Conditionals";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.IsSplitterFixed = true;
            this.splitContainer7.Location = new System.Drawing.Point(3, 16);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.lstConditionals);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.cmbEvaluateType);
            this.splitContainer7.Panel2.Controls.Add(this.label8);
            this.splitContainer7.Panel2.Controls.Add(this.btnAddConditional);
            this.splitContainer7.Panel2.Controls.Add(this.btnDeleteConditional);
            this.splitContainer7.Size = new System.Drawing.Size(448, 207);
            this.splitContainer7.SplitterDistance = 177;
            this.splitContainer7.TabIndex = 9;
            // 
            // lstConditionals
            // 
            this.lstConditionals.ConditionalCollection = null;
            this.lstConditionals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConditionals.EvaluationTypeComboBox = null;
            this.lstConditionals.FormattingEnabled = true;
            this.lstConditionals.Location = new System.Drawing.Point(0, 0);
            this.lstConditionals.Name = "lstConditionals";
            this.lstConditionals.Size = new System.Drawing.Size(448, 177);
            this.lstConditionals.TabIndex = 9;
            this.lstConditionals.DoubleClick += new System.EventHandler(this.lstConditionals_DoubleClick);
            // 
            // cmbEvaluateType
            // 
            this.cmbEvaluateType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEvaluateType.FormattingEnabled = true;
            this.cmbEvaluateType.Location = new System.Drawing.Point(87, 0);
            this.cmbEvaluateType.Name = "cmbEvaluateType";
            this.cmbEvaluateType.Size = new System.Drawing.Size(257, 21);
            this.cmbEvaluateType.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Evaluation Type:";
            // 
            // btnAddConditional
            // 
            this.btnAddConditional.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddConditional.Location = new System.Drawing.Point(344, 0);
            this.btnAddConditional.Name = "btnAddConditional";
            this.btnAddConditional.Size = new System.Drawing.Size(52, 26);
            this.btnAddConditional.TabIndex = 25;
            this.btnAddConditional.Text = "Add";
            this.btnAddConditional.UseVisualStyleBackColor = true;
            this.btnAddConditional.Click += new System.EventHandler(this.btnAddConditional_Click);
            // 
            // btnDeleteConditional
            // 
            this.btnDeleteConditional.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteConditional.Location = new System.Drawing.Point(396, 0);
            this.btnDeleteConditional.Name = "btnDeleteConditional";
            this.btnDeleteConditional.Size = new System.Drawing.Size(52, 26);
            this.btnDeleteConditional.TabIndex = 24;
            this.btnDeleteConditional.Text = "Delete";
            this.btnDeleteConditional.UseVisualStyleBackColor = true;
            this.btnDeleteConditional.Click += new System.EventHandler(this.btnDeleteConditional_Click);
            // 
            // NPCChatEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 629);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "NPCChatEditorForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPC Chat Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.gbSelectedNode.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.tcChatDialogItem.ResumeLayout(false);
            this.tpDialog.ResumeLayout(false);
            this.tpDialog.PerformLayout();
            this.tpResponse.ResumeLayout(false);
            this.tpResponse.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.tpRedirect.ResumeLayout(false);
            this.tpRedirect.PerformLayout();
            this.gbConditionals.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private NPCChatDialogView npcChatDialogView;
        private System.Windows.Forms.GroupBox gbSelectedNode;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tcChatDialogItem;
        private System.Windows.Forms.TabPage tpDialog;
        private System.Windows.Forms.CheckBox chkIsBranch;
        private System.Windows.Forms.Button btnDeleteDialog;
        private System.Windows.Forms.Button btnAddResponse;
        private System.Windows.Forms.TextBox txtDialogPage;
        private System.Windows.Forms.TextBox txtDialogText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpResponse;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnAddAction;
        private System.Windows.Forms.ComboBox cmbAddAction;
        private System.Windows.Forms.ListBox lstActions;
        private System.Windows.Forms.Button btnAddRedirect;
        private System.Windows.Forms.Button btnAddDialog;
        private System.Windows.Forms.TextBox txtResponseValue;
        private System.Windows.Forms.TextBox txtResponseIndex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDeleteResponse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tpRedirect;
        private System.Windows.Forms.Button btnDeleteRedirect;
        private System.Windows.Forms.TextBox txtRedirectID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbConditionals;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private NPCChatConditionalsListBox lstConditionals;
        private System.Windows.Forms.ComboBox cmbEvaluateType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAddConditional;
        private System.Windows.Forms.Button btnDeleteConditional;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private NPCChatDialogComboBox cmbSelectedDialog;
        private System.Windows.Forms.TextBox txtDialogTitle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;


    }
}

