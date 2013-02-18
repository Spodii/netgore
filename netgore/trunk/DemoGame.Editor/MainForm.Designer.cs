using NetGore.Editor;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor
{
    partial class MainForm
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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.particleEffectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newPEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedMapObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grhDatasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.npcChatEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skeletonEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bodyEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tssInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssWorldPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssScreenPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tbMap = new NetGore.Editor.EditorTool.ToolBar();
            this.tbGlobal = new NetGore.Editor.EditorTool.ToolBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDepth = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarDepth = new System.Windows.Forms.TrackBar();
            this.grhTilesetViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msMenu.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            resources.ApplyResources(this.msMenu, "msMenu");
            this.msMenu.Name = "msMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem1,
            this.particleEffectToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // mapToolStripMenuItem1
            // 
            this.mapToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMapToolStripMenuItem,
            this.newMapToolStripMenuItem});
            this.mapToolStripMenuItem1.Name = "mapToolStripMenuItem1";
            resources.ApplyResources(this.mapToolStripMenuItem1, "mapToolStripMenuItem1");
            // 
            // loadMapToolStripMenuItem
            // 
            this.loadMapToolStripMenuItem.Name = "loadMapToolStripMenuItem";
            resources.ApplyResources(this.loadMapToolStripMenuItem, "loadMapToolStripMenuItem");
            this.loadMapToolStripMenuItem.Click += new System.EventHandler(this.loadMapToolStripMenuItem_Click);
            // 
            // newMapToolStripMenuItem
            // 
            this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
            resources.ApplyResources(this.newMapToolStripMenuItem, "newMapToolStripMenuItem");
            this.newMapToolStripMenuItem.Click += new System.EventHandler(this.newMapToolStripMenuItem_Click);
            // 
            // particleEffectToolStripMenuItem
            // 
            this.particleEffectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPEToolStripMenuItem,
            this.newPEToolStripMenuItem});
            this.particleEffectToolStripMenuItem.Name = "particleEffectToolStripMenuItem";
            resources.ApplyResources(this.particleEffectToolStripMenuItem, "particleEffectToolStripMenuItem");
            // 
            // loadPEToolStripMenuItem
            // 
            this.loadPEToolStripMenuItem.Name = "loadPEToolStripMenuItem";
            resources.ApplyResources(this.loadPEToolStripMenuItem, "loadPEToolStripMenuItem");
            this.loadPEToolStripMenuItem.Click += new System.EventHandler(this.loadPEToolStripMenuItem_Click);
            // 
            // newPEToolStripMenuItem
            // 
            this.newPEToolStripMenuItem.Name = "newPEToolStripMenuItem";
            resources.ApplyResources(this.newPEToolStripMenuItem, "newPEToolStripMenuItem");
            this.newPEToolStripMenuItem.Click += new System.EventHandler(this.newPEToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedMapObjectsToolStripMenuItem,
            this.grhTilesetViewMenuItem,
            this.grhDatasToolStripMenuItem,
            this.npcChatEditorToolStripMenuItem,
            this.skeletonEditorToolStripMenuItem,
            this.dbEditorToolStripMenuItem,
            this.musicEditorToolStripMenuItem,
            this.soundEditorToolStripMenuItem,
            this.bodyEditorToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // selectedMapObjectsToolStripMenuItem
            // 
            this.selectedMapObjectsToolStripMenuItem.CheckOnClick = true;
            this.selectedMapObjectsToolStripMenuItem.Name = "selectedMapObjectsToolStripMenuItem";
            resources.ApplyResources(this.selectedMapObjectsToolStripMenuItem, "selectedMapObjectsToolStripMenuItem");
            this.selectedMapObjectsToolStripMenuItem.Click += new System.EventHandler(this.selectedMapObjectsToolStripMenuItem_CheckedChanged);
            // 
            // grhDatasToolStripMenuItem
            // 
            this.grhDatasToolStripMenuItem.CheckOnClick = true;
            this.grhDatasToolStripMenuItem.Name = "grhDatasToolStripMenuItem";
            resources.ApplyResources(this.grhDatasToolStripMenuItem, "grhDatasToolStripMenuItem");
            this.grhDatasToolStripMenuItem.Click += new System.EventHandler(this.grhDatasToolStripMenuItem_Click);
            // 
            // npcChatEditorToolStripMenuItem
            // 
            this.npcChatEditorToolStripMenuItem.CheckOnClick = true;
            this.npcChatEditorToolStripMenuItem.Name = "npcChatEditorToolStripMenuItem";
            resources.ApplyResources(this.npcChatEditorToolStripMenuItem, "npcChatEditorToolStripMenuItem");
            this.npcChatEditorToolStripMenuItem.Click += new System.EventHandler(this.npcChatEditorToolStripMenuItem_Click);
            // 
            // skeletonEditorToolStripMenuItem
            // 
            this.skeletonEditorToolStripMenuItem.CheckOnClick = true;
            this.skeletonEditorToolStripMenuItem.Name = "skeletonEditorToolStripMenuItem";
            resources.ApplyResources(this.skeletonEditorToolStripMenuItem, "skeletonEditorToolStripMenuItem");
            this.skeletonEditorToolStripMenuItem.Click += new System.EventHandler(this.skeletonEditorToolStripMenuItem_Click);
            // 
            // dbEditorToolStripMenuItem
            // 
            this.dbEditorToolStripMenuItem.CheckOnClick = true;
            this.dbEditorToolStripMenuItem.Name = "dbEditorToolStripMenuItem";
            resources.ApplyResources(this.dbEditorToolStripMenuItem, "dbEditorToolStripMenuItem");
            this.dbEditorToolStripMenuItem.Click += new System.EventHandler(this.dbEditorToolStripMenuItem_Click);
            // 
            // musicEditorToolStripMenuItem
            // 
            this.musicEditorToolStripMenuItem.CheckOnClick = true;
            this.musicEditorToolStripMenuItem.Name = "musicEditorToolStripMenuItem";
            resources.ApplyResources(this.musicEditorToolStripMenuItem, "musicEditorToolStripMenuItem");
            this.musicEditorToolStripMenuItem.Click += new System.EventHandler(this.musicEditorToolStripMenuItem_Click);
            // 
            // soundEditorToolStripMenuItem
            // 
            this.soundEditorToolStripMenuItem.CheckOnClick = true;
            this.soundEditorToolStripMenuItem.Name = "soundEditorToolStripMenuItem";
            resources.ApplyResources(this.soundEditorToolStripMenuItem, "soundEditorToolStripMenuItem");
            this.soundEditorToolStripMenuItem.Click += new System.EventHandler(this.soundEditorToolStripMenuItem_Click);
            // 
            // bodyEditorToolStripMenuItem
            // 
            this.bodyEditorToolStripMenuItem.CheckOnClick = true;
            this.bodyEditorToolStripMenuItem.Name = "bodyEditorToolStripMenuItem";
            resources.ApplyResources(this.bodyEditorToolStripMenuItem, "bodyEditorToolStripMenuItem");
            this.bodyEditorToolStripMenuItem.Click += new System.EventHandler(this.bodyEditorToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shiftContentsToolStripMenuItem});
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            resources.ApplyResources(this.mapToolStripMenuItem, "mapToolStripMenuItem");
            this.mapToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.mapToolStripMenuItem_Paint);
            // 
            // shiftContentsToolStripMenuItem
            // 
            this.shiftContentsToolStripMenuItem.Name = "shiftContentsToolStripMenuItem";
            resources.ApplyResources(this.shiftContentsToolStripMenuItem, "shiftContentsToolStripMenuItem");
            this.shiftContentsToolStripMenuItem.Click += new System.EventHandler(this.shiftContentsToolStripMenuItem_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssInfo,
            this.tssWorldPos,
            this.tssScreenPos});
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.Name = "ssStatus";
            // 
            // tssInfo
            // 
            this.tssInfo.Name = "tssInfo";
            resources.ApplyResources(this.tssInfo, "tssInfo");
            this.tssInfo.Spring = true;
            // 
            // tssWorldPos
            // 
            this.tssWorldPos.Name = "tssWorldPos";
            resources.ApplyResources(this.tssWorldPos, "tssWorldPos");
            // 
            // tssScreenPos
            // 
            this.tssScreenPos.Name = "tssScreenPos";
            resources.ApplyResources(this.tssScreenPos, "tssScreenPos");
            // 
            // dockPanel
            // 
            this.dockPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Name = "dockPanel";
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
            autoHideStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
            tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
            dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
            tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
            dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
            dockPaneStripSkin2.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
            tabGradient12.EndColor = System.Drawing.SystemColors.Control;
            tabGradient12.StartColor = System.Drawing.SystemColors.Control;
            tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
            dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
            tabGradient13.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
            tabGradient14.EndColor = System.Drawing.Color.Transparent;
            tabGradient14.StartColor = System.Drawing.Color.Transparent;
            tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
            dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
            dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
            this.dockPanel.Skin = dockPanelSkin2;
            this.dockPanel.Click += new System.EventHandler(this.dockPanel_Click);
            // 
            // tbMap
            // 
            this.tbMap.AllowItemReorder = true;
            this.tbMap.DisplayObject = null;
            resources.ApplyResources(this.tbMap, "tbMap");
            this.tbMap.Name = "tbMap";
            // 
            // tbGlobal
            // 
            this.tbGlobal.AllowItemReorder = true;
            this.tbGlobal.DisplayObject = null;
            resources.ApplyResources(this.tbGlobal, "tbGlobal");
            this.tbGlobal.Name = "tbGlobal";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDepth);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbLayer);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.trackBarDepth);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lblDepth
            // 
            resources.ApplyResources(this.lblDepth, "lblDepth");
            this.lblDepth.Name = "lblDepth";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbLayer
            // 
            this.cmbLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLayer.Items.AddRange(new object[] {
            resources.GetString("cmbLayer.Items"),
            resources.GetString("cmbLayer.Items1"),
            resources.GetString("cmbLayer.Items2")});
            resources.ApplyResources(this.cmbLayer, "cmbLayer");
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.SelectedValueChanged += new System.EventHandler(this.cmbLayer_SelectedValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // trackBarDepth
            // 
            resources.ApplyResources(this.trackBarDepth, "trackBarDepth");
            this.trackBarDepth.LargeChange = 10;
            this.trackBarDepth.Minimum = -10;
            this.trackBarDepth.Name = "trackBarDepth";
            this.trackBarDepth.Scroll += new System.EventHandler(this.trackBarDepth_Scroll);
            this.trackBarDepth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarDepth_MouseDown);
            // 
            // grhTilesetViewMenuItem
            // 
            this.grhTilesetViewMenuItem.CheckOnClick = true;
            this.grhTilesetViewMenuItem.Name = "grhTilesetViewMenuItem";
            resources.ApplyResources(this.grhTilesetViewMenuItem, "grhTilesetViewMenuItem");
            this.grhTilesetViewMenuItem.Click += new System.EventHandler(this.grhTilesetViewMenuItem_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.tbMap);
            this.Controls.Add(this.tbGlobal);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMenu);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMenu;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
        private ToolBar tbGlobal;
        private ToolBar tbMap;
        private DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem selectedMapObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grhDatasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem npcChatEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skeletonEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dbEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem particleEffectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newPEToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tssInfo;
        private System.Windows.Forms.ToolStripStatusLabel tssWorldPos;
        private System.Windows.Forms.ToolStripStatusLabel tssScreenPos;
        private System.Windows.Forms.ToolStripMenuItem musicEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bodyEditorToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarDepth;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shiftContentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grhTilesetViewMenuItem;
    }
}

