using NetGore.EditorTools;

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
            this.cmbEmitter = new NetGore.EditorTools.ParticleEmitterComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
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
            this.tabControl1.Size = new System.Drawing.Size(276, 520);
            this.tabControl1.TabIndex = 1;
            // 
            // tabEffect
            // 
            this.tabEffect.Controls.Add(this.pgEffect);
            this.tabEffect.Controls.Add(this.gbEmitter);
            this.tabEffect.Location = new System.Drawing.Point(4, 22);
            this.tabEffect.Name = "tabEffect";
            this.tabEffect.Padding = new System.Windows.Forms.Padding(3);
            this.tabEffect.Size = new System.Drawing.Size(268, 494);
            this.tabEffect.TabIndex = 0;
            this.tabEffect.Text = "Effect";
            this.tabEffect.UseVisualStyleBackColor = true;
            // 
            // pgEffect
            // 
            this.pgEffect.Location = new System.Drawing.Point(6, 64);
            this.pgEffect.Name = "pgEffect";
            this.pgEffect.Size = new System.Drawing.Size(256, 424);
            this.pgEffect.TabIndex = 1;
            // 
            // gbEmitter
            // 
            this.gbEmitter.Controls.Add(this.cmbEmitter);
            this.gbEmitter.Controls.Add(this.label1);
            this.gbEmitter.Location = new System.Drawing.Point(6, 6);
            this.gbEmitter.Name = "gbEmitter";
            this.gbEmitter.Size = new System.Drawing.Size(256, 52);
            this.gbEmitter.TabIndex = 0;
            this.gbEmitter.TabStop = false;
            this.gbEmitter.Text = "Emitter";
            // 
            // cmbEmitter
            // 
            this.cmbEmitter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEmitter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmitter.FormattingEnabled = true;
            this.cmbEmitter.Location = new System.Drawing.Point(54, 19);
            this.cmbEmitter.Name = "cmbEmitter";
            this.cmbEmitter.Size = new System.Drawing.Size(196, 21);
            this.cmbEmitter.TabIndex = 1;
            this.cmbEmitter.SelectedEmitterChanged += new NetGore.EditorTools.ParticleEmitterComboBoxHandler(this.cmbEmitter_SelectedEmitterChanged);
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
            this.tabSettings.Size = new System.Drawing.Size(268, 494);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(67, 538);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(76, 25);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(149, 538);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 25);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // GameScreen
            // 
            this.GameScreen.Location = new System.Drawing.Point(294, 12);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.ScreenForm = null;
            this.GameScreen.Size = new System.Drawing.Size(727, 551);
            this.GameScreen.TabIndex = 0;
            this.GameScreen.Text = "gameScreenControl1";
            this.GameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseDown);
            this.GameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameScreen_MouseMove);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 575);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.GameScreen);
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Particle Effect Editor";
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
        private ParticleEmitterComboBox cmbEmitter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid pgEffect;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
    }
}

