using NetGore.Editor.Grhs;

namespace NetGore.Editor.UI
{
    partial class GrhUITypeEditorForm
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        private void InitializeComponent()
        {
            this.gtv = new NetGore.Editor.Grhs.GrhTreeView();
            this.SuspendLayout();
            // 
            // gtv
            // 
            this.gtv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gtv.Location = new System.Drawing.Point(0, 0);
            this.gtv.Name = "gtv";
            this.gtv.Size = new System.Drawing.Size(308, 494);
            this.gtv.TabIndex = 0;
            this.gtv.GrhMouseClick += new System.EventHandler<GrhTreeNodeMouseClickEventArgs>(this.gtv_GrhMouseClick);
            this.gtv.GrhMouseDoubleClick += new System.EventHandler<GrhTreeNodeMouseClickEventArgs>(this.gtv_GrhMouseDoubleClick);
            // 
            // GrhUITypeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 494);
            this.Controls.Add(this.gtv);
            this.MinimizeBox = false;
            this.Name = "GrhUITypeEditorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the new GrhData...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private GrhTreeView gtv;
    }
}