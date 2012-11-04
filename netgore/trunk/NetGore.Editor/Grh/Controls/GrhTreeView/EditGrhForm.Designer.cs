// ReSharper disable RedundantThisQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantDelegateCreation
// ReSharper disable RedundantCast

namespace NetGore.Editor.Grhs
{
    partial class EditGrhForm
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



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Grh")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.gbAnimated = new System.Windows.Forms.GroupBox();
            this.txtFrames = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.radioAnimated = new System.Windows.Forms.RadioButton();
            this.radioStationary = new System.Windows.Forms.RadioButton();
            this.gbCategorization = new System.Windows.Forms.GroupBox();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCategory = new NetGore.Editor.Grhs.GrhDataCategoryTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbStationary = new System.Windows.Forms.GroupBox();
            this.chkAutoSize = new System.Windows.Forms.CheckBox();
            this.txtH = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtW = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTexture = new NetGore.Editor.Grhs.GrhDataTextureTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpBoundWalls = new System.Windows.Forms.TabPage();
            this.lstWalls = new NetGore.Editor.Grhs.WallsListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkPlatform = new System.Windows.Forms.CheckBox();
            this.txtWallH = new System.Windows.Forms.TextBox();
            this.txtWallW = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtWallY = new System.Windows.Forms.TextBox();
            this.txtWallX = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.screen = new NetGore.Editor.Grhs.GrhPreviewScreenControl();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.gbAnimated.SuspendLayout();
            this.gbCategorization.SuspendLayout();
            this.gbStationary.SuspendLayout();
            this.tpBoundWalls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.screen);
            this.splitContainer1.Size = new System.Drawing.Size(618, 363);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 13;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpBoundWalls);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(225, 330);
            this.tabControl1.TabIndex = 21;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.gbAnimated);
            this.tpGeneral.Controls.Add(this.radioAnimated);
            this.tpGeneral.Controls.Add(this.radioStationary);
            this.tpGeneral.Controls.Add(this.gbCategorization);
            this.tpGeneral.Controls.Add(this.gbStationary);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(217, 304);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // gbAnimated
            // 
            this.gbAnimated.Controls.Add(this.txtFrames);
            this.gbAnimated.Controls.Add(this.label9);
            this.gbAnimated.Controls.Add(this.txtSpeed);
            this.gbAnimated.Controls.Add(this.label8);
            this.gbAnimated.Location = new System.Drawing.Point(2, 137);
            this.gbAnimated.Name = "gbAnimated";
            this.gbAnimated.Size = new System.Drawing.Size(212, 161);
            this.gbAnimated.TabIndex = 19;
            this.gbAnimated.TabStop = false;
            this.gbAnimated.Text = "Animated Grh";
            // 
            // txtFrames
            // 
            this.txtFrames.Location = new System.Drawing.Point(10, 33);
            this.txtFrames.Multiline = true;
            this.txtFrames.Name = "txtFrames";
            this.txtFrames.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFrames.Size = new System.Drawing.Size(196, 95);
            this.txtFrames.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Frames:";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(94, 134);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(112, 20);
            this.txtSpeed.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Speed:";
            // 
            // radioAnimated
            // 
            this.radioAnimated.AutoSize = true;
            this.radioAnimated.Location = new System.Drawing.Point(103, 114);
            this.radioAnimated.Name = "radioAnimated";
            this.radioAnimated.Size = new System.Drawing.Size(69, 17);
            this.radioAnimated.TabIndex = 18;
            this.radioAnimated.TabStop = true;
            this.radioAnimated.Text = "Animated";
            this.radioAnimated.UseVisualStyleBackColor = true;
            this.radioAnimated.CheckedChanged += new System.EventHandler(this.radioAnimated_CheckedChanged);
            // 
            // radioStationary
            // 
            this.radioStationary.AutoSize = true;
            this.radioStationary.Location = new System.Drawing.Point(25, 114);
            this.radioStationary.Name = "radioStationary";
            this.radioStationary.Size = new System.Drawing.Size(72, 17);
            this.radioStationary.TabIndex = 17;
            this.radioStationary.TabStop = true;
            this.radioStationary.Text = "Stationary";
            this.radioStationary.UseVisualStyleBackColor = true;
            this.radioStationary.CheckedChanged += new System.EventHandler(this.radioStationary_CheckedChanged);
            // 
            // gbCategorization
            // 
            this.gbCategorization.Controls.Add(this.txtIndex);
            this.gbCategorization.Controls.Add(this.label10);
            this.gbCategorization.Controls.Add(this.txtTitle);
            this.gbCategorization.Controls.Add(this.label2);
            this.gbCategorization.Controls.Add(this.txtCategory);
            this.gbCategorization.Controls.Add(this.label1);
            this.gbCategorization.Location = new System.Drawing.Point(3, 6);
            this.gbCategorization.Name = "gbCategorization";
            this.gbCategorization.Size = new System.Drawing.Size(212, 102);
            this.gbCategorization.TabIndex = 15;
            this.gbCategorization.TabStop = false;
            this.gbCategorization.Text = "Categorization";
            // 
            // txtIndex
            // 
            this.txtIndex.Enabled = false;
            this.txtIndex.Location = new System.Drawing.Point(139, 9);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(67, 20);
            this.txtIndex.TabIndex = 9;
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(97, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Index:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(6, 74);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(200, 20);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.TextChanged += new System.EventHandler(this.txtTitle_TextChanged);
            this.txtTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTitle_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Title:";
            // 
            // txtCategory
            // 
            this.txtCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtCategory.Location = new System.Drawing.Point(6, 35);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(200, 20);
            this.txtCategory.TabIndex = 5;
            this.txtCategory.TrackTextChanged = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Category:";
            // 
            // gbStationary
            // 
            this.gbStationary.Controls.Add(this.chkAutoSize);
            this.gbStationary.Controls.Add(this.txtH);
            this.gbStationary.Controls.Add(this.label7);
            this.gbStationary.Controls.Add(this.txtW);
            this.gbStationary.Controls.Add(this.label6);
            this.gbStationary.Controls.Add(this.txtY);
            this.gbStationary.Controls.Add(this.label5);
            this.gbStationary.Controls.Add(this.txtX);
            this.gbStationary.Controls.Add(this.label4);
            this.gbStationary.Controls.Add(this.txtTexture);
            this.gbStationary.Controls.Add(this.label3);
            this.gbStationary.Location = new System.Drawing.Point(3, 137);
            this.gbStationary.Name = "gbStationary";
            this.gbStationary.Size = new System.Drawing.Size(212, 161);
            this.gbStationary.TabIndex = 16;
            this.gbStationary.TabStop = false;
            this.gbStationary.Text = "Stationary Grh";
            // 
            // chkAutoSize
            // 
            this.chkAutoSize.AutoSize = true;
            this.chkAutoSize.Location = new System.Drawing.Point(110, 116);
            this.chkAutoSize.Name = "chkAutoSize";
            this.chkAutoSize.Size = new System.Drawing.Size(96, 17);
            this.chkAutoSize.TabIndex = 15;
            this.chkAutoSize.Text = "Automatic Size";
            this.chkAutoSize.UseVisualStyleBackColor = true;
            this.chkAutoSize.CheckStateChanged += new System.EventHandler(this.chkAutoSize_CheckedChanged);
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(144, 90);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(62, 20);
            this.txtH.TabIndex = 14;
            this.txtH.TextChanged += new System.EventHandler(this.txtH_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(120, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "H:";
            // 
            // txtW
            // 
            this.txtW.Location = new System.Drawing.Point(47, 90);
            this.txtW.Name = "txtW";
            this.txtW.Size = new System.Drawing.Size(60, 20);
            this.txtW.TabIndex = 12;
            this.txtW.TextChanged += new System.EventHandler(this.txtW_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "W:";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(144, 64);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(62, 20);
            this.txtY.TabIndex = 10;
            this.txtY.TextChanged += new System.EventHandler(this.txtY_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Y:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(47, 64);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(60, 20);
            this.txtX.TabIndex = 8;
            this.txtX.TextChanged += new System.EventHandler(this.txtX_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "X:";
            // 
            // txtTexture
            // 
            this.txtTexture.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtTexture.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTexture.Location = new System.Drawing.Point(6, 38);
            this.txtTexture.Name = "txtTexture";
            this.txtTexture.Size = new System.Drawing.Size(200, 20);
            this.txtTexture.TabIndex = 6;
            this.txtTexture.TrackTextChanged = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Texture:";
            // 
            // tpBoundWalls
            // 
            this.tpBoundWalls.Controls.Add(this.lstWalls);
            this.tpBoundWalls.Controls.Add(this.panel1);
            this.tpBoundWalls.Location = new System.Drawing.Point(4, 22);
            this.tpBoundWalls.Name = "tpBoundWalls";
            this.tpBoundWalls.Padding = new System.Windows.Forms.Padding(3);
            this.tpBoundWalls.Size = new System.Drawing.Size(217, 304);
            this.tpBoundWalls.TabIndex = 1;
            this.tpBoundWalls.Text = "Bound Walls";
            this.tpBoundWalls.UseVisualStyleBackColor = true;
            // 
            // lstWalls
            // 
            this.lstWalls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstWalls.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstWalls.FormattingEnabled = true;
            this.lstWalls.Location = new System.Drawing.Point(3, 3);
            this.lstWalls.Name = "lstWalls";
            this.lstWalls.Size = new System.Drawing.Size(211, 192);
            this.lstWalls.TabIndex = 24;
            this.lstWalls.SelectedIndexChanged += new System.EventHandler(this.lstWalls_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkPlatform);
            this.panel1.Controls.Add(this.txtWallH);
            this.panel1.Controls.Add(this.txtWallW);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.txtWallY);
            this.panel1.Controls.Add(this.txtWallX);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 106);
            this.panel1.TabIndex = 23;
            // 
            // chkPlatform
            // 
            this.chkPlatform.AutoSize = true;
            this.chkPlatform.Location = new System.Drawing.Point(3, 3);
            this.chkPlatform.Name = "chkPlatform";
            this.chkPlatform.Size = new System.Drawing.Size(64, 17);
            this.chkPlatform.TabIndex = 44;
            this.chkPlatform.Text = "Platform";
            this.chkPlatform.UseVisualStyleBackColor = true;
            this.chkPlatform.CheckedChanged += new System.EventHandler(this.chkPlatform_CheckedChanged);
            // 
            // txtWallH
            // 
            this.txtWallH.Enabled = false;
            this.txtWallH.Location = new System.Drawing.Point(111, 52);
            this.txtWallH.Name = "txtWallH";
            this.txtWallH.Size = new System.Drawing.Size(55, 20);
            this.txtWallH.TabIndex = 43;
            this.txtWallH.TextChanged += new System.EventHandler(this.txtWallH_TextChanged);
            // 
            // txtWallW
            // 
            this.txtWallW.Enabled = false;
            this.txtWallW.Location = new System.Drawing.Point(26, 52);
            this.txtWallW.Name = "txtWallW";
            this.txtWallW.Size = new System.Drawing.Size(55, 20);
            this.txtWallW.TabIndex = 42;
            this.txtWallW.TextChanged += new System.EventHandler(this.txtWallW_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(91, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 13);
            this.label18.TabIndex = 41;
            this.label18.Text = "H:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(21, 13);
            this.label17.TabIndex = 40;
            this.label17.Text = "W:";
            // 
            // txtWallY
            // 
            this.txtWallY.Enabled = false;
            this.txtWallY.Location = new System.Drawing.Point(111, 26);
            this.txtWallY.Name = "txtWallY";
            this.txtWallY.Size = new System.Drawing.Size(55, 20);
            this.txtWallY.TabIndex = 39;
            this.txtWallY.TextChanged += new System.EventHandler(this.txtWallY_TextChanged);
            // 
            // txtWallX
            // 
            this.txtWallX.Enabled = false;
            this.txtWallX.Location = new System.Drawing.Point(26, 26);
            this.txtWallX.Name = "txtWallX";
            this.txtWallX.Size = new System.Drawing.Size(55, 20);
            this.txtWallX.TabIndex = 38;
            this.txtWallX.TextChanged += new System.EventHandler(this.txtWallX_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(92, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Y:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 13);
            this.label15.TabIndex = 36;
            this.label15.Text = "X:";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(106, 78);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 35;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(24, 78);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 34;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnAccept);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 330);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(4);
            this.panel2.Size = new System.Drawing.Size(225, 33);
            this.panel2.TabIndex = 20;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Location = new System.Drawing.Point(71, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 25);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAccept.Location = new System.Drawing.Point(4, 4);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(67, 25);
            this.btnAccept.TabIndex = 18;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // screen
            // 
            this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.screen.Location = new System.Drawing.Point(0, 0);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(389, 363);
            this.screen.TabIndex = 13;
            this.screen.Text = "grhPreviewScreenControl1";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 30;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // EditGrhForm
            // 
            this.ClientSize = new System.Drawing.Size(618, 363);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EditGrhForm";
            this.Text = "Grh Data Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            this.gbAnimated.ResumeLayout(false);
            this.gbAnimated.PerformLayout();
            this.gbCategorization.ResumeLayout(false);
            this.gbCategorization.PerformLayout();
            this.gbStationary.ResumeLayout(false);
            this.gbStationary.PerformLayout();
            this.tpBoundWalls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GrhPreviewScreenControl screen;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.GroupBox gbAnimated;
        private System.Windows.Forms.TextBox txtFrames;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioAnimated;
        private System.Windows.Forms.RadioButton radioStationary;
        private System.Windows.Forms.GroupBox gbCategorization;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label2;
        private GrhDataCategoryTextBox txtCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbStationary;
        private System.Windows.Forms.CheckBox chkAutoSize;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label4;
        private GrhDataTextureTextBox txtTexture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpBoundWalls;
        private WallsListBox lstWalls;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkPlatform;
        private System.Windows.Forms.TextBox txtWallH;
        private System.Windows.Forms.TextBox txtWallW;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtWallY;
        private System.Windows.Forms.TextBox txtWallX;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAccept;

    }
}