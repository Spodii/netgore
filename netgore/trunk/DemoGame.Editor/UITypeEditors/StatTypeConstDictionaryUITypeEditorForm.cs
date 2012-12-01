using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using NetGore;

namespace DemoGame.Editor.UITypeEditors
{
    public class StatTypeConstDictionaryUITypeEditorForm : Form
    {
        readonly StatTypeConstDictionary _src;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        readonly IContainer components = null;

        DataGridViewTextBoxColumn cKey;
        DataGridViewTextBoxColumn cValue;
        DataGridView dgv;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatTypeConstDictionaryUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="src">The <see cref="StatTypeConstDictionary"/> to edit.</param>
        public StatTypeConstDictionaryUITypeEditorForm(StatTypeConstDictionary src)
        {
            _src = src;

            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Ensure there are no existing rows
            dgv.Rows.Clear();

            // Add the rows for each of the values
            foreach (var s in _src)
            {
                var i = dgv.Rows.Add();
                var row = dgv.Rows[i];

                row.Cells[0].ValueType = typeof(string);
                row.Cells[0].Value = s.Key.ToString();

                row.Cells[1].ValueType = s.Value.GetType();
                row.Cells[1].Value = s.Value;
            }

            // Get the size to shrink down to
            var width = dgv.Columns.Cast<DataGridViewColumn>().Sum(x => x.Width);
            var height = dgv.Rows.Cast<DataGridViewRow>().Sum(x => x.Height);

            // Add the column header height
            height += dgv.ColumnHeadersHeight;

            // Add a little boost to the size
            width += 32;
            height += 4;

            // Set the new size
            dgv.Size = new Size(width, height);

            // Resize the form to the size of the control
            ClientSize = dgv.Size;

            // Set the value column to fill up, and the DataGridView to take up the whole form
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Dock = DockStyle.Fill;

            dgv.CellValueChanged -= dgv_CellValueChanged;
            dgv.CellValueChanged += dgv_CellValueChanged;
        }

        /// <summary>
        /// Handles the CellValueChanged event of the dgv control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing
        /// the event data.</param>
        void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var key = EnumHelper<StatType>.Parse(dgv[0, e.RowIndex].Value.ToString());

            try
            {
                _src[key] = (int)dgv[1, e.RowIndex].Value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                dgv[1, e.RowIndex].ErrorText = ex.Message;
                return;
            }

            dgv[1, e.RowIndex].Value = _src[key];
            dgv[1, e.RowIndex].ErrorText = string.Empty;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.dgv = new System.Windows.Forms.DataGridView();
            this.cKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.cKey, this.cValue });
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(221, 190);
            this.dgv.TabIndex = 0;
            // 
            // cKey
            // 
            this.cKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cKey.HeaderText = "Key";
            this.cKey.Name = "cKey";
            this.cKey.ReadOnly = true;
            this.cKey.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cKey.Width = 50;
            // 
            // cValue
            // 
            this.cValue.HeaderText = "Value";
            this.cValue.MaxInputLength = 65535;
            this.cValue.Name = "cValue";
            this.cValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cValue.Width = 59;
            // 
            // StatTypeConstDictionaryUITypeEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 328);
            this.Controls.Add(this.dgv);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatTypeConstDictionaryUITypeEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StatTypeConstDictionaryUITypeEditorForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}