namespace DemoGame.Editor.Forms
{
    partial class SelectedMapObjectsForm
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
            this.pgSelected = new System.Windows.Forms.PropertyGrid();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // pgSelected
            // 
            this.pgSelected.Location = new System.Drawing.Point(12, 12);
            this.pgSelected.Name = "pgSelected";
            this.pgSelected.Size = new System.Drawing.Size(205, 252);
            this.pgSelected.TabIndex = 0;
            // 
            // lstItems
            // 
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(223, 12);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(205, 251);
            this.lstItems.TabIndex = 1;
            // 
            // SelectedMapObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 276);
            this.Controls.Add(this.lstItems);
            this.Controls.Add(this.pgSelected);
            this.Name = "SelectedMapObjectsForm";
            this.Text = "Selected Map Objects";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgSelected;
        private System.Windows.Forms.ListBox lstItems;
    }
}