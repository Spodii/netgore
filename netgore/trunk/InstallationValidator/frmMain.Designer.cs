namespace InstallationValidator
{
    partial class frmMain
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gbTests = new System.Windows.Forms.GroupBox();
            this.btnSetupGuide = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.gbTestDesc = new System.Windows.Forms.GroupBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.lstTests = new InstallationValidator.TestListBox();
            this.btnDbSettings = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gbTests.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.gbTestDesc.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(623, 420);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gbTests);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnDbSettings);
            this.splitContainer2.Panel2.Controls.Add(this.btnSetupGuide);
            this.splitContainer2.Panel2.Controls.Add(this.btnRun);
            this.splitContainer2.Size = new System.Drawing.Size(250, 420);
            this.splitContainer2.SplitterDistance = 389;
            this.splitContainer2.TabIndex = 0;
            // 
            // gbTests
            // 
            this.gbTests.Controls.Add(this.lstTests);
            this.gbTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTests.Location = new System.Drawing.Point(0, 0);
            this.gbTests.Name = "gbTests";
            this.gbTests.Size = new System.Drawing.Size(250, 389);
            this.gbTests.TabIndex = 0;
            this.gbTests.TabStop = false;
            this.gbTests.Text = "Tests";
            // 
            // btnSetupGuide
            // 
            this.btnSetupGuide.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSetupGuide.Location = new System.Drawing.Point(91, 0);
            this.btnSetupGuide.Name = "btnSetupGuide";
            this.btnSetupGuide.Size = new System.Drawing.Size(76, 27);
            this.btnSetupGuide.TabIndex = 2;
            this.btnSetupGuide.Text = "Setup Guide";
            this.tt.SetToolTip(this.btnSetupGuide, "Opens the Setup Guide page on the NetGore site");
            this.btnSetupGuide.UseVisualStyleBackColor = true;
            this.btnSetupGuide.Click += new System.EventHandler(this.btnSetupGuide_Click);
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRun.Image = global::InstallationValidator.Properties.Resources.idle;
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRun.Location = new System.Drawing.Point(167, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(83, 27);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run Tests";
            this.tt.SetToolTip(this.btnRun, "Runs all of the tests");
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.gbTestDesc);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(369, 420);
            this.splitContainer3.SplitterDistance = 124;
            this.splitContainer3.TabIndex = 0;
            // 
            // gbTestDesc
            // 
            this.gbTestDesc.Controls.Add(this.txtDesc);
            this.gbTestDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTestDesc.Location = new System.Drawing.Point(0, 0);
            this.gbTestDesc.Name = "gbTestDesc";
            this.gbTestDesc.Size = new System.Drawing.Size(369, 124);
            this.gbTestDesc.TabIndex = 0;
            this.gbTestDesc.TabStop = false;
            this.gbTestDesc.Text = "Selected Test Description";
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc.Location = new System.Drawing.Point(3, 16);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(363, 105);
            this.txtDesc.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.gbStatus);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.lblStatus);
            this.splitContainer4.Size = new System.Drawing.Size(369, 292);
            this.splitContainer4.SplitterDistance = 263;
            this.splitContainer4.TabIndex = 0;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.txtStatus);
            this.gbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbStatus.Location = new System.Drawing.Point(0, 0);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(369, 263);
            this.gbStatus.TabIndex = 0;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Selected Test Status";
            // 
            // txtStatus
            // 
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStatus.Location = new System.Drawing.Point(3, 16);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(363, 244);
            this.txtStatus.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(369, 25);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "label1";
            // 
            // lstTests
            // 
            this.lstTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTests.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstTests.FormattingEnabled = true;
            this.lstTests.Location = new System.Drawing.Point(3, 16);
            this.lstTests.Name = "lstTests";
            this.lstTests.Size = new System.Drawing.Size(244, 368);
            this.lstTests.TabIndex = 1;
            this.lstTests.SelectedIndexChanged += new System.EventHandler(this.lstTests_SelectedIndexChanged);
            // 
            // btnDbSettings
            // 
            this.btnDbSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDbSettings.Location = new System.Drawing.Point(0, 0);
            this.btnDbSettings.Name = "btnDbSettings";
            this.btnDbSettings.Size = new System.Drawing.Size(91, 27);
            this.btnDbSettings.TabIndex = 3;
            this.btnDbSettings.Text = "DbSettings.dat";
            this.tt.SetToolTip(this.btnDbSettings, "Opens the DbSettings.dat file, which contains the database connection information" +
                    ".");
            this.btnDbSettings.UseVisualStyleBackColor = true;
            this.btnDbSettings.Click += new System.EventHandler(this.btnDbSettings_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 420);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "NetGore Installation Validator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.gbTests.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.gbTestDesc.ResumeLayout(false);
            this.gbTestDesc.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox gbTests;
        private TestListBox lstTests;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox gbTestDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnSetupGuide;
        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.Button btnDbSettings;

    }
}