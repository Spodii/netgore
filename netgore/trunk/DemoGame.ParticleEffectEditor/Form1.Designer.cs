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
            this.tc = new System.Windows.Forms.TabControl();
            this.tpEffect = new System.Windows.Forms.TabPage();
            this.pgEffect = new System.Windows.Forms.PropertyGrid();
            this.tpEmitter = new System.Windows.Forms.TabPage();
            this.pgEmitter = new System.Windows.Forms.PropertyGrid();
            this.gbEmitter = new System.Windows.Forms.GroupBox();
            this.lstEmitters = new DemoGame.ParticleEffectEditor.ParticleEmitterListBox();
            this.pButtons = new System.Windows.Forms.Panel();
            this.btnDeleteEmitter = new System.Windows.Forms.Button();
            this.btnNewEmitter = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.gameScreen = new DemoGame.ParticleEffectEditor.GameScreenControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpEffect.SuspendLayout();
            this.tpEmitter.SuspendLayout();
            this.gbEmitter.SuspendLayout();
            this.pButtons.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tc);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gameScreen);
            this.splitContainer1.Size = new System.Drawing.Size(815, 516);
            this.splitContainer1.SplitterDistance = 262;
            this.splitContainer1.TabIndex = 0;
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpEffect);
            this.tc.Controls.Add(this.tpEmitter);
            this.tc.Controls.Add(this.tpSettings);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(262, 516);
            this.tc.TabIndex = 0;
            // 
            // tpEffect
            // 
            this.tpEffect.Controls.Add(this.pgEffect);
            this.tpEffect.Location = new System.Drawing.Point(4, 22);
            this.tpEffect.Name = "tpEffect";
            this.tpEffect.Padding = new System.Windows.Forms.Padding(3);
            this.tpEffect.Size = new System.Drawing.Size(254, 490);
            this.tpEffect.TabIndex = 2;
            this.tpEffect.Text = "Effect";
            this.tpEffect.UseVisualStyleBackColor = true;
            // 
            // pgEffect
            // 
            this.pgEffect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgEffect.Location = new System.Drawing.Point(3, 3);
            this.pgEffect.Name = "pgEffect";
            this.pgEffect.Size = new System.Drawing.Size(248, 484);
            this.pgEffect.TabIndex = 0;
            // 
            // tpEmitter
            // 
            this.tpEmitter.Controls.Add(this.pgEmitter);
            this.tpEmitter.Controls.Add(this.gbEmitter);
            this.tpEmitter.Location = new System.Drawing.Point(4, 22);
            this.tpEmitter.Name = "tpEmitter";
            this.tpEmitter.Padding = new System.Windows.Forms.Padding(3);
            this.tpEmitter.Size = new System.Drawing.Size(254, 490);
            this.tpEmitter.TabIndex = 0;
            this.tpEmitter.Text = "Emitters";
            this.tpEmitter.UseVisualStyleBackColor = true;
            // 
            // pgEmitter
            // 
            this.pgEmitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgEmitter.Location = new System.Drawing.Point(3, 184);
            this.pgEmitter.Name = "pgEmitter";
            this.pgEmitter.Size = new System.Drawing.Size(248, 303);
            this.pgEmitter.TabIndex = 3;
            // 
            // gbEmitter
            // 
            this.gbEmitter.Controls.Add(this.lstEmitters);
            this.gbEmitter.Controls.Add(this.pButtons);
            this.gbEmitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEmitter.Location = new System.Drawing.Point(3, 3);
            this.gbEmitter.Name = "gbEmitter";
            this.gbEmitter.Size = new System.Drawing.Size(248, 181);
            this.gbEmitter.TabIndex = 2;
            this.gbEmitter.TabStop = false;
            this.gbEmitter.Text = "Emitters";
            // 
            // lstEmitters
            // 
            this.lstEmitters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstEmitters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstEmitters.FormattingEnabled = true;
            this.lstEmitters.Location = new System.Drawing.Point(3, 16);
            this.lstEmitters.Name = "lstEmitters";
            this.lstEmitters.Size = new System.Drawing.Size(242, 131);
            this.lstEmitters.TabIndex = 1;
            this.lstEmitters.TypedSelectedItemChanged += new NetGore.Editor.WinForms.TypedListBoxChangeEventHandler<NetGore.Graphics.ParticleEngine.IParticleEmitter>(this.lstEmitters_TypedSelectedItemChanged);
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.btnDeleteEmitter);
            this.pButtons.Controls.Add(this.btnNewEmitter);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pButtons.Location = new System.Drawing.Point(3, 147);
            this.pButtons.Name = "pButtons";
            this.pButtons.Padding = new System.Windows.Forms.Padding(3);
            this.pButtons.Size = new System.Drawing.Size(242, 31);
            this.pButtons.TabIndex = 0;
            // 
            // btnDeleteEmitter
            // 
            this.btnDeleteEmitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDeleteEmitter.Location = new System.Drawing.Point(78, 3);
            this.btnDeleteEmitter.Name = "btnDeleteEmitter";
            this.btnDeleteEmitter.Size = new System.Drawing.Size(75, 25);
            this.btnDeleteEmitter.TabIndex = 1;
            this.btnDeleteEmitter.Text = "Delete";
            this.btnDeleteEmitter.UseVisualStyleBackColor = true;
            // 
            // btnNewEmitter
            // 
            this.btnNewEmitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNewEmitter.Location = new System.Drawing.Point(3, 3);
            this.btnNewEmitter.Name = "btnNewEmitter";
            this.btnNewEmitter.Size = new System.Drawing.Size(75, 25);
            this.btnNewEmitter.TabIndex = 0;
            this.btnNewEmitter.Text = "New";
            this.btnNewEmitter.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(254, 490);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // gameScreen
            // 
            this.gameScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameScreen.Location = new System.Drawing.Point(0, 0);
            this.gameScreen.Name = "gameScreen";
            this.gameScreen.ScreenForm = null;
            this.gameScreen.Size = new System.Drawing.Size(549, 516);
            this.gameScreen.TabIndex = 0;
            this.gameScreen.Text = "gameScreenControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 524);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpEffect.ResumeLayout(false);
            this.tpEmitter.ResumeLayout(false);
            this.gbEmitter.ResumeLayout(false);
            this.pButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpEmitter;
        private System.Windows.Forms.TabPage tpSettings;
        private GameScreenControl gameScreen;
        private System.Windows.Forms.PropertyGrid pgEmitter;
        private System.Windows.Forms.GroupBox gbEmitter;
        private ParticleEmitterListBox lstEmitters;
        private System.Windows.Forms.Panel pButtons;
        private System.Windows.Forms.Button btnDeleteEmitter;
        private System.Windows.Forms.Button btnNewEmitter;
        private System.Windows.Forms.TabPage tpEffect;
        private System.Windows.Forms.PropertyGrid pgEffect;

    }
}