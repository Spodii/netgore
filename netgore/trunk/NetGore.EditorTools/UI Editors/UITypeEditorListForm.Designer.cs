namespace NetGore.EditorTools
{
    partial class UITypeEditorListForm<T>
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
            this.lstItems = new NetGore.EditorTools.UITypeEditorListFormListBox();
            this.SuspendLayout();
            // 
            // lstItems
            // 
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.DrawItemHandler = null;
            this.lstItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(284, 329);
            this.lstItems.TabIndex = 0;
            this.lstItems.DoubleClick += new System.EventHandler(this.lstItems_DoubleClick);
            // 
            // UITypeEditorListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 329);
            this.Controls.Add(this.lstItems);
            this.MinimizeBox = false;
            this.Name = "UITypeEditorListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the new value...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private UITypeEditorListFormListBox lstItems;
    }
}