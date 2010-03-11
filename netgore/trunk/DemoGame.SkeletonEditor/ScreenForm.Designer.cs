// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace DemoGame.SkeletonEditor
{
    partial class ScreenForm
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
            this.tcMenu = new System.Windows.Forms.TabControl();
            this.tabSkeleton = new System.Windows.Forms.TabPage();
            this.gbNodes = new System.Windows.Forms.GroupBox();
            this.cmbSkeletonNodes = new System.Windows.Forms.ComboBox();
            this.gbSkeletonActions = new System.Windows.Forms.GroupBox();
            this.btnCopyInherits = new System.Windows.Forms.Button();
            this.btnInterpolate = new System.Windows.Forms.Button();
            this.btnShiftNodes = new System.Windows.Forms.Button();
            this.btnCopyRoot = new System.Windows.Forms.Button();
            this.btnCopyLen = new System.Windows.Forms.Button();
            this.gbSkeletonIO = new System.Windows.Forms.GroupBox();
            this.btnSkeletonSaveAs = new System.Windows.Forms.Button();
            this.btnSkeletonSave = new System.Windows.Forms.Button();
            this.btnSkeletonLoad = new System.Windows.Forms.Button();
            this.lblSkeleton = new System.Windows.Forms.Label();
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
            this.tabAnimation = new System.Windows.Forms.TabPage();
            this.btnFall = new System.Windows.Forms.Button();
            this.btnJump = new System.Windows.Forms.Button();
            this.btnWalk = new System.Windows.Forms.Button();
            this.gbAnimIO = new System.Windows.Forms.GroupBox();
            this.btnAnimSaveAs = new System.Windows.Forms.Button();
            this.btnAnimSave = new System.Windows.Forms.Button();
            this.btnAnimLoad = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStand = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lblAnimation = new System.Windows.Forms.Label();
            this.gbFrames = new System.Windows.Forms.GroupBox();
            this.txtFrames = new System.Windows.Forms.TextBox();
            this.tabBody = new System.Windows.Forms.TabPage();
            this.gbBodyIO = new System.Windows.Forms.GroupBox();
            this.btnBodySaveAs = new System.Windows.Forms.Button();
            this.btnBodySave = new System.Windows.Forms.Button();
            this.btnBodyLoad = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTarget = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbSource = new System.Windows.Forms.ComboBox();
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
            this.gbBodies = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.lstBodies = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
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
            this.lblXY = new System.Windows.Forms.Label();
            this.GameScreen = new DemoGame.SkeletonEditor.GameScreenControl();
            this.tcMenu.SuspendLayout();
            this.tabSkeleton.SuspendLayout();
            this.gbNodes.SuspendLayout();
            this.gbSkeletonActions.SuspendLayout();
            this.gbSkeletonIO.SuspendLayout();
            this.gbSelectedNode.SuspendLayout();
            this.tabAnimation.SuspendLayout();
            this.gbAnimIO.SuspendLayout();
            this.gbFrames.SuspendLayout();
            this.tabBody.SuspendLayout();
            this.gbBodyIO.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbBodies.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.gbHistory.SuspendLayout();
            this.gbState.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMenu
            // 
            this.tcMenu.Controls.Add(this.tabSkeleton);
            this.tcMenu.Controls.Add(this.tabAnimation);
            this.tcMenu.Controls.Add(this.tabBody);
            this.tcMenu.Controls.Add(this.tabSettings);
            this.tcMenu.Location = new System.Drawing.Point(815, 12);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(211, 497);
            this.tcMenu.TabIndex = 23;
            // 
            // tabSkeleton
            // 
            this.tabSkeleton.Controls.Add(this.gbNodes);
            this.tabSkeleton.Controls.Add(this.gbSkeletonActions);
            this.tabSkeleton.Controls.Add(this.gbSkeletonIO);
            this.tabSkeleton.Controls.Add(this.lblSkeleton);
            this.tabSkeleton.Controls.Add(this.gbSelectedNode);
            this.tabSkeleton.Location = new System.Drawing.Point(4, 22);
            this.tabSkeleton.Name = "tabSkeleton";
            this.tabSkeleton.Padding = new System.Windows.Forms.Padding(3);
            this.tabSkeleton.Size = new System.Drawing.Size(203, 471);
            this.tabSkeleton.TabIndex = 0;
            this.tabSkeleton.Text = "Skeleton";
            this.tabSkeleton.UseVisualStyleBackColor = true;
            // 
            // gbNodes
            // 
            this.gbNodes.Controls.Add(this.cmbSkeletonNodes);
            this.gbNodes.Location = new System.Drawing.Point(6, 19);
            this.gbNodes.Name = "gbNodes";
            this.gbNodes.Size = new System.Drawing.Size(191, 48);
            this.gbNodes.TabIndex = 47;
            this.gbNodes.TabStop = false;
            this.gbNodes.Text = "Skeleton Nodes";
            // 
            // cmbSkeletonNodes
            // 
            this.cmbSkeletonNodes.FormattingEnabled = true;
            this.cmbSkeletonNodes.Location = new System.Drawing.Point(8, 19);
            this.cmbSkeletonNodes.Name = "cmbSkeletonNodes";
            this.cmbSkeletonNodes.Size = new System.Drawing.Size(175, 21);
            this.cmbSkeletonNodes.Sorted = true;
            this.cmbSkeletonNodes.TabIndex = 47;
            this.cmbSkeletonNodes.SelectedIndexChanged += new System.EventHandler(this.cmbSkeletonNodes_SelectedIndexChanged);
            // 
            // gbSkeletonActions
            // 
            this.gbSkeletonActions.Controls.Add(this.btnCopyInherits);
            this.gbSkeletonActions.Controls.Add(this.btnInterpolate);
            this.gbSkeletonActions.Controls.Add(this.btnShiftNodes);
            this.gbSkeletonActions.Controls.Add(this.btnCopyRoot);
            this.gbSkeletonActions.Controls.Add(this.btnCopyLen);
            this.gbSkeletonActions.Location = new System.Drawing.Point(9, 314);
            this.gbSkeletonActions.Name = "gbSkeletonActions";
            this.gbSkeletonActions.Size = new System.Drawing.Size(191, 102);
            this.gbSkeletonActions.TabIndex = 33;
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
            // gbSkeletonIO
            // 
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonSaveAs);
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonSave);
            this.gbSkeletonIO.Controls.Add(this.btnSkeletonLoad);
            this.gbSkeletonIO.Location = new System.Drawing.Point(6, 422);
            this.gbSkeletonIO.Name = "gbSkeletonIO";
            this.gbSkeletonIO.Size = new System.Drawing.Size(191, 43);
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
            this.lblSkeleton.Location = new System.Drawing.Point(3, 3);
            this.lblSkeleton.Name = "lblSkeleton";
            this.lblSkeleton.Size = new System.Drawing.Size(46, 13);
            this.lblSkeleton.TabIndex = 24;
            this.lblSkeleton.Text = "Loaded:";
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
            this.gbSelectedNode.Location = new System.Drawing.Point(6, 73);
            this.gbSelectedNode.Name = "gbSelectedNode";
            this.gbSelectedNode.Size = new System.Drawing.Size(191, 146);
            this.gbSelectedNode.TabIndex = 20;
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
            this.txtAngle.TextChanged += new System.EventHandler(this.txtAngle_TextChanged);
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
            this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
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
            // tabAnimation
            // 
            this.tabAnimation.Controls.Add(this.btnFall);
            this.tabAnimation.Controls.Add(this.btnJump);
            this.tabAnimation.Controls.Add(this.btnWalk);
            this.tabAnimation.Controls.Add(this.gbAnimIO);
            this.tabAnimation.Controls.Add(this.btnPause);
            this.tabAnimation.Controls.Add(this.btnStand);
            this.tabAnimation.Controls.Add(this.btnPlay);
            this.tabAnimation.Controls.Add(this.lblAnimation);
            this.tabAnimation.Controls.Add(this.gbFrames);
            this.tabAnimation.Location = new System.Drawing.Point(4, 22);
            this.tabAnimation.Name = "tabAnimation";
            this.tabAnimation.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnimation.Size = new System.Drawing.Size(203, 471);
            this.tabAnimation.TabIndex = 1;
            this.tabAnimation.Text = "Animation";
            this.tabAnimation.UseVisualStyleBackColor = true;
            // 
            // btnFall
            // 
            this.btnFall.Location = new System.Drawing.Point(134, 393);
            this.btnFall.Name = "btnFall";
            this.btnFall.Size = new System.Drawing.Size(55, 23);
            this.btnFall.TabIndex = 33;
            this.btnFall.Text = "Fall";
            this.btnFall.UseVisualStyleBackColor = true;
            this.btnFall.Click += new System.EventHandler(this.btnFall_Click);
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(73, 393);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(55, 23);
            this.btnJump.TabIndex = 32;
            this.btnJump.Text = "Jump";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnWalk
            // 
            this.btnWalk.Location = new System.Drawing.Point(12, 393);
            this.btnWalk.Name = "btnWalk";
            this.btnWalk.Size = new System.Drawing.Size(55, 23);
            this.btnWalk.TabIndex = 31;
            this.btnWalk.Text = "Walk";
            this.btnWalk.UseVisualStyleBackColor = true;
            this.btnWalk.Click += new System.EventHandler(this.btnWalk_Click);
            // 
            // gbAnimIO
            // 
            this.gbAnimIO.Controls.Add(this.btnAnimSaveAs);
            this.gbAnimIO.Controls.Add(this.btnAnimSave);
            this.gbAnimIO.Controls.Add(this.btnAnimLoad);
            this.gbAnimIO.Location = new System.Drawing.Point(6, 422);
            this.gbAnimIO.Name = "gbAnimIO";
            this.gbAnimIO.Size = new System.Drawing.Size(191, 43);
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
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(73, 364);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(55, 23);
            this.btnPause.TabIndex = 29;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStand
            // 
            this.btnStand.Location = new System.Drawing.Point(134, 364);
            this.btnStand.Name = "btnStand";
            this.btnStand.Size = new System.Drawing.Size(55, 23);
            this.btnStand.TabIndex = 28;
            this.btnStand.Text = "Stand";
            this.btnStand.UseVisualStyleBackColor = true;
            this.btnStand.Click += new System.EventHandler(this.btnStand_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 364);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(55, 23);
            this.btnPlay.TabIndex = 27;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblAnimation
            // 
            this.lblAnimation.AutoSize = true;
            this.lblAnimation.Location = new System.Drawing.Point(3, 3);
            this.lblAnimation.Name = "lblAnimation";
            this.lblAnimation.Size = new System.Drawing.Size(46, 13);
            this.lblAnimation.TabIndex = 23;
            this.lblAnimation.Text = "Loaded:";
            // 
            // gbFrames
            // 
            this.gbFrames.Controls.Add(this.txtFrames);
            this.gbFrames.Location = new System.Drawing.Point(6, 19);
            this.gbFrames.Name = "gbFrames";
            this.gbFrames.Size = new System.Drawing.Size(191, 339);
            this.gbFrames.TabIndex = 22;
            this.gbFrames.TabStop = false;
            this.gbFrames.Text = "Frames:";
            // 
            // txtFrames
            // 
            this.txtFrames.Location = new System.Drawing.Point(6, 19);
            this.txtFrames.Multiline = true;
            this.txtFrames.Name = "txtFrames";
            this.txtFrames.Size = new System.Drawing.Size(179, 314);
            this.txtFrames.TabIndex = 1;
            this.txtFrames.TextChanged += new System.EventHandler(this.txtFrames_TextChanged);
            // 
            // tabBody
            // 
            this.tabBody.Controls.Add(this.gbBodyIO);
            this.tabBody.Controls.Add(this.groupBox1);
            this.tabBody.Controls.Add(this.gbBodies);
            this.tabBody.Location = new System.Drawing.Point(4, 22);
            this.tabBody.Name = "tabBody";
            this.tabBody.Size = new System.Drawing.Size(203, 471);
            this.tabBody.TabIndex = 2;
            this.tabBody.Text = "Body";
            this.tabBody.UseVisualStyleBackColor = true;
            // 
            // gbBodyIO
            // 
            this.gbBodyIO.Controls.Add(this.btnBodySaveAs);
            this.gbBodyIO.Controls.Add(this.btnBodySave);
            this.gbBodyIO.Controls.Add(this.btnBodyLoad);
            this.gbBodyIO.Location = new System.Drawing.Point(5, 425);
            this.gbBodyIO.Name = "gbBodyIO";
            this.gbBodyIO.Size = new System.Drawing.Size(191, 43);
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
            // groupBox1
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(5, 266);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 153);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Body";
            // 
            // cmbTarget
            // 
            this.cmbTarget.FormattingEnabled = true;
            this.cmbTarget.Location = new System.Drawing.Point(60, 122);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(124, 21);
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
            this.txtGrhIndex.Size = new System.Drawing.Size(124, 20);
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
            // gbBodies
            // 
            this.gbBodies.Controls.Add(this.btnDown);
            this.gbBodies.Controls.Add(this.btnUp);
            this.gbBodies.Controls.Add(this.lstBodies);
            this.gbBodies.Controls.Add(this.btnDelete);
            this.gbBodies.Controls.Add(this.btnAdd);
            this.gbBodies.Location = new System.Drawing.Point(5, 3);
            this.gbBodies.Name = "gbBodies";
            this.gbBodies.Size = new System.Drawing.Size(191, 257);
            this.gbBodies.TabIndex = 25;
            this.gbBodies.TabStop = false;
            this.gbBodies.Text = "Bone Bodies:";
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(155, 230);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(28, 21);
            this.btnDown.TabIndex = 36;
            this.btnDown.Text = "\\/";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(6, 230);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(28, 21);
            this.btnUp.TabIndex = 35;
            this.btnUp.Text = "/\\";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // lstBodies
            // 
            this.lstBodies.FormattingEnabled = true;
            this.lstBodies.Location = new System.Drawing.Point(6, 19);
            this.lstBodies.Name = "lstBodies";
            this.lstBodies.Size = new System.Drawing.Size(179, 199);
            this.lstBodies.TabIndex = 34;
            this.lstBodies.SelectedIndexChanged += new System.EventHandler(this.lstBodies_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(98, 230);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(52, 21);
            this.btnDelete.TabIndex = 33;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(40, 230);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(52, 21);
            this.btnAdd.TabIndex = 32;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.gbHistory);
            this.tabSettings.Controls.Add(this.chkCanAlter);
            this.tabSettings.Controls.Add(this.chkCanTransform);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(203, 471);
            this.tabSettings.TabIndex = 3;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // gbHistory
            // 
            this.gbHistory.Controls.Add(this.lstHistory);
            this.gbHistory.Location = new System.Drawing.Point(4, 58);
            this.gbHistory.Name = "gbHistory";
            this.gbHistory.Size = new System.Drawing.Size(197, 382);
            this.gbHistory.TabIndex = 2;
            this.gbHistory.TabStop = false;
            this.gbHistory.Text = "History";
            // 
            // lstHistory
            // 
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.Location = new System.Drawing.Point(5, 19);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(186, 355);
            this.lstHistory.TabIndex = 0;
            // 
            // chkCanAlter
            // 
            this.chkCanAlter.AutoSize = true;
            this.chkCanAlter.Location = new System.Drawing.Point(9, 35);
            this.chkCanAlter.Name = "chkCanAlter";
            this.chkCanAlter.Size = new System.Drawing.Size(165, 17);
            this.chkCanAlter.TabIndex = 1;
            this.chkCanAlter.Text = "Allow Joint Adding / Removal";
            this.chkCanAlter.UseVisualStyleBackColor = true;
            // 
            // chkCanTransform
            // 
            this.chkCanTransform.AutoSize = true;
            this.chkCanTransform.Location = new System.Drawing.Point(9, 12);
            this.chkCanTransform.Name = "chkCanTransform";
            this.chkCanTransform.Size = new System.Drawing.Size(153, 17);
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
            this.gbState.Location = new System.Drawing.Point(819, 515);
            this.gbState.Name = "gbState";
            this.gbState.Size = new System.Drawing.Size(203, 114);
            this.gbState.TabIndex = 24;
            this.gbState.TabStop = false;
            this.gbState.Text = "Display State";
            // 
            // chkDrawBody
            // 
            this.chkDrawBody.AutoSize = true;
            this.chkDrawBody.Checked = true;
            this.chkDrawBody.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.chkDrawSkel.Checked = true;
            this.chkDrawSkel.CheckState = System.Windows.Forms.CheckState.Checked;
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
            // lblXY
            // 
            this.lblXY.Location = new System.Drawing.Point(695, 615);
            this.lblXY.Name = "lblXY";
            this.lblXY.Size = new System.Drawing.Size(114, 14);
            this.lblXY.TabIndex = 25;
            this.lblXY.Text = "X: 0, Y: 0";
            this.lblXY.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Location = new System.Drawing.Point(9, 12);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.ScreenForm = null;
            this.GameScreen.Size = new System.Drawing.Size(800, 600);
            this.GameScreen.TabIndex = 0;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseUp);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 636);
            this.Controls.Add(this.lblXY);
            this.Controls.Add(this.gbState);
            this.Controls.Add(this.tcMenu);
            this.Controls.Add(this.GameScreen);
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skeleton Editor";
            this.tcMenu.ResumeLayout(false);
            this.tabSkeleton.ResumeLayout(false);
            this.tabSkeleton.PerformLayout();
            this.gbNodes.ResumeLayout(false);
            this.gbSkeletonActions.ResumeLayout(false);
            this.gbSkeletonIO.ResumeLayout(false);
            this.gbSelectedNode.ResumeLayout(false);
            this.gbSelectedNode.PerformLayout();
            this.tabAnimation.ResumeLayout(false);
            this.tabAnimation.PerformLayout();
            this.gbAnimIO.ResumeLayout(false);
            this.gbFrames.ResumeLayout(false);
            this.gbFrames.PerformLayout();
            this.tabBody.ResumeLayout(false);
            this.gbBodyIO.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbBodies.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.gbHistory.ResumeLayout(false);
            this.gbState.ResumeLayout(false);
            this.gbState.PerformLayout();
            this.ResumeLayout(false);

        }



        private GameScreenControl GameScreen;
        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tabSkeleton;
        private System.Windows.Forms.TabPage tabAnimation;
        private System.Windows.Forms.GroupBox gbSelectedNode;
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
        private System.Windows.Forms.GroupBox gbFrames;
        private System.Windows.Forms.TextBox txtFrames;
        private System.Windows.Forms.TabPage tabBody;
        private System.Windows.Forms.GroupBox gbBodies;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ListBox lstBodies;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox gbState;
        private System.Windows.Forms.CheckBox chkDrawBody;
        private System.Windows.Forms.CheckBox chkDrawSkel;
        private System.Windows.Forms.RadioButton radioAnimate;
        private System.Windows.Forms.RadioButton radioEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbTarget;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbSource;
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
        private System.Windows.Forms.Label lblAnimation;
        private System.Windows.Forms.Label lblSkeleton;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStand;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.GroupBox gbAnimIO;
        private System.Windows.Forms.Button btnAnimSaveAs;
        private System.Windows.Forms.Button btnAnimSave;
        private System.Windows.Forms.Button btnAnimLoad;
        private System.Windows.Forms.GroupBox gbSkeletonIO;
        private System.Windows.Forms.Button btnSkeletonSaveAs;
        private System.Windows.Forms.Button btnSkeletonSave;
        private System.Windows.Forms.Button btnSkeletonLoad;
        private System.Windows.Forms.GroupBox gbBodyIO;
        private System.Windows.Forms.Button btnBodySaveAs;
        private System.Windows.Forms.Button btnBodySave;
        private System.Windows.Forms.Button btnBodyLoad;
        private System.Windows.Forms.GroupBox gbSkeletonActions;
        private System.Windows.Forms.Button btnCopyLen;
        private System.Windows.Forms.Button btnCopyRoot;
        private System.Windows.Forms.GroupBox gbNodes;
        private System.Windows.Forms.ComboBox cmbSkeletonNodes;
        private System.Windows.Forms.Button btnShiftNodes;
        private System.Windows.Forms.CheckBox chkCanTransform;
        private System.Windows.Forms.CheckBox chkCanAlter;
        private System.Windows.Forms.Label lblXY;
        private System.Windows.Forms.GroupBox gbHistory;
        private System.Windows.Forms.ListBox lstHistory;
        private System.Windows.Forms.Button btnInterpolate;
        private System.Windows.Forms.Button btnFall;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.Button btnWalk;
        private System.Windows.Forms.Button btnCopyInherits;
        private System.Windows.Forms.CheckBox chkIsMod;
    }
}

