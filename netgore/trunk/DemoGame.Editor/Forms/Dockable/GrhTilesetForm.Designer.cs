namespace DemoGame.Editor.Forms.Dockable
{
    partial class GrhTilesetForm
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
            this.camScrollY = new System.Windows.Forms.VScrollBar();
            this.camScrollX = new System.Windows.Forms.HScrollBar();
            this.contextTilesetSettings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectGRHFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grhAtlasView1 = new DemoGame.Editor.GrhAtlasView();
            this.contextTilesetSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // camScrollY
            // 
            this.camScrollY.Dock = System.Windows.Forms.DockStyle.Right;
            this.camScrollY.Location = new System.Drawing.Point(267, 0);
            this.camScrollY.Name = "camScrollY";
            this.camScrollY.Size = new System.Drawing.Size(17, 261);
            this.camScrollY.TabIndex = 0;
            this.camScrollY.ValueChanged += new System.EventHandler(this.camScrollY_ValueChanged);
            // 
            // camScrollX
            // 
            this.camScrollX.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.camScrollX.Location = new System.Drawing.Point(0, 244);
            this.camScrollX.Name = "camScrollX";
            this.camScrollX.Size = new System.Drawing.Size(267, 17);
            this.camScrollX.TabIndex = 1;
            this.camScrollX.ValueChanged += new System.EventHandler(this.camScrollX_ValueChanged);
            // 
            // contextTilesetSettings
            // 
            this.contextTilesetSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectGRHFolderToolStripMenuItem});
            this.contextTilesetSettings.Name = "contextMenuStrip1";
            this.contextTilesetSettings.Size = new System.Drawing.Size(176, 26);
            // 
            // selectGRHFolderToolStripMenuItem
            // 
            this.selectGRHFolderToolStripMenuItem.Name = "selectGRHFolderToolStripMenuItem";
            this.selectGRHFolderToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.selectGRHFolderToolStripMenuItem.Text = "Select GRH folder...";
            this.selectGRHFolderToolStripMenuItem.Click += new System.EventHandler(this.selectGRHFolderToolStripMenuItem_Click);
            // 
            // grhAtlasView1
            // 
            this.grhAtlasView1.ContextMenuStrip = this.contextTilesetSettings;
            this.grhAtlasView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grhAtlasView1.Location = new System.Drawing.Point(0, 0);
            this.grhAtlasView1.Name = "grhAtlasView1";
            this.grhAtlasView1.Size = new System.Drawing.Size(267, 244);
            this.grhAtlasView1.TabIndex = 2;
            this.grhAtlasView1.Text = "grhAtlasView1";
            this.grhAtlasView1.TilesetConfiguration = null;
            // 
            // GrhTilesetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.grhAtlasView1);
            this.Controls.Add(this.camScrollX);
            this.Controls.Add(this.camScrollY);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "GrhTilesetForm";
            this.TabText = "GRH Tileset";
            this.Text = "GrhTilesetForm";
            this.contextTilesetSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar camScrollY;
        private System.Windows.Forms.HScrollBar camScrollX;
        private GrhAtlasView grhAtlasView1;
        private System.Windows.Forms.ContextMenuStrip contextTilesetSettings;
        private System.Windows.Forms.ToolStripMenuItem selectGRHFolderToolStripMenuItem;
    }
}