// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

using NetGore.EditorTools;

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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.SplitContainer splitContainer3;
            this.panToolBar = new System.Windows.Forms.Panel();
            this.GameScreen = new DemoGame.MapEditor.GameScreenControl();
            this.tcMenu = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.MapSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.cmdApplySize = new System.Windows.Forms.Button();
            this.txtMapHeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMapWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NameLabel = new System.Windows.Forms.Label();
            this.txtMusic = new System.Windows.Forms.TextBox();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageGrhs = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.chkForeground = new System.Windows.Forms.CheckBox();
            this.chkSnapGrhGrid = new System.Windows.Forms.CheckBox();
            this.treeGrhs = new NetGore.EditorTools.GrhTreeView();
            this.tabPageWalls = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.chkSnapWallGrid = new System.Windows.Forms.CheckBox();
            this.chkSnapWallWall = new System.Windows.Forms.CheckBox();
            this.cmbWallType = new System.Windows.Forms.ComboBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.lstSelectedWalls = new System.Windows.Forms.ListBox();
            this.pgWall = new System.Windows.Forms.PropertyGrid();
            this.tabPageEntities = new System.Windows.Forms.TabPage();
            this.cmbEntityTypes = new System.Windows.Forms.ComboBox();
            this.btnNewEntity = new System.Windows.Forms.Button();
            this.pgEntity = new System.Windows.Forms.PropertyGrid();
            this.lstEntities = new DemoGame.MapEditor.EntityListBox();
            this.tabPageBackground = new System.Windows.Forms.TabPage();
            this.pgBGItem = new System.Windows.Forms.PropertyGrid();
            this.btnNewBGSprite = new System.Windows.Forms.Button();
            this.btnNewBGLayer = new System.Windows.Forms.Button();
            this.btnDeleteBGItem = new System.Windows.Forms.Button();
            this.lstBGItems = new DemoGame.MapEditor.BackgroundItemListBox();
            this.tabEffects = new System.Windows.Forms.TabPage();
            this.mapParticleEffectsListBox1 = new DemoGame.MapEditor.MapParticleEffectsListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lstAvailableParticleEffects = new NetGore.EditorTools.ParticleEffectListBox();
            this.tabPageNPCs = new System.Windows.Forms.TabPage();
            this.tcSpawns = new System.Windows.Forms.TabControl();
            this.tpSpawns = new System.Windows.Forms.TabPage();
            this.btnAddSpawn = new System.Windows.Forms.Button();
            this.btnDeleteSpawn = new System.Windows.Forms.Button();
            this.lstNPCSpawns = new DemoGame.MapEditor.NPCSpawnsListBox();
            this.pgNPCSpawn = new System.Windows.Forms.PropertyGrid();
            this.tpPersistent = new System.Windows.Forms.TabPage();
            this.lstPersistentNPCs = new DemoGame.MapEditor.PersistentNPCListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDrawPersistentNPCs = new System.Windows.Forms.CheckBox();
            this.chkDrawSpawnAreas = new System.Windows.Forms.CheckBox();
            this.chkDrawBackground = new System.Windows.Forms.CheckBox();
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
            this.cmdOptimize = new System.Windows.Forms.Button();
            this.cmdSaveAs = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdLoad = new System.Windows.Forms.Button();
            this.cmdNew = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            this.tcMenu.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.MapSizeGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPageGrhs.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tabPageWalls.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.tabPageEntities.SuspendLayout();
            this.tabPageBackground.SuspendLayout();
            this.tabEffects.SuspendLayout();
            this.tabPageNPCs.SuspendLayout();
            this.tcSpawns.SuspendLayout();
            this.tpSpawns.SuspendLayout();
            this.tpPersistent.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(5, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1MinSize = 800;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer3);
            splitContainer1.Size = new System.Drawing.Size(1197, 631);
            splitContainer1.SplitterDistance = 800;
            splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.panToolBar);
            splitContainer2.Panel1MinSize = 26;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.GameScreen);
            splitContainer2.Size = new System.Drawing.Size(800, 631);
            splitContainer2.SplitterDistance = 26;
            splitContainer2.TabIndex = 0;
            // 
            // panToolBar
            // 
            this.panToolBar.BackColor = System.Drawing.SystemColors.Window;
            this.panToolBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panToolBar.Location = new System.Drawing.Point(0, 0);
            this.panToolBar.Name = "panToolBar";
            this.panToolBar.Size = new System.Drawing.Size(800, 26);
            this.panToolBar.TabIndex = 7;
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Location = new System.Drawing.Point(0, 0);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Padding = new System.Windows.Forms.Padding(5);
            this.GameScreen.ScreenForm = null;
            this.GameScreen.Size = new System.Drawing.Size(800, 600);
            this.GameScreen.TabIndex = 8;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseUp);
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer3.IsSplitterFixed = true;
            splitContainer3.Location = new System.Drawing.Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(this.tcMenu);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(this.cmdOptimize);
            splitContainer3.Panel2.Controls.Add(this.cmdSaveAs);
            splitContainer3.Panel2.Controls.Add(this.cmdSave);
            splitContainer3.Panel2.Controls.Add(this.cmdLoad);
            splitContainer3.Panel2.Controls.Add(this.cmdNew);
            splitContainer3.Size = new System.Drawing.Size(393, 631);
            splitContainer3.SplitterDistance = 599;
            splitContainer3.TabIndex = 0;
            // 
            // tcMenu
            // 
            this.tcMenu.Controls.Add(this.tabPageGeneral);
            this.tcMenu.Controls.Add(this.tabPageGrhs);
            this.tcMenu.Controls.Add(this.tabPageWalls);
            this.tcMenu.Controls.Add(this.tabPageEntities);
            this.tcMenu.Controls.Add(this.tabPageBackground);
            this.tcMenu.Controls.Add(this.tabEffects);
            this.tcMenu.Controls.Add(this.tabPageNPCs);
            this.tcMenu.Controls.Add(this.tabPageSettings);
            this.tcMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMenu.Location = new System.Drawing.Point(0, 0);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(393, 599);
            this.tcMenu.TabIndex = 2;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.MapSizeGroupBox);
            this.tabPageGeneral.Controls.Add(this.tableLayoutPanel1);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(385, 573);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.ToolTipText = "General map information";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // MapSizeGroupBox
            // 
            this.MapSizeGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MapSizeGroupBox.Controls.Add(this.cmdApplySize);
            this.MapSizeGroupBox.Controls.Add(this.txtMapHeight);
            this.MapSizeGroupBox.Controls.Add(this.label2);
            this.MapSizeGroupBox.Controls.Add(this.txtMapWidth);
            this.MapSizeGroupBox.Controls.Add(this.label1);
            this.MapSizeGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.MapSizeGroupBox.Location = new System.Drawing.Point(3, 55);
            this.MapSizeGroupBox.Name = "MapSizeGroupBox";
            this.MapSizeGroupBox.Size = new System.Drawing.Size(379, 49);
            this.MapSizeGroupBox.TabIndex = 7;
            this.MapSizeGroupBox.TabStop = false;
            this.MapSizeGroupBox.Text = "Map Size";
            // 
            // cmdApplySize
            // 
            this.cmdApplySize.Location = new System.Drawing.Point(216, 16);
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtMusic, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtMapName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 52);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NameLabel.Location = new System.Drawing.Point(3, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(64, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Map Name:";
            // 
            // txtMusic
            // 
            this.txtMusic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMusic.Location = new System.Drawing.Point(73, 28);
            this.txtMusic.Name = "txtMusic";
            this.txtMusic.Size = new System.Drawing.Size(303, 20);
            this.txtMusic.TabIndex = 5;
            this.txtMusic.TextChanged += new System.EventHandler(this.txtMusic_TextChanged);
            // 
            // txtMapName
            // 
            this.txtMapName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMapName.Location = new System.Drawing.Point(73, 3);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(303, 20);
            this.txtMapName.TabIndex = 1;
            this.txtMapName.TextChanged += new System.EventHandler(this.txtMapName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(3, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Music:";
            // 
            // tabPageGrhs
            // 
            this.tabPageGrhs.Controls.Add(this.splitContainer4);
            this.tabPageGrhs.Location = new System.Drawing.Point(4, 22);
            this.tabPageGrhs.Name = "tabPageGrhs";
            this.tabPageGrhs.Size = new System.Drawing.Size(385, 573);
            this.tabPageGrhs.TabIndex = 5;
            this.tabPageGrhs.Text = "Grhs";
            this.tabPageGrhs.UseVisualStyleBackColor = true;
            this.tabPageGrhs.Enter += new System.EventHandler(this.tabPageGrhs_Enter);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.chkForeground);
            this.splitContainer4.Panel1.Controls.Add(this.chkSnapGrhGrid);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.treeGrhs);
            this.splitContainer4.Size = new System.Drawing.Size(385, 573);
            this.splitContainer4.SplitterDistance = 25;
            this.splitContainer4.TabIndex = 10;
            // 
            // chkForeground
            // 
            this.chkForeground.AutoSize = true;
            this.chkForeground.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkForeground.Location = new System.Drawing.Point(89, 0);
            this.chkForeground.Name = "chkForeground";
            this.chkForeground.Size = new System.Drawing.Size(80, 25);
            this.chkForeground.TabIndex = 11;
            this.chkForeground.Text = "Foreground";
            this.chkForeground.UseVisualStyleBackColor = true;
            // 
            // chkSnapGrhGrid
            // 
            this.chkSnapGrhGrid.AutoSize = true;
            this.chkSnapGrhGrid.Checked = true;
            this.chkSnapGrhGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapGrhGrid.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSnapGrhGrid.Location = new System.Drawing.Point(0, 0);
            this.chkSnapGrhGrid.Name = "chkSnapGrhGrid";
            this.chkSnapGrhGrid.Size = new System.Drawing.Size(89, 25);
            this.chkSnapGrhGrid.TabIndex = 10;
            this.chkSnapGrhGrid.Text = "Snap To Grid";
            this.chkSnapGrhGrid.UseVisualStyleBackColor = true;
            // 
            // treeGrhs
            // 
            this.treeGrhs.AllowDrop = true;
            this.treeGrhs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGrhs.ImageSize = new System.Drawing.Size(0, 0);
            this.treeGrhs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeGrhs.LabelEdit = true;
            this.treeGrhs.Location = new System.Drawing.Point(0, 0);
            this.treeGrhs.Name = "treeGrhs";
            this.treeGrhs.ShowNodeToolTips = true;
            this.treeGrhs.Size = new System.Drawing.Size(385, 544);
            this.treeGrhs.Sorted = true;
            this.treeGrhs.TabIndex = 9;
            this.treeGrhs.GrhAfterSelect += new NetGore.EditorTools.GrhTreeViewEvent(this.treeGrhs_GrhAfterSelect);
            // 
            // tabPageWalls
            // 
            this.tabPageWalls.Controls.Add(this.splitContainer5);
            this.tabPageWalls.Location = new System.Drawing.Point(4, 22);
            this.tabPageWalls.Name = "tabPageWalls";
            this.tabPageWalls.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWalls.Size = new System.Drawing.Size(385, 573);
            this.tabPageWalls.TabIndex = 1;
            this.tabPageWalls.Text = "Walls";
            this.tabPageWalls.ToolTipText = "Collision walls";
            this.tabPageWalls.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer5.Size = new System.Drawing.Size(379, 567);
            this.splitContainer5.SplitterDistance = 51;
            this.splitContainer5.TabIndex = 9;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.chkSnapWallGrid);
            this.splitContainer6.Panel1.Controls.Add(this.chkSnapWallWall);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.cmbWallType);
            this.splitContainer6.Size = new System.Drawing.Size(379, 51);
            this.splitContainer6.SplitterDistance = 25;
            this.splitContainer6.TabIndex = 0;
            // 
            // chkSnapWallGrid
            // 
            this.chkSnapWallGrid.AutoSize = true;
            this.chkSnapWallGrid.Checked = true;
            this.chkSnapWallGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapWallGrid.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSnapWallGrid.Location = new System.Drawing.Point(96, 0);
            this.chkSnapWallGrid.Name = "chkSnapWallGrid";
            this.chkSnapWallGrid.Size = new System.Drawing.Size(89, 25);
            this.chkSnapWallGrid.TabIndex = 7;
            this.chkSnapWallGrid.Text = "Snap To Grid";
            this.chkSnapWallGrid.UseVisualStyleBackColor = true;
            // 
            // chkSnapWallWall
            // 
            this.chkSnapWallWall.AutoSize = true;
            this.chkSnapWallWall.Checked = true;
            this.chkSnapWallWall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnapWallWall.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkSnapWallWall.Location = new System.Drawing.Point(0, 0);
            this.chkSnapWallWall.Name = "chkSnapWallWall";
            this.chkSnapWallWall.Size = new System.Drawing.Size(96, 25);
            this.chkSnapWallWall.TabIndex = 6;
            this.chkSnapWallWall.Text = "Snap To Walls";
            this.chkSnapWallWall.UseVisualStyleBackColor = true;
            // 
            // cmbWallType
            // 
            this.cmbWallType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbWallType.FormattingEnabled = true;
            this.cmbWallType.Location = new System.Drawing.Point(0, 0);
            this.cmbWallType.Name = "cmbWallType";
            this.cmbWallType.Size = new System.Drawing.Size(379, 21);
            this.cmbWallType.TabIndex = 8;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.lstSelectedWalls);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.pgWall);
            this.splitContainer7.Size = new System.Drawing.Size(379, 512);
            this.splitContainer7.SplitterDistance = 200;
            this.splitContainer7.TabIndex = 0;
            // 
            // lstSelectedWalls
            // 
            this.lstSelectedWalls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelectedWalls.FormattingEnabled = true;
            this.lstSelectedWalls.Location = new System.Drawing.Point(0, 0);
            this.lstSelectedWalls.Name = "lstSelectedWalls";
            this.lstSelectedWalls.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelectedWalls.Size = new System.Drawing.Size(379, 199);
            this.lstSelectedWalls.Sorted = true;
            this.lstSelectedWalls.TabIndex = 4;
            this.lstSelectedWalls.SelectedIndexChanged += new System.EventHandler(this.lstSelectedWalls_SelectedIndexChanged);
            this.lstSelectedWalls.SelectedValueChanged += new System.EventHandler(this.lstSelectedWalls_SelectedValueChanged);
            // 
            // pgWall
            // 
            this.pgWall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgWall.HelpVisible = false;
            this.pgWall.Location = new System.Drawing.Point(0, 0);
            this.pgWall.Name = "pgWall";
            this.pgWall.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.pgWall.Size = new System.Drawing.Size(379, 308);
            this.pgWall.TabIndex = 9;
            this.pgWall.ToolbarVisible = false;
            // 
            // tabPageEntities
            // 
            this.tabPageEntities.Controls.Add(this.cmbEntityTypes);
            this.tabPageEntities.Controls.Add(this.btnNewEntity);
            this.tabPageEntities.Controls.Add(this.pgEntity);
            this.tabPageEntities.Controls.Add(this.lstEntities);
            this.tabPageEntities.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntities.Name = "tabPageEntities";
            this.tabPageEntities.Size = new System.Drawing.Size(385, 573);
            this.tabPageEntities.TabIndex = 7;
            this.tabPageEntities.Text = "Entities";
            this.tabPageEntities.UseVisualStyleBackColor = true;
            // 
            // cmbEntityTypes
            // 
            this.cmbEntityTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEntityTypes.FormattingEnabled = true;
            this.cmbEntityTypes.Location = new System.Drawing.Point(3, 195);
            this.cmbEntityTypes.Name = "cmbEntityTypes";
            this.cmbEntityTypes.Size = new System.Drawing.Size(308, 21);
            this.cmbEntityTypes.TabIndex = 11;
            // 
            // btnNewEntity
            // 
            this.btnNewEntity.Location = new System.Drawing.Point(317, 192);
            this.btnNewEntity.Name = "btnNewEntity";
            this.btnNewEntity.Size = new System.Drawing.Size(59, 24);
            this.btnNewEntity.TabIndex = 10;
            this.btnNewEntity.Text = "New";
            this.btnNewEntity.UseVisualStyleBackColor = true;
            this.btnNewEntity.Click += new System.EventHandler(this.btnNewEntity_Click);
            // 
            // pgEntity
            // 
            this.pgEntity.Location = new System.Drawing.Point(3, 222);
            this.pgEntity.Name = "pgEntity";
            this.pgEntity.Size = new System.Drawing.Size(373, 348);
            this.pgEntity.TabIndex = 1;
            this.pgEntity.ToolbarVisible = false;
            // 
            // lstEntities
            // 
            this.lstEntities.Camera = null;
            this.lstEntities.FormattingEnabled = true;
            this.lstEntities.Location = new System.Drawing.Point(3, 3);
            this.lstEntities.Map = null;
            this.lstEntities.Name = "lstEntities";
            this.lstEntities.Size = new System.Drawing.Size(373, 186);
            this.lstEntities.TabIndex = 0;
            this.lstEntities.SelectedIndexChanged += new System.EventHandler(this.lstEntities_SelectedIndexChanged);
            // 
            // tabPageBackground
            // 
            this.tabPageBackground.Controls.Add(this.pgBGItem);
            this.tabPageBackground.Controls.Add(this.btnNewBGSprite);
            this.tabPageBackground.Controls.Add(this.btnNewBGLayer);
            this.tabPageBackground.Controls.Add(this.btnDeleteBGItem);
            this.tabPageBackground.Controls.Add(this.lstBGItems);
            this.tabPageBackground.Location = new System.Drawing.Point(4, 22);
            this.tabPageBackground.Name = "tabPageBackground";
            this.tabPageBackground.Size = new System.Drawing.Size(385, 573);
            this.tabPageBackground.TabIndex = 6;
            this.tabPageBackground.Text = "Background";
            this.tabPageBackground.UseVisualStyleBackColor = true;
            // 
            // pgBGItem
            // 
            this.pgBGItem.Location = new System.Drawing.Point(3, 173);
            this.pgBGItem.Name = "pgBGItem";
            this.pgBGItem.Size = new System.Drawing.Size(373, 397);
            this.pgBGItem.TabIndex = 5;
            this.pgBGItem.ToolbarVisible = false;
            // 
            // btnNewBGSprite
            // 
            this.btnNewBGSprite.Location = new System.Drawing.Point(163, 143);
            this.btnNewBGSprite.Name = "btnNewBGSprite";
            this.btnNewBGSprite.Size = new System.Drawing.Size(74, 24);
            this.btnNewBGSprite.TabIndex = 4;
            this.btnNewBGSprite.Text = "New Sprite";
            this.btnNewBGSprite.UseVisualStyleBackColor = true;
            this.btnNewBGSprite.Click += new System.EventHandler(this.btnNewBGSprite_Click);
            // 
            // btnNewBGLayer
            // 
            this.btnNewBGLayer.Location = new System.Drawing.Point(83, 143);
            this.btnNewBGLayer.Name = "btnNewBGLayer";
            this.btnNewBGLayer.Size = new System.Drawing.Size(74, 24);
            this.btnNewBGLayer.TabIndex = 3;
            this.btnNewBGLayer.Text = "New Layer";
            this.btnNewBGLayer.UseVisualStyleBackColor = true;
            this.btnNewBGLayer.Click += new System.EventHandler(this.btnNewBGLayer_Click);
            // 
            // btnDeleteBGItem
            // 
            this.btnDeleteBGItem.Location = new System.Drawing.Point(3, 143);
            this.btnDeleteBGItem.Name = "btnDeleteBGItem";
            this.btnDeleteBGItem.Size = new System.Drawing.Size(74, 24);
            this.btnDeleteBGItem.TabIndex = 2;
            this.btnDeleteBGItem.Text = "Delete";
            this.btnDeleteBGItem.UseVisualStyleBackColor = true;
            this.btnDeleteBGItem.Click += new System.EventHandler(this.btnDeleteBGItem_Click);
            // 
            // lstBGItems
            // 
            this.lstBGItems.Camera = null;
            this.lstBGItems.FormattingEnabled = true;
            this.lstBGItems.Location = new System.Drawing.Point(3, 3);
            this.lstBGItems.Map = null;
            this.lstBGItems.Name = "lstBGItems";
            this.lstBGItems.Size = new System.Drawing.Size(373, 134);
            this.lstBGItems.TabIndex = 0;
            this.lstBGItems.SelectedIndexChanged += new System.EventHandler(this.lstBGItems_SelectedIndexChanged);
            // 
            // tabEffects
            // 
            this.tabEffects.Controls.Add(this.mapParticleEffectsListBox1);
            this.tabEffects.Controls.Add(this.label7);
            this.tabEffects.Controls.Add(this.label6);
            this.tabEffects.Controls.Add(this.lstAvailableParticleEffects);
            this.tabEffects.Location = new System.Drawing.Point(4, 22);
            this.tabEffects.Name = "tabEffects";
            this.tabEffects.Size = new System.Drawing.Size(385, 573);
            this.tabEffects.TabIndex = 8;
            this.tabEffects.Text = "Effects";
            this.tabEffects.UseVisualStyleBackColor = true;
            // 
            // mapParticleEffectsListBox1
            // 
            this.mapParticleEffectsListBox1.Camera = null;
            this.mapParticleEffectsListBox1.FormattingEnabled = true;
            this.mapParticleEffectsListBox1.Location = new System.Drawing.Point(3, 354);
            this.mapParticleEffectsListBox1.Map = null;
            this.mapParticleEffectsListBox1.Name = "mapParticleEffectsListBox1";
            this.mapParticleEffectsListBox1.Size = new System.Drawing.Size(373, 212);
            this.mapParticleEffectsListBox1.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 338);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Particle Effects On Map:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Available Particle Effects:";
            // 
            // lstAvailableParticleEffects
            // 
            this.lstAvailableParticleEffects.FormattingEnabled = true;
            this.lstAvailableParticleEffects.Location = new System.Drawing.Point(3, 29);
            this.lstAvailableParticleEffects.Name = "lstAvailableParticleEffects";
            this.lstAvailableParticleEffects.Size = new System.Drawing.Size(373, 303);
            this.lstAvailableParticleEffects.TabIndex = 0;
            this.lstAvailableParticleEffects.RequestCreateEffect += new NetGore.EditorTools.ParticleEffectListBoxCreateEventHandler(this.lstAvailableParticleEffects_RequestCreateEffect);
            // 
            // tabPageNPCs
            // 
            this.tabPageNPCs.Controls.Add(this.tcSpawns);
            this.tabPageNPCs.Controls.Add(this.pgNPCSpawn);
            this.tabPageNPCs.Location = new System.Drawing.Point(4, 22);
            this.tabPageNPCs.Name = "tabPageNPCs";
            this.tabPageNPCs.Size = new System.Drawing.Size(385, 573);
            this.tabPageNPCs.TabIndex = 4;
            this.tabPageNPCs.Text = "NPCs";
            this.tabPageNPCs.ToolTipText = "Mob and NPC spawning and settings";
            this.tabPageNPCs.UseVisualStyleBackColor = true;
            // 
            // tcSpawns
            // 
            this.tcSpawns.Controls.Add(this.tpSpawns);
            this.tcSpawns.Controls.Add(this.tpPersistent);
            this.tcSpawns.Location = new System.Drawing.Point(3, 3);
            this.tcSpawns.Name = "tcSpawns";
            this.tcSpawns.SelectedIndex = 0;
            this.tcSpawns.Size = new System.Drawing.Size(375, 200);
            this.tcSpawns.TabIndex = 7;
            // 
            // tpSpawns
            // 
            this.tpSpawns.Controls.Add(this.btnAddSpawn);
            this.tpSpawns.Controls.Add(this.btnDeleteSpawn);
            this.tpSpawns.Controls.Add(this.lstNPCSpawns);
            this.tpSpawns.Location = new System.Drawing.Point(4, 22);
            this.tpSpawns.Name = "tpSpawns";
            this.tpSpawns.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpawns.Size = new System.Drawing.Size(367, 174);
            this.tpSpawns.TabIndex = 0;
            this.tpSpawns.Text = "Spawns";
            this.tpSpawns.UseVisualStyleBackColor = true;
            // 
            // btnAddSpawn
            // 
            this.btnAddSpawn.Location = new System.Drawing.Point(285, 147);
            this.btnAddSpawn.Name = "btnAddSpawn";
            this.btnAddSpawn.Size = new System.Drawing.Size(74, 24);
            this.btnAddSpawn.TabIndex = 7;
            this.btnAddSpawn.Text = "Add";
            this.btnAddSpawn.UseVisualStyleBackColor = true;
            this.btnAddSpawn.Click += new System.EventHandler(this.btnAddSpawn_Click);
            // 
            // btnDeleteSpawn
            // 
            this.btnDeleteSpawn.Location = new System.Drawing.Point(205, 147);
            this.btnDeleteSpawn.Name = "btnDeleteSpawn";
            this.btnDeleteSpawn.Size = new System.Drawing.Size(74, 24);
            this.btnDeleteSpawn.TabIndex = 6;
            this.btnDeleteSpawn.Text = "Delete";
            this.btnDeleteSpawn.UseVisualStyleBackColor = true;
            this.btnDeleteSpawn.Click += new System.EventHandler(this.btnDeleteSpawn_Click);
            // 
            // lstNPCSpawns
            // 
            this.lstNPCSpawns.FormattingEnabled = true;
            this.lstNPCSpawns.Location = new System.Drawing.Point(6, 6);
            this.lstNPCSpawns.Name = "lstNPCSpawns";
            this.lstNPCSpawns.PropertyGrid = this.pgNPCSpawn;
            this.lstNPCSpawns.Size = new System.Drawing.Size(353, 134);
            this.lstNPCSpawns.TabIndex = 5;
            // 
            // pgNPCSpawn
            // 
            this.pgNPCSpawn.Location = new System.Drawing.Point(3, 209);
            this.pgNPCSpawn.Name = "pgNPCSpawn";
            this.pgNPCSpawn.Size = new System.Drawing.Size(371, 361);
            this.pgNPCSpawn.TabIndex = 6;
            this.pgNPCSpawn.ToolbarVisible = false;
            // 
            // tpPersistent
            // 
            this.tpPersistent.Controls.Add(this.lstPersistentNPCs);
            this.tpPersistent.Controls.Add(this.button1);
            this.tpPersistent.Controls.Add(this.button2);
            this.tpPersistent.Location = new System.Drawing.Point(4, 22);
            this.tpPersistent.Name = "tpPersistent";
            this.tpPersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tpPersistent.Size = new System.Drawing.Size(367, 174);
            this.tpPersistent.TabIndex = 1;
            this.tpPersistent.Text = "Persistent";
            this.tpPersistent.UseVisualStyleBackColor = true;
            // 
            // lstPersistentNPCs
            // 
            this.lstPersistentNPCs.FormattingEnabled = true;
            this.lstPersistentNPCs.Location = new System.Drawing.Point(6, 6);
            this.lstPersistentNPCs.Name = "lstPersistentNPCs";
            this.lstPersistentNPCs.PropertyGrid = this.pgNPCSpawn;
            this.lstPersistentNPCs.Size = new System.Drawing.Size(311, 134);
            this.lstPersistentNPCs.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(243, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 24);
            this.button1.TabIndex = 10;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(163, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(74, 24);
            this.button2.TabIndex = 9;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(385, 573);
            this.tabPageSettings.TabIndex = 3;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.ToolTipText = "Map editor settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDrawPersistentNPCs);
            this.groupBox3.Controls.Add(this.chkDrawSpawnAreas);
            this.groupBox3.Controls.Add(this.chkDrawBackground);
            this.groupBox3.Controls.Add(this.chkDrawEntities);
            this.groupBox3.Controls.Add(this.chkDrawAutoWalls);
            this.groupBox3.Controls.Add(this.chkShowGrhs);
            this.groupBox3.Controls.Add(this.chkShowWalls);
            this.groupBox3.Controls.Add(this.chkDrawGrid);
            this.groupBox3.Location = new System.Drawing.Point(3, 52);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(221, 116);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display Options";
            // 
            // chkDrawPersistentNPCs
            // 
            this.chkDrawPersistentNPCs.AutoSize = true;
            this.chkDrawPersistentNPCs.Location = new System.Drawing.Point(120, 90);
            this.chkDrawPersistentNPCs.Name = "chkDrawPersistentNPCs";
            this.chkDrawPersistentNPCs.Size = new System.Drawing.Size(102, 17);
            this.chkDrawPersistentNPCs.TabIndex = 16;
            this.chkDrawPersistentNPCs.Text = "Persistent NPCs";
            this.chkDrawPersistentNPCs.UseVisualStyleBackColor = true;
            // 
            // chkDrawSpawnAreas
            // 
            this.chkDrawSpawnAreas.AutoSize = true;
            this.chkDrawSpawnAreas.Location = new System.Drawing.Point(6, 90);
            this.chkDrawSpawnAreas.Name = "chkDrawSpawnAreas";
            this.chkDrawSpawnAreas.Size = new System.Drawing.Size(89, 17);
            this.chkDrawSpawnAreas.TabIndex = 15;
            this.chkDrawSpawnAreas.Text = "Spawn Areas";
            this.chkDrawSpawnAreas.UseVisualStyleBackColor = true;
            // 
            // chkDrawBackground
            // 
            this.chkDrawBackground.AutoSize = true;
            this.chkDrawBackground.Checked = true;
            this.chkDrawBackground.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawBackground.Location = new System.Drawing.Point(120, 67);
            this.chkDrawBackground.Name = "chkDrawBackground";
            this.chkDrawBackground.Size = new System.Drawing.Size(84, 17);
            this.chkDrawBackground.TabIndex = 14;
            this.chkDrawBackground.Text = "Background";
            this.chkDrawBackground.UseVisualStyleBackColor = true;
            this.chkDrawBackground.CheckedChanged += new System.EventHandler(this.chkDrawBackground_CheckedChanged);
            // 
            // chkDrawEntities
            // 
            this.chkDrawEntities.AutoSize = true;
            this.chkDrawEntities.Location = new System.Drawing.Point(120, 21);
            this.chkDrawEntities.Name = "chkDrawEntities";
            this.chkDrawEntities.Size = new System.Drawing.Size(60, 17);
            this.chkDrawEntities.TabIndex = 13;
            this.chkDrawEntities.Text = "Entities";
            this.chkDrawEntities.UseVisualStyleBackColor = true;
            // 
            // chkDrawAutoWalls
            // 
            this.chkDrawAutoWalls.AutoSize = true;
            this.chkDrawAutoWalls.Location = new System.Drawing.Point(120, 44);
            this.chkDrawAutoWalls.Name = "chkDrawAutoWalls";
            this.chkDrawAutoWalls.Size = new System.Drawing.Size(77, 17);
            this.chkDrawAutoWalls.TabIndex = 12;
            this.chkDrawAutoWalls.Text = "Auto-Walls";
            this.chkDrawAutoWalls.UseVisualStyleBackColor = true;
            // 
            // chkShowGrhs
            // 
            this.chkShowGrhs.AutoSize = true;
            this.chkShowGrhs.Checked = true;
            this.chkShowGrhs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowGrhs.Location = new System.Drawing.Point(6, 67);
            this.chkShowGrhs.Name = "chkShowGrhs";
            this.chkShowGrhs.Size = new System.Drawing.Size(48, 17);
            this.chkShowGrhs.TabIndex = 11;
            this.chkShowGrhs.Text = "Grhs";
            this.chkShowGrhs.UseVisualStyleBackColor = true;
            this.chkShowGrhs.CheckedChanged += new System.EventHandler(this.chkShowGrhs_CheckedChanged);
            // 
            // chkShowWalls
            // 
            this.chkShowWalls.AutoSize = true;
            this.chkShowWalls.Location = new System.Drawing.Point(6, 44);
            this.chkShowWalls.Name = "chkShowWalls";
            this.chkShowWalls.Size = new System.Drawing.Size(52, 17);
            this.chkShowWalls.TabIndex = 9;
            this.chkShowWalls.Text = "Walls";
            this.chkShowWalls.UseVisualStyleBackColor = true;
            // 
            // chkDrawGrid
            // 
            this.chkDrawGrid.AutoSize = true;
            this.chkDrawGrid.Location = new System.Drawing.Point(6, 21);
            this.chkDrawGrid.Name = "chkDrawGrid";
            this.chkDrawGrid.Size = new System.Drawing.Size(45, 17);
            this.chkDrawGrid.TabIndex = 1;
            this.chkDrawGrid.Text = "Grid";
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
            this.groupBox1.Size = new System.Drawing.Size(204, 40);
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
            // cmdOptimize
            // 
            this.cmdOptimize.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmdOptimize.Location = new System.Drawing.Point(260, 0);
            this.cmdOptimize.Name = "cmdOptimize";
            this.cmdOptimize.Size = new System.Drawing.Size(65, 28);
            this.cmdOptimize.TabIndex = 10;
            this.cmdOptimize.Text = "Optimize";
            this.cmdOptimize.UseVisualStyleBackColor = true;
            // 
            // cmdSaveAs
            // 
            this.cmdSaveAs.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmdSaveAs.Location = new System.Drawing.Point(195, 0);
            this.cmdSaveAs.Name = "cmdSaveAs";
            this.cmdSaveAs.Size = new System.Drawing.Size(65, 28);
            this.cmdSaveAs.TabIndex = 9;
            this.cmdSaveAs.Text = "Save As";
            this.cmdSaveAs.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmdSave.Location = new System.Drawing.Point(130, 0);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(65, 28);
            this.cmdSave.TabIndex = 7;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdLoad
            // 
            this.cmdLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmdLoad.Location = new System.Drawing.Point(65, 0);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(65, 28);
            this.cmdLoad.TabIndex = 6;
            this.cmdLoad.Text = "Load";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmdNew.Location = new System.Drawing.Point(0, 0);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(65, 28);
            this.cmdNew.TabIndex = 5;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 641);
            this.Controls.Add(splitContainer1);
            this.KeyPreview = true;
            this.Name = "ScreenForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetGore Map Editor";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScreenForm_FormClosing);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.ResumeLayout(false);
            this.tcMenu.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.MapSizeGroupBox.ResumeLayout(false);
            this.MapSizeGroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPageGrhs.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.tabPageWalls.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.ResumeLayout(false);
            this.tabPageEntities.ResumeLayout(false);
            this.tabPageBackground.ResumeLayout(false);
            this.tabEffects.ResumeLayout(false);
            this.tabEffects.PerformLayout();
            this.tabPageNPCs.ResumeLayout(false);
            this.tcSpawns.ResumeLayout(false);
            this.tpSpawns.ResumeLayout(false);
            this.tpPersistent.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panToolBar;
        private GameScreenControl GameScreen;
        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TextBox txtMusic;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPageGrhs;
        private System.Windows.Forms.TabPage tabPageWalls;
        private System.Windows.Forms.TabPage tabPageEntities;
        private System.Windows.Forms.ComboBox cmbEntityTypes;
        private System.Windows.Forms.Button btnNewEntity;
        private System.Windows.Forms.PropertyGrid pgEntity;
        private EntityListBox lstEntities;
        private System.Windows.Forms.TabPage tabPageBackground;
        private System.Windows.Forms.PropertyGrid pgBGItem;
        private System.Windows.Forms.Button btnNewBGSprite;
        private System.Windows.Forms.Button btnNewBGLayer;
        private System.Windows.Forms.Button btnDeleteBGItem;
        private BackgroundItemListBox lstBGItems;
        private System.Windows.Forms.TabPage tabEffects;
        private MapParticleEffectsListBox mapParticleEffectsListBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private ParticleEffectListBox lstAvailableParticleEffects;
        private System.Windows.Forms.TabPage tabPageNPCs;
        private System.Windows.Forms.TabControl tcSpawns;
        private System.Windows.Forms.TabPage tpSpawns;
        private System.Windows.Forms.Button btnAddSpawn;
        private System.Windows.Forms.Button btnDeleteSpawn;
        private NPCSpawnsListBox lstNPCSpawns;
        private System.Windows.Forms.PropertyGrid pgNPCSpawn;
        private System.Windows.Forms.TabPage tpPersistent;
        private PersistentNPCListBox lstPersistentNPCs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDrawPersistentNPCs;
        private System.Windows.Forms.CheckBox chkDrawSpawnAreas;
        private System.Windows.Forms.CheckBox chkDrawBackground;
        private System.Windows.Forms.CheckBox chkDrawEntities;
        private System.Windows.Forms.CheckBox chkDrawAutoWalls;
        private System.Windows.Forms.CheckBox chkShowGrhs;
        private System.Windows.Forms.CheckBox chkShowWalls;
        private System.Windows.Forms.CheckBox chkDrawGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtGridHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGridWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox MapSizeGroupBox;
        private System.Windows.Forms.Button cmdApplySize;
        private System.Windows.Forms.TextBox txtMapHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMapWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdOptimize;
        private System.Windows.Forms.Button cmdSaveAs;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdLoad;
        private System.Windows.Forms.Button cmdNew;
        private System.Windows.Forms.SplitContainer splitContainer4;
        public System.Windows.Forms.CheckBox chkForeground;
        public System.Windows.Forms.CheckBox chkSnapGrhGrid;
        public GrhTreeView treeGrhs;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        public System.Windows.Forms.CheckBox chkSnapWallGrid;
        public System.Windows.Forms.CheckBox chkSnapWallWall;
        private System.Windows.Forms.SplitContainer splitContainer7;
        public System.Windows.Forms.ListBox lstSelectedWalls;
        private System.Windows.Forms.PropertyGrid pgWall;
        public System.Windows.Forms.ComboBox cmbWallType;
    }
}