using NetGore.Editor;
using NetGore.Editor.Docking;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            NetGore.Editor.Docking.DockPanelSkin dockPanelSkin3 = new NetGore.Editor.Docking.DockPanelSkin();
            NetGore.Editor.Docking.AutoHideStripSkin autoHideStripSkin3 = new NetGore.Editor.Docking.AutoHideStripSkin();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient7 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient15 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPaneStripSkin dockPaneStripSkin3 = new NetGore.Editor.Docking.DockPaneStripSkin();
            NetGore.Editor.Docking.DockPaneStripGradient dockPaneStripGradient3 = new NetGore.Editor.Docking.DockPaneStripGradient();
            NetGore.Editor.Docking.TabGradient tabGradient16 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient8 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient17 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient3 = new NetGore.Editor.Docking.DockPaneStripToolWindowGradient();
            NetGore.Editor.Docking.TabGradient tabGradient18 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.TabGradient tabGradient19 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient9 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient20 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.TabGradient tabGradient21 = new NetGore.Editor.Docking.TabGradient();
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
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tssInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssWorldPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssScreenPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new NetGore.Editor.Docking.DockPanel();
            this.tbMap = new NetGore.Editor.EditorTool.ToolBar();
            this.tbGlobal = new NetGore.Editor.EditorTool.ToolBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDepth = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarDepth = new System.Windows.Forms.TrackBar();
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
            this.viewToolStripMenuItem});
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
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Name = "dockPanel";
            dockPanelGradient7.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient7.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin3.DockStripGradient = dockPanelGradient7;
            tabGradient15.EndColor = System.Drawing.SystemColors.Control;
            tabGradient15.StartColor = System.Drawing.SystemColors.Control;
            tabGradient15.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin3.TabGradient = tabGradient15;
            dockPanelSkin3.AutoHideStripSkin = autoHideStripSkin3;
            tabGradient16.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient16.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient16.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient3.ActiveTabGradient = tabGradient16;
            dockPanelGradient8.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient8.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient3.DockStripGradient = dockPanelGradient8;
            tabGradient17.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient17.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient17.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient3.InactiveTabGradient = tabGradient17;
            dockPaneStripSkin3.DocumentGradient = dockPaneStripGradient3;
            tabGradient18.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient18.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient18.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient18.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient3.ActiveCaptionGradient = tabGradient18;
            tabGradient19.EndColor = System.Drawing.SystemColors.Control;
            tabGradient19.StartColor = System.Drawing.SystemColors.Control;
            tabGradient19.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient3.ActiveTabGradient = tabGradient19;
            dockPanelGradient9.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient9.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient3.DockStripGradient = dockPanelGradient9;
            tabGradient20.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient20.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient20.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient20.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient3.InactiveCaptionGradient = tabGradient20;
            tabGradient21.EndColor = System.Drawing.Color.Transparent;
            tabGradient21.StartColor = System.Drawing.Color.Transparent;
            tabGradient21.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient3.InactiveTabGradient = tabGradient21;
            dockPaneStripSkin3.ToolWindowGradient = dockPaneStripToolWindowGradient3;
            dockPanelSkin3.DockPaneStripSkin = dockPaneStripSkin3;
            this.dockPanel.Skin = dockPanelSkin3;
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
            this.trackBarDepth.LargeChange = 100;
            this.trackBarDepth.Maximum = 500;
            this.trackBarDepth.Minimum = -500;
            this.trackBarDepth.Name = "trackBarDepth";
            this.trackBarDepth.TickFrequency = 250;
            this.trackBarDepth.Scroll += new System.EventHandler(this.trackBarDepth_Scroll);
            this.trackBarDepth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackBarDepth_MouseDown);
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
    }
}

