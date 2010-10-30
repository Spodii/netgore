namespace NetGore.Features.NPCChat
{
    public partial class NPCChatConditionalEditorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.chkNot = new System.Windows.Forms.CheckBox();
            this.cmbConditionalType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lstParameters = new NetGore.Features.NPCChat.NPCChatParameterListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parameters:";
            // 
            // chkNot
            // 
            this.chkNot.AutoSize = true;
            this.chkNot.Location = new System.Drawing.Point(12, 12);
            this.chkNot.Name = "chkNot";
            this.chkNot.Size = new System.Drawing.Size(49, 17);
            this.chkNot.TabIndex = 1;
            this.chkNot.Text = "NOT";
            this.chkNot.UseVisualStyleBackColor = true;
            this.chkNot.CheckedChanged += new System.EventHandler(this.chkNot_CheckedChanged);
            // 
            // cmbConditionalType
            // 
            this.cmbConditionalType.FormattingEnabled = true;
            this.cmbConditionalType.Location = new System.Drawing.Point(67, 8);
            this.cmbConditionalType.Name = "cmbConditionalType";
            this.cmbConditionalType.Size = new System.Drawing.Size(210, 21);
            this.cmbConditionalType.TabIndex = 2;
            this.cmbConditionalType.SelectedIndexChanged += new System.EventHandler(this.cmbConditionalType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Value:";
            // 
            // txtValue
            // 
            this.txtValue.Enabled = false;
            this.txtValue.Location = new System.Drawing.Point(67, 195);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(210, 20);
            this.txtValue.TabIndex = 5;
            // 
            // lstParameters
            // 
            this.lstParameters.ConditionalCollectionItem = null;
            this.lstParameters.FormattingEnabled = true;
            this.lstParameters.Location = new System.Drawing.Point(12, 55);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(265, 134);
            this.lstParameters.TabIndex = 3;
            this.lstParameters.ValueTextBox = this.txtValue;
            // 
            // NPCChatConditionalEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 225);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstParameters);
            this.Controls.Add(this.cmbConditionalType);
            this.Controls.Add(this.chkNot);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NPCChatConditionalEditorForm";
            this.Text = "NPC Chat Conditional Editor";
            this.Load += new System.EventHandler(this.NPCChatConditionalEditorForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NPCChatConditionalEditorForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkNot;
        private System.Windows.Forms.ComboBox cmbConditionalType;
        private NPCChatParameterListBox lstParameters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtValue;
    }
}