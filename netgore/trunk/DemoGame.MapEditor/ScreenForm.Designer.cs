// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace DemoGame.MapEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenForm));
            this.tcMenu = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.MapSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.cmdApplySize = new System.Windows.Forms.Button();
            this.txtMapHeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMapWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.tabPageGrhs = new System.Windows.Forms.TabPage();
            this.chkForeground = new System.Windows.Forms.CheckBox();
            this.chkSnapGrhGrid = new System.Windows.Forms.CheckBox();
            this.treeGrhs = new NetGore.EditorTools.GrhTreeView();
            this.tabPageWalls = new System.Windows.Forms.TabPage();
            this.cmbWallType = new System.Windows.Forms.ComboBox();
            this.gbCurrentWall = new System.Windows.Forms.GroupBox();
            this.cmbCurrWallType = new System.Windows.Forms.ComboBox();
            this.chkSnapWallGrid = new System.Windows.Forms.CheckBox();
            this.chkSnapWallWall = new System.Windows.Forms.CheckBox();
            this.lstSelectedWalls = new System.Windows.Forms.ListBox();
            this.tabPageEnvironment = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTeleports = new System.Windows.Forms.TabPage();
            this.btnTeleportNew = new System.Windows.Forms.Button();
            this.gbSelectedTeleporter = new System.Windows.Forms.GroupBox();
            this.txtTeleportMap = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTeleportHeight = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTeleportWidth = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTeleportLocate = new System.Windows.Forms.Button();
            this.btnTeleportDelete = new System.Windows.Forms.Button();
            this.btnTeleportCopy = new System.Windows.Forms.Button();
            this.txtTeleportToY = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTeleportToX = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTeleportY = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTeleportX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lstTeleports = new System.Windows.Forms.ListBox();
            this.tabPageNPCs = new System.Windows.Forms.TabPage();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDrawEntities = new System.Windows.Forms.CheckBox();
            this.chkDrawAutoWalls = new System.Windows.Forms.CheckBox();
            this.chkShowGrhs = new System.Windows.Forms.CheckBox();
            this.chkShowWalls = new System.Windows.Forms.CheckBox();
            this.chkDrawGrid = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGridHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGridWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdLoad = new System.Windows.Forms.Button();
            this.cmdNew = new System.Windows.Forms.Button();
            this.cmdOptimize = new System.Windows.Forms.Button();
            this.panToolBar = new System.Windows.Forms.Panel();
            this.picToolGrhsAdd = new System.Windows.Forms.PictureBox();
            this.picToolGrhs = new System.Windows.Forms.PictureBox();
            this.picToolWallsAdd = new System.Windows.Forms.PictureBox();
            this.picToolWalls = new System.Windows.Forms.PictureBox();
            this.picToolSelect = new System.Windows.Forms.PictureBox();
            this.GameScreen = new DemoGame.MapEditor.GameScreenControl();
            this.tcMenu.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.MapSizeGroupBox.SuspendLayout();
            this.tabPageGrhs.SuspendLayout();
            this.tabPageWalls.SuspendLayout();
            this.gbCurrentWall.SuspendLayout();
            this.tabPageEnvironment.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTeleports.SuspendLayout();
            this.gbSelectedTeleporter.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picToolGrhsAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolGrhs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolWallsAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolWalls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // tcMenu
            // 
            this.tcMenu.Controls.Add(this.tabPageGeneral);
            this.tcMenu.Controls.Add(this.tabPageGrhs);
            this.tcMenu.Controls.Add(this.tabPageWalls);
            this.tcMenu.Controls.Add(this.tabPageEnvironment);
            this.tcMenu.Controls.Add(this.tabPageNPCs);
            this.tcMenu.Controls.Add(this.tabPageSettings);
            this.tcMenu.Location = new System.Drawing.Point(839, 3);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(304, 570);
            this.tcMenu.TabIndex = 1;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.MapSizeGroupBox);
            this.tabPageGeneral.Controls.Add(this.txtMapName);
            this.tabPageGeneral.Controls.Add(this.NameLabel);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(296, 544);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.ToolTipText = "General map information";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // MapSizeGroupBox
            // 
            this.MapSizeGroupBox.Controls.Add(this.cmdApplySize);
            this.MapSizeGroupBox.Controls.Add(this.txtMapHeight);
            this.MapSizeGroupBox.Controls.Add(this.label2);
            this.MapSizeGroupBox.Controls.Add(this.txtMapWidth);
            this.MapSizeGroupBox.Controls.Add(this.label1);
            this.MapSizeGroupBox.Location = new System.Drawing.Point(9, 36);
            this.MapSizeGroupBox.Name = "MapSizeGroupBox";
            this.MapSizeGroupBox.Size = new System.Drawing.Size(272, 49);
            this.MapSizeGroupBox.TabIndex = 3;
            this.MapSizeGroupBox.TabStop = false;
            this.MapSizeGroupBox.Text = "Map Size";
            // 
            // cmdApplySize
            // 
            this.cmdApplySize.Location = new System.Drawing.Point(216, 19);
            this.cmdApplySize.Name = "cmdApplySize";
            this.cmdApplySize.Size = new System.Drawing.Size(50, 25);
            this.cmdApplySize.TabIndex = 7;
            this.cmdApplySize.Text = "Apply";
            this.cmdApplySize.UseVisualStyleBackColor = true;
            this.cmdApplySize.Click += new System.EventHandler(this.cmdApplySize_Click);
            // 
            // txtMapHeight
            // 
            this.txtMapHeight.Location = new System.Drawing.Point(150, 19);
            this.txtMapHeight.Name = "txtMapHeight";
            this.txtMapHeight.Size = new System.Drawing.Size(47, 20);
            this.txtMapHeight.TabIndex = 6;
            this.txtMapHeight.TextChanged += new System.EventHandler(this.txtMapHeight_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Height:";
            // 
            // txtMapWidth
            // 
            this.txtMapWidth.Location = new System.Drawing.Point(50, 19);
            this.txtMapWidth.Name = "txtMapWidth";
            this.txtMapWidth.Size = new System.Drawing.Size(47, 20);
            this.txtMapWidth.TabIndex = 4;
            this.txtMapWidth.TextChanged += new System.EventHandler(this.txtMapWidth_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Width:";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(74, 10);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(207, 20);
            this.txtMapName.TabIndex = 1;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(6, 13);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(62, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Map Name:";
            // 
            // tabPageGrhs
            // 
            this.tabPageGrhs.Controls.Add(this.chkForeground);
            this.tabPageGrhs.Controls.Add(this.chkSnapGrhGrid);
            this.tabPageGrhs.Controls.Add(this.treeGrhs);
            this.tabPageGrhs.Location = new System.Drawing.Point(4, 22);
            this.tabPageGrhs.Name = "tabPageGrhs";
            this.tabPageGrhs.Size = new System.Drawing.Size(296, 544);
            this.tabPageGrhs.TabIndex = 5;
            this.tabPageGrhs.Text = "Grhs";
            this.tabPageGrhs.UseVisualStyleBackColor = true;
            this.tabPageGrhs.Enter += new System.EventHandler(this.tabPageGrhs_Enter);
            // 
            // chkForeground
            // 
            this.chkForeground.AutoSize = true;
            this.chkForeground.Location = new System.Drawing.Point(98, 6);
            this.chkForeground.Name = "chkForeground";
            this.chkForeground.Size = new System.Drawing.Size(80, 17);
            this.chkForeground.TabIndex = 9;
            this.chkForeground.Text = "Foreground";
            this.chkForeground.UseVisualStyleBackColor = true;
            // 
            // chkSnapGrhGrid
            // 
            this.chkSnapGrhGrid.AutoSize = true;
            this.chkSnapGrhGrid.Checked = true;
            this.chkSnapGrhGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapGrhGrid.Location = new System.Drawing.Point(3, 6);
            this.chkSnapGrhGrid.Name = "chkSnapGrhGrid";
            this.chkSnapGrhGrid.Size = new System.Drawing.Size(89, 17);
            this.chkSnapGrhGrid.TabIndex = 7;
            this.chkSnapGrhGrid.Text = "Snap To Grid";
            this.chkSnapGrhGrid.UseVisualStyleBackColor = true;
            // 
            // treeGrhs
            // 
            this.treeGrhs.AllowDrop = true;
            this.treeGrhs.ImageIndex = 0;
            this.treeGrhs.ImageSize = new System.Drawing.Size(16, 16);
            this.treeGrhs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeGrhs.LabelEdit = true;
            this.treeGrhs.Location = new System.Drawing.Point(3, 29);
            this.treeGrhs.Name = "treeGrhs";
            this.treeGrhs.SelectedImageIndex = 0;
            this.treeGrhs.ShowNodeToolTips = true;
            this.treeGrhs.Size = new System.Drawing.Size(290, 512);
            this.treeGrhs.Sorted = true;
            this.treeGrhs.TabIndex = 8;
            this.treeGrhs.GrhAfterSelect += new NetGore.EditorTools.GrhTreeViewEvent(this.treeGrhs_SelectGrh);
            this.treeGrhs.GrhMouseDoubleClick += new NetGore.EditorTools.GrhTreeNodeMouseClickEvent(this.treeGrhs_DoubleClickGrh);
            // 
            // tabPageWalls
            // 
            this.tabPageWalls.Controls.Add(this.cmbWallType);
            this.tabPageWalls.Controls.Add(this.gbCurrentWall);
            this.tabPageWalls.Controls.Add(this.chkSnapWallGrid);
            this.tabPageWalls.Controls.Add(this.chkSnapWallWall);
            this.tabPageWalls.Controls.Add(this.lstSelectedWalls);
            this.tabPageWalls.Location = new System.Drawing.Point(4, 22);
            this.tabPageWalls.Name = "tabPageWalls";
            this.tabPageWalls.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWalls.Size = new System.Drawing.Size(296, 544);
            this.tabPageWalls.TabIndex = 1;
            this.tabPageWalls.Text = "Walls";
            this.tabPageWalls.ToolTipText = "Collision walls";
            this.tabPageWalls.UseVisualStyleBackColor = true;
            // 
            // cmbWallType
            // 
            this.cmbWallType.FormattingEnabled = true;
            this.cmbWallType.Location = new System.Drawing.Point(3, 29);
            this.cmbWallType.Name = "cmbWallType";
            this.cmbWallType.Size = new System.Drawing.Size(147, 21);
            this.cmbWallType.TabIndex = 7;
            // 
            // gbCurrentWall
            // 
            this.gbCurrentWall.Controls.Add(this.cmbCurrWallType);
            this.gbCurrentWall.Enabled = false;
            this.gbCurrentWall.Location = new System.Drawing.Point(6, 455);
            this.gbCurrentWall.Name = "gbCurrentWall";
            this.gbCurrentWall.Size = new System.Drawing.Size(284, 79);
            this.gbCurrentWall.TabIndex = 6;
            this.gbCurrentWall.TabStop = false;
            this.gbCurrentWall.Text = "Selected Wall";
            // 
            // cmbCurrWallType
            // 
            this.cmbCurrWallType.FormattingEnabled = true;
            this.cmbCurrWallType.Location = new System.Drawing.Point(6, 19);
            this.cmbCurrWallType.Name = "cmbCurrWallType";
            this.cmbCurrWallType.Size = new System.Drawing.Size(110, 21);
            this.cmbCurrWallType.TabIndex = 0;
            this.cmbCurrWallType.SelectedIndexChanged += new System.EventHandler(this.cmbCurrWallType_SelectedIndexChanged);
            // 
            // chkSnapWallGrid
            // 
            this.chkSnapWallGrid.AutoSize = true;
            this.chkSnapWallGrid.Checked = true;
            this.chkSnapWallGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapWallGrid.Location = new System.Drawing.Point(6, 6);
            this.chkSnapWallGrid.Name = "chkSnapWallGrid";
            this.chkSnapWallGrid.Size = new System.Drawing.Size(89, 17);
            this.chkSnapWallGrid.TabIndex = 5;
            this.chkSnapWallGrid.Text = "Snap To Grid";
            this.chkSnapWallGrid.UseVisualStyleBackColor = true;
            // 
            // chkSnapWallWall
            // 
            this.chkSnapWallWall.AutoSize = true;
            this.chkSnapWallWall.Checked = true;
            this.chkSnapWallWall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapWallWall.Location = new System.Drawing.Point(101, 6);
            this.chkSnapWallWall.Name = "chkSnapWallWall";
            this.chkSnapWallWall.Size = new System.Drawing.Size(96, 17);
            this.chkSnapWallWall.TabIndex = 4;
            this.chkSnapWallWall.Text = "Snap To Walls";
            this.chkSnapWallWall.UseVisualStyleBackColor = true;
            // 
            // lstSelectedWalls
            // 
            this.lstSelectedWalls.FormattingEnabled = true;
            this.lstSelectedWalls.Location = new System.Drawing.Point(3, 56);
            this.lstSelectedWalls.Name = "lstSelectedWalls";
            this.lstSelectedWalls.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelectedWalls.Size = new System.Drawing.Size(290, 394);
            this.lstSelectedWalls.Sorted = true;
            this.lstSelectedWalls.TabIndex = 3;
            this.lstSelectedWalls.SelectedValueChanged += new System.EventHandler(this.lstSelectedWalls_SelectedValueChanged);
            // 
            // tabPageEnvironment
            // 
            this.tabPageEnvironment.Controls.Add(this.tabControl1);
            this.tabPageEnvironment.Location = new System.Drawing.Point(4, 22);
            this.tabPageEnvironment.Name = "tabPageEnvironment";
            this.tabPageEnvironment.Size = new System.Drawing.Size(296, 544);
            this.tabPageEnvironment.TabIndex = 2;
            this.tabPageEnvironment.Text = "Environment";
            this.tabPageEnvironment.ToolTipText = "Graphics and effects";
            this.tabPageEnvironment.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTeleports);
            this.tabControl1.Location = new System.Drawing.Point(0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(296, 541);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTeleports
            // 
            this.tabTeleports.Controls.Add(this.btnTeleportNew);
            this.tabTeleports.Controls.Add(this.gbSelectedTeleporter);
            this.tabTeleports.Controls.Add(this.lstTeleports);
            this.tabTeleports.Location = new System.Drawing.Point(4, 22);
            this.tabTeleports.Name = "tabTeleports";
            this.tabTeleports.Padding = new System.Windows.Forms.Padding(3);
            this.tabTeleports.Size = new System.Drawing.Size(288, 515);
            this.tabTeleports.TabIndex = 0;
            this.tabTeleports.Text = "Teleports";
            this.tabTeleports.UseVisualStyleBackColor = true;
            // 
            // btnTeleportNew
            // 
            this.btnTeleportNew.Location = new System.Drawing.Point(220, 338);
            this.btnTeleportNew.Name = "btnTeleportNew";
            this.btnTeleportNew.Size = new System.Drawing.Size(59, 24);
            this.btnTeleportNew.TabIndex = 9;
            this.btnTeleportNew.Text = "New";
            this.btnTeleportNew.UseVisualStyleBackColor = true;
            this.btnTeleportNew.Click += new System.EventHandler(this.btnTeleportNew_Click);
            // 
            // gbSelectedTeleporter
            // 
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportMap);
            this.gbSelectedTeleporter.Controls.Add(this.label11);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportHeight);
            this.gbSelectedTeleporter.Controls.Add(this.label10);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportWidth);
            this.gbSelectedTeleporter.Controls.Add(this.label9);
            this.gbSelectedTeleporter.Controls.Add(this.btnTeleportLocate);
            this.gbSelectedTeleporter.Controls.Add(this.btnTeleportDelete);
            this.gbSelectedTeleporter.Controls.Add(this.btnTeleportCopy);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportToY);
            this.gbSelectedTeleporter.Controls.Add(this.label7);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportToX);
            this.gbSelectedTeleporter.Controls.Add(this.label8);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportY);
            this.gbSelectedTeleporter.Controls.Add(this.label6);
            this.gbSelectedTeleporter.Controls.Add(this.txtTeleportX);
            this.gbSelectedTeleporter.Controls.Add(this.label5);
            this.gbSelectedTeleporter.Location = new System.Drawing.Point(9, 358);
            this.gbSelectedTeleporter.Name = "gbSelectedTeleporter";
            this.gbSelectedTeleporter.Size = new System.Drawing.Size(270, 151);
            this.gbSelectedTeleporter.TabIndex = 5;
            this.gbSelectedTeleporter.TabStop = false;
            this.gbSelectedTeleporter.Text = "Selected Teleporter";
            // 
            // txtTeleportMap
            // 
            this.txtTeleportMap.Location = new System.Drawing.Point(51, 16);
            this.txtTeleportMap.Name = "txtTeleportMap";
            this.txtTeleportMap.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportMap.TabIndex = 16;
            this.txtTeleportMap.TextChanged += new System.EventHandler(this.txtTeleportMap_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Map:";
            // 
            // txtTeleportHeight
            // 
            this.txtTeleportHeight.Location = new System.Drawing.Point(179, 95);
            this.txtTeleportHeight.Name = "txtTeleportHeight";
            this.txtTeleportHeight.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportHeight.TabIndex = 14;
            this.txtTeleportHeight.TextChanged += new System.EventHandler(this.txtTeleportHeight_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(132, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Height:";
            // 
            // txtTeleportWidth
            // 
            this.txtTeleportWidth.Location = new System.Drawing.Point(51, 95);
            this.txtTeleportWidth.Name = "txtTeleportWidth";
            this.txtTeleportWidth.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportWidth.TabIndex = 12;
            this.txtTeleportWidth.TextChanged += new System.EventHandler(this.txtTeleportWidth_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Width:";
            // 
            // btnTeleportLocate
            // 
            this.btnTeleportLocate.Location = new System.Drawing.Point(163, 121);
            this.btnTeleportLocate.Name = "btnTeleportLocate";
            this.btnTeleportLocate.Size = new System.Drawing.Size(59, 24);
            this.btnTeleportLocate.TabIndex = 10;
            this.btnTeleportLocate.Text = "Locate";
            this.btnTeleportLocate.UseVisualStyleBackColor = true;
            this.btnTeleportLocate.Click += new System.EventHandler(this.btnTeleportLocate_Click);
            // 
            // btnTeleportDelete
            // 
            this.btnTeleportDelete.Location = new System.Drawing.Point(33, 121);
            this.btnTeleportDelete.Name = "btnTeleportDelete";
            this.btnTeleportDelete.Size = new System.Drawing.Size(59, 24);
            this.btnTeleportDelete.TabIndex = 9;
            this.btnTeleportDelete.Text = "Delete";
            this.btnTeleportDelete.UseVisualStyleBackColor = true;
            this.btnTeleportDelete.Click += new System.EventHandler(this.btnTeleportDelete_Click);
            // 
            // btnTeleportCopy
            // 
            this.btnTeleportCopy.Location = new System.Drawing.Point(98, 121);
            this.btnTeleportCopy.Name = "btnTeleportCopy";
            this.btnTeleportCopy.Size = new System.Drawing.Size(59, 24);
            this.btnTeleportCopy.TabIndex = 8;
            this.btnTeleportCopy.Text = "Copy";
            this.btnTeleportCopy.UseVisualStyleBackColor = true;
            this.btnTeleportCopy.Click += new System.EventHandler(this.btnTeleportCopy_Click);
            // 
            // txtTeleportToY
            // 
            this.txtTeleportToY.Location = new System.Drawing.Point(179, 68);
            this.txtTeleportToY.Name = "txtTeleportToY";
            this.txtTeleportToY.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportToY.TabIndex = 7;
            this.txtTeleportToY.TextChanged += new System.EventHandler(this.txtTeleportToY_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(143, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "ToY:";
            // 
            // txtTeleportToX
            // 
            this.txtTeleportToX.Location = new System.Drawing.Point(51, 68);
            this.txtTeleportToX.Name = "txtTeleportToX";
            this.txtTeleportToX.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportToX.TabIndex = 5;
            this.txtTeleportToX.TextChanged += new System.EventHandler(this.txtTeleportToX_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "ToX:";
            // 
            // txtTeleportY
            // 
            this.txtTeleportY.Location = new System.Drawing.Point(179, 42);
            this.txtTeleportY.Name = "txtTeleportY";
            this.txtTeleportY.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportY.TabIndex = 3;
            this.txtTeleportY.TextChanged += new System.EventHandler(this.txtTeleportY_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(156, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Y:";
            // 
            // txtTeleportX
            // 
            this.txtTeleportX.Location = new System.Drawing.Point(51, 42);
            this.txtTeleportX.Name = "txtTeleportX";
            this.txtTeleportX.Size = new System.Drawing.Size(59, 20);
            this.txtTeleportX.TabIndex = 1;
            this.txtTeleportX.TextChanged += new System.EventHandler(this.txtTeleportX_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "X:";
            // 
            // lstTeleports
            // 
            this.lstTeleports.FormattingEnabled = true;
            this.lstTeleports.Location = new System.Drawing.Point(6, 6);
            this.lstTeleports.Name = "lstTeleports";
            this.lstTeleports.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTeleports.Size = new System.Drawing.Size(276, 329);
            this.lstTeleports.TabIndex = 4;
            this.lstTeleports.SelectedIndexChanged += new System.EventHandler(this.lstTeleports_SelectedIndexChanged);
            // 
            // tabPageNPCs
            // 
            this.tabPageNPCs.Location = new System.Drawing.Point(4, 22);
            this.tabPageNPCs.Name = "tabPageNPCs";
            this.tabPageNPCs.Size = new System.Drawing.Size(296, 544);
            this.tabPageNPCs.TabIndex = 4;
            this.tabPageNPCs.Text = "NPCs";
            this.tabPageNPCs.ToolTipText = "Mob and NPC spawning and settings";
            this.tabPageNPCs.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(296, 544);
            this.tabPageSettings.TabIndex = 3;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.ToolTipText = "Map editor settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDrawEntities);
            this.groupBox3.Controls.Add(this.chkDrawAutoWalls);
            this.groupBox3.Controls.Add(this.chkShowGrhs);
            this.groupBox3.Controls.Add(this.chkShowWalls);
            this.groupBox3.Controls.Add(this.chkDrawGrid);
            this.groupBox3.Location = new System.Drawing.Point(3, 52);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 136);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display Options";
            // 
            // chkDrawEntities
            // 
            this.chkDrawEntities.AutoSize = true;
            this.chkDrawEntities.Location = new System.Drawing.Point(120, 21);
            this.chkDrawEntities.Name = "chkDrawEntities";
            this.chkDrawEntities.Size = new System.Drawing.Size(88, 17);
            this.chkDrawEntities.TabIndex = 13;
            this.chkDrawEntities.Text = "Draw Entities";
            this.chkDrawEntities.UseVisualStyleBackColor = true;
            this.chkDrawEntities.CheckedChanged += new System.EventHandler(this.chkDrawEntities_CheckedChanged);
            // 
            // chkDrawAutoWalls
            // 
            this.chkDrawAutoWalls.AutoSize = true;
            this.chkDrawAutoWalls.Location = new System.Drawing.Point(120, 44);
            this.chkDrawAutoWalls.Name = "chkDrawAutoWalls";
            this.chkDrawAutoWalls.Size = new System.Drawing.Size(105, 17);
            this.chkDrawAutoWalls.TabIndex = 12;
            this.chkDrawAutoWalls.Text = "Draw Auto-Walls";
            this.chkDrawAutoWalls.UseVisualStyleBackColor = true;
            // 
            // chkShowGrhs
            // 
            this.chkShowGrhs.AutoSize = true;
            this.chkShowGrhs.Checked = true;
            this.chkShowGrhs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowGrhs.Location = new System.Drawing.Point(6, 67);
            this.chkShowGrhs.Name = "chkShowGrhs";
            this.chkShowGrhs.Size = new System.Drawing.Size(76, 17);
            this.chkShowGrhs.TabIndex = 11;
            this.chkShowGrhs.Text = "Draw Grhs";
            this.chkShowGrhs.UseVisualStyleBackColor = true;
            this.chkShowGrhs.CheckedChanged += new System.EventHandler(this.chkShowGrhs_CheckedChanged);
            // 
            // chkShowWalls
            // 
            this.chkShowWalls.AutoSize = true;
            this.chkShowWalls.Location = new System.Drawing.Point(6, 44);
            this.chkShowWalls.Name = "chkShowWalls";
            this.chkShowWalls.Size = new System.Drawing.Size(80, 17);
            this.chkShowWalls.TabIndex = 9;
            this.chkShowWalls.Text = "Draw Walls";
            this.chkShowWalls.UseVisualStyleBackColor = true;
            this.chkShowWalls.CheckedChanged += new System.EventHandler(this.chkShowWalls_CheckedChanged);
            // 
            // chkDrawGrid
            // 
            this.chkDrawGrid.AutoSize = true;
            this.chkDrawGrid.Checked = true;
            this.chkDrawGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawGrid.Location = new System.Drawing.Point(6, 21);
            this.chkDrawGrid.Name = "chkDrawGrid";
            this.chkDrawGrid.Size = new System.Drawing.Size(73, 17);
            this.chkDrawGrid.TabIndex = 1;
            this.chkDrawGrid.Text = "Draw Grid";
            this.chkDrawGrid.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGridHeight);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtGridWidth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 40);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid";
            // 
            // txtGridHeight
            // 
            this.txtGridHeight.Location = new System.Drawing.Point(149, 13);
            this.txtGridHeight.Name = "txtGridHeight";
            this.txtGridHeight.Size = new System.Drawing.Size(46, 20);
            this.txtGridHeight.TabIndex = 4;
            this.txtGridHeight.TextChanged += new System.EventHandler(this.txtGridHeight_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(102, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Height:";
            // 
            // txtGridWidth
            // 
            this.txtGridWidth.Location = new System.Drawing.Point(50, 13);
            this.txtGridWidth.Name = "txtGridWidth";
            this.txtGridWidth.Size = new System.Drawing.Size(46, 20);
            this.txtGridWidth.TabIndex = 2;
            this.txtGridWidth.TextChanged += new System.EventHandler(this.txtGridWidth_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Width:";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(1002, 579);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(59, 24);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(937, 579);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(59, 24);
            this.cmdLoad.TabIndex = 3;
            this.cmdLoad.Text = "Load";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.Location = new System.Drawing.Point(872, 579);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(59, 24);
            this.cmdNew.TabIndex = 4;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdOptimize
            // 
            this.cmdOptimize.Location = new System.Drawing.Point(1067, 579);
            this.cmdOptimize.Name = "cmdOptimize";
            this.cmdOptimize.Size = new System.Drawing.Size(59, 24);
            this.cmdOptimize.TabIndex = 5;
            this.cmdOptimize.Text = "Optimize";
            this.cmdOptimize.UseVisualStyleBackColor = true;
            // 
            // panToolBar
            // 
            this.panToolBar.BackColor = System.Drawing.SystemColors.Window;
            this.panToolBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panToolBar.Controls.Add(this.picToolGrhsAdd);
            this.panToolBar.Controls.Add(this.picToolGrhs);
            this.panToolBar.Controls.Add(this.picToolWallsAdd);
            this.panToolBar.Controls.Add(this.picToolWalls);
            this.panToolBar.Controls.Add(this.picToolSelect);
            this.panToolBar.Location = new System.Drawing.Point(0, 0);
            this.panToolBar.Name = "panToolBar";
            this.panToolBar.Size = new System.Drawing.Size(30, 611);
            this.panToolBar.TabIndex = 6;
            // 
            // picToolGrhsAdd
            // 
            this.picToolGrhsAdd.Image = ((System.Drawing.Image)(resources.GetObject("picToolGrhsAdd.Image")));
            this.picToolGrhsAdd.InitialImage = null;
            this.picToolGrhsAdd.Location = new System.Drawing.Point(2, 120);
            this.picToolGrhsAdd.Name = "picToolGrhsAdd";
            this.picToolGrhsAdd.Size = new System.Drawing.Size(24, 24);
            this.picToolGrhsAdd.TabIndex = 4;
            this.picToolGrhsAdd.TabStop = false;
            this.picToolGrhsAdd.Click += new System.EventHandler(this.picToolGrhsAdd_Click);
            // 
            // picToolGrhs
            // 
            this.picToolGrhs.Image = ((System.Drawing.Image)(resources.GetObject("picToolGrhs.Image")));
            this.picToolGrhs.InitialImage = null;
            this.picToolGrhs.Location = new System.Drawing.Point(1, 90);
            this.picToolGrhs.Name = "picToolGrhs";
            this.picToolGrhs.Size = new System.Drawing.Size(24, 24);
            this.picToolGrhs.TabIndex = 3;
            this.picToolGrhs.TabStop = false;
            // 
            // picToolWallsAdd
            // 
            this.picToolWallsAdd.Image = ((System.Drawing.Image)(resources.GetObject("picToolWallsAdd.Image")));
            this.picToolWallsAdd.InitialImage = null;
            this.picToolWallsAdd.Location = new System.Drawing.Point(2, 60);
            this.picToolWallsAdd.Name = "picToolWallsAdd";
            this.picToolWallsAdd.Size = new System.Drawing.Size(24, 24);
            this.picToolWallsAdd.TabIndex = 2;
            this.picToolWallsAdd.TabStop = false;
            // 
            // picToolWalls
            // 
            this.picToolWalls.Image = ((System.Drawing.Image)(resources.GetObject("picToolWalls.Image")));
            this.picToolWalls.InitialImage = null;
            this.picToolWalls.Location = new System.Drawing.Point(2, 30);
            this.picToolWalls.Name = "picToolWalls";
            this.picToolWalls.Size = new System.Drawing.Size(24, 24);
            this.picToolWalls.TabIndex = 1;
            this.picToolWalls.TabStop = false;
            // 
            // picToolSelect
            // 
            this.picToolSelect.Image = ((System.Drawing.Image)(resources.GetObject("picToolSelect.Image")));
            this.picToolSelect.InitialImage = null;
            this.picToolSelect.Location = new System.Drawing.Point(2, 2);
            this.picToolSelect.Name = "picToolSelect";
            this.picToolSelect.Size = new System.Drawing.Size(24, 24);
            this.picToolSelect.TabIndex = 0;
            this.picToolSelect.TabStop = false;
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Location = new System.Drawing.Point(33, 3);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Screen = null;
            this.GameScreen.Size = new System.Drawing.Size(800, 600);
            this.GameScreen.TabIndex = 7;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseUp);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 606);
            this.Controls.Add(this.GameScreen);
            this.Controls.Add(this.panToolBar);
            this.Controls.Add(this.cmdOptimize);
            this.Controls.Add(this.cmdNew);
            this.Controls.Add(this.cmdLoad);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.tcMenu);
            this.KeyPreview = true;
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetGore Map Editor";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScreenForm_FormClosing);
            this.tcMenu.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.MapSizeGroupBox.ResumeLayout(false);
            this.MapSizeGroupBox.PerformLayout();
            this.tabPageGrhs.ResumeLayout(false);
            this.tabPageGrhs.PerformLayout();
            this.tabPageWalls.ResumeLayout(false);
            this.tabPageWalls.PerformLayout();
            this.gbCurrentWall.ResumeLayout(false);
            this.tabPageEnvironment.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTeleports.ResumeLayout(false);
            this.gbSelectedTeleporter.ResumeLayout(false);
            this.gbSelectedTeleporter.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panToolBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picToolGrhsAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolGrhs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolWallsAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolWalls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picToolSelect)).EndInit();
            this.ResumeLayout(false);

        }



        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageWalls;
        private System.Windows.Forms.TabPage tabPageEnvironment;
        private System.Windows.Forms.TabPage tabPageNPCs;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdLoad;
        private System.Windows.Forms.Button cmdNew;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.GroupBox MapSizeGroupBox;
        private System.Windows.Forms.Button cmdApplySize;
        private System.Windows.Forms.TextBox txtMapHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMapWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.Button cmdOptimize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtGridHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGridWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panToolBar;
        private System.Windows.Forms.TabPage tabPageGrhs;
        private GameScreenControl GameScreen;
        private System.Windows.Forms.GroupBox gbCurrentWall;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTeleports;
        private System.Windows.Forms.ListBox lstTeleports;
        private System.Windows.Forms.GroupBox gbSelectedTeleporter;
        private System.Windows.Forms.TextBox txtTeleportToY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTeleportToX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTeleportY;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTeleportX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTeleportLocate;
        private System.Windows.Forms.Button btnTeleportDelete;
        private System.Windows.Forms.Button btnTeleportCopy;
        private System.Windows.Forms.Button btnTeleportNew;
        private System.Windows.Forms.ComboBox cmbCurrWallType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkShowGrhs;
        private System.Windows.Forms.CheckBox chkShowWalls;
        private System.Windows.Forms.CheckBox chkDrawGrid;
        private System.Windows.Forms.CheckBox chkDrawAutoWalls;
        private System.Windows.Forms.CheckBox chkDrawEntities;
        public System.Windows.Forms.ListBox lstSelectedWalls;
        public System.Windows.Forms.CheckBox chkSnapWallGrid;
        public System.Windows.Forms.CheckBox chkSnapWallWall;
        public System.Windows.Forms.ComboBox cmbWallType;
        public System.Windows.Forms.PictureBox picToolWalls;
        public System.Windows.Forms.PictureBox picToolSelect;
        public System.Windows.Forms.PictureBox picToolWallsAdd;
        public System.Windows.Forms.PictureBox picToolGrhsAdd;
        public System.Windows.Forms.PictureBox picToolGrhs;
        public System.Windows.Forms.CheckBox chkSnapGrhGrid;
        public NetGore.EditorTools.GrhTreeView treeGrhs;
        public System.Windows.Forms.CheckBox chkForeground;
        private System.Windows.Forms.TextBox txtTeleportHeight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTeleportWidth;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTeleportMap;
        private System.Windows.Forms.Label label11;
    }
}