namespace NetGore.EditorTools.Grhs
{
    partial class MissingTexturesForm
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
            this.TextureList = new System.Windows.Forms.ListBox();
            this.CurrentTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewTxt = new System.Windows.Forms.TextBox();
            this.PreviewPic = new System.Windows.Forms.PictureBox();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GrhDatasList = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DeleteBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPic)).BeginInit();
            this.SuspendLayout();
            // 
            // TextureList
            // 
            this.TextureList.FormattingEnabled = true;
            this.TextureList.Location = new System.Drawing.Point(12, 25);
            this.TextureList.Name = "TextureList";
            this.TextureList.Size = new System.Drawing.Size(318, 264);
            this.TextureList.TabIndex = 0;
            this.TextureList.SelectedIndexChanged += new System.EventHandler(this.BadGrhDatasList_SelectedIndexChanged);
            // 
            // CurrentTxt
            // 
            this.CurrentTxt.Enabled = false;
            this.CurrentTxt.Location = new System.Drawing.Point(15, 308);
            this.CurrentTxt.Name = "CurrentTxt";
            this.CurrentTxt.Size = new System.Drawing.Size(315, 20);
            this.CurrentTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Texture:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 331);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "New Texture:";
            // 
            // NewTxt
            // 
            this.NewTxt.Location = new System.Drawing.Point(15, 347);
            this.NewTxt.Name = "NewTxt";
            this.NewTxt.Size = new System.Drawing.Size(315, 20);
            this.NewTxt.TabIndex = 4;
            this.NewTxt.TextChanged += new System.EventHandler(this.NewTxt_TextChanged);
            // 
            // PreviewPic
            // 
            this.PreviewPic.BackColor = System.Drawing.Color.White;
            this.PreviewPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreviewPic.Location = new System.Drawing.Point(336, 243);
            this.PreviewPic.Name = "PreviewPic";
            this.PreviewPic.Size = new System.Drawing.Size(160, 160);
            this.PreviewPic.TabIndex = 5;
            this.PreviewPic.TabStop = false;
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(243, 373);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(87, 26);
            this.ApplyBtn.TabIndex = 6;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Missing Textures:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(333, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Texture\'s GrhDatas:";
            // 
            // GrhDatasList
            // 
            this.GrhDatasList.FormattingEnabled = true;
            this.GrhDatasList.Location = new System.Drawing.Point(336, 25);
            this.GrhDatasList.Name = "GrhDatasList";
            this.GrhDatasList.Size = new System.Drawing.Size(160, 199);
            this.GrhDatasList.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(336, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "New Texture Preview:";
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(150, 373);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(87, 26);
            this.DeleteBtn.TabIndex = 11;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // MissingTexturesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 414);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GrhDatasList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.PreviewPic);
            this.Controls.Add(this.NewTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentTxt);
            this.Controls.Add(this.TextureList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MissingTexturesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Missing Textures";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MissingTexturesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox TextureList;
        private System.Windows.Forms.TextBox CurrentTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NewTxt;
        private System.Windows.Forms.PictureBox PreviewPic;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox GrhDatasList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DeleteBtn;

    }
}