using System;
using System.Linq;
using System.Windows.Forms;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public partial class ShiftNodesInputForm : Form
    {
        Vector2 _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftNodesInputForm"/> class.
        /// </summary>
        public ShiftNodesInputForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the value entered into the form.
        /// </summary>
        public Vector2 Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Handles the Click event of the btnApply control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnApply_Click(object sender, EventArgs e)
        {
            float x;
            if (!float.TryParse(txtX.Text, out x))
            {
                MessageBox.Show("Invalid value entered for X.", "Invalid value");
                return;
            }

            float y;
            if (!float.TryParse(txtY.Text, out y))
            {
                MessageBox.Show("Invalid value entered for Y.", "Invalid value");
                return;
            }

            _value = new Vector2(x, y);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}