using NetGore.EditorTools;

namespace DemoGame.NPCChatEditor
{
    partial class frmMain
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
            this.gbSelectedNode = new System.Windows.Forms.GroupBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcChatDialogItem = new System.Windows.Forms.TabControl();
            this.tpDialog = new System.Windows.Forms.TabPage();
            this.btnAddResponse = new System.Windows.Forms.Button();
            this.txtDialogPage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDialogText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpResponse = new System.Windows.Forms.TabPage();
            this.btnDeleteResponse = new System.Windows.Forms.Button();
            this.txtResponseIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tpRedirect = new System.Windows.Forms.TabPage();
            this.txtRedirectIndex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDialogTitle = new System.Windows.Forms.TextBox();
            this.cmbSelectedDialog = new NetGore.EditorTools.NPCChatDialogComboBox();
            this.npcChatDialogView = new NetGore.EditorTools.NPCChatDialogView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.gbSelectedNode.SuspendLayout();
            this.tcChatDialogItem.SuspendLayout();
            this.tpDialog.SuspendLayout();
            this.tpResponse.SuspendLayout();
            this.tpRedirect.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelectedNode
            // 
            this.gbSelectedNode.Controls.Add(this.txtTitle);
            this.gbSelectedNode.Controls.Add(this.label1);
            this.gbSelectedNode.Controls.Add(this.tcChatDialogItem);
            this.gbSelectedNode.Location = new System.Drawing.Point(12, 373);
            this.gbSelectedNode.Name = "gbSelectedNode";
            this.gbSelectedNode.Size = new System.Drawing.Size(445, 246);
            this.gbSelectedNode.TabIndex = 2;
            this.gbSelectedNode.TabStop = false;
            this.gbSelectedNode.Text = "Selected Node";
            this.gbSelectedNode.Resize += new System.EventHandler(this.gbSelectedNode_Resize);
            // 
            // txtTitle
            // 
            this.txtTitle.Enabled = false;
            this.txtTitle.Location = new System.Drawing.Point(43, 19);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(396, 20);
            this.txtTitle.TabIndex = 4;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Title:";
            // 
            // tcChatDialogItem
            // 
            this.tcChatDialogItem.Controls.Add(this.tpDialog);
            this.tcChatDialogItem.Controls.Add(this.tpResponse);
            this.tcChatDialogItem.Controls.Add(this.tpRedirect);
            this.tcChatDialogItem.Location = new System.Drawing.Point(6, 45);
            this.tcChatDialogItem.Name = "tcChatDialogItem";
            this.tcChatDialogItem.SelectedIndex = 0;
            this.tcChatDialogItem.Size = new System.Drawing.Size(433, 195);
            this.tcChatDialogItem.TabIndex = 2;
            // 
            // tpDialog
            // 
            this.tpDialog.Controls.Add(this.btnAddResponse);
            this.tpDialog.Controls.Add(this.txtDialogPage);
            this.tpDialog.Controls.Add(this.label5);
            this.tpDialog.Controls.Add(this.txtDialogText);
            this.tpDialog.Controls.Add(this.label3);
            this.tpDialog.Location = new System.Drawing.Point(4, 22);
            this.tpDialog.Name = "tpDialog";
            this.tpDialog.Padding = new System.Windows.Forms.Padding(3);
            this.tpDialog.Size = new System.Drawing.Size(425, 169);
            this.tpDialog.TabIndex = 0;
            this.tpDialog.Text = "Dialog";
            this.tpDialog.UseVisualStyleBackColor = true;
            // 
            // btnAddResponse
            // 
            this.btnAddResponse.Location = new System.Drawing.Point(330, 136);
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
            this.txtDialogPage.EnabledChanged += new System.EventHandler(this.AlwaysDisabledControl_EnabledChanged);
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
            // txtDialogText
            // 
            this.txtDialogText.Location = new System.Drawing.Point(6, 19);
            this.txtDialogText.Multiline = true;
            this.txtDialogText.Name = "txtDialogText";
            this.txtDialogText.Size = new System.Drawing.Size(413, 111);
            this.txtDialogText.TabIndex = 1;
            this.txtDialogText.TextChanged += new System.EventHandler(this.txtDialogText_TextChanged);
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
            this.tpResponse.Controls.Add(this.btnDeleteResponse);
            this.tpResponse.Controls.Add(this.txtResponseIndex);
            this.tpResponse.Controls.Add(this.label2);
            this.tpResponse.Location = new System.Drawing.Point(4, 22);
            this.tpResponse.Name = "tpResponse";
            this.tpResponse.Padding = new System.Windows.Forms.Padding(3);
            this.tpResponse.Size = new System.Drawing.Size(425, 169);
            this.tpResponse.TabIndex = 1;
            this.tpResponse.Text = "Response";
            this.tpResponse.UseVisualStyleBackColor = true;
            // 
            // btnDeleteResponse
            // 
            this.btnDeleteResponse.Location = new System.Drawing.Point(330, 136);
            this.btnDeleteResponse.Name = "btnDeleteResponse";
            this.btnDeleteResponse.Size = new System.Drawing.Size(89, 27);
            this.btnDeleteResponse.TabIndex = 7;
            this.btnDeleteResponse.Text = "Delete";
            this.btnDeleteResponse.UseVisualStyleBackColor = true;
            this.btnDeleteResponse.Click += new System.EventHandler(this.btnDeleteResponse_Click);
            // 
            // txtResponseIndex
            // 
            this.txtResponseIndex.Location = new System.Drawing.Point(48, 6);
            this.txtResponseIndex.Name = "txtResponseIndex";
            this.txtResponseIndex.Size = new System.Drawing.Size(48, 20);
            this.txtResponseIndex.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Index:";
            // 
            // tpRedirect
            // 
            this.tpRedirect.Controls.Add(this.txtRedirectIndex);
            this.tpRedirect.Controls.Add(this.label4);
            this.tpRedirect.Location = new System.Drawing.Point(4, 22);
            this.tpRedirect.Name = "tpRedirect";
            this.tpRedirect.Size = new System.Drawing.Size(425, 169);
            this.tpRedirect.TabIndex = 2;
            this.tpRedirect.Text = "Redirect";
            this.tpRedirect.UseVisualStyleBackColor = true;
            // 
            // txtRedirectIndex
            // 
            this.txtRedirectIndex.Location = new System.Drawing.Point(45, 6);
            this.txtRedirectIndex.Name = "txtRedirectIndex";
            this.txtRedirectIndex.Size = new System.Drawing.Size(48, 20);
            this.txtRedirectIndex.TabIndex = 6;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(447, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(19, 89);
            this.button1.TabIndex = 8;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Dialog Title:";
            // 
            // txtDialogTitle
            // 
            this.txtDialogTitle.Location = new System.Drawing.Point(81, 35);
            this.txtDialogTitle.Name = "txtDialogTitle";
            this.txtDialogTitle.Size = new System.Drawing.Size(360, 20);
            this.txtDialogTitle.TabIndex = 11;
            this.txtDialogTitle.TextChanged += new System.EventHandler(this.txtDialogTitle_TextChanged);
            // 
            // cmbSelectedDialog
            // 
            this.cmbSelectedDialog.FormattingEnabled = true;
            this.cmbSelectedDialog.Location = new System.Drawing.Point(12, 8);
            this.cmbSelectedDialog.Name = "cmbSelectedDialog";
            this.cmbSelectedDialog.Size = new System.Drawing.Size(429, 21);
            this.cmbSelectedDialog.TabIndex = 9;
            this.cmbSelectedDialog.OnChangeDialog += new NetGore.EditorTools.NPCChatDialogComboBoxChangeDialogHandler(this.cmbSelectedDialog_OnChangeDialog);
            // 
            // npcChatDialogView
            // 
            this.npcChatDialogView.Location = new System.Drawing.Point(12, 64);
            this.npcChatDialogView.Name = "npcChatDialogView";
            this.npcChatDialogView.NodeForeColorGoTo = System.Drawing.Color.Blue;
            this.npcChatDialogView.NodeForeColorNormal = System.Drawing.Color.Black;
            this.npcChatDialogView.NodeForeColorResponse = System.Drawing.Color.Green;
            this.npcChatDialogView.NPCChatDialog = null;
            this.npcChatDialogView.Size = new System.Drawing.Size(445, 303);
            this.npcChatDialogView.TabIndex = 0;
            this.npcChatDialogView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.npcChatDialogView_NodeMouseClick);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(332, 628);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(125, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 661);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtDialogTitle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbSelectedDialog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbSelectedNode);
            this.Controls.Add(this.npcChatDialogView);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPC Chat Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbSelectedNode.ResumeLayout(false);
            this.gbSelectedNode.PerformLayout();
            this.tcChatDialogItem.ResumeLayout(false);
            this.tpDialog.ResumeLayout(false);
            this.tpDialog.PerformLayout();
            this.tpResponse.ResumeLayout(false);
            this.tpResponse.PerformLayout();
            this.tpRedirect.ResumeLayout(false);
            this.tpRedirect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NPCChatDialogView npcChatDialogView;
        private System.Windows.Forms.GroupBox gbSelectedNode;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tcChatDialogItem;
        private System.Windows.Forms.TabPage tpDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpResponse;
        private System.Windows.Forms.TabPage tpRedirect;
        private System.Windows.Forms.TextBox txtDialogText;
        private System.Windows.Forms.TextBox txtResponseIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRedirectIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDialogPage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddResponse;
        private System.Windows.Forms.Button btnDeleteResponse;
        private System.Windows.Forms.Button button1;
        private NPCChatDialogComboBox cmbSelectedDialog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDialogTitle;
        private System.Windows.Forms.ComboBox comboBox1;

    }
}

