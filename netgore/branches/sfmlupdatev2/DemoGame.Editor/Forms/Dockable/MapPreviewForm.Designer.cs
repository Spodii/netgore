namespace DemoGame.Editor
{
    partial class MapPreviewForm
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
            this.mapScreen = new DemoGame.Editor.MapPreviewScreenControl();
            this.SuspendLayout();
            // 
            // mapScreen
            // 
            this.mapScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapScreen.Location = new System.Drawing.Point(0, 0);
            this.mapScreen.Name = "mapScreen";
            this.mapScreen.Size = new System.Drawing.Size(360, 246);
            this.mapScreen.TabIndex = 0;
            this.mapScreen.Text = "mapPreviewScreenControl1";
            // 
            // MapPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 246);
            this.Controls.Add(this.mapScreen);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MapPreviewForm";
            this.Text = "MapPreviewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private MapPreviewScreenControl mapScreen;
    }
}