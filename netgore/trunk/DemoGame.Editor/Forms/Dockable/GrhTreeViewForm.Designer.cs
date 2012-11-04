using NetGore.Editor.Grhs;

namespace DemoGame.Editor
{
    partial class GrhTreeViewForm
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.filterTxt = new System.Windows.Forms.TextBox();
            this.gtv = new NetGore.Editor.Grhs.GrhTreeView();
            this.SuspendLayout();
            // 
            // filterTxt
            // 
            this.filterTxt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filterTxt.Location = new System.Drawing.Point(0, 307);
            this.filterTxt.Name = "filterTxt";
            this.filterTxt.Size = new System.Drawing.Size(267, 20);
            this.filterTxt.TabIndex = 7;
            this.toolTip1.SetToolTip(this.filterTxt, "Filter the GrhDatas by their category and title. Separate multiple filters with c" +
        "ommas (e.g. cat,dog).");
            this.filterTxt.TextChanged += new System.EventHandler(this.filterTxt_TextChanged);
            // 
            // gtv
            // 
            this.gtv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gtv.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.gtv.Filter = null;
            this.gtv.ImageIndex = 0;
            this.gtv.Location = new System.Drawing.Point(0, 0);
            this.gtv.Name = "gtv";
            this.gtv.SelectedImageIndex = 0;
            this.gtv.Size = new System.Drawing.Size(267, 307);
            this.gtv.TabIndex = 8;
            this.gtv.GrhAfterSelect += new System.EventHandler<NetGore.Editor.Grhs.GrhTreeViewEventArgs>(this.gtv_GrhAfterSelect);
            this.gtv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gtv_KeyDown);
            // 
            // GrhTreeViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 327);
            this.Controls.Add(this.gtv);
            this.Controls.Add(this.filterTxt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "GrhTreeViewForm";
            this.Text = "GrhDatas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox filterTxt;
        private GrhTreeView gtv;
    }
}