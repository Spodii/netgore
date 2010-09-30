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
            NetGore.EditorTools.Docking.DockPanelSkin dockPanelSkin1 = new NetGore.EditorTools.Docking.DockPanelSkin();
            NetGore.EditorTools.Docking.AutoHideStripSkin autoHideStripSkin1 = new NetGore.EditorTools.Docking.AutoHideStripSkin();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient1 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient1 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPaneStripSkin dockPaneStripSkin1 = new NetGore.EditorTools.Docking.DockPaneStripSkin();
            NetGore.EditorTools.Docking.DockPaneStripGradient dockPaneStripGradient1 = new NetGore.EditorTools.Docking.DockPaneStripGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient2 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient2 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient3 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new NetGore.EditorTools.Docking.DockPaneStripToolWindowGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient4 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient5 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.DockPanelGradient dockPanelGradient3 = new NetGore.EditorTools.Docking.DockPanelGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient6 = new NetGore.EditorTools.Docking.TabGradient();
            NetGore.EditorTools.Docking.TabGradient tabGradient7 = new NetGore.EditorTools.Docking.TabGradient();
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
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(687, 24);
            this.msMenu.TabIndex = 0;
            this.msMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapToolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mapToolStripMenuItem1
            // 
            this.mapToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.newToolStripMenuItem});
            this.mapToolStripMenuItem1.Name = "mapToolStripMenuItem1";
            this.mapToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.mapToolStripMenuItem1.Text = "Map";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miniMapToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.viewToolStripMenuItem.Text = "Screens";
            // 
            // miniMapToolStripMenuItem
            // 
            this.miniMapToolStripMenuItem.Name = "miniMapToolStripMenuItem";
            this.miniMapToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.miniMapToolStripMenuItem.Text = "Mini-Map";
            // 
            // tsTools
            // 
            this.tsTools.Location = new System.Drawing.Point(0, 24);
            this.tsTools.Name = "tsTools";
            this.tsTools.Size = new System.Drawing.Size(687, 25);
            this.tsTools.TabIndex = 1;
            this.tsTools.Text = "toolStrip1";
            // 
            // ssStatus
            // 
            this.ssStatus.Location = new System.Drawing.Point(0, 499);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(687, 22);
            this.ssStatus.TabIndex = 2;
            this.ssStatus.Text = "statusStrip1";
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Location = new System.Drawing.Point(0, 49);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(687, 450);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel.Skin = dockPanelSkin1;
            this.dockPanel.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 521);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.tsTools);
            this.Controls.Add(this.msMenu);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.msMenu;
            this.Name = "MainForm";
            this.Text = "NetGore Editor";
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

