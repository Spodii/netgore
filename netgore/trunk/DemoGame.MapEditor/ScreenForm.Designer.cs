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
            this.pgWall = new System.Windows.Forms.PropertyGrid();
            this.cmbWallType = new System.Windows.Forms.ComboBox();
            this.chkSnapWallGrid = new System.Windows.Forms.CheckBox();
            this.chkSnapWallWall = new System.Windows.Forms.CheckBox();
            this.lstSelectedWalls = new System.Windows.Forms.ListBox();
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
            this.tabPageNPCs = new System.Windows.Forms.TabPage();
            this.btnAddSpawn = new System.Windows.Forms.Button();
            this.btnDeleteSpawn = new System.Windows.Forms.Button();
            this.lstNPCSpawns = new DemoGame.MapEditor.NPCSpawnsListBox();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
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
            this.pgNPCSpawn = new System.Windows.Forms.PropertyGrid();
            this.tcMenu.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.MapSizeGroupBox.SuspendLayout();
            this.tabPageGrhs.SuspendLayout();
            this.tabPageWalls.SuspendLayout();
            this.tabPageEntities.SuspendLayout();
            this.tabPageBackground.SuspendLayout();
            this.tabPageNPCs.SuspendLayout();
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
            this.tcMenu.Controls.Add(this.tabPageEntities);
            this.tcMenu.Controls.Add(this.tabPageBackground);
            this.tcMenu.Controls.Add(this.tabPageNPCs);
            this.tcMenu.Controls.Add(this.tabPageSettings);
            this.tcMenu.Location = new System.Drawing.Point(810, 3);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(345, 605);
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
            this.tabPageGeneral.Size = new System.Drawing.Size(337, 579);
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
            this.tabPageGrhs.Size = new System.Drawing.Size(337, 579);
            this.tabPageGrhs.TabIndex = 5;
            this.tabPageGrhs.Text = "Grhs";
            this.tabPageGrhs.UseVisualStyleBackColor = true;
            this.tabPageGrhs.Enter += new System.EventHandler(this.tabPageGrhs_Enter);
            // 
            // chkForeground
            // 
            this.chkForeground.AutoSize = true;
            this.chkForeground.Location = new System.Drawing.Point(101, 6);
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
            this.chkSnapGrhGrid.Location = new System.Drawing.Point(6, 6);
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
            this.treeGrhs.Location = new System.Drawing.Point(6, 29);
            this.treeGrhs.Name = "treeGrhs";
            this.treeGrhs.SelectedImageIndex = 0;
            this.treeGrhs.ShowNodeToolTips = true;
            this.treeGrhs.Size = new System.Drawing.Size(326, 547);
            this.treeGrhs.Sorted = true;
            this.treeGrhs.TabIndex = 8;
            this.treeGrhs.GrhAfterSelect += new NetGore.EditorTools.GrhTreeViewEvent(this.treeGrhs_SelectGrh);
            this.treeGrhs.GrhMouseDoubleClick += new NetGore.EditorTools.GrhTreeNodeMouseClickEvent(this.treeGrhs_DoubleClickGrh);
            // 
            // tabPageWalls
            // 
            this.tabPageWalls.Controls.Add(this.pgWall);
            this.tabPageWalls.Controls.Add(this.cmbWallType);
            this.tabPageWalls.Controls.Add(this.chkSnapWallGrid);
            this.tabPageWalls.Controls.Add(this.chkSnapWallWall);
            this.tabPageWalls.Controls.Add(this.lstSelectedWalls);
            this.tabPageWalls.Location = new System.Drawing.Point(4, 22);
            this.tabPageWalls.Name = "tabPageWalls";
            this.tabPageWalls.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWalls.Size = new System.Drawing.Size(337, 579);
            this.tabPageWalls.TabIndex = 1;
            this.tabPageWalls.Text = "Walls";
            this.tabPageWalls.ToolTipText = "Collision walls";
            this.tabPageWalls.UseVisualStyleBackColor = true;
            // 
            // pgWall
            // 
            this.pgWall.HelpVisible = false;
            this.pgWall.Location = new System.Drawing.Point(6, 469);
            this.pgWall.Name = "pgWall";
            this.pgWall.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.pgWall.Size = new System.Drawing.Size(325, 104);
            this.pgWall.TabIndex = 8;
            this.pgWall.ToolbarVisible = false;
            // 
            // cmbWallType
            // 
            this.cmbWallType.FormattingEnabled = true;
            this.cmbWallType.Location = new System.Drawing.Point(6, 29);
            this.cmbWallType.Name = "cmbWallType";
            this.cmbWallType.Size = new System.Drawing.Size(181, 21);
            this.cmbWallType.TabIndex = 7;
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
            this.lstSelectedWalls.Location = new System.Drawing.Point(6, 56);
            this.lstSelectedWalls.Name = "lstSelectedWalls";
            this.lstSelectedWalls.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelectedWalls.Size = new System.Drawing.Size(325, 407);
            this.lstSelectedWalls.Sorted = true;
            this.lstSelectedWalls.TabIndex = 3;
            this.lstSelectedWalls.SelectedIndexChanged += new System.EventHandler(this.lstSelectedWalls_SelectedIndexChanged);
            this.lstSelectedWalls.SelectedValueChanged += new System.EventHandler(this.lstSelectedWalls_SelectedValueChanged);
            // 
            // tabPageEntities
            // 
            this.tabPageEntities.Controls.Add(this.cmbEntityTypes);
            this.tabPageEntities.Controls.Add(this.btnNewEntity);
            this.tabPageEntities.Controls.Add(this.pgEntity);
            this.tabPageEntities.Controls.Add(this.lstEntities);
            this.tabPageEntities.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntities.Name = "tabPageEntities";
            this.tabPageEntities.Size = new System.Drawing.Size(337, 579);
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
            this.cmbEntityTypes.Size = new System.Drawing.Size(264, 21);
            this.cmbEntityTypes.TabIndex = 11;
            // 
            // btnNewEntity
            // 
            this.btnNewEntity.Location = new System.Drawing.Point(273, 192);
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
            this.pgEntity.Size = new System.Drawing.Size(329, 354);
            this.pgEntity.TabIndex = 1;
            this.pgEntity.ToolbarVisible = false;
            // 
            // lstEntities
            // 
            this.lstEntities.Camera = null;
            this.lstEntities.FormattingEnabled = true;
            this.lstEntities.IMap = null;
            this.lstEntities.Location = new System.Drawing.Point(3, 3);
            this.lstEntities.Name = "lstEntities";
            this.lstEntities.Size = new System.Drawing.Size(329, 186);
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
            this.tabPageBackground.Size = new System.Drawing.Size(337, 579);
            this.tabPageBackground.TabIndex = 6;
            this.tabPageBackground.Text = "Background";
            this.tabPageBackground.UseVisualStyleBackColor = true;
            // 
            // pgBGItem
            // 
            this.pgBGItem.Location = new System.Drawing.Point(3, 173);
            this.pgBGItem.Name = "pgBGItem";
            this.pgBGItem.Size = new System.Drawing.Size(329, 403);
            this.pgBGItem.TabIndex = 5;
            this.pgBGItem.ToolbarVisible = false;
            // 
            // btnNewBGSprite
            // 
            this.btnNewBGSprite.Location = new System.Drawing.Point(224, 143);
            this.btnNewBGSprite.Name = "btnNewBGSprite";
            this.btnNewBGSprite.Size = new System.Drawing.Size(74, 24);
            this.btnNewBGSprite.TabIndex = 4;
            this.btnNewBGSprite.Text = "New Sprite";
            this.btnNewBGSprite.UseVisualStyleBackColor = true;
            // 
            // btnNewBGLayer
            // 
            this.btnNewBGLayer.Location = new System.Drawing.Point(144, 143);
            this.btnNewBGLayer.Name = "btnNewBGLayer";
            this.btnNewBGLayer.Size = new System.Drawing.Size(74, 24);
            this.btnNewBGLayer.TabIndex = 3;
            this.btnNewBGLayer.Text = "New Layer";
            this.btnNewBGLayer.UseVisualStyleBackColor = true;
            this.btnNewBGLayer.Click += new System.EventHandler(this.btnNewBGLayer_Click);
            // 
            // btnDeleteBGItem
            // 
            this.btnDeleteBGItem.Location = new System.Drawing.Point(64, 143);
            this.btnDeleteBGItem.Name = "btnDeleteBGItem";
            this.btnDeleteBGItem.Size = new System.Drawing.Size(74, 24);
            this.btnDeleteBGItem.TabIndex = 2;
            this.btnDeleteBGItem.Text = "Delete";
            this.btnDeleteBGItem.UseVisualStyleBackColor = true;
            // 
            // lstBGItems
            // 
            this.lstBGItems.Camera = null;
            this.lstBGItems.FormattingEnabled = true;
            this.lstBGItems.IMap = null;
            this.lstBGItems.Location = new System.Drawing.Point(3, 3);
            this.lstBGItems.Name = "lstBGItems";
            this.lstBGItems.Size = new System.Drawing.Size(329, 134);
            this.lstBGItems.TabIndex = 0;
            this.lstBGItems.SelectedIndexChanged += new System.EventHandler(this.lstBGItems_SelectedIndexChanged);
            // 
            // tabPageNPCs
            // 
            this.tabPageNPCs.Controls.Add(this.pgNPCSpawn);
            this.tabPageNPCs.Controls.Add(this.btnAddSpawn);
            this.tabPageNPCs.Controls.Add(this.btnDeleteSpawn);
            this.tabPageNPCs.Controls.Add(this.lstNPCSpawns);
            this.tabPageNPCs.Location = new System.Drawing.Point(4, 22);
            this.tabPageNPCs.Name = "tabPageNPCs";
            this.tabPageNPCs.Size = new System.Drawing.Size(337, 579);
            this.tabPageNPCs.TabIndex = 4;
            this.tabPageNPCs.Text = "NPCs";
            this.tabPageNPCs.ToolTipText = "Mob and NPC spawning and settings";
            this.tabPageNPCs.UseVisualStyleBackColor = true;
            // 
            // btnAddSpawn
            // 
            this.btnAddSpawn.Location = new System.Drawing.Point(258, 143);
            this.btnAddSpawn.Name = "btnAddSpawn";
            this.btnAddSpawn.Size = new System.Drawing.Size(74, 24);
            this.btnAddSpawn.TabIndex = 4;
            this.btnAddSpawn.Text = "Add";
            this.btnAddSpawn.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSpawn
            // 
            this.btnDeleteSpawn.Location = new System.Drawing.Point(178, 143);
            this.btnDeleteSpawn.Name = "btnDeleteSpawn";
            this.btnDeleteSpawn.Size = new System.Drawing.Size(74, 24);
            this.btnDeleteSpawn.TabIndex = 3;
            this.btnDeleteSpawn.Text = "Delete";
            this.btnDeleteSpawn.UseVisualStyleBackColor = true;
            // 
            // lstNPCSpawns
            // 
            this.lstNPCSpawns.FormattingEnabled = true;
            this.lstNPCSpawns.Location = new System.Drawing.Point(3, 3);
            this.lstNPCSpawns.Name = "lstNPCSpawns";
            this.lstNPCSpawns.Size = new System.Drawing.Size(329, 134);
            this.lstNPCSpawns.TabIndex = 1;
            this.lstNPCSpawns.PropertyGrid = pgNPCSpawn;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.groupBox3);
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Size = new System.Drawing.Size(337, 579);
            this.tabPageSettings.TabIndex = 3;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.ToolTipText = "Map editor settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDrawBackground);
            this.groupBox3.Controls.Add(this.chkDrawEntities);
            this.groupBox3.Controls.Add(this.chkDrawAutoWalls);
            this.groupBox3.Controls.Add(this.chkShowGrhs);
            this.groupBox3.Controls.Add(this.chkShowWalls);
            this.groupBox3.Controls.Add(this.chkDrawGrid);
            this.groupBox3.Location = new System.Drawing.Point(3, 52);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(290, 95);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display Options";
            // 
            // chkDrawBackground
            // 
            this.chkDrawBackground.AutoSize = true;
            this.chkDrawBackground.Checked = true;
            this.chkDrawBackground.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawBackground.Location = new System.Drawing.Point(120, 67);
            this.chkDrawBackground.Name = "chkDrawBackground";
            this.chkDrawBackground.Size = new System.Drawing.Size(112, 17);
            this.chkDrawBackground.TabIndex = 14;
            this.chkDrawBackground.Text = "Draw Background";
            this.chkDrawBackground.UseVisualStyleBackColor = true;
            this.chkDrawBackground.CheckedChanged += new System.EventHandler(this.chkDrawBackground_CheckedChanged);
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
            this.cmdSave.Location = new System.Drawing.Point(987, 610);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(65, 24);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(916, 610);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(65, 24);
            this.cmdLoad.TabIndex = 3;
            this.cmdLoad.Text = "Load";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.Location = new System.Drawing.Point(845, 610);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(65, 24);
            this.cmdNew.TabIndex = 4;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdOptimize
            // 
            this.cmdOptimize.Location = new System.Drawing.Point(1058, 610);
            this.cmdOptimize.Name = "cmdOptimize";
            this.cmdOptimize.Size = new System.Drawing.Size(65, 24);
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
            this.panToolBar.Location = new System.Drawing.Point(4, 4);
            this.panToolBar.Name = "panToolBar";
            this.panToolBar.Size = new System.Drawing.Size(800, 32);
            this.panToolBar.TabIndex = 6;
            // 
            // picToolGrhsAdd
            // 
            this.picToolGrhsAdd.Image = ((System.Drawing.Image)(resources.GetObject("picToolGrhsAdd.Image")));
            this.picToolGrhsAdd.InitialImage = null;
            this.picToolGrhsAdd.Location = new System.Drawing.Point(123, 3);
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
            this.picToolGrhs.Location = new System.Drawing.Point(93, 3);
            this.picToolGrhs.Name = "picToolGrhs";
            this.picToolGrhs.Size = new System.Drawing.Size(24, 24);
            this.picToolGrhs.TabIndex = 3;
            this.picToolGrhs.TabStop = false;
            // 
            // picToolWallsAdd
            // 
            this.picToolWallsAdd.Image = ((System.Drawing.Image)(resources.GetObject("picToolWallsAdd.Image")));
            this.picToolWallsAdd.InitialImage = null;
            this.picToolWallsAdd.Location = new System.Drawing.Point(63, 3);
            this.picToolWallsAdd.Name = "picToolWallsAdd";
            this.picToolWallsAdd.Size = new System.Drawing.Size(24, 24);
            this.picToolWallsAdd.TabIndex = 2;
            this.picToolWallsAdd.TabStop = false;
            // 
            // picToolWalls
            // 
            this.picToolWalls.Image = ((System.Drawing.Image)(resources.GetObject("picToolWalls.Image")));
            this.picToolWalls.InitialImage = null;
            this.picToolWalls.Location = new System.Drawing.Point(33, 3);
            this.picToolWalls.Name = "picToolWalls";
            this.picToolWalls.Size = new System.Drawing.Size(24, 24);
            this.picToolWalls.TabIndex = 1;
            this.picToolWalls.TabStop = false;
            // 
            // picToolSelect
            // 
            this.picToolSelect.Image = ((System.Drawing.Image)(resources.GetObject("picToolSelect.Image")));
            this.picToolSelect.InitialImage = null;
            this.picToolSelect.Location = new System.Drawing.Point(3, 3);
            this.picToolSelect.Name = "picToolSelect";
            this.picToolSelect.Size = new System.Drawing.Size(24, 24);
            this.picToolSelect.TabIndex = 0;
            this.picToolSelect.TabStop = false;
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Location = new System.Drawing.Point(4, 38);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Screen = null;
            this.GameScreen.Size = new System.Drawing.Size(800, 600);
            this.GameScreen.TabIndex = 7;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseUp);
            // 
            // pgNPCSpawn
            // 
            this.pgNPCSpawn.Location = new System.Drawing.Point(3, 173);
            this.pgNPCSpawn.Name = "pgNPCSpawn";
            this.pgNPCSpawn.Size = new System.Drawing.Size(329, 403);
            this.pgNPCSpawn.TabIndex = 6;
            this.pgNPCSpawn.ToolbarVisible = false;
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 642);
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
            this.tabPageEntities.ResumeLayout(false);
            this.tabPageBackground.ResumeLayout(false);
            this.tabPageNPCs.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPageBackground;
        private BackgroundItemListBox lstBGItems;
        private System.Windows.Forms.Button btnNewBGSprite;
        private System.Windows.Forms.Button btnNewBGLayer;
        private System.Windows.Forms.Button btnDeleteBGItem;
        private System.Windows.Forms.PropertyGrid pgBGItem;
        private System.Windows.Forms.TabPage tabPageEntities;
        private System.Windows.Forms.PropertyGrid pgEntity;
        private EntityListBox lstEntities;
        private System.Windows.Forms.Button btnNewEntity;
        private System.Windows.Forms.ComboBox cmbEntityTypes;
        private System.Windows.Forms.CheckBox chkDrawBackground;
        private System.Windows.Forms.PropertyGrid pgWall;
        private System.Windows.Forms.Button btnAddSpawn;
        private System.Windows.Forms.Button btnDeleteSpawn;
        private NPCSpawnsListBox lstNPCSpawns;
        private System.Windows.Forms.PropertyGrid pgNPCSpawn;
    }
}