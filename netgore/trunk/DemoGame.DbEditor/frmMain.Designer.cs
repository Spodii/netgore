namespace DemoGame.DbEditor
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
            this.tc = new System.Windows.Forms.TabControl();
            this.tpItemTemplate = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtItemTemplate = new System.Windows.Forms.TextBox();
            this.btnItemTemplate = new System.Windows.Forms.Button();
            this.btnItemTemplateSave = new System.Windows.Forms.Button();
            this.btnItemTemplateNew = new System.Windows.Forms.Button();
            this.btnItemTemplateDelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pgItemTemplate = new System.Windows.Forms.PropertyGrid();
            this.tpCharacterTemplate = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtCharacterTemplate = new System.Windows.Forms.TextBox();
            this.btnCharacterTemplate = new System.Windows.Forms.Button();
            this.btnCharacterTemplateSave = new System.Windows.Forms.Button();
            this.btnCharacterTemplateNew = new System.Windows.Forms.Button();
            this.btnCharacterTemplateDelete = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pgCharacterTemplate = new System.Windows.Forms.PropertyGrid();
            this.tpQuests = new System.Windows.Forms.TabPage();
            this.tpAlliances = new System.Windows.Forms.TabPage();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnMessages = new System.Windows.Forms.Button();
            this.btnMessagesSave = new System.Windows.Forms.Button();
            this.btnMessagesNew = new System.Windows.Forms.Button();
            this.btnMessagesDelete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.lstMessages = new DemoGame.DbEditor.GameMessageValueListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSelectedMessage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.lstMissingMessages = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMessagesGlobalJS = new System.Windows.Forms.Button();
            this.btnMessagesLanguageJS = new System.Windows.Forms.Button();
            this.btnMessagesTest = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tc.SuspendLayout();
            this.tpItemTemplate.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tpCharacterTemplate.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tpMessages.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpItemTemplate);
            this.tc.Controls.Add(this.tpCharacterTemplate);
            this.tc.Controls.Add(this.tpQuests);
            this.tc.Controls.Add(this.tpAlliances);
            this.tc.Controls.Add(this.tpMessages);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.Location = new System.Drawing.Point(6, 6);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(464, 403);
            this.tc.TabIndex = 0;
            // 
            // tpItemTemplate
            // 
            this.tpItemTemplate.Controls.Add(this.splitContainer1);
            this.tpItemTemplate.Location = new System.Drawing.Point(4, 22);
            this.tpItemTemplate.Name = "tpItemTemplate";
            this.tpItemTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tpItemTemplate.Size = new System.Drawing.Size(456, 377);
            this.tpItemTemplate.TabIndex = 0;
            this.tpItemTemplate.Text = "Item Templates";
            this.tpItemTemplate.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtItemTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplateSave);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplateNew);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplateDelete);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1MinSize = 22;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgItemTemplate);
            this.splitContainer1.Size = new System.Drawing.Size(450, 371);
            this.splitContainer1.SplitterDistance = 22;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtItemTemplate
            // 
            this.txtItemTemplate.BackColor = System.Drawing.SystemColors.Window;
            this.txtItemTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtItemTemplate.Location = new System.Drawing.Point(52, 0);
            this.txtItemTemplate.Name = "txtItemTemplate";
            this.txtItemTemplate.ReadOnly = true;
            this.txtItemTemplate.Size = new System.Drawing.Size(240, 20);
            this.txtItemTemplate.TabIndex = 16;
            // 
            // btnItemTemplate
            // 
            this.btnItemTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplate.Location = new System.Drawing.Point(292, 0);
            this.btnItemTemplate.Name = "btnItemTemplate";
            this.btnItemTemplate.Size = new System.Drawing.Size(26, 22);
            this.btnItemTemplate.TabIndex = 15;
            this.btnItemTemplate.Text = "...";
            this.btnItemTemplate.UseVisualStyleBackColor = true;
            this.btnItemTemplate.Click += new System.EventHandler(this.btnItemTemplate_Click);
            // 
            // btnItemTemplateSave
            // 
            this.btnItemTemplateSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplateSave.Location = new System.Drawing.Point(318, 0);
            this.btnItemTemplateSave.Name = "btnItemTemplateSave";
            this.btnItemTemplateSave.Size = new System.Drawing.Size(41, 22);
            this.btnItemTemplateSave.TabIndex = 14;
            this.btnItemTemplateSave.Text = "Save";
            this.btnItemTemplateSave.UseVisualStyleBackColor = true;
            this.btnItemTemplateSave.Click += new System.EventHandler(this.btnItemTemplateSave_Click);
            // 
            // btnItemTemplateNew
            // 
            this.btnItemTemplateNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplateNew.Location = new System.Drawing.Point(359, 0);
            this.btnItemTemplateNew.Name = "btnItemTemplateNew";
            this.btnItemTemplateNew.Size = new System.Drawing.Size(41, 22);
            this.btnItemTemplateNew.TabIndex = 11;
            this.btnItemTemplateNew.Text = "New";
            this.btnItemTemplateNew.UseVisualStyleBackColor = true;
            this.btnItemTemplateNew.Click += new System.EventHandler(this.btnItemTemplateNew_Click);
            // 
            // btnItemTemplateDelete
            // 
            this.btnItemTemplateDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplateDelete.Location = new System.Drawing.Point(400, 0);
            this.btnItemTemplateDelete.Name = "btnItemTemplateDelete";
            this.btnItemTemplateDelete.Size = new System.Drawing.Size(50, 22);
            this.btnItemTemplateDelete.TabIndex = 10;
            this.btnItemTemplateDelete.Text = "Delete";
            this.btnItemTemplateDelete.UseVisualStyleBackColor = true;
            this.btnItemTemplateDelete.Click += new System.EventHandler(this.btnItemTemplateDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected:";
            // 
            // pgItemTemplate
            // 
            this.pgItemTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgItemTemplate.Location = new System.Drawing.Point(0, 0);
            this.pgItemTemplate.Name = "pgItemTemplate";
            this.pgItemTemplate.Size = new System.Drawing.Size(450, 345);
            this.pgItemTemplate.TabIndex = 0;
            this.pgItemTemplate.SelectedObjectsChanged += new System.EventHandler(this.pgItemTemplate_SelectedObjectsChanged);
            // 
            // tpCharacterTemplate
            // 
            this.tpCharacterTemplate.Controls.Add(this.splitContainer3);
            this.tpCharacterTemplate.Location = new System.Drawing.Point(4, 22);
            this.tpCharacterTemplate.Name = "tpCharacterTemplate";
            this.tpCharacterTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tpCharacterTemplate.Size = new System.Drawing.Size(456, 377);
            this.tpCharacterTemplate.TabIndex = 1;
            this.tpCharacterTemplate.Text = "Character Templates";
            this.tpCharacterTemplate.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtCharacterTemplate);
            this.splitContainer3.Panel1.Controls.Add(this.btnCharacterTemplate);
            this.splitContainer3.Panel1.Controls.Add(this.btnCharacterTemplateSave);
            this.splitContainer3.Panel1.Controls.Add(this.btnCharacterTemplateNew);
            this.splitContainer3.Panel1.Controls.Add(this.btnCharacterTemplateDelete);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            this.splitContainer3.Panel1MinSize = 22;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.pgCharacterTemplate);
            this.splitContainer3.Size = new System.Drawing.Size(450, 371);
            this.splitContainer3.SplitterDistance = 22;
            this.splitContainer3.TabIndex = 1;
            // 
            // txtCharacterTemplate
            // 
            this.txtCharacterTemplate.BackColor = System.Drawing.SystemColors.Window;
            this.txtCharacterTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCharacterTemplate.Location = new System.Drawing.Point(52, 0);
            this.txtCharacterTemplate.Name = "txtCharacterTemplate";
            this.txtCharacterTemplate.ReadOnly = true;
            this.txtCharacterTemplate.Size = new System.Drawing.Size(240, 20);
            this.txtCharacterTemplate.TabIndex = 16;
            // 
            // btnCharacterTemplate
            // 
            this.btnCharacterTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCharacterTemplate.Location = new System.Drawing.Point(292, 0);
            this.btnCharacterTemplate.Name = "btnCharacterTemplate";
            this.btnCharacterTemplate.Size = new System.Drawing.Size(26, 22);
            this.btnCharacterTemplate.TabIndex = 15;
            this.btnCharacterTemplate.Text = "...";
            this.btnCharacterTemplate.UseVisualStyleBackColor = true;
            this.btnCharacterTemplate.Click += new System.EventHandler(this.btnCharacterTemplate_Click);
            // 
            // btnCharacterTemplateSave
            // 
            this.btnCharacterTemplateSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCharacterTemplateSave.Location = new System.Drawing.Point(318, 0);
            this.btnCharacterTemplateSave.Name = "btnCharacterTemplateSave";
            this.btnCharacterTemplateSave.Size = new System.Drawing.Size(41, 22);
            this.btnCharacterTemplateSave.TabIndex = 14;
            this.btnCharacterTemplateSave.Text = "Save";
            this.btnCharacterTemplateSave.UseVisualStyleBackColor = true;
            this.btnCharacterTemplateSave.Click += new System.EventHandler(this.btnCharacterTemplateSave_Click);
            // 
            // btnCharacterTemplateNew
            // 
            this.btnCharacterTemplateNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCharacterTemplateNew.Location = new System.Drawing.Point(359, 0);
            this.btnCharacterTemplateNew.Name = "btnCharacterTemplateNew";
            this.btnCharacterTemplateNew.Size = new System.Drawing.Size(41, 22);
            this.btnCharacterTemplateNew.TabIndex = 11;
            this.btnCharacterTemplateNew.Text = "New";
            this.btnCharacterTemplateNew.UseVisualStyleBackColor = true;
            this.btnCharacterTemplateNew.Click += new System.EventHandler(this.btnCharacterTemplateNew_Click);
            // 
            // btnCharacterTemplateDelete
            // 
            this.btnCharacterTemplateDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCharacterTemplateDelete.Location = new System.Drawing.Point(400, 0);
            this.btnCharacterTemplateDelete.Name = "btnCharacterTemplateDelete";
            this.btnCharacterTemplateDelete.Size = new System.Drawing.Size(50, 22);
            this.btnCharacterTemplateDelete.TabIndex = 10;
            this.btnCharacterTemplateDelete.Text = "Delete";
            this.btnCharacterTemplateDelete.UseVisualStyleBackColor = true;
            this.btnCharacterTemplateDelete.Click += new System.EventHandler(this.btnCharacterTemplateDelete_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Selected:";
            // 
            // pgCharacterTemplate
            // 
            this.pgCharacterTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgCharacterTemplate.Location = new System.Drawing.Point(0, 0);
            this.pgCharacterTemplate.Name = "pgCharacterTemplate";
            this.pgCharacterTemplate.Size = new System.Drawing.Size(450, 345);
            this.pgCharacterTemplate.TabIndex = 0;
            this.pgCharacterTemplate.SelectedObjectsChanged += new System.EventHandler(this.pgCharacterTemplate_SelectedObjectsChanged);
            // 
            // tpQuests
            // 
            this.tpQuests.Location = new System.Drawing.Point(4, 22);
            this.tpQuests.Name = "tpQuests";
            this.tpQuests.Size = new System.Drawing.Size(456, 377);
            this.tpQuests.TabIndex = 2;
            this.tpQuests.Text = "Quests";
            this.tpQuests.UseVisualStyleBackColor = true;
            // 
            // tpAlliances
            // 
            this.tpAlliances.Location = new System.Drawing.Point(4, 22);
            this.tpAlliances.Name = "tpAlliances";
            this.tpAlliances.Size = new System.Drawing.Size(456, 377);
            this.tpAlliances.TabIndex = 3;
            this.tpAlliances.Text = "Alliances";
            this.tpAlliances.UseVisualStyleBackColor = true;
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.splitContainer4);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Size = new System.Drawing.Size(456, 377);
            this.tpMessages.TabIndex = 4;
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.txtMessages);
            this.splitContainer4.Panel1.Controls.Add(this.btnMessages);
            this.splitContainer4.Panel1.Controls.Add(this.btnMessagesSave);
            this.splitContainer4.Panel1.Controls.Add(this.btnMessagesNew);
            this.splitContainer4.Panel1.Controls.Add(this.btnMessagesDelete);
            this.splitContainer4.Panel1.Controls.Add(this.label4);
            this.splitContainer4.Panel1MinSize = 22;
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(456, 377);
            this.splitContainer4.SplitterDistance = 22;
            this.splitContainer4.TabIndex = 30;
            // 
            // txtMessages
            // 
            this.txtMessages.BackColor = System.Drawing.SystemColors.Window;
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(52, 0);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(246, 20);
            this.txtMessages.TabIndex = 33;
            // 
            // btnMessages
            // 
            this.btnMessages.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMessages.Location = new System.Drawing.Point(298, 0);
            this.btnMessages.Name = "btnMessages";
            this.btnMessages.Size = new System.Drawing.Size(26, 22);
            this.btnMessages.TabIndex = 32;
            this.btnMessages.Text = "...";
            this.btnMessages.UseVisualStyleBackColor = true;
            this.btnMessages.Click += new System.EventHandler(this.btnMessages_Click);
            // 
            // btnMessagesSave
            // 
            this.btnMessagesSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMessagesSave.Location = new System.Drawing.Point(324, 0);
            this.btnMessagesSave.Name = "btnMessagesSave";
            this.btnMessagesSave.Size = new System.Drawing.Size(41, 22);
            this.btnMessagesSave.TabIndex = 31;
            this.btnMessagesSave.Text = "Save";
            this.btnMessagesSave.UseVisualStyleBackColor = true;
            this.btnMessagesSave.Click += new System.EventHandler(this.btnMessagesSave_Click);
            // 
            // btnMessagesNew
            // 
            this.btnMessagesNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMessagesNew.Location = new System.Drawing.Point(365, 0);
            this.btnMessagesNew.Name = "btnMessagesNew";
            this.btnMessagesNew.Size = new System.Drawing.Size(41, 22);
            this.btnMessagesNew.TabIndex = 30;
            this.btnMessagesNew.Text = "New";
            this.btnMessagesNew.UseVisualStyleBackColor = true;
            this.btnMessagesNew.Click += new System.EventHandler(this.btnMessagesNew_Click);
            // 
            // btnMessagesDelete
            // 
            this.btnMessagesDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMessagesDelete.Location = new System.Drawing.Point(406, 0);
            this.btnMessagesDelete.Name = "btnMessagesDelete";
            this.btnMessagesDelete.Size = new System.Drawing.Size(50, 22);
            this.btnMessagesDelete.TabIndex = 29;
            this.btnMessagesDelete.Text = "Delete";
            this.btnMessagesDelete.UseVisualStyleBackColor = true;
            this.btnMessagesDelete.Click += new System.EventHandler(this.btnMessagesDelete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "Selected:";
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer5.Size = new System.Drawing.Size(456, 351);
            this.splitContainer5.SplitterDistance = 265;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.lstMessages);
            this.splitContainer7.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.txtSelectedMessage);
            this.splitContainer7.Panel2.Controls.Add(this.label7);
            this.splitContainer7.Size = new System.Drawing.Size(456, 265);
            this.splitContainer7.SplitterDistance = 229;
            this.splitContainer7.TabIndex = 0;
            // 
            // lstMessages
            // 
            this.lstMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMessages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMessages.FormattingEnabled = true;
            this.lstMessages.Location = new System.Drawing.Point(0, 13);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(456, 212);
            this.lstMessages.TabIndex = 5;
            this.lstMessages.SelectedIndexChanged += new System.EventHandler(this.lstMessages_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Added language messages:";
            // 
            // txtSelectedMessage
            // 
            this.txtSelectedMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectedMessage.Location = new System.Drawing.Point(0, 13);
            this.txtSelectedMessage.Name = "txtSelectedMessage";
            this.txtSelectedMessage.Size = new System.Drawing.Size(456, 20);
            this.txtSelectedMessage.TabIndex = 7;
            this.txtSelectedMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSelectedMessage_KeyDown);
            this.txtSelectedMessage.Leave += new System.EventHandler(this.txtSelectedMessage_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Selected message:";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.lstMissingMessages);
            this.splitContainer6.Panel1.Controls.Add(this.label6);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.btnMessagesGlobalJS);
            this.splitContainer6.Panel2.Controls.Add(this.btnMessagesLanguageJS);
            this.splitContainer6.Panel2.Controls.Add(this.btnMessagesTest);
            this.splitContainer6.Size = new System.Drawing.Size(456, 82);
            this.splitContainer6.SplitterDistance = 377;
            this.splitContainer6.TabIndex = 0;
            // 
            // lstMissingMessages
            // 
            this.lstMissingMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMissingMessages.FormattingEnabled = true;
            this.lstMissingMessages.Location = new System.Drawing.Point(0, 13);
            this.lstMissingMessages.Name = "lstMissingMessages";
            this.lstMissingMessages.Size = new System.Drawing.Size(377, 69);
            this.lstMissingMessages.TabIndex = 6;
            this.lstMissingMessages.DoubleClick += new System.EventHandler(this.lstMissingMessages_DoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Missing messages:";
            // 
            // btnMessagesGlobalJS
            // 
            this.btnMessagesGlobalJS.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMessagesGlobalJS.Location = new System.Drawing.Point(0, 44);
            this.btnMessagesGlobalJS.Name = "btnMessagesGlobalJS";
            this.btnMessagesGlobalJS.Size = new System.Drawing.Size(75, 22);
            this.btnMessagesGlobalJS.TabIndex = 34;
            this.btnMessagesGlobalJS.Text = "Global.js";
            this.btnMessagesGlobalJS.UseVisualStyleBackColor = true;
            this.btnMessagesGlobalJS.Click += new System.EventHandler(this.btnMessagesGlobalJS_Click);
            // 
            // btnMessagesLanguageJS
            // 
            this.btnMessagesLanguageJS.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMessagesLanguageJS.Location = new System.Drawing.Point(0, 22);
            this.btnMessagesLanguageJS.Name = "btnMessagesLanguageJS";
            this.btnMessagesLanguageJS.Size = new System.Drawing.Size(75, 22);
            this.btnMessagesLanguageJS.TabIndex = 33;
            this.btnMessagesLanguageJS.Text = "Language.js";
            this.btnMessagesLanguageJS.UseVisualStyleBackColor = true;
            this.btnMessagesLanguageJS.Click += new System.EventHandler(this.btnMessagesLanguageJS_Click);
            // 
            // btnMessagesTest
            // 
            this.btnMessagesTest.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMessagesTest.Location = new System.Drawing.Point(0, 0);
            this.btnMessagesTest.Name = "btnMessagesTest";
            this.btnMessagesTest.Size = new System.Drawing.Size(75, 22);
            this.btnMessagesTest.TabIndex = 32;
            this.btnMessagesTest.Text = "Test Build";
            this.btnMessagesTest.UseVisualStyleBackColor = true;
            this.btnMessagesTest.Click += new System.EventHandler(this.btnMessagesTest_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.button2);
            this.splitContainer2.Panel1.Controls.Add(this.button3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1MinSize = 22;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(450, 371);
            this.splitContainer2.SplitterDistance = 22;
            this.splitContainer2.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(52, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(281, 20);
            this.textBox1.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(333, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 22);
            this.button1.TabIndex = 12;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(359, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(41, 22);
            this.button2.TabIndex = 11;
            this.button2.Text = "New";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3.Location = new System.Drawing.Point(400, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 22);
            this.button3.TabIndex = 10;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Selected:";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(450, 345);
            this.propertyGrid1.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 415);
            this.Controls.Add(this.tc);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "NetGore Database Editor";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tc.ResumeLayout(false);
            this.tpItemTemplate.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tpCharacterTemplate.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.tpMessages.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel1.PerformLayout();
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.Panel2.PerformLayout();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpItemTemplate;
        private System.Windows.Forms.TabPage tpCharacterTemplate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid pgItemTemplate;
        private System.Windows.Forms.Button btnItemTemplateNew;
        private System.Windows.Forms.Button btnItemTemplateDelete;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnCharacterTemplateNew;
        private System.Windows.Forms.Button btnCharacterTemplateDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PropertyGrid pgCharacterTemplate;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage tpQuests;
        private System.Windows.Forms.TabPage tpAlliances;
        private System.Windows.Forms.Button btnItemTemplate;
        private System.Windows.Forms.Button btnItemTemplateSave;
        private System.Windows.Forms.Button btnCharacterTemplate;
        private System.Windows.Forms.Button btnCharacterTemplateSave;
        private System.Windows.Forms.TextBox txtItemTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCharacterTemplate;
        private System.Windows.Forms.TabPage tpMessages;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnMessages;
        private System.Windows.Forms.Button btnMessagesSave;
        private System.Windows.Forms.Button btnMessagesNew;
        private System.Windows.Forms.Button btnMessagesDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.ListBox lstMissingMessages;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMessagesGlobalJS;
        private System.Windows.Forms.Button btnMessagesLanguageJS;
        private System.Windows.Forms.Button btnMessagesTest;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private GameMessageValueListBox lstMessages;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSelectedMessage;
        private System.Windows.Forms.Label label7;
    }
}

