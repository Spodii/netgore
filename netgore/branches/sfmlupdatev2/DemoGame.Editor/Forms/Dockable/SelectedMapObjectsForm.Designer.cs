namespace DemoGame.Editor
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
            this.components = new System.ComponentModel.Container();
            this.sc = new System.Windows.Forms.SplitContainer();
            this.pgSelected = new System.Windows.Forms.PropertyGrid();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.tmrUpdateLstItemsVisibility = new System.Windows.Forms.Timer(this.components);
            this.sc.Panel1.SuspendLayout();
            this.sc.Panel2.SuspendLayout();
            this.sc.SuspendLayout();
            this.SuspendLayout();
            // 
            // sc
            // 
            this.sc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc.Location = new System.Drawing.Point(0, 0);
            this.sc.Name = "sc";
            // 
            // sc.Panel1
            // 
            this.sc.Panel1.Controls.Add(this.pgSelected);
            // 
            // sc.Panel2
            // 
            this.sc.Panel2.Controls.Add(this.lstItems);
            this.sc.Size = new System.Drawing.Size(470, 318);
            this.sc.SplitterDistance = 297;
            this.sc.TabIndex = 2;
            // 
            // pgSelected
            // 
            this.pgSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSelected.Location = new System.Drawing.Point(0, 0);
            this.pgSelected.Name = "pgSelected";
            this.pgSelected.Size = new System.Drawing.Size(297, 318);
            this.pgSelected.TabIndex = 1;
            // 
            // lstItems
            // 
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(169, 318);
            this.lstItems.TabIndex = 2;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // tmrUpdateLstItemsVisibility
            // 
            this.tmrUpdateLstItemsVisibility.Enabled = true;
            this.tmrUpdateLstItemsVisibility.Interval = 500;
            this.tmrUpdateLstItemsVisibility.Tick += new System.EventHandler(this.tmrUpdateLstItemsVisibility_Tick);
            // 
            // SelectedMapObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 318);
            this.Controls.Add(this.sc);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "SelectedMapObjectsForm";
            this.Text = "Selected Map Objects";
            this.sc.Panel1.ResumeLayout(false);
            this.sc.Panel2.ResumeLayout(false);
            this.sc.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer sc;
        private System.Windows.Forms.PropertyGrid pgSelected;
        private System.Windows.Forms.ListBox lstItems;
        private System.Windows.Forms.Timer tmrUpdateLstItemsVisibility;

    }
}