namespace DemoGame.ParticleEffectEditor
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gameScreen = new DemoGame.ParticleEffectEditor.GameScreenControl();
            this.pgEffect = new System.Windows.Forms.PropertyGrid();
            this.gbEmitter = new System.Windows.Forms.GroupBox();
            this.cmbEmitter = new NetGore.Editor.WinForms.ParticleEmitterComboBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbEmitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gameScreen);
            this.splitContainer1.Size = new System.Drawing.Size(981, 631);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(327, 631);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pgEffect);
            this.tabPage1.Controls.Add(this.gbEmitter);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(319, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gameScreen
            // 
            this.gameScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameScreen.Location = new System.Drawing.Point(0, 0);
            this.gameScreen.Name = "gameScreen";
            this.gameScreen.ScreenForm = null;
            this.gameScreen.Size = new System.Drawing.Size(650, 631);
            this.gameScreen.TabIndex = 0;
            this.gameScreen.Text = "gameScreenControl1";
            // 
            // pgEffect
            // 
            this.pgEffect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgEffect.Location = new System.Drawing.Point(3, 47);
            this.pgEffect.Name = "pgEffect";
            this.pgEffect.Size = new System.Drawing.Size(313, 555);
            this.pgEffect.TabIndex = 3;
            // 
            // gbEmitter
            // 
            this.gbEmitter.Controls.Add(this.cmbEmitter);
            this.gbEmitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEmitter.Location = new System.Drawing.Point(3, 3);
            this.gbEmitter.Name = "gbEmitter";
            this.gbEmitter.Size = new System.Drawing.Size(313, 44);
            this.gbEmitter.TabIndex = 2;
            this.gbEmitter.TabStop = false;
            this.gbEmitter.Text = "Emitter";
            // 
            // cmbEmitter
            // 
            this.cmbEmitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEmitter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEmitter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmitter.FormattingEnabled = true;
            this.cmbEmitter.Location = new System.Drawing.Point(3, 16);
            this.cmbEmitter.Name = "cmbEmitter";
            this.cmbEmitter.Size = new System.Drawing.Size(307, 21);
            this.cmbEmitter.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 639);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbEmitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private GameScreenControl gameScreen;
        private System.Windows.Forms.PropertyGrid pgEffect;
        private System.Windows.Forms.GroupBox gbEmitter;
        private NetGore.Editor.WinForms.ParticleEmitterComboBox cmbEmitter;

    }
}