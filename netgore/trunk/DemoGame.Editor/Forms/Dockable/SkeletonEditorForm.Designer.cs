// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace DemoGame.Editor
{
    partial class SkeletonEditorForm
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.GameScreen = new DemoGame.Editor.SkeletonEditorScreenControl();
            this.tcMenu = new System.Windows.Forms.TabControl();
            this.tabSkeleton = new System.Windows.Forms.TabPage();
            this.gbSkeletonActions = new System.Windows.Forms.GroupBox();
            this.btnCopyInherits = new System.Windows.Forms.Button();
            this.btnInterpolate = new System.Windows.Forms.Button();
            this.btnShiftNodes = new System.Windows.Forms.Button();
            this.btnCopyRoot = new System.Windows.Forms.Button();
            this.btnCopyLen = new System.Windows.Forms.Button();
            this.gbSelectedNode = new System.Windows.Forms.GroupBox();
            this.chkIsMod = new System.Windows.Forms.CheckBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSkeletonNodes = new DemoGame.Editor.SkeletonNodesComboBox();
            this.gbSkeletonIO = new System.Windows.Forms.GroupBox();
            this.btnSkeletonSaveAs = new System.Windows.Forms.Button();
            this.btnSkeletonSave = new System.Windows.Forms.Button();
            this.btnSkeletonLoad = new System.Windows.Forms.Button();
            this.lblSkeleton = new System.Windows.Forms.Label();
            this.tabAnimation = new System.Windows.Forms.TabPage();
            this.txtFrames = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFall = new System.Windows.Forms.Button();
            this.btnJump = new System.Windows.Forms.Button();
            this.btnWalk = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStand = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.gbAnimIO = new System.Windows.Forms.GroupBox();
            this.btnAnimSaveAs = new System.Windows.Forms.Button();
            this.btnAnimSave = new System.Windows.Forms.Button();
            this.btnAnimLoad = new System.Windows.Forms.Button();
            this.lblAnimation = new System.Windows.Forms.Label();
            this.tabBody = new System.Windows.Forms.TabPage();
            this.gbBodies = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstBodies = new DemoGame.Editor.SkeletonBodyItemsListBox();
            this.cmbSkeletonBodyNodes = new System.Windows.Forms.ComboBox();
            this.cmbSkeletonBodies = new System.Windows.Forms.ComboBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectBodyGrhData = new DemoGame.Editor.SelectGrhDataButton();
            this.btnClearTarget = new System.Windows.Forms.Button();
            this.cmbTarget = new DemoGame.Editor.SkeletonNodesComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbSource = new DemoGame.Editor.SkeletonNodesComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtGrhIndex = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtOriginY = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOriginX = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOffsetY = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOffsetX = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.gbBodyIO = new System.Windows.Forms.GroupBox();
            this.btnBodySaveAs = new System.Windows.Forms.Button();
            this.btnBodySave = new System.Windows.Forms.Button();
            this.btnBodyLoad = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.gbHistory = new System.Windows.Forms.GroupBox();
            this.lstHistory = new System.Windows.Forms.ListBox();
            this.chkCanAlter = new System.Windows.Forms.CheckBox();
            this.chkCanTransform = new System.Windows.Forms.CheckBox();
            this.gbState = new System.Windows.Forms.GroupBox();
            this.chkDrawBody = new System.Windows.Forms.CheckBox();
            this.chkDrawSkel = new System.Windows.Forms.CheckBox();
            this.radioAnimate = new System.Windows.Forms.RadioButton();
            this.radioEdit = new System.Windows.Forms.RadioButton();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tcMenu.SuspendLayout();
            this.tabSkeleton.SuspendLayout();
            this.gbSkeletonActions.SuspendLayout();
            this.gbSelectedNode.SuspendLayout();
            this.gbSkeletonIO.SuspendLayout();
            this.tabAnimation.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbAnimIO.SuspendLayout();
            this.tabBody.SuspendLayout();
            this.gbBodies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbBodyIO.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.gbHistory.SuspendLayout();
            this.gbState.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.GameScreen);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tcMenu);
            this.splitContainer2.Panel2.Controls.Add(this.gbState);
            this.splitContainer2.Size = new System.Drawing.Size(931, 549);
            this.splitContainer2.SplitterDistance = 630;
            this.splitContainer2.TabIndex = 26;
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameScreen.Location = new System.Drawing.Point(0, 0);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Size = new System.Drawing.Size(630, 549);
            this.GameScreen.SkeletonEditorForm = null;
            this.GameScreen.TabIndex = 2;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            this.GameScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseUp);
            this.GameScreen.Resize += new System.EventHandler(this.GameScreen_Resize);
            // 
            // tcMenu
            // 
            this.tcMenu.Controls.Add(this.tabSkeleton);
            this.tcMenu.Controls.Add(this.tabAnimation);
            this.tcMenu.Controls.Add(this.tabBody);
            this.tcMenu.Controls.Add(this.tabSettings);
            this.tcMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMenu.Location = new System.Drawing.Point(0, 0);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(297, 435);
            this.tcMenu.TabIndex = 26;
            // 
            // tabSkeleton
            // 
            this.tabSkeleton.Controls.Add(this.gbSkeletonActions);
            this.tabSkeleton.Controls.Add(this.gbSelectedNode);
            this.tabSkeleton.Controls.Add(this.cmbSkeletonNodes);
            this.tabSkeleton.Controls.Add(this.gbSkeletonIO);
            this.tabSkeleton.Controls.Add(this.lblSkeleton);
            this.tabSkeleton.Location = new System.Drawing.Point(4, 22);
            this.tabSkeleton.Name = "tabSkeleton";
            this.tabSkeleton.Padding = new System.Windows.Forms.Padding(3);
            this.tabSkeleton.Size = new System.Drawing.Size(289, 409);
            this.tabSkeleton.TabIndex = 0;
            this.tabSkeleton.Text = "Skeleton";
            this.tabSkeleton.UseVisualStyleBackColor = true;
            // 
            // gbSkeletonActions
            // 
            this.gbSkeletonActions.Controls.Add(this.btnCopyInherits);
            this.gbSkeletonActions.Controls.Add(this.btnInterpolate);
            this.gbSkeletonActions.Controls.Add(this.btnShiftNodes);
            this.gbSkeletonActions.Controls.Add(this.btnCopyRoot);
            this.gbSkeletonActions.Controls.Add(this.btnCopyLen);
            this.gbSkeletonActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbSkeletonActions.Location = new System.Drawing.Point(3, 261);
            this.gbSkeletonActions.Name = "gbSkeletonActions";
            this.gbSkeletonActions.Size = new System.Drawing.Size(283, 102);
            this.gbSkeletonActions.TabIndex = 50;
            this.gbSkeletonActions.TabStop = false;
            this.gbSkeletonActions.Text = "Special Actions";
            // 
            // btnCopyInherits
            // 
            this.btnCopyInherits.Location = new System.Drawing.Point(5, 72);
            this.btnCopyInherits.Name = "btnCopyInherits";
            this.btnCopyInherits.Size = new System.Drawing.Size(84, 23);
            this.btnCopyInherits.TabIndex = 34;
            this.btnCopyInherits.Text = "Copy \'IsMod\'";
            this.btnCopyInherits.UseVisualStyleBackColor = true;
            this.btnCopyInherits.Click += new System.EventHandler(this.btnCopyInherits_Click);
            // 
            // btnInterpolate
            // 
            this.btnInterpolate.Location = new System.Drawing.Point(101, 43);
            this.btnInterpolate.Name = "btnInterpolate";
            this.btnInterpolate.Size = new System.Drawing.Size(84, 23);
            this.btnInterpolate.TabIndex = 33;
            this.btnInterpolate.Text = "Interpolate";
            this.btnInterpolate.UseVisualStyleBackColor = true;
            this.btnInterpolate.Click += new System.EventHandler(this.btnInterpolate_Click);
            // 
            // btnShiftNodes
            // 
            this.btnShiftNodes.Location = new System.Drawing.Point(6, 43);
            this.btnShiftNodes.Name = "btnShiftNodes";
            this.btnShiftNodes.Size = new System.Drawing.Size(84, 23);
            this.btnShiftNodes.TabIndex = 32;
            this.btnShiftNodes.Text = "Shift Nodes";
            this.btnShiftNodes.UseVisualStyleBackColor = true;
            this.btnShiftNodes.Click += new System.EventHandler(this.btnShiftNodes_Click);
            // 
            // btnCopyRoot
            // 
            this.btnCopyRoot.Location = new System.Drawing.Point(101, 14);
            this.btnCopyRoot.Name = "btnCopyRoot";
            this.btnCopyRoot.Size = new System.Drawing.Size(84, 23);
            this.btnCopyRoot.TabIndex = 31;
            this.btnCopyRoot.Text = "Copy Root Pos";
            this.btnCopyRoot.UseVisualStyleBackColor = true;
            this.btnCopyRoot.Click += new System.EventHandler(this.btnCopyRoot_Click);
            // 
            // btnCopyLen
            // 
            this.btnCopyLen.Location = new System.Drawing.Point(6, 14);
            this.btnCopyLen.Name = "btnCopyLen";
            this.btnCopyLen.Size = new System.Drawing.Size(84, 23);
            this.btnCopyLen.TabIndex = 28;
            this.btnCopyLen.Text = "Copy Lengths";
            this.btnCopyLen.UseVisualStyleBackColor = true;
            this.btnCopyLen.Click += new System.EventHandler(this.btnCopyLen_Click);
            // 
            // gbSelectedNode
            // 
            this.gbSelectedNode.Controls.Add(this.chkIsMod);
            this.gbSelectedNode.Controls.Add(this.txtAngle);
            this.gbSelectedNode.Controls.Add(this.label5);
            this.gbSelectedNode.Controls.Add(this.txtLength);
            this.gbSelectedNode.Controls.Add(this.label4);
            this.gbSelectedNode.Controls.Add(this.txtY);
            this.gbSelectedNode.Controls.Add(this.label3);
            this.gbSelectedNode.Controls.Add(this.txtX);
            this.gbSelectedNode.Controls.Add(this.label2);
            this.gbSelectedNode.Controls.Add(this.txtName);
            this.gbSelectedNode.Controls.Add(this.label1);
            this.gbSelectedNode.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSelectedNode.Location = new System.Drawing.Point(3, 37);
            this.gbSelectedNode.Name = "gbSelectedNode";
            this.gbSelectedNode.Size = new System.Drawing.Size(283, 146);
            this.gbSelectedNode.TabIndex = 49;
            this.gbSelectedNode.TabStop = false;
            this.gbSelectedNode.Text = "Selected Node";
            // 
            // chkIsMod
            // 
            this.chkIsMod.AutoSize = true;
            this.chkIsMod.Location = new System.Drawing.Point(111, 123);
            this.chkIsMod.Name = "chkIsMod";
            this.chkIsMod.Size = new System.Drawing.Size(74, 17);
            this.chkIsMod.TabIndex = 24;
            this.chkIsMod.Text = "Is Modifier";
            this.chkIsMod.UseVisualStyleBackColor = true;
            this.chkIsMod.CheckedChanged += new System.EventHandler(this.chkIsMod_CheckedChanged);
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(54, 71);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(131, 20);
            this.txtAngle.TabIndex = 20;
            this.tt.SetToolTip(this.txtAngle, "Angle of the node in relation to the parent node");
            this.txtAngle.TextChanged += new System.EventHandler(this.txtAngle_TextChanged);
            this.txtAngle.Leave += new System.EventHandler(this.txtAngle_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Angle:";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(54, 45);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(131, 20);
            this.txtLength.TabIndex = 18;
            this.tt.SetToolTip(this.txtLength, "Length of the bone from this node to its parent, in pixels");
            this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
            this.txtLength.Leave += new System.EventHandler(this.txtLength_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Length:";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(138, 97);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(47, 20);
            this.txtY.TabIndex = 16;
            this.tt.SetToolTip(this.txtY, "Y coordinate of the node (relative to the skeleton, not parent)");
            this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Y:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(54, 97);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(47, 20);
            this.txtX.TabIndex = 14;
            this.tt.SetToolTip(this.txtX, "X coordinate of the node (relative to the skeleton, not parent)");
            this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "X:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(54, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(131, 20);
            this.txtName.TabIndex = 12;
            this.tt.SetToolTip(this.txtName, "The unique name of the node");
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name:";
            // 
            // cmbSkeletonNodes
            // 
            this.cmbSkeletonNodes.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSkeletonNodes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSkeletonNodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSkeletonNodes.FormattingEnabled = true;
            this.cmbSkeletonNodes.Location = new System.Drawing.Point(3, 16);
            this.cmbSkeletonNodes.Name = "cmbSkeletonNodes";
            this.cmbSkeletonNodes.Size = new System.Drawing.Size(283, 21);
            this.cmbSkeletonNodes.Sorted = true;
            this.cmbSkeletonNodes.TabIndex = 48;
            this.tt.SetToolTip(this.cmbSkeletonNodes, "The currently selected skeleton node (joint)");
            this.cmbSkeletonNodes.SelectedIndexChanged += new System.EventHandler(this.cmbSkeletonNodes_SelectedIndexChanged);
            // 
            // gbSkeletonIO
            // 
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonSaveAs);
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonSave);
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonLoad);
            this.gbSkeletonIO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbSkeletonIO.Location = new System.Drawing.Point(3, 363);
            this.gbSkeletonIO.Name = "gbSkeletonIO";
            this.gbSkeletonIO.Size = new System.Drawing.Size(283, 43);
            this.gbSkeletonIO.TabIndex = 31;
            this.gbSkeletonIO.TabStop = false;
            this.gbSkeletonIO.Text = "Load / Save";
            // 
            // btnSkeletonSaveAs
            // 
            this.btnSkeletonSaveAs.Location = new System.Drawing.Point(128, 14);
            this.btnSkeletonSaveAs.Name = "btnSkeletonSaveAs";
            this.btnSkeletonSaveAs.Size = new System.Drawing.Size(55, 23);
            this.btnSkeletonSaveAs.TabIndex = 30;
            this.btnSkeletonSaveAs.Text = "Save As";
            this.btnSkeletonSaveAs.UseVisualStyleBackColor = true;
            this.btnSkeletonSaveAs.Click += new System.EventHandler(this.btnSkeletonSaveAs_Click);
            // 
            // btnSkeletonSave
            // 
            this.btnSkeletonSave.Location = new System.Drawing.Point(67, 14);
            this.btnSkeletonSave.Name = "btnSkeletonSave";
            this.btnSkeletonSave.Size = new System.Drawing.Size(55, 23);
            this.btnSkeletonSave.TabIndex = 29;
            this.btnSkeletonSave.Text = "Save";
            this.btnSkeletonSave.UseVisualStyleBackColor = true;
            this.btnSkeletonSave.Click += new System.EventHandler(this.btnSkeletonSave_Click);
            // 
            // btnSkeletonLoad
            // 
            this.btnSkeletonLoad.Location = new System.Drawing.Point(6, 14);
            this.btnSkeletonLoad.Name = "btnSkeletonLoad";
            this.btnSkeletonLoad.Size = new System.Drawing.Size(55, 23);
            this.btnSkeletonLoad.TabIndex = 28;
            this.btnSkeletonLoad.Text = "Load";
            this.btnSkeletonLoad.UseVisualStyleBackColor = true;
            this.btnSkeletonLoad.Click += new System.EventHandler(this.btnSkeletonLoad_Click);
            // 
            // lblSkeleton
            // 
            this.lblSkeleton.AutoSize = true;
            this.lblSkeleton.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSkeleton.Location = new System.Drawing.Point(3, 3);
            this.lblSkeleton.Name = "lblSkeleton";
            this.lblSkeleton.Size = new System.Drawing.Size(86, 13);
            this.lblSkeleton.TabIndex = 24;
            this.lblSkeleton.Text = "Skeleton Nodes:";
            // 
            // tabAnimation
            // 
            this.tabAnimation.Controls.Add(this.txtFrames);
            this.tabAnimation.Controls.Add(this.groupBox2);
            this.tabAnimation.Controls.Add(this.gbAnimIO);
            this.tabAnimation.Controls.Add(this.lblAnimation);
            this.tabAnimation.Location = new System.Drawing.Point(4, 22);
            this.tabAnimation.Name = "tabAnimation";
            this.tabAnimation.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnimation.Size = new System.Drawing.Size(289, 409);
            this.tabAnimation.TabIndex = 1;
            this.tabAnimation.Text = "Animation";
            this.tabAnimation.UseVisualStyleBackColor = true;
            // 
            // txtFrames
            // 
            this.txtFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFrames.Location = new System.Drawing.Point(3, 16);
            this.txtFrames.Multiline = true;
            this.txtFrames.Name = "txtFrames";
            this.txtFrames.Size = new System.Drawing.Size(283, 267);
            this.txtFrames.TabIndex = 36;
            this.txtFrames.TextChanged += new System.EventHandler(this.txtFrames_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFall);
            this.groupBox2.Controls.Add(this.btnJump);
            this.groupBox2.Controls.Add(this.btnWalk);
            this.groupBox2.Controls.Add(this.btnPause);
            this.groupBox2.Controls.Add(this.btnStand);
            this.groupBox2.Controls.Add(this.btnPlay);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 283);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 80);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pre-made Animations + Controls";
            // 
            // btnFall
            // 
            this.btnFall.Location = new System.Drawing.Point(128, 48);
            this.btnFall.Name = "btnFall";
            this.btnFall.Size = new System.Drawing.Size(55, 23);
            this.btnFall.TabIndex = 39;
            this.btnFall.Text = "Fall";
            this.btnFall.UseVisualStyleBackColor = true;
            this.btnFall.Click += new System.EventHandler(this.btnFall_Click);
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(67, 48);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(55, 23);
            this.btnJump.TabIndex = 38;
            this.btnJump.Text = "Jump";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnWalk
            // 
            this.btnWalk.Location = new System.Drawing.Point(6, 48);
            this.btnWalk.Name = "btnWalk";
            this.btnWalk.Size = new System.Drawing.Size(55, 23);
            this.btnWalk.TabIndex = 37;
            this.btnWalk.Text = "Walk";
            this.btnWalk.UseVisualStyleBackColor = true;
            this.btnWalk.Click += new System.EventHandler(this.btnWalk_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(67, 19);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(55, 23);
            this.btnPause.TabIndex = 36;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStand
            // 
            this.btnStand.Location = new System.Drawing.Point(128, 19);
            this.btnStand.Name = "btnStand";
            this.btnStand.Size = new System.Drawing.Size(55, 23);
            this.btnStand.TabIndex = 35;
            this.btnStand.Text = "Stand";
            this.btnStand.UseVisualStyleBackColor = true;
            this.btnStand.Click += new System.EventHandler(this.btnStand_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(6, 19);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(55, 23);
            this.btnPlay.TabIndex = 34;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // gbAnimIO
            // 
            this.gbAnimIO.Controls.Add(this.btnAnimSaveAs);
            this.gbAnimIO.Controls.Add(this.btnAnimSave);
            this.gbAnimIO.Controls.Add(this.btnAnimLoad);
            this.gbAnimIO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbAnimIO.Location = new System.Drawing.Point(3, 363);
            this.gbAnimIO.Name = "gbAnimIO";
            this.gbAnimIO.Size = new System.Drawing.Size(283, 43);
            this.gbAnimIO.TabIndex = 30;
            this.gbAnimIO.TabStop = false;
            this.gbAnimIO.Text = "Load / Save";
            // 
            // btnAnimSaveAs
            // 
            this.btnAnimSaveAs.Location = new System.Drawing.Point(128, 14);
            this.btnAnimSaveAs.Name = "btnAnimSaveAs";
            this.btnAnimSaveAs.Size = new System.Drawing.Size(55, 23);
            this.btnAnimSaveAs.TabIndex = 30;
            this.btnAnimSaveAs.Text = "Save As";
            this.btnAnimSaveAs.UseVisualStyleBackColor = true;
            this.btnAnimSaveAs.Click += new System.EventHandler(this.btnAnimSaveAs_Click);
            // 
            // btnAnimSave
            // 
            this.btnAnimSave.Location = new System.Drawing.Point(67, 14);
            this.btnAnimSave.Name = "btnAnimSave";
            this.btnAnimSave.Size = new System.Drawing.Size(55, 23);
            this.btnAnimSave.TabIndex = 29;
            this.btnAnimSave.Text = "Save";
            this.btnAnimSave.UseVisualStyleBackColor = true;
            this.btnAnimSave.Click += new System.EventHandler(this.btnAnimSave_Click);
            // 
            // btnAnimLoad
            // 
            this.btnAnimLoad.Location = new System.Drawing.Point(6, 14);
            this.btnAnimLoad.Name = "btnAnimLoad";
            this.btnAnimLoad.Size = new System.Drawing.Size(55, 23);
            this.btnAnimLoad.TabIndex = 28;
            this.btnAnimLoad.Text = "Load";
            this.btnAnimLoad.UseVisualStyleBackColor = true;
            this.btnAnimLoad.Click += new System.EventHandler(this.btnAnimLoad_Click);
            // 
            // lblAnimation
            // 
            this.lblAnimation.AutoSize = true;
            this.lblAnimation.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAnimation.Location = new System.Drawing.Point(3, 3);
            this.lblAnimation.Name = "lblAnimation";
            this.lblAnimation.Size = new System.Drawing.Size(44, 13);
            this.lblAnimation.TabIndex = 23;
            this.lblAnimation.Text = "Frames:";
            // 
            // tabBody
            // 
            this.tabBody.Controls.Add(this.gbBodies);
            this.tabBody.Controls.Add(this.groupBox1);
            this.tabBody.Controls.Add(this.gbBodyIO);
            this.tabBody.Location = new System.Drawing.Point(4, 22);
            this.tabBody.Name = "tabBody";
            this.tabBody.Size = new System.Drawing.Size(289, 409);
            this.tabBody.TabIndex = 2;
            this.tabBody.Text = "Body";
            this.tabBody.UseVisualStyleBackColor = true;
            // 
            // gbBodies
            // 
            this.gbBodies.Controls.Add(this.splitContainer1);
            this.gbBodies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBodies.Location = new System.Drawing.Point(0, 0);
            this.gbBodies.Name = "gbBodies";
            this.gbBodies.Size = new System.Drawing.Size(289, 213);
            this.gbBodies.TabIndex = 33;
            this.gbBodies.TabStop = false;
            this.gbBodies.Text = "Bone Bodies:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstBodies);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cmbSkeletonBodyNodes);
            this.splitContainer1.Panel2.Controls.Add(this.cmbSkeletonBodies);
            this.splitContainer1.Panel2.Controls.Add(this.btnUp);
            this.splitContainer1.Panel2.Controls.Add(this.btnDown);
            this.splitContainer1.Panel2.Controls.Add(this.btnDelete);
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Size = new System.Drawing.Size(283, 194);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 38;
            // 
            // lstBodies
            // 
            this.lstBodies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBodies.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstBodies.FormattingEnabled = true;
            this.lstBodies.Location = new System.Drawing.Point(0, 0);
            this.lstBodies.Name = "lstBodies";
            this.lstBodies.Size = new System.Drawing.Size(283, 163);
            this.lstBodies.TabIndex = 35;
            this.lstBodies.SelectedIndexChanged += new System.EventHandler(this.lstBodies_SelectedIndexChanged);
            // 
            // cmbSkeletonBodyNodes
            // 
            this.cmbSkeletonBodyNodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSkeletonBodyNodes.FormattingEnabled = true;
            this.cmbSkeletonBodyNodes.Location = new System.Drawing.Point(168, 3);
            this.cmbSkeletonBodyNodes.Name = "cmbSkeletonBodyNodes";
            this.cmbSkeletonBodyNodes.Size = new System.Drawing.Size(56, 21);
            this.cmbSkeletonBodyNodes.TabIndex = 49;
            // 
            // cmbSkeletonBodies
            // 
            this.cmbSkeletonBodies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSkeletonBodies.FormattingEnabled = true;
            this.cmbSkeletonBodies.Location = new System.Drawing.Point(107, 3);
            this.cmbSkeletonBodies.Name = "cmbSkeletonBodies";
            this.cmbSkeletonBodies.Size = new System.Drawing.Size(56, 21);
            this.cmbSkeletonBodies.TabIndex = 48;
            this.cmbSkeletonBodies.SelectedIndexChanged += new System.EventHandler(this.cmbSkeletonBodies_SelectedIndexChanged);
            // 
            // btnUp
            // 
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUp.Location = new System.Drawing.Point(227, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(28, 27);
            this.btnUp.TabIndex = 43;
            this.btnUp.Text = "/\\";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDown.Location = new System.Drawing.Point(255, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(28, 27);
            this.btnDown.TabIndex = 42;
            this.btnDown.Text = "\\/";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDelete.Location = new System.Drawing.Point(52, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(52, 27);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(52, 27);
            this.btnAdd.TabIndex = 37;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectBodyGrhData);
            this.groupBox1.Controls.Add(this.btnClearTarget);
            this.groupBox1.Controls.Add(this.cmbTarget);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmbSource);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtGrhIndex);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtOriginY);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtOriginX);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtOffsetY);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtOffsetX);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 153);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Body";
            // 
            // btnSelectBodyGrhData
            // 
            this.btnSelectBodyGrhData.Location = new System.Drawing.Point(160, 67);
            this.btnSelectBodyGrhData.Name = "btnSelectBodyGrhData";
            this.btnSelectBodyGrhData.SelectedGrhDataHandler = null;
            this.btnSelectBodyGrhData.Size = new System.Drawing.Size(24, 23);
            this.btnSelectBodyGrhData.TabIndex = 49;
            this.btnSelectBodyGrhData.UseVisualStyleBackColor = true;
            // 
            // btnClearTarget
            // 
            this.btnClearTarget.Location = new System.Drawing.Point(165, 122);
            this.btnClearTarget.Name = "btnClearTarget";
            this.btnClearTarget.Size = new System.Drawing.Size(20, 23);
            this.btnClearTarget.TabIndex = 48;
            this.btnClearTarget.Text = "X";
            this.btnClearTarget.UseVisualStyleBackColor = true;
            this.btnClearTarget.TextChanged += new System.EventHandler(this.btnClearTarget_Click);
            this.btnClearTarget.Click += new System.EventHandler(this.btnClearTarget_Click);
            // 
            // cmbTarget
            // 
            this.cmbTarget.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.Location = new System.Drawing.Point(60, 122);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(99, 21);
            this.cmbTarget.Sorted = true;
            this.cmbTarget.TabIndex = 47;
            this.cmbTarget.SelectedIndexChanged += new System.EventHandler(this.cmbTarget_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 125);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 46;
            this.label15.Text = "Target:";
            // 
            // cmbSource
            // 
            this.cmbSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(60, 95);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(124, 21);
            this.cmbSource.Sorted = true;
            this.cmbSource.TabIndex = 45;
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "Source:";
            // 
            // txtGrhIndex
            // 
            this.txtGrhIndex.Location = new System.Drawing.Point(60, 69);
            this.txtGrhIndex.Name = "txtGrhIndex";
            this.txtGrhIndex.Size = new System.Drawing.Size(94, 20);
            this.txtGrhIndex.TabIndex = 43;
            this.txtGrhIndex.TextChanged += new System.EventHandler(this.txtGrhIndex_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Grh:";
            // 
            // txtOriginY
            // 
            this.txtOriginY.Location = new System.Drawing.Point(149, 45);
            this.txtOriginY.Name = "txtOriginY";
            this.txtOriginY.Size = new System.Drawing.Size(35, 20);
            this.txtOriginY.TabIndex = 41;
            this.txtOriginY.TextChanged += new System.EventHandler(this.txtOriginY_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(126, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Y:";
            // 
            // txtOriginX
            // 
            this.txtOriginX.Location = new System.Drawing.Point(85, 45);
            this.txtOriginX.Name = "txtOriginX";
            this.txtOriginX.Size = new System.Drawing.Size(35, 20);
            this.txtOriginX.TabIndex = 39;
            this.txtOriginX.TextChanged += new System.EventHandler(this.txtOriginX_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(62, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "X:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Origin:";
            // 
            // txtOffsetY
            // 
            this.txtOffsetY.Location = new System.Drawing.Point(149, 19);
            this.txtOffsetY.Name = "txtOffsetY";
            this.txtOffsetY.Size = new System.Drawing.Size(35, 20);
            this.txtOffsetY.TabIndex = 36;
            this.txtOffsetY.TextChanged += new System.EventHandler(this.txtOffsetY_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(126, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Y:";
            // 
            // txtOffsetX
            // 
            this.txtOffsetX.Location = new System.Drawing.Point(85, 19);
            this.txtOffsetX.Name = "txtOffsetX";
            this.txtOffsetX.Size = new System.Drawing.Size(35, 20);
            this.txtOffsetX.TabIndex = 34;
            this.txtOffsetX.TextChanged += new System.EventHandler(this.txtOffsetX_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Offset:";
            // 
            // gbBodyIO
            // 
            this.gbBodyIO.Controls.Add(this.btnBodySaveAs);
            this.gbBodyIO.Controls.Add(this.btnBodySave);
            this.gbBodyIO.Controls.Add(this.btnBodyLoad);
            this.gbBodyIO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbBodyIO.Location = new System.Drawing.Point(0, 366);
            this.gbBodyIO.Name = "gbBodyIO";
            this.gbBodyIO.Size = new System.Drawing.Size(289, 43);
            this.gbBodyIO.TabIndex = 31;
            this.gbBodyIO.TabStop = false;
            this.gbBodyIO.Text = "Load / Save";
            // 
            // btnBodySaveAs
            // 
            this.btnBodySaveAs.Location = new System.Drawing.Point(128, 14);
            this.btnBodySaveAs.Name = "btnBodySaveAs";
            this.btnBodySaveAs.Size = new System.Drawing.Size(55, 23);
            this.btnBodySaveAs.TabIndex = 30;
            this.btnBodySaveAs.Text = "Save As";
            this.btnBodySaveAs.UseVisualStyleBackColor = true;
            this.btnBodySaveAs.Click += new System.EventHandler(this.btnBodySaveAs_Click);
            // 
            // btnBodySave
            // 
            this.btnBodySave.Location = new System.Drawing.Point(67, 14);
            this.btnBodySave.Name = "btnBodySave";
            this.btnBodySave.Size = new System.Drawing.Size(55, 23);
            this.btnBodySave.TabIndex = 29;
            this.btnBodySave.Text = "Save";
            this.btnBodySave.UseVisualStyleBackColor = true;
            this.btnBodySave.Click += new System.EventHandler(this.btnBodySave_Click);
            // 
            // btnBodyLoad
            // 
            this.btnBodyLoad.Location = new System.Drawing.Point(6, 14);
            this.btnBodyLoad.Name = "btnBodyLoad";
            this.btnBodyLoad.Size = new System.Drawing.Size(55, 23);
            this.btnBodyLoad.TabIndex = 28;
            this.btnBodyLoad.Text = "Load";
            this.btnBodyLoad.UseVisualStyleBackColor = true;
            this.btnBodyLoad.Click += new System.EventHandler(this.btnBodyLoad_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.gbHistory);
            this.tabSettings.Controls.Add(this.chkCanAlter);
            this.tabSettings.Controls.Add(this.chkCanTransform);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(289, 409);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // gbHistory
            // 
            this.gbHistory.Controls.Add(this.lstHistory);
            this.gbHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbHistory.Location = new System.Drawing.Point(0, 34);
            this.gbHistory.Name = "gbHistory";
            this.gbHistory.Size = new System.Drawing.Size(289, 375);
            this.gbHistory.TabIndex = 2;
            this.gbHistory.TabStop = false;
            this.gbHistory.Text = "History";
            // 
            // lstHistory
            // 
            this.lstHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.Location = new System.Drawing.Point(3, 16);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(283, 356);
            this.lstHistory.TabIndex = 0;
            // 
            // chkCanAlter
            // 
            this.chkCanAlter.AutoSize = true;
            this.chkCanAlter.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCanAlter.Location = new System.Drawing.Point(0, 17);
            this.chkCanAlter.Name = "chkCanAlter";
            this.chkCanAlter.Size = new System.Drawing.Size(289, 17);
            this.chkCanAlter.TabIndex = 1;
            this.chkCanAlter.Text = "Allow Joint Adding / Removal";
            this.chkCanAlter.UseVisualStyleBackColor = true;
            // 
            // chkCanTransform
            // 
            this.chkCanTransform.AutoSize = true;
            this.chkCanTransform.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkCanTransform.Location = new System.Drawing.Point(0, 0);
            this.chkCanTransform.Name = "chkCanTransform";
            this.chkCanTransform.Size = new System.Drawing.Size(289, 17);
            this.chkCanTransform.TabIndex = 0;
            this.chkCanTransform.Text = "Allow Bone Length Altering";
            this.chkCanTransform.UseVisualStyleBackColor = true;
            // 
            // gbState
            // 
            this.gbState.Controls.Add(this.chkDrawBody);
            this.gbState.Controls.Add(this.chkDrawSkel);
            this.gbState.Controls.Add(this.radioAnimate);
            this.gbState.Controls.Add(this.radioEdit);
            this.gbState.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbState.Location = new System.Drawing.Point(0, 435);
            this.gbState.Name = "gbState";
            this.gbState.Size = new System.Drawing.Size(297, 114);
            this.gbState.TabIndex = 25;
            this.gbState.TabStop = false;
            this.gbState.Text = "Display State";
            // 
            // chkDrawBody
            // 
            this.chkDrawBody.AutoSize = true;
            this.chkDrawBody.Location = new System.Drawing.Point(23, 88);
            this.chkDrawBody.Name = "chkDrawBody";
            this.chkDrawBody.Size = new System.Drawing.Size(78, 17);
            this.chkDrawBody.TabIndex = 4;
            this.chkDrawBody.Text = "Draw Body";
            this.chkDrawBody.UseVisualStyleBackColor = true;
            // 
            // chkDrawSkel
            // 
            this.chkDrawSkel.AutoSize = true;
            this.chkDrawSkel.Location = new System.Drawing.Point(23, 65);
            this.chkDrawSkel.Name = "chkDrawSkel";
            this.chkDrawSkel.Size = new System.Drawing.Size(96, 17);
            this.chkDrawSkel.TabIndex = 3;
            this.chkDrawSkel.Text = "Draw Skeleton";
            this.chkDrawSkel.UseVisualStyleBackColor = true;
            // 
            // radioAnimate
            // 
            this.radioAnimate.AutoSize = true;
            this.radioAnimate.Location = new System.Drawing.Point(9, 42);
            this.radioAnimate.Name = "radioAnimate";
            this.radioAnimate.Size = new System.Drawing.Size(69, 17);
            this.radioAnimate.TabIndex = 2;
            this.radioAnimate.Text = "Animated";
            this.radioAnimate.UseVisualStyleBackColor = true;
            this.radioAnimate.CheckedChanged += new System.EventHandler(this.radioAnimate_CheckedChanged);
            // 
            // radioEdit
            // 
            this.radioEdit.AutoSize = true;
            this.radioEdit.Checked = true;
            this.radioEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioEdit.Location = new System.Drawing.Point(9, 19);
            this.radioEdit.Name = "radioEdit";
            this.radioEdit.Size = new System.Drawing.Size(84, 17);
            this.radioEdit.TabIndex = 0;
            this.radioEdit.TabStop = true;
            this.radioEdit.Text = "Frame Editor";
            this.radioEdit.UseVisualStyleBackColor = true;
            // 
            // SkeletonEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 549);
            this.Controls.Add(this.splitContainer2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "SkeletonEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skeleton Editor";
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tcMenu.ResumeLayout(false);
            this.tabSkeleton.ResumeLayout(false);
            this.tabSkeleton.PerformLayout();
            this.gbSkeletonActions.ResumeLayout(false);
            this.gbSelectedNode.ResumeLayout(false);
            this.gbSelectedNode.PerformLayout();
            this.gbSkeletonIO.ResumeLayout(false);
            this.tabAnimation.ResumeLayout(false);
            this.tabAnimation.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbAnimIO.ResumeLayout(false);
            this.tabBody.ResumeLayout(false);
            this.gbBodies.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbBodyIO.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.gbHistory.ResumeLayout(false);
            this.gbState.ResumeLayout(false);
            this.gbState.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer2;
        private SkeletonEditorScreenControl GameScreen;
        private System.Windows.Forms.GroupBox gbState;
        private System.Windows.Forms.CheckBox chkDrawBody;
        private System.Windows.Forms.CheckBox chkDrawSkel;
        private System.Windows.Forms.RadioButton radioAnimate;
        private System.Windows.Forms.RadioButton radioEdit;
        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tabSkeleton;
        private System.Windows.Forms.GroupBox gbSkeletonActions;
        private System.Windows.Forms.Button btnCopyInherits;
        private System.Windows.Forms.Button btnInterpolate;
        private System.Windows.Forms.Button btnShiftNodes;
        private System.Windows.Forms.Button btnCopyRoot;
        private System.Windows.Forms.Button btnCopyLen;
        private System.Windows.Forms.GroupBox gbSelectedNode;
        private System.Windows.Forms.CheckBox chkIsMod;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private SkeletonNodesComboBox cmbSkeletonNodes;
        private System.Windows.Forms.GroupBox gbSkeletonIO;
        private System.Windows.Forms.Button btnSkeletonSaveAs;
        private System.Windows.Forms.Button btnSkeletonSave;
        private System.Windows.Forms.Button btnSkeletonLoad;
        private System.Windows.Forms.Label lblSkeleton;
        private System.Windows.Forms.TabPage tabAnimation;
        private System.Windows.Forms.TextBox txtFrames;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFall;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.Button btnWalk;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStand;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.GroupBox gbAnimIO;
        private System.Windows.Forms.Button btnAnimSaveAs;
        private System.Windows.Forms.Button btnAnimSave;
        private System.Windows.Forms.Button btnAnimLoad;
        private System.Windows.Forms.Label lblAnimation;
        private System.Windows.Forms.TabPage tabBody;
        private System.Windows.Forms.GroupBox gbBodies;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private SkeletonBodyItemsListBox lstBodies;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClearTarget;
        private SkeletonNodesComboBox cmbTarget;
        private System.Windows.Forms.Label label15;
        private SkeletonNodesComboBox cmbSource;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtGrhIndex;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtOriginY;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOriginX;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtOffsetY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOffsetX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbBodyIO;
        private System.Windows.Forms.Button btnBodySaveAs;
        private System.Windows.Forms.Button btnBodySave;
        private System.Windows.Forms.Button btnBodyLoad;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox gbHistory;
        private System.Windows.Forms.ListBox lstHistory;
        private System.Windows.Forms.CheckBox chkCanAlter;
        private System.Windows.Forms.CheckBox chkCanTransform;
        private System.Windows.Forms.ToolTip tt;
        private SelectGrhDataButton btnSelectBodyGrhData;
        private System.Windows.Forms.ComboBox cmbSkeletonBodies;
        private System.Windows.Forms.ComboBox cmbSkeletonBodyNodes;
    }
}

