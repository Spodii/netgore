using NetGore.Editor;

namespace DemoGame.Editor
{
    partial class ParticleEditorForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbEmitterType = new NetGore.Editor.WinForms.ParticleEmitterComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbEmitter = new System.Windows.Forms.GroupBox();
            this.lstEmitters = new NetGore.Editor.ParticleEmitterListBox();
            this.pButtons = new System.Windows.Forms.Panel();
            this.btnEmitterDown = new System.Windows.Forms.Button();
            this.btnEmitterUp = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.btnDeleteEmitter = new System.Windows.Forms.Button();
            this.btnNewEmitter = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.btnDeleteEffect = new System.Windows.Forms.Button();
            this.gameScreen = new DemoGame.Editor.ParticleEffectScreenControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpEffect.SuspendLayout();
            this.tpEmitter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbEmitter.SuspendLayout();
            this.pButtons.SuspendLayout();
            this.tpSettings.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(1050, 601);
            this.splitContainer1.SplitterDistance = 286;
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
            this.tc.Size = new System.Drawing.Size(286, 601);
            this.tc.TabIndex = 0;
            // 
            // tpEffect
            // 
            this.tpEffect.Controls.Add(this.pgEffect);
            this.tpEffect.Location = new System.Drawing.Point(4, 22);
            this.tpEffect.Name = "tpEffect";
            this.tpEffect.Padding = new System.Windows.Forms.Padding(3);
            this.tpEffect.Size = new System.Drawing.Size(278, 575);
            this.tpEffect.TabIndex = 2;
            this.tpEffect.Text = "Effect";
            this.tpEffect.UseVisualStyleBackColor = true;
            // 
            // pgEffect
            // 
            this.pgEffect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgEffect.Location = new System.Drawing.Point(3, 3);
            this.pgEffect.Name = "pgEffect";
            this.pgEffect.Size = new System.Drawing.Size(272, 569);
            this.pgEffect.TabIndex = 0;
            // 
            // tpEmitter
            // 
            this.tpEmitter.Controls.Add(this.pgEmitter);
            this.tpEmitter.Controls.Add(this.panel1);
            this.tpEmitter.Controls.Add(this.gbEmitter);
            this.tpEmitter.Location = new System.Drawing.Point(4, 22);
            this.tpEmitter.Name = "tpEmitter";
            this.tpEmitter.Padding = new System.Windows.Forms.Padding(3);
            this.tpEmitter.Size = new System.Drawing.Size(278, 575);
            this.tpEmitter.TabIndex = 0;
            this.tpEmitter.Text = "Emitters";
            this.tpEmitter.UseVisualStyleBackColor = true;
            // 
            // pgEmitter
            // 
            this.pgEmitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgEmitter.Location = new System.Drawing.Point(3, 209);
            this.pgEmitter.Name = "pgEmitter";
            this.pgEmitter.Size = new System.Drawing.Size(272, 363);
            this.pgEmitter.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbEmitterType);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 184);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(272, 25);
            this.panel1.TabIndex = 6;
            // 
            // cmbEmitterType
            // 
            this.cmbEmitterType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbEmitterType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEmitterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmitterType.FormattingEnabled = true;
            this.cmbEmitterType.Location = new System.Drawing.Point(65, 0);
            this.cmbEmitterType.Name = "cmbEmitterType";
            this.cmbEmitterType.Size = new System.Drawing.Size(207, 21);
            this.cmbEmitterType.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Emitter type:";
            // 
            // gbEmitter
            // 
            this.gbEmitter.Controls.Add(this.lstEmitters);
            this.gbEmitter.Controls.Add(this.pButtons);
            this.gbEmitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEmitter.Location = new System.Drawing.Point(3, 3);
            this.gbEmitter.Name = "gbEmitter";
            this.gbEmitter.Size = new System.Drawing.Size(272, 181);
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
            this.lstEmitters.Size = new System.Drawing.Size(266, 131);
            this.lstEmitters.TabIndex = 1;
            this.lstEmitters.SelectedValueChanged += new System.EventHandler(this.lstEmitters_SelectedValueChanged);
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.btnEmitterDown);
            this.pButtons.Controls.Add(this.btnEmitterUp);
            this.pButtons.Controls.Add(this.btnClone);
            this.pButtons.Controls.Add(this.btnDeleteEmitter);
            this.pButtons.Controls.Add(this.btnNewEmitter);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pButtons.Location = new System.Drawing.Point(3, 147);
            this.pButtons.Name = "pButtons";
            this.pButtons.Padding = new System.Windows.Forms.Padding(3);
            this.pButtons.Size = new System.Drawing.Size(266, 31);
            this.pButtons.TabIndex = 0;
            // 
            // btnEmitterDown
            // 
            this.btnEmitterDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEmitterDown.Location = new System.Drawing.Point(203, 3);
            this.btnEmitterDown.Name = "btnEmitterDown";
            this.btnEmitterDown.Size = new System.Drawing.Size(30, 25);
            this.btnEmitterDown.TabIndex = 9;
            this.btnEmitterDown.Text = "\\/";
            this.btnEmitterDown.UseVisualStyleBackColor = true;
            this.btnEmitterDown.Click += new System.EventHandler(this.btnEmitterDown_Click);
            // 
            // btnEmitterUp
            // 
            this.btnEmitterUp.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEmitterUp.Location = new System.Drawing.Point(233, 3);
            this.btnEmitterUp.Name = "btnEmitterUp";
            this.btnEmitterUp.Size = new System.Drawing.Size(30, 25);
            this.btnEmitterUp.TabIndex = 8;
            this.btnEmitterUp.Text = "/\\";
            this.btnEmitterUp.UseVisualStyleBackColor = true;
            this.btnEmitterUp.Click += new System.EventHandler(this.btnEmitterUp_Click);
            // 
            // btnClone
            // 
            this.btnClone.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnClone.Location = new System.Drawing.Point(113, 3);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(55, 25);
            this.btnClone.TabIndex = 7;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnDeleteEmitter
            // 
            this.btnDeleteEmitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDeleteEmitter.Location = new System.Drawing.Point(58, 3);
            this.btnDeleteEmitter.Name = "btnDeleteEmitter";
            this.btnDeleteEmitter.Size = new System.Drawing.Size(55, 25);
            this.btnDeleteEmitter.TabIndex = 6;
            this.btnDeleteEmitter.Text = "Delete";
            this.btnDeleteEmitter.UseVisualStyleBackColor = true;
            this.btnDeleteEmitter.Click += new System.EventHandler(this.btnDeleteEmitter_Click);
            // 
            // btnNewEmitter
            // 
            this.btnNewEmitter.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNewEmitter.Location = new System.Drawing.Point(3, 3);
            this.btnNewEmitter.Name = "btnNewEmitter";
            this.btnNewEmitter.Size = new System.Drawing.Size(55, 25);
            this.btnNewEmitter.TabIndex = 5;
            this.btnNewEmitter.Text = "New";
            this.btnNewEmitter.UseVisualStyleBackColor = true;
            this.btnNewEmitter.Click += new System.EventHandler(this.btnNewEmitter_Click);
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.btnDeleteEffect);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(278, 575);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // btnDeleteEffect
            // 
            this.btnDeleteEffect.Location = new System.Drawing.Point(6, 6);
            this.btnDeleteEffect.Name = "btnDeleteEffect";
            this.btnDeleteEffect.Size = new System.Drawing.Size(105, 23);
            this.btnDeleteEffect.TabIndex = 0;
            this.btnDeleteEffect.Text = "Delete Effect";
            this.btnDeleteEffect.UseVisualStyleBackColor = true;
            this.btnDeleteEffect.Click += new System.EventHandler(this.btnDeleteEffect_Click);
            // 
            // gameScreen
            // 
            this.gameScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameScreen.Location = new System.Drawing.Point(0, 0);
            this.gameScreen.Name = "gameScreen";
            this.gameScreen.ParticleEffect = null;
            this.gameScreen.Size = new System.Drawing.Size(760, 601);
            this.gameScreen.TabIndex = 0;
            this.gameScreen.Text = "gameScreenControl1";
            this.gameScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameScreen_MouseDown);
            this.gameScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gameScreen_MouseMove);
            // 
            // ParticleEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 609);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ParticleEditorForm";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "Particle Effect Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpEffect.ResumeLayout(false);
            this.tpEmitter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbEmitter.ResumeLayout(false);
            this.pButtons.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpEmitter;
        private System.Windows.Forms.TabPage tpSettings;
        private ParticleEffectScreenControl gameScreen;
        private System.Windows.Forms.GroupBox gbEmitter;
        private ParticleEmitterListBox lstEmitters;
        private System.Windows.Forms.Panel pButtons;
        private System.Windows.Forms.TabPage tpEffect;
        private System.Windows.Forms.PropertyGrid pgEffect;
        private System.Windows.Forms.PropertyGrid pgEmitter;
        private System.Windows.Forms.Panel panel1;
        private NetGore.Editor.WinForms.ParticleEmitterComboBox cmbEmitterType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.Button btnDeleteEmitter;
        private System.Windows.Forms.Button btnNewEmitter;
        private System.Windows.Forms.Button btnEmitterDown;
        private System.Windows.Forms.Button btnEmitterUp;
        private System.Windows.Forms.Button btnDeleteEffect;

    }
}