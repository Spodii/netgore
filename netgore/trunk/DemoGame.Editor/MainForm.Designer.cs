using NetGore.EditorTools.Docking;

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
            NetGore.EditorTools.Docking.DockPanelSkin dockPanelSkin2 = new NetGore.EditorTools.Docking.DockPanelSkin();
            NetGore.EditorTools.Docking.AutoHideStripSkin autoHideStripSkin2 = new NetGore.EditorTools.Docking.AutoHideStripSkin();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient4 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient8 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPaneStripSkin dockPaneStripSkin2 = new NetGore.EditorTools.Docking.DockPaneStripSkin();
            NetGore.EditorTools.Docking.DockPaneStripGradient dockPaneStripGradient2 = new NetGore.EditorTools.Docking.DockPaneStripGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient9 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient5 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient10 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new NetGore.EditorTools.Docking.DockPaneStripToolWindowGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient11 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient12 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient6 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient13 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient14 = new NetGore.EditorTools.Docking.TabGradient();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miniMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsTools = new System.Windows.Forms.ToolStrip();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.dockPanel = new NetGore.EditorTools.Docking.DockPanel();
            this.msMenu.SuspendLayout();
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
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // mapToolStripMenuItem1
            // 
            this.mapToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.newToolStripMenuItem});
            this.mapToolStripMenuItem1.Name = "mapToolStripMenuItem1";
            resources.ApplyResources(this.mapToolStripMenuItem1, "mapToolStripMenuItem1");
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            resources.ApplyResources(this.loadToolStripMenuItem, "loadToolStripMenuItem");
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
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
            this.miniMapToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // miniMapToolStripMenuItem
            // 
            this.miniMapToolStripMenuItem.Name = "miniMapToolStripMenuItem";
            resources.ApplyResources(this.miniMapToolStripMenuItem, "miniMapToolStripMenuItem");
            // 
            // tsTools
            // 
            resources.ApplyResources(this.tsTools, "tsTools");
            this.tsTools.Name = "tsTools";
            // 
            // ssStatus
            // 
            resources.ApplyResources(this.ssStatus, "ssStatus");
            this.ssStatus.Name = "ssStatus";
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
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.tsTools);
            this.Controls.Add(this.msMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMenu;
            this.Name = "MainForm";
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStrip tsTools;
        private System.Windows.Forms.StatusStrip ssStatus;
        private DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miniMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    }
}

