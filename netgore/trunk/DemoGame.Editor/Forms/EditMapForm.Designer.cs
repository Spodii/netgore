namespace DemoGame.Editor
{
    partial class EditMapForm
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
            this.components = new System.ComponentModel.Container();
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapScreen = new DemoGame.Editor.MapScreenControl();
            this.cmsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetSizeToolStripMenuItem});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(126, 26);
            // 
            // resetSizeToolStripMenuItem
            // 
            this.resetSizeToolStripMenuItem.Name = "resetSizeToolStripMenuItem";
            this.resetSizeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.resetSizeToolStripMenuItem.Text = "Reset Size";
            // 
            // mapScreen
            // 
            this.mapScreen.CursorPos = new SFML.Graphics.Vector2(0F, 0F);
            this.mapScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapScreen.Location = new System.Drawing.Point(0, 0);
            this.mapScreen.Map = null;
            this.mapScreen.Name = "mapScreen";
            this.mapScreen.Size = new System.Drawing.Size(784, 562);
            this.mapScreen.TabIndex = 1;
            this.mapScreen.Text = "Map Screen";
            // 
            // EditMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.ContextMenuStrip = this.cmsMenu;
            this.Controls.Add(this.mapScreen);
            this.HideOnClose = false;
            this.Name = "EditMapForm";
            this.Text = "Map";
            this.ToolBarVisibility = NetGore.EditorTools.ToolBarVisibility.Map;
            this.cmsMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem resetSizeToolStripMenuItem;
        private MapScreenControl mapScreen;
    }
}