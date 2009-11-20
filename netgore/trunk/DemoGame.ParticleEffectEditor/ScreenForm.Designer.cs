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
            this.GameScreen = new DemoGame.ParticleEffectEditor.GameScreenControl();
            this.SuspendLayout();
            // 
            // GameScreen
            // 
            this.GameScreen.Location = new System.Drawing.Point(294, 12);
            this.GameScreen.Name = "GameScreen";
            this.GameScreen.Screen = null;
            this.GameScreen.Size = new System.Drawing.Size(727, 551);
            this.GameScreen.TabIndex = 0;
            this.GameScreen.Text = "gameScreenControl1";
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 575);
            this.Controls.Add(this.GameScreen);
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Particle Effect Editor";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GameScreenControl GameScreen;
    }
}

