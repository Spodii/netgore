using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Displays a <see cref="Form"/> that is used to accept an input string from the user.
    /// </summary>
    public partial class InputBox : Form
    {
        /// <summary>
        /// The minimum height allowed for <see cref="txtMessage"/>.
        /// </summary>
        const int _mintxtMessageHeight = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the message to display.
        /// </summary>
        public string Message
        {
            get { return txtMessage.Text; }
            set
            {
                if (value == null)
                    value = string.Empty;

                txtMessage.Text = value;

                var reqHeight = (int)Math.Ceiling(txtMessage.Font.GetHeight() * (txtMessage.Lines.Length + 1));
                var newTxtBoxHeight = Math.Max(_mintxtMessageHeight, reqHeight);

                ClientSize = new Size(ClientSize.Width, Math.Max(200, (ClientSize.Height - txtMessage.Height) + newTxtBoxHeight));
            }
        }

        /// <summary>
        /// Gets or sets the text that has been entered into the <see cref="InputBox"/>.
        /// </summary>
        public string Value
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Shown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            txtValue.Focus();
        }

        /// <summary>
        /// Shows an <see cref="InputBox"/>.
        /// </summary>
        /// <param name="title">The <see cref="InputBox"/> title.</param>
        /// <param name="message">The <see cref="InputBox"/> message.</param>
        /// <param name="initialValue">The initial value to display.</param>
        /// <returns>
        /// When the <see cref="DialogResult"/> equals <see cref="DialogResult.OK"/> or <see cref="DialogResult.Yes"/>, then
        /// contains the text the user entered. Otherwise, a null string is returned.
        /// </returns>
        public static string Show(string title, string message, string initialValue = "")
        {
            using (var ib = new InputBox { Text = title, Message = message, Value = initialValue })
            {
                var result = ib.ShowDialog();

                if (result != DialogResult.OK && result != DialogResult.Yes)
                    return null;

                return ib.Value;
            }
        }

        /// <summary>
        /// Handles the Click event of the <see cref="btnCancel"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handles the Click event of the <see cref="btnOK"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}