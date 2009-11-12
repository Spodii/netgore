// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace DemoGame.MapEditor
{
    partial class BatchRenameTextureForm
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
            this.lblTreeLoc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTextures = new System.Windows.Forms.ListBox();
            this.txtCurrent = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblRefBy = new System.Windows.Forms.Label();
            this.lstReferenced = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.picOldTexture = new System.Windows.Forms.PictureBox();
            this.picNewTexture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picOldTexture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewTexture)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTreeLoc
            // 
            this.lblTreeLoc.AutoSize = true;
            this.lblTreeLoc.Location = new System.Drawing.Point(9, 9);
            this.lblTreeLoc.Name = "lblTreeLoc";
            this.lblTreeLoc.Size = new System.Drawing.Size(76, 13);
            this.lblTreeLoc.TabIndex = 0;
            this.lblTreeLoc.Text = "Tree Location:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Referenced Textures:";
            // 
            // lstTextures
            // 
            this.lstTextures.FormattingEnabled = true;
            this.lstTextures.Location = new System.Drawing.Point(12, 50);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(438, 134);
            this.lstTextures.TabIndex = 2;
            this.lstTextures.SelectedIndexChanged += new System.EventHandler(this.lstTextures_SelectedIndexChanged);
            // 
            // txtCurrent
            // 
            this.txtCurrent.Enabled = false;
            this.txtCurrent.Location = new System.Drawing.Point(61, 399);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.Size = new System.Drawing.Size(336, 20);
            this.txtCurrent.TabIndex = 3;
            this.txtCurrent.TextChanged += new System.EventHandler(this.txtCurrent_TextChanged);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(403, 395);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(47, 27);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblRefBy
            // 
            this.lblRefBy.AutoSize = true;
            this.lblRefBy.Location = new System.Drawing.Point(12, 187);
            this.lblRefBy.Name = "lblRefBy";
            this.lblRefBy.Size = new System.Drawing.Size(81, 13);
            this.lblRefBy.TabIndex = 5;
            this.lblRefBy.Text = "Referenced By:";
            // 
            // lstReferenced
            // 
            this.lstReferenced.FormattingEnabled = true;
            this.lstReferenced.Location = new System.Drawing.Point(12, 203);
            this.lstReferenced.Name = "lstReferenced";
            this.lstReferenced.Size = new System.Drawing.Size(438, 186);
            this.lstReferenced.Sorted = true;
            this.lstReferenced.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(453, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Old Texture:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(453, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "New Texture:";
            // 
            // picOldTexture
            // 
            this.picOldTexture.Location = new System.Drawing.Point(456, 25);
            this.picOldTexture.Name = "picOldTexture";
            this.picOldTexture.Size = new System.Drawing.Size(237, 187);
            this.picOldTexture.TabIndex = 9;
            this.picOldTexture.TabStop = false;
            // 
            // picNewTexture
            // 
            this.picNewTexture.Location = new System.Drawing.Point(456, 235);
            this.picNewTexture.Name = "picNewTexture";
            this.picNewTexture.Size = new System.Drawing.Size(237, 187);
            this.picNewTexture.TabIndex = 10;
            this.picNewTexture.TabStop = false;
            // 
            // BatchRenameTextureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 432);
            this.Controls.Add(this.picNewTexture);
            this.Controls.Add(this.picOldTexture);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstReferenced);
            this.Controls.Add(this.lblRefBy);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.txtCurrent);
            this.Controls.Add(this.lstTextures);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTreeLoc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BatchRenameTextureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Texture Renamer";
            this.Load += new System.EventHandler(this.BatchRenameTextureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picOldTexture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewTexture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTreeLoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstTextures;
        private System.Windows.Forms.TextBox txtCurrent;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblRefBy;
        private System.Windows.Forms.ListBox lstReferenced;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picOldTexture;
        private System.Windows.Forms.PictureBox picNewTexture;
    }
}