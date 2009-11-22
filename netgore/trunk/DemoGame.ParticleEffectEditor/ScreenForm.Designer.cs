namespace DemoGame.ParticleEffectEditor
{
    partial class ScreenForm
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
            this.tabEffect = new System.Windows.Forms.TabPage();
            this.pgEffect = new System.Windows.Forms.PropertyGrid();
            this.gbEmitter = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.GameScreen = new DemoGame.ParticleEffectEditor.GameScreenControl();
            this.tabControl1.SuspendLayout();
            this.tabEffect.SuspendLayout();
            this.gbEmitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabEffect);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(276, 551);
            this.tabControl1.TabIndex = 1;
            // 
            // tabEffect
            // 
            this.tabEffect.Controls.Add(this.pgEffect);
            this.tabEffect.Controls.Add(this.gbEmitter);
            this.tabEffect.Location = new System.Drawing.Point(4, 22);
            this.tabEffect.Name = "tabEffect";
            this.tabEffect.Padding = new System.Windows.Forms.Padding(3);
            this.tabEffect.Size = new System.Drawing.Size(268, 525);
            this.tabEffect.TabIndex = 0;
            this.tabEffect.Text = "Effect";
            this.tabEffect.UseVisualStyleBackColor = true;
            // 
            // pgEffect
            // 
            this.pgEffect.Location = new System.Drawing.Point(6, 87);
            this.pgEffect.Name = "pgEffect";
            this.pgEffect.Size = new System.Drawing.Size(256, 432);
            this.pgEffect.TabIndex = 1;
            // 
            // gbEmitter
            // 
            this.gbEmitter.Controls.Add(this.textBox2);
            this.gbEmitter.Controls.Add(this.label3);
            this.gbEmitter.Controls.Add(this.textBox1);
            this.gbEmitter.Controls.Add(this.label2);
            this.gbEmitter.Controls.Add(this.comboBox1);
            this.gbEmitter.Controls.Add(this.label1);
            this.gbEmitter.Location = new System.Drawing.Point(6, 6);
            this.gbEmitter.Name = "gbEmitter";
            this.gbEmitter.Size = new System.Drawing.Size(256, 75);
            this.gbEmitter.TabIndex = 0;
            this.gbEmitter.TabStop = false;
            this.gbEmitter.Text = "Emitter";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(162, 46);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(88, 20);
            this.textBox2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(129, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Life:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(67, 20);
            this.textBox1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Budget:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(54, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(196, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Emitter:";
            // 
            // tabSettings
            // 
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(268, 525);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // GameScreen
            // 
            this.GameScreen.Location = new System.Drawing.Point(294, 12);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Screen = null;
            this.GameScreen.Size = new System.Drawing.Size(727, 551);
            this.GameScreen.TabIndex = 0;
            this.GameScreen.Text = "gameScreenControl1";
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 575);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.GameScreen);
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Particle Effect Editor";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabEffect.ResumeLayout(false);
            this.gbEmitter.ResumeLayout(false);
            this.gbEmitter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GameScreenControl GameScreen;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabEffect;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.GroupBox gbEmitter;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid pgEffect;
    }
}

