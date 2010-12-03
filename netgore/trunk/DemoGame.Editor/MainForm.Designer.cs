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
            NetGore.Editor.Docking.DockPanelSkin dockPanelSkin2 = new NetGore.Editor.Docking.DockPanelSkin();
            NetGore.Editor.Docking.AutoHideStripSkin autoHideStripSkin2 = new NetGore.Editor.Docking.AutoHideStripSkin();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient4 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient8 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPaneStripSkin dockPaneStripSkin2 = new NetGore.Editor.Docking.DockPaneStripSkin();
            NetGore.Editor.Docking.DockPaneStripGradient dockPaneStripGradient2 = new NetGore.Editor.Docking.DockPaneStripGradient();
            NetGore.Editor.Docking.TabGradient tabGradient9 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient5 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient10 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new NetGore.Editor.Docking.DockPaneStripToolWindowGradient();
            NetGore.Editor.Docking.TabGradient tabGradient11 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.TabGradient tabGradient12 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.DockPanelGradient dockPanelGradient6 = new NetGore.Editor.Docking.DockPanelGradient();
            NetGore.Editor.Docking.TabGradient tabGradient13 = new NetGore.Editor.Docking.TabGradient();
            NetGore.Editor.Docking.TabGradient tabGradient14 = new NetGore.Editor.Docking.TabGradient();
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
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tssInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssWorldPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssScreenPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.dockPanel = new NetGore.Editor.Docking.DockPanel();
            this.tbMap = new NetGore.Editor.EditorTool.ToolBar();
            this.tbGlobal = new NetGore.Editor.EditorTool.ToolBar();
            this.msMenu.SuspendLayout();
            this.ssStatus.SuspendLayout();
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
            this.dbEditorToolStripMenuItem});
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
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
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
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.tbMap);
            this.Controls.Add(this.tbGlobal);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMenu;
            this.Name = "MainForm";
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
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
    }
}

