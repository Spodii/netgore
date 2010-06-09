namespace DemoGame.MapEditor
{
    partial class GameScreen
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
            this.gameScreenCtrl = new DemoGame.MapEditor.GameScreenControl();
            this.SuspendLayout();
            // 
            // gameScreenCtrl
            // 
            this.gameScreenCtrl.Camera = null;
            this.gameScreenCtrl.CursorPos = new SFML.Graphics.Vector2(0F, 0F);
            this.gameScreenCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameScreenCtrl.DrawHandler = null;
            this.gameScreenCtrl.Location = new System.Drawing.Point(0, 0);
            this.gameScreenCtrl.Name = "gameScreenCtrl";
            this.gameScreenCtrl.Size = new System.Drawing.Size(603, 491);
            this.gameScreenCtrl.TabIndex = 0;
            this.gameScreenCtrl.Text = "GAME SCREEN";
            this.gameScreenCtrl.UpdateHandler = null;
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 491);
            this.Controls.Add(this.gameScreenCtrl);
            this.Name = "GameScreen";
            this.Text = "GameScreen";
            this.Load += new System.EventHandler(this.GameScreen_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GameScreenControl gameScreenCtrl;
    }
}