namespace DemoGame.Editor
{
    partial class EditMapForm
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
            this.mapScreen = new DemoGame.Editor.MapScreenControl();
            this.SuspendLayout();
            // 
            // mapScreen
            // 
            this.mapScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapScreen.Location = new System.Drawing.Point(0, 0);
            this.mapScreen.Map = null;
            this.mapScreen.Name = "mapScreen";
            this.mapScreen.Size = new System.Drawing.Size(784, 562);
            this.mapScreen.TabIndex = 1;
            this.mapScreen.Text = "Map Screen";
            this.mapScreen.Enter += new System.EventHandler(this.mapScreen_Enter);
            this.mapScreen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mapScreen_KeyDown);
            this.mapScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapScreen_MouseDown);
            this.mapScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapScreen_MouseUp);
            // 
            // EditMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.mapScreen);
            this.HideOnClose = false;
            this.Name = "EditMapForm";
            this.Text = "Map";
            this.ToolBarVisibility = NetGore.Editor.EditorTool.ToolBarVisibility.Map;
            this.ResumeLayout(false);

        }

        #endregion

        private MapScreenControl mapScreen;
    }
}