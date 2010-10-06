namespace DemoGame.Editor
{
    partial class GrhTreeViewForm
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
            this.gtv = new NetGore.Editor.Grhs.GrhTreeView();
            this.SuspendLayout();
            // 
            // gtv
            // 
            this.gtv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gtv.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.gtv.ImageIndex = 0;
            this.gtv.Location = new System.Drawing.Point(0, 0);
            this.gtv.Name = "gtv";
            this.gtv.SelectedImageIndex = 0;
            this.gtv.Size = new System.Drawing.Size(284, 262);
            this.gtv.TabIndex = 0;
            // 
            // GrhTreeViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.gtv);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "GrhTreeViewForm";
            this.Text = "GrhDatas";
            this.ResumeLayout(false);

        }

        #endregion

        private NetGore.Editor.Grhs.GrhTreeView gtv;
    }
}