namespace DemoGame.MapEditor.Forms
{
    partial class MiniMapForm
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
            this.cmsMPC = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miResizeToAspect = new System.Windows.Forms.ToolStripMenuItem();
            this.mpc = new DemoGame.MapEditor.MapPreviewControl();
            this.cmsMPC.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsMPC
            // 
            this.cmsMPC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miResizeToAspect});
            this.cmsMPC.Name = "cmsMPC";
            this.cmsMPC.Size = new System.Drawing.Size(231, 26);
            // 
            // miResizeToAspect
            // 
            this.miResizeToAspect.Name = "miResizeToAspect";
            this.miResizeToAspect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miResizeToAspect.Size = new System.Drawing.Size(230, 22);
            this.miResizeToAspect.Text = "Resize to Aspect Ratio";
            this.miResizeToAspect.Click += new System.EventHandler(this.miResizeToAspect_Click);
            // 
            // mpc
            // 
            this.mpc.Camera = null;
            this.mpc.ContextMenuStrip = this.cmsMPC;
            this.mpc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpc.Location = new System.Drawing.Point(0, 0);
            this.mpc.Name = "mpc";
            this.mpc.Size = new System.Drawing.Size(284, 262);
            this.mpc.TabIndex = 0;
            this.mpc.Text = "mapPreviewControl1";
            // 
            // MiniMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mpc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "MiniMapForm";
            this.ShowInTaskbar = false;
            this.Text = "Mini-Map";
            this.TopMost = true;
            this.cmsMPC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MapPreviewControl mpc;
        private System.Windows.Forms.ContextMenuStrip cmsMPC;
        private System.Windows.Forms.ToolStripMenuItem miResizeToAspect;
    }
}