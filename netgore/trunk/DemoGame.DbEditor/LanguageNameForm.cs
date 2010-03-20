using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoGame.DbEditor
{
    public partial class LanguageNameForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageNameForm"/> class.
        /// </summary>
        public LanguageNameForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the entered language name value.
        /// </summary>
        public string Value { get { return txtValue.Text; } }

        /// <summary>
        /// Handles the Click event of the btnCreate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValue.Text))
            {
                MessageBox.Show("You must enter a name for the new language first!");
                return;
            }

            if (GameMessageCollection.GetLanguages().Contains(txtValue.Text, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("A language with that name already exists! Please enter a different name.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
