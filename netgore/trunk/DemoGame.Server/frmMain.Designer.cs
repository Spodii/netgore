namespace DemoGame.Server
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbStatus = new System.Windows.Forms.TabPage();
            this.lblIP = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRAMFree = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRAMUsed = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCPU = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpConsole = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtConsoleOut = new System.Windows.Forms.RichTextBox();
            this.txtConsoleIn = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chkFatal = new System.Windows.Forms.CheckBox();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.chkWarn = new System.Windows.Forms.CheckBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.lbLog = new DemoGame.Server.LogListBox();
            this.tmrUpdateDisplay = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbStatus.SuspendLayout();
            this.tpConsole.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Size = new System.Drawing.Size(552, 515);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbStatus);
            this.tabControl1.Controls.Add(this.tpConsole);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 207);
            this.tabControl1.TabIndex = 1;
            // 
            // tbStatus
            // 
            this.tbStatus.Controls.Add(this.lblIP);
            this.tbStatus.Controls.Add(this.label4);
            this.tbStatus.Controls.Add(this.lblRAMFree);
            this.tbStatus.Controls.Add(this.label3);
            this.tbStatus.Controls.Add(this.lblRAMUsed);
            this.tbStatus.Controls.Add(this.label2);
            this.tbStatus.Controls.Add(this.lblCPU);
            this.tbStatus.Controls.Add(this.label1);
            this.tbStatus.Location = new System.Drawing.Point(4, 22);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tbStatus.Size = new System.Drawing.Size(534, 181);
            this.tbStatus.TabIndex = 1;
            this.tbStatus.Text = "Status";
            this.tbStatus.UseVisualStyleBackColor = true;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(80, 3);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(52, 13);
            this.lblIP.TabIndex = 7;
            this.lblIP.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "External IP:";
            // 
            // lblRAMFree
            // 
            this.lblRAMFree.AutoSize = true;
            this.lblRAMFree.Location = new System.Drawing.Point(80, 43);
            this.lblRAMFree.Name = "lblRAMFree";
            this.lblRAMFree.Size = new System.Drawing.Size(13, 13);
            this.lblRAMFree.TabIndex = 5;
            this.lblRAMFree.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "RAM Free:";
            // 
            // lblRAMUsed
            // 
            this.lblRAMUsed.AutoSize = true;
            this.lblRAMUsed.Location = new System.Drawing.Point(80, 30);
            this.lblRAMUsed.Name = "lblRAMUsed";
            this.lblRAMUsed.Size = new System.Drawing.Size(13, 13);
            this.lblRAMUsed.TabIndex = 3;
            this.lblRAMUsed.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "RAM Usage:";
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.Location = new System.Drawing.Point(80, 16);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(21, 13);
            this.lblCPU.TabIndex = 1;
            this.lblCPU.Text = "0%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU Usage:";
            // 
            // tpConsole
            // 
            this.tpConsole.Controls.Add(this.splitContainer3);
            this.tpConsole.Location = new System.Drawing.Point(4, 22);
            this.tpConsole.Name = "tpConsole";
            this.tpConsole.Padding = new System.Windows.Forms.Padding(3);
            this.tpConsole.Size = new System.Drawing.Size(534, 181);
            this.tpConsole.TabIndex = 0;
            this.tpConsole.Text = "Console";
            this.tpConsole.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.txtConsoleOut);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtConsoleIn);
            this.splitContainer3.Size = new System.Drawing.Size(528, 175);
            this.splitContainer3.SplitterDistance = 146;
            this.splitContainer3.TabIndex = 2;
            // 
            // txtConsoleOut
            // 
            this.txtConsoleOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConsoleOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleOut.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleOut.Name = "txtConsoleOut";
            this.txtConsoleOut.Size = new System.Drawing.Size(528, 146);
            this.txtConsoleOut.TabIndex = 0;
            this.txtConsoleOut.Text = "";
            // 
            // txtConsoleIn
            // 
            this.txtConsoleIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleIn.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleIn.Name = "txtConsoleIn";
            this.txtConsoleIn.Size = new System.Drawing.Size(528, 20);
            this.txtConsoleIn.TabIndex = 2;
            this.txtConsoleIn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConsoleIn_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 284);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logging";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chkFatal);
            this.splitContainer2.Panel1.Controls.Add(this.chkError);
            this.splitContainer2.Panel1.Controls.Add(this.chkWarn);
            this.splitContainer2.Panel1.Controls.Add(this.chkInfo);
            this.splitContainer2.Panel1.Controls.Add(this.chkDebug);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lbLog);
            this.splitContainer2.Size = new System.Drawing.Size(536, 265);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 9;
            // 
            // chkFatal
            // 
            this.chkFatal.AutoSize = true;
            this.chkFatal.Checked = true;
            this.chkFatal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFatal.Location = new System.Drawing.Point(229, 3);
            this.chkFatal.Name = "chkFatal";
            this.chkFatal.Size = new System.Drawing.Size(49, 17);
            this.chkFatal.TabIndex = 13;
            this.chkFatal.Text = "Fatal";
            this.chkFatal.UseVisualStyleBackColor = true;
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.Checked = true;
            this.chkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkError.Location = new System.Drawing.Point(175, 3);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(48, 17);
            this.chkError.TabIndex = 12;
            this.chkError.Text = "Error";
            this.chkError.UseVisualStyleBackColor = true;
            // 
            // chkWarn
            // 
            this.chkWarn.AutoSize = true;
            this.chkWarn.Checked = true;
            this.chkWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarn.Location = new System.Drawing.Point(117, 3);
            this.chkWarn.Name = "chkWarn";
            this.chkWarn.Size = new System.Drawing.Size(52, 17);
            this.chkWarn.TabIndex = 11;
            this.chkWarn.Text = "Warn";
            this.chkWarn.UseVisualStyleBackColor = true;
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Location = new System.Drawing.Point(67, 3);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(44, 17);
            this.chkInfo.TabIndex = 10;
            this.chkInfo.Text = "Info";
            this.chkInfo.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Checked = true;
            this.chkDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDebug.Location = new System.Drawing.Point(3, 3);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(58, 17);
            this.chkDebug.TabIndex = 9;
            this.chkDebug.Text = "Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // lbLog
            // 
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(0, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(536, 225);
            this.lbLog.TabIndex = 0;
            this.lbLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbLog_KeyDown);
            // 
            // tmrUpdateDisplay
            // 
            this.tmrUpdateDisplay.Enabled = true;
            this.tmrUpdateDisplay.Interval = 500;
            this.tmrUpdateDisplay.Tick += new System.EventHandler(this.tmrUpdateDisplay_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 515);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbStatus.ResumeLayout(false);
            this.tbStatus.PerformLayout();
            this.tpConsole.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpConsole;
        private System.Windows.Forms.TabPage tbStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox chkFatal;
        private System.Windows.Forms.CheckBox chkError;
        private System.Windows.Forms.CheckBox chkWarn;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtConsoleIn;
        private System.Windows.Forms.Timer tmrUpdateDisplay;
        private System.Windows.Forms.RichTextBox txtConsoleOut;
        private LogListBox lbLog;
        private System.Windows.Forms.Label lblRAMUsed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRAMFree;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label4;
    }
}