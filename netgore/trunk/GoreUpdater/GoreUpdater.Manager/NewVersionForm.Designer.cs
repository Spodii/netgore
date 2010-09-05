namespace GoreUpdater.Manager
{
    partial class NewVersionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIgnoreFilter = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRootPath = new System.Windows.Forms.TextBox();
            this.lblVersionHelp = new System.Windows.Forms.Label();
            this.lblRootPathHelp = new System.Windows.Forms.Label();
            this.lblFiltersHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version:";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(185, 224);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(104, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Delete Ignore Filters:";
            // 
            // txtIgnoreFilter
            // 
            this.txtIgnoreFilter.Location = new System.Drawing.Point(15, 84);
            this.txtIgnoreFilter.Multiline = true;
            this.txtIgnoreFilter.Name = "txtIgnoreFilter";
            this.txtIgnoreFilter.Size = new System.Drawing.Size(245, 134);
            this.txtIgnoreFilter.TabIndex = 7;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(63, 15);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(13, 13);
            this.lblVersion.TabIndex = 8;
            this.lblVersion.Text = "0";
            this.lblVersion.TextChanged += new System.EventHandler(this.lblVersion_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Root path:";
            // 
            // txtRootPath
            // 
            this.txtRootPath.Location = new System.Drawing.Point(75, 31);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.ReadOnly = true;
            this.txtRootPath.Size = new System.Drawing.Size(166, 20);
            this.txtRootPath.TabIndex = 10;
            // 
            // lblVersionHelp
            // 
            this.lblVersionHelp.AutoSize = true;
            this.lblVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblVersionHelp.Location = new System.Drawing.Point(82, 15);
            this.lblVersionHelp.Name = "lblVersionHelp";
            this.lblVersionHelp.Size = new System.Drawing.Size(13, 13);
            this.lblVersionHelp.TabIndex = 11;
            this.lblVersionHelp.Text = "?";
            this.lblVersionHelp.Click += new System.EventHandler(lblVersionHelp_Click);
            // 
            // lblRootPathHelp
            // 
            this.lblRootPathHelp.AutoSize = true;
            this.lblRootPathHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblRootPathHelp.Location = new System.Drawing.Point(247, 34);
            this.lblRootPathHelp.Name = "lblRootPathHelp";
            this.lblRootPathHelp.Size = new System.Drawing.Size(13, 13);
            this.lblRootPathHelp.TabIndex = 12;
            this.lblRootPathHelp.Text = "?";
            this.lblRootPathHelp.Click += new System.EventHandler(lblRootPathHelp_Click);
            // 
            // lblFiltersHelp
            // 
            this.lblFiltersHelp.AutoSize = true;
            this.lblFiltersHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblFiltersHelp.Location = new System.Drawing.Point(122, 68);
            this.lblFiltersHelp.Name = "lblFiltersHelp";
            this.lblFiltersHelp.Size = new System.Drawing.Size(13, 13);
            this.lblFiltersHelp.TabIndex = 13;
            this.lblFiltersHelp.Text = "?";
            this.lblFiltersHelp.Click += new System.EventHandler(lblFiltersHelp_Click);
            // 
            // NewVersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 257);
            this.Controls.Add(this.lblFiltersHelp);
            this.Controls.Add(this.lblRootPathHelp);
            this.Controls.Add(this.lblVersionHelp);
            this.Controls.Add(this.txtRootPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.txtIgnoreFilter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "NewVersionForm";
            this.Text = "Create New Version";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIgnoreFilter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRootPath;
        private System.Windows.Forms.Label lblVersionHelp;
        private System.Windows.Forms.Label lblRootPathHelp;
        private System.Windows.Forms.Label lblFiltersHelp;
    }
}