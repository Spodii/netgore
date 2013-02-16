namespace DemoGame.Editor
{
    partial class SoundEditorForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstItems = new DemoGame.Editor.SoundInfoListBox();
            this.pgItem = new System.Windows.Forms.PropertyGrid();
            this.buttonStop = new System.Windows.Forms.PictureBox();
            this.buttonPlay = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPlay)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonStop);
            this.splitContainer1.Panel2.Controls.Add(this.buttonPlay);
            this.splitContainer1.Size = new System.Drawing.Size(375, 313);
            this.splitContainer1.SplitterDistance = 274;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lstItems);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pgItem);
            this.splitContainer2.Size = new System.Drawing.Size(375, 274);
            this.splitContainer2.SplitterDistance = 154;
            this.splitContainer2.TabIndex = 0;
            // 
            // lstItems
            // 
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(154, 274);
            this.lstItems.TabIndex = 0;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // pgItem
            // 
            this.pgItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgItem.Location = new System.Drawing.Point(0, 0);
            this.pgItem.Name = "pgItem";
            this.pgItem.Size = new System.Drawing.Size(217, 274);
            this.pgItem.TabIndex = 0;
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonStop.Image = global::DemoGame.Editor.Properties.Resources.Xion;
            this.buttonStop.InitialImage = global::DemoGame.Editor.Properties.Resources.Media_Player;
            this.buttonStop.Location = new System.Drawing.Point(44, 3);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(36, 28);
            this.buttonStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonStop.TabIndex = 4;
            this.buttonStop.TabStop = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonPlay.Image = global::DemoGame.Editor.Properties.Resources.Media_Player;
            this.buttonPlay.InitialImage = global::DemoGame.Editor.Properties.Resources.Media_Player;
            this.buttonPlay.Location = new System.Drawing.Point(3, 2);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(36, 28);
            this.buttonPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonPlay.TabIndex = 3;
            this.buttonPlay.TabStop = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // SoundEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 317);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "SoundEditorForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "Sound Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SoundEditorForm_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPlay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private SoundInfoListBox lstItems;
        private System.Windows.Forms.PropertyGrid pgItem;
        private System.Windows.Forms.PictureBox buttonStop;
        private System.Windows.Forms.PictureBox buttonPlay;
    }
}