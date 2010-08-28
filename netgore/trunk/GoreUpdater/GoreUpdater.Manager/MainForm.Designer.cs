namespace GoreUpdater.Manager
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCurrentVersion = new System.Windows.Forms.TabPage();
            this.lblCreateNewVersionHelp = new System.Windows.Forms.Label();
            this.btnNewVersion = new System.Windows.Forms.Button();
            this.lblLiveVersionHelp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpMasterServers = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstMS = new System.Windows.Forms.ListBox();
            this.btnMSNew = new System.Windows.Forms.Button();
            this.btnMSInfo = new System.Windows.Forms.Button();
            this.btnMSDelete = new System.Windows.Forms.Button();
            this.tpFileServers = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstFS = new ServerInfoListBox();
            this.btnFSNew = new System.Windows.Forms.Button();
            this.btnFSInfo = new System.Windows.Forms.Button();
            this.btnFSDelete = new System.Windows.Forms.Button();
            this.lblLiveVersion = new System.Windows.Forms.Label();
            this.btnChangeLiveVersion = new System.Windows.Forms.Button();
            this.lblChangeLiveVersionHelp = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tpCurrentVersion.SuspendLayout();
            this.tpMasterServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tpFileServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCurrentVersion);
            this.tabControl1.Controls.Add(this.tpMasterServers);
            this.tabControl1.Controls.Add(this.tpFileServers);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 354);
            this.tabControl1.TabIndex = 0;
            // 
            // tpCurrentVersion
            // 
            this.tpCurrentVersion.Controls.Add(this.lblChangeLiveVersionHelp);
            this.tpCurrentVersion.Controls.Add(this.btnChangeLiveVersion);
            this.tpCurrentVersion.Controls.Add(this.lblLiveVersion);
            this.tpCurrentVersion.Controls.Add(this.lblCreateNewVersionHelp);
            this.tpCurrentVersion.Controls.Add(this.btnNewVersion);
            this.tpCurrentVersion.Controls.Add(this.lblLiveVersionHelp);
            this.tpCurrentVersion.Controls.Add(this.label1);
            this.tpCurrentVersion.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentVersion.Name = "tpCurrentVersion";
            this.tpCurrentVersion.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentVersion.Size = new System.Drawing.Size(365, 328);
            this.tpCurrentVersion.TabIndex = 0;
            this.tpCurrentVersion.Text = "Version";
            this.tpCurrentVersion.UseVisualStyleBackColor = true;
            // 
            // lblCreateNewVersionHelp
            // 
            this.lblCreateNewVersionHelp.AutoSize = true;
            this.lblCreateNewVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblCreateNewVersionHelp.Location = new System.Drawing.Point(6, 67);
            this.lblCreateNewVersionHelp.Name = "lblCreateNewVersionHelp";
            this.lblCreateNewVersionHelp.Size = new System.Drawing.Size(13, 13);
            this.lblCreateNewVersionHelp.TabIndex = 4;
            this.lblCreateNewVersionHelp.Text = "?";
            // 
            // btnNewVersion
            // 
            this.btnNewVersion.Location = new System.Drawing.Point(25, 61);
            this.btnNewVersion.Name = "btnNewVersion";
            this.btnNewVersion.Size = new System.Drawing.Size(127, 24);
            this.btnNewVersion.TabIndex = 3;
            this.btnNewVersion.Text = " Create New Version";
            this.btnNewVersion.UseVisualStyleBackColor = true;
            this.btnNewVersion.Click += new System.EventHandler(this.btnNewVersion_Click);
            // 
            // lblLiveVersionHelp
            // 
            this.lblLiveVersionHelp.AutoSize = true;
            this.lblLiveVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblLiveVersionHelp.Location = new System.Drawing.Point(6, 12);
            this.lblLiveVersionHelp.Name = "lblLiveVersionHelp";
            this.lblLiveVersionHelp.Size = new System.Drawing.Size(13, 13);
            this.lblLiveVersionHelp.TabIndex = 2;
            this.lblLiveVersionHelp.Text = "?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Live Version:";
            // 
            // tpMasterServers
            // 
            this.tpMasterServers.Controls.Add(this.splitContainer2);
            this.tpMasterServers.Location = new System.Drawing.Point(4, 22);
            this.tpMasterServers.Name = "tpMasterServers";
            this.tpMasterServers.Size = new System.Drawing.Size(365, 328);
            this.tpMasterServers.TabIndex = 2;
            this.tpMasterServers.Text = "Master Servers";
            this.tpMasterServers.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lstMS);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnMSNew);
            this.splitContainer2.Panel2.Controls.Add(this.btnMSInfo);
            this.splitContainer2.Panel2.Controls.Add(this.btnMSDelete);
            this.splitContainer2.Size = new System.Drawing.Size(365, 328);
            this.splitContainer2.SplitterDistance = 299;
            this.splitContainer2.TabIndex = 1;
            // 
            // lstMS
            // 
            this.lstMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMS.FormattingEnabled = true;
            this.lstMS.Location = new System.Drawing.Point(0, 0);
            this.lstMS.Name = "lstMS";
            this.lstMS.Size = new System.Drawing.Size(365, 299);
            this.lstMS.TabIndex = 0;
            // 
            // btnMSNew
            // 
            this.btnMSNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMSNew.Location = new System.Drawing.Point(203, 0);
            this.btnMSNew.Name = "btnMSNew";
            this.btnMSNew.Size = new System.Drawing.Size(54, 25);
            this.btnMSNew.TabIndex = 5;
            this.btnMSNew.Text = "New";
            this.btnMSNew.UseVisualStyleBackColor = true;
            // 
            // btnMSInfo
            // 
            this.btnMSInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMSInfo.Location = new System.Drawing.Point(257, 0);
            this.btnMSInfo.Name = "btnMSInfo";
            this.btnMSInfo.Size = new System.Drawing.Size(54, 25);
            this.btnMSInfo.TabIndex = 4;
            this.btnMSInfo.Text = "Info";
            this.btnMSInfo.UseVisualStyleBackColor = true;
            // 
            // btnMSDelete
            // 
            this.btnMSDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMSDelete.Location = new System.Drawing.Point(311, 0);
            this.btnMSDelete.Name = "btnMSDelete";
            this.btnMSDelete.Size = new System.Drawing.Size(54, 25);
            this.btnMSDelete.TabIndex = 2;
            this.btnMSDelete.Text = "Delete";
            this.btnMSDelete.UseVisualStyleBackColor = true;
            // 
            // tpFileServers
            // 
            this.tpFileServers.Controls.Add(this.splitContainer1);
            this.tpFileServers.Location = new System.Drawing.Point(4, 22);
            this.tpFileServers.Name = "tpFileServers";
            this.tpFileServers.Size = new System.Drawing.Size(365, 328);
            this.tpFileServers.TabIndex = 3;
            this.tpFileServers.Text = "File Servers";
            this.tpFileServers.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstFS);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnFSNew);
            this.splitContainer1.Panel2.Controls.Add(this.btnFSInfo);
            this.splitContainer1.Panel2.Controls.Add(this.btnFSDelete);
            this.splitContainer1.Size = new System.Drawing.Size(365, 328);
            this.splitContainer1.SplitterDistance = 299;
            this.splitContainer1.TabIndex = 0;
            // 
            // lstFS
            // 
            this.lstFS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFS.FormattingEnabled = true;
            this.lstFS.Location = new System.Drawing.Point(0, 0);
            this.lstFS.Name = "lstFS";
            this.lstFS.Size = new System.Drawing.Size(365, 299);
            this.lstFS.TabIndex = 0;
            // 
            // btnFSNew
            // 
            this.btnFSNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFSNew.Location = new System.Drawing.Point(203, 0);
            this.btnFSNew.Name = "btnFSNew";
            this.btnFSNew.Size = new System.Drawing.Size(54, 25);
            this.btnFSNew.TabIndex = 5;
            this.btnFSNew.Text = "New";
            this.btnFSNew.UseVisualStyleBackColor = true;
            // 
            // btnFSInfo
            // 
            this.btnFSInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFSInfo.Location = new System.Drawing.Point(257, 0);
            this.btnFSInfo.Name = "btnFSInfo";
            this.btnFSInfo.Size = new System.Drawing.Size(54, 25);
            this.btnFSInfo.TabIndex = 4;
            this.btnFSInfo.Text = "Info";
            this.btnFSInfo.UseVisualStyleBackColor = true;
            // 
            // btnFSDelete
            // 
            this.btnFSDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFSDelete.Location = new System.Drawing.Point(311, 0);
            this.btnFSDelete.Name = "btnFSDelete";
            this.btnFSDelete.Size = new System.Drawing.Size(54, 25);
            this.btnFSDelete.TabIndex = 2;
            this.btnFSDelete.Text = "Delete";
            this.btnFSDelete.UseVisualStyleBackColor = true;
            // 
            // lblLiveVersion
            // 
            this.lblLiveVersion.AutoSize = true;
            this.lblLiveVersion.Location = new System.Drawing.Point(99, 12);
            this.lblLiveVersion.Name = "lblLiveVersion";
            this.lblLiveVersion.Size = new System.Drawing.Size(13, 13);
            this.lblLiveVersion.TabIndex = 5;
            this.lblLiveVersion.Text = "0";
            // 
            // btnChangeLiveVersion
            // 
            this.btnChangeLiveVersion.Location = new System.Drawing.Point(25, 31);
            this.btnChangeLiveVersion.Name = "btnChangeLiveVersion";
            this.btnChangeLiveVersion.Size = new System.Drawing.Size(127, 24);
            this.btnChangeLiveVersion.TabIndex = 6;
            this.btnChangeLiveVersion.Text = "Change Live Version";
            this.btnChangeLiveVersion.UseVisualStyleBackColor = true;
            this.btnChangeLiveVersion.Click += new System.EventHandler(this.btnChangeLiveVersion_Click);
            // 
            // lblChangeLiveVersionHelp
            // 
            this.lblChangeLiveVersionHelp.AutoSize = true;
            this.lblChangeLiveVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblChangeLiveVersionHelp.Location = new System.Drawing.Point(6, 37);
            this.lblChangeLiveVersionHelp.Name = "lblChangeLiveVersionHelp";
            this.lblChangeLiveVersionHelp.Size = new System.Drawing.Size(13, 13);
            this.lblChangeLiveVersionHelp.TabIndex = 7;
            this.lblChangeLiveVersionHelp.Text = "?";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 362);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "GoreUpdate Manager";
            this.tabControl1.ResumeLayout(false);
            this.tpCurrentVersion.ResumeLayout(false);
            this.tpCurrentVersion.PerformLayout();
            this.tpMasterServers.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tpFileServers.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCurrentVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpMasterServers;
        private System.Windows.Forms.TabPage tpFileServers;
        private System.Windows.Forms.Label lblCreateNewVersionHelp;
        private System.Windows.Forms.Button btnNewVersion;
        private System.Windows.Forms.Label lblLiveVersionHelp;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ServerInfoListBox lstFS;
        private System.Windows.Forms.Button btnFSNew;
        private System.Windows.Forms.Button btnFSInfo;
        private System.Windows.Forms.Button btnFSDelete;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox lstMS;
        private System.Windows.Forms.Button btnMSNew;
        private System.Windows.Forms.Button btnMSInfo;
        private System.Windows.Forms.Button btnMSDelete;
        private System.Windows.Forms.Label lblLiveVersion;
        private System.Windows.Forms.Button btnChangeLiveVersion;
        private System.Windows.Forms.Label lblChangeLiveVersionHelp;

    }
}

