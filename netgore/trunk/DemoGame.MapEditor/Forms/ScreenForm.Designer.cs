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
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panToolBar = new System.Windows.Forms.Panel();
            this.numZoom = new System.Windows.Forms.NumericUpDown();
            this.lblZoom = new System.Windows.Forms.Label();
            this.GameScreen = new DemoGame.MapEditor.GameScreenControl();
            this.scTabsAndSelected = new System.Windows.Forms.SplitContainer();
            this.tcMenu = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.pgMap = new System.Windows.Forms.PropertyGrid();
            this.tpGrhs = new System.Windows.Forms.TabPage();
            this.treeGrhs = new NetGore.EditorTools.GrhTreeView();
            this.tpBackground = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.lstBGItems = new DemoGame.MapEditor.BackgroundItemListBox();
            this.btnDeleteBGItem = new System.Windows.Forms.Button();
            this.btnNewBGLayer = new System.Windows.Forms.Button();
            this.tpEffects = new System.Windows.Forms.TabPage();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.lstMapParticleEffects = new DemoGame.MapEditor.MapParticleEffectsListBox();
            this.btnDeleteEmitter = new System.Windows.Forms.Button();
            this.btnNewEmitter = new System.Windows.Forms.Button();
            this.tpNPCs = new System.Windows.Forms.TabPage();
            this.tcSpawns = new System.Windows.Forms.TabControl();
            this.tpSpawns = new System.Windows.Forms.TabPage();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.lstNPCSpawns = new DemoGame.MapEditor.NPCSpawnsListBox();
            this.btnDeleteSpawn = new System.Windows.Forms.Button();
            this.btnAddSpawn = new System.Windows.Forms.Button();
            this.tpPersistent = new System.Windows.Forms.TabPage();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.lstPersistentNPCs = new DemoGame.MapEditor.PersistentNPCListBox();
            this.btnDeletePersistentNPC = new System.Windows.Forms.Button();
            this.btnAddPersistentNPC = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkLightSources = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawPersistentNPCs = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawSpawnAreas = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawBackground = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawEntities = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawAutoWalls = new NetGore.EditorTools.PersistableCheckBox();
            this.chkShowGrhs = new NetGore.EditorTools.PersistableCheckBox();
            this.chkShowWalls = new NetGore.EditorTools.PersistableCheckBox();
            this.chkDrawGrid = new NetGore.EditorTools.PersistableCheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGridHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGridWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.scSelectedItems = new System.Windows.Forms.SplitContainer();
            this.pgSelected = new System.Windows.Forms.PropertyGrid();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            splitContainer3 = new System.Windows.Forms.SplitContainer();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            this.scTabsAndSelected.Panel1.SuspendLayout();
            this.scTabsAndSelected.Panel2.SuspendLayout();
            this.scTabsAndSelected.SuspendLayout();
            this.tcMenu.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpGrhs.SuspendLayout();
            this.tpBackground.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.tpEffects.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.tpNPCs.SuspendLayout();
            this.tcSpawns.SuspendLayout();
            this.tpSpawns.SuspendLayout();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.tpPersistent.SuspendLayout();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.scSelectedItems.Panel1.SuspendLayout();
            this.scSelectedItems.Panel2.SuspendLayout();
            this.scSelectedItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Location = new System.Drawing.Point(5, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer3);
            splitContainer1.Size = new System.Drawing.Size(1132, 630);
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
            splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            splitContainer2.Panel1MinSize = 26;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.GameScreen);
            splitContainer2.Size = new System.Drawing.Size(800, 630);
            splitContainer2.SplitterDistance = 26;
            splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panToolBar);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.numZoom);
            this.splitContainer4.Panel2.Controls.Add(this.lblZoom);
            this.splitContainer4.Size = new System.Drawing.Size(800, 26);
            this.splitContainer4.SplitterDistance = 683;
            this.splitContainer4.TabIndex = 0;
            // 
            // panToolBar
            // 
            this.panToolBar.BackColor = System.Drawing.SystemColors.Window;
            this.panToolBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panToolBar.Location = new System.Drawing.Point(0, 0);
            this.panToolBar.Name = "panToolBar";
            this.panToolBar.Size = new System.Drawing.Size(683, 26);
            this.panToolBar.TabIndex = 8;
            // 
            // numZoom
            // 
            this.numZoom.Location = new System.Drawing.Point(56, 3);
            this.numZoom.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numZoom.Name = "numZoom";
            this.numZoom.Size = new System.Drawing.Size(54, 20);
            this.numZoom.TabIndex = 9;
            this.numZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numZoom.ValueChanged += new System.EventHandler(this.numZoom_ValueChanged);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Location = new System.Drawing.Point(3, 6);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(54, 13);
            this.lblZoom.TabIndex = 9;
            this.lblZoom.Text = "Zoom (%):";
            // 
            // GameScreen
            // 
            this.GameScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.GameScreen.Camera = null;
            this.GameScreen.CursorPos = new Microsoft.Xna.Framework.Vector2(0F, 0F);
            this.GameScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameScreen.DrawHandler = null;
            this.GameScreen.Location = new System.Drawing.Point(0, 0);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Padding = new System.Windows.Forms.Padding(5);
            this.GameScreen.Size = new System.Drawing.Size(800, 600);
            this.GameScreen.TabIndex = 8;
            this.GameScreen.Text = "Game Screen";
            this.GameScreen.UpdateHandler = null;
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
            splitContainer3.Panel1.Controls.Add(this.scTabsAndSelected);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(this.btnSaveAs);
            splitContainer3.Panel2.Controls.Add(this.btnSave);
            splitContainer3.Panel2.Controls.Add(this.btnLoad);
            splitContainer3.Panel2.Controls.Add(this.btnNew);
            splitContainer3.Size = new System.Drawing.Size(328, 630);
            splitContainer3.SplitterDistance = 601;
            splitContainer3.TabIndex = 0;
            // 
            // scTabsAndSelected
            // 
            this.scTabsAndSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTabsAndSelected.Location = new System.Drawing.Point(0, 0);
            this.scTabsAndSelected.Name = "scTabsAndSelected";
            this.scTabsAndSelected.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTabsAndSelected.Panel1
            // 
            this.scTabsAndSelected.Panel1.Controls.Add(this.tcMenu);
            // 
            // scTabsAndSelected.Panel2
            // 
            this.scTabsAndSelected.Panel2.Controls.Add(this.scSelectedItems);
            this.scTabsAndSelected.Size = new System.Drawing.Size(328, 601);
            this.scTabsAndSelected.SplitterDistance = 328;
            this.scTabsAndSelected.TabIndex = 0;
            // 
            // tcMenu
            // 
            this.tcMenu.Controls.Add(this.tpGeneral);
            this.tcMenu.Controls.Add(this.tpGrhs);
            this.tcMenu.Controls.Add(this.tpBackground);
            this.tcMenu.Controls.Add(this.tpEffects);
            this.tcMenu.Controls.Add(this.tpNPCs);
            this.tcMenu.Controls.Add(this.tpSettings);
            this.tcMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMenu.Location = new System.Drawing.Point(0, 0);
            this.tcMenu.Name = "tcMenu";
            this.tcMenu.SelectedIndex = 0;
            this.tcMenu.Size = new System.Drawing.Size(328, 328);
            this.tcMenu.TabIndex = 3;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.pgMap);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(320, 302);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.ToolTipText = "General map information";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // pgMap
            // 
            this.pgMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMap.Location = new System.Drawing.Point(3, 3);
            this.pgMap.Name = "pgMap";
            this.pgMap.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.pgMap.Size = new System.Drawing.Size(314, 296);
            this.pgMap.TabIndex = 10;
            this.pgMap.ToolbarVisible = false;
            // 
            // tpGrhs
            // 
            this.tpGrhs.Controls.Add(this.treeGrhs);
            this.tpGrhs.Location = new System.Drawing.Point(4, 22);
            this.tpGrhs.Name = "tpGrhs";
            this.tpGrhs.Size = new System.Drawing.Size(320, 302);
            this.tpGrhs.TabIndex = 5;
            this.tpGrhs.Text = "Grhs";
            this.tpGrhs.UseVisualStyleBackColor = true;
            this.tpGrhs.Enter += new System.EventHandler(this.tabPageGrhs_Enter);
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
            this.treeGrhs.Size = new System.Drawing.Size(320, 302);
            this.treeGrhs.Sorted = true;
            this.treeGrhs.TabIndex = 10;
            this.treeGrhs.GrhAfterSelect += new NetGore.EditorTools.GrhTreeViewEvent(this.treeGrhs_GrhAfterSelect);
            // 
            // tpBackground
            // 
            this.tpBackground.Controls.Add(this.splitContainer5);
            this.tpBackground.Location = new System.Drawing.Point(4, 22);
            this.tpBackground.Name = "tpBackground";
            this.tpBackground.Size = new System.Drawing.Size(320, 302);
            this.tpBackground.TabIndex = 6;
            this.tpBackground.Text = "Background";
            this.tpBackground.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.lstBGItems);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.btnDeleteBGItem);
            this.splitContainer5.Panel2.Controls.Add(this.btnNewBGLayer);
            this.splitContainer5.Size = new System.Drawing.Size(320, 302);
            this.splitContainer5.SplitterDistance = 273;
            this.splitContainer5.TabIndex = 4;
            // 
            // lstBGItems
            // 
            this.lstBGItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBGItems.FormattingEnabled = true;
            this.lstBGItems.Location = new System.Drawing.Point(0, 0);
            this.lstBGItems.Map = null;
            this.lstBGItems.Name = "lstBGItems";
            this.lstBGItems.Size = new System.Drawing.Size(320, 264);
            this.lstBGItems.TabIndex = 1;
            this.lstBGItems.SelectedIndexChanged += new System.EventHandler(this.lstBGItems_SelectedIndexChanged);
            // 
            // btnDeleteBGItem
            // 
            this.btnDeleteBGItem.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteBGItem.Location = new System.Drawing.Point(172, 0);
            this.btnDeleteBGItem.Name = "btnDeleteBGItem";
            this.btnDeleteBGItem.Size = new System.Drawing.Size(74, 25);
            this.btnDeleteBGItem.TabIndex = 6;
            this.btnDeleteBGItem.Text = "Delete";
            this.btnDeleteBGItem.UseVisualStyleBackColor = true;
            this.btnDeleteBGItem.Click += new System.EventHandler(this.btnDeleteBGItem_Click);
            // 
            // btnNewBGLayer
            // 
            this.btnNewBGLayer.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNewBGLayer.Location = new System.Drawing.Point(246, 0);
            this.btnNewBGLayer.Name = "btnNewBGLayer";
            this.btnNewBGLayer.Size = new System.Drawing.Size(74, 25);
            this.btnNewBGLayer.TabIndex = 5;
            this.btnNewBGLayer.Text = "New Layer";
            this.btnNewBGLayer.UseVisualStyleBackColor = true;
            this.btnNewBGLayer.Click += new System.EventHandler(this.btnNewBGLayer_Click);
            // 
            // tpEffects
            // 
            this.tpEffects.Controls.Add(this.splitContainer6);
            this.tpEffects.Location = new System.Drawing.Point(4, 22);
            this.tpEffects.Name = "tpEffects";
            this.tpEffects.Size = new System.Drawing.Size(320, 302);
            this.tpEffects.TabIndex = 8;
            this.tpEffects.Text = "Effects";
            this.tpEffects.UseVisualStyleBackColor = true;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer6.IsSplitterFixed = true;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.lstMapParticleEffects);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.btnDeleteEmitter);
            this.splitContainer6.Panel2.Controls.Add(this.btnNewEmitter);
            this.splitContainer6.Size = new System.Drawing.Size(320, 302);
            this.splitContainer6.SplitterDistance = 273;
            this.splitContainer6.TabIndex = 4;
            // 
            // lstMapParticleEffects
            // 
            this.lstMapParticleEffects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMapParticleEffects.FormattingEnabled = true;
            this.lstMapParticleEffects.Location = new System.Drawing.Point(0, 0);
            this.lstMapParticleEffects.Map = null;
            this.lstMapParticleEffects.Name = "lstMapParticleEffects";
            this.lstMapParticleEffects.Size = new System.Drawing.Size(320, 264);
            this.lstMapParticleEffects.TabIndex = 4;
            this.lstMapParticleEffects.SelectedIndexChanged += new System.EventHandler(this.lstMapParticleEffects_SelectedIndexChanged);
            // 
            // btnDeleteEmitter
            // 
            this.btnDeleteEmitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteEmitter.Location = new System.Drawing.Point(172, 0);
            this.btnDeleteEmitter.Name = "btnDeleteEmitter";
            this.btnDeleteEmitter.Size = new System.Drawing.Size(74, 25);
            this.btnDeleteEmitter.TabIndex = 13;
            this.btnDeleteEmitter.Text = "Delete";
            this.btnDeleteEmitter.UseVisualStyleBackColor = true;
            this.btnDeleteEmitter.Click += new System.EventHandler(this.btnDeleteEmitter_Click);
            // 
            // btnNewEmitter
            // 
            this.btnNewEmitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNewEmitter.Location = new System.Drawing.Point(246, 0);
            this.btnNewEmitter.Name = "btnNewEmitter";
            this.btnNewEmitter.Size = new System.Drawing.Size(74, 25);
            this.btnNewEmitter.TabIndex = 12;
            this.btnNewEmitter.Text = "New Effect";
            this.btnNewEmitter.UseVisualStyleBackColor = true;
            this.btnNewEmitter.Click += new System.EventHandler(this.btnNewEmitter_Click);
            // 
            // tpNPCs
            // 
            this.tpNPCs.Controls.Add(this.tcSpawns);
            this.tpNPCs.Location = new System.Drawing.Point(4, 22);
            this.tpNPCs.Name = "tpNPCs";
            this.tpNPCs.Size = new System.Drawing.Size(320, 302);
            this.tpNPCs.TabIndex = 4;
            this.tpNPCs.Text = "NPCs";
            this.tpNPCs.ToolTipText = "Mob and NPC spawning and settings";
            this.tpNPCs.UseVisualStyleBackColor = true;
            // 
            // tcSpawns
            // 
            this.tcSpawns.Controls.Add(this.tpSpawns);
            this.tcSpawns.Controls.Add(this.tpPersistent);
            this.tcSpawns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSpawns.Location = new System.Drawing.Point(0, 0);
            this.tcSpawns.Name = "tcSpawns";
            this.tcSpawns.SelectedIndex = 0;
            this.tcSpawns.Size = new System.Drawing.Size(320, 302);
            this.tcSpawns.TabIndex = 7;
            // 
            // tpSpawns
            // 
            this.tpSpawns.Controls.Add(this.splitContainer7);
            this.tpSpawns.Location = new System.Drawing.Point(4, 22);
            this.tpSpawns.Name = "tpSpawns";
            this.tpSpawns.Padding = new System.Windows.Forms.Padding(3);
            this.tpSpawns.Size = new System.Drawing.Size(312, 276);
            this.tpSpawns.TabIndex = 0;
            this.tpSpawns.Text = "Spawns";
            this.tpSpawns.UseVisualStyleBackColor = true;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.Location = new System.Drawing.Point(3, 3);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.lstNPCSpawns);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.btnDeleteSpawn);
            this.splitContainer7.Panel2.Controls.Add(this.btnAddSpawn);
            this.splitContainer7.Size = new System.Drawing.Size(306, 270);
            this.splitContainer7.SplitterDistance = 241;
            this.splitContainer7.TabIndex = 8;
            // 
            // lstNPCSpawns
            // 
            this.lstNPCSpawns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNPCSpawns.FormattingEnabled = true;
            this.lstNPCSpawns.Location = new System.Drawing.Point(0, 0);
            this.lstNPCSpawns.Map = null;
            this.lstNPCSpawns.Name = "lstNPCSpawns";
            this.lstNPCSpawns.Size = new System.Drawing.Size(306, 238);
            this.lstNPCSpawns.TabIndex = 6;
            this.lstNPCSpawns.SelectedIndexChanged += new System.EventHandler(this.lstNPCSpawns_SelectedIndexChanged);
            // 
            // btnDeleteSpawn
            // 
            this.btnDeleteSpawn.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteSpawn.Location = new System.Drawing.Point(158, 0);
            this.btnDeleteSpawn.Name = "btnDeleteSpawn";
            this.btnDeleteSpawn.Size = new System.Drawing.Size(74, 25);
            this.btnDeleteSpawn.TabIndex = 9;
            this.btnDeleteSpawn.Text = "Delete";
            this.btnDeleteSpawn.UseVisualStyleBackColor = true;
            this.btnDeleteSpawn.Click += new System.EventHandler(this.btnDeleteSpawn_Click);
            // 
            // btnAddSpawn
            // 
            this.btnAddSpawn.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddSpawn.Location = new System.Drawing.Point(232, 0);
            this.btnAddSpawn.Name = "btnAddSpawn";
            this.btnAddSpawn.Size = new System.Drawing.Size(74, 25);
            this.btnAddSpawn.TabIndex = 8;
            this.btnAddSpawn.Text = "Add";
            this.btnAddSpawn.UseVisualStyleBackColor = true;
            this.btnAddSpawn.Click += new System.EventHandler(this.btnAddSpawn_Click);
            // 
            // tpPersistent
            // 
            this.tpPersistent.Controls.Add(this.splitContainer8);
            this.tpPersistent.Location = new System.Drawing.Point(4, 22);
            this.tpPersistent.Name = "tpPersistent";
            this.tpPersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tpPersistent.Size = new System.Drawing.Size(312, 276);
            this.tpPersistent.TabIndex = 1;
            this.tpPersistent.Text = "Persistent";
            this.tpPersistent.UseVisualStyleBackColor = true;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer8.Location = new System.Drawing.Point(3, 3);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.lstPersistentNPCs);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.btnDeletePersistentNPC);
            this.splitContainer8.Panel2.Controls.Add(this.btnAddPersistentNPC);
            this.splitContainer8.Size = new System.Drawing.Size(306, 270);
            this.splitContainer8.SplitterDistance = 241;
            this.splitContainer8.TabIndex = 12;
            // 
            // lstPersistentNPCs
            // 
            this.lstPersistentNPCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPersistentNPCs.FormattingEnabled = true;
            this.lstPersistentNPCs.Location = new System.Drawing.Point(0, 0);
            this.lstPersistentNPCs.Map = null;
            this.lstPersistentNPCs.Name = "lstPersistentNPCs";
            this.lstPersistentNPCs.PropertyGrid = this.pgSelected;
            this.lstPersistentNPCs.Size = new System.Drawing.Size(306, 238);
            this.lstPersistentNPCs.TabIndex = 12;
            this.lstPersistentNPCs.SelectedIndexChanged += new System.EventHandler(this.lstPersistentNPCs_SelectedIndexChanged);
            // 
            // btnDeletePersistentNPC
            // 
            this.btnDeletePersistentNPC.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeletePersistentNPC.Location = new System.Drawing.Point(158, 0);
            this.btnDeletePersistentNPC.Name = "btnDeletePersistentNPC";
            this.btnDeletePersistentNPC.Size = new System.Drawing.Size(74, 25);
            this.btnDeletePersistentNPC.TabIndex = 12;
            this.btnDeletePersistentNPC.Text = "Delete";
            this.btnDeletePersistentNPC.UseVisualStyleBackColor = true;
            this.btnDeletePersistentNPC.Click += new System.EventHandler(this.btnDeletePersistentNPC_Click);
            // 
            // btnAddPersistentNPC
            // 
            this.btnAddPersistentNPC.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddPersistentNPC.Location = new System.Drawing.Point(232, 0);
            this.btnAddPersistentNPC.Name = "btnAddPersistentNPC";
            this.btnAddPersistentNPC.Size = new System.Drawing.Size(74, 25);
            this.btnAddPersistentNPC.TabIndex = 11;
            this.btnAddPersistentNPC.Text = "Add";
            this.btnAddPersistentNPC.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.groupBox3);
            this.tpSettings.Controls.Add(this.groupBox1);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Size = new System.Drawing.Size(320, 302);
            this.tpSettings.TabIndex = 3;
            this.tpSettings.Text = "Settings";
            this.tpSettings.ToolTipText = "Map editor settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkLightSources);
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
            this.groupBox3.Size = new System.Drawing.Size(221, 137);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display Options";
            // 
            // chkLightSources
            // 
            this.chkLightSources.AutoSize = true;
            this.chkLightSources.Checked = true;
            this.chkLightSources.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLightSources.Location = new System.Drawing.Point(6, 113);
            this.chkLightSources.Name = "chkLightSources";
            this.chkLightSources.Size = new System.Drawing.Size(91, 17);
            this.chkLightSources.TabIndex = 17;
            this.chkLightSources.Text = "Light Sources";
            this.chkLightSources.UseVisualStyleBackColor = true;
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
            this.txtGridHeight.Text = "32";
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
            this.txtGridWidth.Text = "32";
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
            // scSelectedItems
            // 
            this.scSelectedItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSelectedItems.Location = new System.Drawing.Point(0, 0);
            this.scSelectedItems.Name = "scSelectedItems";
            // 
            // scSelectedItems.Panel1
            // 
            this.scSelectedItems.Panel1.Controls.Add(this.pgSelected);
            // 
            // scSelectedItems.Panel2
            // 
            this.scSelectedItems.Panel2.Controls.Add(this.lstSelected);
            this.scSelectedItems.Size = new System.Drawing.Size(328, 269);
            this.scSelectedItems.SplitterDistance = 176;
            this.scSelectedItems.TabIndex = 0;
            // 
            // pgSelected
            // 
            this.pgSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSelected.Location = new System.Drawing.Point(0, 0);
            this.pgSelected.Name = "pgSelected";
            this.pgSelected.Size = new System.Drawing.Size(176, 269);
            this.pgSelected.TabIndex = 1;
            // 
            // lstSelected
            // 
            this.lstSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.IntegralHeight = false;
            this.lstSelected.Location = new System.Drawing.Point(0, 0);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(148, 269);
            this.lstSelected.TabIndex = 0;
            this.lstSelected.Click += new System.EventHandler(this.lstSelected_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSaveAs.Location = new System.Drawing.Point(195, 0);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(65, 25);
            this.btnSaveAs.TabIndex = 9;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Location = new System.Drawing.Point(130, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 25);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLoad.Location = new System.Drawing.Point(65, 0);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(65, 25);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // btnNew
            // 
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNew.Location = new System.Drawing.Point(0, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(65, 25);
            this.btnNew.TabIndex = 5;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 640);
            this.Controls.Add(splitContainer1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(674, 38);
            this.Name = "ScreenForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetGore Map Editor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).EndInit();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.ResumeLayout(false);
            this.scTabsAndSelected.Panel1.ResumeLayout(false);
            this.scTabsAndSelected.Panel2.ResumeLayout(false);
            this.scTabsAndSelected.ResumeLayout(false);
            this.tcMenu.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGrhs.ResumeLayout(false);
            this.tpBackground.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.tpEffects.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.tpNPCs.ResumeLayout(false);
            this.tcSpawns.ResumeLayout(false);
            this.tpSpawns.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.ResumeLayout(false);
            this.tpPersistent.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.scSelectedItems.Panel1.ResumeLayout(false);
            this.scSelectedItems.Panel2.ResumeLayout(false);
            this.scSelectedItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.SplitContainer scTabsAndSelected;
        private System.Windows.Forms.TabControl tcMenu;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpGrhs;
        private System.Windows.Forms.TabPage tpBackground;
        private System.Windows.Forms.TabPage tpEffects;
        private System.Windows.Forms.TabPage tpNPCs;
        private System.Windows.Forms.TabControl tcSpawns;
        private System.Windows.Forms.TabPage tpSpawns;
        private System.Windows.Forms.TabPage tpPersistent;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.GroupBox groupBox3;
        private PersistableCheckBox chkDrawPersistentNPCs;
        private PersistableCheckBox chkDrawSpawnAreas;
        private PersistableCheckBox chkDrawBackground;
        private PersistableCheckBox chkDrawEntities;
        private PersistableCheckBox chkDrawAutoWalls;
        private PersistableCheckBox chkShowGrhs;
        private PersistableCheckBox chkShowWalls;
        private PersistableCheckBox chkDrawGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtGridHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGridWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer scSelectedItems;
        private System.Windows.Forms.PropertyGrid pgSelected;
        private System.Windows.Forms.ListBox lstSelected;
        public GrhTreeView treeGrhs;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.Button btnNewBGLayer;
        private BackgroundItemListBox lstBGItems;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private MapParticleEffectsListBox lstMapParticleEffects;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private NPCSpawnsListBox lstNPCSpawns;
        private System.Windows.Forms.Button btnDeleteSpawn;
        private System.Windows.Forms.Button btnAddSpawn;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private PersistentNPCListBox lstPersistentNPCs;
        private System.Windows.Forms.Button btnDeletePersistentNPC;
        private System.Windows.Forms.Button btnAddPersistentNPC;
        public GameScreenControl GameScreen;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Panel panToolBar;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.NumericUpDown numZoom;
        private PersistableCheckBox chkLightSources;
        private System.Windows.Forms.PropertyGrid pgMap;
        private System.Windows.Forms.Button btnDeleteBGItem;
        private System.Windows.Forms.Button btnDeleteEmitter;
        private System.Windows.Forms.Button btnNewEmitter;
    }
}