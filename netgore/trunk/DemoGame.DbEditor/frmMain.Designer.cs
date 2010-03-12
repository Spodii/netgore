namespace DemoGame.DbEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpItemTemplate = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtItemTemplate = new System.Windows.Forms.TextBox();
            this.btnItemTemplate = new System.Windows.Forms.Button();
            this.btnItemTemplateNew = new System.Windows.Forms.Button();
            this.btnItemTemplateDelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pgItemTemplate = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tpItemTemplate.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpItemTemplate);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(464, 403);
            this.tabControl1.TabIndex = 0;
            // 
            // tpItemTemplate
            // 
            this.tpItemTemplate.Controls.Add(this.splitContainer1);
            this.tpItemTemplate.Location = new System.Drawing.Point(4, 22);
            this.tpItemTemplate.Name = "tpItemTemplate";
            this.tpItemTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tpItemTemplate.Size = new System.Drawing.Size(456, 377);
            this.tpItemTemplate.TabIndex = 0;
            this.tpItemTemplate.Text = "Item Templates";
            this.tpItemTemplate.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtItemTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplate);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplateNew);
            this.splitContainer1.Panel1.Controls.Add(this.btnItemTemplateDelete);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1MinSize = 22;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgItemTemplate);
            this.splitContainer1.Size = new System.Drawing.Size(450, 371);
            this.splitContainer1.SplitterDistance = 22;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtItemTemplate
            // 
            this.txtItemTemplate.BackColor = System.Drawing.SystemColors.Window;
            this.txtItemTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtItemTemplate.Location = new System.Drawing.Point(52, 0);
            this.txtItemTemplate.Name = "txtItemTemplate";
            this.txtItemTemplate.ReadOnly = true;
            this.txtItemTemplate.Size = new System.Drawing.Size(281, 20);
            this.txtItemTemplate.TabIndex = 13;
            // 
            // btnItemTemplate
            // 
            this.btnItemTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplate.Location = new System.Drawing.Point(333, 0);
            this.btnItemTemplate.Name = "btnItemTemplate";
            this.btnItemTemplate.Size = new System.Drawing.Size(26, 22);
            this.btnItemTemplate.TabIndex = 12;
            this.btnItemTemplate.Text = "...";
            this.btnItemTemplate.UseVisualStyleBackColor = true;
            this.btnItemTemplate.Click += new System.EventHandler(this.btnItemTemplate_Click);
            // 
            // btnItemTemplateNew
            // 
            this.btnItemTemplateNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplateNew.Location = new System.Drawing.Point(359, 0);
            this.btnItemTemplateNew.Name = "btnItemTemplateNew";
            this.btnItemTemplateNew.Size = new System.Drawing.Size(41, 22);
            this.btnItemTemplateNew.TabIndex = 11;
            this.btnItemTemplateNew.Text = "New";
            this.btnItemTemplateNew.UseVisualStyleBackColor = true;
            this.btnItemTemplateNew.Click += new System.EventHandler(this.btnItemTemplateNew_Click);
            // 
            // btnItemTemplateDelete
            // 
            this.btnItemTemplateDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemTemplateDelete.Location = new System.Drawing.Point(400, 0);
            this.btnItemTemplateDelete.Name = "btnItemTemplateDelete";
            this.btnItemTemplateDelete.Size = new System.Drawing.Size(50, 22);
            this.btnItemTemplateDelete.TabIndex = 10;
            this.btnItemTemplateDelete.Text = "Delete";
            this.btnItemTemplateDelete.UseVisualStyleBackColor = true;
            this.btnItemTemplateDelete.Click += new System.EventHandler(this.btnItemTemplateDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected:";
            // 
            // pgItemTemplate
            // 
            this.pgItemTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgItemTemplate.Location = new System.Drawing.Point(0, 0);
            this.pgItemTemplate.Name = "pgItemTemplate";
            this.pgItemTemplate.Size = new System.Drawing.Size(450, 345);
            this.pgItemTemplate.TabIndex = 0;
            this.pgItemTemplate.SelectedObjectsChanged += new System.EventHandler(this.pgItemTemplate_SelectedObjectsChanged);
            this.pgItemTemplate.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgItemTemplate_PropertyValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(456, 377);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 415);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "NetGore Database Editor";
            this.tabControl1.ResumeLayout(false);
            this.tpItemTemplate.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpItemTemplate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid pgItemTemplate;
        private System.Windows.Forms.Button btnItemTemplate;
        private System.Windows.Forms.Button btnItemTemplateNew;
        private System.Windows.Forms.Button btnItemTemplateDelete;
        private System.Windows.Forms.TextBox txtItemTemplate;
    }
}

