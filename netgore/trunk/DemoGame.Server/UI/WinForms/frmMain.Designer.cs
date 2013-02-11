namespace DemoGame.Server.UI
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
            this.tbLogItem = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.txtLogTime = new System.Windows.Forms.TextBox();
            this.txtLogClass = new System.Windows.Forms.TextBox();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.txtLogMethod = new System.Windows.Forms.TextBox();
            this.txtLogLevel = new System.Windows.Forms.TextBox();
            this.txtLogLine = new System.Windows.Forms.TextBox();
            this.txtLogMsg = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFilterRegex = new System.Windows.Forms.TextBox();
            this.chkFatal = new System.Windows.Forms.CheckBox();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.chkWarn = new System.Windows.Forms.CheckBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.tmrUpdateDisplay = new System.Windows.Forms.Timer(this.components);
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.lblBandwidth = new System.Windows.Forms.Label();
            this.lstLog = new DemoGame.Server.UI.LogListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblUserCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbStatus.SuspendLayout();
            this.tpConsole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tbLogItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
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
            this.splitContainer1.Size = new System.Drawing.Size(552, 521);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbStatus);
            this.tabControl1.Controls.Add(this.tpConsole);
            this.tabControl1.Controls.Add(this.tbLogItem);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 166);
            this.tabControl1.TabIndex = 1;
            // 
            // tbStatus
            // 
            this.tbStatus.Controls.Add(this.lblUserCount);
            this.tbStatus.Controls.Add(this.label9);
            this.tbStatus.Controls.Add(this.lblBandwidth);
            this.tbStatus.Controls.Add(this.label8);
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
            this.tbStatus.Size = new System.Drawing.Size(534, 140);
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
            this.tpConsole.Size = new System.Drawing.Size(534, 140);
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
            this.splitContainer3.Size = new System.Drawing.Size(528, 134);
            this.splitContainer3.SplitterDistance = 105;
            this.splitContainer3.TabIndex = 2;
            // 
            // txtConsoleOut
            // 
            this.txtConsoleOut.BackColor = System.Drawing.SystemColors.Window;
            this.txtConsoleOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConsoleOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleOut.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleOut.Name = "txtConsoleOut";
            this.txtConsoleOut.ReadOnly = true;
            this.txtConsoleOut.Size = new System.Drawing.Size(528, 105);
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
            // tbLogItem
            // 
            this.tbLogItem.Controls.Add(this.splitContainer4);
            this.tbLogItem.Location = new System.Drawing.Point(4, 22);
            this.tbLogItem.Name = "tbLogItem";
            this.tbLogItem.Size = new System.Drawing.Size(534, 140);
            this.tbLogItem.TabIndex = 2;
            this.tbLogItem.Text = "Log Item";
            this.tbLogItem.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.txtLogMsg);
            this.splitContainer4.Panel2.Controls.Add(this.label7);
            this.splitContainer4.Size = new System.Drawing.Size(534, 140);
            this.splitContainer4.SplitterDistance = 25;
            this.splitContainer4.TabIndex = 5;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer6.IsSplitterFixed = true;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.label6);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(534, 25);
            this.splitContainer6.SplitterDistance = 44;
            this.splitContainer6.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Source:";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.splitContainer9);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer8);
            this.splitContainer7.Size = new System.Drawing.Size(486, 25);
            this.splitContainer7.SplitterDistance = 240;
            this.splitContainer7.TabIndex = 0;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer9.IsSplitterFixed = true;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.txtLogTime);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.txtLogClass);
            this.splitContainer9.Size = new System.Drawing.Size(240, 25);
            this.splitContainer9.SplitterDistance = 56;
            this.splitContainer9.TabIndex = 0;
            // 
            // txtLogTime
            // 
            this.txtLogTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogTime.Location = new System.Drawing.Point(0, 0);
            this.txtLogTime.Name = "txtLogTime";
            this.txtLogTime.ReadOnly = true;
            this.txtLogTime.Size = new System.Drawing.Size(56, 20);
            this.txtLogTime.TabIndex = 12;
            this.tt.SetToolTip(this.txtLogTime, "Time the event was created");
            // 
            // txtLogClass
            // 
            this.txtLogClass.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogClass.Location = new System.Drawing.Point(0, 0);
            this.txtLogClass.Name = "txtLogClass";
            this.txtLogClass.ReadOnly = true;
            this.txtLogClass.Size = new System.Drawing.Size(180, 20);
            this.txtLogClass.TabIndex = 11;
            this.tt.SetToolTip(this.txtLogClass, "Class the event came from");
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer8.IsSplitterFixed = true;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.txtLogLine);
            this.splitContainer8.Size = new System.Drawing.Size(242, 25);
            this.splitContainer8.SplitterDistance = 191;
            this.splitContainer8.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.txtLogMethod);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.txtLogLevel);
            this.splitContainer5.Size = new System.Drawing.Size(191, 25);
            this.splitContainer5.SplitterDistance = 109;
            this.splitContainer5.TabIndex = 0;
            // 
            // txtLogMethod
            // 
            this.txtLogMethod.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogMethod.Location = new System.Drawing.Point(0, 0);
            this.txtLogMethod.Name = "txtLogMethod";
            this.txtLogMethod.ReadOnly = true;
            this.txtLogMethod.Size = new System.Drawing.Size(109, 20);
            this.txtLogMethod.TabIndex = 12;
            this.tt.SetToolTip(this.txtLogMethod, "Method on the class that the event came from");
            // 
            // txtLogLevel
            // 
            this.txtLogLevel.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogLevel.Location = new System.Drawing.Point(0, 0);
            this.txtLogLevel.Name = "txtLogLevel";
            this.txtLogLevel.ReadOnly = true;
            this.txtLogLevel.Size = new System.Drawing.Size(78, 20);
            this.txtLogLevel.TabIndex = 13;
            this.tt.SetToolTip(this.txtLogLevel, "The log severity level");
            // 
            // txtLogLine
            // 
            this.txtLogLine.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogLine.Location = new System.Drawing.Point(0, 0);
            this.txtLogLine.Name = "txtLogLine";
            this.txtLogLine.ReadOnly = true;
            this.txtLogLine.Size = new System.Drawing.Size(47, 20);
            this.txtLogLine.TabIndex = 13;
            this.tt.SetToolTip(this.txtLogLine, "The line of code the event was created on");
            // 
            // txtLogMsg
            // 
            this.txtLogMsg.BackColor = System.Drawing.SystemColors.Window;
            this.txtLogMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogMsg.Location = new System.Drawing.Point(0, 13);
            this.txtLogMsg.Multiline = true;
            this.txtLogMsg.Name = "txtLogMsg";
            this.txtLogMsg.ReadOnly = true;
            this.txtLogMsg.Size = new System.Drawing.Size(534, 98);
            this.txtLogMsg.TabIndex = 14;
            this.tt.SetToolTip(this.txtLogMsg, "The fully parsed log message");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Message:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 331);
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
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.txtFilterRegex);
            this.splitContainer2.Panel1.Controls.Add(this.chkFatal);
            this.splitContainer2.Panel1.Controls.Add(this.chkError);
            this.splitContainer2.Panel1.Controls.Add(this.chkWarn);
            this.splitContainer2.Panel1.Controls.Add(this.chkInfo);
            this.splitContainer2.Panel1.Controls.Add(this.chkDebug);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lstLog);
            this.splitContainer2.Size = new System.Drawing.Size(536, 312);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Right;
            this.label5.Location = new System.Drawing.Point(314, 4);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label5.Size = new System.Drawing.Size(41, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Regex:";
            // 
            // txtFilterRegex
            // 
            this.txtFilterRegex.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtFilterRegex.Location = new System.Drawing.Point(355, 4);
            this.txtFilterRegex.Name = "txtFilterRegex";
            this.txtFilterRegex.Size = new System.Drawing.Size(177, 20);
            this.txtFilterRegex.TabIndex = 15;
            this.tt.SetToolTip(this.txtFilterRegex, "Regex used to filter the log messages. Set to empty to avoid using. List is refre" +
                    "shed when changed to a valid Regex.");
            this.txtFilterRegex.TextChanged += new System.EventHandler(this.txtFilterRegex_TextChanged);
            // 
            // chkFatal
            // 
            this.chkFatal.AutoSize = true;
            this.chkFatal.Checked = true;
            this.chkFatal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFatal.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkFatal.Location = new System.Drawing.Point(206, 4);
            this.chkFatal.Name = "chkFatal";
            this.chkFatal.Size = new System.Drawing.Size(49, 17);
            this.chkFatal.TabIndex = 13;
            this.chkFatal.Text = "Fatal";
            this.tt.SetToolTip(this.chkFatal, "Tick to show Fatal logs.");
            this.chkFatal.UseVisualStyleBackColor = true;
            this.chkFatal.CheckedChanged += new System.EventHandler(this.LogCheckBox_CheckedChanged);
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.Checked = true;
            this.chkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkError.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkError.Location = new System.Drawing.Point(158, 4);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(48, 17);
            this.chkError.TabIndex = 12;
            this.chkError.Text = "Error";
            this.tt.SetToolTip(this.chkError, "Tick to show Error logs.");
            this.chkError.UseVisualStyleBackColor = true;
            this.chkError.CheckedChanged += new System.EventHandler(this.LogCheckBox_CheckedChanged);
            // 
            // chkWarn
            // 
            this.chkWarn.AutoSize = true;
            this.chkWarn.Checked = true;
            this.chkWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarn.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkWarn.Location = new System.Drawing.Point(106, 4);
            this.chkWarn.Name = "chkWarn";
            this.chkWarn.Size = new System.Drawing.Size(52, 17);
            this.chkWarn.TabIndex = 11;
            this.chkWarn.Text = "Warn";
            this.tt.SetToolTip(this.chkWarn, "Tick to show Warn logs.");
            this.chkWarn.UseVisualStyleBackColor = true;
            this.chkWarn.CheckedChanged += new System.EventHandler(this.LogCheckBox_CheckedChanged);
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkInfo.Location = new System.Drawing.Point(62, 4);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(44, 17);
            this.chkInfo.TabIndex = 10;
            this.chkInfo.Text = "Info";
            this.tt.SetToolTip(this.chkInfo, "Tick to show Info logs.");
            this.chkInfo.UseVisualStyleBackColor = true;
            this.chkInfo.CheckedChanged += new System.EventHandler(this.LogCheckBox_CheckedChanged);
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkDebug.Location = new System.Drawing.Point(4, 4);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(58, 17);
            this.chkDebug.TabIndex = 9;
            this.chkDebug.Text = "Debug";
            this.tt.SetToolTip(this.chkDebug, "Tick to show Debug logs.");
            this.chkDebug.UseVisualStyleBackColor = true;
            this.chkDebug.CheckedChanged += new System.EventHandler(this.LogCheckBox_CheckedChanged);
            // 
            // tmrUpdateDisplay
            // 
            this.tmrUpdateDisplay.Enabled = true;
            this.tmrUpdateDisplay.Interval = 500;
            this.tmrUpdateDisplay.Tick += new System.EventHandler(this.tmrUpdateDisplay_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Bandwidth:";
            // 
            // lblBandwidth
            // 
            this.lblBandwidth.AutoSize = true;
            this.lblBandwidth.Location = new System.Drawing.Point(80, 56);
            this.lblBandwidth.Name = "lblBandwidth";
            this.lblBandwidth.Size = new System.Drawing.Size(13, 13);
            this.lblBandwidth.TabIndex = 9;
            this.lblBandwidth.Text = "0";
            // 
            // lstLog
            // 
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(0, 0);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(536, 283);
            this.lstLog.TabIndex = 0;
            this.lstLog.SelectedIndexChanged += new System.EventHandler(this.lbLog_SelectedIndexChanged);
            this.lstLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbLog_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Users Online:";
            // 
            // lblUserCount
            // 
            this.lblUserCount.AutoSize = true;
            this.lblUserCount.Location = new System.Drawing.Point(80, 69);
            this.lblUserCount.Name = "lblUserCount";
            this.lblUserCount.Size = new System.Drawing.Size(13, 13);
            this.lblUserCount.TabIndex = 11;
            this.lblUserCount.Text = "0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 521);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "NetGore Server";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbStatus.ResumeLayout(false);
            this.tbStatus.PerformLayout();
            this.tpConsole.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tbLogItem.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel1.PerformLayout();
            this.splitContainer9.Panel2.ResumeLayout(false);
            this.splitContainer9.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
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
        private LogListBox lstLog;
        private System.Windows.Forms.Label lblRAMUsed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRAMFree;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tbLogItem;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.TextBox txtLogLine;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox txtLogMethod;
        private System.Windows.Forms.TextBox txtLogLevel;
        private System.Windows.Forms.TextBox txtLogMsg;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFilterRegex;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.TextBox txtLogTime;
        private System.Windows.Forms.TextBox txtLogClass;
        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.Label lblBandwidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblUserCount;
        private System.Windows.Forms.Label label9;
    }
}